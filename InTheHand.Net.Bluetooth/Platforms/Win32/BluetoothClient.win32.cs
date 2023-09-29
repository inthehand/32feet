// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Win32)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    internal sealed class Win32BluetoothClient : IBluetoothClient
    {
        internal const AddressFamily AddressFamilyBluetooth = (AddressFamily)32;
        private const SocketOptionLevel SocketOptionLevelRFComm = (SocketOptionLevel)0x03;
        private const SocketOptionName SocketOptionNameAuthenticate = unchecked((SocketOptionName)0x80000001);
        private const SocketOptionName SocketOptionNameEncrypt = (SocketOptionName)0x00000002;

        private Socket _socket;

        public Win32BluetoothClient()
        {
            if (NativeMethods.IsRunningOnMono())
            {
                _socket = new Win32Socket();
            }
            else
            {
                _socket = new Socket(AddressFamilyBluetooth, SocketType.Stream, BluetoothProtocolType.RFComm);
            }
        }

        internal Win32BluetoothClient(Socket s)
        {
            _socket = s;
        }

        public IEnumerable<BluetoothDeviceInfo> PairedDevices
        {
            get
            {
                BLUETOOTH_DEVICE_SEARCH_PARAMS search = BLUETOOTH_DEVICE_SEARCH_PARAMS.Create();
                search.cTimeoutMultiplier = 8;
                search.fReturnAuthenticated = true;
                search.fReturnRemembered = false;
                search.fReturnUnknown = false;
                search.fReturnConnected = false;
                search.fIssueInquiry = false;

                BLUETOOTH_DEVICE_INFO device = BLUETOOTH_DEVICE_INFO.Create();
                IntPtr searchHandle = NativeMethods.BluetoothFindFirstDevice(ref search, ref device);
                if (searchHandle != IntPtr.Zero)
                {
                    yield return new Win32BluetoothDeviceInfo(device);

                    while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device))
                    {
                        yield return new Win32BluetoothDeviceInfo(device);
                    }

                    NativeMethods.BluetoothFindDeviceClose(searchHandle);
                }

                yield break;
            }
        }

        public IReadOnlyCollection<BluetoothDeviceInfo> DiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            BLUETOOTH_DEVICE_SEARCH_PARAMS search = BLUETOOTH_DEVICE_SEARCH_PARAMS.Create();
            search.fReturnAuthenticated = false;
            search.fReturnRemembered = true;
            search.fReturnUnknown = true;
            search.fReturnConnected = true;
            search.fIssueInquiry = true;
            search.cTimeoutMultiplier = Convert.ToByte(InquiryLength.TotalSeconds / 1.28);
            
            BLUETOOTH_DEVICE_INFO device = BLUETOOTH_DEVICE_INFO.Create();
            IntPtr searchHandle = NativeMethods.BluetoothFindFirstDevice(ref search, ref device);
            if(searchHandle != IntPtr.Zero)
            {
                NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref device);
                devices.Add(new Win32BluetoothDeviceInfo(device));

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device) && devices.Count < maxDevices)
                {
                    NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref device);
                    devices.Add(new Win32BluetoothDeviceInfo(device));
                }

                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }

            // get full paired devices list and remove those not recently seen (they were added to the results above regardless)
            search.fReturnAuthenticated = true;
            search.fReturnRemembered = true;
            search.fReturnUnknown = false;
            search.fReturnConnected = false;
            search.fIssueInquiry = false;

            searchHandle = NativeMethods.BluetoothFindFirstDevice(ref search, ref device);
            if (searchHandle != IntPtr.Zero)
            {
                if (device.LastSeen < DateTime.Now.AddMinutes(-1))
                {
                    devices.Remove(new Win32BluetoothDeviceInfo(device));
                }

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device))
                {
                    if (device.LastSeen < DateTime.Now.AddMinutes(-1))
                    {
                        devices.Remove(new Win32BluetoothDeviceInfo(device));
                    }
                }

                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }

            return devices.AsReadOnly();
        }

#if NET6_0_OR_GREATER
        public async IAsyncEnumerable<BluetoothDeviceInfo> DiscoverDevicesAsync([EnumeratorCancellation]  CancellationToken cancellationToken)
        {
            foreach(var device in DiscoverDevices(255))
            {
                yield return device;
            }

            yield break;
        }
