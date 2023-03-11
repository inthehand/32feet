//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Threading.Tasks;
using Linux.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private IGattCharacteristic1 _characteristic;

        internal GattCharacteristic(IGattCharacteristic1 characteristic)
        {
            _characteristic = characteristic;
        }

        BluetoothUuid GetUuid()
        {
            return BluetoothUuid.FromGuid(Guid.Parse(AsyncHelpers.RunSync(() => { return _characteristic.GetUUIDAsync(); })));
        }

        GattCharacteristicProperties GetProperties()
        {
            return 0;
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
            return AsyncHelpers.RunSync(() => { return _characteristic.GetValueAsync(); });
        }

        Task<byte[]> PlatformReadValue()
        {
            //TODO understand options
            return _characteristic.ReadValueAsync(null);
        }

        async Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            //TODO understand options
            await _characteristic.WriteValueAsync(value, null);
        }

        void AddCharacteristicValueChanged()
        {
        }

        void RemoveCharacteristicValueChanged()
        {
        }

        private Task PlatformStartNotifications()
        {
            return _characteristic.StartNotifyAsync();
        }

        private Task PlatformStopNotifications()
        {
            return _characteristic.StopNotifyAsync();
        }
    }
}
