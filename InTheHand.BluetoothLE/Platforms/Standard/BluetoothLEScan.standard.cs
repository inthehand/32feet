//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScan
    {
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
        }
    }
}
