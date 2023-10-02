//-----------------------------------------------------------------------
// <copyright file="BluetoothPhy.cs" company="In The Hand Ltd">
//   Copyright (c) 2020-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Bluetooth physical layer.
    /// </summary>
    public enum BluetoothPhy
    {
        /// <summary>
        /// LE 1M Physical Channel.
        /// </summary>
        Le1m = 1,
        /// <summary>
        /// LE 2M Physical Channel.
        /// </summary>
        Le2m = 2,
        /// <summary>
        /// LE Coded Physical Channel.
        /// </summary>
        LeCoded = 3,
    }
}
