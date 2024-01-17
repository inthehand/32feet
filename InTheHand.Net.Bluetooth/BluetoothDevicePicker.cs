// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker
// 
// Copyright (c) 2018-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Picker dialog to select a single Bluetooth device.
    /// </summary>
    public sealed class BluetoothDevicePicker
    {
        private readonly IBluetoothDevicePicker _bluetoothDevicePicker;

        public BluetoothDevicePicker()
        {
#if ANDROID || MONOANDROID
            _bluetoothDevicePicker = new AndroidBluetoothDevicePicker();
#elif IOS || __IOS__
            _bluetoothDevicePicker = new ExternalAccessoryBluetoothDevicePicker();
#elif WINDOWS_UWP || WINDOWS10_0_17763_0_OR_GREATER
            _bluetoothDevicePicker = new WindowsBluetoothDevicePicker();
#elif WINDOWS7_0_OR_GREATER
            _bluetoothDevicePicker = new Win32BluetoothDevicePicker();
#elif NETSTANDARD
#else
            switch (Environment.OSVersion.Platform)
            {
                /*case PlatformID.Unix:
                    _bluetoothDevicePicker = new LinuxBluetoothDevicePicker();
                    break;*/
                case PlatformID.Win32NT:
                    _bluetoothDevicePicker = new Win32BluetoothDevicePicker();
                    break;
            }
#endif
            if (_bluetoothDevicePicker == null)
                throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the dialog and allow the user to pick a single device.
        /// </summary>
        /// <returns></returns>
        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync()
        {
            return _bluetoothDevicePicker.PickSingleDeviceAsync(ClassOfDevices, RequireAuthentication);
        }

        /// <summary>
        /// Class of device to filter the list of devices.
        /// </summary>
        public List<ClassOfDevice> ClassOfDevices { get; } = new List<ClassOfDevice>();

        public bool RequireAuthentication { get; set; }
    }
}
