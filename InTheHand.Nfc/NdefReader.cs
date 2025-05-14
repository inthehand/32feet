// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Nfc;

/// <summary>
/// Class to read NDEF formatted NFC tags.
/// </summary>
/// <remarks>See https://w3c.github.io/web-nfc/ for more details.</remarks>
public sealed partial class NdefReader : INdefReader
#if !ANDROID
    , IDisposable
#endif
{
    private bool _disposed;

    /// <summary>
    /// Gets a value to indicate whether NFC reading is available on this device.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetAvailability() => PlatformGetAvailability();

    /// <summary>
    /// Start scanning for NDEF tags.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to stop scanning.</param>
    /// <returns></returns>
    /// <remarks>If no cancellation token is passed the reader will end the session after the first Nfc tag is scanned.</remarks>
    public Task ScanAsync(CancellationToken cancellationToken = default)
    {
        return PlatformScanAsync(cancellationToken);
    }

    public Task WriteAsync(NdefMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        return PlatformWriteAsync(message, cancellationToken);
    }
    /// <summary>
    /// Notify that a new reading is available.
    /// </summary>
    public event EventHandler<NdefReadingEventArgs> Reading;

    /// <summary>
    /// Notify that an error happened during reading.
    /// </summary>
    public event EventHandler Error;

#if !ANDROID
    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
#endif

    /// <inheritdoc/>
    ~NdefReader()
    {
        Dispose(false);
    }
}