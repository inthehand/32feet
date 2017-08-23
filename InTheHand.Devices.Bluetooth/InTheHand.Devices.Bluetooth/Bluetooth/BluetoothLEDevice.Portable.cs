//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Threading.Tasks;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InTheHand.Foundation;
using System.Threading;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {
        private static Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return null;
        }

        private static string GetDeviceSelectorImpl()
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromConnectionStatusImpl(BluetoothConnectionStatus connectionStatus)
        {
            return string.Empty;
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothAddressType GetBluetoothAddressType()
        {
            return BluetoothAddressType.Unspecified;
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return BluetoothConnectionStatus.Disconnected;
        }


        private string GetDeviceId()
        {
            return string.Empty;
        }
        
        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
            return null;
        }

        private void ConnectionStatusChangedAdd() { }
        private void ConnectionStatusChangedRemove() { }
        private void NameChangedAdd() { }
        private void NameChangedRemove() { }
    }
}