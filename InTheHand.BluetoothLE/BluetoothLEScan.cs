//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.cs" company="In The Hand Ltd">
//   Copyright (c) 2020 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------
#if DEBUG
using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothLEScan
    {
        public readonly List<BluetoothLEScanFilter> Filters = new List<BluetoothLEScanFilter>();

        public bool KeepRepeatedDevices { get; set; }

        public bool AcceptAllAdvertisements { get => PlatformAcceptAllAdvertisements; }

        public bool Active { get; private set; }

        public void Stop()
        {
            Active = false;
            PlatformStop();
        }
    }
}
#endif