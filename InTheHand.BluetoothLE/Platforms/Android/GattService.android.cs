//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTService.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class GattService
    {
        internal GattService(BluetoothDevice device, ABluetooth.BluetoothGattService service) : this(device)
        {
            if (service is null)
                throw new ArgumentNullException("service");

            NativeService = service;
        }

        public static implicit operator ABluetooth.BluetoothGattService(GattService service)
        {
            return service.NativeService;
        }

        internal ABluetooth.BluetoothGattService NativeService { get; }

        private BluetoothUuid GetUuid()
        {
            return NativeService.Uuid;
        }

        private bool GetIsPrimary()
        {
            return NativeService.Type == ABluetooth.GattServiceType.Primary;
        }

        private Task<GattCharacteristic> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            var nativeCharacteristic = NativeService.GetCharacteristic(characteristic);
            if (nativeCharacteristic is null)
                return Task.FromResult((GattCharacteristic)null);

            return Task.FromResult(new GattCharacteristic(this, nativeCharacteristic));
        }

        private Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            foreach(var characteristic in NativeService.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, characteristic));
            }
            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }

        private async Task<GattService> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            foreach (var includedService in NativeService.IncludedServices)
            {
                if (includedService.Uuid == service)
                    return new GattService(Device, includedService);
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            List<GattService> services = new List<GattService>();

            foreach(var includedService in NativeService.IncludedServices)
            {
                services.Add(new GattService(Device, includedService));
            }

            return services;
        }
    }
}
