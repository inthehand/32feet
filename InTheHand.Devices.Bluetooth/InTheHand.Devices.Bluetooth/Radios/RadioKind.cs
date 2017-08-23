//-----------------------------------------------------------------------
// <copyright file="RadioKind.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------


namespace InTheHand.Devices.Radios
{
    /// <summary>
    /// Enumeration that represents the kinds of radio devices.
    /// </summary>
    public enum RadioKind
    {
        /// <summary>
        /// An unspecified kind of radio device.
        /// </summary>
        Other,
        
        /// <summary>
        /// A Wi-Fi radio.
        /// </summary>
        WiFi,

        /// <summary>
        /// A mobile broadband radio. 
        /// </summary>
        MobileBroadband,

        /// <summary>
        /// A Bluetooth radio.
        /// </summary>
        Bluetooth,

        /// <summary>
        /// An FM radio. 
        /// </summary>
        FM,
    }
}