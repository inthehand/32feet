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

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        internal BluetoothRemoteGATTService(BluetoothDevice device, ABluetooth.BluetoothGattService service) : this(device)
        {
            if (service is null)
                throw new ArgumentNullException("service");

            NativeService = service;
        }

        internal ABluetooth.BluetoothGattService NativeService { get; }

        private Guid GetUuid()
        {
            return NativeService.Uuid.ToGuid();
        }

        private bool GetIsPrimary()
        {
            return NativeService.Type == ABluetooth.GattServiceType.Primary;
        }

        private Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            var nativeCharacteristic = NativeService.GetCharacteristic(characteristic.ToUuid());
            if (nativeCharacteristic is null)
                return Task.FromResult((GattCharacteristic)null);

            return Task.FromResult(new GattCharacteristic(this, nativeCharacteristic));
        }

        private Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            foreach(var characteristic in NativeService.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, characteristic));
            }
            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }

        private async Task<BluetoothRemoteGATTService> DoGetIncludedServiceAsync(Guid service)
        {
            foreach (var includedService in NativeService.IncludedServices)
            {
                if (includedService.Uuid.ToGuid() == service)
                    return new BluetoothRemoteGATTService(Device, includedService);
            }

            return null;
        }

        private async Task<IReadOnlyList<BluetoothRemoteGATTService>> DoGetIncludedServicesAsync()
        {
            List<BluetoothRemoteGATTService> services = new List<BluetoothRemoteGATTService>();

            foreach(var includedService in NativeService.IncludedServices)
            {
                services.Add(new BluetoothRemoteGATTService(Device, includedService));
            }

            return services;
        }
    }
}
