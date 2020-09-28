//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Windows.Devices.Bluetooth.Advertisement;

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        private BluetoothLEScan(BluetoothLEAdvertisementWatcher watcher)
        {
            _watcher = watcher;
            _watcher.Received += _watcher_Received;
            _watcher.Start();

            Active = true;
        }

        public static implicit operator BluetoothLEAdvertisementWatcher(BluetoothLEScan scan)
        {
            return scan._watcher;
        }

        public static implicit operator BluetoothLEScan(BluetoothLEAdvertisementWatcher watcher)
        {
            return new BluetoothLEScan(watcher);
        }

        private bool PlatformAcceptAllAdvertisements
        {
            get
            {
                return _watcher.AdvertisementFilter == null;
            }
        }

        private bool PlatformKeepRepeatedDevices
        {
            get
            {
                return true;
            }
        }

        private void _watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.Advertisement);
            Bluetooth.OnAdvertisementReceived(args);
        }

        private void PlatformStop()
        {
            _watcher.Stop();
        }
    }
}