//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    public sealed partial class GattDescriptor
    {
        internal GattDescriptor(GattCharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        public GattCharacteristic Characteristic { get; private set; }

        public BluetoothUuid Uuid { get { return GetUuid(); } }

        public byte[] Value
        {
            get
            {
                var task = DoGetValue();
                task.Wait();
                return task.Result;
            }
        }

        public Task<byte[]> ReadValueAsync()
        {
            return DoReadValue();
        }

        public Task WriteValueAsync(byte[] value)
        {
            return DoWriteValue(value);
        }
    }
}