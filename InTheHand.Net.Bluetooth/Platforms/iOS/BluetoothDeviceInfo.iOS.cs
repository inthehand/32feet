// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (iOS)
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
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
    internal sealed class ExternalAccessoryBluetoothDeviceInfo : IBluetoothDeviceInfo
    {
        private readonly EAAccessory _accessory;

        internal ExternalAccessoryBluetoothDeviceInfo(EAAccessory accessory)
        {
            _accessory = accessory;
        }

        public static implicit operator EAAccessory(ExternalAccessoryBluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._accessory;
        }

        public static implicit operator ExternalAccessoryBluetoothDeviceInfo(EAAccessory accessory)
        {
            return new ExternalAccessoryBluetoothDeviceInfo(accessory);
        }

        public BluetoothAddress DeviceAddress => new BluetoothAddress(_accessory);

        public string DeviceName => _accessory.Name;

        /// <summary>
        /// On iOS returns the ExternalAccessory Protocol strings for the device.
        /// </summary>
        /// <remarks>Protocol names are formatted as reverse-DNS strings. For example, the string “com.apple.myProtocol” might represent a custom protocol defined by Apple.
        /// Manufacturers can define custom protocols for their accessories or work with other manufacturers and organizations to define standard protocols for different accessory types.</remarks>
        public IReadOnlyCollection<string> ProtocolStrings => new ReadOnlyCollection<string>(_accessory.ProtocolStrings.ToList());

        public bool Connected => _accessory.Connected;

        public bool Authenticated => true;

        ClassOfDevice IBluetoothDeviceInfo.ClassOfDevice => (ClassOfDevice)0;

        public string SerialNumber => _accessory.SerialNumber;

        void IBluetoothDeviceInfo.Refresh() { }

        Task<IEnumerable<Guid>> IBluetoothDeviceInfo.GetRfcommServicesAsync(bool cached)
        {
            throw new PlatformNotSupportedException();
        }
    }
}