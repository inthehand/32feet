//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using InTheHand.Foundation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class BluetoothDevice
    {
        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given BluetoothAddress.
        /// </summary>
        /// <param name="address">The address of the Bluetooth device.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object with the given BluetoothAddress or null if the address does not resolve to a valid device.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public static Task<BluetoothDevice> FromBluetoothAddressAsync(ulong address)
        {
            return FromBluetoothAddressAsyncImpl(address);
        }

        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given Id.
        /// </summary>
        /// <param name="deviceId">The DeviceId value that identifies the <see cref="BluetoothDevice"/> instance.</param>
        /// <returns>After the asynchronous operation completes, returns the <see cref="BluetoothDevice"/> object identified by the given DeviceId.</returns>
        public static Task<BluetoothDevice> FromIdAsync(string deviceId)
        {
            return FromIdAsyncImpl(deviceId);
        }

        /// <summary>
        /// Returns a <see cref="BluetoothDevice"/> object for the given DeviceInformation.
        /// </summary>
        /// <param name="deviceInformation">The <see cref="DeviceInformation"/> value that identifies the BluetoothDevice instance.</param>
        /// <returns>After the asynchronous operation completes, returns the BluetoothDevice object identified by the given DeviceInformation.</returns>
        public static Task<BluetoothDevice> FromDeviceInformationAsync(DeviceInformation deviceInformation)
        {
            return FromDeviceInformationAsyncImpl(deviceInformation);
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying all Bluetooth devices.
        /// This string is passed to the <see cref="DeviceInformation.FindAllAsync"/> or CreateWatcher method in order to get a list of Bluetooth devices.
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceSelector()
        {
            return GetDeviceSelectorImpl();
        }

        /// <summary>
        /// Creates an Advanced Query Syntax (AQS) filter string from a BluetoothClassOfDevice object.
        /// The AQS string is passed into the CreateWatcher method to return a collection of DeviceInformation objects.
        /// </summary>
        /// <param name="classOfDevice">The class of device used for constructing the AQS string.</param>
        /// <returns>An AQS string that can be passed as a parameter to the CreateWatcher method.</returns>
        public static string GetDeviceSelectorFromClassOfDevice(BluetoothClassOfDevice classOfDevice)
        {
            return GetDeviceSelectorFromClassOfDeviceImpl(classOfDevice);
        }

        /// <summary>
        /// Creates an Advanced Query Syntax (AQS) filter string that contains a query for Bluetooth devices that are either paired or unpaired.
        /// The AQS string is passed into the CreateWatcher method to return a collection of <see cref="DeviceInformation"/> objects.
        /// </summary>
        /// <param name="pairingState">The current pairing state for Bluetooth devices used for constructing the AQS string.
        /// Bluetooth devices can be either paired (true) or unpaired (false).
        /// The AQS Filter string will request scanning to be performed when the pairingState is false.</param>
        /// <returns>An AQS string that can be passed as a parameter to the CreateWatcher method.</returns>
        public static string GetDeviceSelectorFromPairingState(bool pairingState)
        {
            return GetDeviceSelectorFromPairingStateImpl(pairingState);
        }

        /*public Task<RfcommDeviceServicesResult> GetRfcommServicesAsync()
        {

        }*/

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
        /// Gets the Bluetooth Class Of Device information of the device.
        /// </summary>
        public BluetoothClassOfDevice ClassOfDevice
        {
            get
            {
                return GetClassOfDevice();
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

        private event EventHandler _connectionStatusChanged;

        /// <summary>
        /// Occurs when the connection status for the device has changed.
        /// </summary>
        public event EventHandler ConnectionStatusChanged
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
        /// <value>The ID of the device.</value>
        public string DeviceId
        {
            get
            {
                return GetDeviceId();
            }
        }

        /// <summary>
        /// Gets the Name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name
        {
            get
            {
                return GetName();
            }
        }

        private void RaiseNameChanged()
        {
            _nameChanged?.Invoke(this, null);
        }

        private event TypedEventHandler<BluetoothDevice, object> _nameChanged;

        /// <summary>
        /// Occurs when the name of the device has changed.
        /// </summary>
        public event TypedEventHandler<BluetoothDevice, object> NameChanged
        {
            add
            {
                if(_nameChanged == null)
                {
                    NameChangedAdd();
                }

                _nameChanged += value;
            }
            remove
            {
                _nameChanged -= value;
                if(_nameChanged == null)
                {
                    NameChangedRemove();
                }
            }
        }

        /// <summary>
        /// Retrieves all Rfcomm Services on the remote Bluetooth Device.
        /// </summary>
        /// <returns></returns>
        public Task<RfcommDeviceServicesResult> GetRfcommServicesAsync()
        {
            return GetRfcommServicesAsyncImpl(BluetoothCacheMode.Cached);
        }

        /// <summary>
        /// Retrieves all cached Rfcomm Services on the remote Bluetooth Device.
        /// </summary>
        /// <param name="cacheMode">The cache mode.</param>
        /// <returns></returns>
        public Task<RfcommDeviceServicesResult> GetRfcommServicesAsync(BluetoothCacheMode cacheMode)
        {
            return GetRfcommServicesAsyncImpl(cacheMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "BLUETOOTH#" + BluetoothAddress.ToString("X12");
        }
    }
}