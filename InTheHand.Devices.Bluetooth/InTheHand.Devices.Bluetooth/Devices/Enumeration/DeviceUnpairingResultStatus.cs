//-----------------------------------------------------------------------
// <copyright file="DeviceUnpairingResultStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// The result of the unpairing action.
    /// </summary>
    public enum DeviceUnpairingResultStatus
    {
        /// <summary>
        /// The device object is successfully unpaired.
        /// </summary>
        Unpaired = 0,

        /// <summary>
        /// The device object was already unpaired.
        /// </summary>
        AlreadyUnpaired = 1,

        /// <summary>
        /// The device object is currently in the middle of either a pairing or unpairing action.
        /// </summary>
        OperationAlreadyInProgress = 2,

        /// <summary>
        /// The caller does not have sufficient permissions to unpair the device.
        /// </summary>
        AccessDenied = 3,

        /// <summary>
        /// An unknown failure occurred.
        /// </summary>
        Failed = 4,
    }
}