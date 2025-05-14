// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReadingEventArgs
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Nfc;

/// <summary>
/// Contains details of a scanned NDEF tag.
/// </summary>
public class NdefReadingEventArgs : EventArgs
{
    /// <summary>
    /// Creates a new instance of NdefReadingEventArgs.
    /// </summary>
    /// <param name="message">The NDEF message.</param>
    public NdefReadingEventArgs(NdefMessage message) : this(message, string.Empty) { }

    /// <summary>
    /// Creates a new instance of NdefReadingEventArgs.
    /// </summary>
    /// <param name="message">The NDEF message.</param>
    /// <param name="serialNumber">Tag serial number if available.</param>
    public NdefReadingEventArgs(NdefMessage message, string serialNumber)
    {
        Message = message;
        SerialNumber = serialNumber;
    } 
    
    /// <summary>
    /// The NDEF message.
    /// </summary>
    public NdefMessage Message { get; init; }

    /// <summary>
    /// Unique serial number for the tag.
    /// </summary>
    /// <remarks>If not available contains <see cref="string.Empty"/>.</remarks>
    public string SerialNumber { get; init; }
}