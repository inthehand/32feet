//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
        private readonly BluetoothLEScanOptions _options;

        private bool PlatformAcceptAllAdvertisements
        {
            get
            {
                return _options.AcceptAllAdvertisements;
            }
        }

        private bool PlatformKeepRepeatedDevices
        {
            get
            {
                return _options.KeepRepeatedDevices;
            }
        }

        private void PlatformStop()
        {
            Bluetooth.adapter.StopDiscoveryAsync();
        }
    }
}
