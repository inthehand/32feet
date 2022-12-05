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
        /// Allows for editing the authentication and MITM protection level
        /// </summary>
        /// <param name="device"></param>
        /// <param name="pin"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool PairRequest(BluetoothAddress device, string pin, BluetoothAuthenticationType type)
        {
            return PlatformPairRequest(device, pin, type);
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

        public enum BluetoothAuthenticationType : int // MSFT+Win32 AUTHENTICATION_REQUIREMENTS
        {
            /// <summary>
            /// Protection against a "Man in the Middle" attack is not required for authentication.
            /// </summary>
            MITMProtectionNotRequired = 0,

            /// <summary>
            /// Protection against a "Man in the Middle" attack is required for authentication.
            /// </summary>
            MITMProtectionRequired = 0x1,

            /// <summary>
            /// Protection against a "Man in the Middle" attack is not required for bonding.
            /// </summary>
            MITMProtectionNotRequiredBonding = 0x2,

            /// <summary>
            /// Protection against a "Man in the Middle" attack is required for bonding.
            /// </summary>
            MITMProtectionRequiredBonding = 0x3,

            /// <summary>
            /// Protection against a "Man in the Middle" attack is not required for General Bonding.
            /// </summary>
            MITMProtectionNotRequiredGeneralBonding = 0x4,

            /// <summary>
            /// Protection against a "Man in the Middle" attack is required for General Bonding.
            /// </summary>
            MITMProtectionRequiredGeneralBonding = 0x5,

            /// <summary>
            /// Protection against "Man in the Middle" attack is not defined.
            /// </summary>
            MITMProtectionNotDefined = 0xff
        }
    }
}
