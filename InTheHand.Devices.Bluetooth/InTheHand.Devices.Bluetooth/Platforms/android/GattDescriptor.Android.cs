//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Android.Bluetooth;
using System.Threading;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        private GattCharacteristic _characteristic;
        private BluetoothGattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, BluetoothGattDescriptor descriptor)
        {
            _characteristic = characteristic;
            _descriptor = descriptor;
            _characteristic.Service.Device.DescriptorRead += _device_DescriptorRead;
        }

        private void _device_DescriptorRead(object sender, BluetoothGattDescriptor e)
        {
            if (e == _descriptor)
                _readHandle.Set();
        }

        public static implicit operator BluetoothGattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        private EventWaitHandle _readHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            bool success = true;

            if (cacheMode == BluetoothCacheMode.Uncached)
            {
                success = _characteristic.Service.Device._bluetoothGatt.ReadDescriptor(_descriptor);
                _readHandle.WaitOne();
            }

            return new GattReadResult(success ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable, _descriptor.GetValue());
        }
        
        private Guid GetUuid()
        {
            return _descriptor.Uuid.ToGuid();
        }


    }
}