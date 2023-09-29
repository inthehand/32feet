// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Linux)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    internal sealed class LinuxBluetoothSecurity : IBluetoothSecurity
    {
        public bool PairRequest(BluetoothAddress device, string pin, bool? requireMitmProtection)
        {
            return false;
        }

        public bool RemoveDevice(BluetoothAddress device)
        {
            return false;
        }  
    }
}
