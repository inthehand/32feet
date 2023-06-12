// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (Multiplatform)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace InTheHand.Net.Sockets
{
    internal interface IBluetoothClient
    {
        void PlatformConnect(BluetoothAddress address, Guid service);

        void PlatformClose();

        bool GetAuthenticate();

        void SetAuthenticate(bool value);

        Socket GetClient();

        bool GetConnected();

        bool GetEncrypt();

        void SetEncrypt(bool value);

        TimeSpan GetInquiryLength();

        void SetInquiryLength(TimeSpan length);

        string GetRemoteMachineName();

        NetworkStream PlatformGetStream();
    }

    partial class BluetoothClient
    {
        private void PlatformInitialize()
        {
            throw Exceptions.GetNotImplementedException();
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> PlatformDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

#if NETSTANDARD2_1_OR_GREATER
        async IAsyncEnumerable<BluetoothDeviceInfo> PlatformDiscoverDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield break;
        }
#endif

        void PlatformConnect(BluetoothAddress address, Guid service)
        {
        }

        void PlatformClose()
        {
        }

        bool GetAuthenticate()
        {
            return false;
        }

        void SetAuthenticate(bool value)
        {
        }

        Socket GetClient()
        {
            return null;
        }

        bool GetConnected()
        {
            return false;
        }

        bool GetEncrypt()
        {
            return false;
        }

        void SetEncrypt(bool value)
        {
        }

        TimeSpan GetInquiryLength()
        {
            return TimeSpan.Zero;
        }

        void SetInquiryLength(TimeSpan length)
        {

        }

        public string GetRemoteMachineName()
        {
            return string.Empty;
        }

        NetworkStream PlatformGetStream()
        {
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
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        
        #endregion
    }
}