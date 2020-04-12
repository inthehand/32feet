// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.BTH_SET_SERVICE
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BTH_SET_SERVICE
    {
        public IntPtr pSdpVersion;
        public IntPtr pRecordHandle;
        [MarshalAs(UnmanagedType.I4)]
        public ServiceClass fCodService;
        uint Reserved1;
        uint Reserved2;
        uint Reserved3;
        uint Reserved4;
        uint Reserved5;
        public uint ulRecordLength;
        public IntPtr pRecord;
    }
}
