// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Provides client connections for Bluetooth Rfcomm network services.
    /// </summary>
    public sealed class BluetoothClient : IDisposable
    {
        private IBluetoothClient _bluetoothClient;

        public BluetoothClient()
        {
#if ANDROID || MONOANDROID
            _bluetoothClient = new AndroidBluetoothClient();
#elif IOS || __IOS__
            _bluetoothClient = new ExternalAccessoryBluetoothClient();
#elif WINDOWS_UWP || WINDOWS10_0_17763_0_OR_GREATER
            _bluetoothClient = new WindowsBluetoothClient();
#elif NET462 || WINDOWS7_0_OR_GREATER
            _bluetoothClient = new Win32BluetoothClient();
#elif NETSTANDARD
#else
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // check for macOS goes here
                    _bluetoothClient = new LinuxBluetoothClient();
                    break;
                case PlatformID.Win32NT:
                    // check for Windows 10 goes here
                    _bluetoothClient = new Win32BluetoothClient();
                    break;
            }
#endif
            if (_bluetoothClient == null)
                throw new PlatformNotSupportedException();
        }

        internal BluetoothClient(IBluetoothClient client)
        {
            _bluetoothClient = client;
        }

        /// <summary>
        /// Returns a collection of paired devices.
        /// </summary>
        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                return _bluetoothClient.PairedDevices;
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
            return _bluetoothClient.DiscoverDevices(maxDevices);
        }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return _bluetoothClient.DiscoverDevicesAsync(cancellationToken);
        }
#endif
        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified Bluetooth address and service identifier.
        /// </summary>
        /// <param name="address">The BluetoothAddress of the remote host.</param>
        /// <param name="service">The Service UUID of the service on the remote host.
        /// The standard Bluetooth service UUIDs are provided on <see cref="BluetoothService"/>.</param>
        /// <remarks>On iOS the connection will be made to the first protocol exposed by the ExternalAccessory unless you first provide a mapping between the Uuid of your choice and the protocol name using <see cref="BluetoothServiceProtocolMapping"/>.</remarks>
        public void Connect(BluetoothAddress address, Guid service)
        {
            _bluetoothClient.Connect(address, service);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            _bluetoothClient.Connect(remoteEP);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified address and service UUID as an asynchronous operation.
        /// </summary>
        /// <param name="address">The BluetoothAddress of the remote host.</param>
        /// <param name="service">The service UUID of the remote host.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public Task ConnectAsync(BluetoothAddress address, Guid service)
        {
            return _bluetoothClient.ConnectAsync(address, service);
        }

        /// <summary>
        /// Closes the BluetoothClient and the underlying connection.
        /// </summary>
        /// <remarks>The Close method marks the instance as disposed and requests that the associated Socket close the Bluetooth connection</remarks>
        public void Close()
        {
            _bluetoothClient.Close();
        }

        /// <summary>
        /// Sets whether an authenticated connection is required.
        /// </summary>
        public bool Authenticate
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.Authenticate;
            [DebuggerStepThrough]
            set => _bluetoothClient.Authenticate = value;
        }

        /// <summary>
        /// Gets the underlying Socket.
        /// </summary>
        public Socket Client
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.Client;
        }

        /// <summary>
        /// Gets a value indicating whether the underlying Socket for a BluetoothClient is connected to a remote host.
        /// </summary>
        public bool Connected
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.Connected;
        }

        /// <summary>
        /// Sets whether an encrypted connection is required.
        /// </summary>
        public bool Encrypt
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.Encrypt;
            [DebuggerStepThrough]
            set => _bluetoothClient.Encrypt = value;
        }

        /// <summary>
        /// Amount of time allowed to perform the query.
        /// </summary>
        /// <remarks>On Windows the actual value used is expressed in units of 1.28 seconds, so will be the nearest match for the value supplied.
        /// The default value is 10 seconds. The maximum is 61 seconds.</remarks>
        public TimeSpan InquiryLength
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.InquiryLength;
            [DebuggerStepThrough]
            set => _bluetoothClient.InquiryLength = value;
        }

        /// <summary>
        /// Gets the name of the remote device.
        /// </summary>
        public string RemoteMachineName
        {
            [DebuggerStepThrough]
            get => _bluetoothClient.RemoteMachineName;
        }

        /// <summary>
        /// Gets the underlying stream of data.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public NetworkStream GetStream() => _bluetoothClient.GetStream();

        /// <summary>
        /// Closes the BluetoothClient and the underlying connection.
        /// </summary>
        public void Dispose()
        {
            _bluetoothClient.Dispose();
            _bluetoothClient = null;
        }
    }
}