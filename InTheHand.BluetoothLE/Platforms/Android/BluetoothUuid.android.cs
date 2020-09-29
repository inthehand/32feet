//-----------------------------------------------------------------------
// <copyright file="BluetoothUuid.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2019-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.OS;
using Java.Util;
using System;

namespace InTheHand.Bluetooth
{
    partial struct BluetoothUuid
    {
        public static implicit operator UUID(BluetoothUuid uuid)
        {
            return UUID.FromString(uuid.Value.ToString());
        }

        public static implicit operator ParcelUuid(BluetoothUuid uuid)
        {
            return ParcelUuid.FromString(uuid.Value.ToString());
        }

        public static implicit operator BluetoothUuid(UUID uuid)
        {
            return Guid.Parse(uuid.ToString());
        }

        public static implicit operator BluetoothUuid(ParcelUuid uuid)
        {
            return Guid.Parse(uuid.ToString());
        }
    }
}
