// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader (Android)
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Nfc;
using Android.Nfc.Tech;
using Activity = Android.App.Activity;

[assembly: UsesPermission("android.permission.NFC")]
[assembly: UsesFeature("android.hardware.nfc", Required = false)]

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefReader(Activity activity) : Java.Lang.Object, NfcAdapter.IReaderCallback
{
    private static readonly NfcAdapter SAdapter = NfcAdapter.GetDefaultAdapter(Application.Context);

    private static Task<bool> PlatformGetAvailability() => Task.FromResult(SAdapter is { IsEnabled: true });

    private bool _autoCancel;

    private Ndef _currentNdef;

    public NdefReader() : this(AndroidActivity.CurrentActivity)
    {
    }

    void NfcAdapter.IReaderCallback.OnTagDiscovered(Tag tag)
    {
        var techList = tag?.GetTechList();
        if (techList == null) return;

        foreach (var tech in techList)
        {
            if (tech != "android.nfc.tech.Ndef") continue;
            
            var serialBytes = tag.GetId();
            var serial = FormatSerialNumber(serialBytes);

            var ndef = Ndef.Get(tag);

            // this shouldn't happen given the tech property
            if (ndef == null)
                break;

            try
            {
                _currentNdef = ndef;
                ndef.Connect();
                Reading?.Invoke(this, new AndroidNdefReadingEventArgs(ndef, ndef.NdefMessage, serial));
                break;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Error?.Invoke(this, EventArgs.Empty);
                break;
            }
            finally
            {
                // if no valid cancellation token, end the session after first NFC scan
                if (_autoCancel)
                {
                    Task.Run(async () =>
                    {
                        // add a pause to avoid double scanning
                        await Task.Delay(2000);
                        CancelScan();
                    });
                    
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
            builder.Append(':');
        }

        if (builder.Length > 0)
            builder.Length -= 1;

        return builder.ToString();
    }

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(CancelScan);
        }
        else
        {
            _autoCancel = true;
        }

        SAdapter.EnableReaderMode(activity, this,
            NfcReaderFlags.NfcA | NfcReaderFlags.NfcB | NfcReaderFlags.NfcF | NfcReaderFlags.NfcV, null);
        return Task.CompletedTask;
    }

    private void CancelScan()
    {
        SAdapter.DisableReaderMode(activity);
        _currentNdef?.Dispose();
    }

    private async Task PlatformWriteAsync(NdefMessage message, CancellationToken cancellationToken)
    {
        if (_currentNdef == null)
            throw new InvalidOperationException("No tag currently in range");

        if (!_currentNdef.IsConnected)
        {
            _currentNdef.Connect();
        }

        if (_currentNdef.IsWritable)
        {
            await _currentNdef.WriteNdefMessageAsync(message);
        }
        else
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            CancelScan();

            _disposed = true;
        }

        base.Dispose(disposing);
    }
}