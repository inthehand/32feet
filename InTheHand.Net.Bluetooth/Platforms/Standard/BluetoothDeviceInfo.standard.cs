// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (.NET Standard)
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        public BluetoothDeviceInfo(BluetoothAddress address)
        {
        }

        BluetoothAddress GetDeviceAddress()
        {
            return BluetoothAddress.None;
        }

        string GetDeviceName()
        {
            return string.Empty;
        }

        ClassOfDevice GetClassOfDevice()
        {
            return (ClassOfDevice)0;
        }

        Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            return Task.FromException<IEnumerable<Guid>>(new PlatformNotSupportedException());
        }

            IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void PlatformSetServiceState(Guid service, bool state)
        {
        }

        bool GetConnected()
        {
            return false;
        }

        bool GetAuthenticated()
        {
            return false;
        }

        void PlatformRefresh()
        {
        }
    }
}
