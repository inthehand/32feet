// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (Win32)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private Socket _socket;

        private void PlatformInitialize()
        {
        }

        internal BluetoothClient(Socket s)
        {
            _socket = s;
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
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
                yield return new BluetoothDeviceInfo(device);

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device))
                {
                    yield return new BluetoothDeviceInfo(device);
                }

                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> DoDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            BLUETOOTH_DEVICE_SEARCH_PARAMS search = BLUETOOTH_DEVICE_SEARCH_PARAMS.Create();
            search.cTimeoutMultiplier = 8;
            search.fReturnAuthenticated = false;
            search.fReturnRemembered = true;
            search.fReturnUnknown = true;
            search.fReturnConnected = true;
            search.fIssueInquiry = true;

            BLUETOOTH_DEVICE_INFO device = BLUETOOTH_DEVICE_INFO.Create();
            IntPtr searchHandle = NativeMethods.BluetoothFindFirstDevice(ref search, ref device);
            if(searchHandle != IntPtr.Zero)
            {
                devices.Add(new BluetoothDeviceInfo(device));

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device) && devices.Count < maxDevices)
                {
                    devices.Add(new BluetoothDeviceInfo(device));
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
                    devices.Remove(new BluetoothDeviceInfo(device));
                }

                while (NativeMethods.BluetoothFindNextDevice(searchHandle, ref device))
                {
                    if (device.LastSeen < DateTime.Now.AddMinutes(-1))
                    {
                        devices.Remove(new BluetoothDeviceInfo(device));
                    }
                }

                NativeMethods.BluetoothFindDeviceClose(searchHandle);
            }

            return devices.AsReadOnly();
        }

        void DoConnect(BluetoothAddress address, Guid service)
        {
            var ep = new BluetoothEndPoint(address, service);
            if (NativeMethods.IsRunningOnMono())
            {
                _socket = new Win32Socket();
                ((Win32Socket)_socket).Connect(ep);
            }
            else
            {
                _socket = new Socket(AddressFamilyBluetooth, SocketType.Stream, BluetoothProtocolType.RFComm);
                _socket.Connect(ep);
            }
            
        }

        public IAsyncResult BeginConnect(BluetoothEndPoint endpoint, AsyncCallback requestCallback, object state)
        {
            IAsyncResult result = Client.BeginConnect(endpoint, requestCallback, state);
            return result;
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            Client.EndConnect(asyncResult);
        }

        public Task ConnectAsync(BluetoothEndPoint endpoint)
        {
            if (NativeMethods.IsRunningOnMono())
                throw new PlatformNotSupportedException();

            return Task.Factory.FromAsync(BeginConnect, EndConnect, endpoint, null);
        }

        void DoClose()
        {
            if(_socket != null && _socket.Connected)
                _socket.Close();
        }

        private bool _authenticate;

        bool GetAuthenticate()
        {
            return _authenticate;
        }

        void SetAuthenticate(bool value)
        {
            if (_authenticate != value)
            {
                _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameAuthenticate, value);
                _authenticate = value;
            }
        }

        Socket GetClient()
        {
            return _socket;
        }

        bool GetConnected()
        {
            if (_socket == null)
                return false;

            if (NativeMethods.IsRunningOnMono())
                return ((Win32Socket)_socket).Connected;

            return _socket.Connected;
        }

        private bool _encrypt = false;

        bool GetEncrypt()
        {
            return _encrypt;
        }

        void SetEncrypt(bool value)
        {
            if (_encrypt != value)
            {
                _socket.SetSocketOption(SocketOptionLevelRFComm, SocketOptionNameEncrypt, value ? 1 : 0);
                _encrypt = value;
            }
        }
        
        internal const AddressFamily AddressFamilyBluetooth = (AddressFamily)32;
        private const SocketOptionLevel SocketOptionLevelRFComm = (SocketOptionLevel)0x03;
        private const SocketOptionName SocketOptionNameAuthenticate = unchecked((SocketOptionName)0x80000001);
        private const SocketOptionName SocketOptionNameEncrypt = (SocketOptionName)0x00000002;

        string GetRemoteMachineName()
        {
            if (GetConnected())
            {
                var remote = _socket.RemoteEndPoint as BluetoothEndPoint;
                var info = BLUETOOTH_DEVICE_INFO.Create();
                info.Address = remote.Address;
                NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref info);
                return info.szName;
            }

            return string.Empty;
        }

        System.Net.Sockets.NetworkStream DoGetStream()
        {
            if (Connected)
            {
                if (NativeMethods.IsRunningOnMono())
                {
                    return new Win32NetworkStream((Win32Socket)_socket, true);
                }
                else
                {
                    return new System.Net.Sockets.NetworkStream(_socket, true);
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
        #endregion
    }
}
