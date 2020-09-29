//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth.LE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        private readonly ScanResult _scanResult;

        private BluetoothAdvertisingEvent(ScanResult scanResult) : this(scanResult.Device)
        {
            _scanResult = scanResult;
        }

        public static implicit operator ScanResult(BluetoothAdvertisingEvent advertisingEvent)
        {
            return advertisingEvent._scanResult;
        }

        public static implicit operator BluetoothAdvertisingEvent(ScanResult scanResult)
        {
            return new BluetoothAdvertisingEvent(scanResult);
        }

        ushort GetAppearance()
        {
            return 0;
        }

        BluetoothUuid[] GetUuids()
        {
            List<BluetoothUuid> uuids = new List<BluetoothUuid>();

            if (_scanResult.ScanRecord != null && _scanResult.ScanRecord.ServiceUuids != null)
            {
                foreach (var u in _scanResult.ScanRecord.ServiceUuids)
                {
                    uuids.Add(u);
                }
            }

            return uuids.ToArray();
        }

        string GetName()
        {
            return _scanResult.ScanRecord.DeviceName;
        }

        short GetRssi()
        {
            return (short)_scanResult.Rssi;
        }

        sbyte GetTxPower()
        {
            return (sbyte)_scanResult.TxPower;
        }

        IReadOnlyDictionary<ushort, byte[]> GetManufacturerData()
        {
            Dictionary<ushort, byte[]> data = new Dictionary<ushort, byte[]>();
            for (int i = 0; i < _scanResult.ScanRecord.ManufacturerSpecificData.Size(); i++)
            {
                var id = _scanResult.ScanRecord.ManufacturerSpecificData.KeyAt(i);
                var val = (byte[])_scanResult.ScanRecord.ManufacturerSpecificData.ValueAt(i);
                data.Add((ushort)id, val);
            }

            return new ReadOnlyDictionary<ushort, byte[]>(data);
        }

        IReadOnlyDictionary<BluetoothUuid, byte[]> GetServiceData()
        {
            Dictionary<BluetoothUuid, byte[]> data = new Dictionary<BluetoothUuid, byte[]>();
            foreach(var entry in _scanResult.ScanRecord.ServiceData)
            {
                data.Add(entry.Key, entry.Value);
            }

            return new ReadOnlyDictionary<BluetoothUuid, byte[]>(data);
        }
    }
}