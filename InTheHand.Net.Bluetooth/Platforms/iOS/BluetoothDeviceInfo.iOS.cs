// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (iOS)
// 
// Copyright (c) 2003-2025 In The Hand Ltd, All rights reserved.
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
        /// <summary>
        /// On iOS returns the ExternalAccessory Protocol strings for the device.
        /// </summary>
        /// <remarks>Protocol names are formatted as reverse-DNS strings. For example, the string “com.apple.myProtocol” might represent a custom protocol defined by Apple.
        /// Manufacturers can define custom protocols for their accessories or work with other manufacturers and organizations to define standard protocols for different accessory types.</remarks>
        public IReadOnlyCollection<string> ProtocolStrings => ((ExternalAccessoryBluetoothDeviceInfo)_bluetoothDeviceInfo).ProtocolStrings;
        
        /// <summary>
        /// On iOS returns the ExternalAccessory serial number.
        /// </summary>
        public string SerialNumber => ((ExternalAccessoryBluetoothDeviceInfo)_bluetoothDeviceInfo).SerialNumber;
    }

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

        public IReadOnlyCollection<string> ProtocolStrings => new ReadOnlyCollection<string>(_accessory.ProtocolStrings.ToList());

        public bool Connected => _accessory.Connected;

        public bool Authenticated => true;

        ClassOfDevice IBluetoothDeviceInfo.ClassOfDevice => (ClassOfDevice)0;
        
        public string SerialNumber => _accessory.SerialNumber;

        void IBluetoothDeviceInfo.Refresh() { }

        async Task<IEnumerable<Guid>> IBluetoothDeviceInfo.GetRfcommServicesAsync(bool cached)
        {
            var services = new List<Guid>();

            foreach (var protocol in ProtocolStrings)
            {
                if (BluetoothServiceProtocolMapping.TryGetUuidForProtocol(protocol, out var uuid))
                    services.Add(uuid);
            }
            return services.AsReadOnly();
        }
    }
}