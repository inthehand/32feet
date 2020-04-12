// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.CSADDR_INFO
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    internal struct CSADDR_INFO
    {
        public IntPtr lpLocalSockaddr;
        public int iLocalSockaddrLength;
        public IntPtr lpRemoteSockaddr;
        public int iRemoteSockaddrLength;
        [MarshalAs(UnmanagedType.I4)]
        public SocketType iSocketType;
        public int iProtocol;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SOCKET_ADDRESS
    {
        [MarshalAs(UnmanagedType.LPStruct)]
        public SOCKADDR_BTH lpSockaddr;
        public int iSockaddrLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SOCKADDR_BTH
    {
        public ushort addressFamily;
        public ulong btAddr;
        public Guid serviceClassId;
        public int port;
    }
}
