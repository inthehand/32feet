//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTService.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-26 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;
using System.Linq;

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

        private Task<GattCharacteristic?> PlatformGetCharacteristic(BluetoothUuid characteristic)
        {
            var nativeCharacteristic = NativeService.GetCharacteristic(characteristic);
            if (nativeCharacteristic is null)
                return Task.FromResult((GattCharacteristic?)null);

            return Task.FromResult((GattCharacteristic?)new GattCharacteristic(this, nativeCharacteristic));
        }

        private Task<IReadOnlyList<GattCharacteristic>> PlatformGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = (from characteristic in NativeService.Characteristics
                                                        select new GattCharacteristic(this, characteristic)).ToList();
            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }

        private async Task<GattService?> PlatformGetIncludedServiceAsync(BluetoothUuid service)
        {
            foreach (var includedService in from includedService in NativeService.IncludedServices
                                            where includedService.Uuid == service
                                            select includedService)
            {
                return new GattService(Device, includedService);
            }

            return null;
        }

        private async Task<IReadOnlyList<GattService>> PlatformGetIncludedServicesAsync()
        {
            return (from includedService in NativeService.IncludedServices
                    select new GattService(Device, includedService)).ToList();
        }
    }
}