// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefMessage (Android)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Text;

namespace InTheHand.Nfc;

partial class NdefMessage
{
    internal NdefMessage(Android.Nfc.NdefMessage message)
    {
        var records = message.GetRecords();

        if (records == null) return;

        foreach (var record in records)
        {
            switch (record.Tnf)
            {
                case Android.Nfc.NdefRecord.TnfEmpty:
                    AddRecord(new NdefRecord { RecordType = NdefRecordType.Empty });
                    break;

                case Android.Nfc.NdefRecord.TnfWellKnown:
                    var typeString = Encoding.UTF8.GetString(record.GetTypeInfo()!);
                    switch (typeString)
                    {
                        case "U":
                            AddRecord(new NdefRecord
                            {
                                RecordType = NdefRecordType.Url,
                                Data = new Uri(record.ToUri().ToString()),
                                Id = GetId(record)
                            });
                            break;

                        case "T":
                            AddRecord(new NdefRecord
                            {
                                RecordType = NdefRecordType.Text,
                                Data = Encoding.UTF8.GetString(record.GetPayload() ?? ReadOnlySpan<byte>.Empty),
                                Id = GetId(record)
                            });
                            break;

                        case "Sp":
                            AddRecord(new NdefRecord
                            {
                                RecordType = NdefRecordType.SmartPoster,
                                Data = record.GetPayload(),
                                Id = GetId(record),
                                MediaType = record.ToMimeType()
                            });
                            break;
                    }
                    break;

                case Android.Nfc.NdefRecord.TnfMimeMedia:
                    AddRecord(new NdefRecord
                    {
                        RecordType = NdefRecordType.Mime,
                        Data = record.GetPayload(),
                        Id = GetId(record),
                        MediaType = record.ToMimeType()
                    });
                    break;

                case Android.Nfc.NdefRecord.TnfAbsoluteUri:
                    AddRecord(new NdefRecord
                    {
                        RecordType = NdefRecordType.AbsoluteUri,
                        Data = new Uri(record.ToUri().ToString()),
                        Id = GetId(record)
                    });
                    break;
                    
                default:
                    AddRecord(new NdefRecord
                    {
                        RecordType = NdefRecordType.Unknown, Data = record.GetPayload(),
                        Id = GetId(record),
                        MediaType = record.ToMimeType()
                    });
                    break;
            }
        }

        return;

        string GetId(Android.Nfc.NdefRecord record)
        {
            return Encoding.UTF8.GetString(record.GetId() ?? ReadOnlySpan<byte>.Empty);
        }
    }
}