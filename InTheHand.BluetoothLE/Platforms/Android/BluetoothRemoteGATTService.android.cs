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

        Guid GetUuid()
        {
            return NativeService.Uuid.ToGuid();
        }

        bool GetIsPrimary()
        {
            return NativeService.Type == ABluetooth.GattServiceType.Primary;
        }

        Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            var nativeCharacteristic = NativeService.GetCharacteristic(characteristic.ToUuid());
            if (nativeCharacteristic is null)
                return Task.FromResult((GattCharacteristic)null);

            return Task.FromResult(new GattCharacteristic(this, nativeCharacteristic));
        }

        Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            foreach(var characteristic in NativeService.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, characteristic));
            }
            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }
    }
}
