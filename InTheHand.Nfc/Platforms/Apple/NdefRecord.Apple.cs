// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord (Apple)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Linq;
using CoreNFC;
using Foundation;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefRecord
{
    private readonly NFCNdefPayload _record;

    public static implicit operator NFCNdefPayload(NdefRecord record)
    {
        return record._record;
    }

    public static implicit operator NdefRecord(NFCNdefPayload record)
    {
        return record == null ? null : new NdefRecord(record);
    }
    private static NdefRecord PlatformCreateEmpty()
    {
        return new NdefRecord { RecordType = NdefRecordType.Unknown };
    }

    private static NdefRecord PlatformCreateText(string text, string language = null)
    {
        var langLength = 2;
        var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
        byte[] langBytes = [0x65, 0x6E];
        if (!string.IsNullOrEmpty(language))
        {
            langBytes = System.Text.Encoding.UTF8.GetBytes(language);
            langLength = langBytes.Length;
        }

        var payloadBytes = new byte[1 + langLength + textBytes.Length];
        payloadBytes[0] = (byte)langLength;
        langBytes.CopyTo(payloadBytes, 1);

        textBytes.CopyTo(payloadBytes, 1 + langLength);
        
        return new NdefRecord(new NFCNdefPayload(NFCTypeNameFormat.NFCWellKnown, NSData.FromString("T"), NSData.FromArray([]), NSData.FromArray(payloadBytes)));
    }

    private static NdefRecord PlatformCreateUri(Uri uri)
    {
        return new NdefRecord(NFCNdefPayload.CreateWellKnownTypePayload(uri));
    }

    private static NdefRecord PlatformCreateMime(string mimeType, byte[] data)
    {
        return new NdefRecord(new NFCNdefPayload(NFCTypeNameFormat.Media, NSData.FromString(mimeType),
            NSData.FromArray([]), NSData.FromArray(data)));
    }

    internal NdefRecord(NFCNdefPayload record)
    {
        _record = record;
    }
}