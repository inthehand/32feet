// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.IBluetoothRadio
// 
// Copyright (c) 2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    internal interface IBluetoothRadio : IDisposable
    {
        string Name { get; }

        BluetoothAddress LocalAddress { get; }

        RadioMode Mode { get; set; }

        CompanyIdentifier Manufacturer { get; }
    }
}
