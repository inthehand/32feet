// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    public sealed partial class BluetoothSecurity
    {
        public static bool PairRequest(BluetoothAddress device, string pin)
        {
            return PlatformPairRequest(device, pin);
        }

        public static bool RemoveDevice(BluetoothAddress device)
        {
            return PlatformRemoveDevice(device);
        }
    }
}
