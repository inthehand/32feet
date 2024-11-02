//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using Linux.Bluetooth;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private readonly IGattCharacteristic1 _characteristic;
        private GattCharacteristicProperties _characteristicProperties;
        private IDisposable? _eventHandler;

        internal GattCharacteristic(IGattCharacteristic1 characteristic, BluetoothUuid uuid)
        {
            _characteristic = characteristic;
            _uuid = uuid;
        }

        internal async Task Init()
        {
            await UpdateCharacteristicProperties();
        }

        private async Task UpdateCharacteristicProperties()
        {
            string[] flags = await GattCharacteristic1Extensions.GetFlagsAsync(_characteristic);

            var characteristicProperties = GattCharacteristicProperties.None;
            foreach (var flag in flags)
            {
                switch (flag)
                {
                    case "broadcast":
                        characteristicProperties |= GattCharacteristicProperties.Broadcast;
                        break;
                    
                    case "read":
                        characteristicProperties |= GattCharacteristicProperties.Read;
                        break;
                    
                    case "write-without-response":
                        characteristicProperties |= GattCharacteristicProperties.WriteWithoutResponse;
                        break;
                    
                    case "write":
                        characteristicProperties |= GattCharacteristicProperties.Write;
                        break;
                    
                    case "notify":
                        characteristicProperties |= GattCharacteristicProperties.Notify;
                        break;
                    
                    case "indicate":
                        characteristicProperties |= GattCharacteristicProperties.Indicate;
                        break;
                    
                    case "authenticated-signed-writes":
                        characteristicProperties |= GattCharacteristicProperties.AuthenticatedSignedWrites;
                        break;
                    
                    case "extended-properties":
                        characteristicProperties |= GattCharacteristicProperties.ExtendedProperties;
                        break;
                    
                    case "reliable-write":
                        characteristicProperties |= GattCharacteristicProperties.ReliableWrites;
                        break;
                    
                    case "writable-auxiliaries":
                        characteristicProperties |= GattCharacteristicProperties.WriteableAuxiliaries;
                        break;
                    
                    /* Not handled values:
                       "encrypt-read"
                       "encrypt-write"
                       "encrypt-notify" (Server only)
                       "encrypt-indicate" (Server only)
                       "encrypt-authenticated-read"
                       "encrypt-authenticated-write"
                       "encrypt-authenticated-notify" (Server only)
                       "encrypt-authenticated-indicate" (Server only)
                       "secure-read" (Server only)
                       "secure-write" (Server only)
                       "secure-notify" (Server only)
                       "secure-indicate" (Server only)
                       "authorize"
                     */
                }
            }

            _characteristicProperties = characteristicProperties;
        }

        private BluetoothUuid _uuid;
        BluetoothUuid GetUuid()
        {
            return _uuid;
        }

        GattCharacteristicProperties GetProperties()
        {
            return _characteristicProperties;
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