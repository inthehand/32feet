// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.ObexHeader
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net
{
    /// <summary>
    /// Header identifiers to describe some aspect of the object being transferred.
    /// </summary>
    public enum ObexHeader : byte
    {
        // OBEX Header codes
        None = 0x0,

        /// <summary>
        /// String describing the name of the object.
        /// </summary>
        /// <remarks>Unicode String. Often a file name.</remarks>
        Name = 0x01,

        /// <summary>
        /// Text description of the object.
        /// </summary>
        /// <remarks>Unicode String.</remarks>
        Description = 0x05,

        /// <summary>
        /// Type of object - e.g. text, html, binary, manufacturer specific 
        /// </summary>
        /// <remarks>Byte sequence. ASCII encoded MIME defined media type.</remarks>
        Type = 0x42,

        /// <summary>
        /// Date/time stamp – ISO 8601 version - preferred 
        /// </summary>
        /// <remarks>Byte sequence. ASCII encoded ISO 8601 DateTime string.</remarks>
        Time = 0x44,

        /// <summary>
        /// Name of service that operation is targeted to.
        /// </summary>
        /// <remarks>Byte sequence. Usually a Uuid for a well-known target.</remarks>
        Target = 0x46,

        /// <summary>
        /// An HTTP 1.x header.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        Http = 0x47,

        /// <summary>
        /// A chunk of the object body. 
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        Body = 0x48,
        /// <summary>
        /// The final chunk of the object body. 
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        EndOfBody = 0x49,

        /// <summary>
        /// Identifies the OBEX application, used to tell if talking to a peer.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        Who = 0x4A,

        /// <summary>
        /// Extended application request & response information.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        AppParameters = 0x4C,

        /// <summary>
        /// Authentication digest-challenge.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        AuthChallenge = 0x4D,

        /// <summary>
        /// Authentication digest-response.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        AuthResponse = 0x4E,

        /// <summary>
        /// OBEX Object class of object.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        [Obsolete("Use ObexHeader.ObjectClass instead", false)]
        ObjectClassOld = 0x4F,

        /// <summary>
        /// Uniquely identifies the network client (OBEX server).
        /// </summary>
        /// <remarks>Byte sequence. 16-byte UUID.</remarks>
        WanUuid = 0x50,

        /// <summary>
        /// OBEX Object class of object.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        ObjectClass = 0x51,

        /// <summary>
        /// Parameters used in session commands/responses.
        /// </summary>
        /// <remarks>Byte sequence.</remarks>
        SessionParameters = 0x52,

        /// <summary>
        /// Sequence number used in each OBEX packet for reliability.
        /// </summary>
        /// <remarks>Byte.</remarks>
        SessionSequenceNumber = 0x93,

        /// <summary>
        /// Number of objects (used by Connect).
        /// </summary>
        /// <remarks>4-byte unsigned integer.</remarks>
        Count = 0xC0,

        /// <summary>
        /// The length of the object in bytes.
        /// </summary>
        /// <remarks>4-byte unsigned integer.</remarks>
        Length = 0xC3,

        /// <summary>
        /// Date/time stamp – 4-byte version.
        /// </summary>
        /// <remarks>4-byte unsigned integer.</remarks>
        [Obsolete("Use ObexHeader.Time instead", false)]
        Time4Byte = 0xC4,

        /// <summary>
        /// An identifier used for OBEX connection multiplexing.
        /// </summary>
        /// <remarks>4-byte unsigned integer.</remarks>
        ConnectionID = 0xCB,

        /// <summary>
        /// Indicates the creator of an object.
        /// </summary>
        /// <remarks>4-byte unsigned integer.</remarks>
        CreatorID = 0xCF,

        //user-defined headers
        User0 = 0x30,
        User1 = 0x31,
        User2 = 0x32,
        User3 = 0x33,
        User4 = 0x34,
        User5 = 0x35,
        User6 = 0x36,
        User7 = 0x37,
        User8 = 0x38,
        User9 = 0x39,
        User10 = 0x3A,
        User11 = 0x3B,
        User12 = 0x3C,
        User13 = 0x3D,
        User14 = 0x3E,
        User15 = 0x3F,
    }
}
