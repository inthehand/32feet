//-----------------------------------------------------------------------
// <copyright file="BluetoothAdvertisingEvent.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        private readonly short _rssi;
        private readonly BluetoothLEAdvertisement _advertisement;

        internal BluetoothAdvertisingEvent(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            _rssi = args.RawSignalStrengthInDBm;
            _txPower = args.TransmitPowerLevelInDBm.HasValue ? (sbyte)args.TransmitPowerLevelInDBm.Value : (sbyte)0;

            /*var sections = args.Advertisement.GetSectionsByType(0xA);
            if(sections != null && sections.Count > 0)
            {
                var array = sections[0].Data.ToArray();

                _txPower = sections[0].Data.GetByte(0);
            }*/

            var appearanceSections = args.Advertisement.GetSectionsByType(0x19);
            if (appearanceSections != null && appearanceSections.Count > 0)
            {
                var appearanceArray = appearanceSections[0].Data.ToArray();
                _appearance = BitConverter.ToUInt16(appearanceArray, 0);
            }

            // https://docs.microsoft.com/en-us/uwp/api/windows.devices.bluetooth.bluetoothledevice.frombluetoothaddressasync?view=winrt-20348
            // If there are no other pending request, and the remote device is unreachable,
            // then the system will wait for seven (7) seconds before it times out. If
            // there are other pending requests, then each of the requests in the queue can
            // take seven (7) seconds to process, so the further yours is toward the back
            // of the queue, the longer you'll wait.
            IAsyncOperation<BluetoothLEDevice> deviceAsync = BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress, args.BluetoothAddressType);

            // https://github.com/inthehand/32feet/issues/96
            // Wait some time for this task to complete otherwise the event will fire
            // before the 'Device' property as been set.
            if (deviceAsync.AsTask().Wait(7000))
            {
                Device = deviceAsync.GetResults();
            }
            else
            {
                try
                {
                    // The documents state that it is not possible to cancel 'FromBluetoothAddressAsync'
                    // so mask any exceptions before calling this.
                    deviceAsync.Cancel();
                }
                catch
                {
                }
            }
            _advertisement = args.Advertisement;
        }

        public static implicit operator BluetoothAdvertisingEvent(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            return new BluetoothAdvertisingEvent(args);
        }

        private ushort _appearance;

        ushort GetAppearance()
        {
            return _appearance;
        }

        short GetRssi()
        {
            return _rssi;
        }

        private sbyte _txPower;

        sbyte GetTxPower()
        {
            return _txPower;
        }

        BluetoothUuid[] GetUuids()
        {
            List<BluetoothUuid> uuids = new List<BluetoothUuid>();
            foreach(var u in _advertisement.ServiceUuids)
            {
                uuids.Add(u);
            }

            return uuids.ToArray();
        }

        string GetName()
        {
            return _advertisement.LocalName;
        }

        IReadOnlyDictionary<ushort,byte[]> GetManufacturerData()
        {
            Dictionary<ushort, byte[]> manufacturerData = new Dictionary<ushort, byte[]>();

            foreach(BluetoothLEManufacturerData data in _advertisement.ManufacturerData)
            {
                manufacturerData.Add(data.CompanyId, data.Data.ToArray());
            }

            return new ReadOnlyDictionary<ushort,byte[]>(manufacturerData);
        }

        IReadOnlyDictionary<BluetoothUuid, byte[]> GetServiceData()
        {
            Dictionary<BluetoothUuid, byte[]> serviceData = new Dictionary<BluetoothUuid, byte[]>();

            foreach (BluetoothLEAdvertisementDataSection data in _advertisement.DataSections)
            {
                byte[] uuidBytes = new byte[16];

                if (data.DataType == BluetoothLEAdvertisementDataTypes.ServiceData128BitUuids)
                {
                    // read uuid
                    data.Data.CopyTo(0, uuidBytes, 0, 16);
                    // read data
                    byte[] dataBytes = new byte[data.Data.Length - 16];
                    data.Data.CopyTo(16, dataBytes, 0, dataBytes.Length);
                    serviceData.Add(new Guid(uuidBytes), dataBytes);
                }
                else if (data.DataType == BluetoothLEAdvertisementDataTypes.ServiceData32BitUuids)
                {
                    // read uuid
                    data.Data.CopyTo(0, uuidBytes, 0, 4);
                    // read data
                    byte[] dataBytes = new byte[data.Data.Length - 4];
                    data.Data.CopyTo(4, dataBytes, 0, dataBytes.Length);
                    serviceData.Add(BluetoothUuid.FromShortId(BitConverter.ToUInt16(uuidBytes, 0)), dataBytes);
                }
                else if (data.DataType == BluetoothLEAdvertisementDataTypes.ServiceData16BitUuids)
                {
                    // read uuid
                    data.Data.CopyTo(0, uuidBytes, 0, 2);
                    // read data
                    byte[] dataBytes = new byte[data.Data.Length - 2];
                    data.Data.CopyTo(2, dataBytes, 0, dataBytes.Length);
                    serviceData.Add(BluetoothUuid.FromShortId(BitConverter.ToUInt16(uuidBytes, 0)), dataBytes);
                }
                else
                {
                    if (data.DataType != BluetoothLEAdvertisementDataTypes.Flags)
                    {
                        System.Diagnostics.Debug.WriteLine(data.DataType);
                    }
                }
                
            }

            return new ReadOnlyDictionary<BluetoothUuid, byte[]>(serviceData);
        }
    }
}
