// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Nfc;

[assembly: UsesPermission("android.permission.NFC")]

namespace InTheHand.Nfc
{
    partial class NdefReader
    {
        static readonly NfcAdapter s_adapter = NfcAdapter.GetDefaultAdapter(Application.Context);

        private Task PlatformScanAsync(NdefScanOptions options = null)
        {
            return Task.FromException(new PlatformNotSupportedException());
        }
    }
}
