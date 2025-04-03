// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Nfc;

/// <summary>
/// The content of any NDEF record.
/// </summary>
public sealed class NdefRecord
{
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

    public object Data { get; internal set; }

    /// <summary>
    /// Represents the encoding name used for encoding the payload in the case it is textual data.
    /// </summary>
    /// <remarks>Only valid for RecordType NdefRecordType.Text.</remarks>
    public string Encoding { get; internal set; }

    /// <summary>
    /// Represents the language tag of the payload in the case that was encoded.
    /// </summary>
    /// <remarks>Only valid for RecordType NdefRecordType.Text.
    /// A language tag is a string that matches the production of a Language-Tag defined in the [BCP47] specifications (see the IANA Language Subtag Registry for an authoritative list of possible values).
    /// That is, a language range is composed of one or more subtags that are delimited by a U+002D HYPHEN-MINUS ("-").
    /// For example, the 'en-AU' language range represents English as spoken in Australia, and 'fr-CA' represents French as spoken in Canada.
    /// Language tags that meet the validity criteria of [RFC5646] section 2.2.9 that can be verified without reference to the IANA Language Subtag Registry are considered structurally valid.</remarks>
    public string Language { get; internal set; }
}