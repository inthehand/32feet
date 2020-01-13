using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth.Win32;

namespace InTheHand.Net.Bluetooth
{
    internal abstract class SocketBluetoothClient
    {
        private Socket _socket;

        public abstract Win32BluetoothDeviceInfo[] DiscoverDevices(int maxDevices, bool authenticated, bool remembered, bool unknown);
        
        public void Connect(BluetoothAddress address, Guid service)
        {
            _socket = new Socket((AddressFamily)32, SocketType.Stream, (ProtocolType)3);
            _socket.Connect(new BluetoothEndPoint(address, service));
        }

        public void Close()
        {
            _socket?.Close();
        }

        private bool _authenticate;

        public bool Authenticate
        {
            get
            {
                return _authenticate;
            }

            set
            {
                if (_authenticate != value)
                {
                    _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameAuthenticate, value);
                    _authenticate = value;
                }
            }
        }

        public bool Connected
        {
            get
            {
                if (_socket == null)
                    return false;

                return _socket.Connected;
            }
        }

        private bool _encrypt = false;

        public bool Encrypt
        {
            get
            {
                return _encrypt;
            }

            set
            {
                if (_encrypt != value)
                {
                    _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameEncrypt, value ? 1 : 0);
                    _encrypt = value;
                }
            }
        }

        private const SocketOptionLevel SocketOptionLevelRFComm = (SocketOptionLevel)0x03;
        private const SocketOptionName SocketOptionNameAuthenticate = unchecked((SocketOptionName)0x80000001);
        private const SocketOptionName SocketOptionNameEncrypt = (SocketOptionName)0x00000002;

        public string RemoteMachineName
        {
            get
            {
                if (Connected)
                {
                    var remote = _socket.RemoteEndPoint as BluetoothEndPoint;
                    var info = Win32.BLUETOOTH_DEVICE_INFO.Create();
                    info.Address = remote.Address;
                    Win32.NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref info);
                    return info.szName;
                }

                return string.Empty;
            }
        }


        public NetworkStream GetStream()
        {
            if (Connected)
                return new NetworkStream(_socket, true);

            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _socket?.Close();
                    _socket?.Dispose();
                    _socket = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
