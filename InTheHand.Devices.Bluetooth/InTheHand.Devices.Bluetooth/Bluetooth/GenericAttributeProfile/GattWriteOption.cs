//-----------------------------------------------------------------------
// <copyright file="GattWriteOption.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Indicates what type of write operation is to be performed.
    /// </summary>
    public enum GattWriteOption
    {
        /// <summary>
        /// The default GATT write procedure shall be used.
        /// </summary>
        WriteWithResponse = 0,

        /// <summary>
        /// The Write Without Response procedure shall be used.
        /// </summary>
        WriteWithoutResponse = 1,
    }
}