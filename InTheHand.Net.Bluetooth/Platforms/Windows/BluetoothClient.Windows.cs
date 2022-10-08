// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (WinRT)
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private StreamSocket streamSocket;

        internal BluetoothClient(StreamSocket socket)
        {
            this.streamSocket = socket;
        }

        private void PlatformInitialize()
        {
        }

        internal const AddressFamily AddressFamilyBluetooth = (AddressFamily)32;
        private const SocketOptionLevel SocketOptionLevelRFComm = (SocketOptionLevel)0x03;
        private const SocketOptionName SocketOptionNameAuthenticate = unchecked((SocketOptionName)0x80000001);
        private const SocketOptionName SocketOptionNameEncrypt = (SocketOptionName)0x00000002;

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            var t = DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));

            t.AsTask().ConfigureAwait(false);
            t.AsTask().Wait();
            var devices = t.GetResults();

            foreach(var device in devices)
            {
                var td = BluetoothDevice.FromIdAsync(device.Id).AsTask();
                td.Wait();
                var bluetoothDevice = td.Result;
                yield return bluetoothDevice;
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> PlatformDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> results = new List<BluetoothDeviceInfo>();

            var devices = DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(false)).GetResults();

            foreach (var device in devices)
            {
                var bluetoothDevice = BluetoothDevice.FromIdAsync(device.Id).GetResults();
                results.Add(bluetoothDevice);
            }
            return results.AsReadOnly();
        }

        void PlatformConnect(BluetoothAddress address, Guid service)
        {
            var t = Task.Run(async () =>
            {
                var device = await BluetoothDevice.FromBluetoothAddressAsync(address);
                var rfcommServices = await device.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(service));

                if(rfcommServices.Error == BluetoothError.Success)
                {
                    var rfCommService = rfcommServices.Services[0];
                    streamSocket = new StreamSocket();
                    await streamSocket.ConnectAsync(rfCommService.ConnectionHostName, rfCommService.ConnectionServiceName);
                }
            });

            t.Wait();

            /*var ep = new BluetoothEndPoint(address, service);

            Connect(ep);*/
        }

        /*
        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            _socket.Connect(remoteEP);
        }*/

        void PlatformClose()
        {
            if (streamSocket != null)
            {
                streamSocket.Dispose();
                streamSocket = null;
            }
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
            return null;
        }

        bool GetConnected()
        {
            if (streamSocket == null)
                return false;

            return true;
        }

        bool GetEncrypt()
        {
            return false;
        }

        TimeSpan GetInquiryLength()
        {
            return TimeSpan.Zero;
        }

        void SetInquiryLength(TimeSpan length)
        {

        }

        void SetEncrypt(bool value)
        {
        }

        string GetRemoteMachineName()
        {
            if (GetConnected())
            {
                return streamSocket.Information.RemoteHostName.DisplayName;
            }

            return string.Empty;
        }

        NetworkStream PlatformGetStream()
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
        
        #endregion
    }
}