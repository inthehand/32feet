//-----------------------------------------------------------------------
// <copyright file="DevicePickerFilter.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using InTheHand.Devices.Bluetooth;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents the filter used to determine which devices to show in the device picker.
    /// The filter parameters are OR-ed together to build the resulting filter.
    /// </summary>
    public sealed class DevicePickerFilter
    {
#if WINDOWS_UWP
        private Windows.Devices.Enumeration.DevicePickerFilter _filter;

        private DevicePickerFilter(Windows.Devices.Enumeration.DevicePickerFilter filter)
        {
            _filter = filter;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePickerFilter(DevicePickerFilter filter)
        {
            return filter._filter;
        }

        public static implicit operator DevicePickerFilter(Windows.Devices.Enumeration.DevicePickerFilter filter)
        {
            return new DevicePickerFilter(filter);
        }
#else
        private List<string> _supportedDeviceSelectors = new List<string>();
#endif

        /// <summary>
        /// Gets a list of AQS filter strings.
        /// This defaults to empty list (no filter).
        /// You can add one or more AQS filter strings to this vector and filter the devices list to those that meet one or more of the provided filters.
        /// </summary>
        /// <remarks>
        /// Some platforms have limitations on the filter types they support:-
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Supports only a small set of ClassOfDevice restrictions.
        /// <see cref="BluetoothServiceCapabilities.AudioService"/>, 
        /// <see cref="BluetoothServiceCapabilities.ObjectTransferService"/>, 
        /// <see cref="BluetoothServiceCapabilities.NetworkingService"/>, 
        /// <see cref="BluetoothMajorClass.NetworkAccessPoint"/> any other class values will display all device types.</description></item>
        /// <item><term>Windows UWP</term><description>No restrictions. Additional filter types are added in Creators Update.</description></item>
        /// <item><term>Windows Store</term><description>No restrictions</description></item>
        /// <item><term>Windows Phone Store</term><description>No restrictions</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Supports <see cref="BluetoothDevice.GetDeviceSelectorFromClassOfDevice()"/> and <see cref="BluetoothDevice.GetDeviceSelectorFromPairingState()"/>.</description></item></list>
        /// </remarks>
        public IList<string> SupportedDeviceSelectors
        {
            get
            {
#if WINDOWS_UWP
                return _filter.SupportedDeviceSelectors;
#else
                return _supportedDeviceSelectors;
#endif
            }
        }
    }
}