// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (WinRT)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace InTheHand.Net.Sockets
{
    internal sealed class WindowsBluetoothDeviceInfo : BluetoothDeviceInfo
    {
        internal BluetoothDevice NativeDevice;

        internal WindowsBluetoothDeviceInfo(BluetoothDevice device)
        {
            NativeDevice = device;
        }

        public static implicit operator BluetoothDevice(WindowsBluetoothDeviceInfo device)
        {
            return device.NativeDevice;
        }

        public static implicit operator WindowsBluetoothDeviceInfo(BluetoothDevice device)
        {
            return new WindowsBluetoothDeviceInfo(device);
        }

        public override BluetoothAddress DeviceAddress { get => NativeDevice.BluetoothAddress; }

        public override string DeviceName { get => NativeDevice.Name; }

        public override ClassOfDevice ClassOfDevice { get => (ClassOfDevice)NativeDevice.ClassOfDevice.RawValue; }

        public override async Task<IEnumerable<Guid>> GetRfcommServicesAsync(bool cached)
        {
            List<Guid> services = new List<Guid>();

            var servicesResult = await NativeDevice.GetRfcommServicesAsync(cached ? BluetoothCacheMode.Cached : BluetoothCacheMode.Uncached);

            if(servicesResult != null && servicesResult.Error == BluetoothError.Success)
            {
                foreach(var service in servicesResult.Services)
                {
                    services.Add(service.ServiceId.Uuid);
                }
            }

            return services;
        }

        public override bool Connected { get => NativeDevice.ConnectionStatus == BluetoothConnectionStatus.Connected; }

        public override bool Authenticated { get => NativeDevice.DeviceInformation.Pairing.IsPaired; }
    }
}
