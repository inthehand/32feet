﻿// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefMessage
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Collections.Generic;

namespace InTheHand.Nfc;

/// <summary>
/// The content of any NDEF message
/// </summary>
public partial class NdefMessage
{
    private readonly List<NdefRecord> _records = [];

    /// <summary>
    ///  A list of NDEF records defining the NDEF message.
    /// </summary>
    public IReadOnlyCollection<NdefRecord> Records => _records.AsReadOnly();

    internal void AddRecord(NdefRecord record)
    {
        _records.Add(record);
    }
}