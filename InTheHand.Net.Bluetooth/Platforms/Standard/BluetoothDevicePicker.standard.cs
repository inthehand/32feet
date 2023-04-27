// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (.NET Standard)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothDevicePicker
    {
        private Task<BluetoothDeviceInfo> PlatformPickSingleDeviceAsync()
        {
            return Task.FromResult<BluetoothDeviceInfo>(null);
        }
    }
}