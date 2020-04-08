// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Win32)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        private BluetoothEndPoint endPoint;
        private Win32Socket socket;
        private IntPtr handle = IntPtr.Zero;

        void DoStart()
        {
            if (handle != IntPtr.Zero)
                throw new InvalidOperationException();

            endPoint = new BluetoothEndPoint(0, serviceUuid);
            socket = new Win32Socket();
            socket.Bind(endPoint);
            socket.Listen(1);

            WSAQUERYSET qs = new WSAQUERYSET();
            //qs.dwSize = Marshal.SizeOf(qs);
            qs.dwNameSpace = NativeMethods.NS_BTH;

            qs.lpServiceClassId = serviceUuid;
            qs.lpszServiceInstanceName = ServiceName;

            qs.dwNumberOfCsAddrs = 1;
            qs.lpcsaBuffer = new CSADDR_INFO
            {
                LocalAddr = socket.LocalEndPointRaw,
                iSocketType = SocketType.Stream,
                iProtocol = NativeMethods.PROTOCOL_RFCOMM
            };
            int sb = Marshal.SizeOf(qs.lpcsaBuffer);

            qs.lpBlob = new BLOB();
            qs.lpBlob.blobData = new BTH_SET_SERVICE();
            qs.lpBlob.size = Marshal.SizeOf(qs.lpBlob.blobData);
            qs.lpBlob.blobData.pSdpVersion = 1;
            qs.lpBlob.blobData.fCodService = ServiceClass;
            int result = NativeMethods.WSASetService(ref qs, WSAESETSERVICEOP.RNRSERVICE_DELETE, 0);

        }

        void DoStop()
        {
            if(handle != IntPtr.Zero)
            {
                WSAQUERYSET qs = new WSAQUERYSET();
                qs.dwSize = Marshal.SizeOf(qs);
                qs.dwNameSpace = NativeMethods.NS_BTH;
                qs.lpBlob = new BLOB();
                qs.lpBlob.blobData = new BTH_SET_SERVICE();
                qs.lpBlob.size = Marshal.SizeOf(qs.lpBlob.blobData);
                qs.lpBlob.blobData.pRecordHandle = handle;
                qs.lpBlob.blobData.pSdpVersion = 1;
                int result = NativeMethods.WSASetService(ref qs, WSAESETSERVICEOP.RNRSERVICE_DELETE, 0);
            }
        }

        bool DoPending()
        {
            return socket.Poll(0, SelectMode.SelectRead);
        }

        BluetoothClient DoAcceptBluetoothClient()
        {
            var s = socket.Accept();

            return new BluetoothClient(s);
        }

    }
}
