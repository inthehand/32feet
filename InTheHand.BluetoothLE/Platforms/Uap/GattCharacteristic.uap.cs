//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Uap = Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private readonly Uap.GattCharacteristic _characteristic;

        internal GattCharacteristic(GattService service, Uap.GattCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator Uap.GattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        BluetoothUuid GetUuid()
        {
            return _characteristic.Uuid;
        }

        string GetUserDescription()
        {
            return _characteristic.UserDescription;
        }

        GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)(int)_characteristic.CharacteristicProperties;
        }

        async Task<GattDescriptor> PlatformGetDescriptor(Guid descriptor)
        {
            var result = await _characteristic.GetDescriptorsForUuidAsync(descriptor);

            if (result.Status == Uap.GattCommunicationStatus.Success && result.Descriptors.Count > 0)
                return new GattDescriptor(this, result.Descriptors[0]);

            return null;
        }

        async Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();

            var result = await _characteristic.GetDescriptorsAsync();

            if(result.Status == Uap.GattCommunicationStatus.Success)
            {
                foreach (var d in result.Descriptors)
                {
                    descriptors.Add(new GattDescriptor(this, d));
                }
            }

            return descriptors;
        }

        byte[] PlatformGetValue()
        {
            var result = _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Cached).GetResults();
            
            if(result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task<byte[]> PlatformReadValue()
        {
            var result = await _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().ConfigureAwait(false);

            if(result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            await _characteristic.WriteValueAsync(value.AsBuffer(), requireResponse ? Uap.GattWriteOption.WriteWithResponse : Uap.GattWriteOption.WriteWithoutResponse);
        }

        void AddCharacteristicValueChanged()
        {
            _characteristic.ValueChanged += Characteristic_ValueChanged;
        }

        private void Characteristic_ValueChanged(Uap.GattCharacteristic sender, Uap.GattValueChangedEventArgs args)
        {
            OnCharacteristicValueChanged();
        }

        void RemoveCharacteristicValueChanged()
        {
            _characteristic.ValueChanged -= Characteristic_ValueChanged;
        }

        private async Task PlatformStartNotifications()
        {
            Uap.GattClientCharacteristicConfigurationDescriptorValue value = Uap.GattClientCharacteristicConfigurationDescriptorValue.None;
            if (_characteristic.CharacteristicProperties.HasFlag(Uap.GattCharacteristicProperties.Notify))
                value = Uap.GattClientCharacteristicConfigurationDescriptorValue.Notify;
            else if (_characteristic.CharacteristicProperties.HasFlag(Uap.GattCharacteristicProperties.Indicate))
                value = Uap.GattClientCharacteristicConfigurationDescriptorValue.Indicate;
            else
                return;

            var result = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(value);

            return;
        }

        private async Task PlatformStopNotifications()
        {
            var result = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(Uap.GattClientCharacteristicConfigurationDescriptorValue.None);

            return;
        }
    }
}
