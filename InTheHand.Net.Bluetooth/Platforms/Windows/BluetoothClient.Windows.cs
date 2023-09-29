// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (WinRT)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    internal sealed class WindowsBluetoothClient : IBluetoothClient
    {
        private StreamSocket streamSocket;
        private bool _authenticate = false;

        internal WindowsBluetoothClient() { }

        internal WindowsBluetoothClient(StreamSocket socket)
        {
            streamSocket = socket;
        }

        internal const AddressFamily AddressFamilyBluetooth = (AddressFamily)32;
        private const SocketOptionLevel SocketOptionLevelRFComm = (SocketOptionLevel)0x03;
        private const SocketOptionName SocketOptionNameAuthenticate = unchecked((SocketOptionName)0x80000001);
        private const SocketOptionName SocketOptionNameEncrypt = (SocketOptionName)0x00000002;

        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                var t = DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));

                t.AsTask().ConfigureAwait(false);
                t.AsTask().Wait();
                var devices = t.GetResults();

                foreach (var device in devices)
                {
                    var td = BluetoothDevice.FromIdAsync(device.Id).AsTask();
                    td.Wait();
                    var bluetoothDevice = td.Result;
                    yield return new WindowsBluetoothDeviceInfo(bluetoothDevice);
                }

                yield break;
            }
        }

        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> results = new List<BluetoothDeviceInfo>();

            var devices = Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking.Logging.AsyncHelpers.RunSync<DeviceInformationCollection>(async ()=>
            {
                return await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(false));
            });

            foreach (var device in devices)
            {
                var bluetoothDevice = Microsoft.MixedReality.Toolkit.Examples.Demos.EyeTracking.Logging.AsyncHelpers.RunSync<BluetoothDevice>(async () =>
                {
                    return await BluetoothDevice.FromIdAsync(device.Id);
                });
                results.Add(new WindowsBluetoothDeviceInfo(bluetoothDevice));
            }
            return results.AsReadOnly();
        }

#if NET6_0_OR_GREATER
        public async IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var devices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(false)).AsTask(cancellationToken);
            
            foreach(var device in devices)
            {
                yield return new WindowsBluetoothDeviceInfo(await BluetoothDevice.FromIdAsync(device.Id));
            }

            yield break;
        }
#endif

        public async Task ConnectAsync(BluetoothAddress address, Guid service)
        {
            var device = await BluetoothDevice.FromBluetoothAddressAsync(address);
            var rfcommServices = await device.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(service), BluetoothCacheMode.Uncached);

            if (rfcommServices.Error == BluetoothError.Success)
            {
                var rfCommService = rfcommServices.Services[0];
                streamSocket = new StreamSocket();
                await streamSocket.ConnectAsync(rfCommService.ConnectionHostName, rfCommService.ConnectionServiceName, Authenticate ? SocketProtectionLevel.BluetoothEncryptionWithAuthentication : SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
            }
        }

        public void Connect(BluetoothAddress address, Guid service)
        {
            var t = Task.Run(async () =>
            {
                await ConnectAsync(address, service);
            });

            t.Wait();
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            Connect(remoteEP.Address, remoteEP.Service);
        }

        public void Close()
        {
            if (streamSocket != null)
            {
                streamSocket.Dispose();
                streamSocket = null;
            }
        }

        public bool Authenticate { get => _authenticate; set => _authenticate = value; }

        Socket IBluetoothClient.Client { get => throw new PlatformNotSupportedException(); }

        public bool Connected
        {
            get
            {
                if (streamSocket == null)
                    return false;

                return true;
            }
        }

        bool IBluetoothClient.Encrypt { get => false; set => throw new PlatformNotSupportedException(); }

        TimeSpan IBluetoothClient.InquiryLength { get => TimeSpan.Zero; set => throw new PlatformNotSupportedException(); }

        public string RemoteMachineName
        {
            get
            {
                if (Connected)
                {
                    return streamSocket.Information.RemoteHostName.DisplayName;
                }

                return string.Empty;
            }
        }

        public NetworkStream GetStream()
        {
            if (Connected)
            {
                return new WinRTNetworkStream(streamSocket, true);
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