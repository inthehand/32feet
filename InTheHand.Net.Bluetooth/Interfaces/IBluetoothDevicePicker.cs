// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.IBluetoothDevicePicker
// 
// Copyright (c) 2023 In The Hand Ltd, All rights reserved.
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
        Task<BluetoothDeviceInfo> PickSingleDeviceAsync(List<ClassOfDevice> classOfDevices, bool requireAuthentication);
    }
}