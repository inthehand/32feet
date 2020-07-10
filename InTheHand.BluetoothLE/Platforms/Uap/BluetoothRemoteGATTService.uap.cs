//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTService.uap.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using InTheHand.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        readonly GattDeviceService _service;

        internal BluetoothRemoteGATTService(BluetoothDevice device, GattDeviceService service) : this(device)
        {
            _service = service;
        }

        public static implicit operator GattDeviceService(BluetoothRemoteGATTService service)
        {
            return service._service;
        }

        async Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            var result = await _service.GetCharacteristicsForUuidAsync(characteristic);

            if (result.Status == GattCommunicationStatus.Success && result.Characteristics.Count > 0)
                return new GattCharacteristic(this, result.Characteristics[0]);

            return null;
        }

        async Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            var result = await _service.GetCharacteristicsAsync();
            if(result.Status == GattCommunicationStatus.Success)
            {
                foreach(var c in result.Characteristics)
                {
                    characteristics.Add(new GattCharacteristic(this, c));
                }
            }

            return characteristics.AsReadOnly();
        }

        private async Task<BluetoothRemoteGATTService> DoGetIncludedServiceAsync(Guid service)
        {
            var servicesResult = await _service.GetIncludedServicesForUuidAsync(service);

            if(servicesResult.Status == GattCommunicationStatus.Success)
            {
                return new BluetoothRemoteGATTService(Device, servicesResult.Services[0]);
            }

            return null;
        }

        private async Task<IReadOnlyList<BluetoothRemoteGATTService>> DoGetIncludedServicesAsync()
        {
            List<BluetoothRemoteGATTService> services = new List<BluetoothRemoteGATTService>();

            var servicesResult = await _service.GetIncludedServicesAsync();

            if (servicesResult.Status == GattCommunicationStatus.Success)
            {
                foreach(var includedService in servicesResult.Services)
                {
                    services.Add(new BluetoothRemoteGATTService(Device, includedService));
                }

                return services;
            }

            return null;
        }

        Guid GetUuid()
        {
            return _service.Uuid;
        }

        bool GetIsPrimary()
        {
            return _service.ParentServices == null || _service.ParentServices.Count == 0;
        }
    }
}
