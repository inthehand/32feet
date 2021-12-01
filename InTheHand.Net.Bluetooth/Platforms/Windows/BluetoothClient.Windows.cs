// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothClient (Windows 10)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private readonly StreamSocket _socket;

        private void PlatformInitialize()
        {
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            var t = DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(true));

            t.AsTask().ConfigureAwait(false);
            t.AsTask().Wait();
            var devices = t.GetResults();

            foreach(var device in devices)
            {
                var td = BluetoothDevice.FromIdAsync(device.Id).AsTask();
                td.Wait();
                var bluetoothDevice = td.Result;
                yield return bluetoothDevice;
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> DoDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> results = new List<BluetoothDeviceInfo>();

            var devices = DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(false)).GetResults();

            foreach (var device in devices)
            {
                var bluetoothDevice = BluetoothDevice.FromIdAsync(device.Id).GetResults();
                results.Add(bluetoothDevice);
            }
            return results.AsReadOnly();
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

        TimeSpan GetInquiryLength()
        {
            return TimeSpan.Zero;
        }

        void SetInquiryLength(TimeSpan length)
        {

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