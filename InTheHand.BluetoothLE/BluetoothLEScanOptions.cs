//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanOptions.cs" company="In The Hand Ltd">
//   Copyright (c) 2020 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothLEScanOptions
    {
        public readonly List<BluetoothLEScanFilter> Filters = new List<BluetoothLEScanFilter>();

        public bool KeepRepeatedDevices { get; set; }

        public bool AcceptAllAdvertisements { get; set; }
    }
}
