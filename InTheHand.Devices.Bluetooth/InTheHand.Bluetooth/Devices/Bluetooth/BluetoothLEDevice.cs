//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InTheHand.Foundation;
using System.Threading;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device.
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
    public sealed partial class BluetoothLEDevice
    {
        /// <summary>
        /// Returns a BluetoothLEDevice object for the given BluetoothAddress.
        /// </summary>
        /// <param name="bluetoothAddress">The address of the Bluetooth LE device.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothLEDevice object with the given BluetoothAddress.</returns>
        public static Task<BluetoothLEDevice> FromBluetoothAddressAsync(ulong bluetoothAddress)
        {
            return FromBluetoothAddressAsyncImpl(bluetoothAddress);
        }

        /// <summary>
        /// Returns a <see cref="BluetoothLEDevice"/> object for the given Id.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static Task<BluetoothLEDevice> FromIdAsync(string deviceId)
        {
            return FromIdAsyncImpl(deviceId);
        }

        /// <summary>
        /// Returns a <see cref="BluetoothLEDevice"/> object for the given DeviceInformation.
        /// </summary>
        /// <param name="deviceInformation">The DeviceInformation value that identifies the <see cref="BluetoothLEDevice"/> instance.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object identified by the given DeviceInformation.</returns>
        public static async Task<BluetoothLEDevice> FromDeviceInformationAsync(DeviceInformation deviceInformation)
        {
            return await FromDeviceInformationAsyncImpl(deviceInformation);
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying all Bluetooth Low Energy (LE) devices.
        /// This string is passed to the <see cref="DeviceInformation.FindAllAsync"/> or CreateWatcher method in order to get a list of Bluetooth LE devices.
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceSelector()
        {
            return GetDeviceSelectorImpl();
        }

        /// <summary>
        /// Creates a filter string that contains a query for Bluetooth LE devices with the indicated BluetoothConnectionStatus.
        /// The string is passed into the CreateWatcher method to return a collection of DeviceInformation objects with the indicated Bluetooth connection status.
        /// </summary>
        /// <param name="connectionStatus">The connection status used for constructing the AQS string.</param>
        /// <returns></returns>
        public static string GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus connectionStatus)
        {
            return GetDeviceSelectorFromConnectionStatusImpl(connectionStatus);
        }

        private event TypedEventHandler<BluetoothLEDevice, object> _nameChanged;
        /// <summary>
        /// Occurs when the name of the device has changed.
        /// </summary>
        public event TypedEventHandler<BluetoothLEDevice, Object> NameChanged
        {
            add
            {
                if (_nameChanged == null)
                {
                    NameChangedAdd();
                }

                _nameChanged += value;

            }

            remove
            {
                _nameChanged -= value;

                if (_nameChanged == null)
                {
                    NameChangedRemove();
                }
            }
        }

        private void RaiseNameChanged()
        {
            _nameChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
                return GetBluetoothAddress();
            }
        }

        /// <summary>
        /// Gets the address type for the Bluetooth LE device.
        /// </summary>
        public BluetoothAddressType BluetoothAddressType
        {
            get
            {
                return GetBluetoothAddressType();
            }
        }

        /// <summary>
        /// Gets the connection status of the device.
        /// </summary>
        public BluetoothConnectionStatus ConnectionStatus
        {
            get
            {
                return GetConnectionStatus();
            }
        }

        private void RaiseConnectionStatusChanged()
        {
            _connectionStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private event TypedEventHandler<BluetoothLEDevice, object> _connectionStatusChanged;
        /// <summary>
        /// Occurs when the connection status for the device has changed.
        /// </summary>
        public event TypedEventHandler<BluetoothLEDevice, object> ConnectionStatusChanged
        {
            add
            {
                if(_connectionStatusChanged == null)
                {
                    ConnectionStatusChangedAdd();
                }

                _connectionStatusChanged += value;
            }
            remove
            {
                _connectionStatusChanged -= value;

                if (_connectionStatusChanged == null)
                {
                    ConnectionStatusChangedRemove();
                }
            }
        }


        /// <summary>
        /// Gets the device Id.
        /// </summary>
        public string DeviceId
        {
            get
            {
                return GetDeviceId();
            }
        }

        /// <summary>
        /// Gets the read-only list of GATT services supported by the device.
        /// </summary>
        public IReadOnlyList<GattDeviceService> GattServices
        {
            get
            {
                return GetGattServices();
            }
        }

        public override string ToString()
        {
            return DeviceId;
        }
    }
}