// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader (Android)
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Nfc;
using Android.Nfc.Tech;
using Microsoft.Maui.ApplicationModel;

[assembly: UsesPermission("android.permission.NFC")]
[assembly: UsesFeature("android.hardware.nfc", Required = false)]

namespace InTheHand.Nfc;

partial class NdefReader : Java.Lang.Object, NfcAdapter.IReaderCallback
{
    static readonly NfcAdapter s_adapter = NfcAdapter.GetDefaultAdapter(Application.Context);

    void NfcAdapter.IReaderCallback.OnTagDiscovered(Tag tag)
    {
        var techList = tag?.GetTechList();
        if (techList == null) return;

        foreach (var tech in techList)
        {
            if (tech == "android.nfc.tech.Ndef")
            {
                var serialBytes = tag.GetId();
                var serial = FormatSerialNumber(serialBytes);

                var ndef = Ndef.Get(tag);

                // this shouldn't happen given the tech property
                if (ndef == null)
                    break;

                try
                {
                    ndef.Connect();
                    var msg = new NdefMessage(ndef.NdefMessage);
                    ndef.Dispose();

                    Reading?.Invoke(this, new NdefReadingEventArgs(serial, msg));
                    break;
                }
                catch (Exception e)
                {
                    break;
                }

            }
        }
    }

    private static string FormatSerialNumber(byte[] id)
    {
        var builder = new StringBuilder();

        foreach (var b in id)
        {
            builder.Append(b.ToString("X2"));
            builder.Append(":");
        }

        if (builder.Length > 0)
            builder.Length -= 1;

        return builder.ToString();
    }

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        cancellationToken.Register(CancelScan);
        s_adapter.EnableReaderMode(Platform.CurrentActivity, this,
            NfcReaderFlags.NfcA | NfcReaderFlags.NfcB | NfcReaderFlags.NfcF | NfcReaderFlags.NfcV, null);
        return Task.CompletedTask;
    }

    private void CancelScan()
    {
        s_adapter.DisableReaderMode(Platform.CurrentActivity);
    }

    protected override void Dispose(bool disposing)
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

        base.Dispose(disposing);
    }
}