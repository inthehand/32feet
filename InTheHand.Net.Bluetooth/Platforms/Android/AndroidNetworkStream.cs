// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.AndroidNetworkStream (Android)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
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

        /// <inheritdoc/>
        public AndroidNetworkStream(Stream input, Stream output)
        {
            _inputStream = input;
            _outputStream = output;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool CanRead => _inputStream.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => false;

        /// <inheritdoc/>
        public override bool CanTimeout => false;

        /// <inheritdoc/>
        public override bool CanWrite => _outputStream.CanWrite;

        /// <inheritdoc/>
        public override bool DataAvailable => Length > 0;

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

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                return GetAvailable(_inputStream as Android.Runtime.InputStreamInvoker);
            }
        }

        /// <inheritdoc/>
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        /// <inheritdoc/>
        public override int ReadTimeout { get => _inputStream.ReadTimeout; set => _inputStream.ReadTimeout = value; }

        /// <inheritdoc/>
        public override int WriteTimeout { get => _outputStream.WriteTimeout; set => _outputStream.WriteTimeout = value; }


        /// <inheritdoc/>
        public override void Flush()
        {
            _outputStream.Flush();
        }

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _outputStream.FlushAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return _inputStream.BeginRead(buffer, offset, size, callback, state);
        }

        /// <inheritdoc/>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return _inputStream.EndRead(asyncResult);
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inputStream.Read(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override int Read(Span<byte> destination)
        {
            return _inputStream.Read(destination);
        }

        /// <inheritdoc/>
        public override int ReadByte()
        {
            return _inputStream.ReadByte();
        }

        /// <inheritdoc/>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _inputStream.ReadAsync(buffer, offset, size, cancellationToken);
        }

        /// <inheritdoc/>
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            return _inputStream.ReadAsync(buffer, cancellationToken);
        }

        /// <inheritdoc/>
        public override void CopyTo(Stream destination, int bufferSize)
        {
            _inputStream.CopyTo(destination, bufferSize);
        }

        /// <inheritdoc/>
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _inputStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return _outputStream.BeginWrite(buffer, offset, size, callback, state);
        }

        /// <inheritdoc/>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            _outputStream.EndWrite(asyncResult);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _outputStream.Write(buffer, offset, count);
        }

        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> source)
        {
            _outputStream.Write(source);
        }

        /// <inheritdoc/>
        public override void WriteByte(byte value)
        {
            _outputStream.WriteByte(value);
        }

        /// <inheritdoc/>
        public override Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _outputStream.WriteAsync(buffer, offset, size, cancellationToken);
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            return _outputStream.WriteAsync(buffer, cancellationToken);
        }
    }
}