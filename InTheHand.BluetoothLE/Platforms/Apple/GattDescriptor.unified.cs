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

        Task<byte[]> PlatformGetValue()
        {
            return Task.FromResult(((NSData)_descriptor.Value).ToArray());
        }

        Task<byte[]> PlatformReadValue()
        {
            ((CBPeripheral)Characteristic.Service.Device).ReadValue(_descriptor);
            return Task.FromResult(((NSData)_descriptor.Value).ToArray());
        }

        Task PlatformWriteValue(byte[] value)
        {
            ((CBPeripheral)Characteristic.Service.Device).WriteValue(NSData.FromArray(value), _descriptor);
            return Task.CompletedTask;
        }
    }
}