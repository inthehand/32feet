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
    private const int AndroidCancelDelayMilliseconds = 2000;
    
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
                if (ndef.IsConnected && ndef.NdefMessage != null)
                {
                    activity.RunOnUiThread(() =>
                    {
                        Reading?.Invoke(this,
                            new AndroidNdefReadingEventArgs(ndef, new NdefMessage(tag, ndef.NdefMessage), serial));
                    });
                }

                break;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                activity.RunOnUiThread(() => Error?.Invoke(this, new NdefErrorEventArgs(e)));
                break;
            }
            finally
            {
                // if no valid cancellation token, end the session after first NFC scan
                if (_autoCancel)
                {
                    CancelScan();
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
        var tcs = new TaskCompletionSource();
        
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() => CancelScan());
        }
        else
        {
            _autoCancel = true;
        }

        SAdapter.EnableReaderMode(activity, this,
            NfcReaderFlags.NfcA | NfcReaderFlags.NfcB | NfcReaderFlags.NfcF | NfcReaderFlags.NfcV, null);

        return tcs.Task.WaitAsync(cancellationToken);
    }

    private async Task CancelScan()
    {
        await Task.Delay(AndroidCancelDelayMilliseconds);
        _currentNdef?.Dispose();
        _currentNdef = null;
        SAdapter.DisableReaderMode(activity);
    }

    private Task PlatformWriteAsync(NdefMessage message, CancellationToken cancellationToken)
    {
        if (_currentNdef == null)
            throw new InvalidOperationException("No tag currently in range");

        if (!_currentNdef.IsConnected)
        {
            _currentNdef.Connect();
        }

        if (_currentNdef.IsWritable)
        {
            var t = _currentNdef.WriteNdefMessageAsync(message);
            return t.WaitAsync(cancellationToken);
        }
        else
        {
            var ex = new InvalidOperationException("NFC tag is not writeable");
            activity.RunOnUiThread(() => Error?.Invoke(this, new NdefErrorEventArgs(ex)));
            return Task.FromException(ex);
        }
    }
    
    private async Task PlatformMakeReadOnlyAsync(CancellationToken cancellationToken)
    {
        if (_currentNdef == null)
            throw new InvalidOperationException("No tag currently in range");

        if (!_currentNdef.IsConnected)
        {
            _currentNdef.Connect();
        }

        if (_currentNdef.IsWritable && _currentNdef.CanMakeReadOnly())
        {
            var t = _currentNdef.MakeReadOnlyAsync();
            var success = await t.WaitAsync(cancellationToken);
            if (!success)
                throw new InvalidOperationException("Error making tag read-only");
        }
        else
        {
            var ex = new InvalidOperationException("NFC tag cannot be made read-only");
            activity.RunOnUiThread(() => Error?.Invoke(this, new NdefErrorEventArgs(ex)));
            throw ex;
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