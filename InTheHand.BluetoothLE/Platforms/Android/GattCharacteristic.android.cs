//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private readonly BluetoothGattCharacteristic _characteristic;

        internal GattCharacteristic(GattService service, BluetoothGattCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator BluetoothGattCharacteristic(GattCharacteristic characteristic)
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

        string GetUserDescription()
        {
            return GetManualUserDescription();
        }

        Task<GattDescriptor> DoGetDescriptor(BluetoothUuid descriptor)
        {
            var gattDescriptor = _characteristic.GetDescriptor(descriptor);
            if (gattDescriptor is null)
                return Task.FromResult<GattDescriptor>(null);

            return Task.FromResult(new GattDescriptor(this, gattDescriptor));
        }

        async Task<IReadOnlyList<GattDescriptor>> DoGetDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            foreach(var descriptor in _characteristic.Descriptors)
            {
                descriptors.Add(new GattDescriptor(this, descriptor));
            }

            return descriptors;
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult(_characteristic.GetValue());
        }

        Task<byte[]> DoReadValue()
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            void handler(object s, CharacteristicEventArgs e)
            {
                if (e.Characteristic == _characteristic)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.SetResult(_characteristic.GetValue());
                    }

                    Service.Device.Gatt.CharacteristicRead -= handler;
                }
            };

            Service.Device.Gatt.CharacteristicRead += handler;
            bool read = Service.Device.Gatt.NativeGatt.ReadCharacteristic(_characteristic);
            return tcs.Task;
        }

        Task DoWriteValue(byte[] value)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, CharacteristicEventArgs e)
            {
                if (e.Characteristic == _characteristic)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.SetResult(e.Status == GattStatus.Success);
                    }

                    Service.Device.Gatt.CharacteristicWrite -= handler;
                }
            };

            Service.Device.Gatt.CharacteristicWrite += handler;
            bool written = _characteristic.SetValue(value);
            written = Service.Device.Gatt.NativeGatt.WriteCharacteristic(_characteristic);
            return tcs.Task;
        }

        void AddCharacteristicValueChanged()
        {
            Service.Device.Gatt.CharacteristicChanged += Gatt_CharacteristicChanged;
        }

        private void Gatt_CharacteristicChanged(object sender, CharacteristicEventArgs e)
        {
            if(e.Characteristic == _characteristic)
                characteristicValueChanged?.Invoke(this, EventArgs.Empty);
        }

        void RemoveCharacteristicValueChanged()
        {
            Service.Device.Gatt.CharacteristicChanged -= Gatt_CharacteristicChanged;
        }

        private async Task DoStartNotifications()
        {
            byte[] data;

            if (_characteristic.Properties.HasFlag(GattProperty.Notify))
                data = BluetoothGattDescriptor.EnableNotificationValue.ToArray();
            else if (_characteristic.Properties.HasFlag(GattProperty.Indicate))
                data = BluetoothGattDescriptor.EnableIndicationValue.ToArray();
            else
                return;

            Service.Device.Gatt.NativeGatt.SetCharacteristicNotification(_characteristic, true);
            var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
            await descriptor.WriteValueAsync(data);
        }

        private async Task DoStopNotifications()
        {
            Service.Device.Gatt.NativeGatt.SetCharacteristicNotification(_characteristic, false);
            var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
            await descriptor.WriteValueAsync(new byte[] { 0, 0 });
        }
    }
}
