//-----------------------------------------------------------------------
// <copyright file="GattService.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        private IGattService1 _service;

        internal GattService(BluetoothDevice device, IGattService1 service, BluetoothUuid uuid = default) : this(device)
        {
            _service = service;

            if(uuid != BluetoothUuid.Empty)
            {
                _uuid = uuid;
            }
        }

        internal async Task Init()
        {
            if (_uuid == BluetoothUuid.Empty)
            {
                _uuid = Guid.Parse(await _service.GetUUIDAsync());
            }

            _isPrimary = await _service.GetPrimaryAsync();
        }

        private BluetoothUuid _uuid;

        BluetoothUuid GetUuid()
        {
            return _uuid;
        }

        private bool _isPrimary;

        bool GetIsPrimary()
        {
            return _isPrimary;
        }

        async Task<GattCharacteristic> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            var linuxCharacteristic = await _service.GetCharacteristicAsync(characteristic.Value.ToString());

            if (linuxCharacteristic != null)
            {
                var gattCharacteristic = new GattCharacteristic(linuxCharacteristic, characteristic);
                await gattCharacteristic.Init();
                return gattCharacteristic;
            }

            return null;
        }

        async Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            var chars = await _service.GetCharacteristicsAsync();
            if(chars != null && chars.Count > 0)
            {
                foreach(var linuxCharacteristic in chars)
                {
                    var gattCharacteristic = new GattCharacteristic(linuxCharacteristic,
                        Guid.Parse(await linuxCharacteristic.GetUUIDAsync()));
                    await gattCharacteristic.Init();
                    characteristics.Add(gattCharacteristic);
                }
            }

            return characteristics.AsReadOnly();
        }

        private async Task<GattService> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            var paths = await _service.GetIncludesAsync();
            foreach(var path in paths)
            {
                var serv = Tmds.DBus.Connection.System.CreateProxy<IGattService1>(BluezConstants.DbusService, path);
                var uuid = await serv.GetUUIDAsync();
                if(uuid == service.Value.ToString())
                {
                    return new GattService(Device, serv, service);
                }
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            var includedServices = new List<GattService>();

            var paths = await _service.GetIncludesAsync();
            foreach (var path in paths)
            {
                var serv = Tmds.DBus.Connection.System.CreateProxy<IGattService1>(BluezConstants.DbusService, path);
                includedServices.Add(new GattService(Device, serv, Guid.Parse(await serv.GetUUIDAsync())));
            }

            return includedServices.AsReadOnly();
        }
    }
}