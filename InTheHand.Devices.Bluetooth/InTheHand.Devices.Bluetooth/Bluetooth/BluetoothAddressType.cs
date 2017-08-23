//-----------------------------------------------------------------------
// <copyright file="BluetoothAddressType.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Describes the Bluetooth address type.
    /// </summary>
    public enum BluetoothAddressType
    {
        /// <summary>
        /// Public address.
        /// </summary>
        Public = 0,

        /// <summary>
        /// Random address.
        /// </summary>
        Random = 1,

        /// <summary>
        /// Unspecified type.
        /// </summary>
        Unspecified = 2,
    }
}