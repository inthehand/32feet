// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Win32)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    internal sealed class Win32BluetoothListener : IBluetoothListener
    {
        private BluetoothEndPoint endPoint;
        private Socket socket;
        private IntPtr handle = IntPtr.Zero;

        public bool Active { get; private set; }

        public ServiceClass ServiceClass { get; set; }
        public string ServiceName { get; set; }
        public ServiceRecord ServiceRecord { get; set; }
        public Guid ServiceUuid { get; set; }

        public void Start()
        {
            if (handle != IntPtr.Zero)
                throw new InvalidOperationException();

            endPoint = new BluetoothEndPoint(BluetoothAddress.None, ServiceUuid, NativeMethods.BTH_PORT_ANY);

            if (NativeMethods.IsRunningOnMono())
            {
                socket = new Win32Socket();
                ((Win32Socket)socket).Bind(endPoint);
                Debug.WriteLine(socket.IsBound);
                ((Win32Socket)socket).Listen(1);
                // get endpoint with channel
                endPoint = ((Win32Socket)socket).LocalEndPoint as BluetoothEndPoint;
            }
            else
            {
                socket = new Socket(Win32BluetoothClient.AddressFamilyBluetooth, SocketType.Stream, BluetoothProtocolType.RFComm);
                socket.Bind(endPoint);
                Debug.WriteLine(socket.IsBound);
                socket.Listen(1);
                // get endpoint with channel
                endPoint = socket.LocalEndPoint as BluetoothEndPoint;
            }

            var socketAddressBytes = endPoint.Serialize().ToByteArray();
            

            WSAQUERYSET qs = new WSAQUERYSET();
            qs.dwSize = Marshal.SizeOf(qs);
            qs.dwNameSpace = NativeMethods.NS_BTH;
            //var uuidh = GCHandle.Alloc(serviceUuid, GCHandleType.Pinned);
            //qs.lpServiceClassId = uuidh.AddrOfPinnedObject();
            //qs.lpszServiceInstanceName = ServiceName;

            qs.dwNumberOfCsAddrs = 1;

            var csa = new CSADDR_INFO
            {
                lpLocalSockaddr = Marshal.AllocHGlobal(NativeMethods.BluetoothSocketAddressLength),
                iLocalSockaddrLength = NativeMethods.BluetoothSocketAddressLength,
                iSocketType = SocketType.Stream,
                iProtocol = NativeMethods.PROTOCOL_RFCOMM
            };
            Marshal.Copy(socketAddressBytes, 0, csa.lpLocalSockaddr, NativeMethods.BluetoothSocketAddressLength);

            IntPtr pcsa = Marshal.AllocHGlobal(Marshal.SizeOf(csa));
            Marshal.StructureToPtr(csa, pcsa, false);
            qs.lpcsaBuffer = pcsa;

            var blob = new BLOB();
            var blobData = new BTH_SET_SERVICE();
            int sdpVer = 1;

            blobData.pSdpVersion = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteInt32(blobData.pSdpVersion, sdpVer);
            blobData.pRecordHandle = Marshal.AllocHGlobal(IntPtr.Size);
            Marshal.WriteIntPtr(blobData.pRecordHandle, 0, IntPtr.Zero);
            blobData.fCodService = ServiceClass;

            if (ServiceRecord == null)
            {
                ServiceRecordBuilder builder = new ServiceRecordBuilder();
                builder.AddServiceClass(ServiceUuid);
                builder.ProtocolType = BluetoothProtocolDescriptorType.Rfcomm;

                if (!string.IsNullOrEmpty(ServiceName))
                    builder.ServiceName = ServiceName;

                ServiceRecord = builder.ServiceRecord;
            }
            ServiceRecordHelper.SetRfcommChannelNumber(ServiceRecord, socketAddressBytes[26]);

            byte[] rawBytes = ServiceRecord.ToByteArray();
            blobData.ulRecordLength = (uint)rawBytes.Length;
            int structSize = (2 * IntPtr.Size) + (7 * 4);
            blob.size = structSize + rawBytes.Length;
            blob.blobData = Marshal.AllocHGlobal(blob.size);
            Marshal.StructureToPtr(blobData, blob.blobData, false);
            Marshal.Copy(rawBytes, 0, IntPtr.Add(blob.blobData, structSize), rawBytes.Length);

            var blobh = GCHandle.Alloc(blob, GCHandleType.Pinned);
            qs.lpBlob = blobh.AddrOfPinnedObject();

            try
            {
                int result = NativeMethods.WSASetService(ref qs, WSAESETSERVICEOP.RNRSERVICE_REGISTER, 0);

                var newstruct = Marshal.PtrToStructure<BTH_SET_SERVICE>(blob.blobData);
                handle = Marshal.ReadIntPtr(newstruct.pRecordHandle);
                if (result == -1)
                {
                    int werr = Marshal.GetLastWin32Error();
                    throw new SocketException(werr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(csa.lpLocalSockaddr);
                Marshal.FreeHGlobal(qs.lpcsaBuffer);

                Marshal.FreeHGlobal(blobData.pSdpVersion);
                Marshal.FreeHGlobal(blobData.pRecordHandle);

                Marshal.FreeHGlobal(blob.blobData);
                blobh.Free();
            }

            Active = true;
        }

        public void Stop()
        {
            Debug.WriteLine("BluetoothListener Stop");

            if (handle != IntPtr.Zero)
            {
                WSAQUERYSET qs = new WSAQUERYSET();
                qs.dwSize = Marshal.SizeOf(qs);
                qs.dwNameSpace = NativeMethods.NS_BTH;
                var blob = new BLOB();
                var blobData = new BTH_SET_SERVICE();
                int sdpVer = 1;

                blobData.pSdpVersion = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteInt32(blobData.pSdpVersion, sdpVer);
                blobData.pRecordHandle = Marshal.AllocHGlobal(IntPtr.Size);
                Marshal.WriteIntPtr(blobData.pRecordHandle, 0, handle);

                blob.size = (2 * IntPtr.Size) + (7 * 4) + 1;
                blob.blobData = Marshal.AllocHGlobal(blob.size);
                Marshal.StructureToPtr(blobData, blob.blobData, false);

                var blobh = GCHandle.Alloc(blob, GCHandleType.Pinned);
                qs.lpBlob = blobh.AddrOfPinnedObject();

                /*qs.lpBlob = new BLOB();
                qs.lpBlob.blobData = new BTH_SET_SERVICE();
                qs.lpBlob.size = Marshal.SizeOf(qs.lpBlob.blobData);
                qs.lpBlob.blobData.pRecordHandle = handle;
                qs.lpBlob.blobData.pSdpVersion = 1;*/
                try
                {
                    int result = NativeMethods.WSASetService(ref qs, WSAESETSERVICEOP.RNRSERVICE_DELETE, 0);
                    handle = IntPtr.Zero;
                    if (result == -1)
                    {
                        int werr = Marshal.GetLastWin32Error();
                        throw new SocketException(werr);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(blobData.pSdpVersion);
                    Marshal.FreeHGlobal(blobData.pRecordHandle);

                    Marshal.FreeHGlobal(blob.blobData);
                    blobh.Free();
                }
            }

            if (NativeMethods.IsRunningOnMono())
            {
                ((Win32Socket)socket).Close();
            }
            else
            {
                socket.Close();
            }

            socket = null;

            Active = false;
        }

        public bool Pending()
        {
            if (NativeMethods.IsRunningOnMono())
            {
                return ((Win32Socket)socket).Poll(0, SelectMode.SelectRead);
            }
            else
            {
                return socket.Poll(0, SelectMode.SelectRead);
            }
        }

        public Socket AcceptSocket()
        {
            if (NativeMethods.IsRunningOnMono())
            {
                return ((Win32Socket)socket).Accept();
            }
            else
            {
                return socket.Accept();
            }
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            return new BluetoothClient(new Win32BluetoothClient(AcceptSocket()));
        }
    }
}
