// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace InTheHand.Nfc
{
    partial class NdefReader
    {
        ProximityDevice proximityDevice = ProximityDevice.GetDefault();
        long subscriptionReference;

        private Task PlatformScanAsync(NdefScanOptions options = null)
        {
            string subscription = "NDEF";
            if(options != null)
            {
                if (options.Signal != null)
                    options.Signal.Register(Unsubscribe);

                if(!string.IsNullOrEmpty(options.RecordType))
                    subscription = options.RecordType;
            }

            subscriptionReference = proximityDevice.SubscribeForMessage(subscription, MessageReceived);

            return Task.CompletedTask;
        }

        private void Unsubscribe()
        {
            if (subscriptionReference != 0)
            {
                proximityDevice.StopSubscribingForMessage(subscriptionReference);
                subscriptionReference = 0;
            }
        }

        private void MessageReceived(ProximityDevice sender, ProximityMessage message)
        {
            NdefMessage ndefMessage = new NdefMessage();
            ndefMessage.AddRecord(new NdefRecord
            {
                Data = message.Data.ToArray(),
                RecordType = message.MessageType
            });

            Reading?.Invoke(this, ndefMessage);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Unsubscribe();
                proximityDevice = null;

                disposedValue = true;
            }
        }
    }
}
