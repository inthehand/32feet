//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        private BluetoothGattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, BluetoothGattDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        Guid GetUuid()
        {
            return new Guid(_descriptor.Uuid.ToString());
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult(_descriptor.GetValue());
        }

        Task<byte[]> DoReadValue()
        {
            return Task.Run(() =>
            {
                bool read = Characteristic.Service.Device.Gatt.NativeGatt.ReadDescriptor(_descriptor);
                Characteristic.Service.Device.Gatt.WaitForDescriptorRead();

                return DoGetValue();
            });
        }

        Task DoWriteValue(byte[] value)
        {
            _descriptor.SetValue(value);
            bool written = Characteristic.Service.Device.Gatt.NativeGatt.WriteDescriptor(_descriptor);
            Characteristic.Service.Device.Gatt.WaitForDescriptorWrite();
            return Task.CompletedTask;
        }
    }
}