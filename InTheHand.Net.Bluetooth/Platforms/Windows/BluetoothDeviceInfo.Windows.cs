// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (WinRT)
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        internal BluetoothDevice NativeDevice;

        internal BluetoothDeviceInfo(BluetoothDevice device)
        {
            NativeDevice = device;
        }

        public static implicit operator BluetoothDevice(BluetoothDeviceInfo device)
        {
            return device.NativeDevice;
        }

        public static implicit operator BluetoothDeviceInfo(BluetoothDevice device)
        {
            return new BluetoothDeviceInfo(device);
        }

        BluetoothAddress GetDeviceAddress()
        {
            return NativeDevice.BluetoothAddress;
        }

        string GetDeviceName()
        {
            return NativeDevice.Name;
        }

        ClassOfDevice GetClassOfDevice()
        {
            return (ClassOfDevice)NativeDevice.ClassOfDevice.RawValue;
        }

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
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

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void PlatformSetServiceState(Guid service, bool state)
        {
        }

        bool GetConnected()
        {
            return NativeDevice.ConnectionStatus == BluetoothConnectionStatus.Connected;
        }

        bool GetAuthenticated()
        {
            return NativeDevice.DeviceInformation.Pairing.IsPaired;
        }

        void PlatformRefresh()
        {
        }
    }
}
