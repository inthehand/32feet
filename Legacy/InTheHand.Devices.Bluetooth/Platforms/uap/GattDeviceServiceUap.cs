//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InTheHand.Devices.Enumeration;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDeviceService
    {
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService  _service;

#if WINDOWS_PHONE
        private Windows.Devices.Bluetooth.BluetoothLEDevice _device;

        internal GattDeviceService(Windows.Devices.Bluetooth.BluetoothLEDevice device, Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            _device = device;
            _service = service;
        }
#endif
        private GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService(GattDeviceService service)
        {
            return service._service;
        }

        public static implicit operator GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            return new GattDeviceService(service);
        }

        private static async Task<GattDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return await Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(deviceId).AsTask();
        }

        private static string GetDeviceSelectorFromShortIdImpl(ushort serviceShortId)
        {
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromShortId(serviceShortId);
        }

        private static string GetDeviceSelectorFromUuidImpl(Guid serviceUuid)
        {
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid);
        }


        private void GetAllCharacteristics(List<GattCharacteristic> characteristics)
        {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetAllCharacteristics())
            {
                characteristics.Add(characteristic);
            }

#elif WINDOWS_PHONE || WINDOWS_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetCharacteristics(Guid.Empty))
            {
                characteristics.Add(characteristic);
            }
#endif
        }

        private void GetCharacteristics(Guid characteristicUuid, List<GattCharacteristic> characteristics)
        {
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetCharacteristics(characteristicUuid))
            {
                characteristics.Add(characteristic);
            }
        }

        /*private async Task<DeviceAccessStatus> RequestAccessAsync()
        {
            return (DeviceAccessStatus)((int)await _service.RequestAccessAsync());
        }*/

        private BluetoothLEDevice GetDevice()
        {
#if WINDOWS_APP
            return null;

#elif WINDOWS_PHONE
            return _device;

#else
            return _service.Device;
#endif
        }

        private Guid GetUuid()
        {
            return _service.Uuid;
        }

        private void DoDispose()
        {
            _service.Dispose();
            _service = null;
        }
    }
}