//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        private readonly BluetoothGattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, BluetoothGattDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        public static implicit operator BluetoothGattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        BluetoothUuid GetUuid()
        {
            return _descriptor.Uuid;
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult(_descriptor.GetValue());
        }

        Task<byte[]> DoReadValue()
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            void handler(object s, DescriptorEventArgs e)
            {
                if (e.Descriptor == _descriptor)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        if (e.Status == GattStatus.Success)
                        {
                            tcs.SetResult(_descriptor.GetValue());
                        }
                        else
                        {
                            tcs.SetResult(null);
                        }
                    }

                    Characteristic.Service.Device.Gatt.DescriptorRead -= handler;
                }
            }

            Characteristic.Service.Device.Gatt.DescriptorRead += handler;
            bool read = Characteristic.Service.Device.Gatt.NativeGatt.ReadDescriptor(_descriptor);
            return tcs.Task;
        }

        Task DoWriteValue(byte[] value)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, DescriptorEventArgs e)
            {
                if (e.Descriptor == _descriptor)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.SetResult(e.Status == GattStatus.Success);
                    }

                    Characteristic.Service.Device.Gatt.DescriptorWrite -= handler;
                }
            };

            Characteristic.Service.Device.Gatt.DescriptorWrite += handler;
            bool written = _descriptor.SetValue(value);
            written = Characteristic.Service.Device.Gatt.NativeGatt.WriteDescriptor(_descriptor);
            return tcs.Task;
        }
    }
}
