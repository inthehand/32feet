// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.AndroidNetworkStream (Android)
// 
// Copyright (c) 2003-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth.Droid
{
    internal sealed class AndroidNetworkStream : NonSocketNetworkStream
    {
        private Stream _inputStream;
        private Stream _outputStream;

        public AndroidNetworkStream(Stream input, Stream output)
        {
            _inputStream = input;
            _outputStream = output;
        }

        public override void Close()
        {
            if(_inputStream is object)
            {
                _inputStream.Close();
                _inputStream.Dispose();
                _inputStream = null;
            }

            if(_outputStream is object)
            {
                _outputStream.Close();
                _outputStream.Dispose();
                _outputStream = null;
            }

            base.Close();
        }

        public override bool CanRead => _inputStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanTimeout => false;

        public override bool CanWrite => _outputStream.CanWrite;

        public override bool DataAvailable
        {
            get
            {
                return Length > 0;
            }
        }

        internal static int GetAvailable(Android.Runtime.InputStreamInvoker stream)
        {
            if (stream == null)
                return 0;

            try
            {
                return stream.BaseInputStream.Available();
            }
            catch (Java.IO.IOException)
            {
            }

            return 0;
        }

        public override long Length
        {
            get
            {
                return GetAvailable(_inputStream as Android.Runtime.InputStreamInvoker);
            }
        }

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush()
        {
            _outputStream.Flush();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _outputStream.FlushAsync(cancellationToken);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return _inputStream.BeginRead(buffer, offset, size, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _inputStream.EndRead(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inputStream.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> destination)
        {
            return _inputStream.Read(destination);
        }

        public override int ReadByte()
        {
            return _inputStream.ReadByte();
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _inputStream.ReadAsync(buffer, offset, size, cancellationToken);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            return _inputStream.ReadAsync(buffer, cancellationToken);
        }

        public override int ReadTimeout { get => _inputStream.ReadTimeout; set => _inputStream.ReadTimeout = value; }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            _inputStream.CopyTo(destination, bufferSize);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _inputStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return _outputStream.BeginWrite(buffer, offset, size, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _outputStream.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _outputStream.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> source)
        {
            _outputStream.Write(source);
        }

        public override void WriteByte(byte value)
        {
            _outputStream.WriteByte(value);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _outputStream.WriteAsync(buffer, offset, size, cancellationToken);
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            return _outputStream.WriteAsync(buffer, cancellationToken);
        }

        public override int WriteTimeout { get => _outputStream.WriteTimeout; set => _outputStream.WriteTimeout = value; }
    }
}