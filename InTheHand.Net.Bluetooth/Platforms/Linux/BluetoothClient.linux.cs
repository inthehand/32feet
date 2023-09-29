// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (Linux)
// 
// Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace InTheHand.Net.Sockets
{
    internal sealed class LinuxBluetoothClient : IBluetoothClient, IDisposable
    {
        private LinuxSocket _socket;

        public LinuxBluetoothClient()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new PlatformNotSupportedException("Linux library used on non-Linux OS.");

            _socket = new LinuxSocket();
        }

        internal LinuxBluetoothClient(LinuxSocket s)
        {
            _socket = s;
        }

        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                var t = Task.Run<IEnumerable<BluetoothDeviceInfo>>(async () =>
                {
                    List<BluetoothDeviceInfo> pairedDevices = new List<BluetoothDeviceInfo>();
                    var devices = await ((Adapter)BluetoothRadio.Default).GetDevicesAsync();
                    foreach (var device in devices)
                    {
                        var bdi = new LinuxBluetoothDeviceInfo(device);
                        await bdi.Init();
                        pairedDevices.Add(bdi);
                    }
                    return pairedDevices.AsReadOnly();
                });
                t.Wait();
                return t.Result;
            }
        }

        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

        public async IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();
            LinuxBluetoothDeviceInfo device = null;
            var waitable = new AutoResetEvent(false);
            var adapter = (Adapter)BluetoothRadio.Default;

            async Task handler(Adapter sender, DeviceFoundEventArgs eventArgs)
            {
                if (!eventArgs.IsStateChange)
                {
                    if (!devices.Contains((LinuxBluetoothDeviceInfo)eventArgs.Device))
                    {
                        device = eventArgs.Device;
                        await device.Init();
                        waitable.Set();
                    }
                }
            }

            adapter.DeviceFound += handler;

            var t = Task.Run(async () =>
            {
                await adapter.StartDiscoveryAsync();
                await Task.Delay(5000);
                await adapter.StopDiscoveryAsync();
                waitable.Set();
                result.TrySetResult(true);
            }, cancellationToken);

            while (!t.IsCompleted)
            {
                waitable.WaitOne();
                yield return device;
            }

            adapter.DeviceFound -= handler;
            yield break;
        }

        public void Connect(BluetoothAddress address, Guid service)
        {
            var ep = new BluetoothEndPoint(address, service);

            Connect(ep);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            _socket.Connect(remoteEP);
        }

        public async Task ConnectAsync(BluetoothAddress address, Guid service)
        {
            Connect(address, service);
        }

        public void Close()
        {
            if (_socket != null && _socket.Connected)
                _socket.Close();
        }

        bool IBluetoothClient.Authenticate { get => false; set { } }

        Socket IBluetoothClient.Client { get => _socket; }

        public bool Connected
        {
            get
            {
                if (_socket == null)
                    return false;

                return _socket.Connected;
            }
        }

        bool IBluetoothClient.Encrypt { get=> false; set { } }

        TimeSpan IBluetoothClient.InquiryLength { get => TimeSpan.Zero; set { } }

        string IBluetoothClient.RemoteMachineName
        {
            get
            {
                var remote = _socket.RemoteEndPoint as BluetoothEndPoint;
                return new BluetoothAddress(remote.Address).ToString();
            }
        }

        public NetworkStream GetStream()
        {
            if (Connected)
            {
                return new LinuxNetworkStream(_socket, true);
            }

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