// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Contains functionality to pair (and un-pair) Bluetooth devices.
    /// </summary>
    public sealed partial class BluetoothSecurity
    {
        /// <summary>
        /// Requests the pairing process for the specified device with the provided pin or numeric code.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static bool PairRequest(BluetoothAddress device, string pin)
        {
            return PlatformPairRequest(device, pin);
        }

        /// <summary>
        /// Requests that the specified device is un-paired.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool RemoveDevice(BluetoothAddress device)
        {
            return PlatformRemoveDevice(device);
        }
    }
}
