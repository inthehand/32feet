//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private CBPeripheral _peripheral;
        private bool _watchingAdvertisements = false;

        private BluetoothDevice(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
        }

        public static implicit operator BluetoothDevice(CBPeripheral peripheral)
        {
            return new BluetoothDevice(peripheral);
        }

        public static implicit operator CBPeripheral(BluetoothDevice device)
        {
            return device._peripheral;
        }

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            var devices = Bluetooth._manager.RetrievePeripheralsWithIdentifiers(new NSUuid(id));

            if (devices != null && devices.Length > 0)
                return devices[0];

            return null;
        }

        string GetId()
        {
            return _peripheral.Identifier.ToString();
        }

        string GetName()
        {
            return _peripheral.Name;
        }

        BluetoothRemoteGATTServer GetGatt()
        {
            return new BluetoothRemoteGATTServer(this);
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
