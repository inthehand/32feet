//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
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

        byte[] PlatformGetValue()
        {
            return null;
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
