//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
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
            _gatt = ((ABluetooth.BluetoothDevice)Device).ConnectGatt(AndroidActivity.CurrentActivity, AutoConnect, _gattCallback, ABluetooth.BluetoothTransports.Auto);
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
        internal event EventHandler<MtuEventArgs> MtuChanged;

        private bool _servicesDiscovered = false;

        internal class GattCallback : ABluetooth.BluetoothGattCallback
        {
            private readonly RemoteGattServer _owner;

            internal GattCallback(RemoteGattServer owner)
            {
                _owner = owner;
            }

            public override void OnMtuChanged(ABluetooth.BluetoothGatt gatt, int mtu, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"OnMtuChanged Status:{status} Mtu:{mtu}");
                
                // only store the new value if successfully changed
                if(status == ABluetooth.GattStatus.Success)
                {
                    _owner.Mtu = mtu;
                }
                
                base.OnMtuChanged(gatt, mtu, status);
            }

            public override void OnConnectionStateChange(ABluetooth.BluetoothGatt gatt, ABluetooth.GattStatus status, ABluetooth.ProfileState newState)
            {
                System.Diagnostics.Debug.WriteLine($"ConnectionStateChanged Status:{status} NewState:{newState}");
                _owner.ConnectionStateChanged?.Invoke(_owner, new ConnectionStateEventArgs { Status = status, State = newState });
                if (newState == ABluetooth.ProfileState.Connected)
                {
                    // set MTU if previously requested
                    if (_owner.requestedMtu != 0)
                        gatt.RequestMtu(_owner.requestedMtu);

                    if (!_owner._servicesDiscovered)
                    {
                        Task.Run(async () =>
                        {
                            System.Diagnostics.Debug.WriteLine(Android.OS.Build.VERSION.SdkInt);
                            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.N && gatt.Device.BondState == ABluetooth.Bond.Bonded)
                                await Task.Delay(1000);

                            gatt.DiscoverServices();
                        });
                    }
                }
                else
                {
                    // reset requested MTU
                    _owner.requestedMtu = 0;
                    _owner._servicesDiscovered = false;
                    _owner.Device.OnGattServerDisconnected();
                }
            }

#if NET7_0_OR_GREATER
            public override void OnCharacteristicRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, byte[] value, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicRead {characteristic.Uuid} Status:{status}");
                _owner.CharacteristicRead?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status, Value = value });
            }
#endif
            public override void OnCharacteristicRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattCharacteristic characteristic, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"CharacteristicRead {characteristic.Uuid} Status:{status}");
                _owner.CharacteristicRead?.Invoke(_owner, new CharacteristicEventArgs { Characteristic = characteristic, Status = status, Value = characteristic.GetValue() });
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

#if NET7_0_OR_GREATER
            public override void OnDescriptorRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status, byte[] value)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorRead {descriptor.Uuid} Status:{status}");
                _owner.DescriptorRead?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status, Value = value });
            }
#endif
            public override void OnDescriptorRead(ABluetooth.BluetoothGatt gatt, ABluetooth.BluetoothGattDescriptor descriptor, ABluetooth.GattStatus status)
            {
                System.Diagnostics.Debug.WriteLine($"DescriptorRead {descriptor.Uuid} Status:{status}");
                _owner.DescriptorRead?.Invoke(_owner, new DescriptorEventArgs { Descriptor = descriptor, Status = status, Value = descriptor.GetValue() });
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

#if NET6_0_OR_GREATER
            public override void OnServiceChanged(ABluetooth.BluetoothGatt gatt)
            {
                _owner._servicesDiscovered = false;
                gatt.DiscoverServices();

                base.OnServiceChanged(gatt);
            }
#endif
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
                    tcs.SetResult(e.Status == ABluetooth.GattStatus.Success);
                }
            };

            ServicesDiscovered += handler;

            return await tcs.Task;
        }

        private void Refresh()
        {
            var refreshMethod = _gatt.Class.GetMethod("refresh"); // unsupported
            if (refreshMethod != null)
            {
                var result = (bool)refreshMethod.Invoke(_gatt);
            }
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
                return Task.FromException(new System.OperationCanceledException());
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

        private int requestedMtu;

        Task<bool> PlatformRequestMtuAsync(int mtu)
        {
            requestedMtu = mtu;
            
            if (IsConnected)
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                // we may not get an event (even for failure) so need a timeout
                var cs = new CancellationTokenSource(1000);
                cs.Token.Register(() => tcs.TrySetResult(false), useSynchronizationContext: false);

                void handler(object s, MtuEventArgs e)
                {
                    MtuChanged -= handler;
                    switch (e.Status)
                    {
                        case ABluetooth.GattStatus.Success:
                            tcs.SetResult(true);
                            break;

                        default:
                            tcs.SetResult(false);
                            break;
                    }
                }

                MtuChanged += handler;
                bool success = _gatt.RequestMtu(mtu);

                if (success)
                {
                    return tcs.Task;
                }
                else
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(false);
        }

        private static ABluetooth.BluetoothPhy ToAndroidPhy(BluetoothPhy phy)
        {
            switch (phy)
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
        public ABluetooth.BluetoothGattCharacteristic Characteristic { get; internal set; }

        public byte[] Value { get; internal set; }
    }

    internal class DescriptorEventArgs : GattEventArgs
    {
        public ABluetooth.BluetoothGattDescriptor Descriptor { get; internal set; }

        public byte[] Value { get; internal set; }
    }

    internal class RssiEventArgs : GattEventArgs
    {
        public short Rssi
        {
            get; internal set;
        }
    }

    internal class MtuEventArgs : GattEventArgs
    {
        public int Mtu
        {
            get; internal set;
        }
    }
}