// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;
using CoreNFC;
using Foundation;

namespace InTheHand.Nfc
{
    partial class NdefReader
    {
        readonly NFCNdefReaderSession session;
        readonly ReaderSessionDelegate sessionDelegate;

        public NdefReader()
        {
            sessionDelegate = new ReaderSessionDelegate(this);
            session = new NFCNdefReaderSession(sessionDelegate, null, false);
        }

        private Task PlatformScanAsync(NdefScanOptions options = null)
        {
            session?.BeginSession();

            if (options != null)
            {
                if (options.Signal != null)
                {
                    options.Signal.Register(Cancelled);
                }
            }
            return Task.CompletedTask;
        }

        private void Cancelled()
        {
            session.InvalidateSession();
        }

        private class ReaderSessionDelegate : NFCNdefReaderSessionDelegate
        {
            readonly NdefReader reader;

            public ReaderSessionDelegate(NdefReader owner)
            {
                reader = owner;
            }

            public override void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
            {
                foreach (var message in messages)
                {
                    NdefMessage newMessage = new NdefMessage();
                    foreach (var record in message.Records)
                    {
                        var parsedRecord = ParseNdefRecord(record);
                        newMessage.AddRecord(parsedRecord);
                    }

                    reader.Reading?.Invoke(this, newMessage);
                }
            }

            public override void DidInvalidate(NFCNdefReaderSession session, NSError error)
            {
                // filter which codes are errors
                System.Diagnostics.Debug.WriteLine(error);
                if (error.Code != 0xC8)
                {
                    reader.Error?.Invoke(reader, EventArgs.Empty);
                }
            }
        }

        private static NdefRecord ParseNdefRecord(NFCNdefPayload payload)
        {
            string typeString = System.Text.Encoding.UTF8.GetString(payload.Type.ToArray());
            var parsedRecord = new NdefRecord()
            {
                Id = System.Text.Encoding.UTF8.GetString(payload.Identifier.ToArray()),
                RecordType = ParseRecordType(payload.TypeNameFormat, typeString)
            };

            switch(parsedRecord.RecordType)
            {
                case NdefRecordType.Url:
                    byte[] dataArray = payload.Payload.ToArray();
                    string urlText = string.Empty;
                    switch(dataArray[0])
                    {
                        case 1:
                            urlText = "http://www.";
                            break;

                        case 2:
                            urlText = "https://www.";
                            break;
                        case 3:
                            urlText = "http://";
                            break;

                        case 4:
                            urlText = "https://";
                            break;

                    }
                    urlText  += System.Text.Encoding.UTF8.GetString(dataArray, 1, dataArray.Length - 1);
                    parsedRecord.Data = urlText;
                    break;

                case NdefRecordType.AbsoluteUri:
                    parsedRecord.Data = typeString;
                    break;

                case NdefRecordType.Text:
                    byte[] textDataArray = payload.Payload.ToArray();
                    byte header = textDataArray[0];
                    int languageLength = header & 0x1f;
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

                    switch (ndefType)
                    {
                        case "T":
                            return NdefRecordType.Text;

                        case "U":
                            return NdefRecordType.Url;

                        case "Sp":
                            return NdefRecordType.SmartPoster;

                        default:
                            return ":" + ndefType;
                    }

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
    }
}
