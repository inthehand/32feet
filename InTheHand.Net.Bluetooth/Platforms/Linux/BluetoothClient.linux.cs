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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        internal const AddressFamily AddressFamilyBluetooth = (AddressFamily)31;
        private Socket _socket;

        private void PlatformInitialize()
        {
            try
            {
                _socket = new Socket(AddressFamilyBluetooth, SocketType.Stream, BluetoothProtocolType.L2Cap);
            }
            catch(Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            var t = Task.Run<IEnumerable<BluetoothDeviceInfo>>(async () => 
            {
                List<BluetoothDeviceInfo> pairedDevices = new List<BluetoothDeviceInfo>();
                var devices = await BluetoothRadio.Default.Adapter.GetDevicesAsync();
                foreach (var device in devices)
                {
                    BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(device);
                    await bdi.Init();
                    pairedDevices.Add(bdi);
                }
                return pairedDevices.AsReadOnly();
            });
            t.Wait();
            return t.Result;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> PlatformDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

        async IAsyncEnumerable<BluetoothDeviceInfo> PlatformDiscoverDevicesAsync([EnumeratorCancellation]CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();
            BluetoothDeviceInfo device = null;
            var waitable = new AutoResetEvent(false);

            async Task handler(Adapter sender, DeviceFoundEventArgs eventArgs)
            {
                if (!eventArgs.IsStateChange)
                {
                    if (!devices.Contains((BluetoothDeviceInfo)eventArgs.Device))
                    {
                        device = eventArgs.Device;
                        await device.Init();
                        waitable.Set();
                    }
                }
            }

            BluetoothRadio.Default.Adapter.DeviceFound += handler;

            var t = Task.Run(async () =>
            {
                await BluetoothRadio.Default.Adapter.StartDiscoveryAsync();
                await Task.Delay(5000);
                await BluetoothRadio.Default.Adapter.StopDiscoveryAsync();
                waitable.Set();
                result.TrySetResult(true);
            }, cancellationToken);

            while(!t.IsCompleted)
            {
                waitable.WaitOne();
                yield return device;
            }

            yield break;
        }

        void PlatformConnect(BluetoothAddress address, Guid service)
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

        async Task PlatformConnectAsync(BluetoothAddress address, Guid service)
        {
            PlatformConnect(address, service);
        }

        void PlatformClose()
        {
            if (_socket != null && _socket.Connected)
                _socket.Close();
        }

        bool GetAuthenticate()
        {
            return false;
        }

        void SetAuthenticate(bool value)
        {
        }

        Socket GetClient()
        {
            return _socket;
        }

        bool GetConnected()
        {
            if (_socket == null)
                return false;

            return _socket.Connected;
        }

        bool GetEncrypt()
        {
            return false;
        }

        void SetEncrypt(bool value)
        {
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
            var remote = _socket.RemoteEndPoint as BluetoothEndPoint;
            return new BluetoothAddress(remote.Address).ToString();
        }

        NetworkStream PlatformGetStream()
        {
            if (Connected)
            {
                return new NetworkStream(_socket, true);
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
        
        #endregion
    }
}