// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord (Apple)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
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
        return new NdefRecord(NFCNdefPayload.CreateWellKnownTypePayload(text,
            string.IsNullOrEmpty(language) ? NSLocale.SystemLocale : NSLocale.FromLocaleIdentifier(language)));
    }

    private static NdefRecord PlatformCreateUri(Uri uri)
    {
        return new NdefRecord(NFCNdefPayload.CreateWellKnownTypePayload(uri));
    }

    private static NdefRecord PlatformCreateMime(string mimeType, byte[] data)
    {
        return new NdefRecord(new NFCNdefPayload(NFCTypeNameFormat.Media, NSData.FromString(mimeType),
            null, NSData.FromArray(data)));
    }

    internal NdefRecord(NFCNdefPayload record)
    {
        _record = record;
    }
}