// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefRecord
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Nfc
{
    public sealed class NdefRecord
    {
        /// <summary>
        /// The NDEF record type.
        /// </summary>
        public string RecordType { get; internal set; }

        /// <summary>
        /// The MIME type of the NDEF record payload.
        /// </summary>
        public string MediaType { get; internal set; }

        /// <summary>
        /// The record identifier, which is an absolute or relative URL.
        /// </summary>
        public string Id { get; internal set; }

        public object Data { get; internal set; }


        public string Encoding { get; internal set; }

        public string Language { get; internal set; }
    }
}