//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth.LE;
using Android.OS;
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

        /// <summary>
        /// Implicit conversion from <see cref="BluetoothAdvertisingEvent"/> to <see cref="ScanResult"/>.
        /// </summary>
        /// <param name="advertisingEvent"></param>
        public static implicit operator ScanResult(BluetoothAdvertisingEvent advertisingEvent)
        {
            return advertisingEvent._scanResult;
        }

        /// <summary>
        /// Implicit conversion from <see cref="ScanResult"/> to <see cref="BluetoothAdvertisingEvent"/>.
        /// </summary>
        /// <param name="scanResult"></param>
        public static implicit operator BluetoothAdvertisingEvent(ScanResult scanResult)
        {
            return new BluetoothAdvertisingEvent(scanResult);
        }

        private ushort PlatformGetAppearance()
        {
            return 0;
        }

        private BluetoothUuid[] PlatformGetUuids()
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

        private string PlatformGetName()
        {
            if(_scanResult.ScanRecord != null)
                return _scanResult.ScanRecord.DeviceName;

            if(_scanResult.Device != null)
                return _scanResult.Device.Name;

            return string.Empty;
        }

        private short PlatformGetRssi()
        {
            return (short)_scanResult.Rssi;
        }

        private sbyte PlatformGetTxPower()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                return (sbyte)_scanResult.TxPower;
            }

            return 0;
        }

        private IReadOnlyDictionary<ushort, byte[]> PlatformGetManufacturerData()
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

        private IReadOnlyDictionary<BluetoothUuid, byte[]> PlatformGetServiceData()
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