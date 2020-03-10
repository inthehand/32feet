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
    [StructLayout(LayoutKind.Sequential)]
    internal struct WSAQUERYSET
    {
        public uint dwSize;
        string lpszServiceInstanceName;
        public Guid lpServiceClassId;
        IntPtr lpVersion;
        IntPtr lpszComment;
        public uint dwNameSpace; //NS_BTH
        IntPtr lpNSProviderId;
        IntPtr lpszContext;
        uint dwNumberOfProtocols;
        IntPtr lpafpProtocols;
        string lpszQueryString;
        uint dwNumberOfCsAddrs;
        IntPtr lpcsaBuffer;
        uint dwOutputFlags;
        [MarshalAs(UnmanagedType.LPStruct)]
        public BLOB lpBlob;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BLOB
    {
        public uint size;
        public BTH_SET_SERVICE blobData;
    }
}
