// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Windows 10)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using Windows.Devices.Enumeration;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Bluetooth;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothDevicePicker
    {
        DevicePicker picker = new DevicePicker();
            
        private async Task<BluetoothDeviceInfo> DoPickSingleDeviceAsync()
        {
            Rect bounds = Rect.Empty;

            var deviceInfo = await picker.PickSingleDeviceAsync(bounds);

            if (deviceInfo == null)
                return null;

            var device = await BluetoothDevice.FromIdAsync(deviceInfo.Id);
            var access = await device.RequestAccessAsync();

            return new BluetoothDeviceInfo(device);
        }
    }
}
