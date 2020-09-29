//-----------------------------------------------------------------------
// <copyright file="GattService.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        private CBService _service;

        internal GattService(BluetoothDevice device, CBService service) : this(device)
        {
            _service = service;
        }

        public static implicit operator CBService(GattService service)
        {
            return service._service;
        }

        BluetoothUuid GetUuid()
        {
            return _service.UUID;
        }

        bool GetIsPrimary()
        {
            return true;
        }

        Task<GattCharacteristic> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            ((CBPeripheral)Device).DiscoverCharacteristics(new CBUUID[] { characteristic }, _service);
            GattCharacteristic matchingCharacteristic = null;

            foreach(CBCharacteristic cbcharacteristic in _service.Characteristics)
            {
                if((BluetoothUuid)cbcharacteristic.UUID == characteristic)
                {
                    matchingCharacteristic = new GattCharacteristic(this, cbcharacteristic);
                    break;
                }
            }

            return Task.FromResult(matchingCharacteristic);
        }

        Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            ((CBPeripheral)Device).DiscoverCharacteristics(_service);

            foreach (CBCharacteristic cbcharacteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, cbcharacteristic));
            }

            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }

        private async Task<GattService> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            ((CBPeripheral)Device).DiscoverIncludedServices(new CBUUID[] { }, _service);

            foreach (var includedService in _service.IncludedServices)
            {
                if ((BluetoothUuid)includedService.UUID == service)
                    return new GattService(Device, includedService);
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            List<GattService> services = new List<GattService>();

            ((CBPeripheral)Device).DiscoverIncludedServices(new CBUUID[] { }, _service);

            foreach(var includedService in _service.IncludedServices)
            {
                services.Add(new GattService(Device, includedService));
            }

            return services;
        }
    }
}
