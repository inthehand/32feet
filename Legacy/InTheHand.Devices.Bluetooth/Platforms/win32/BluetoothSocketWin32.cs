//-----------------------------------------------------------------------
// <copyright file="BluetoothSocket.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Bluetooth;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class BluetoothSocket : IDisposable
    {
        private int _socket = 0;

        /// <summary>
        /// 
        /// </summary>
        public BluetoothSocket()
        {
            _socket = NativeMethods.socket(BluetoothSockets.BluetoothAddressFamily, SocketType.Stream, BluetoothSockets.RfcommProtocolType);
        }

        internal BluetoothSocket(int socket)
        {
            _socket = socket;
        }

        private void ThrowIfSocketClosed()
        {
            if (_socket == 0)
                throw new ObjectDisposedException("BluetoothSocket");
        }

        public BluetoothSocket Accept()
        {
            ThrowIfSocketClosed();

            int newSocket = NativeMethods.accept(_socket, IntPtr.Zero, IntPtr.Zero);

            if (newSocket < 0)
                throw new SocketException(NativeMethods.WSAGetLastError());

            return new BluetoothSocket(newSocket);
        }

        public void Bind(System.Net.EndPoint localEP)
        {
            ThrowIfSocketClosed();

            if (localEP == null)
                throw new ArgumentNullException("localEP");

            var sockAddr = localEP.Serialize();

            int result = NativeMethods.bind(_socket, SocketAddressToArray(sockAddr), sockAddr.Size);

            if(result < 0)
                throw new SocketException(NativeMethods.WSAGetLastError());
        }

        private static byte[] SocketAddressToArray(System.Net.SocketAddress socketAddress)
        {
            byte[] buffer = new byte[socketAddress.Size+1];
            buffer[0] = (byte)socketAddress.Family;
            for(int i = 1; i <buffer.Length; i++)
            {
                buffer[i] = socketAddress[i];
            }

            return buffer;
        }

        public void Close()
        {
            if (_socket != 0)
            {
                int result = NativeMethods.closesocket(_socket);
                _socket = 0;
            }
        }

        public void Connect(global::System.Net.EndPoint remoteEP)
        {
            ThrowIfSocketClosed();

            if (remoteEP == null)
                throw new ArgumentNullException("remoteEP");
            
            var sockAddr = remoteEP.Serialize();

            int result = NativeMethods.connect(_socket, SocketAddressToArray(sockAddr), sockAddr.Size);

            if (result < 0)
                throw new SocketException(NativeMethods.WSAGetLastError());
        }

        public int Receive(byte[] buffer)
        {
            return Receive(buffer, buffer.Length, 0);
        }

        public int Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return Receive(buffer, buffer.Length, socketFlags);
        }

        public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            ThrowIfSocketClosed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (size > buffer.Length)
                throw new ArgumentOutOfRangeException("size");

            int result = NativeMethods.recv(_socket, buffer, size, (int)socketFlags);

            if (result < 0)
                throw new SocketException(NativeMethods.WSAGetLastError());

            return result;
        }

        public int Send(byte[] buffer)
        {
            return Send(buffer, buffer.Length, 0);
        }

        public int Send(byte[] buffer, SocketFlags socketFlags)
        {
            return Send(buffer, buffer.Length, socketFlags);
        }

        public int Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            ThrowIfSocketClosed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (size > buffer.Length)
                throw new ArgumentOutOfRangeException("size");

            int result = NativeMethods.send(_socket, buffer, size, (int)socketFlags);

            if (result < 0)
                throw new SocketException(NativeMethods.WSAGetLastError());

            return result;
        }

        public int Available
        {
            get
            {
                byte[] outVal = new byte[4];
                NativeMethods.ioctlsocket(_socket, NativeMethods.FIONREAD, outVal);
                return BitConverter.ToInt32(outVal, 0);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BluetoothSocket()
        {
            Dispose(false);
        }

        private static class NativeMethods
        {
            private const string winsockDll = "ws2_32.dll";
            internal const int FIONREAD = 0x4004667F;

            [DllImport(winsockDll)]
            internal static extern int socket(AddressFamily af, SocketType type, ProtocolType protocol);

            [DllImport(winsockDll)]
            internal static extern int closesocket(int s);

            [DllImport(winsockDll)]
            internal static extern int connect(int s, byte[] name, int namelen);

            [DllImport(winsockDll)]
            internal static extern int recv(int s, byte[] buf, int len, int flags);

            [DllImport(winsockDll)]
            internal static extern int send(int s, byte[] buf, int len, int flags);


            [DllImport(winsockDll)]
            internal static extern int bind(int s, byte[] name, int namelen);

            [DllImport(winsockDll)]
            internal static extern int accept(int s, IntPtr addr, IntPtr addrlen);

            [DllImport(winsockDll)]
            internal static extern int WSAGetLastError();

            [DllImport(winsockDll)]
            internal static extern int ioctlsocket(int s, int cmd, byte[] argp);
        }
    }
}