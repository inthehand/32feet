//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Bluetooth;
using Android.Content;
using Android.OS;
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
        internal Android.Bluetooth.BluetoothDevice _device;

        private BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }

        private static async Task<BluetoothDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            byte[] buffer = new byte[6];
            var addressBytes = BitConverter.GetBytes(bluetoothAddress);
            for (int i = 0; i < 6; i++)
            {
                buffer[i] = addressBytes[i];
            }

            var device = DeviceInformation.Manager.Adapter.GetRemoteDevice(buffer);

            if (device.Type.HasFlag(BluetoothDeviceType.Classic))
            {
                return device;
            }

            return null;
        }

        private static async Task<BluetoothDevice> FromIdAsyncImpl(string deviceId)
        {
            var device = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceId);

            if (device.Type.HasFlag(BluetoothDeviceType.Classic))
            {
                return device;
            }

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
            return ulong.Parse(_device.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice((uint)_device.BluetoothClass.DeviceClass);
        }

        private BluetoothConnectionStatus _connectionStatus = BluetoothConnectionStatus.Disconnected;
        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _connectionStatus;
        }

        private string GetDeviceId()
        {
            return _device.Address;
        }

        private string GetName()
        {
            return _device.Name;
        }

        private async Task<RfcommDeviceServicesResult> GetRfcommServicesAsyncImpl(BluetoothCacheMode cacheMode)
        {
            BluetoothError error = BluetoothError.Success;
            List<RfcommDeviceService> services = new List<RfcommDeviceService>();

            if (cacheMode == BluetoothCacheMode.Uncached)
            {
                error = _device.FetchUuidsWithSdp() ? BluetoothError.Success : BluetoothError.DeviceNotConnected;
            }

            ParcelUuid[] uuids = _device.GetUuids();
            if (uuids != null)
            {
                foreach (ParcelUuid g in uuids)
                {
                    services.Add(new RfcommDeviceService(this, RfcommServiceId.FromUuid(new Guid(g.Uuid.ToString()))));
                }
            }

            return new RfcommDeviceServicesResult(error, services.AsReadOnly());
        }
        
        private void ConnectionStatusChangedAdd()
        {
            BluetoothAdapter.Default.DeviceConnected += Default_DeviceConnected;
            BluetoothAdapter.Default.DeviceDisconnected += Default_DeviceDisconnected;
        }

        private void Default_DeviceDisconnected(object sender, ulong e)
        {
            if(e == BluetoothAddress)
            {
                _connectionStatus = BluetoothConnectionStatus.Disconnected;
                RaiseConnectionStatusChanged();
            }
        }

        private void Default_DeviceConnected(object sender, ulong e)
        {
            if (e == BluetoothAddress)
            {
                _connectionStatus = BluetoothConnectionStatus.Connected;
                RaiseConnectionStatusChanged();
            }
        }

        private void ConnectionStatusChangedRemove()
        {
            BluetoothAdapter.Default.DeviceConnected -= Default_DeviceConnected;
            BluetoothAdapter.Default.DeviceDisconnected -= Default_DeviceDisconnected;
        }
        
        private void NameChangedAdd()
        {
            BluetoothAdapter.Default.NameChanged += Default_NameChanged;
        }

        private void NameChangedRemove()
        {
            BluetoothAdapter.Default.NameChanged -= Default_NameChanged;
        }

        private void Default_NameChanged(object sender, ulong e)
        {
            if (e == BluetoothAddress)
            {
                RaiseNameChanged();
            }
        }
    }
}