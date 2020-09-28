//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.cs" company="In The Hand Ltd">
//   Copyright (c) 2020 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothLEScan
    {
        private List<BluetoothLEScanFilter> _filters = new List<BluetoothLEScanFilter>();

        internal BluetoothLEScan()
        {

        }

        public IReadOnlyList<BluetoothLEScanFilter> Filters { get => _filters.AsReadOnly(); }

        public bool KeepRepeatedDevices { get => PlatformKeepRepeatedDevices; }

        public bool AcceptAllAdvertisements { get => PlatformAcceptAllAdvertisements; }

        public bool Active { get; private set; }

        public void Stop()
        {
            Active = false;
            PlatformStop();
        }
    }
}
