using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Runtime;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTServer
    {
        internal Android.Bluetooth.BluetoothGatt NativeGatt;
        private Android.Bluetooth.BluetoothGattCallback _gattCallback;
        private EventWaitHandle _connectedHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _servicesDiscoveredHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle _characteristicReadHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _characteristicWriteHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _descriptorReadHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _descriptorWriteHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private bool _connected = false;

        internal BluetoothRemoteGATTServer(BluetoothDevice device, Android.Bluetooth.BluetoothDevice bluetoothDevice) : this(device)
        {
            _gattCallback = new GattCallback(this);
            NativeGatt = bluetoothDevice.ConnectGatt(Android.App.Application.Context, false, _gattCallback);
        }

        internal void WaitForCharacteristicRead()
        {
            _characteristicReadHandle.WaitOne();
        }

        internal void WaitForCharacteristicWrite()
        {
            _characteristicWriteHandle.WaitOne();
        }

        internal void WaitForDescriptorRead()
        {
            _descriptorReadHandle.WaitOne();
        }

        internal void WaitForDescriptorWrite()
        {
            _descriptorWriteHandle.WaitOne();
        }

        internal class GattCallback : Android.Bluetooth.BluetoothGattCallback
        {
            private BluetoothRemoteGATTServer _owner;

            internal GattCallback(BluetoothRemoteGATTServer owner)
            {
                _owner = owner;
            }

            public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
            {
                System.Diagnostics.Debug.WriteLine($"ConnectionStateChanged {status}");
                if (newState == ProfileState.Connected)
                {
                    _owner._connected = true;
                    _owner._connectedHandle.Set();
                    gatt.DiscoverServices();
                }
            }

            public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicRead {characteristic.Uuid} {status}");
                _owner._characteristicReadHandle.Set();
            }

            public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicWrite {characteristic.Uuid} {status}");
                _owner._characteristicWriteHandle.Set();
            }

            public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicChanged {characteristic.Uuid}");

            }

            public override void OnDescriptorRead(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorRead {descriptor.Uuid} {status}");
                _owner._descriptorReadHandle.Set();
            }

            public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, [GeneratedEnum] GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorWrite {descriptor.Uuid} {status}");
                _owner._descriptorWriteHandle.Set();
            }

            public override void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"ServicesDiscovered {status}");
                _owner._servicesDiscoveredHandle.Set();
            }
        }

        bool GetConnected()
        {
            return Bluetooth._manager.GetConnectionState(Device._device, ProfileType.Gatt) == ProfileState.Connected;
        }

        Task DoConnect()
        {
            return Task.Run(() =>
            {
                bool success = NativeGatt.Connect();
                _connectedHandle.WaitOne();
            });
        }

        void DoDisconnect()
        {
            NativeGatt.Disconnect();
        }

        Task<BluetoothRemoteGATTService> DoGetPrimaryService(Guid? service)
        {
            BluetoothGattService nativeService = null;
            _servicesDiscoveredHandle.WaitOne();
            if (service.HasValue)
            {
                nativeService = NativeGatt.GetService(Java.Util.UUID.FromString(service.ToString()));
            }
            else
            {
                foreach (var serv in NativeGatt.Services)
                {
                    if (serv.Type == GattServiceType.Primary)
                    {
                        nativeService = serv;
                        break;
                    }
                }
            }
            return Task.FromResult(nativeService is null ? null : new BluetoothRemoteGATTService(Device, nativeService));
        }

        async Task<List<BluetoothRemoteGATTService>> DoGetPrimaryServices(Guid? service)
        {
            var services = new List<BluetoothRemoteGATTService>();

            _servicesDiscoveredHandle.WaitOne();

            foreach (var serv in NativeGatt.Services)
            {
                services.Add(new BluetoothRemoteGATTService(Device, serv));
            }

            return services;
        }
    }
}
