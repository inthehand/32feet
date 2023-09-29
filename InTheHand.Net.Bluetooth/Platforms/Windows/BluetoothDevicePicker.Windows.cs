// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (WinRT)
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using Windows.Devices.Enumeration;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Bluetooth;
using System.Collections.Generic;
#if WinRT
using System.Diagnostics;
using System.Runtime.InteropServices;
#endif

namespace InTheHand.Net.Bluetooth
{
    internal sealed class WindowsBluetoothDevicePicker : IBluetoothDevicePicker
    {
        private DevicePicker picker = new DevicePicker();
            
        public async Task<BluetoothDeviceInfo> PickSingleDeviceAsync(List<ClassOfDevice> classOfDevices, bool requiresAuthentication)
        {
            Rect bounds = new Rect(0, 0, 0, 0);
            var window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
            if (window != null)
            {
                bounds = window.Bounds;
            }
            else
            {
#if WinRT
                var hwnd = NativeMethods.GetActiveWindow();
                if(hwnd == IntPtr.Zero)
                    hwnd = NativeMethods.GetConsoleWindow();

                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
#endif
            }

            picker.Filter.SupportedDeviceSelectors.Add(requiresAuthentication ? BluetoothDevice.GetDeviceSelectorFromPairingState(true) : BluetoothDevice.GetDeviceSelector());
            var deviceInfo = await picker.PickSingleDeviceAsync(bounds);

            if (deviceInfo == null)
                return null;

            var device = await BluetoothDevice.FromIdAsync(deviceInfo.Id);
            var access = await device.RequestAccessAsync();

            return new WindowsBluetoothDeviceInfo(device);
        }

#if WinRT
        internal static class NativeMethods
        {
            [DllImport("user32", ExactSpelling = true, SetLastError = true)]
            internal static extern IntPtr GetActiveWindow();

            [DllImport("kernel32")]
            public static extern IntPtr GetConsoleWindow();
        }
#endif
    }
}
