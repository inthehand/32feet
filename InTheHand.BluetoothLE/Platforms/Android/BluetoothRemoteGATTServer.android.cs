//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTServer.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;
using Android.Runtime;
using System.Runtime.InteropServices;

namespace InTheHand.Bluetooth
{
    partial class BluetoothRemoteGATTServer
    {
        internal readonly ABluetooth.BluetoothGatt NativeGatt;
        private readonly ABluetooth.BluetoothGattCallback _gattCallback;
        private EventWaitHandle _servicesDiscoveredHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        
        private void PlatformInit()
        {
        }

        internal BluetoothRemoteGATTServer(BluetoothDevice device, ABluetooth.BluetoothDevice bluetoothDevice) : this(device)
        {
            _gattCallback = new GattCallback(this);
            NativeGatt = bluetoothDevice.ConnectGatt(Android.App.Application.Context, false, _gattCallback);
        }

        internal event EventHandler<ConnectionStateEventArgs> ConnectionStateChanged;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicChanged;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicRead;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicWrite;
        internal event EventHandler<DescriptorEventArgs> DescriptorRead;
        internal event EventHandler<DescriptorEventArgs> DescriptorWrite;

        internal class GattCallback : ABluetooth.BluetoothGattCallback
        {
            private BluetoothRemoteGATTServer _owner;

            internal GattCallback(BluetoothRemoteGATTServer owner)
            {
                _owner = owner;
            }

            public override void OnConnectionStateChange(ABluetooth.BluetoothGatt gatt, ABluetooth.GattStatus status, ABluetooth.ProfileState newState)
            {
                System.Diagnostics.Debug.WriteLine($"ConnectionStateChanged {status}");
                _owner.ConnectionStateChanged?.Invoke(_owner, new ConnectionStateEventArgs { Status = status, State = newState });
                if (newState == ABluetooth.ProfileState.Connected)
                {
                    gatt.DiscoverServices();
                }
                else
                {
                    _owner.Device.OnGattServerDisconnected();
                }
            }

            public override void OnCharacteristicRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicRead {characteristic.Uuid} {status}");
                _owner.CharacteristicRead?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status });
            }

            public override void OnCharacteristicWrite(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicWrite {characteristic.Uuid} {status}");
                _owner.CharacteristicWrite?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status });
            }

            public override void OnCharacteristicChanged(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicChanged {characteristic.Uuid}");
                _owner.CharacteristicChanged?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic });
            }

            public override void OnDescriptorRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorRead {descriptor.Uuid} {status}");
                _owner.DescriptorRead?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status });
            }

            public override void OnDescriptorWrite(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorWrite {descriptor.Uuid} {status}");
                _owner.DescriptorWrite?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status });
            }

            public override void OnServicesDiscovered(ABluetooth.BluetoothGatt gatt, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"ServicesDiscovered {status}");
                _owner._servicesDiscoveredHandle.Set();
            }
        }

        bool GetConnected()
        {
            return Bluetooth._manager.GetConnectionState(Device._device, ABluetooth.ProfileType.Gatt) == ABluetooth.ProfileState.Connected;
        }

        Task DoConnect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, ConnectionStateEventArgs e)
            {
                switch (e.Status)
                {
                    case ABluetooth.GattStatus.Success:
                        tcs.SetResult(e.State == ABluetooth.ProfileState.Connected);
                        break;

                    default:
                        tcs.SetResult(false);
                        break;
                }

                ConnectionStateChanged -= handler;
            }

            ConnectionStateChanged += handler;
            bool success = NativeGatt.Connect();
            return tcs.Task;
        }

        void DoDisconnect()
        {
            NativeGatt.Disconnect();
        }

        Task<GattService> DoGetPrimaryService(BluetoothUuid service)
        {
            _servicesDiscoveredHandle.WaitOne();
            ABluetooth.BluetoothGattService nativeService = NativeGatt.GetService(service);
            
            return Task.FromResult(nativeService is null ? null : new GattService(Device, nativeService));
        }

        async Task<List<GattService>> DoGetPrimaryServices(BluetoothUuid? service)
        {
            var services = new List<GattService>();

            _servicesDiscoveredHandle.WaitOne();

            foreach (var serv in NativeGatt.Services)
            {
                // if a service was specified only add if service uuid is a match
                if (serv.Type == ABluetooth.GattServiceType.Primary && (!service.HasValue || service.Value == serv.Uuid))
                {
                    services.Add(new GattService(Device, serv));
                }
            }

            return services;
        }
    }

    internal class GattEventArgs : EventArgs
    {
        public ABluetooth.GattStatus Status
        {
            get; internal set;
        }
    }

    internal class ConnectionStateEventArgs : GattEventArgs
    {
        public ABluetooth.ProfileState State
        {
            get; internal set;
        }
    }

    internal class CharacteristicEventArgs : GattEventArgs
    {
        public ABluetooth.BluetoothGattCharacteristic Characteristic
        {
            get; internal set;
        }
    }

    internal class DescriptorEventArgs : GattEventArgs
    {
        public ABluetooth.BluetoothGattDescriptor Descriptor
        {
            get; internal set;
        }
    }
}
