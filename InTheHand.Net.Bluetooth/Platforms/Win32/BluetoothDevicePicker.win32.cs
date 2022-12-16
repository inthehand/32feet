// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Win32)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothDevicePicker
    {
        private Task<BluetoothDeviceInfo> DoPickSingleDeviceAsync()
        {
            BluetoothDeviceInfo info = null;

            BLUETOOTH_SELECT_DEVICE_PARAMS p = new BLUETOOTH_SELECT_DEVICE_PARAMS();
            p.Reset();
            p.SetClassOfDevices(ClassOfDevices.ToArray());
            p.fForceAuthentication = RequireAuthentication;
            p.hwndParent = NativeMethods.GetActiveWindow();
            if (p.hwndParent == IntPtr.Zero)
                p.hwndParent = NativeMethods.GetConsoleWindow();

            bool success = NativeMethods.BluetoothSelectDevices(ref p);

            if (success)
            {
                info = new BluetoothDeviceInfo(p.Device);
            }

            return Task.FromResult(info);
        }
    }
}