#endif

        public void Connect(BluetoothAddress address, Guid service)
        {
            var ep = new BluetoothEndPoint(address, service);

            Connect(ep);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        public void Connect(BluetoothEndPoint remoteEP)
        {
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            if (NativeMethods.IsRunningOnMono())
            {
                ((Win32Socket)_socket).Connect(remoteEP);
            }
            else
            {
                _socket.Connect(remoteEP);
            }
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        /// <param name="requestCallback">An <see cref="AsyncCallback"/> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the connect operation.
        /// This object is passed to the requestCallback delegate when the operation is complete.</param>
        /// <returns>An <see cref="IAsyncResult"/> object that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">The underlying Socket has been closed.</exception>
        /// <exception cref="PlatformNotSupportedException">Async Socket operations not currently supported on Mono</exception>
        public IAsyncResult BeginConnect(BluetoothEndPoint remoteEP, AsyncCallback requestCallback, object state)
        {
            if (NativeMethods.IsRunningOnMono())
                throw new PlatformNotSupportedException("Async Socket operations not currently supported on Mono");

            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            if (_socket == null)
                throw new ObjectDisposedException(nameof(Client));

            return _socket.BeginConnect(remoteEP, requestCallback, state);
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// The remote host is specified by a <see cref="BluetoothAddress"/> and a service UUID (Guid).
        /// </summary>
        /// <param name="address">The BluetoothAddress of the remote host.</param>
        /// <param name="service">The service UUID of the remote host.</param>
        /// <param name="requestCallback">An <see cref="AsyncCallback"/> delegate that references the method to invoke when the operation is complete.</param>
        /// <param name="state">A user-defined object that contains information about the connect operation.
        /// This object is passed to the requestCallback delegate when the operation is complete.</param>
        /// <returns>An <see cref="IAsyncResult"/> object that references the asynchronous connection.</returns>
        /// <exception cref="ObjectDisposedException">The underlying Socket has been closed.</exception>
        /// <exception cref="PlatformNotSupportedException">Async Socket operations not currently supported on Mono</exception>
        public IAsyncResult BeginConnect(BluetoothAddress address, Guid service, AsyncCallback requestCallback, object state)
        {
            var ep = new BluetoothEndPoint(address, service);
            return BeginConnect(ep, requestCallback, state);
        }

        /// <summary>
        /// Ends a pending asynchronous connection attempt.
        /// </summary>
        /// <param name="asyncResult">An IAsyncResult object returned by a call to <see cref="BeginConnect"/>.</param>
        /// <exception cref="ArgumentNullException">The asyncResult parameter is null.</exception>
        /// <exception cref="ObjectDisposedException">The underlying Socket has been closed.</exception>
        /// <exception cref="PlatformNotSupportedException">Async Socket operations not currently supported on Mono</exception>
        public void EndConnect(IAsyncResult asyncResult)
        {
            if (NativeMethods.IsRunningOnMono())
                throw new PlatformNotSupportedException("Async Socket operations not currently supported on Mono");

            if (_socket == null)
                throw new ObjectDisposedException(nameof(Client));

            if (asyncResult == null)
                throw new ArgumentNullException(nameof(asyncResult));

            _socket.EndConnect(asyncResult);
        }

        /// <summary>
        /// Connects the client to a remote Bluetooth host using the specified endpoint as an asynchronous operation.
        /// </summary>
        /// <param name="remoteEP">The <see cref="BluetoothEndPoint"/> to which you intend to connect.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public Task ConnectAsync(BluetoothEndPoint remoteEP)
        {
            if (NativeMethods.IsRunningOnMono())
                throw new PlatformNotSupportedException("Async Socket operations not currently supported on Mono");
            
            if (remoteEP == null)
                throw new ArgumentNullException(nameof(remoteEP));

            return Task.Factory.FromAsync(BeginConnect, EndConnect, remoteEP, null);
        }

        public Task ConnectAsync(BluetoothAddress address, Guid service)
        {
            if (NativeMethods.IsRunningOnMono())
                throw new PlatformNotSupportedException("Async Socket operations not currently supported on Mono");

            return Task.Factory.FromAsync(BeginConnect, EndConnect, address, service, null);
        }

        public void Close()
        {
            if(_socket != null && _socket.Connected)
                _socket.Close();
        }

        private bool _authenticate;

        public bool Authenticate 
        { 
            get => _authenticate; 
            set 
            {
                if (_authenticate != value)
                {
                    _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameAuthenticate, value);
                    _authenticate = value;
                }
            } 
        }

        public Socket Client { get => _socket; }

        public bool Connected
        {
            get
            {
                if (_socket == null)
                    return false;

                if (NativeMethods.IsRunningOnMono())
                    return ((Win32Socket)_socket).Connected;

                return _socket.Connected;
            }
        }

        private bool _encrypt = false;
        public bool Encrypt 
        { 
            get => _encrypt;
            set 
            {
                if (_encrypt != value)
                {
                    _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameEncrypt, value ? 1 : 0);
                    _encrypt = value;
                }
            }
        }

        //length of time for query
        private TimeSpan _inquiryLength = new TimeSpan(0, 0, 10);
        public TimeSpan InquiryLength 
        { 
            get => _inquiryLength; 
            set 
            {
                if ((value.TotalSeconds > 0) && (value.TotalSeconds <= 61))
                {
                    _inquiryLength = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(InquiryLength),
                        "InquiryLength must be a positive TimeSpan between 0 and 61 seconds.");
                }
            } 
        }


        string IBluetoothClient.RemoteMachineName
        {
            get
            {
                if (Connected)
                {
                    var remote = _socket.RemoteEndPoint as BluetoothEndPoint;
                    var info = BLUETOOTH_DEVICE_INFO.Create();
                    info.Address = remote.Address;
                    NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref info);
                    return info.szName;
                }

                return string.Empty;
            }
        }

        public NetworkStream GetStream()
        {
            if (Connected)
            {
                if (NativeMethods.IsRunningOnMono())
                {
                    return new Win32NetworkStream((Win32Socket)_socket, true);
                }
                else
                {
                    return new NetworkStream(_socket, true);
                }
            }
            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
