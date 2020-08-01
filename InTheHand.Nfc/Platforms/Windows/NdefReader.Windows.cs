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
                subscription = options.RecordType;
            }
            subscriptionReference = proximityDevice.SubscribeForMessage(subscription, MessageReceived);

            return Task.FromException(new NotImplementedException());
        }

        private void MessageReceived(ProximityDevice sender, ProximityMessage message)
        {
            NdefMessage ndefMessage = new NdefMessage();
            ndefMessage.AddRecord(new NdefRecord
            {
                Data = message.Data.ToArray(),
                RecordType = message.MessageType
            });
        }
    }
}
