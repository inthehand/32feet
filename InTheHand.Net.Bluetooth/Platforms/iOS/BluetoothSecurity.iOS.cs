// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (iOS)
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothSecurity
    {
        static bool PlatformPairRequest(BluetoothAddress device, bool? requireMitmProtection, string pin)
        {
            throw new PlatformNotSupportedException();
        }

        static bool PlatformRemoveDevice(BluetoothAddress device)
        {
            throw new PlatformNotSupportedException();
        }  
    }
}
