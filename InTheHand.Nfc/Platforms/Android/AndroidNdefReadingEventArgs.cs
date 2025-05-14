// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.AndroidNdefReadingEventArgs
// 
// Copyright (c) 2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using Android.Nfc.Tech;

// ReSharper disable once CheckNamespace
namespace InTheHand.Nfc
{
    internal sealed class AndroidNdefReadingEventArgs(Ndef ndef, NdefMessage message, string serialNumber)
        : NdefReadingEventArgs(message, serialNumber), IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            ndef?.Dispose();
        }

        ~AndroidNdefReadingEventArgs()
        {
            Dispose(false);
        }
    }
}
