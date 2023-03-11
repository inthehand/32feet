//-----------------------------------------------------------------------
// <copyright file="GattService.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Threading.Tasks;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        private IGattService1 _service;

        internal GattService(BluetoothDevice device, IGattService1 service) : this(device)
        {
            _service = service;
        }

        BluetoothUuid GetUuid()
        {
            return BluetoothUuid.FromGuid(Guid.Parse(AsyncHelpers.RunSync(() => { return _service.GetUUIDAsync(); })));
        }

        bool GetIsPrimary()
        {
            return AsyncHelpers.RunSync(() => { return _service.GetPrimaryAsync(); });
        }

        async Task<GattCharacteristic> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            var linuxCharacteristic = await _service.GetCharacteristicAsync(characteristic.Value.ToString());
            return linuxCharacteristic == null ? null : new GattCharacteristic(linuxCharacteristic);
        }

        async Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            var chars = await _service.GetCharacteristicsAsync();
            if(chars != null && chars.Count > 0)
            {
                foreach(var linuxCharacteristic in chars)
                {
                    characteristics.Add(new GattCharacteristic(linuxCharacteristic));
                }
            }

            return characteristics.AsReadOnly();
        }

        private async Task<GattService> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            return null;
        }
    }
}
