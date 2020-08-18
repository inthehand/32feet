//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using ABluetooth = Android.Bluetooth;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        internal readonly ABluetooth.BluetoothDevice _device;
        private BluetoothRemoteGATTServer _gattServer;
        private bool _watchingAdvertisements = false;

        internal BluetoothDevice(ABluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator ABluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(ABluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
        }

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            return ABluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(id);
        }

        string GetId()
        {
            return _device.Address;
        }

        string GetName()
        {
            return _device.Name;
        }

        BluetoothRemoteGATTServer GetGatt()
        {
            if (_gattServer is null)
            {
                _gattServer = new BluetoothRemoteGATTServer(this, _device);
            }

            return _gattServer;
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
