//-----------------------------------------------------------------------
// <copyright file="DevicePicker.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-18 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;


namespace InTheHand.Devices.Enumeration
{
    internal class DevicePickerUap : IDevicePicker
    {
        private Windows.Devices.Enumeration.DevicePicker _devicePicker;

        internal DevicePickerUap()
        {
            _devicePicker = new Windows.Devices.Enumeration.DevicePicker();
        }

        private DevicePickerUap(Windows.Devices.Enumeration.DevicePicker devicePicker)
        {
            _devicePicker = devicePicker;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePicker(DevicePickerUap devicePicker)
        {
            return devicePicker._devicePicker;
        }

        public static implicit operator DevicePickerUap(Windows.Devices.Enumeration.DevicePicker devicePicker)
        {
            return new DevicePickerUap(devicePicker);
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

        /*public DevicePickerAppearance Appearance
        {
            get
            {
                return _devicePicker.Appearance;
            }
        }*/

        public DevicePickerFilter Filter
        {
            get
            {
#if WIN32
                var filter = new DevicePickerFilter();
                //_devicePicker.Filter.SupportedDeviceSelectors.CopyTo(filter.SupportedDeviceSelectors,0);
                return filter;
#else
                return _devicePicker.Filter;
#endif
            }
        }

        public DeviceInformation PickSingleDevice()
        {
            Task<DeviceInformation> t = Task.Run<DeviceInformation>(async () =>
            {

                var d = await _devicePicker.PickSingleDeviceAsync(Windows.Foundation.Rect.Empty);
#if WIN32
                if (d != null)
                {
                    Bluetooth.BLUETOOTH_DEVICE_INFO info = new Bluetooth.BLUETOOTH_DEVICE_INFO();
                    info.Address = 0;
                    return new DeviceInformation(info);
                }

                return null;
#else
                return d == null ? null : d;
#endif
            });

            // TODO: this compiles but does it behave?
            t.Wait();

            return t.Result;
        }

        public async Task<DeviceInformation> PickSingleDeviceAsync()
        {
            var d = await _devicePicker.PickSingleDeviceAsync(Windows.Foundation.Rect.Empty);
#if WIN32
            if(d != null)
            {
                Bluetooth.BLUETOOTH_DEVICE_INFO info = new Bluetooth.BLUETOOTH_DEVICE_INFO();
                info.Address = 0;
                return new DeviceInformation(info);
            }

            return null;
#else
            return d == null ? null : d;
#endif
        }


    }
}