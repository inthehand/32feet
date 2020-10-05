//-----------------------------------------------------------------------
// <copyright file="ClientCharacteristicDescriptorValue.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Indicates the state of the Client Characteristic Configuration descriptor.
    /// </summary>
    [Flags]
    internal enum ClientCharacteristicDescriptorValue
    {
        /// <summary>
        /// Neither notification nor indications are enabled.
        /// </summary>
        None = 0,
        /// <summary>
        /// Characteristic notifications are enabled.
        /// </summary>
        Notify = 1,
        /// <summary>
        /// Characteristic indications are enabled.
        /// </summary>
        Indicate = 2,
    }
}
