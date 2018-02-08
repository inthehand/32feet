//-----------------------------------------------------------------------
// <copyright file="GattCommunicationStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the return status of a WinRT GATT API related Async operation.
    /// Indicates the status of the asynchronous operation.
    /// </summary>
    public enum GattCommunicationStatus : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// No communication can be performed with the device, at this time.
        /// </summary>
        Unreachable = 1,
    }
}