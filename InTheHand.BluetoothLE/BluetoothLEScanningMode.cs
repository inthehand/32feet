//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanningMode.cs" company="In The Hand Ltd">
//   Copyright (c) 2026 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Specifies whether the radio sends scan request packets while scanning for advertisements.
    /// </summary>
    /// <remarks>
    /// <see cref="Active"/> scanning additionally issues scan requests so that the remote device
    /// returns scan response packets, which carry advertisement data (for example the local name)
    /// that is not present in the advertising packet itself. Platforms that have no notion of
    /// active/passive scanning may ignore this option.
    /// </remarks>
    public enum BluetoothLEScanningMode
    {
        /// <summary>
        /// Passive scanning: only advertisement packets are received. This is the default mode.
        /// </summary>
        Passive = 0,

        /// <summary>
        /// Active scanning: scan request packets are sent so that scan response data is also received.
        /// </summary>
        Active = 1,
    }
}
