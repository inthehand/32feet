//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

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

        internal GattCharacteristic(IGattCharacteristic1 characteristic, BluetoothUuid uuid)
        {
            _characteristic = characteristic;
            _uuid = uuid;
        }

        private BluetoothUuid _uuid;
        BluetoothUuid GetUuid()
        {
            return _uuid;
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
            return default;
        }

        Task<byte[]> PlatformReadValue()
        {
            //TODO understand options
            return _characteristic.ReadValueAsync(null);// new Dictionary<string, object> { });
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
