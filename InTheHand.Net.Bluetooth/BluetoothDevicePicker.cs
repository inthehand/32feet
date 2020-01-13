// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    public sealed partial class BluetoothDevicePicker
    {
        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync()
        {
            return DoPickSingleDeviceAsync();
        }

        public List<ClassOfDevice> ClassOfDevices { get; } = new List<ClassOfDevice>();

        public bool RequireAuthentication { get; set; }
    }
}
