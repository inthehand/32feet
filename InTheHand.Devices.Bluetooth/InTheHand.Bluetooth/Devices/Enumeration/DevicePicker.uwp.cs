//-----------------------------------------------------------------------
// <copyright file="DevicePicker.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;
using System.Diagnostics;
using InTheHand.Foundation;
using InTheHand.UI.Popups;

namespace InTheHand.Devices.Enumeration
{
    partial class DevicePicker
    {
        private Windows.Devices.Enumeration.DevicePicker _devicePicker = new Windows.Devices.Enumeration.DevicePicker();

        private DevicePicker(Windows.Devices.Enumeration.DevicePicker devicePicker)
        {
            _devicePicker = devicePicker;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePicker(DevicePicker devicePicker)
        {
            return devicePicker._devicePicker;
        }

        public static implicit operator DevicePicker(Windows.Devices.Enumeration.DevicePicker devicePicker)
        {
            return new DevicePicker(devicePicker);
        }

        private void Initialize()
        {
            //_picker.DevicePickerDismissed += _picker_DevicePickerDismissed;
            //_picker.DeviceSelected += _picker_DeviceSelected;
        }
        /*
        private void _picker_DeviceSelected(Windows.Devices.Enumeration.DevicePicker sender, Windows.Devices.Enumeration.DeviceSelectedEventArgs args)
        {
            OnDeviceSelected(args.SelectedDevice);
        }

        private void _picker_DevicePickerDismissed(Windows.Devices.Enumeration.DevicePicker sender, object args)
        {
            OnDevicePickerDismissed();
        }*/

        private DevicePickerAppearance GetAppearance()
        {
            return _devicePicker.Appearance;
        }

        private DevicePickerFilter GetFilter()
        {
            return _devicePicker.Filter;
        }

        private async Task<DeviceInformation> DoPickSingleDeviceAsync(Rect selection, Placement placement)
        {
            var d = await _devicePicker.PickSingleDeviceAsync(selection, (Windows.UI.Popups.Placement)((int)placement));
            return d == null ? null : d;
        }


    }
}