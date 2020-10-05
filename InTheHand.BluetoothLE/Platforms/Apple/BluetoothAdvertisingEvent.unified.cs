//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        private NSDictionary _advertisementData;
        private short _rssi;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private sbyte _txPower;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BluetoothUuid[] _uuids = new BluetoothUuid[] { };
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<ushort, byte[]>  _manufacturerData = new Dictionary<ushort, byte[]>();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<BluetoothUuid, byte[]> _serviceData = new Dictionary<BluetoothUuid, byte[]>();

        internal BluetoothAdvertisingEvent(BluetoothDevice device, NSDictionary advertisementData, NSNumber RSSI) : this(device)
        {
            Device = device;
            _rssi = RSSI.Int16Value;
            _advertisementData = advertisementData;

            foreach (var entry in advertisementData)
            {
                switch(entry.Key.ToString())
                {
                    case "kCBAdvDataLocalName":
                        _name = entry.Value.ToString();
                        break;

                    case "kCBAdvDataServiceUUIDs":
                        List<BluetoothUuid> uuids = new List<BluetoothUuid>();
                        var array = ((NSArray)entry.Value);
                        for(nuint i = 0; i < array.Count; i++)
                        {
                            uuids.Add(array.GetItem<CBUUID>(i));
                        }
                        _uuids = uuids.ToArray();
                        break;

                    case "kCBAdvDataManufacturerData":
                        var data = ((NSData)entry.Value).ToArray();
                        var manufacturerId = BitConverter.ToUInt16(data, 0);
                        var manufacturerData = new byte[data.Length - 2];
                        Buffer.BlockCopy(data, 2, manufacturerData, 0, manufacturerData.Length);
                        _manufacturerData.Add(manufacturerId, manufacturerData);
                        break;

                    case "kCBAdvDataServiceData":
                        var rawServiceData = ((NSDictionary)entry.Value);
                        foreach(var serviceEntry in rawServiceData)
                        {
                            _serviceData.Add(((CBUUID)serviceEntry.Key), ((NSData)serviceEntry.Value).ToArray());
                        }
                        break;

                    case "kCBAdvDataTxPowerLevel":
                        _txPower = ((NSNumber)entry.Value).SByteValue;
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine(entry.Key);
                        break;
                }
            }
        }

        ushort GetAppearance()
        {
            return 0;
        }

        BluetoothUuid[] GetUuids()
        {
            return _uuids;
        }

        string GetName()
        {
            return _name;
        }

        short GetRssi()
        {
            return _rssi;
        }

        sbyte GetTxPower()
        {
            return _txPower;
        }

        IReadOnlyDictionary<ushort, byte[]> GetManufacturerData()
        {
            return new ReadOnlyDictionary<ushort, byte[]>(_manufacturerData);
        }

        IReadOnlyDictionary<BluetoothUuid, byte[]> GetServiceData()
        {
            return new ReadOnlyDictionary<BluetoothUuid, byte[]>(_serviceData);
        }
    }
}