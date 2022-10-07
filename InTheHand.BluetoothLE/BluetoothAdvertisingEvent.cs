//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-22 In The Hand Ltd, All rights reserved.
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

        public BluetoothUuid[] Uuids { get { return PlatformGetUuids(); } }

        public string Name { get { return PlatformGetName(); } }

        public ushort Appearance { get { return PlatformGetAppearance(); } }

        public sbyte TxPower { get { return PlatformGetTxPower(); } }

        public short Rssi { get { return PlatformGetRssi(); } }

        public IReadOnlyDictionary<ushort,byte[]> ManufacturerData { get { return PlatformGetManufacturerData(); } }

        public IReadOnlyDictionary<BluetoothUuid, byte[]> ServiceData { get { return PlatformGetServiceData(); } }
    }
}
