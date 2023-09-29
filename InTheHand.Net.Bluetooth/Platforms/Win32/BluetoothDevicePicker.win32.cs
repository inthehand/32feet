// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Win32)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class Win32BluetoothDevicePicker : IBluetoothDevicePicker
    {
        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync(List<ClassOfDevice> classOfDevices, bool requiresAuthentication)
        {
            BluetoothDeviceInfo info = null;

            BLUETOOTH_SELECT_DEVICE_PARAMS p = new BLUETOOTH_SELECT_DEVICE_PARAMS();
            p.Reset();
            if(classOfDevices != null &&  classOfDevices.Count > 0 )
            {
                p.SetClassOfDevices(classOfDevices.ToArray());          
            }
            p.fForceAuthentication = requiresAuthentication;
            p.hwndParent = NativeMethods.GetActiveWindow();
            if (p.hwndParent == IntPtr.Zero)
                p.hwndParent = NativeMethods.GetConsoleWindow();

            bool success = NativeMethods.BluetoothSelectDevices(ref p);

            if (success)
            {
                info = new Win32BluetoothDeviceInfo(p.Device);
            }

            return Task.FromResult(info);
        }
    }
}
