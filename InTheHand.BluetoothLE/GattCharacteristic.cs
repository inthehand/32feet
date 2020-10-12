//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
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
        public BluetoothUuid Uuid { get { return GetUuid(); } }

        /// <summary>
        /// The properties of this characteristic.
        /// </summary>
        /// <seealso cref="GattCharacteristicProperties"/>
        public GattCharacteristicProperties Properties { get { return GetProperties(); } }

        /// <summary>
        /// Get the user friendly description for this GattCharacteristic, if the User Description <see cref="GattDescriptor">Descriptor</see> is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription { get { return GetUserDescription(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used on multiple platforms")]
        private string GetManualUserDescription()
        {
            var descriptor = GetDescriptorAsync(GattDescriptorUuids.CharacteristicUserDescription).Result;

            if(descriptor != null)
            {
                var bytes = descriptor.ReadValueAsync().Result;
                return System.Text.Encoding.UTF8.GetString(bytes);
            }

            return string.Empty;
        }

        /// <summary>
        /// The currently cached characteristic value. 
        /// This value gets updated when the value of the characteristic is read or updated via a notification or indication.
        /// </summary>
        public byte[] Value
        {
            get
            {
                return PlatformGetValue();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> ReadValueAsync()
        {
            //if (!Service.Device.Gatt.Connected)
                //throw new NetworkException();

            return PlatformReadValue();
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task WriteValueWithResponseAsync(byte[] value)
        {
            ThrowOnInvalidValue(value);
            return PlatformWriteValue(value, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task WriteValueWithoutResponseAsync(byte[] value)
        {
            ThrowOnInvalidValue(value);
            return PlatformWriteValue(value, false);
        }

        private void ThrowOnInvalidValue(byte[] value)
        {
            if (value is null)
                throw new ArgumentNullException("value");

            if (value.Length > 512)
                throw new ArgumentOutOfRangeException("value", "Attribute value cannot be longer than 512 bytes");
        }

        public Task<GattDescriptor> GetDescriptorAsync(BluetoothUuid descriptor)
        {
            return PlatformGetDescriptor(descriptor);
        }

        public Task<IReadOnlyList<GattDescriptor>> GetDescriptorsAsync()
        {
            return PlatformGetDescriptors();
        }

        private event EventHandler characteristicValueChanged;

        void OnCharacteristicValueChanged()
        {
            characteristicValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CharacteristicValueChanged
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

        public Task StartNotificationsAsync()
        {
            return PlatformStartNotifications();
        }

        public Task StopNotificationsAsync()
        {
            return PlatformStopNotifications();
        }
    }
}
