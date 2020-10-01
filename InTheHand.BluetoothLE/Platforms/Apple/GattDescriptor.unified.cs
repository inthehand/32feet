//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        private CBDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, CBDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        public static implicit operator CBDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        BluetoothUuid GetUuid()
        {
            return _descriptor.UUID;
        }

        byte[] PlatformGetValue()
        {
            return ((NSData)_descriptor.Value).ToArray();
        }

        Task<byte[]> PlatformReadValue()
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();
            CBPeripheral peripheral = Characteristic.Service.Device;

            void handler(object s, CBDescriptorEventArgs e)
            {
                if (e.Descriptor == _descriptor)
                {
                    peripheral.UpdatedValue -= handler;

                    if (!tcs.Task.IsCompleted)
                    {
                        tcs.SetResult(((NSData)e.Descriptor.Value).ToArray());
                    }
                }
            };

            peripheral.UpdatedValue += handler;

            ((CBPeripheral)Characteristic.Service.Device).ReadValue(_descriptor);
            return tcs.Task;
        }

        Task PlatformWriteValue(byte[] value)
        {
            TaskCompletionSource<bool> tcs = null;
            CBPeripheral peripheral = Characteristic.Service.Device;

                tcs = new TaskCompletionSource<bool>();

                void handler(object s, CBDescriptorEventArgs e)
                {
                    if (e.Descriptor == _descriptor)
                    {
                        peripheral.WroteDescriptorValue -= handler;

                        if (!tcs.Task.IsCompleted)
                        {
                            tcs.SetResult(e.Error == null);
                        }
                    }
                };

                peripheral.WroteDescriptorValue += handler;

            peripheral.WriteValue(NSData.FromArray(value), _descriptor);
            return tcs.Task;
        }
    }
}