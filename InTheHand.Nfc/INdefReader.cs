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
    /// Start scanning for NDEF tags.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to stop scanning.</param>
    /// <returns></returns>
    /// <remarks>If no cancellation token is passed the reader will end the session after the first Nfc tag is scanned.</remarks>
    Task ScanAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Notify that a new reading is available.
    /// </summary>
    event EventHandler<NdefReadingEventArgs> Reading;

    /// <summary>
    /// Notify that an error happened during reading.
    /// </summary>
    event EventHandler Error;
}