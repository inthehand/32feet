//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.wasm.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        ushort PlatformGetAppearance()
        {
            return 0;
        }

        BluetoothUuid[] PlatformGetUuids()
        {
            return new BluetoothUuid[] { };
        }

        string PlatformGetName()
        {
            return string.Empty;
        }

        short PlatformGetRssi()
        {
            return 0;
        }

        sbyte PlatformGetTxPower()
        {
            return 0;
        }

        IReadOnlyDictionary<ushort, byte[]> PlatformGetManufacturerData()
        {
            return new ReadOnlyDictionary<ushort, byte[]>(new Dictionary<ushort,byte[]>());
        }

        IReadOnlyDictionary<BluetoothUuid, byte[]> PlatformGetServiceData()
        {
            return new ReadOnlyDictionary<BluetoothUuid, byte[]>(new Dictionary<BluetoothUuid, byte[]>());
        }
    }
}