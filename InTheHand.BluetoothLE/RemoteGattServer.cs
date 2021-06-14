//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    public sealed partial class RemoteGattServer
    {
        internal RemoteGattServer(BluetoothDevice device)
        {
            Device = device;
            PlatformInit();
        }

        private void Device_GattServerDisconnected(object sender, EventArgs e)
        {
            Device.OnGattServerDisconnected();
        }

        /// <summary>
        /// Gets the parent remote device.
        /// </summary>
        public BluetoothDevice Device { get; private set; }

        [Obsolete("Use IsConnected instead", true)]
        public bool Connected { get { return IsConnected; } }

        /// <summary>
        /// Gets a value indicating if there is an active connection with the remote GATT server.
        /// </summary>
        /// <value>True if connected, else False.</value>
        public bool IsConnected { get { return GetConnected(); } }

        /// <summary>
        /// Open a connection to the remote GATT server.
        /// </summary>
        /// <returns></returns>
        public Task ConnectAsync()
        {
            return PlatformConnect();
        }

        /// <summary>
        /// Disconnects the remote GATT server.
        /// </summary>
        /// <remarks>Some platforms do not have an explicit Disconnect and will keep the connection open until there are no references to associated services and characteristics.</remarks>
        public void Disconnect()
        {
            PlatformDisconnect();
            Device.OnGattServerDisconnected();
            PlatformCleanup();
        }

        /// <summary>
        /// Returns the primary service matching the specified UUID.
        /// </summary>
        /// <param name="service">The requested service or null if not present.</param>
        /// <returns></returns>
        public Task<GattService> GetPrimaryServiceAsync(BluetoothUuid service)
        {
            return PlatformGetPrimaryService(service);
        }

        /// <summary>
        /// Returns primary services, optionally filtering to the supplied UUID.
        /// </summary>
        /// <param name="service">Optional service UUID.</param>
        /// <returns>A list of matching primary services.</returns>
        public Task<List<GattService>> GetPrimaryServicesAsync(BluetoothUuid? service = null)
        {
            return PlatformGetPrimaryServices(service);
        }

        /// <summary>
        /// Requests the Received Signal Strength Indication (RSSI) from the remote device.
        /// </summary>
        /// <returns>RSSI or Zero if unavailable.</returns>
        public Task<short> ReadRssi()
        {
            return PlatformReadRssi();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BluetoothPhy _preferredPhy = BluetoothPhy.Le1m;
        /// <summary>
        /// Sets the preferred Physical Layer (PHY) if supported.
        /// </summary>
        /// <remarks>Currently only supported on Android.</remarks>
        /// <param name="phy"></param>
        /// <returns></returns>
        public BluetoothPhy PreferredPhy
        {
            get
            {
                return _preferredPhy;
            }
            set
            {
                _preferredPhy = value;
                PlatformSetPreferredPhy(value);
            }
        }
    }
}
