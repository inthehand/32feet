//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    public sealed partial class GattCharacteristic
    {
        internal GattCharacteristic(BluetoothRemoteGATTService service)
        {
            Service = service;
        }

        public BluetoothRemoteGATTService Service { get; private set; }

        public Guid Uuid { get { return GetUuid(); } }

        public GattCharacteristicProperties Properties { get { return GetProperties(); } }

        public byte[] Value
        {
            get
            {
                var task = DoGetValue();
                task.Wait();
                return task.Result;
            }
        }

        public Task<byte[]> ReadValueAsync()
        {
            //if (!Service.Device.Gatt.Connected)
                //throw new NetworkException();

            return DoReadValue();
        }

        public Task WriteValueAsync(byte[] value)
        {
            if (value is null)
                throw new ArgumentNullException("value");

            if (value.Length > 512)
                throw new ArgumentOutOfRangeException("value", "Attribute value cannot be longer than 512 bytes");

            return DoWriteValue(value);
        }

        public Task<GattDescriptor> GetDescriptorAsync(Guid descriptor)
        {
            return DoGetDescriptor(descriptor);
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
    }
}
