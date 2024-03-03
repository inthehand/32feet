//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicValueChangedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Contains the characteristic value whenever a change notification is raised.
    /// </summary>
    /// <seealso cref="GattCharacteristic"/>
    public sealed class GattCharacteristicValueChangedEventArgs : EventArgs
    {
        internal GattCharacteristicValueChangedEventArgs(byte[]? newValue, Exception? error = null)
        {
            Value = newValue;
            Error = error;
        }

        /// <summary>
        /// The new value of the <see cref="GattCharacteristic"/>.
        /// </summary>
        public byte[]? Value { get; private set; }

        /// <summary>
        /// If set, reports the error occurred in the communication. In such case, <see cref="Value" /> is invalid.
        /// </summary>
        public Exception? Error { get; private set; }
    }
}
