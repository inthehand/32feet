using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Uap = Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private readonly Uap.GattCharacteristic _characteristic;

        internal GattCharacteristic(BluetoothRemoteGATTService service, Uap.GattCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator Uap.GattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        Guid GetUuid()
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

        async Task<GattDescriptor> DoGetDescriptor(Guid descriptor)
        {
            var result = await _characteristic.GetDescriptorsForUuidAsync(descriptor);

            if (result.Status == Uap.GattCommunicationStatus.Success && result.Descriptors.Count > 0)
                return new GattDescriptor(this, result.Descriptors[0]);

            return null;
        }

        async Task<IReadOnlyList<GattDescriptor>> DoGetDescriptors()
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

        async Task<byte[]> DoGetValue()
        {
            var result = await _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Cached).AsTask().ConfigureAwait(false);
            
            if(result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task<byte[]> DoReadValue()
        {
            var result = await _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().ConfigureAwait(false);

            if(result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task DoWriteValue(byte[] value)
        {
            await _characteristic.WriteValueAsync(value.AsBuffer());
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

        private async Task DoStartNotifications()
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

        private async Task DoStopNotifications()
        {
            var result = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(Uap.GattClientCharacteristicConfigurationDescriptorValue.None);

            return;
        }
    }
}
