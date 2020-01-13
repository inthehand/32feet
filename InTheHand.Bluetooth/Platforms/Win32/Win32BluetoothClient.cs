// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Win32)
// 
// Copyright (c) 2003-2019 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal sealed class Win32BluetoothClient : SocketBluetoothClient
    {
        public override Win32BluetoothDeviceInfo[] DiscoverDevices(int maxDevices, bool authenticated, bool remembered, bool unknown)
        {
            List<Win32BluetoothDeviceInfo> devices = new List<Win32BluetoothDeviceInfo>();
            BLUETOOTH_DEVICE_SEARCH_PARAMS search = BLUETOOTH_DEVICE_SEARCH_PARAMS.Create();
            search.cTimeoutMultiplier = 8;
            search.fReturnAuthenticated = authenticated;
            search.fReturnRemembered = remembered;
            search.fReturnUnknown = unknown;
            search.fReturnConnected = true;
            search.fIssueInquiry = unknown;

            BLUETOOTH_DEVICE_INFO device = BLUETOOTH_DEVICE_INFO.Create();
            IntPtr searchHandle = NativeMethods.BluetoothFindFirstDevice(ref search, ref device);
            if(searchHandle != IntPtr.Zero)
            {
                devices.Add(new Win32BluetoothDeviceInfo(device));

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device) && devices.Count <= maxDevices)
                {
                    devices.Add(new Win32BluetoothDeviceInfo(device));
                }

                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }
            return devices.ToArray();
        }
    }
}
