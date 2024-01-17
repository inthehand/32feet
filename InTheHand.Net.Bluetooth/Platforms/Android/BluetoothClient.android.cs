// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.AndroidBluetoothClient
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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static Android.Bluetooth.BluetoothClass;

namespace InTheHand.Net.Sockets
{
    internal sealed class AndroidBluetoothClient : IBluetoothClient
    {
        private BluetoothSocket _socket;
        private readonly BluetoothRadio _radio;

        public AndroidBluetoothClient()
        {
            _radio = BluetoothRadio.Default;
            if (_radio != null && _radio.Mode == RadioMode.PowerOff)
                _radio.Mode = RadioMode.Connectable;
        }

        internal AndroidBluetoothClient(BluetoothSocket socket) : this()
        {
            _socket = socket;
        }

        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                foreach (var device in ((BluetoothAdapter)_radio).BondedDevices)
                {
                    yield return new BluetoothDeviceInfo(new AndroidBluetoothDeviceInfo(device));
                }
            }
        }

        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            if (InTheHand.AndroidActivity.CurrentActivity == null)
                throw new NotSupportedException("CurrentActivity was not detected or specified");


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
            InTheHand.AndroidActivity.CurrentActivity.RegisterReceiver(receiver, filter, null, handler);

            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            receiver.DeviceFound += (s, e) =>
            {
                var bdi = new BluetoothDeviceInfo(new AndroidBluetoothDeviceInfo(e));
                if (!devices.Contains(bdi))
                {
                    devices.Add(bdi);

                    if(devices.Count == maxDevices)
                    {
                        ((BluetoothAdapter)_radio).CancelDiscovery();
                    }
                }
            };

            ((BluetoothAdapter)_radio).StartDiscovery();

            receiver.DiscoveryComplete += (s, e) =>
            {
                InTheHand.AndroidActivity.CurrentActivity.UnregisterReceiver(receiver);
                handle.Set();
                handlerThread.QuitSafely();
            };

            handle.WaitOne();

            return devices.AsReadOnly();
        }

#if NET6_0_OR_GREATER
        public async IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (InTheHand.AndroidActivity.CurrentActivity == null)
                throw new NotSupportedException("CurrentActivity was not detected or specified");
            
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();
            var waitable = new AutoResetEvent(false);

            HandlerThread handlerThread = new HandlerThread("ht");
            handlerThread.Start();
            Looper looper = handlerThread.Looper;
            Handler handler = new Handler(looper);

            BluetoothDiscoveryReceiver receiver = new BluetoothDiscoveryReceiver();
            IntentFilter filter = new IntentFilter();
            filter.AddAction(BluetoothDevice.ActionFound);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);
            filter.AddAction(BluetoothAdapter.ActionDiscoveryStarted);
            InTheHand.AndroidActivity.CurrentActivity.RegisterReceiver(receiver, filter, null, handler);

            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            receiver.DeviceFound += (s, e) =>
            {
                var bdi = new BluetoothDeviceInfo(new AndroidBluetoothDeviceInfo(e));
                if (cancellationToken.IsCancellationRequested)
                {
                    ((BluetoothAdapter)_radio).CancelDiscovery();
                }
                else
                {
                    if (!devices.Contains(bdi))
                    {
                        devices.Add(bdi);
                        waitable.Set();
                    }
                }
            };

            ((BluetoothAdapter)_radio).StartDiscovery();

            receiver.DiscoveryComplete += (s, e) =>
            {
                InTheHand.AndroidActivity.CurrentActivity.UnregisterReceiver(receiver);
                handle.Set();
                handlerThread.QuitSafely();
                waitable.Set();
            };

            handle.WaitOne();

            while(((BluetoothAdapter)_radio).IsDiscovering && !cancellationToken.IsCancellationRequested)
            {
                waitable.WaitOne();
                if(devices.Count > 0)
                {
                    yield return devices[devices.Count - 1];
                }
            }

            yield break;
        }
#endif

        public void Connect(BluetoothAddress address, Guid service)
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

        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            Connect(remoteEP.Address, remoteEP.Service);
        }

        public async Task ConnectAsync(BluetoothAddress address, Guid service)
        {
            Connect(address, service);
        }

        public void Close()
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

        public bool Authenticate { get => _authenticate; set => _authenticate = value; }

        Socket IBluetoothClient.Client { get => throw new PlatformNotSupportedException(); }

        public bool Connected
        {
            get
            {
                return _socket is object && _socket.IsConnected;
            }
        }

        private bool _encrypt;

        public bool Encrypt { get => _encrypt; set => _encrypt = value; }

        TimeSpan IBluetoothClient.InquiryLength { get => TimeSpan.Zero; set => throw new PlatformNotSupportedException(); }

        string IBluetoothClient.RemoteMachineName
        {
            get
            {
                if (_socket is object && _socket.IsConnected)
                    return _socket.RemoteDevice.Name;

                return null;
            }
        }

        public NetworkStream GetStream()
        {
            if (Connected)
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
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
