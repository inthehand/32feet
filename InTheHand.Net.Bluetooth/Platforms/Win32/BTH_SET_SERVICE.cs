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
        public uint pSdpVersion;
        public IntPtr pRecordHandle;
        public ServiceClass fCodService;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst =5)]
        uint[] Reserved;
        public uint ulRecordLength;
        public IntPtr pRecord;
    }
}
