// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Contains functionality to pair (and un-pair) Bluetooth devices.
    /// </summary>
    public sealed class BluetoothSecurity
    {
        readonly static IBluetoothSecurity _bluetoothSecurity = null;

        static BluetoothSecurity()
        {
#if ANDROID || MONOANDROID
            _bluetoothSecurity = new AndroidBluetoothSecurity();
#elif IOS || __IOS__
            _bluetoothSecurity = new ExternalAccessoryBluetoothSecurity();
#elif WINDOWS_UWP || WINDOWS10_0_17763_0_OR_GREATER
            _bluetoothSecurity = new WindowsBluetoothSecurity();
#elif NET462 || WINDOWS7_0_OR_GREATER
            _bluetoothSecurity = new Win32BluetoothSecurity();
#elif NETSTANDARD
#else
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    _bluetoothSecurity = new LinuxBluetoothSecurity();
                    break;
                case PlatformID.Win32NT:
                    //detect Windows 10
                    _bluetoothSecurity = new Win32BluetoothSecurity();
                    break;
            }
#endif
            if (_bluetoothSecurity == null)
                throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Requests the pairing process for the specified device with the provided pin or numeric code.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="requireMitmProtection">MITM not required only if set to false.</param>
        /// <param name="pin">Optional numeric pin.</param>
        /// <returns></returns>
        public static bool PairRequest(BluetoothAddress device, string pin, bool? requireMitmProtection)
        {
            return _bluetoothSecurity.PairRequest(device, pin, requireMitmProtection);
        }

        public static bool PairRequest(BluetoothAddress device, string pin = null)
        {
            return _bluetoothSecurity.PairRequest(device, pin, null);
        }        

        /// <summary>
        /// Requests that the specified device is un-paired.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool RemoveDevice(BluetoothAddress device)
        {
            return _bluetoothSecurity.RemoveDevice(device);
        }
    }
}
