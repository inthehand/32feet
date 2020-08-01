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
    partial class NdefReader : INFCNdefReaderSessionDelegate
    {
        NFCNdefReaderSession session;

        public NdefReader()
        {
            session = new NFCNdefReaderSession(this, null, false);
        }

        private Task PlatformScanAsync(NdefScanOptions options = null)
        {
            session?.BeginSession();

            if(options != null)
            {
                if(options.Signal != null)
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

        public IntPtr Handle => throw new NotImplementedException();

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            foreach(var message in messages)
            {
                NdefMessage newMessage = new NdefMessage();
                foreach(var record in message.Records)
                {
                    newMessage.AddRecord(new NdefRecord() { Data = record.Payload.ToArray(), Id = System.Text.Encoding.UTF8.GetString(record.Identifier.ToArray()), RecordType = System.Text.Encoding.UTF8.GetString(record.Type.ToArray()) });
                }

                Reading?.Invoke(this, newMessage);
            }
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            System.Diagnostics.Debug.WriteLine(error);
        }

        public void Dispose()
        {
        }

        
    }
}
