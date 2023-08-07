// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (iOS)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using ExternalAccessory;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

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

        /// <summary>
        /// On iOS returns the ExternalAccessory Protocol strings for the device.
        /// </summary>
        /// <remarks>Protocol names are formatted as reverse-DNS strings. For example, the string “com.apple.myProtocol” might represent a custom protocol defined by Apple.
        /// Manufacturers can define custom protocols for their accessories or work with other manufacturers and organizations to define standard protocols for different accessory types.</remarks>
        public IReadOnlyCollection<string> ProtocolStrings
        {
            get
            {
                return new ReadOnlyCollection<string>( _accessory.ProtocolStrings.ToList());
            }
        }

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            throw new PlatformNotSupportedException();
        }

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void PlatformSetServiceState(Guid service, bool state)
        {
            throw new PlatformNotSupportedException();
        }

        bool GetConnected()
        {
            return _accessory.Connected;
        }

        bool GetAuthenticated()
        {
            return true;
        }

        void PlatformRefresh()
        {
        }
    }
}
