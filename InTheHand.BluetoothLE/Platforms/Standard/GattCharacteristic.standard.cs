//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        BluetoothUuid GetUuid()
        {
            return default;
        }

        GattCharacteristicProperties GetProperties()
        {
            return 0;
        }

        string GetUserDescription()
        {
            return string.Empty;
        }

        Task<GattDescriptor> PlatformGetDescriptor(BluetoothUuid descriptor)
        {
            return Task.FromResult((GattDescriptor)null);
        }

        Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            return Task.FromResult((IReadOnlyList<GattDescriptor>)null);
        }

        byte[] PlatformGetValue()
        {
            return null;
        }

        Task<byte[]> PlatformReadValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            return Task.FromException(new PlatformNotSupportedException());
        }

        void AddCharacteristicValueChanged()
        {
        }

        void RemoveCharacteristicValueChanged()
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
