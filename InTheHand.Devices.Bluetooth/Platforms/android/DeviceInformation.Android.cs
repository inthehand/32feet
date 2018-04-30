//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;
using System.Threading;
using System.Diagnostics;

using Android.Bluetooth;
using Android.Bluetooth.LE;

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceInformation
    {
        private static BluetoothManager _manager;

        static DeviceInformation()
        {
            _manager = (BluetoothManager)global::Android.App.Application.Context.GetSystemService(Android.App.Application.BluetoothService);

        }

        internal static BluetoothManager Manager
        {
            get
            {
                return _manager;
            }
        }

        internal Android.Bluetooth.BluetoothDevice _device;

        private DeviceInformation(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(DeviceInformation deviceInformation)
        {
            return deviceInformation._device;
        }

        public static implicit operator DeviceInformation(Android.Bluetooth.BluetoothDevice device)
        {
            return new DeviceInformation(device);
        }

        private static async Task FindAllAsyncImpl(string aqsFilter, List<DeviceInformation> list)
        {
            // Step 1: Return all paired devices
            foreach (Android.Bluetooth.BluetoothDevice d in _manager.Adapter.BondedDevices)
            {
                list.Add(new DeviceInformation(d));
            }
        }

        private string GetId()
        {
            return "BLUETOOTH#" + _device.Address;
        }

        private string GetName()
        {
            return _device.Name;
        }

        private DeviceInformationPairing GetPairing()
        {
            return new DeviceInformationPairing(_device);
        }
    }
}