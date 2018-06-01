using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class MonoBluetoothSocket : IDisposable
    {
        private int _socket = 0;

        /// <summary>
        /// 
        /// </summary>
        public MonoBluetoothSocket()
        {
            //AF_BT, Type_Stream, Protocol_Rfcomm
            _socket = NativeMethods.socket(32, 1, 3);
        }

        internal MonoBluetoothSocket(int socket)
        {
            _socket = socket;
        }

        private void ThrowIfSocketClosed()
        {
            if (_socket == 0)
                throw new ObjectDisposedException("MonoBluetoothSocket");
        }

        public MonoBluetoothSocket Accept()
        {
            ThrowIfSocketClosed();

            int newSocket = NativeMethods.accept(_socket, IntPtr.Zero, IntPtr.Zero);

            if (newSocket > 0)
                throw new global::System.Net.Sockets.SocketException(NativeMethods.WSAGetLastError());

            return new MonoBluetoothSocket(newSocket);
        }

        public void Bind(global::System.Net.EndPoint localEP)
        {
            ThrowIfSocketClosed();

            if (localEP == null)
                throw new ArgumentNullException("localEP");

            var sockAddr = localEP.Serialize();

            int result = NativeMethods.bind(_socket, SocketAddressToArray(sockAddr), sockAddr.Size);

            if(result < 0)
                throw new global::System.Net.Sockets.SocketException(NativeMethods.WSAGetLastError());
        }

        private static byte[] SocketAddressToArray(global::System.Net.SocketAddress socketAddress)
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
                throw new global::System.Net.Sockets.SocketException(NativeMethods.WSAGetLastError());
        }

        public int Receive(byte[] buffer)
        {
            return Receive(buffer, buffer.Length, 0);
        }

        public int Receive(byte[] buffer, global::System.Net.Sockets.SocketFlags socketFlags)
        {
            return Receive(buffer, buffer.Length, socketFlags);
        }

        public int Receive(byte[] buffer, int size, global::System.Net.Sockets.SocketFlags socketFlags)
        {
            ThrowIfSocketClosed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (size > buffer.Length)
                throw new ArgumentOutOfRangeException("size");

            int result = NativeMethods.recv(_socket, buffer, size, (int)socketFlags);

            if (result < 0)
                throw new global::System.Net.Sockets.SocketException(NativeMethods.WSAGetLastError());

            return result;
        }

        public int Send(byte[] buffer)
        {
            return Send(buffer, buffer.Length, 0);
        }

        public int Send(byte[] buffer, global::System.Net.Sockets.SocketFlags socketFlags)
        {
            return Send(buffer, buffer.Length, socketFlags);
        }

        public int Send(byte[] buffer, int size, global::System.Net.Sockets.SocketFlags socketFlags)
        {
            ThrowIfSocketClosed();

            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (size > buffer.Length)
                throw new ArgumentOutOfRangeException("size");

            int result = NativeMethods.send(_socket, buffer, size, (int)socketFlags);

            if (result < 0)
                throw new global::System.Net.Sockets.SocketException(NativeMethods.WSAGetLastError());

            return result;
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

        ~MonoBluetoothSocket()
        {
            Dispose(false);
        }

        private static class NativeMethods
        {
            private const string winsockDll = "ws2_32.dll";

            [DllImport(winsockDll)]
            internal static extern int socket(int af, int type, int protocol);

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

        }
    }
}