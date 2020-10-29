// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Windows 10)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
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

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void DoSetServiceState(Guid service, bool state)
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

        void DoRefresh()
        {
        }
    }
}
