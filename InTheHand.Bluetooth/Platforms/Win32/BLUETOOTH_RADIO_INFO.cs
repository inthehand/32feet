// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.BLUETOOTH_RADIO_INFO
// 
// Copyright (c) 2003-2019 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    internal struct BLUETOOTH_RADIO_INFO
    {
        private const int BLUETOOTH_MAX_NAME_SIZE = 248;

        internal int dwSize;
        internal ulong address;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
        internal string szName;
        internal uint ulClassofDevice;
        internal ushort lmpSubversion;
        [MarshalAs(UnmanagedType.U2)]
        internal ushort manufacturer;

        public static BLUETOOTH_RADIO_INFO Create()
        {
            BLUETOOTH_RADIO_INFO r = new BLUETOOTH_RADIO_INFO();
            r.dwSize = Marshal.SizeOf(r);
            return r;
        }
    }
}