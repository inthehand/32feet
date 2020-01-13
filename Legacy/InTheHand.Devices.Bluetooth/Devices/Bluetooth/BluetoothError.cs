//-----------------------------------------------------------------------
// <copyright file="BluetoothError.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Specifies common Bluetooth error cases.
    /// </summary>
    public enum BluetoothError
    {
        /// <summary>
        /// The operation was successfully completed or serviced.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The Bluetooth radio was not available.
        /// This error occurs when the Bluetooth radio has been turned off.
        /// </summary>
        RadioNotAvailable = 1,

        /// <summary>
        /// The operation cannot be serviced because the necessary resources are currently in use.
        /// </summary>
        ResourceInUse = 2,

        /// <summary>
        /// The operation cannot be completed because the remote device is not connected.
        /// </summary>
        DeviceNotConnected = 3,

        /// <summary>
        /// An unexpected error has occurred.
        /// </summary>
        OtherError = 4,

        /// <summary>
        /// The operation is disabled by policy.
        /// </summary>
        DisabledByPolicy = 5,

        /// <summary>
        /// The operation is not supported on the current Bluetooth radio hardware.
        /// </summary>
        NotSupported = 6,

        /// <summary>
        /// The operation is disabled by the user.
        /// </summary>
        DisabledByUser = 7,

        /// <summary>
        /// The operation requires consent.
        /// </summary>
        ConsentRequired = 8,

        /// <summary>
        /// Not supported.
        /// </summary>
        TransportNotSupported = 9,
    }
}