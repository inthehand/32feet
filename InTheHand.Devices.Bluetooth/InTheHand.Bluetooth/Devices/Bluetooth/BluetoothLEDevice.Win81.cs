//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Win81.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using InTheHand.Devices.Enumeration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {
        private static Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static string GetDeviceSelectorImpl()
        {
            return null;
        }
        
        private void NameChangedAdd()
        {
        }

        private void NameChangedRemove()
        {
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
        
        private void ConnectionStatusChangedAdd()
        {
        }

        private void ConnectionStatusChangedRemove()
        {
        }

        private string GetDeviceId()
        {
            return null;
        }

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
            List<GattDeviceService> _services = new List<GattDeviceService>();

            return _services.AsReadOnly();
        }
    }
}