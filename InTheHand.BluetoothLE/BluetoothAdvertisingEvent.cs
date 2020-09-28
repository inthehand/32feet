//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothAdvertisingEvent
    {
        internal BluetoothAdvertisingEvent(BluetoothDevice device)
        {
            Device = device;
        }

        public BluetoothDevice Device { get; private set; }

        public BluetoothUuid[] Uuids { get { return GetUuids(); } }

        public string Name { get { return GetName(); } }

        public ushort Appearance { get { return GetAppearance(); } }

        public sbyte TxPower { get { return GetTxPower(); } }

        public short Rssi { get { return GetRssi(); } }

        public IReadOnlyDictionary<ushort,byte[]> ManufacturerData { get { return GetManufacturerData(); } }

        public IReadOnlyDictionary<BluetoothUuid, byte[]> ServiceData { get { return GetServiceData(); } }
    }
}
