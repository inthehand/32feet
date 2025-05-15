// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.INdefReader
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Nfc;

/// <summary>
/// Defines an NDEF tag reader.
/// </summary>
public interface INdefReader
{
    /// <summary>
    /// Gets a value to indicate whether NFC reading is available on this device.
    /// </summary>
    /// <returns>True if scanning is available, otherwise false.</returns>
    Task<bool> GetAvailability();

    /// <summary>
    /// Start scanning for NDEF tags.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to stop scanning.</param>
    /// <returns></returns>
    /// <remarks>If no cancellation token is passed the reader will end the session after the first Nfc tag is scanned.</remarks>
    Task ScanAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Write a message to the current tag.
    /// </summary>
    /// <param name="message">NDEF message containing one or more records.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteAsync(NdefMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Notify that a new reading is available.
    /// </summary>
    event EventHandler<NdefReadingEventArgs> Reading;

    /// <summary>
    /// Notify that an error happened during reading.
    /// </summary>
    event EventHandler<NdefErrorEventArgs> Error;
}