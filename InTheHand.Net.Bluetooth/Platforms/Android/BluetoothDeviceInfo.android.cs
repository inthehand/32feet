// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using System;
using System.Collections.Generic;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private BluetoothDevice _device;

        internal BluetoothDeviceInfo(BluetoothDevice device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            _device = device;
        }

        public static implicit operator BluetoothDevice(BluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._device;
        }

        public static implicit operator BluetoothDeviceInfo(BluetoothDevice device)
        {
            return new BluetoothDeviceInfo(device);
        }

        BluetoothAddress GetDeviceAddress()
        {
            return BluetoothAddress.Parse(_device.Address);
        }

        string GetDeviceName()
        {
            return _device.Name;
        }

        private ClassOfDevice _cod;
        ClassOfDevice GetClassOfDevice()
        {
            if (_cod == 0)
            {
                _cod = ClassOfDeviceHelper.ToClassOfDevice(_device.BluetoothClass);
            }

            return _cod;
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
            return false;
        }

        bool GetAuthenticated()
        {
            return _device.BondState == Bond.Bonded;
        }

        void DoRefresh()
        {
        }
    }
}
