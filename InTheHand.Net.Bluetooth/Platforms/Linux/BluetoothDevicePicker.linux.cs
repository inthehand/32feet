// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Linux)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class LinuxBluetoothDevicePicker : IBluetoothDevicePicker
    {
        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync(List<ClassOfDevice> classOfDevices, bool requiresAuthentication)
        {
            return Task.FromException<BluetoothDeviceInfo>(new PlatformNotSupportedException());
        }
    }
}