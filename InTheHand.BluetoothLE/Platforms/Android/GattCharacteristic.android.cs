//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using ABluetooth = Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.OS;
using Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private readonly ABluetooth.BluetoothGattCharacteristic _characteristic;

        internal GattCharacteristic(GattService service, ABluetooth.BluetoothGattCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator ABluetooth.BluetoothGattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        BluetoothUuid GetUuid()
        {
            return _characteristic.Uuid;
        }

        GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)(int)_characteristic.Properties;
        }

        Task<GattDescriptor> PlatformGetDescriptor(BluetoothUuid descriptor)
        {
            var gattDescriptor = _characteristic.GetDescriptor(descriptor);
            if (gattDescriptor is null)
                return Task.FromResult<GattDescriptor>(null);

            return Task.FromResult(new GattDescriptor(this, gattDescriptor));
        }

        async Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            foreach (var descriptor in _characteristic.Descriptors)
            {
                descriptors.Add(new GattDescriptor(this, descriptor));
            }

            return descriptors;
        }

        byte[] PlatformGetValue()
        {
            return _characteristic.GetValue();
        }

        Task<byte[]> PlatformReadValue()
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            void handler(object s, CharacteristicEventArgs e)
            {
                if (e.Characteristic == _characteristic)
                {
                    Service.Device.Gatt.CharacteristicRead -= handler;
                    if (!tcs.Task.IsCompleted)
                    {
                        if (e.Status == ABluetooth.GattStatus.Success)
                        {
                            tcs.SetResult(e.Value);
                        }
                        else
                        {
                            tcs.SetResult(null);
                        }

                    }
                }
            };

            Service.Device.Gatt.CharacteristicRead += handler;
            bool read = ((ABluetooth.BluetoothGatt)Service.Device.Gatt).ReadCharacteristic(_characteristic);
            return tcs.Task;
        }

        Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            TaskCompletionSource<bool> tcs = null;

            if (requireResponse)
            {
                tcs = new TaskCompletionSource<bool>();

                void handler(object s, CharacteristicEventArgs e)
                {
                    if (e.Characteristic == _characteristic)
                    {
                        Service.Device.Gatt.CharacteristicWrite -= handler;

                        if (!tcs.Task.IsCompleted)
                        {
                            tcs.SetResult(e.Status == ABluetooth.GattStatus.Success);
                        }
                    }
                };

                Service.Device.Gatt.CharacteristicWrite += handler;
            }

            bool written = false;
#if NET7_0_OR_GREATER
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                int result = ((ABluetooth.BluetoothGatt)Service.Device.Gatt).WriteCharacteristic(_characteristic, value, requireResponse ? (int)ABluetooth.GattWriteType.Default : (int)ABluetooth.GattWriteType.NoResponse);
                written = result == (int)CurrentBluetoothStatusCodes.Success;
            }
            else
            {
#endif
                written = _characteristic.SetValue(value);
                _characteristic.WriteType = requireResponse ? ABluetooth.GattWriteType.Default : ABluetooth.GattWriteType.NoResponse;
                written = ((ABluetooth.BluetoothGatt)Service.Device.Gatt).WriteCharacteristic(_characteristic);
#if NET7_0_OR_GREATER
            } 
#endif
            if (written && requireResponse)
                return tcs.Task;

            return Task.CompletedTask;
        }

        void AddCharacteristicValueChanged()
        {
            Service.Device.Gatt.CharacteristicChanged += Gatt_CharacteristicChanged;
        }

        private void Gatt_CharacteristicChanged(object sender, CharacteristicEventArgs e)
        {
            if (e.Characteristic == _characteristic)
                OnCharacteristicValueChanged(new GattCharacteristicValueChangedEventArgs(e.Characteristic.GetValue()));
        }

        void RemoveCharacteristicValueChanged()
        {
            Service.Device.Gatt.CharacteristicChanged -= Gatt_CharacteristicChanged;
        }

        private async Task PlatformStartNotifications()
        {
            byte[] data;

            if (_characteristic.Properties.HasFlag(ABluetooth.GattProperty.Notify))
                data = ABluetooth.BluetoothGattDescriptor.EnableNotificationValue.ToArray();
            else if (_characteristic.Properties.HasFlag(ABluetooth.GattProperty.Indicate))
                data = ABluetooth.BluetoothGattDescriptor.EnableIndicationValue.ToArray();
            else
                return;

            ((ABluetooth.BluetoothGatt)Service.Device.Gatt).SetCharacteristicNotification(_characteristic, true);
            var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
            await descriptor.WriteValueAsync(data);
        }

        private async Task PlatformStopNotifications()
        {
            ((ABluetooth.BluetoothGatt)Service.Device.Gatt).SetCharacteristicNotification(_characteristic, false);
            if (Service.Device.Gatt.IsConnected)
            {
                var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
                await descriptor.WriteValueAsync(new byte[] { 0, 0 });
            }
        }
    }
}
