// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Multiplatform)
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
    internal interface IBluetoothDevicePicker
    {
        Task<BluetoothDeviceInfo> PlatformPickSingleDeviceAsync();
    }

    partial class BluetoothDevicePicker
    {
        private Task<BluetoothDeviceInfo> PlatformPickSingleDeviceAsync()
        {
            return Task.FromResult<BluetoothDeviceInfo>(null);
        }
    }
}