//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Mac.cs" company="In The Hand Ltd">
//   Copyright (c) 2018 In The Hand Ltd, All rights reserved.
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
            return deviceInformation._device;
        }

        private static string GetDeviceSelectorImpl()
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromClassOfDeviceImpl(BluetoothClassOfDevice classOfDevice)
        {
            return "bluetoothClassOfDevice:" + classOfDevice.RawValue.ToString("X12");
        }

        private static string GetDeviceSelectorFromPairingStateImpl(bool pairingState)
        {
            return "bluetoothPairingState:" + pairingState.ToString();
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(0);
        }

        private BluetoothConnectionStatus _connectionStatus = BluetoothConnectionStatus.Disconnected;
        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _connectionStatus;
        }

        private string GetDeviceId()
        {
            return "";
        }

        private string GetName()
        {
            return "";
        }

        private async Task<RfcommDeviceServicesResult> GetRfcommServicesAsyncImpl(BluetoothCacheMode cacheMode)
        {
            BluetoothError error = BluetoothError.Success;
            List<RfcommDeviceService> services = new List<RfcommDeviceService>();
            
            return new RfcommDeviceServicesResult(error, services.AsReadOnly());
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