// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Android)
// 
// Copyright (c) 2018-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Xamarin.Essentials;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private BluetoothSocket _socket;
        private BluetoothRadio _radio;

        private void PlatformInitialize()
        {
            _radio = BluetoothRadio.Default;
            if (_radio != null && _radio.Mode == RadioMode.PowerOff)
                _radio.Mode = RadioMode.Connectable;
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            foreach(BluetoothDevice device in ((BluetoothAdapter)_radio).BondedDevices)
            {
                yield return device;
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> DoDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            HandlerThread handlerThread = new HandlerThread("ht");
            handlerThread.Start();
            Looper looper = handlerThread.Looper;
            Handler handler = new Handler(looper);

            BluetoothDiscoveryReceiver receiver = new BluetoothDiscoveryReceiver();
            IntentFilter filter = new IntentFilter();
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            Platform.CurrentActivity.RegisterReceiver(receiver, filter, null, handler);

            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            receiver.DeviceFound += (s, e) =>
            {
                if (!devices.Contains(e))
                {
                    devices.Add(e);

                    if(devices.Count == maxDevices)
                    {
                        ((BluetoothAdapter)_radio).CancelDiscovery();
                    }
                }
            };

            ((BluetoothAdapter)_radio).StartDiscovery();

            receiver.DiscoveryComplete += (s, e) =>
            {
                Platform.CurrentActivity.UnregisterReceiver(receiver);
                handle.Set();
                handlerThread.QuitSafely();
            };

            handle.WaitOne();

            return devices.AsReadOnly();
        }

        async void DoConnect(BluetoothAddress address, Guid service)
        {
            var nativeDevice = ((BluetoothAdapter)_radio).GetRemoteDevice(address.ToString("C"));

            if (!Authenticate && !Encrypt)
            {
                _socket = nativeDevice.CreateInsecureRfcommSocketToServiceRecord(Java.Util.UUID.FromString(service.ToString()));
            }
            else
            {
                _socket = nativeDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(service.ToString()));
            }

            if (_socket is object)
            {
                try
                {
                    _socket.Connect();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    try
                    {
                        _socket.Connect();
                    }
                    catch (Exception ex2)
                    {
                        System.Diagnostics.Debug.WriteLine(ex2.Message);

                        _socket = null;
                    }
                }
            }
        }

        void DoClose()
        {
            if (_socket is object)
            {
                if (_socket.IsConnected)
                {
                    _socket.Close();
                }

                _socket.Dispose();
                _socket = null;
            }
        }

        private bool _authenticate;

        bool GetAuthenticate()
        {
            return _authenticate;
        }

        void SetAuthenticate(bool value)
        {
            _authenticate = value;
        }

        Socket GetClient()
        {
            return null;
        }

        bool GetConnected()
        {
            return _socket is object && _socket.IsConnected;
        }

        private bool _encrypt = false;

        bool GetEncrypt()
        {

            return _encrypt;
        }

        void SetEncrypt(bool value)
        {
            _encrypt = value;
        }

        TimeSpan GetInquiryLength()
        {
            return TimeSpan.Zero;
        }

        void SetInquiryLength(TimeSpan length)
        {

        }

        public string GetRemoteMachineName()
        {
            if(_socket is object && _socket.IsConnected)
                return _socket.RemoteDevice.Name;

            return null;
        }

        NetworkStream DoGetStream()
        {
            if(Connected)
                return new AndroidNetworkStream(_socket.InputStream, _socket.OutputStream);

            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        
        #endregion
    }
}
