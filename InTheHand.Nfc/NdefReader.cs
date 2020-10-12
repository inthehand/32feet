// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Nfc.NdefReader
// 
// Copyright (c) 2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;

namespace InTheHand.Nfc
{
    /// <summary>
    /// Class to read NDEF formatted NFC tags.
    /// </summary>
    /// <remarks>See https://w3c.github.io/web-nfc/ for more details.</remarks>
    public sealed partial class NdefReader : IDisposable
    {
        private bool disposedValue;

        public Task ScanAsync(NdefScanOptions options = null)
        {
            return PlatformScanAsync(options);
        }

        public event EventHandler<NdefMessage> Reading;

        public event EventHandler Error;

        

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~NdefReader()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
