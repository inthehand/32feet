//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        BluetoothUuid GetUuid()
        {
            return default;
        }

        private byte[] _value;
        byte[] PlatformGetValue()
        {
            return _value;
        }

        Task<byte[]> PlatformReadValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task PlatformWriteValue(byte[] value)
        {
            return Task.CompletedTask;
        }
    }
}
