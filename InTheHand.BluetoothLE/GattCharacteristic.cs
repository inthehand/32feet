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
    [DebuggerDisplay("{Uuid} ({UserDescription})")]
    public sealed partial class GattCharacteristic
    {
        internal GattCharacteristic(GattService service)
        {
            Service = service;
        }

        public GattService Service { get; private set; }

        public BluetoothUuid Uuid { get { return GetUuid(); } }

        public GattCharacteristicProperties Properties { get { return GetProperties(); } }

        public string UserDescription { get { return GetUserDescription(); } }

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

        public byte[] Value
        {
            get
            {
                var task = PlatformGetValue();
                task.Wait();
                return task.Result;
            }
        }

        public Task<byte[]> ReadValueAsync()
        {
            //if (!Service.Device.Gatt.Connected)
                //throw new NetworkException();

            return PlatformReadValue();
        }

        public Task WriteValueWithResponseAsync(byte[] value)
        {
            ThrowOnInvalidValue(value);
            return PlatformWriteValue(value, true);
        }

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
