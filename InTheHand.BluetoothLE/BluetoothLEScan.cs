//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScan.cs" company="In The Hand Ltd">
//   Copyright (c) 2020-26 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothLEScan
    {
        private readonly List<BluetoothLEScanFilter> _filters = [];

        internal BluetoothLEScan()
        {

        }

        public IReadOnlyList<BluetoothLEScanFilter> Filters => _filters.AsReadOnly();

        public bool KeepRepeatedDevices => PlatformKeepRepeatedDevices;

        public bool AcceptAllAdvertisements => PlatformAcceptAllAdvertisements;

        public bool Active { get; private set; }

        public void Stop()
        {
            Active = false;
            PlatformStop();
        }
    }
}
