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

        /// <summary>
        /// Gets or sets the scanning mode used when starting the scan.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="BluetoothLEScanningMode.Active"/>, which also issues scan requests
        /// so that scan response packets are received (for example to obtain the local name of
        /// devices that advertise it there). This matches the native default on Android and iOS,
        /// which both scan actively. Set to <see cref="BluetoothLEScanningMode.Passive"/> to
        /// suppress scan requests.
        /// Honoured on Windows; on Android and iOS the radio always scans actively, so this option
        /// currently has no effect there.
        /// </remarks>
        public BluetoothLEScanningMode ScanningMode { get; set; } = BluetoothLEScanningMode.Active;
    }
}
