// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.WinRTNetworkStream (WinRT)
// 
// Copyright (c) 2021-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    /// <exclude/>
    public sealed class WinRTNetworkStream : NonSocketNetworkStream
    {
        private readonly StreamSocket _socket;
        private readonly Stream _inputStream;
        private readonly Stream _outputStream;

        private readonly bool _ownsSocket;

        public WinRTNetworkStream(StreamSocket socket, bool ownsSocket)
        {
            if (socket is null)
                throw new ArgumentNullException(nameof(socket));

            _socket = socket;
            _inputStream = _socket.InputStream.AsStreamForRead();
            _outputStream = _socket.OutputStream.AsStreamForWrite();
            _ownsSocket = ownsSocket;
        }

        public override void Close()
        {
            if (_ownsSocket)
            {
                _inputStream.Close();
                _outputStream.Close();
                _socket.Dispose();
            }

            base.Close();
        }

        public override bool DataAvailable
        {
            get
            {
                return _inputStream.Length > 0;
            }
        }

        public override bool CanRead => _inputStream.CanRead;

        public override bool CanSeek => false;

        public override long Length => _inputStream.Length;

        public override bool CanWrite => _outputStream.CanWrite;

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override int ReadTimeout { get => _inputStream.ReadTimeout; set => _inputStream.ReadTimeout = value; }

        public override int WriteTimeout { get => _outputStream.WriteTimeout; set => _outputStream.WriteTimeout = value; }


        public override void CopyTo(Stream destination, int bufferSize)
        {
            _inputStream.CopyTo(destination, bufferSize);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _inputStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

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

        public override int ReadByte()
        {
            return _inputStream.ReadByte();
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _inputStream.ReadAsync(buffer, offset, size, cancellationToken);
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

        public override void WriteByte(byte value)
        {
            _outputStream.WriteByte(value);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
        {
            return _outputStream.WriteAsync(buffer, offset, size, cancellationToken);
        }
    }
}
