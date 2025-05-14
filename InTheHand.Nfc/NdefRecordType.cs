// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecordType
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Nfc;

/// <summary>
/// Specifies the type of the NDEF record.
/// </summary>
public static class NdefRecordType
{
    /// <summary>
    /// Record has no contents.
    /// </summary>
    public const string Empty = "empty";
    /// <summary>
    /// Record contains text.
    /// </summary>
    public const string Text = "text";
    /// <summary>
    /// Record contains a Uri.
    /// </summary>
    public const string Url = "url";
    /// <summary>
    /// Record contains a smart poster. This will include a Uri and optional text and image data.
    /// </summary>
    public const string SmartPoster = "smart-poster";
    /// <summary>
    /// Record contains Mime data. <see cref="NdefRecord.MediaType"/> will contain the specific type.
    /// </summary>
    public const string Mime = "mime";
    /// <summary>
    /// Record contains an absolute Uri.
    /// </summary>
    public const string AbsoluteUri = "absolute-url";
    /// <summary>
    /// The contents of the record are unknown.
    /// </summary>
    public const string Unknown = "unknown";
}