using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace InTheHand.Bluetooth
{
    partial class BluetoothAdvertisingEvent
    {
        private short _rssi;
        private BluetoothLEDevice _device;
        private BluetoothLEAdvertisement _advertisement;

        internal BluetoothAdvertisingEvent(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            _rssi = args.RawSignalStrengthInDBm;
            _device = BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress).GetResults();
            _advertisement = args.Advertisement;
        }

        public static implicit operator BluetoothAdvertisingEvent(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            return new BluetoothAdvertisingEvent(args);
        }

        ushort GetAppearance()
        {
            return _device.Appearance.RawValue;
        }

        short GetRssi()
        {
            return _rssi;
        }

        Guid[] GetUuids()
        {
            return _advertisement.ServiceUuids.ToArray();
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

        IReadOnlyDictionary<Guid, byte[]> GetServiceData()
        {
            Dictionary<Guid, byte[]> serviceData = new Dictionary<Guid, byte[]>();

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
                    serviceData.Add(BluetoothUuid.FromShortId(BitConverter.ToUInt32(uuidBytes, 0)), dataBytes);
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

            return new ReadOnlyDictionary<Guid, byte[]>(serviceData);
        }
    }
}
