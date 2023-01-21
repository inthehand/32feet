//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-22 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private readonly ABluetooth.BluetoothDevice _device;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RemoteGattServer _gattServer;

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

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            var adapter = Bluetooth._manager.Adapter;
            return adapter.GetRemoteDevice(id);
        }

        public override int GetHashCode()
        {
            return _device.GetHashCode();
        }

        string GetId()
        {
            return _device.Address;
        }

        string GetName()
        {
            return _device.Name;
        }

        RemoteGattServer GetGatt()
        {
            if (_gattServer is null)
            {
                _gattServer = new RemoteGattServer(this);
            }

            return _gattServer;
        }

        bool GetIsPaired()
        {
            return _device.BondState == ABluetooth.Bond.Bonded;
        }

        Task PlatformPairAsync()
        {
            _device.CreateBond();
            return Task.CompletedTask;
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
