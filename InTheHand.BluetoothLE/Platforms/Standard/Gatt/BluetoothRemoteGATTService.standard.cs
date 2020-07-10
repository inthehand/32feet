//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTService.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        Guid GetUuid()
        {
            return Guid.Empty;
        }

        bool GetIsPrimary()
        {
            return true;
        }

        Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            return Task.FromResult((GattCharacteristic)null);
        }

        Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }

        private async Task<BluetoothRemoteGATTService> DoGetIncludedServiceAsync(Guid service)
        {
            return null;
        }

        private async Task<IReadOnlyList<BluetoothRemoteGATTService>> DoGetIncludedServicesAsync()
        {
            return null;
        }
    }
}
