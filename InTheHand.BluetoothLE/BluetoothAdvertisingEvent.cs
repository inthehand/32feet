//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents a received advertising packet.
    /// </summary>
    public sealed partial class BluetoothAdvertisingEvent
    {
        internal BluetoothAdvertisingEvent(BluetoothDevice device)
        {
            Device = device;
        }

        /// <summary>
        /// The device that sent the advertisement.
        /// </summary>
        public BluetoothDevice Device { get; private set; }

        /// <summary>
        /// List of UUIDs contained in the advertisement.
        /// </summary>
        public BluetoothUuid[] Uuids { get { return PlatformGetUuids(); } }

        /// <summary>
        /// Name sent with the advertisement.
        /// </summary>
        /// <remarks>Could be a shortened or complete name.</remarks>
        public string Name { get { return PlatformGetName(); } }

        /// <summary>
        /// Appearance data describing the type of device sending the advertisement.
        /// </summary>
        public ushort Appearance { get { return PlatformGetAppearance(); } }

        /// <summary>
        /// Transmit power.
        /// </summary>
        public sbyte TxPower { get { return PlatformGetTxPower(); } }

        /// <summary>
        /// Received Signal Strength.
        /// </summary>
        public short Rssi { get { return PlatformGetRssi(); } }

        /// <summary>
        /// Manufacturer specific data in a dictionary with the manufacturer identifier as the key.
        /// </summary>
        public IReadOnlyDictionary<ushort,byte[]> ManufacturerData { get { return PlatformGetManufacturerData(); } }

        /// <summary>
        /// Service data relating to each service UUID.
        /// </summary>
        public IReadOnlyDictionary<BluetoothUuid, byte[]> ServiceData { get { return PlatformGetServiceData(); } }
    }
}
