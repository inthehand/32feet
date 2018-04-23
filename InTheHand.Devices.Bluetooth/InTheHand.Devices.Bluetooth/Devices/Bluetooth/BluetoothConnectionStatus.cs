//-----------------------------------------------------------------------
// <copyright file="BluetoothConnectionStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates the connection status of the device.
    /// </summary>
    public enum BluetoothConnectionStatus
    {
        /// <summary>
        /// The device is disconnected.
        /// </summary>
        Disconnected = 0,

        /// <summary>
        /// The device is connected.
        /// </summary>
        Connected = 1,
    }
}