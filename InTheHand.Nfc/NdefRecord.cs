// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Nfc;

/// <summary>
/// The content of any NDEF record.
/// </summary>
public sealed class NdefRecord
{
    /// <summary>
    /// Creates an empty NDEF record.
    /// </summary>
    /// <returns>A new empty NDEF record.</returns>
    public static NdefRecord CreateEmpty()
    {
        return new NdefRecord { RecordType = NdefRecordType.Unknown };
    }

    /// <summary>
    /// Creates a text record.
    /// </summary>
    /// <param name="text">Plain text content.</param>
    /// <param name="language">Optional IANA language tag.</param>
    /// <returns>A new text NDEF record.</returns>
    public static NdefRecord CreateText(string text, string language = null)
    {
        return new NdefRecord { RecordType = NdefRecordType.Text, Data = text, Language = language };
    }

    /// <summary>
    /// Creates a Uri record.
    /// </summary>
    /// <param name="uri">Uri to use as data for the record.</param>
    /// <returns>A new Uri NDEF record.</returns>
    public static NdefRecord CreateUri(Uri uri)
    {
        return new NdefRecord { RecordType = NdefRecordType.Url, Data = uri };
    }

    /// <summary>
    /// The NDEF record type.
    /// </summary>
    public string RecordType { get; internal set; }

    /// <summary>
    /// The MIME type of the NDEF record payload.
    /// </summary>
    /// <remarks>Only valid for RecordType NdefRecordType.Mime.</remarks>
    public string MediaType { get; internal set; }

    /// <summary>
    /// The record identifier, which is an absolute or relative URL.
    /// </summary>
    public string Id { get; internal set; }

    /// <summary>
    /// The raw data for the record
    /// </summary>
    /// <remarks>For Uri types this is a <see cref="System.Uri"/>, for text types this is a <see cref="string"/>, for other types a <see cref="T:Byte[]"/>.</remarks>
    public object Data { get; internal set; }

    /// <summary>
    /// Represents the encoding name used for encoding the payload in the case it is textual data.
    /// </summary>
    /// <remarks>Only valid for RecordType NdefRecordType.Text.</remarks>
    public string Encoding { get; internal set; }

    /// <summary>
    /// Represents the language tag of the payload in the case that was encoded.
    /// </summary>
    /// <remarks>Only valid for RecordType <see cref="NdefRecordType.Text"/>.
    /// A language tag is a string that matches the production of a Language-Tag defined in the [BCP47] specifications
    /// (see the IANA Language Subtag Registry for an authoritative list of possible values).
    /// That is, a language range is composed of one or more subtags that are delimited by a U+002D HYPHEN-MINUS ("-").
    /// For example, the 'en-AU' language range represents English as spoken in Australia, and 'fr-CA' represents French as spoken in Canada.
    /// Language tags that meet the validity criteria of [RFC5646] section 2.2.9 that can be verified without reference to the IANA Language Subtag Registry are considered structurally valid.</remarks>
    public string Language { get; internal set; }
}