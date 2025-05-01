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

namespace InTheHand.Nfc;

partial class NdefReader
{
    private const string NdefMessageType = "NDEF";
    private ProximityDevice _proximityDevice = ProximityDevice.GetDefault();
    private long _subscriptionReference;

    private Task PlatformScanAsync(CancellationToken cancellationToken)
    {
        if (_proximityDevice is null)
            throw new InvalidOperationException("NFC Scanning unavailable");
        
        cancellationToken.Register(Unsubscribe);
        _subscriptionReference = _proximityDevice.SubscribeForMessage(NdefMessageType, MessageReceived);

        return Task.CompletedTask;
    }

    private void Unsubscribe()
    {
        if (_subscriptionReference == 0) return;

        _proximityDevice.StopSubscribingForMessage(_subscriptionReference);
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

        Reading?.Invoke(this, new NdefReadingEventArgs(string.Empty, ndefMessage));
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Unsubscribe();
            _proximityDevice = null;

            _disposed = true;
        }
    }
}