//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Android.cs" company="In The Hand Ltd">
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
using Android.Bluetooth;
using Android.Runtime;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {
        private static async Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            byte[] buffer = new byte[6];
            var addressBytes = BitConverter.GetBytes(bluetoothAddress);
            for (int i = 0; i < 6; i++)
            {
                buffer[i] = addressBytes[i];
            }

            var device = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(buffer);
            if (device.Type.HasFlag(BluetoothDeviceType.Le))
            {
                return device;
            }

            return null;
        }

        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            var device = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(deviceId);
            if (device.Type.HasFlag(BluetoothDeviceType.Le))
            {
                return device;
            }

            return null;
        }

        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            if (deviceInformation._device.Type.HasFlag(BluetoothDeviceType.Le))
            {
                return deviceInformation._device;
            }

            return null;
        }

        private static string GetDeviceSelectorImpl()
        {
            return "btle";
        }

        private static string GetDeviceSelectorFromConnectionStatusImpl(BluetoothConnectionStatus connectionStatus)
        {
            return "connected:" + (connectionStatus == BluetoothConnectionStatus.Connected ? "true" : "false");
        }

        internal Android.Bluetooth.BluetoothDevice _device;
        internal BluetoothGatt _bluetoothGatt;
        internal GattCallback _gattCallback;

        private BluetoothLEDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothLEDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothLEDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothLEDevice(device);
        }



        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_device.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothAddressType GetBluetoothAddressType()
        {
            if(_device.Type.HasFlag(BluetoothDeviceType.Classic))
            {
                // dual mode devices only support public addresses
                return BluetoothAddressType.Public;
            }

            return BluetoothAddressType.Unspecified;
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            ProfileState state = DeviceInformation.Manager.GetConnectionState(_device, ProfileType.Gatt);
            return state == ProfileState.Connected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
        }


        private string GetDeviceId()
        {
            return _device.Address;
        }

        internal EventWaitHandle _discoveryHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
            List<GattDeviceService> services = new List<GattDeviceService>();
            if (_bluetoothGatt == null)
            {

                _gattCallback = new GattCallback(this);
                _bluetoothGatt = _device.ConnectGatt(Android.App.Application.Context, true, _gattCallback);
                _discoveryHandle.WaitOne();
            }

            if (_bluetoothGatt.DiscoverServices())
            {
                _discoveryHandle.WaitOne();
            }

            foreach (BluetoothGattService service in _bluetoothGatt.Services)
            {
                services.Add(new GattDeviceService(this, service));
            }

            return services.AsReadOnly();
        }

        internal event EventHandler<BluetoothGattCharacteristic> CharacteristicRead;

        internal void RaiseCharacteristicRead(BluetoothGattCharacteristic characteristic)
        {
            CharacteristicRead?.Invoke(this, characteristic);
        }

        internal event EventHandler<BluetoothGattCharacteristic> CharacteristicWrite;

        internal void RaiseCharacteristicWrite(BluetoothGattCharacteristic characteristic)
        {
            CharacteristicWrite?.Invoke(this, characteristic);
        }

        internal event EventHandler<BluetoothGattDescriptor> DescriptorRead;

        internal void RaiseDescriptorRead(BluetoothGattDescriptor descriptor)
        {
            DescriptorRead?.Invoke(this, descriptor);
        }


        private void ConnectionStatusChangedAdd()
        {
            BluetoothAdapter.Default.DeviceConnected += Default_DeviceConnected;
            BluetoothAdapter.Default.DeviceDisconnected += Default_DeviceDisconnected;
        }

        private void Default_DeviceDisconnected(object sender, ulong e)
        {
            if (e == BluetoothAddress)
            {
                RaiseConnectionStatusChanged();
            }
        }

        private void Default_DeviceConnected(object sender, ulong e)
        {
            if (e == BluetoothAddress)
            {
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

    internal class GattCallback : BluetoothGattCallback
    {
        private BluetoothLEDevice _owner;

        internal GattCallback(BluetoothLEDevice owner)
        {
            _owner = owner;
        }

        public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            if(status == GattStatus.Success)
                _owner._discoveryHandle.Set();

            global::System.Diagnostics.Debug.WriteLine(status);
        }

        public event EventHandler<BluetoothGattCharacteristic> CharacteristicChanged;

        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            CharacteristicChanged?.Invoke(this, characteristic);
        }

        public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            _owner.RaiseCharacteristicRead(characteristic);
        }

        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
        }

        public override void OnDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {
            _owner.RaiseDescriptorRead(descriptor);
        }

        public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
        {
            _owner._discoveryHandle.Set();
        }

    }
}