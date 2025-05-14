//
// InTheHand.Nfc.NdefMessage (Apple)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using CoreNFC;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefMessage
{
    private NFCNdefMessage _message;

    public static implicit operator NFCNdefMessage(NdefMessage message)
    {
        return message._message;
    }

    public static implicit operator NdefMessage(NFCNdefMessage message)
    {
        return message == null ? null : new NdefMessage(message);
    }

    internal NdefMessage(NFCNdefMessage message)
    {
        _message = message;
    }

    private void PlatformParseRecords(NdefRecord[] records)
    {
        var nativeRecords = new NFCNdefPayload[records.Length];
        for (var i = 0; i < nativeRecords.Length; i++)
        {
            nativeRecords[i] = records[i];
        }

        _message = new NFCNdefMessage(nativeRecords);
    }
}