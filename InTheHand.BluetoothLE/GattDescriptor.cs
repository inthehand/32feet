//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
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
        public BluetoothUuid Uuid { get { return GetUuid(); } }

        /// <summary>
        /// The currently cached descriptor value. 
        /// This value gets updated when the value of the descriptor is read.
        /// </summary>
        public byte[] Value
        {
            get
            {
                return PlatformGetValue();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> ReadValueAsync()
        {
            return PlatformReadValue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task WriteValueAsync(byte[] value)
        {
            return PlatformWriteValue(value);
        }
    }
}
