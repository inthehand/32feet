//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTheHand.Foundation;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Characteristic of a GATT service.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class GattCharacteristic
    {
        public Task<GattDescriptorsResult> GetDescriptorsAsync()
        {
            return GetDescriptorsAsync(BluetoothCacheMode.Cached);
        }
        public Task<GattDescriptorsResult> GetDescriptorsAsync(BluetoothCacheMode cacheMode)
        {
            return GetDescriptorsAsyncImpl(cacheMode);
        }

        public Task<GattDescriptorsResult> GetDescriptorsForUuidAsync(Guid descriptorUuid)
        {
            return GetDescriptorsForUuidAsync(descriptorUuid, BluetoothCacheMode.Cached);
        }

        public Task<GattDescriptorsResult> GetDescriptorsForUuidAsync(Guid descriptorUuid, BluetoothCacheMode cacheMode)
        {
            return GetDescriptorsForUuidAsyncImpl(descriptorUuid, cacheMode);
        }

        /// <summary>
        /// Performs a Characteristic Value read from the value cache maintained by the system.
        /// </summary>
        /// <returns></returns>
        public Task<GattReadResult> ReadValueAsync()
        {
            return ReadValueAsync(BluetoothCacheMode.Cached);
        }

        /// <summary>
        /// Performs a Characteristic Value read either from the value cache maintained by the system, or directly from the device.
        /// </summary>
        /// <returns></returns>
        public Task<GattReadResult> ReadValueAsync(BluetoothCacheMode cacheMode)
        {
            return DoReadValueAsync(cacheMode);
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        public Task<GattCommunicationStatus> WriteValueAsync(byte[] value)
        {
            return WriteValueAsync(value, GattWriteOption.WriteWithResponse);
        }

        /// <summary>
        /// Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">A byte array object which contains the data to be written to the Bluetooth LE device.</param>
        /// <param name="writeOption">Specifies what type of GATT write should be performed.</param>
        /// <returns>The object that manages the asynchronous operation, which, upon completion, returns the status with which the operation completed.</returns>
        public Task<GattCommunicationStatus> WriteValueAsync(byte[] value, GattWriteOption writeOption)
        {
            return DoWriteValueAsync(value, writeOption);
        }


        /// <summary>
        /// Gets the GATT characteristic properties, as defined by the GATT profile.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties
        {
            get
            {
                return GetCharacteristicProperties();
            }
        }

        /// <summary>
        /// Gets the <see cref="GattDeviceService"/> of which this characteristic is a member.
        /// </summary>
        public GattDeviceService Service
        {
            get
            {
                return GetService();
            }
        }

        /// <summary>
        /// Get the user friendly description for this GattCharacteristic, if the User Description Descriptor is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription
        {
            get
            {
                return GetUserDescription();
            }
        }

        /// <summary>
        /// Gets the GATT Characteristic UUID for this GattCharacteristic.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
            }
        }

        private event TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> valueChanged;
        /// <summary>
        /// An App can register an event handler in order to receive events when notification or indications are received from a device, after setting the Client Characteristic Configuration Descriptor.
        /// </summary>
        public event TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> ValueChanged
        {
            add
            {
                if(valueChanged == null)
                {
                    ValueChangedAdd();
                }

                valueChanged += value;
            }

            remove
            {
                valueChanged -= value;

                if (valueChanged == null)
                {
                    ValueChangedRemove();
                }
            }
        }
        
    }
}