// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Obex.ObexClient
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Net.Obex
{
    /// <preliminary/>
    public sealed class ObexClient : IDisposable
    {
        private Stream _stream;

        private bool disposedValue;

        private Uri _baseAddress;
        public Uri BaseAddress
        {
            get
            {
                return _baseAddress;
            }
            set
            {
                if (_baseAddress != value)
                {
                    if (ObexParser.GetObexTransportFromHost(value.Host) != ObexTransport.Unknown)
                    {
                        _baseAddress = value;
                    }
                    else
                    {
                        throw new UriFormatException("Invalid OBEX Uri Host");
                    }
                }
            }
        }

        public Headers.ObexRequestHeaders DefaultRequestHeaders { get; } = new Headers.ObexRequestHeaders();

        public async Task<bool> ConnectAsync()
        {
            return false;
        }

        public async Task<bool> DisconnectAsync()
        {
            return false;
        }

        public async Task<ObexResponseMessage> GetAsync()
        {
            return null;
        }

        public async Task<ObexResponseMessage> PutAsync(ObexContent content)
        {
            return null;
        }

        public async Task<ObexResponseMessage> SetPathAsync(string path)
        {
            return null;
        }

        public async Task<ObexResponseMessage> AbortAsync()
        {
            return null;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _stream?.Dispose();
                    _stream = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ObexClient()
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
