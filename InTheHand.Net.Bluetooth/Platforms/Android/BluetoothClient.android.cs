// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Android)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private BluetoothSocket _socket;

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            foreach(BluetoothDevice device in BluetoothAdapter.DefaultAdapter.BondedDevices)
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
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.RegisterReceiver(receiver, filter, null, handler);

            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            BluetoothAdapter.DefaultAdapter.StartDiscovery();

            receiver.DeviceFound += (s, e) =>
            {
                if (!devices.Contains(e))
                {
                    devices.Add(e);

                    if(devices.Count == maxDevices)
                    {
                        BluetoothAdapter.DefaultAdapter.CancelDiscovery();
                    }
                }
            };

            receiver.DiscoveryComplete += (s, e) =>
            {
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.UnregisterReceiver(receiver);
                handle.Set();
                handlerThread.QuitSafely();
            };

            handle.WaitOne();

            return devices.AsReadOnly();
        }

        async void DoConnect(BluetoothAddress address, Guid service)
        {
            var nativeDevice = BluetoothAdapter.DefaultAdapter.GetRemoteDevice(address.ToString("C"));

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
