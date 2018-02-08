//-----------------------------------------------------------------------
// <copyright file="DeviceInformationPairing.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System;

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceInformationPairing
    {
        private Android.Bluetooth.BluetoothDevice _device;

        internal DeviceInformationPairing(Android.Bluetooth.BluetoothDevice device)
        {
            _device = device;
        }

        private bool GetIsPaired()
        {
            return _device.BondState == Android.Bluetooth.Bond.Bonded;
        }

        private DevicePairingResult DoPair()
        {
            return new DevicePairingResult(_device.CreateBond());
        }

        private DeviceUnpairingResult DoUnpair()
        {
            return new DeviceUnpairingResult();
        }
    }
}