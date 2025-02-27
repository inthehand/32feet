//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents a GATT Descriptor, which provides further information about a <see cref="GattCharacteristic"/>’s value.
    /// </summary>
    [DebuggerDisplay("{Uuid} (Descriptor)")]
    public sealed partial class GattDescriptor
    {
        internal GattDescriptor(GattCharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        /// <summary>
        /// The GATT characteristic this descriptor belongs to.
        /// </summary>
        public GattCharacteristic Characteristic { get; private set; }

        /// <summary>
        /// The UUID of the characteristic descriptor.
        /// </summary>
        public BluetoothUuid Uuid => GetUuid();

        /// <summary>
        /// The currently cached descriptor value. 
        /// This value gets updated when the value of the descriptor is read.
        /// </summary>
        public byte[] Value => PlatformGetValue();

        /// <summary>
        /// Retrieve the current descriptor value from the remote device.
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> ReadValueAsync() => PlatformReadValue();

        /// <summary>
        /// Writes a new value to the descriptor on the remote device.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task WriteValueAsync(byte[] value)
        {
            Bluetooth.ThrowOnInvalidAttributeValue(value);
            return PlatformWriteValue(value);
        }
    }
}
