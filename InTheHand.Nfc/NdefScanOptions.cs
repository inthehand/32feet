// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefScanOptions
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Threading;

namespace InTheHand.Nfc
{
    /// <preliminary/>
    public class NdefScanOptions
    {
        public string Id { get; set; }

        public string RecordType { get; set; }

        public string MediaType { get; set; }

        public CancellationToken Signal { get; set; }     
    }
}
