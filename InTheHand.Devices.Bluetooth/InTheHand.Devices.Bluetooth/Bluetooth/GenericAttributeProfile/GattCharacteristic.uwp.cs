//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic _characteristic;

        private GattCharacteristic(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        public static implicit operator GattCharacteristic(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic)
        {
            return new GattCharacteristic(characteristic);
        }

        private void GetAllDescriptors(List<GattDescriptor> descriptors)
        {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            foreach (GattDescriptor d in _characteristic.GetAllDescriptors())
            {
                descriptors.Add(d);
            }
#elif WINDOWS_APP || WINDOWS_PHONE_81
            // TODO: GetAll is missing from SL8.1 so test this workaround
            foreach (GattDescriptor d in _characteristic.GetDescriptors(Guid.Empty))
            {
                descriptors.Add(d);
            }
#endif
        }

        private Task<GattDescriptorsResult> GetDescriptorsAsyncImpl(BluetoothCacheMode cacheMode)
        {
            IReadOnlyList<GattDescriptor> descriptors = null;
            // Update for Creators Update
            var result = _characteristic.GetAllDescriptors();
            if (result != null)
            {
                List<GattDescriptor> found = new List<GattDescriptor>();
                foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor d in result)
                {
                    found.Add(d);
                }

                descriptors = found.AsReadOnly();
            }

            return Task.FromResult(new GattDescriptorsResult(descriptors));
        }

        private Task<GattDescriptorsResult> GetDescriptorsForUuidAsyncImpl(Guid descriptorUuid, BluetoothCacheMode cacheMode)
        {
            IReadOnlyList<GattDescriptor> descriptors = null;
            // Update for Creators Update
            var result = _characteristic.GetDescriptors(descriptorUuid);
            if (result != null)
            {
                List<GattDescriptor> found = new List<GattDescriptor>();
                foreach(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDescriptor d in result)
                {
                    found.Add(d);
                }

                descriptors = found.AsReadOnly();
            }

            return Task.FromResult(new GattDescriptorsResult(descriptors));
        }


        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            return await _characteristic.ReadValueAsync((Windows.Devices.Bluetooth.BluetoothCacheMode)((int)cacheMode)).AsTask();
        }

        private async Task<GattCommunicationStatus> DoWriteValueAsync(byte[] value, GattWriteOption writeOption)
        {
            return await _characteristic.WriteValueAsync(value.AsBuffer(), (Windows.Devices.Bluetooth.GenericAttributeProfile.GattWriteOption)writeOption).AsTask() == Windows.Devices.Bluetooth.GenericAttributeProfile.GattCommunicationStatus.Success ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable;
        }
        
        private GattCharacteristicProperties GetCharacteristicProperties()
        {
            return (GattCharacteristicProperties)((uint)_characteristic.CharacteristicProperties);
        }

        private string GetUserDescription()
        {
            return _characteristic.UserDescription;
        }

        private GattDeviceService GetService()
        {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            return _characteristic.Service;
#else
            return null;
#endif
        }

        private Guid GetUuid()
        {
            return _characteristic.Uuid;
        }

        private void ValueChangedAdd()
        {
            _characteristic.ValueChanged += _characteristic_ValueChanged;
        }

        private void ValueChangedRemove()
        {
            _characteristic.ValueChanged -= _characteristic_ValueChanged;
        }

        
        private void _characteristic_ValueChanged(Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic sender, Windows.Devices.Bluetooth.GenericAttributeProfile.GattValueChangedEventArgs args)
        {
            valueChanged?.Invoke(this, args);
        }
    }
}