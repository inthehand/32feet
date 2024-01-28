// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.IBluetoothSecurity
// 
// Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    internal interface IBluetoothSecurity
    {
        bool PairRequest(BluetoothAddress device, string pin, bool? requireMitmProtection);

        bool RemoveDevice(BluetoothAddress device);
    }
}