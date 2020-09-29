//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
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
        ushort GetAppearance()
        {
            return 0;
        }

        BluetoothUuid[] GetUuids()
        {
            return new BluetoothUuid[] { };
        }

        string GetName()
        {
            return string.Empty;
        }

        short GetRssi()
        {
            return 0;
        }

        sbyte GetTxPower()
        {
            return 0;
        }

        IReadOnlyDictionary<ushort, byte[]> GetManufacturerData()
        {
            return new ReadOnlyDictionary<ushort, byte[]>(new Dictionary<ushort,byte[]>());
        }

        IReadOnlyDictionary<BluetoothUuid, byte[]> GetServiceData()
        {
            return new ReadOnlyDictionary<BluetoothUuid, byte[]>(new Dictionary<BluetoothUuid, byte[]>());
        }
    }
}