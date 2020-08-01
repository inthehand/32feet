// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefMessage
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Collections.Generic;

namespace InTheHand.Nfc
{
    public class NdefMessage
    {
        private List<NdefRecord> _records = new List<NdefRecord>();

        public IReadOnlyCollection<NdefRecord> Records 
        { 
            get
            {
                return _records.AsReadOnly();
            }
        }

        internal void AddRecord(NdefRecord record)
        {
            _records.Add(record);
        }
    }
}