//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents a GATT Characteristic, which is a basic data element that provides further information about a peripheral’s service.
    /// </summary>
    /// <remarks>Equivalent to <b>BluetoothRemoteGATTCharacteristic</b> in WebBluetooth.</remarks>
    [DebuggerDisplay("{Uuid} (Characteristic)")]
    public sealed partial class GattCharacteristic
    {
        internal GattCharacteristic(GattService service)
        {
            Service = service;
        }

        /// <summary>
        /// The GATT service this characteristic belongs to.
        /// </summary>
        public GattService Service { get; private set; }

        /// <summary>
        /// The UUID of the characteristic.
        /// </summary>
        /// <seealso cref="BluetoothUuid"/>
        public BluetoothUuid Uuid => GetUuid();

        /// <summary>
        /// The properties of this characteristic.
        /// </summary>
        /// <seealso cref="GattCharacteristicProperties"/>
        public GattCharacteristicProperties Properties => GetProperties();

        /// <summary>
        /// The currently cached characteristic value. 
        /// This value gets updated when the value of the characteristic is read or updated via a notification or indication.
        /// </summary>
        public byte[] Value => PlatformGetValue();

        /// <summary>
        /// Performs a Characteristic Value read from the Bluetooth LE device.
        /// </summary>
        /// <returns>The object required to manage the asynchronous operation, which, upon completion, returns the data read from the device.</returns>
        public Task<byte[]> ReadValueAsync()
        {
            //if (!Service.Device.Gatt.Connected)
                //throw new NetworkException();

            return PlatformReadValue();
        }

        /// <summary>
        /// Performs a Characteristic Value write to the Bluetooth LE device.
        /// </summary>
        /// <param name="value">The data to be written to the Bluetooth LE device.</param>
        /// <returns></returns>
        public Task WriteValueWithResponseAsync(byte[] value)
        {
            Bluetooth.ThrowOnInvalidAttributeValue(value);
            return PlatformWriteValue(value, true);
        }

        /// <summary>
        /// Performs a Characteristic Value write to the Bluetooth LE device.
        /// </summary>
        /// <param name="value">The data to be written to the Bluetooth LE device.</param>
        /// <returns></returns>
        public Task WriteValueWithoutResponseAsync(byte[] value)
        {
            Bluetooth.ThrowOnInvalidAttributeValue(value);
            return PlatformWriteValue(value, false);
        }

        public Task<GattDescriptor?> GetDescriptorAsync(BluetoothUuid descriptor) => PlatformGetDescriptor(descriptor);

        public Task<IReadOnlyList<GattDescriptor>> GetDescriptorsAsync() => PlatformGetDescriptors();

        private event EventHandler<GattCharacteristicValueChangedEventArgs> characteristicValueChanged;

        private void OnCharacteristicValueChanged(GattCharacteristicValueChangedEventArgs args)
        {
            characteristicValueChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Fired when the value changes, as a result of a value change notification/indication.
        /// </summary>
        public event EventHandler<GattCharacteristicValueChangedEventArgs> CharacteristicValueChanged
        {
            add
            {
                characteristicValueChanged += value;
                AddCharacteristicValueChanged();

            }
            remove
            {
                characteristicValueChanged -= value;
                RemoveCharacteristicValueChanged();
            }
        }

        /// <summary>
        /// Registers to start receiving value changed notifications.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Characteristic must support either Notify or Indicate properties.</exception>
        public Task StartNotificationsAsync()
        {
            if (Properties.HasFlag(GattCharacteristicProperties.Notify) | Properties.HasFlag(GattCharacteristicProperties.Indicate))
            {
                return PlatformStartNotifications();
            }

            throw new NotSupportedException("Characteristic does not support Notify or Indicate.");
        }

        /// <summary>
        /// Unregisters for value changed notifications.
        /// </summary>
        /// <returns></returns>
        public Task StopNotificationsAsync()
        {
            return PlatformStopNotifications();
        }
    }
}
