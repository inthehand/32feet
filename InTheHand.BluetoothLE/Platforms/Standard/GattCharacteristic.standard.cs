//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private BluetoothUuid GetUuid()
        {
            return default;
        }

        private GattCharacteristicProperties GetProperties()
        {
            return 0;
        }

        private Task<GattDescriptor?> PlatformGetDescriptor(BluetoothUuid descriptor)
        {
            return Task.FromResult<GattDescriptor?>(null);
        }

        private Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            return Task.FromResult<IReadOnlyList<GattDescriptor>>(null);
        }

        private byte[] PlatformGetValue()
        {
            return null;
        }

        private Task<byte[]> PlatformReadValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        private Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            return Task.FromException(new PlatformNotSupportedException());
        }

        private void AddCharacteristicValueChanged()
        {
        }

        private void RemoveCharacteristicValueChanged()
        {
        }

        private Task PlatformStartNotifications()
        {
            return Task.FromException(new PlatformNotSupportedException());
        }

        private Task PlatformStopNotifications()
        {
            return Task.FromException(new PlatformNotSupportedException());
        }
    }
}