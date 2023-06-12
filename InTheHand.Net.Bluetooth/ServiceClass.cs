// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.ServiceClass
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Class of Service flags as assigned in the Bluetooth specifications.
    /// </summary>
    /// -
    /// <remarks>
    /// <para>Is returned by the property <see
    /// cref="P:InTheHand.Net.Bluetooth.ClassOfDevice.Service">ClassOfDevice.Service</see>.
    /// </para>
    /// <para>Defined in Bluetooth Specifications <see href="https://www.bluetooth.com/specifications/assigned-numbers/"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="DeviceClass"/>
    [Flags]
    public enum ServiceClass
    {
        /// <summary>
        /// No service class bits set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Information (WEB­server, WAP­server, ...)
        /// </summary>
        Information = 0x0400,//0x800000,
        /// <summary>
        /// Telephony (Cordless telephony, Modem, Headset service, ...)
        /// </summary>
        Telephony = 0x0200,//0x400000,
        /// <summary>
        /// Audio (Speaker, Microphone, Headset service, ...)
        /// </summary>
        Audio = 0x0100,//0x200000,
        /// <summary>
        /// Object Transfer (v­Inbox, v­Folder, ...)
        /// </summary>
        ObjectTransfer = 0x0080,//0x100000,
        /// <summary>
        /// Capturing (Scanner, Microphone, ...)
        /// </summary>
        Capturing = 0x0040,//0x080000,
        /// <summary>
        /// Rendering (Printing, Speakers, ...)
        /// </summary>
        Rendering = 0x0020,//0x040000,
        /// <summary>
        /// Networking (LAN, Ad hoc, ...)
        /// </summary>
        Network = 0x0010,//0x020000,
        /// <summary>
        /// Positioning (Location identification)
        /// </summary>
        Positioning = 0x0008,//0x010000,
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        Reserved = 0x0004, //0x008000,
        /// <summary>
        /// Low-energy audio.
        /// </summary>
        LEAudio = 0x00002, //0x004000,
        /// <summary>
        /// Limited Discoverable Mode.
        /// </summary>
        LimitedDiscoverableMode = 0x0001, //0x002000,
    }
}