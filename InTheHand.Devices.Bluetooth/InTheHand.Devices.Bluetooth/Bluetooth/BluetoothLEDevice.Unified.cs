//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {
        internal CBPeripheral _peripheral;
        

        private BluetoothLEDevice(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
            
        }

        public static implicit operator CBPeripheral(BluetoothLEDevice device)
        {
            return device._peripheral;
        }

        public static implicit operator BluetoothLEDevice(CBPeripheral peripheral)
        {
            return new BluetoothLEDevice(peripheral);
        }

        internal CBPeripheral Peripheral
        {
            get
            {
                return _peripheral;
            }
        }
        
        private void BluetoothAdapter_ConnectionStateChanged(object sender, CBPeripheral peripheral)
        {
            if (peripheral == _peripheral)
            {
                _connectionStatusChanged?.Invoke(this, null);
            }
        }

        private static Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            var peripherals = BluetoothAdapter.Default.Manager.RetrievePeripheralsWithIdentifiers(new NSUuid(deviceId));

            if (peripherals.Length > 0)
            {
                return new BluetoothLEDevice(peripherals[0]);
            }

            return null;
        }
    
        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return deviceInformation._peripheral;
        }

        private static string GetDeviceSelectorImpl()
        {
            return "btle";
        }

        private static string GetDeviceSelectorFromConnectionStatusImpl(BluetoothConnectionStatus connectionStatus)
        {
            return "connected:" + (connectionStatus == BluetoothConnectionStatus.Connected ? "true" : "false");
        }

        private void NameChangedAdd()
        {
            _peripheral.UpdatedName += _peripheral_UpdatedName;
        }

        private void NameChangedRemove()
        {
            _peripheral.UpdatedName -= _peripheral_UpdatedName;
        }

        private void _peripheral_UpdatedName(object sender, EventArgs e)
        {
            _nameChanged?.Invoke(this, e);
        }

        private ulong GetBluetoothAddress()
        {
            return ulong.MaxValue;
        }

        private BluetoothAddressType GetBluetoothAddressType()
        {
            return BluetoothAddressType.Unspecified;
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _peripheral.State == CBPeripheralState.Connected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
        }

        private void ConnectionStatusChangedAdd()
        {
            BluetoothAdapter.Default.ConnectionStateChanged += BluetoothAdapter_ConnectionStateChanged;
        }

        private void ConnectionStatusChangedRemove()
        {
            BluetoothAdapter.Default.ConnectionStateChanged -= BluetoothAdapter_ConnectionStateChanged;
        }

        private string GetDeviceId()
        {
            return _peripheral.Identifier.ToString();
        }

        private EventWaitHandle _servicesHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private List<GattDeviceService> _services = new List<GattDeviceService>();

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
                if (_services.Count == 0)
                {
                    _peripheral.DiscoveredService += _peripheral_DiscoveredService;
                    var state = _peripheral.State;
                    if(state == CBPeripheralState.Disconnected)
                    {
                        BluetoothAdapter.Default.Manager.ConnectPeripheral(_peripheral);
                        Thread.Sleep(1000);
                    }

                    if (_peripheral.State == CBPeripheralState.Connected)
                    {
                        _peripheral.DiscoverServices();
                        Task.Run(() =>
                        {
                            Task.Delay(6000);
                            _servicesHandle.Set();
                        });
                        _servicesHandle.WaitOne();
                        foreach (CBService service in _peripheral?.Services)
                        {
                            _services.Add(new GattDeviceService(service, _peripheral));
                        }
                    }
                }
                

                return _services.AsReadOnly();
        }
        
        private void _peripheral_DiscoveredService(object sender, NSErrorEventArgs e)
        {
            _servicesHandle.Set();
        }

        
    }
}