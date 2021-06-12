//-----------------------------------------------------------------------
// <copyright file="GattService.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-21 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using WBluetooth = Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        readonly WBluetooth.GattDeviceService _service;
        readonly bool _isPrimary;

        internal GattService(BluetoothDevice device, WBluetooth.GattDeviceService service, bool isPrimary) : this(device)
        {
            _service = service;
            _isPrimary = isPrimary;
            device.AddDisposableObject(this,service);
        }

        public static implicit operator WBluetooth.GattDeviceService(GattService service)
        {
            return service._service;
        }

        public static implicit operator GattService(WBluetooth.GattDeviceService service)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return new GattService(service.Device, service, false);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        async Task<bool> OpenAsync()
        {
            return IsOpenSuccess(await _service.OpenAsync(WBluetooth.GattSharingMode.SharedReadAndWrite));
        }

        static bool IsOpenSuccess(WBluetooth.GattOpenStatus status)
        {
            switch(status)
            {
                case WBluetooth.GattOpenStatus.Success:
                case WBluetooth.GattOpenStatus.AlreadyOpened:
                    return true;

                default:
                    return false;
            }
        }

        async Task<GattCharacteristic> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            if(!await OpenAsync())
            {
                return null;
            }

            var result = await _service.GetCharacteristicsForUuidAsync(characteristic, Windows.Devices.Bluetooth.BluetoothCacheMode.Cached);

            if (result.Status == WBluetooth.GattCommunicationStatus.Success && result.Characteristics.Count > 0)
                return new GattCharacteristic(this, result.Characteristics[0]);

            return null;
        }

        async Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            if (!await OpenAsync())
            {
                return null;
            }

            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            var result = await _service.GetCharacteristicsAsync(BluetoothCacheMode.Cached);
            if(result.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                foreach(var c in result.Characteristics)
                {
                    characteristics.Add(new GattCharacteristic(this, c));
                }
            }

            return characteristics.AsReadOnly();
        }

        private async Task<GattService> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            if (!await OpenAsync())
            {
                return null;
            }

            var servicesResult = await _service.GetIncludedServicesForUuidAsync(service, BluetoothCacheMode.Cached);

            if(servicesResult.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                return new GattService(Device, servicesResult.Services[0], false);
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            if (!await OpenAsync())
            {
                return null;
            }

            List<GattService> services = new List<GattService>();

            var servicesResult = await _service.GetIncludedServicesAsync();

            if (servicesResult.Status == WBluetooth.GattCommunicationStatus.Success)
            {
                foreach(var includedService in servicesResult.Services)
                {
                    services.Add(new GattService(Device, includedService, false));
                }

                return services;
            }

            return null;
        }

        BluetoothUuid GetUuid()
        {
            return _service.Uuid;
        }

        bool GetIsPrimary()
        {
            return _isPrimary;
        }
    }
}
