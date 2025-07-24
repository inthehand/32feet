//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private readonly ABluetooth.BluetoothDevice _device;

        private BluetoothDevice(ABluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator ABluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(ABluetooth.BluetoothDevice device)
        {
            return device == null ? null : new BluetoothDevice(device);
        }

        public override bool Equals(object? obj)
        {
            if (obj is BluetoothDevice device)
            {
                return _device.Address == device._device.Address;
            }

            return false;
        }

        private static async Task<BluetoothDevice?> PlatformFromId(string id)
        {
            var adapter = Bluetooth._manager.Adapter;
            return adapter.GetRemoteDevice(id);
        }

        public override int GetHashCode() => _device.GetHashCode();

        private string GetId() => _device.Address;

        private string GetName() => _device.Name;

        private RemoteGattServer GetGatt() => new RemoteGattServer(this);

        private bool GetIsPaired() => _device.BondState == ABluetooth.Bond.Bonded;

        private Task PlatformPairAsync()
        {
            _device.CreateBond();
            return Task.CompletedTask;
        }

        private Task PlatformPairAsync(string pairingCode)
        {
            throw new PlatformNotSupportedException();
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

        public void Dispose() {}
    }
}
