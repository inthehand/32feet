//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothDevice
    {
        private static async Task<BluetoothDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return null;
        }

        private static async Task<BluetoothDevice> FromIdAsyncImpl(string deviceId)
        {
            return null;
        }

        private static async Task<BluetoothDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return null;
        }

        private static string GetDeviceSelectorImpl()
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromClassOfDeviceImpl(BluetoothClassOfDevice classOfDevice)
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromPairingStateImpl(bool pairingState)
        {
            return string.Empty;
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(0);
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return BluetoothConnectionStatus.Disconnected;
        }

        private string GetDeviceId()
        {
            return string.Empty;
        }

        private string GetName()
        {
            return string.Empty;
        }

        private void GetRfcommServices(List<RfcommDeviceService> services)
        {
        }

        private Task<RfcommDeviceServicesResult> GetRfcommServicesAsyncImpl(BluetoothCacheMode cacheMode)
        {
            return Task.FromResult(new RfcommDeviceServicesResult(BluetoothError.NotSupported, new List<RfcommDeviceService>().AsReadOnly()));
        }

        private void ConnectionStatusChangedAdd()
        {
        }

        private void ConnectionStatusChangedRemove()
        {
        }

        private void NameChangedAdd()
        {
        }

        private void NameChangedRemove()
        {
        }
    }
}