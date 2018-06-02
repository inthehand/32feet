//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
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
#if !WINDOWS_APP
        private Windows.Devices.Bluetooth.BluetoothLEDevice _device;

        private BluetoothLEDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            _device = device;
        }

        public static implicit operator Windows.Devices.Bluetooth.BluetoothLEDevice(BluetoothLEDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothLEDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            return new BluetoothLEDevice(device);
        }

        private void _device_ConnectionStatusChanged(Windows.Devices.Bluetooth.BluetoothLEDevice sender, object args)
        {
            _connectionStatusChanged?.Invoke(this, null);
        }
#endif
        private static async Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return await Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(bluetoothAddress);
        }

        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceInformation.Id);
        }

        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            return await Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(deviceId);
        }

        private static string GetDeviceSelectorImpl()
        {
            return Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelector();
        }

        private static string GetDeviceSelectorFromConnectionStatusImpl(BluetoothConnectionStatus connectionStatus)
        {
            return Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus((Windows.Devices.Bluetooth.BluetoothConnectionStatus)connectionStatus);
        }

        private void NameChangedAdd()
        {
#if !WINDOWS_APP
            _device.NameChanged += _device_NameChanged;
#endif
        }

        private void _device_NameChanged(Windows.Devices.Bluetooth.BluetoothLEDevice sender, object args)
        {
            _nameChanged?.Invoke(this, null);
        }

        private void NameChangedRemove()
        {
            _device.NameChanged -= _device_NameChanged;
        }

        private ulong GetBluetoothAddress()
        {
            return _device.BluetoothAddress;
        }

        private BluetoothAddressType GetBluetoothAddressType()
        {
#if WINDOWS_UWP
            return (BluetoothAddressType)_device.BluetoothAddressType;
#else
            return BluetoothAddressType.Unspecified;
#endif
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return (BluetoothConnectionStatus)((int)_device.ConnectionStatus);
        }
        
        private void ConnectionStatusChangedAdd()
        {
            _device.ConnectionStatusChanged += _device_ConnectionStatusChanged;
        }

        private void ConnectionStatusChangedRemove()
        {
            _device.ConnectionStatusChanged -= _device_ConnectionStatusChanged;
        }

        private string GetDeviceId()
        {
            return _device.DeviceId;
        }

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
            List<GattDeviceService> _services = new List<GattDeviceService>();

            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service in _device.GattServices)
            {
#if WINDOWS_PHONE
                _services.Add(new GattDeviceService(_device, service));

#else
                _services.Add(service);
#endif
            }

            return _services.AsReadOnly();
        }
    }
}