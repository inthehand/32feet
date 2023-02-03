//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using Android.OS;
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
#if NET7_0_OR_GREATER
                            tcs.SetResult(e.Value);
#else
                            tcs.SetResult(_descriptor.GetValue());
#endif
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

            bool written = false;
#if NET7_0_OR_GREATER
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                int result = ((ABluetooth.BluetoothGatt)Characteristic.Service.Device.Gatt).WriteDescriptor(_descriptor, value);
                written = result == (int)CurrentBluetoothStatusCodes.Success;
            }
            else
            {
#endif
                written = _descriptor.SetValue(value);
                if(written)
                    written = ((ABluetooth.BluetoothGatt)Characteristic.Service.Device.Gatt).WriteDescriptor(_descriptor);
#if NET7_0_OR_GREATER
        } 
#endif
            
            if (written)
                    return tcs.Task;

            return Task.FromException(new System.OperationCanceledException());
        }
    }
}
