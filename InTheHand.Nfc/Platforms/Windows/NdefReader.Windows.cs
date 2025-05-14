// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader (Windows)
// 
// Copyright (c) 2020-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc;

partial class NdefReader
{
    private static readonly ProximityDevice ProximityDevice = ProximityDevice.GetDefault();
    
    private static Task<bool> PlatformGetAvailability() => Task.FromResult(ProximityDevice != null);

    private const string NdefMessageType = "NDEF";
    private long _subscriptionReference;

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        if (ProximityDevice is null)
            throw new InvalidOperationException("NFC Scanning unavailable");
        
        cancellationToken.Register(Unsubscribe);
        _subscriptionReference = ProximityDevice.SubscribeForMessage(NdefMessageType, MessageReceived);

        return Task.CompletedTask;
    }

    private Task PlatformWriteAsync(NdefMessage message, CancellationToken cancellationToken)
    {
        return Task.FromException(new PlatformNotSupportedException());
    }

    private void Unsubscribe()
    {
        if (_subscriptionReference == 0) return;

        ProximityDevice.StopSubscribingForMessage(_subscriptionReference);
        _subscriptionReference = 0;
    }

    private void MessageReceived(ProximityDevice sender, ProximityMessage message)
    {
        var ndefMessage = new NdefMessage();
        ndefMessage.AddRecord(new NdefRecord
        {
            Data = message.Data.ToArray(),
            RecordType = message.MessageType
        });

        Reading?.Invoke(this, new NdefReadingEventArgs(ndefMessage));
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        _disposed = true;
        Unsubscribe();
    }
}