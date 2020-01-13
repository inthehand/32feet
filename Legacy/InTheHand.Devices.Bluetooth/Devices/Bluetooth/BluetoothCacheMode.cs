//-----------------------------------------------------------------------
// <copyright file="BluetoothCacheMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates whether certain Bluetooth API methods should operate on values cached in the system or retrieve those values from the Bluetooth device.
    /// </summary>
    public enum BluetoothCacheMode
    {
        /// <summary>
        /// Use system-cached values.
        /// </summary>
        Cached = 0,

        /// <summary>
        /// Retrieve values from the Bluetooth device.
        /// </summary>
        Uncached = 1,
    }
}