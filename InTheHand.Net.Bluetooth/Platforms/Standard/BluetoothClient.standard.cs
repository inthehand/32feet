// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (.NET Standard)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> DoDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

        void DoConnect(BluetoothAddress address, Guid service)
        {
        }

        void DoClose()
        {
        }

        bool GetAuthenticate()
        {
            return false;
        }

        void SetAuthenticate(bool value)
        {
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

        public string GetRemoteMachineName()
        {
            return string.Empty;
        }

        NetworkStream DoGetStream()
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