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
    [StructLayout(LayoutKind.Sequential)]
    internal struct CSADDR_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public byte[] LocalAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public byte[] RemoteAddr;
        public SocketType iSocketType;
        public int iProtocol;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SOCKADDR_BTH
    {
        public ushort addressFamily;
        public ulong btAddr;
        public Guid serviceClassId;
        public uint port;
    }
}
