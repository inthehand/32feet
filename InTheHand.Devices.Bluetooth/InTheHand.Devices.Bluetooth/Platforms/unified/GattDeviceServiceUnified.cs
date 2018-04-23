//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using CoreBluetooth;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDeviceService
    {
        internal CBService _service;
        private CBPeripheral _peripheral;
      
        internal GattDeviceService(CBService service, CBPeripheral peripheral)
        {
            _service = service;
            _peripheral = peripheral;
            _peripheral.DiscoveredCharacteristic += _peripheral_DiscoveredCharacteristic;
        }

        ~GattDeviceService()
        {
            _peripheral.DiscoveredCharacteristic -= _peripheral_DiscoveredCharacteristic;
        }

        private void _peripheral_DiscoveredCharacteristic(object sender, CBServiceEventArgs e)
        {
            if (e.Service == _service)
            {
                Debug.WriteLine(DateTimeOffset.Now.ToString() + " DiscoveredCharacteristic");
            }
        }

        private static async Task<GattDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return null;
        }

        private static string GetDeviceSelectorFromShortIdImpl(ushort serviceShortId)
        {
            return "bluetoothService:" + BluetoothUuidHelper.FromShortId(serviceShortId).ToString();
        }

        private static string GetDeviceSelectorFromUuidImpl(Guid serviceUuid)
        {
            return "bluetoothService:" + serviceUuid.ToString();
        }


        private void GetAllCharacteristics(List<GattCharacteristic> characteristics)
        {
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, characteristic));
            }
        }

        private void GetCharacteristics(Guid characteristicUuid, List<GattCharacteristic> characteristics)
        {
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                if (characteristic.UUID.ToGuid() == characteristicUuid)
                {
                    characteristics.Add(new GattCharacteristic(this, characteristic));
                }
            }
        }

        private BluetoothLEDevice GetDevice()
        {
            return _peripheral;
        }

        private Guid GetUuid()
        {
            return _service.UUID.ToGuid();
        }

        private void DoDispose()
        {
            _peripheral = null;
            _service.Dispose();
            _service = null;
        }
    }
}