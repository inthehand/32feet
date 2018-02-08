//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTheHand.Foundation;
using System.Diagnostics;
using CoreBluetooth;
using Foundation;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private GattDeviceService _service;
        private CBCharacteristic _characteristic;
        
        internal GattCharacteristic(GattDeviceService service, CBCharacteristic characteristic)
        {
            _service = service;
            _characteristic = characteristic;
            _service.Device.Peripheral.DiscoveredDescriptor += _peripheral_DiscoveredDescriptor;
        }

        ~GattCharacteristic()
        {
            _service.Device.Peripheral.DiscoveredDescriptor -= _peripheral_DiscoveredDescriptor;
        }

        private void _peripheral_DiscoveredDescriptor(object sender, CBCharacteristicEventArgs e)
        {
            if(e.Characteristic == _characteristic)
            {
                Debug.WriteLine(DateTimeOffset.Now.ToString() + " DiscoveredDescriptor");
            }
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        private Task<GattDescriptorsResult> GetDescriptorsAsyncImpl(BluetoothCacheMode cacheMode)
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            try
            {
                if(cacheMode == BluetoothCacheMode.Uncached)
                {
                    _service.Device.Peripheral.DiscoverDescriptors(_characteristic);
                }

                foreach (CBDescriptor d in _characteristic.Descriptors)
                {
                    descriptors.Add(d);
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
                if (cacheMode == BluetoothCacheMode.Uncached)
                {
                    _service.Device.Peripheral.DiscoverDescriptors(_characteristic);
                }

                foreach (CBDescriptor descriptor in _characteristic.Descriptors)
                {
                    if (descriptor.UUID.ToGuid() == descriptorUuid)
                    {
                        descriptors.Add(descriptor);
                    }
                }

                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Success, descriptors.AsReadOnly()));
            }
            catch
            {
                return Task.FromResult(new GattDescriptorsResult(GattCommunicationStatus.Unreachable, null));
            }
        }
                
        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            try
            {
                return new GattReadResult(GattCommunicationStatus.Success, _characteristic.Value.ToArray());
            }
            catch
            {
                return new GattReadResult(GattCommunicationStatus.Unreachable, null);
            }
        }

        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value, GattWriteOption writeOption)
        {
            try
            {
                _characteristic.Value = NSData.FromArray(value);
                _characteristic.Service.Peripheral.WriteValue(NSData.FromArray(value), _characteristic, writeOption == GattWriteOption.WriteWithResponse ? CBCharacteristicWriteType.WithResponse : CBCharacteristicWriteType.WithoutResponse);
                return GattCommunicationStatus.Success;
            }
            catch
            {
                return GattCommunicationStatus.Unreachable;
            }
        }

        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return _characteristic.Properties.ToGattCharacteristicProperties();
        }

        private GattDeviceService GetService()
        {
            return _service;
        }

        private string GetUserDescription()
        {
            foreach (CBDescriptor desc in _characteristic.Descriptors)
            {
                if (desc.UUID.ToGuid() == GattDescriptorUuids.CharacteristicUserDescription)
                {
                    return desc.Value.ToString();
                }
            }

            return string.Empty;
        }

        private Guid GetUuid()
        {
            return _characteristic.UUID.ToGuid();
        }

        private void ValueChangedAdd()
        {
            _service.Device.Peripheral.SetNotifyValue(true, _characteristic);
            _service.Device.Peripheral.UpdatedCharacterteristicValue += _peripheral_UpdatedCharacterteristicValue;
        }

        private void ValueChangedRemove()
        {
            _service.Device.Peripheral.UpdatedCharacterteristicValue -= _peripheral_UpdatedCharacterteristicValue;
            _service.Device.Peripheral.SetNotifyValue(false, _characteristic);
        }
       

        private void _peripheral_UpdatedCharacterteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            valueChanged?.Invoke(this, new GattValueChangedEventArgs(e.Characteristic.Value.ToArray(), DateTimeOffset.Now));
        }
    }
}