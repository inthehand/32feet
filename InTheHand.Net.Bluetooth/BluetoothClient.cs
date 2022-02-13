// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Provides client connections for Bluetooth Rfcomm network services.
    /// </summary>
    public sealed partial class BluetoothClient : IDisposable
    {
        public BluetoothClient()
        {
            PlatformInitialize();
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
        /// <returns>A collection of BluetoothDeviceInfo objects describing the devices discovered.</returns>
        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices()
        {
            return DiscoverDevices(255);
        }

        /// <summary>
        /// Discovers accessible Bluetooth devices, and returns their names and addresses.
        /// </summary>
        /// <returns>A collection of BluetoothDeviceInfo objects describing the devices discovered.</returns>
        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            return PlatformDiscoverDevices(maxDevices);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified Bluetooth address and service identifier.
        /// </summary>
        /// <param name="address">The BluetoothAddress of the remote host.</param>
        /// <param name="service">The Service Class Id of the service on the remote host.
        /// The standard Bluetooth service classes are provided on <see cref="BluetoothService"/>.</param>
        public void Connect(BluetoothAddress address, Guid service)
        {
            PlatformConnect(address, service);
        }

        /// <summary>
        /// Closes the BluetoothClient and the underlying connection.
        /// </summary>
        /// <remarks>The Close method marks the instance as disposed and requests that the associated Socket close the Bluetooth connection</remarks>
        public void Close()
        {
            PlatformClose();
        }

        /// <summary>
        /// Sets whether an authenticated connection is required.
        /// </summary>
        public bool Authenticate
        {
            get => GetAuthenticate();
            set => SetAuthenticate(value);
        }

        /// <summary>
        /// Gets the underlying Socket.
        /// </summary>
        public Socket Client
        {
            get => GetClient();
        }

        /// <summary>
        /// Gets a value indicating whether the underlying Socket for a BluetoothClient is connected to a remote host.
        /// </summary>
        public bool Connected
        {
            get => GetConnected();
        }

        /// <summary>
        /// Sets whether an encrypted connection is required.
        /// </summary>
        public bool Encrypt
        {
            get => GetEncrypt();
            set => SetEncrypt(value);
        }

        /// <summary>
        /// Amount of time allowed to perform the query.
        /// </summary>
        /// <remarks>On Windows the actual value used is expressed in units of 1.28 seconds, so will be the nearest match for the value supplied.
        /// The default value is 10 seconds. The maximum is 61 seconds.</remarks>
        public TimeSpan InquiryLength
        {
            [DebuggerStepThrough]
            get { return GetInquiryLength(); }
            [DebuggerStepThrough]
            set { SetInquiryLength(value); }
        }

        /// <summary>
        /// Gets the name of the remote device.
        /// </summary>
        public string RemoteMachineName
        {
            get
            {
                return GetRemoteMachineName();
            }
        }

        /// <summary>
        /// Gets the underlying stream of data.
        /// </summary>
        /// <returns></returns>
        public NetworkStream GetStream()
        {
            return PlatformGetStream();
        }

        /// <summary>
        /// Closes the BluetoothClient and the underlying connection.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}