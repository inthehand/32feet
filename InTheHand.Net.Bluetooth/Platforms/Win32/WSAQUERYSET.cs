// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.WSAQuerySet
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WSAQUERYSET
    {
        public int dwSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszServiceInstanceName;
        //[MarshalAs(UnmanagedType.LPStruct)]
        public IntPtr lpServiceClassId;
        IntPtr lpVersion;
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpszComment;
        public uint dwNameSpace; // NS_BTH
        IntPtr lpNSProviderId;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszContext;
        uint dwNumberOfProtocols;
        IntPtr lpafpProtocols;
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpszQueryString;
        public uint dwNumberOfCsAddrs;
        public IntPtr lpcsaBuffer;
        uint dwOutputFlags;
        //[MarshalAs(UnmanagedType.LPStruct)]
        public IntPtr lpBlob;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BLOB
    {
        public int size;
        public IntPtr blobData;
    }
}
