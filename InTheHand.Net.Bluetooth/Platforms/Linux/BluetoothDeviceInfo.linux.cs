// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Linux)
// 
// Copyright (c) 2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Linux.Bluetooth;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Linux.Bluetooth.Extensions;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private Device _device;

        internal BluetoothDeviceInfo(Device device)
        {
            _device = device;
        }

        public static implicit operator Device(BluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._device;
        }

        public static implicit operator BluetoothDeviceInfo(Device device)
        {
            return new BluetoothDeviceInfo(device);
        }

        public BluetoothDeviceInfo(BluetoothAddress address)
        {
        }

        internal async Task Init()
        {
            if (_device != null)
            {
                var props = await _device.GetAllAsync();
                _name = props.Name;
                _address = BluetoothAddress.Parse(props.Address);
                _connected = props.Connected;
                _authenticated = props.Paired;

                await _device.WatchPropertiesAsync(OnPropertyChanges);
            }
        }

        private void OnPropertyChanges(Tmds.DBus.PropertyChanges propertyChanges)
        {
            foreach(var change in propertyChanges.Changed)
            {
                Console.WriteLine($"{change.Key} {change.Value}");
                //TODO - for properties we use update the cached values
            }
        }

        BluetoothAddress _address;
        BluetoothAddress GetDeviceAddress()
        {
            return _address;
        }

        string _name = string.Empty;
        string GetDeviceName()
        {
            return _name;
        }

        ClassOfDevice GetClassOfDevice()
        {
            return (ClassOfDevice)0;
        }

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            List<Guid> services = new List<Guid>();
            var uuids = await _device.GetUUIDsAsync();
            foreach(var uuid in uuids )
            {
                services.Add(new Guid(uuid));
            }

            return services;
        }

            IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void PlatformSetServiceState(Guid service, bool state)
        {
        }

        private bool _connected;
        bool GetConnected()
        {
            return _connected;
        }

        private bool _authenticated;
        bool GetAuthenticated()
        {
            return _authenticated;
        }

        void PlatformRefresh()
        {
            Init();
        }
    }
}
