// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;

namespace InTheHand.Net.Sockets
{
    public sealed partial class BluetoothDeviceInfo : IEquatable<BluetoothDeviceInfo>
    {
        public void Refresh()
        {
            DoRefresh();
        }

        public BluetoothAddress DeviceAddress
        {
            get
            {
                return GetDeviceAddress();
            }
        }

        public string DeviceName
        {
            get
            {
                return GetDeviceName();
            }
        }

        public ClassOfDevice ClassOfDevice
        {
            get
            {
                return GetClassOfDevice();
            }
        }

        public IReadOnlyCollection<Guid> InstalledServices
        {
            get
            {
                return GetInstalledServices();
            }
        }

        public bool Connected
        {
            get
            {
                return GetConnected();
            }
        }

        public bool Authenticated
        {
            get
            {
                return GetAuthenticated();
            }
        }

        public void SetServiceState(Guid service, bool state)
        {
            DoSetServiceState(service, state);
        }

        public bool Equals(BluetoothDeviceInfo other)
        {
            if (other is null)
                return false;

            return DeviceAddress == other.DeviceAddress;
        }
    }
}
