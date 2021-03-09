// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.RegisterDeviceNotificationSafeHandle
// 
// Copyright (c) 2008-2021 In The Hand Ltd, All rights reserved.
// Copyright (c) 2008-2011 Alan J.McFarlane, All rights reserved.
// This source code is licensed under the MIT License

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal sealed class RegisterDeviceNotificationSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public RegisterDeviceNotificationSafeHandle() : base(true) // ownsHandle
        {
        }

        protected override bool ReleaseHandle()
        {
            return BluetoothUnregisterDeviceNotification(handle);
        }

        private static bool BluetoothUnregisterDeviceNotification(IntPtr deviceNotificationHandle)
        {
            System.Diagnostics.Debug.Assert(deviceNotificationHandle != IntPtr.Zero, "Device notification not registered.");
            bool success = UnregisterDeviceNotification(deviceNotificationHandle);
            System.Diagnostics.Debug.Assert(success, "UnregisterDeviceNotification success false.");
            return success;
        }

        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);
    }
}
