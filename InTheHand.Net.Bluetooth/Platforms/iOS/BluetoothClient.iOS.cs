// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothClient (iOS)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using ExternalAccessory;
using Foundation;
using InTheHand.Net.Sockets.iOS;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothClient
    {
        private EASession _session;
        private EAAccessory _accessory;
        private ExternalAccessoryNetworkStream _stream;
        private NSObject _connectionObserver;

        private void PlatformInitialize()
        {
            EAAccessoryManager.SharedAccessoryManager.RegisterForLocalNotifications(); 
            _connectionObserver = EAAccessoryManager.Notifications.ObserveDidConnect((s, e) =>
            {
                System.Diagnostics.Debug.WriteLine(e.Notification + " " + e.Accessory + " " + e.Selected);
            });
        }

        IEnumerable<BluetoothDeviceInfo> GetPairedDevices()
        {
            foreach(EAAccessory accessory in EAAccessoryManager.SharedAccessoryManager.ConnectedAccessories)
            {
                yield return accessory;
            }

            yield break;
        }

        IReadOnlyCollection<BluetoothDeviceInfo> DoDiscoverDevices(int maxDevices)
        {
            List<BluetoothDeviceInfo> devices = new List<BluetoothDeviceInfo>();

            return devices.AsReadOnly();
        }

        void DoConnect(BluetoothAddress address, Guid service)
        {
            _accessory = address.Accessory;
            // TODO: provide mapping support for multiple protocol strings
            _session = new EASession(_accessory, _accessory.ProtocolStrings[0]);
            _stream = new ExternalAccessoryNetworkStream(_session);
        }

        void DoClose()
        {
            _session?.Dispose();
            _session = null;
        }
        
        bool GetAuthenticate()
        {
            return true;
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
            if (_accessory is object)
            {
                return _accessory.Connected;
            }

            return false;
        }
        
        bool GetEncrypt()
        {
            return true;
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
            return _accessory?.Name;
        }

        NetworkStream DoGetStream()
        {
            if (Connected)
                return _stream;

            return null;
        }

        void Dispose(bool disposing)
        {
            if (_connectionObserver != null)
            {
                _connectionObserver.Dispose();
                _connectionObserver = null;
            }
        }
    }
}
