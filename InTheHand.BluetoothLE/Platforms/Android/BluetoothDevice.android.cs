//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Bluetooth.GenericAttributeProfile;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        internal Android.Bluetooth.BluetoothDevice _device;
        internal BluetoothRemoteGATTServer GattServer;
        private bool _watchingAdvertisements = false;

        internal BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        public static implicit operator Android.Bluetooth.BluetoothDevice(BluetoothDevice device)
        {
            return device._device;
        }

        public static implicit operator BluetoothDevice(Android.Bluetooth.BluetoothDevice device)
        {
            return new BluetoothDevice(device);
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
            if (GattServer is null)
            {
                GattServer = new BluetoothRemoteGATTServer(this, _device);
            }

            return GattServer;
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
