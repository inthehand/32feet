//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characteristic"></param>
        public static implicit operator Uap.GattCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        private BluetoothUuid GetUuid()
        {
            return _characteristic.Uuid;
        }

        /// <summary>
        /// Get the user-friendly description for this GattCharacteristic, if the User Description <see cref="GattDescriptor">Descriptor</see> is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription => _characteristic.UserDescription;

        private GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)(int)_characteristic.CharacteristicProperties;
        }

        private async Task<GattDescriptor?> PlatformGetDescriptor(Guid descriptor)
        {
            var result = await _characteristic.GetDescriptorsForUuidAsync(descriptor);

            if (result.Status == Uap.GattCommunicationStatus.Success && result.Descriptors.Count > 0)
                return new GattDescriptor(this, result.Descriptors[0]);

            return null;
        }

        private async Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            var descriptors = new List<GattDescriptor>();

            var result = await _characteristic.GetDescriptorsAsync();

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                foreach (var d in result.Descriptors)
                {
                    descriptors.Add(new GattDescriptor(this, d));
                }
            }

            return descriptors;
        }

        private byte[] PlatformGetValue()
        {
            var t = _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Cached).AsTask();
            t.Wait();
            var result = t.Result;

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.Length == 0 ? [] : result.Value.ToArray();
            }

            return null;
        }

        private async Task<byte[]> PlatformReadValue()
        {
            var result = await _characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().ConfigureAwait(false);

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.Length == 0 ? [] : result.Value.ToArray();
            }

            return null;
        }

        private async Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            await _characteristic.WriteValueAsync(value.AsBuffer(), requireResponse ? Uap.GattWriteOption.WriteWithResponse : Uap.GattWriteOption.WriteWithoutResponse);
        }

        private void AddCharacteristicValueChanged()
        {
            _characteristic.ValueChanged += Characteristic_ValueChanged;
        }

        private void Characteristic_ValueChanged(Uap.GattCharacteristic sender, Uap.GattValueChangedEventArgs args)
        {
            OnCharacteristicValueChanged(new GattCharacteristicValueChangedEventArgs(args.CharacteristicValue.Length == 0 ? [] : args.CharacteristicValue.ToArray()));
        }

        void RemoveCharacteristicValueChanged()
        {
            _characteristic.ValueChanged -= Characteristic_ValueChanged;
        }

        private async Task PlatformStartNotifications()
        {
            var value = Uap.GattClientCharacteristicConfigurationDescriptorValue.None;
            if (_characteristic.CharacteristicProperties.HasFlag(Uap.GattCharacteristicProperties.Notify))
                value = Uap.GattClientCharacteristicConfigurationDescriptorValue.Notify;
            else if (_characteristic.CharacteristicProperties.HasFlag(Uap.GattCharacteristicProperties.Indicate))
                value = Uap.GattClientCharacteristicConfigurationDescriptorValue.Indicate;
            else
                return;

            try
            {
                var result = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(value);
            }
            catch(UnauthorizedAccessException)
            {
                // not supported
            }
        }

        private async Task PlatformStopNotifications()
        {
            try
            {
                var result = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(Uap.GattClientCharacteristicConfigurationDescriptorValue.None);
            }
            catch
            {
                throw new NotSupportedException();
                // HRESULT 0x800704D6 means that a connection to the server could not be made because the limit on the number of concurrent connections for this account has been reached.
            }
        }
    }
}