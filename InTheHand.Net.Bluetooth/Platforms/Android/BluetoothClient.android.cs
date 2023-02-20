// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Android)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private BluetoothSocket _socket;
        private BluetoothRadio _radio;
        internal static Activity currentContext;

        private void PlatformInitialize()
        {
            // when used by a cross-platform UI framework like MAUI or Uno we need to get the current Activity in order to launch the picker UI
            // for a "native" app you can use the Android specific RequestDevice overload which accepts a Context

#if NET6_0_OR_GREATER

            // check for Uno without taking a hard dependency
            var t = Type.GetType("Uno.UI.ContextHelper, Uno, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null", false, true);
            if (t != null)
            {
                currentContext = (Activity)t.GetProperty("Current", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
            }
            else
            {
                // try Maui Essentials if not
                t = Type.GetType("Microsoft.Maui.ApplicationModel.Platform, Microsoft.Maui.Essentials, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false, true);
                if (t != null)
                {
                    currentContext = (Activity)t.GetProperty("CurrentActivity", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
                }
            }
#else
            // check for Xamarin.Essentials without taking a hard dependency
            var t = Type.GetType("Xamarin.Essentials.Platform, Xamarin.Essentials, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false, true);
            if (t != null)
            {
                currentContext = (Activity)t.GetProperty("CurrentActivity", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
            }
#endif
            if (currentContext == null)
                System.Diagnostics.Debug.WriteLine("Bluetooth.android Context:Unknown");
            else
                System.Diagnostics.Debug.WriteLine($"Bluetooth.android Context:{currentContext.GetType().FullName}");

            _radio = BluetoothRadio.Default;
            if (_radio != null && _radio.Mode == RadioMode.PowerOff)
                _radio.Mode = RadioMode.Connectable;
        }

        internal BluetoothClient(BluetoothSocket socket)
        {
            PlatformInitialize();
            _socket = socket;
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            foreach(BluetoothDevice device in ((BluetoothAdapter)_radio).BondedDevices)
            {
                yield return device;
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> PlatformDiscoverDevices(int maxDevices)
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
            currentContext.RegisterReceiver(receiver, filter, null, handler);

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
                currentContext.UnregisterReceiver(receiver);
                handle.Set();
                handlerThread.QuitSafely();
            };

            handle.WaitOne();

            return devices.AsReadOnly();
        }

        async void PlatformConnect(BluetoothAddress address, Guid service)
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
                        AndroidNetworkStream.GetAvailable(_socket.InputStream as Android.Runtime.InputStreamInvoker);
                    }
                    catch (Exception ex2)
                    {
                        System.Diagnostics.Debug.WriteLine(ex2.Message);

                        _socket = null;
                    }
                }
            }
        }

        void PlatformClose()
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

        NetworkStream PlatformGetStream()
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
