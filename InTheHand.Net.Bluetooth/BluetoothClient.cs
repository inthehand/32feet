// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class BluetoothClient : IDisposable
    {
        public BluetoothClient()
        {
        }

        /// <summary>
        /// Returns a collection of paired devices.
        /// </summary>
        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                return GetPairedDevices();
            }
        }

        /// <summary>
        /// Discovers accessible Bluetooth devices, and returns their names and addresses.
        /// </summary>
        /// <returns>An array of BluetoothDeviceInfo objects describing the devices discovered.</returns>
        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices()
        {
            return DiscoverDevices(255);
        }

        /// <summary>
        /// Discovers accessible Bluetooth devices, and returns their names and addresses.
        /// </summary>
        /// <returns>An array of BluetoothDeviceInfo objects describing the devices discovered.</returns>
        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            return DoDiscoverDevices(maxDevices);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified Bluetooth address and service identifier.
        /// </summary>
        /// <param name="address">The BluetoothAddress of the remote host.</param>
        /// <param name="service">The Service Class Id of the service on the remote host.
        /// The standard Bluetooth service classes are provided on <see cref="BluetoothService"/>.</param>
        public void Connect(BluetoothAddress address, Guid service)
        {
            DoConnect(address, service);
        }

        /// <summary>
        /// Closes the BluetoothClient and the underlying connection.
        /// </summary>
        /// <remarks>The Close method marks the instance as disposed and requests that the associated Socket close the Bluetooth connection</remarks>
        public void Close()
        {
            DoClose();
        }

        public bool Connected
        {
            get => GetConnected();
        }

        public bool Authenticate
        {
            get => GetAuthenticate();
            set => SetAuthenticate(value);
        }

        public bool Encrypt
        {
            get => GetEncrypt();
            set => SetEncrypt(value);
        }

        public string RemoteMachineName
        {
            get
            {
                return GetRemoteMachineName();
            }
        }

        public NetworkStream GetStream()
        {
            return DoGetStream();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}