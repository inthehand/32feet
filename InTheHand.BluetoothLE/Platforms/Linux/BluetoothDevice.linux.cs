//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private bool _watchingAdvertisements = false;
        internal Device _device;

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            var linuxDevice = await Bluetooth.adapter.GetDeviceAsync(id);

            if(linuxDevice != null)
            {
                var bluetoothDevice = (BluetoothDevice)linuxDevice;
                await bluetoothDevice.Init();
                return bluetoothDevice;
            }

            return null;
        }

        public static implicit operator BluetoothDevice(Device device)
        {
            return new BluetoothDevice(device);
        }

        public static implicit operator Device(BluetoothDevice device)
        {
            return device._device;
        }

        private BluetoothDevice(Device device)
        {
            ArgumentNullException.ThrowIfNull(device, nameof(device));

            _device = device;
            _device.Disconnected += _device_Disconnected;
        }

        internal async Task Init()
        {
            _id = await _device.GetAddressAsync();
            _name = await _device.GetNameAsync();
            _isPaired = await _device.GetPairedAsync();
        }

        private Task _device_Disconnected(Device sender, BlueZEventArgs eventArgs)
        {
            if(eventArgs.IsStateChange)
            {
                OnGattServerDisconnected();
            }
            return Task.CompletedTask;
        }

        private string? _id;
        string GetId()
        {
            return _id == null ? string.Empty : _id;
        }

        private string? _name;
        string GetName()
        {
            return _name == null ? string.Empty : _name;
        }

        RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        private bool _isPaired;
        bool GetIsPaired()
        {
            return _isPaired;
        }

        async Task PlatformPairAsync()
        {
            await _device.PairAsync();
            _isPaired = await _device.GetPairedAsync();
        }

        /*
        bool GetWatchingAdvertisements()
        {
            return _watchingAdvertisements;
        }

        async Task DoWatchAdvertisements()
        {
            _watchingAdvertisements = true;
        }

        void DoUnwatchAdvertisements()
        {
            _watchingAdvertisements = false;
        }*/

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            BluetoothDevice device = obj as BluetoothDevice;
            if (device != null)
            {
                return device.Id == Id;
            }

            return base.Equals(obj);
        }
    }
}
