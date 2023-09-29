// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (iOS)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class ExternalAccessoryBluetoothSecurity : IBluetoothSecurity
    {
        public bool PairRequest(BluetoothAddress device, string pin, bool? requireMitmProtection)
        {
            throw new PlatformNotSupportedException();
        }

        public bool RemoveDevice(BluetoothAddress device)
        {
            throw new PlatformNotSupportedException();
        }  
    }
}
