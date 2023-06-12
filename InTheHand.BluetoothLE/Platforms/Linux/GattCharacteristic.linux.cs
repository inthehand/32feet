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
        private Linux.Bluetooth.GattCharacteristic _characteristic;

        internal GattCharacteristic(Linux.Bluetooth.GattCharacteristic characteristic, BluetoothUuid uuid)
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

        private byte[] _value;
        byte[] PlatformGetValue()
        {
            return _value;
        }

        async Task<byte[]> PlatformReadValue()
        {
            var newValue = await _characteristic.ReadValueAsync(null);
            if(newValue != null)
            {
                _value = newValue;
            }

            return newValue;
        }

        async Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            Dictionary<string,object> options = new Dictionary<string,object>();
            options.Add("type", requireResponse == true ? "request" : "command");
            await _characteristic.WriteValueAsync(value, options);
            _value = value;
        }

        void AddCharacteristicValueChanged()
        {
            _characteristic.Value += _characteristic_Value;
        }

        private Task _characteristic_Value(Linux.Bluetooth.GattCharacteristic sender, GattCharacteristicValueEventArgs eventArgs)
        {
            OnCharacteristicValueChanged(new GattCharacteristicValueChangedEventArgs(eventArgs.Value));
            return Task.CompletedTask;
        }

        void RemoveCharacteristicValueChanged()
        {
            _characteristic.Value -= _characteristic_Value;
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
