//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
        private readonly BluetoothLEScanOptions _options;

        internal BluetoothLEScan(BluetoothLEScanOptions options)
        {
            _options = options;
            if (options != null)
            {
                _filters = options.Filters;
            }

            Bluetooth.StartScanning(new CoreBluetooth.CBUUID[] { });
            Active = true;
        }

        private bool PlatformAcceptAllAdvertisements
        {
            get
            {
                return true;
            }
        }

        private bool PlatformKeepRepeatedDevices
        {
            get
            {
                return true;
            }
        }

        private void PlatformStop()
        {
            Bluetooth.StopScanning();
        }
    }
}
