//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private Task<GattDescriptorsResult> GetDescriptorsAsyncImpl(BluetoothCacheMode cacheMode)
        {
            return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Unreachable, null));
        }

        private Task<GattDescriptorsResult> GetDescriptorsForUuidAsyncImpl(Guid descriptorUuid, BluetoothCacheMode cacheMode)
        {
            return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Unreachable, null));
        }

        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            throw new PlatformNotSupportedException();
        }

        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value, GattWriteOption writeOption)
        {
            throw new PlatformNotSupportedException();
        }

        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return GattCharacteristicProperties.None;
        }

        private GattDeviceService GetService()
        {
            return null;
        }

        private string GetUserDescription()
        {
            return string.Empty;
        }

        private Guid GetUuid()
        {
            return Guid.Empty;
        }

        private void ValueChangedAdd()
        {
        }

        private void ValueChangedRemove()
        {
        }
    }
}