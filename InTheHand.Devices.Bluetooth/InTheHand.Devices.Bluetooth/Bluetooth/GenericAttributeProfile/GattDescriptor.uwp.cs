//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor _descriptor;

        private GattDescriptor(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        public static implicit operator GattDescriptor(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor descriptor)
        {
            return new GattDescriptor(descriptor);
        }

        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            return await _descriptor.ReadValueAsync((Windows.Devices.Bluetooth.BluetoothCacheMode)((int)cacheMode));
        }

        private Guid GetUuid()
        {
            return _descriptor.Uuid;
        }
    }
}