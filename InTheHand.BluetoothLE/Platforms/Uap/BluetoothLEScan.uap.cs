//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.uap.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------
#if DEBUG
using Windows.Devices.Bluetooth.Advertisement;

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
        BluetoothLEAdvertisementWatcher _watcher;

        internal BluetoothLEScan(BluetoothLEAdvertisementFilter filter = null)
        {
            if (filter != null)
            {
                _watcher = new BluetoothLEAdvertisementWatcher(filter);
            }
            else
            {
                _watcher = new BluetoothLEAdvertisementWatcher();
            }

            _watcher.Received += _watcher_Received;
            _watcher.Start();

            Active = true;
        }

        private bool PlatformAcceptAllAdvertisements
        {
            get
            {
                return _watcher.AdvertisementFilter == null;
            }
        }

        private void _watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.Advertisement);
        }

        private void PlatformStop()
        {
            _watcher.Stop();
        }
    }
}
#endif