using System;

namespace InTheHand.Nfc;

public sealed class NdefReadingEventArgs : EventArgs
{
    internal NdefReadingEventArgs(string serialNumber, NdefMessage message)
    {
        SerialNumber = serialNumber;
        Message = message;
    }

    public string SerialNumber { get; init; }

    public NdefMessage Message { get; init; }
}