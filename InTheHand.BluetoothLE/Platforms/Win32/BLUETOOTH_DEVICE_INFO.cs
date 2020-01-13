// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.BLUETOOTH_DEVICE_INFO
// 
// Copyright (c) 2003-2019 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;
using InTheHand.Win32;
using System.Text;
using System.Diagnostics;

namespace InTheHand.Net.Bluetooth.Win32
{

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    internal struct BLUETOOTH_DEVICE_INFO
    {
        internal int dwSize;
        internal ulong Address;
        internal uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fAuthenticated;
        internal SYSTEMTIME stLastSeen;
        internal SYSTEMTIME stLastUsed;  
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=248)]
        internal string szName;

        public static BLUETOOTH_DEVICE_INFO Create()
        {
            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = Marshal.SizeOf(info);
            return info;
        }

        public BLUETOOTH_DEVICE_INFO(long address)
        {
            dwSize = 560;
            Address = (ulong)address;
            ulClassofDevice = 0;
            fConnected = false;
            fRemembered = false;
            fAuthenticated = false;

            stLastSeen = new SYSTEMTIME();
            stLastUsed = new SYSTEMTIME();

            szName = "";
        }

        public BLUETOOTH_DEVICE_INFO(BluetoothAddress address)
        {
            dwSize = 560;
            Address = address ?? throw new ArgumentNullException("address");
            ulClassofDevice = 0;
            fConnected = false;
            fRemembered = false;
            fAuthenticated = false;

            stLastSeen = new SYSTEMTIME();
            stLastUsed = new SYSTEMTIME();

            szName = "";
        }

        internal DateTime LastSeen
        {
            get
            {
                return stLastSeen.ToDateTime(DateTimeKind.Utc);
            }
        }

        internal DateTime LastUsed
        {
            get
            {
                return stLastUsed.ToDateTime(DateTimeKind.Utc);
            }
        }
    }
}