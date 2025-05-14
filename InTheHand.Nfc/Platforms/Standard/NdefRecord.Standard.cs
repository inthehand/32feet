// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefRecord
{
    private static NdefRecord PlatformCreateEmpty()
    {
        return new NdefRecord { RecordType = NdefRecordType.Unknown };
    }

    private static NdefRecord PlatformCreateText(string text, string language = null)
    {
        return new NdefRecord { RecordType = NdefRecordType.Text, Data = text, Language = language };
    }

    private static NdefRecord PlatformCreateUri(Uri uri)
    {
        return new NdefRecord { RecordType = NdefRecordType.Url, Data = uri };
    }

    private static NdefRecord PlatformCreateMime(string mimeType, byte[] data)
    {
        return new NdefRecord { RecordType = NdefRecordType.Mime, MediaType = mimeType, Data = data };
    }
}