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

// ReSharper disable CheckNamespace
namespace InTheHand.Nfc;

partial class NdefReader
{
    private static Task<bool> PlatformGetAvailability() => Task.FromResult(NFCReaderSession.ReadingAvailable);

    private readonly ReaderSessionDelegate _sessionDelegate;
    private NFCNdefReaderSession _session;
    private CancellationToken _cancellationToken;
    private INFCNdefTag _currentTag;
    

    /// <summary>
    /// Creates a new instance of NdefReader.
    /// </summary>
    public NdefReader()
    {
        _sessionDelegate = new ReaderSessionDelegate(this);

    }

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        if (!NFCReaderSession.ReadingAvailable)
        {
            throw new InvalidOperationException("NFC scanning not available");
        }

        if (_session is { Ready: true })
        {
            throw new InvalidOperationException("Session already active");
        }

        _session = new NFCNdefReaderSession(_sessionDelegate, null, !_cancellationToken.CanBeCanceled);

        _cancellationToken = cancellationToken;
        if (_cancellationToken.CanBeCanceled)
        {
            _cancellationToken.Register(CancelScan);
        }

        _session.BeginSession();

        return Task.CompletedTask;
    }

    private void CancelScan()
    {
        if(_session.Ready)
            _session.InvalidateSession();
    }

    private Task PlatformWriteAsync(NdefMessage message, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource();

        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() => { tcs.SetCanceled(cancellationToken); });
        }

        _currentTag?.WriteNdef(message, (error) =>
        {
            if(error != null)
                Error?.Invoke(this, new NdefErrorEventArgs(new InvalidOperationException(error.ToString())));
            
            tcs.SetResult();
        });

        return tcs.Task;
    }

    private sealed class ReaderSessionDelegate(NdefReader owner) : NFCNdefReaderSessionDelegate
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

                owner.Reading?.Invoke(this, new NdefReadingEventArgs(newMessage));
            }
        }

        public override void DidDetectTags(NFCNdefReaderSession session, INFCNdefTag[] tags)
        {
            owner._currentTag = tags[0];
            
            tags[0].ReadNdef((message, error) =>
            {
                if (error != null)
                {
                    owner.Error?.Invoke(owner, new NdefErrorEventArgs(new InvalidOperationException(error.ToString())));
                    
                }
                else
                {
                    var newMessage = new NdefMessage();
                    foreach (var record in message.Records)
                    {
                        var parsedRecord = ParseNdefRecord(record);
                        newMessage.AddRecord(parsedRecord);
                    }

                    owner.Reading?.Invoke(owner, new NdefReadingEventArgs(newMessage));
                }
            });
        }

        public override void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            // filter which codes are errors
            System.Diagnostics.Debug.WriteLine($"DidInvalidate {error}");
            if (error.Code != 0xC8 && error.Code != 0xCC)
            { 
                owner.Error?.Invoke(owner, new NdefErrorEventArgs(new InvalidOperationException(error.ToString())));
            }
        }
    }

    private static NdefRecord ParseNdefRecord(NFCNdefPayload payload)
    {
        var typeString = System.Text.Encoding.UTF8.GetString([.. payload.Type]);
        var parsedRecord = new NdefRecord()
        {
            Id = System.Text.Encoding.UTF8.GetString([.. payload.Identifier]),
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
                parsedRecord.Data = new Uri(urlText);
                break;

            case NdefRecordType.AbsoluteUri:
                parsedRecord.Data = new Uri(typeString);
                break;

            case NdefRecordType.Text:
                var textDataArray = payload.Payload.ToArray();
                var header = textDataArray[0];
                var languageLength = header & 0x1f;
                bool unicode = (header & 0x40) != 0;
                parsedRecord.Encoding = unicode ? "utf-16" : "utf-8";
                if (languageLength > 0)
                {
                    parsedRecord.Language = System.Text.Encoding.ASCII.GetString(textDataArray, 1, languageLength);
                }

                if (unicode)
                {
                    parsedRecord.Data = System.Text.Encoding.Unicode.GetString(textDataArray, languageLength + 1, textDataArray.Length - (languageLength + 1));
                }
                else
                {
                    parsedRecord.Data = System.Text.Encoding.UTF8.GetString(textDataArray, languageLength + 1, textDataArray.Length - (languageLength + 1));
                }
                
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
        return format switch
        {
            NFCTypeNameFormat.Empty => NdefRecordType.Empty,
            NFCTypeNameFormat.NFCWellKnown => ndefType switch
            {
                "T" => NdefRecordType.Text,
                "U" => NdefRecordType.Url,
                "Sp" => NdefRecordType.SmartPoster,
                _ => ":" + ndefType
            },
            NFCTypeNameFormat.Media => NdefRecordType.Mime,
            NFCTypeNameFormat.AbsoluteUri => NdefRecordType.AbsoluteUri,
            NFCTypeNameFormat.NFCExternal => ndefType,
            _ => NdefRecordType.Unknown
        };
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        _disposed = true;
        CancelScan();
    }
}