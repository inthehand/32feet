// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Win32)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothSecurity
    {
        private static List<Win32BluetoothAuthentication> _authenticationHandlers = new List<Win32BluetoothAuthentication>();

        static bool DoPairRequest(BluetoothAddress device, string pin)
        {
            if (pin.Length > BLUETOOTH_PIN_INFO.BTH_MAX_PIN_SIZE)
                throw new ArgumentOutOfRangeException(nameof(pin));

            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = Marshal.SizeOf(info);
            info.Address = device;

            RemoveRedundantAuthHandler(device);

            // Handle response without prompt
            _authenticationHandlers.Add(new Win32BluetoothAuthentication(device, pin));

            bool success = NativeMethods.BluetoothAuthenticateDeviceEx(IntPtr.Zero, IntPtr.Zero, ref info, null, BluetoothAuthenticationRequirements.MITMProtectionNotRequired) == 0;

            BluetoothDeviceInfo deviceInfo = new BluetoothDeviceInfo(info);
            deviceInfo.Refresh();

            // On Windows 7 these services are not automatically activated
            if(deviceInfo.ClassOfDevice.Device == DeviceClass.AudioVideoHeadset || deviceInfo.ClassOfDevice.Device == DeviceClass.AudioVideoHandsFree)
            {
                deviceInfo.SetServiceState(BluetoothService.Headset, true);
                deviceInfo.SetServiceState(BluetoothService.Handsfree, true);
            }

            return success;
        }

        static void RemoveRedundantAuthHandler(ulong address)
        {
            Win32BluetoothAuthentication redundantAuth = null;

            foreach (Win32BluetoothAuthentication authHandler in _authenticationHandlers)
            {
                if (authHandler.Address == address)
                {
                    redundantAuth = authHandler;
                    redundantAuth.Dispose();
                    break;
                }
            }

            if (redundantAuth != null)
            {
                _authenticationHandlers.Remove(redundantAuth);
            }
        }

        static bool DoRemoveDevice(BluetoothAddress device)
        {
            ulong addr = device;
            RemoveRedundantAuthHandler(addr);
            return NativeMethods.BluetoothRemoveDevice(ref addr) == 0;
        }
    }
}