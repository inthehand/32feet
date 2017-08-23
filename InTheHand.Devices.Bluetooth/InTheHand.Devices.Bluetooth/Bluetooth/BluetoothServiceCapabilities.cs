//-----------------------------------------------------------------------
// <copyright file="BluetoothServiceCapabilities.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates the service capabilities of a device.
    /// </summary>
    /// <remarks>
    /// <para>Defined in Bluetooth Specifications <see href="https://www.bluetooth.com/specifications/assigned-numbers/baseband"/>.</para>
    /// </remarks>
    [Flags]
    public enum BluetoothServiceCapabilities
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Limited Discoverable Mode.
        /// </summary>
        LimitedDiscoverableMode = 1,

        //Reserved = 2,
        //Reserved = 4,

        /// <summary>
        /// Positioning or location identification.
        /// </summary>
        PositioningService = 8,

        /// <summary>
        /// Networking, for example, LAN, Ad hoc.
        /// </summary>
        NetworkingService = 16,

        /// <summary>
        /// Rendering, for example, printer, speakers.
        /// </summary>
        RenderingService = 32,

        /// <summary>
        /// Capturing, for example, scanner, microphone.
        /// </summary>
        CapturingService = 64,

        /// <summary>
        /// Object Transfer, for example, v-Inbox, v-folder.
        /// </summary>
        ObjectTransferService = 128,

        /// <summary>
        /// Audio, for example, speaker, microphone, headset service.
        /// </summary>
        AudioService = 256,

        /// <summary>
        /// Telephony, for example cordless, modem, headset service.
        /// </summary>
        TelephoneService = 512,

        /// <summary>
        /// Information, for example, web server, WAP server.
        /// </summary>
        InformationService = 1024,
    }
}