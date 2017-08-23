//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Bluetooth.Rfcomm;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothDevice
    {
        private static async Task<BluetoothDevice> FromBluetoothAddressAsyncImpl(ulong address)
        {
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromBluetoothAddressAsync(address);
        }

        private static async Task<BluetoothDevice> FromIdAsyncImpl(string deviceId)
        {
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(deviceId);
        }

        private static async Task<BluetoothDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return await Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(deviceInformation.Id);
        }
        
        private static string GetDeviceSelectorImpl()
        {
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelector();
        }
        
        private static string GetDeviceSelectorFromClassOfDeviceImpl(BluetoothClassOfDevice classOfDevice)
        {
#if !WINDOWS_PHONE_81 && !WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromClassOfDevice(classOfDevice);
#else
            return string.Empty;
#endif
        }
        
        private static string GetDeviceSelectorFromPairingStateImpl(bool pairingState)
        {
#if WINDOWS_UWP
            return Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(pairingState);
#else
            return string.Empty;
#endif
        }
        
        private Windows.Devices.Bluetooth.BluetoothDevice _device;

        private BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Windows.Devices.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }

        private void ConnectionStatusChangedAdd()
        {
            _device.ConnectionStatusChanged += _device_ConnectionStatusChanged;
        }

        private void _device_ConnectionStatusChanged(Windows.Devices.Bluetooth.BluetoothDevice sender, object args)
        {
            _connectionStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ConnectionStatusChangedRemove()
        {
            _device.ConnectionStatusChanged -= _device_ConnectionStatusChanged;
        }

        private void NameChangedAdd()
        {
            _device.NameChanged += _device_NameChanged;
        }

        private void _device_NameChanged(Windows.Devices.Bluetooth.BluetoothDevice sender, object args)
        {
            RaiseNameChanged();
        }

        private void NameChangedRemove()
        {
            _device.NameChanged -= _device_NameChanged;
        }

        private ulong GetBluetoothAddress()
        {
            return _device.BluetoothAddress;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return _device.ClassOfDevice;
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return (BluetoothConnectionStatus)((int)_device.ConnectionStatus);
        }

        private string GetDeviceId()
        {
            return _device.DeviceId;
        }

        private string GetName()
        {
            return _device.Name;
        }

        private async Task<RfcommDeviceServicesResult> GetRfcommServicesAsyncImpl(BluetoothCacheMode cacheMode)
        {
            BluetoothError error = BluetoothError.Success;
            List<RfcommDeviceService> services = new List<RfcommDeviceService>();

#if WINDOWS_PHONE_APP || WINDOWS_PHONE
            foreach(RfcommDeviceService service in _device.RfcommServices)
            {
                services.Add(service);
            }
#else
            var result = await _device.GetRfcommServicesAsync((Windows.Devices.Bluetooth.BluetoothCacheMode)((int)cacheMode));

            error = (BluetoothError)((int)result.Error);
            foreach(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service in result.Services)
            {
                services.Add(service);
            }
#endif
            return new RfcommDeviceServicesResult(error, services.AsReadOnly());
        }
    }
}