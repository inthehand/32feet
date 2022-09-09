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
        /// <summary>
        /// The unique identifier for the device.
        /// </summary>
        /// <remarks>On most platforms this will be the Bluetooth address, but some, such as iOS, will use a locally assigned unique id.</remarks>
        public string Id { get { return GetId(); } }
        /// <summary>
        /// The human-readable name of the device.
        /// </summary>
        public string Name { get { return GetName(); } }
        /// <summary>
        /// Provides a way to interact with this device’s GATT server.
        /// </summary>
        public RemoteGattServer Gatt { get { return GetGatt(); } }

        public static Task<BluetoothDevice> FromIdAsync(string id)
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
        public event EventHandler GattServerDisconnected;

        public override bool Equals(object obj)
        {
            BluetoothDevice device = obj as BluetoothDevice;
            if (device != null)
            {
                return Id.Equals(device.Id);
            }

            return base.Equals(obj);
        }
    }
}
