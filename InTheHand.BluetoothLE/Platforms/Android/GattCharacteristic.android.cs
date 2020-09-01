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
            return Task.Run(() =>
            {
                bool read = Service.Device.Gatt.NativeGatt.ReadCharacteristic(_characteristic);
                Service.Device.Gatt.WaitForCharacteristicRead();

                return DoGetValue();
            });
        }

        Task DoWriteValue(byte[] value)
        {
            _characteristic.SetValue(value);
            bool written = Service.Device.Gatt.NativeGatt.WriteCharacteristic(_characteristic);
            return Task.Run(() =>
            {
                Service.Device.Gatt.WaitForCharacteristicWrite();
            });
        }

        void AddCharacteristicValueChanged()
        {
            Service.Device.Gatt.CharacteristicChanged += Gatt_CharacteristicChanged;
        }

        private void Gatt_CharacteristicChanged(object sender, BluetoothUuid e)
        {
            if(e == Uuid)
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

            var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
            await descriptor.WriteValueAsync(data);
            Service.Device.Gatt.NativeGatt.SetCharacteristicNotification(_characteristic, true);
        }

        private async Task DoStopNotifications()
        {
            Service.Device.Gatt.NativeGatt.SetCharacteristicNotification(_characteristic, false);
            var descriptor = await GetDescriptorAsync(GattDescriptorUuids.ClientCharacteristicConfiguration);
            await descriptor.WriteValueAsync(new byte[] { 0, 0 });
        }
    }
}
