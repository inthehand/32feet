// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.WSAQuerySet
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    [StructLayout(LayoutKind.Sequential, Size = 60)]
    internal struct WSAQUERYSET
    {
        public int dwSize;
        public string lpszServiceInstanceName;
        [MarshalAs(UnmanagedType.Struct)]
        public Guid lpServiceClassId;
        IntPtr lpVersion;
        string lpszComment;
        public uint dwNameSpace; // NS_BTH
        IntPtr lpNSProviderId;
        IntPtr lpszContext;
        uint dwNumberOfProtocols;
        IntPtr lpafpProtocols;
        string lpszQueryString;
        public uint dwNumberOfCsAddrs;
        [MarshalAs(UnmanagedType.Struct)]
        public CSADDR_INFO lpcsaBuffer;
        uint dwOutputFlags;
        [MarshalAs(UnmanagedType.Struct)]
        public BLOB lpBlob;
    }

    [StructLayout(LayoutKind.Sequential, Size = 8)]
    internal struct BLOB
    {
        public int size;
        [MarshalAs(UnmanagedType.LPStruct)]
        public BTH_SET_SERVICE blobData;
    }
}
