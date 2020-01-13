// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (iOS)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using ExternalAccessory;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private EAAccessory _accessory;

        internal BluetoothDeviceInfo(EAAccessory accessory)
        {
            _accessory = accessory;
        }

        public static implicit operator EAAccessory(BluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._accessory;
        }

        public static implicit operator BluetoothDeviceInfo(EAAccessory accessory)
        {
            return new BluetoothDeviceInfo(accessory);
        }

        BluetoothAddress GetDeviceAddress()
        {
            return new BluetoothAddress(_accessory);
        }

        string GetDeviceName()
        {
            return _accessory.Name;
        }

        ClassOfDevice GetClassOfDevice()
        {
            return (ClassOfDevice)0;
        }

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void DoSetServiceState(Guid service, bool state)
        {
        }

        bool GetConnected()
        {
            return _accessory.Connected;
        }

        bool GetAuthenticated()
        {
            return true;
        }

        void DoRefresh()
        {
        }
    }
}