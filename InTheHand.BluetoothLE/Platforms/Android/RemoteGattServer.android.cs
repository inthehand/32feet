//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class RemoteGattServer
    {
        private ABluetooth.BluetoothGatt _gatt;
        private ABluetooth.BluetoothGattCallback _gattCallback;

        private void PlatformInit()
        {
            _gattCallback = new GattCallback(this);
            _gatt = ((ABluetooth.BluetoothDevice)Device).ConnectGatt(Android.App.Application.Context, true, _gattCallback, ABluetooth.BluetoothTransports.Le);
        }

        public static implicit operator ABluetooth.BluetoothGatt(RemoteGattServer gatt)
        {
            return gatt._gatt;
        }

        internal event EventHandler<ConnectionStateEventArgs> ConnectionStateChanged;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicChanged;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicRead;
        internal event EventHandler<CharacteristicEventArgs> CharacteristicWrite;
        internal event EventHandler<DescriptorEventArgs> DescriptorRead;
        internal event EventHandler<DescriptorEventArgs> DescriptorWrite;
        internal event EventHandler<GattEventArgs> ServicesDiscovered;
        internal event EventHandler<RssiEventArgs> ReadRemoteRssi;

        private bool _servicesDiscovered = false;

        internal class GattCallback : ABluetooth.BluetoothGattCallback
        {
            private readonly RemoteGattServer _owner;

            internal GattCallback(RemoteGattServer owner)
            {
                _owner = owner;
            }

            public override void OnConnectionStateChange(ABluetooth.BluetoothGatt gatt, ABluetooth.GattStatus status, ABluetooth.ProfileState newState)
            {
                System.Diagnostics.Debug.WriteLine($"ConnectionStateChanged Status:{status} NewState:{newState}");
                _owner.ConnectionStateChanged?.Invoke(_owner, new ConnectionStateEventArgs { Status = status, State = newState });
                if (newState == ABluetooth.ProfileState.Connected)
                {
                    if (!_owner._servicesDiscovered)
                        gatt.DiscoverServices();
                }
                else
                {
                    _owner.Device.OnGattServerDisconnected();
                }
            }

            public override void OnCharacteristicRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicRead {characteristic.Uuid} Status:{status}");
                _owner.CharacteristicRead?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status });
            }

            public override void OnCharacteristicWrite(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicWrite {characteristic.Uuid} Status:{status}");
                _owner.CharacteristicWrite?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status });
            }

            public override void OnCharacteristicChanged(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicChanged {characteristic.Uuid}");
                _owner.CharacteristicChanged?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic });
            }

            public override void OnDescriptorRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorRead {descriptor.Uuid} Status:{status}");
                _owner.DescriptorRead?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status });
            }

            public override void OnDescriptorWrite(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorWrite {descriptor.Uuid} Status:{status}");
                _owner.DescriptorWrite?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status });
            }

            public override void OnServicesDiscovered(ABluetooth.BluetoothGatt gatt, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"ServicesDiscovered Status:{status}");
                _owner._servicesDiscovered = true;
                _owner.ServicesDiscovered?.Invoke(_owner, new GattEventArgs { Status = status });
            }

            public override void OnReadRemoteRssi(ABluetooth.BluetoothGatt gatt, int rssi, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"ReadRemoteRssi {rssi}");
                _owner.ReadRemoteRssi?.Invoke(_owner, new RssiEventArgs { Status = status, Rssi = (short)rssi });
            }

            public override void OnPhyUpdate(ABluetooth.BluetoothGatt gatt, ABluetooth.LE.ScanSettingsPhy txPhy, ABluetooth.LE.ScanSettingsPhy rxPhy, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"PhyUpdate TX:{txPhy} RX:{rxPhy} Status:{status}");
            }
        }

        bool GetConnected()
        {
            return Bluetooth._manager.GetConnectionState(Device, ABluetooth.ProfileType.Gatt) == ABluetooth.ProfileState.Connected;
        }

        private async Task<bool> WaitForServiceDiscovery()
        {
            if (_servicesDiscovered)
                return true;

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, GattEventArgs e)
            {
                ServicesDiscovered -= handler;

                if (!tcs.Task.IsCompleted)
                {
                    tcs.SetResult(true);
                }
            };

            ServicesDiscovered += handler;
            return await tcs.Task;
        }

        Task PlatformConnect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void handler(object s, ConnectionStateEventArgs e)
            {
                ConnectionStateChanged -= handler;

                switch (e.Status)
                {
                    case ABluetooth.GattStatus.Success:
                        tcs.SetResult(e.State == ABluetooth.ProfileState.Connected);
                        break;

                    default:
                        tcs.SetResult(false);
                        break;
                }
            }

            ConnectionStateChanged += handler;
            bool success = _gatt.Connect();
            if (success)
            {
                if (IsConnected)
                    return Task.FromResult(true);

                return tcs.Task;
            }
            else
            {
                ConnectionStateChanged -= handler;
                return Task.FromException(new OperationCanceledException());
            }
        }

        void PlatformDisconnect()
        {
            _gatt.Disconnect();
        }

        void PlatformCleanup()
        {
            // Android has no explicit cleanup 🤪
        }

        async Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            await WaitForServiceDiscovery();

            ABluetooth.BluetoothGattService nativeService = _gatt.GetService(service);

            return nativeService is null ? null : new GattService(Device, nativeService);
        }

        async Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            var services = new List<GattService>();

            await WaitForServiceDiscovery();

            foreach (var serv in _gatt.Services)
            {
                // if a service was specified only add if service uuid is a match
                if (serv.Type == ABluetooth.GattServiceType.Primary && (!service.HasValue || service.Value == serv.Uuid))
                {
                    services.Add(new GattService(Device, serv));
                }
            }

            return services;
        }

        Task<short> PlatformReadRssi()
        {
            TaskCompletionSource<short> tcs = new TaskCompletionSource<short>();

            void handler(object s, RssiEventArgs e)
            {
                ReadRemoteRssi -= handler;

                switch (e.Status)
                {
                    case ABluetooth.GattStatus.Success:
                        tcs.SetResult(e.Rssi);
                        break;

                    default:
                        tcs.SetResult(0);
                        break;
                }
            }

            ReadRemoteRssi += handler;
            bool success = _gatt.ReadRemoteRssi();
            if (success)
            {
                return tcs.Task;
            }
            else
            {
                return Task.FromResult((short)0);
            }
        }

        void PlatformSetPreferredPhy(BluetoothPhy phy)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                _gatt.SetPreferredPhy(ToAndroidPhy(phy), ToAndroidPhy(phy), ABluetooth.BluetoothPhyOption.NoPreferred);
        }

        private static ABluetooth.BluetoothPhy ToAndroidPhy(BluetoothPhy phy)
        {
            switch(phy)
            {
                case BluetoothPhy.Le1m:
                    return ABluetooth.BluetoothPhy.Le1m;

                case BluetoothPhy.Le2m:
                    return ABluetooth.BluetoothPhy.Le2m;

                case BluetoothPhy.LeCoded:
                    return ABluetooth.BluetoothPhy.LeCoded;

                default:
                    throw new PlatformNotSupportedException($"Unrecognised PHY {phy}");
            }
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

        internal class RssiEventArgs : GattEventArgs
        {
            public short Rssi
            {
                get; internal set;
            }
        }
}
