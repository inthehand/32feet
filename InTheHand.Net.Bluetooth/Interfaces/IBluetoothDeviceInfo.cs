// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.IBluetoothClient
// 
// Copyright (c) 2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    internal interface IBluetoothDeviceInfo
    {
        void Refresh();

        BluetoothAddress DeviceAddress { get; }

        string DeviceName { get; }

        ClassOfDevice ClassOfDevice { get; }

        Task<IEnumerable<Guid>> GetRfcommServicesAsync(bool cached);
        
        bool Connected { get; }

        bool Authenticated { get; }
    }
}