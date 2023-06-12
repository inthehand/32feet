// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Multiplatform)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothSecurity
    {
        static bool PlatformPairRequest(BluetoothAddress device, string pin)
        {
            return false;
        }

        static bool PlatformRemoveDevice(BluetoothAddress device)
        {
            return false;
        }  
    }
}
