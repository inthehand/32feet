// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefMessage (Android)
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Text;
using ANfc = Android.Nfc;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefMessage
{
    private ANfc.Tag _tag;
    private ANfc.NdefMessage _message;

    public static implicit operator ANfc.NdefMessage(NdefMessage message)
    {
        return message._message;
    }

    public static implicit operator ANfc.Tag(NdefMessage message)
    {
        return message._tag;
    }

    public static implicit operator NdefMessage(ANfc.NdefMessage message)
    {
        return message == null ? null : new NdefMessage(null, message);
    }

    internal NdefMessage(ANfc.Tag tag, ANfc.NdefMessage message)
    {
        _tag = tag;
        _message = message;

        var records = message.GetRecords();

        if (records == null) return;

        foreach (var record in records)
        {
            switch (record.Tnf)
            {
                case ANfc.NdefRecord.TnfEmpty:
                    AddRecord(new NdefRecord { RecordType = NdefRecordType.Empty });
                    break;

                case ANfc.NdefRecord.TnfWellKnown:
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
                            AddRecord(GetTextRecord(record));
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

                case ANfc.NdefRecord.TnfMimeMedia:
                    AddRecord(new NdefRecord
                    {
                        RecordType = NdefRecordType.Mime,
                        Data = record.GetPayload(),
                        Id = GetId(record),
                        MediaType = record.ToMimeType()
                    });
                    break;

                case ANfc.NdefRecord.TnfAbsoluteUri:
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

        string GetId(ANfc.NdefRecord record)
        {
            return Encoding.UTF8.GetString(record.GetId() ?? ReadOnlySpan<byte>.Empty);
        }

        NdefRecord GetTextRecord(ANfc.NdefRecord record)
        {
            var textRecord = new NdefRecord
            {
                RecordType = NdefRecordType.Text,
                Id = GetId(record)
            };

            var payload = record.GetPayload();

            if (payload is null)
                return textRecord;

            var languageCodeLength = payload[0] & 0x1f;
            var encoding = payload[0] & 0x80;

            textRecord.Language = Encoding.UTF8.GetString(payload, 1, languageCodeLength);

            if (encoding != 0)
            {
                textRecord.Encoding = "utf-16";
                textRecord.Data = Encoding.Unicode.GetString(payload, 1 + languageCodeLength,
                    payload.Length - languageCodeLength - 1);
            }
            else
            {
                textRecord.Encoding = "utf-8";
                textRecord.Data = Encoding.UTF8.GetString(payload, 1 + languageCodeLength,
                    payload.Length - languageCodeLength - 1);
            }

            return textRecord;
        }
    }

    private void PlatformParseRecords(NdefRecord[] records)
    {
        var nativeRecords = new ANfc.NdefRecord[records.Length];
        for (int i = 0; i < records.Length; i++)
        {
            nativeRecords[i] = records[i];
        }

        _message = new ANfc.NdefMessage(nativeRecords);
    }
}