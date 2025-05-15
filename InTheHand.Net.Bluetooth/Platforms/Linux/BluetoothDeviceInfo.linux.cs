﻿// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Linux)
// 
// Copyright (c) 2023-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Linux.Bluetooth;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Linux.Bluetooth.Extensions;

namespace InTheHand.Net.Sockets
{
    internal sealed class LinuxBluetoothDeviceInfo : IBluetoothDeviceInfo
    {
        private Device _device;

        internal LinuxBluetoothDeviceInfo(Device device)
        {
            _device = device;
        }

        public static implicit operator Device(LinuxBluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._device;
        }

        public static implicit operator LinuxBluetoothDeviceInfo(Device device)
        {
            return new LinuxBluetoothDeviceInfo(device);
        }

        public LinuxBluetoothDeviceInfo(BluetoothAddress address)
        {
            Task t = Task.Run(async () =>
            {
                _device = await ((Adapter)BluetoothRadio.Default).GetDeviceAsync(address.ToString());

                if (_device != null)
                { 
                    await Init();
                }
            });

            t.Wait();
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
            foreach (var change in propertyChanges.Changed)
            {
                Console.WriteLine($"{change.Key} {change.Value}");
                //TODO - for properties we use update the cached values
            }
        }

        BluetoothAddress _address;
        public BluetoothAddress DeviceAddress { get => _address; }

        string _name = string.Empty;
        public string DeviceName { get => _name; }

        public async Task<IEnumerable<Guid>> GetRfcommServicesAsync(bool cached)
        {
            List<Guid> services = new List<Guid>();
            var uuids = await _device.GetUUIDsAsync();
            foreach(var uuid in uuids )
            {
                services.Add(new Guid(uuid));
            }

            return services;
        }

        private bool _connected;
        public bool Connected { get =>  _connected; }

        
        ClassOfDevice IBluetoothDeviceInfo.ClassOfDevice => (ClassOfDevice)0;

        private bool _authenticated;
        bool IBluetoothDeviceInfo.Authenticated => _authenticated;

        public void Refresh()
        {
            Init();
        }

        public string SerialNumber => string.Empty;

        public IReadOnlyCollection<string> ProtocolStrings => throw new PlatformNotSupportedException();
  }
}