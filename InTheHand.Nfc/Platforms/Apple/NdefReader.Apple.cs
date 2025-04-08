// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader (Apple)
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading;
using System.Threading.Tasks;
using CoreNFC;
using Foundation;

namespace InTheHand.Nfc;

partial class NdefReader
{
    private NFCNdefReaderSession _session;
    private readonly ReaderSessionDelegate _sessionDelegate;

    public NdefReader()
    {
        _sessionDelegate = new ReaderSessionDelegate(this);
    }

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        if (!NFCReaderSession.ReadingAvailable)
            throw new InvalidOperationException("NFC scanning not available");
        
        if (_session is { Ready: true })
            throw new InvalidOperationException("Session already started");
        
        // if a default cancellation token end session after single read
        _session ??= new NFCNdefReaderSession(_sessionDelegate, null, !cancellationToken.CanBeCanceled);

        _session.BeginSession();

        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(CancelScan);
        }

        return Task.CompletedTask;
    }

    private void CancelScan()
    {
        _session.InvalidateSession();
        _session = null;
    }

    private class ReaderSessionDelegate(NdefReader owner) : NFCNdefReaderSessionDelegate
    {
        public override void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            foreach (var message in messages)
            {
                var newMessage = new NdefMessage();
                foreach (var record in message.Records)
                {
                    var parsedRecord = ParseNdefRecord(record);
                    newMessage.AddRecord(parsedRecord);
                }

                owner.Reading?.Invoke(this, new NdefReadingEventArgs(string.Empty, newMessage));
            }
        }

        public override void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            // filter which codes are errors
            System.Diagnostics.Debug.WriteLine(error);
            if (error.Code != 0xC8)
            {
                owner.Error?.Invoke(owner, EventArgs.Empty);
            }
        }
    }

    private static NdefRecord ParseNdefRecord(NFCNdefPayload payload)
    {
        var typeString = System.Text.Encoding.UTF8.GetString(payload.Type.ToArray());
        var parsedRecord = new NdefRecord()
        {
            Id = System.Text.Encoding.UTF8.GetString(payload.Identifier.ToArray()),
            RecordType = ParseRecordType(payload.TypeNameFormat, typeString)
        };

        switch(parsedRecord.RecordType)
        {
            case NdefRecordType.Url:
                var dataArray = payload.Payload.ToArray();
                var urlText = dataArray[0] switch
                {
                    1 => "http://www.",
                    2 => "https://www.",
                    3 => "http://",
                    4 => "https://",
                    _ => string.Empty
                };

                urlText  += System.Text.Encoding.UTF8.GetString(dataArray, 1, dataArray.Length - 1);
                parsedRecord.Data = urlText;
                break;

            case NdefRecordType.AbsoluteUri:
                parsedRecord.Data = typeString;
                break;

            case NdefRecordType.Text:
                var textDataArray = payload.Payload.ToArray();
                var header = textDataArray[0];
                var languageLength = header & 0x1f;
                parsedRecord.Encoding = (header & 0x40) == 0 ? "utf-8" : "utf-16be";
                parsedRecord.Language = System.Text.Encoding.ASCII.GetString(textDataArray, 1, languageLength);
                parsedRecord.Data = System.Text.Encoding.UTF8.GetString(textDataArray, languageLength + 1, textDataArray.Length - (languageLength + 1));
                break;

            case NdefRecordType.Mime:
                parsedRecord.MediaType = typeString;
                parsedRecord.Data = payload.Payload.ToArray();
                break;

            default:
                parsedRecord.Data = payload.Payload.ToArray();
                break;
        }

        return parsedRecord;
    }

    private static string ParseRecordType(NFCTypeNameFormat format, string ndefType)
    {
        switch (format)
        {
            case NFCTypeNameFormat.Empty:
                return NdefRecordType.Empty;

            case NFCTypeNameFormat.NFCWellKnown:

                return ndefType switch
                {
                    "T" => NdefRecordType.Text,
                    "U" => NdefRecordType.Url,
                    "Sp" => NdefRecordType.SmartPoster,
                    _ => ":" + ndefType
                };

            case NFCTypeNameFormat.Media:
                return NdefRecordType.Mime;

            case NFCTypeNameFormat.AbsoluteUri:
                return NdefRecordType.AbsoluteUri;

            case NFCTypeNameFormat.NFCExternal:
                return ndefType;

            default:
                return NdefRecordType.Unknown;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            CancelScan();
            _disposed = true;
        }
    }
}