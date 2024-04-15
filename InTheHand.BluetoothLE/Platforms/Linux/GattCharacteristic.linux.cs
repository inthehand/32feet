//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using Linux.Bluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private readonly IGattCharacteristic1 _characteristic;
        private IDisposable? _eventHandler;

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

        Task<GattDescriptor?> PlatformGetDescriptor(BluetoothUuid descriptor)
        {
            return Task.FromResult((GattDescriptor)null);
        }

        Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            return Task.FromResult((IReadOnlyList<GattDescriptor>)null);
        }

        private byte[] _value;
        private byte[] PlatformGetValue()
        {
            return _value;
        }

        private async Task<byte[]> PlatformReadValue()
        {
            var newValue = await _characteristic.ReadValueAsync(new Dictionary<string, object>());
            if(newValue != null)
            {
                _value = newValue;
            }

            return newValue;
        }

        private async Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            var options = new Dictionary<string, object>
            {
                { "type", requireResponse ? "request" : "command" }
            };

            await _characteristic.WriteValueAsync(value, options);
            _value = value;
        }

        private void AddCharacteristicValueChanged()
        {
            Task.Run(async () =>
            {
                _eventHandler ??= await _characteristic.WatchPropertiesAsync(PropertyChangedHandler);
            });
        }

        private void PropertyChangedHandler(Tmds.DBus.PropertyChanges changes)
        {
            foreach (var change in changes.Changed)
            {
                if (change.Key == "Value")
                {
                    OnCharacteristicValueChanged(new GattCharacteristicValueChangedEventArgs((byte[]?)change.Value));
                }
            }
        }

        private void RemoveCharacteristicValueChanged()
        {
            if (_eventHandler != null)
            {
                _eventHandler.Dispose();
                _eventHandler = null;
            }
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