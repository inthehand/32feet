//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        private readonly ABluetooth.BluetoothGattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, ABluetooth.BluetoothGattDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        public static implicit operator ABluetooth.BluetoothGattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        BluetoothUuid GetUuid()
        {
            return _descriptor.Uuid;
        }

        byte[] PlatformGetValue()
        {
            return _descriptor.GetValue();
        }

        Task<byte[]> PlatformReadValue()
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            void handler(object s, DescriptorEventArgs e)
            {
                if (e.Descriptor == _descriptor)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        if (e.Status == ABluetooth.GattStatus.Success)
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
            bool read = ((ABluetooth.BluetoothGatt)Characteristic.Service.Device.Gatt).ReadDescriptor(_descriptor);
            return tcs.Task;
        }

        Task PlatformWriteValue(byte[] value)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, DescriptorEventArgs e)
            {
                if (e.Descriptor == _descriptor)
                {
                    Characteristic.Service.Device.Gatt.DescriptorWrite -= handler;

                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.SetResult(e.Status == ABluetooth.GattStatus.Success);
                    }
                }
            };

            Characteristic.Service.Device.Gatt.DescriptorWrite += handler;
            bool written = _descriptor.SetValue(value);
            written = ((ABluetooth.BluetoothGatt)Characteristic.Service.Device.Gatt).WriteDescriptor(_descriptor);
            if(written)
                return tcs.Task;

            return Task.FromException(new OperationCanceledException());
        }
    }
}
