//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Threading.Tasks;
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
            return linuxDevice == null ? null : linuxDevice;
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
        }

        string GetId()
        {
            return AsyncHelpers.RunSync(() => { return _device.GetAddressAsync(); });
        }

        string GetName()
        {
            return AsyncHelpers.RunSync(() => { return _device.GetNameAsync(); });
        }

        RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        bool GetIsPaired()
        {
            return AsyncHelpers.RunSync(() => { return _device.GetPairedAsync(); });
        }

        Task PlatformPairAsync()
        {
            return _device.PairAsync();
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
    }
}
