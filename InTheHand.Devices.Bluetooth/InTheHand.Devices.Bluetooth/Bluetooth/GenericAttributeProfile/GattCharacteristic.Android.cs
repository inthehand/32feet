//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private GattDeviceService _service;
        private BluetoothGattCharacteristic _characteristic;

        internal GattCharacteristic(GattDeviceService service, BluetoothGattCharacteristic characteristic)
        {
            _service = service;
            _characteristic = characteristic;
            _service.Device.CharacteristicRead += _device_CharacteristicRead;
        }

        private void _device_CharacteristicRead(object sender, BluetoothGattCharacteristic e)
        {
            if(e == _characteristic)
            {
                _readHandle.Set();
            }
        }


        public static implicit operator BluetoothGattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        private Task<GattDescriptorsResult> GetDescriptorsAsyncImpl(BluetoothCacheMode cacheMode)
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            try
            {
                foreach (BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
                {
                    descriptors.Add(new GattDescriptor(this, descriptor));
                }

                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Success, descriptors.AsReadOnly()));
            }
            catch
            {
                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Unreachable, null));
            }
        }

        private Task<GattDescriptorsResult> GetDescriptorsForUuidAsyncImpl(Guid descriptorUuid, BluetoothCacheMode cacheMode)
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();
           
            try
            {
                foreach (BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
                {
                    if (descriptor.Uuid.ToGuid() == descriptorUuid)
                    {
                        descriptors.Add(new GattDescriptor(this, descriptor));
                    }
                }

                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Success, descriptors.AsReadOnly()));
            }
            catch
            {
                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Unreachable, null));
            }
        }

        private EventWaitHandle _readHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            bool success = true;
            if(cacheMode == BluetoothCacheMode.Uncached)
            {
                success = _service.Device._bluetoothGatt.ReadCharacteristic(_characteristic);
                _readHandle.WaitOne();
            }

            return new GattReadResult(success ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable, _characteristic.GetValue()); 
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value, GattWriteOption writeOption)
        {
            bool success = _characteristic.SetValue(value);
            if (_service.Device._bluetoothGatt.WriteCharacteristic(_characteristic))
            {
                return GattCommunicationStatus.Success;
            }
            
            return GattCommunicationStatus.Unreachable;
        }
        
        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return (GattCharacteristicProperties)((int)_characteristic.Properties);
        }

        private GattDeviceService GetService()
        {
            return _service;
        }

        private string GetUserDescription()
        {
            foreach (BluetoothGattDescriptor descriptor in _characteristic.Descriptors)
            {
                if (descriptor.Uuid.ToGuid() == GattDescriptorUuids.CharacteristicUserDescription)
                {
                    return global::System.Text.Encoding.Unicode.GetString(descriptor.GetValue());
                }
            }

            return string.Empty;
        }

        private Guid GetUuid()
        {
            return _characteristic.Uuid.ToGuid();
        }

        private void ValueChangedAdd()
        {
            if(_service.Device._bluetoothGatt.SetCharacteristicNotification(_characteristic, true))
            {
                _service.Device._gattCallback.CharacteristicChanged += _gattCallback_CharacteristicChanged;
            }
        }

        private void ValueChangedRemove()
        {
            if (_service.Device._bluetoothGatt.SetCharacteristicNotification(_characteristic, false))
            {
                _service.Device._gattCallback.CharacteristicChanged -= _gattCallback_CharacteristicChanged;
            }
        }

        private void _gattCallback_CharacteristicChanged(object sender, BluetoothGattCharacteristic e)
        {
            if (e == _characteristic)
            {
                valueChanged?.Invoke(this, new GattValueChangedEventArgs(_characteristic.GetValue(), DateTimeOffset.Now));
            }
        }
    }
}