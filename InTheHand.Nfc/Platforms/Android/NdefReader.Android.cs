// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;

[assembly: UsesPermission("android.permission.NFC")]
[assembly: UsesFeature("android.hardware.nfc", Required = false)]

namespace InTheHand.Nfc
{
    partial class NdefReader : Java.Lang.Object, NfcAdapter.IReaderCallback
    {
        static readonly NfcAdapter s_adapter = NfcAdapter.GetDefaultAdapter(Application.Context);

        void NfcAdapter.IReaderCallback.OnTagDiscovered(Tag tag)
        {
            var techList = tag.GetTechList();
            foreach(string tech in techList)
            {
                if(tech == "android.nfc.tech.Ndef")
                {
                    var ndef = Ndef.Get(tag);
                    ndef.Connect();
                    var msg = ndef.NdefMessage;
                    ndef.Dispose();
                    
                    // parse message

                    Reading?.Invoke(this, null);
                    break;
                }
            }
        }

        private async Task PlatformScanAsync(NdefScanOptions options = null)
        {
            s_adapter.EnableReaderMode(Xamarin.Essentials.Platform.CurrentActivity, this, NfcReaderFlags.NfcA | NfcReaderFlags.NfcB | NfcReaderFlags.NfcF | NfcReaderFlags.NfcV, null);
        }
    }
}
