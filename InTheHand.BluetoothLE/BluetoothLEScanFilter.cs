//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanFilter.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Defines an individual filter to apply when requested devices.
    /// </summary>
    public sealed partial class BluetoothLEScanFilter
    {
        /// <summary>
        /// List of service UUIDs which a device must expose to match the filter.
        /// </summary>
        public IList<BluetoothUuid> Services { get; } = new List<BluetoothUuid>();

        /// <summary>
        /// If present requires a device to exactly match the supplied name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If present requires a device name to start with the supplied prefix.
        /// </summary>
        public string NamePrefix { get; set; }       
    }
}