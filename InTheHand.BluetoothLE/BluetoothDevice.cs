//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-22 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// A BluetoothDevice instance represents a remote Bluetooth device
    /// </summary>
    [DebuggerDisplay("{Id} ({Name})")]
    public sealed partial class BluetoothDevice
    {
        private BluetoothDevice() { }

        /// <summary>
        /// The unique identifier for the device.
        /// </summary>
        /// <remarks>On most platforms this will be the Bluetooth address, but some, such as iOS, will use a locally assigned unique id.</remarks>
        public string Id => GetId();

        /// <summary>
        /// The human-readable name of the device.
        /// </summary>
        public string Name => GetName();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RemoteGattServer? _remoteGattServer;
        /// <summary>
        /// Provides a way to interact with this device’s GATT server.
        /// </summary>
        public RemoteGattServer Gatt
        {
            get
            {
                if (_remoteGattServer == null)
                {
                    _remoteGattServer = GetGatt();
                }

                return _remoteGattServer;
            }
        }

        /// <summary>
        /// Returns true if the device is paired.
        /// </summary>
        /// <remarks>Supported on Windows and Android only.</remarks>
        public bool IsPaired => GetIsPaired();

        /// <summary>
        /// Initiate pairing. (Work in progress)
        /// </summary>
        /// <remarks>Supported on Windows and Android only.</remarks>
        /// <returns></returns>
        public Task PairAsync()
        {
            return PlatformPairAsync();
        }

        /// <summary>
        /// Initiate pairing with a pairing code (Work in progress)
        /// </summary>
        /// <param name="pairingCode">Bluetooth pairing code</param>
        /// <remarks>Implemented on Windows and Linux only.</remarks>
        public Task PairAsync(string pairingCode)
        {
            return PlatformPairAsync(pairingCode);
        }

        public static Task<BluetoothDevice?> FromIdAsync(string id)
        {
            return PlatformFromId(id);
        }
        /*
        public Task WatchAdvertisementsAsync()
        {
            return DoWatchAdvertisements();
        }

        public void UnwatchAdvertisements()
        {
            DoUnwatchAdvertisements();
        }

        public bool WatchingAdvertisements { get { return GetWatchingAdvertisements(); } }

        internal void OnAdvertismentReceived(BluetoothAdvertisingEvent advertisement)
        {
            AdvertisementReceived?.Invoke(this, advertisement);
        }

        public event EventHandler<BluetoothAdvertisingEvent> AdvertisementReceived;
        */

        internal void OnGattServerDisconnected()
        {
            GattServerDisconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fired when an active GATT connection is lost.
        /// </summary>
        public event EventHandler? GattServerDisconnected;
    }
}