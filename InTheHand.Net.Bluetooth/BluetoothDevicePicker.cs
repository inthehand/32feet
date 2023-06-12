// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Picker dialog to select a single Bluetooth device.
    /// </summary>
    public sealed partial class BluetoothDevicePicker
    {
        /// <summary>
        /// Display the dialog and allow the user to pick a single device.
        /// </summary>
        /// <returns></returns>
        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync()
        {
            return PlatformPickSingleDeviceAsync();
        }

        /// <summary>
        /// Class of device to filter the list of devices.
        /// </summary>
        public List<ClassOfDevice> ClassOfDevices { get; } = new List<ClassOfDevice>();

        public bool RequireAuthentication { get; set; }
    }
}
