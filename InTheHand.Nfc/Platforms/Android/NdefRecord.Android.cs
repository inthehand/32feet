// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord (Android)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using ANfc = Android.Nfc;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefRecord
{
    private readonly ANfc.NdefRecord _record;

    public static implicit operator ANfc.NdefRecord(NdefRecord record)
    {
        return record._record;
    }

    public static implicit operator NdefRecord(ANfc.NdefRecord record)
    {
        return record == null ? null : new NdefRecord(record);
    }

    private static NdefRecord PlatformCreateEmpty()
    {
        return new NdefRecord(new ANfc.NdefRecord(ANfc.NdefRecord.TnfEmpty, null, null, null));
    }

    private static NdefRecord PlatformCreateText(string text, string language = null)
    {
        return new NdefRecord(ANfc.NdefRecord.CreateTextRecord(language, text));
    }

    private static NdefRecord PlatformCreateUri(Uri uri)
    {
        return new NdefRecord(ANfc.NdefRecord.CreateUri(Android.Net.Uri.Parse(uri.ToString())));
    }

    private static NdefRecord PlatformCreateMime(string mimeType, byte[] data)
    {
        return new NdefRecord(ANfc.NdefRecord.CreateMime(mimeType, data));
    }

    internal NdefRecord(ANfc.NdefRecord record)
    {
        _record = record;
    }
}