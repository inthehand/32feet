// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.iOS.ExternalAccessoryNetworkStream (iOS)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using ExternalAccessory;
using Foundation;
using System;
using System.IO;

namespace InTheHand.Net.Sockets.iOS
{
    internal class ExternalAccessoryNetworkStream : NetworkStream
    {
        private NSInputStream _inputStream;
        private NSOutputStream _outputStream;
        MemoryStream _outputBuffer = new MemoryStream();
        EAStreamDelegate _delegate;
        bool _streamReady = false;

        internal ExternalAccessoryNetworkStream(EASession session)
        {
            _delegate = new EAStreamDelegate(this);
            _inputStream = session.InputStream;
            _outputStream = session.OutputStream;

            _inputStream.Open();
            _outputStream.Delegate = _delegate;
            _outputStream.Schedule(NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
            _outputStream.Open();
        }

        internal class EAStreamDelegate : NSStreamDelegate
        {
            private ExternalAccessoryNetworkStream _stream;

            internal EAStreamDelegate(ExternalAccessoryNetworkStream stream)
            {
                _stream = stream;
            }

            public override void HandleEvent(NSStream theStream, NSStreamEvent streamEvent)
            {
                if (theStream == _stream._outputStream && streamEvent == NSStreamEvent.HasSpaceAvailable)
                {
                    _stream._streamReady = true;
                    _stream.Flush();
                    
                }
            }
        }

        protected override void Dispose(bool disposing)
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
                _outputStream.Unschedule(NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
                _outputStream.Dispose();
                _outputStream = null;
            }

            base.Dispose(disposing);
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position { get => 0; set => throw new NotSupportedException(); }
        
        public override void Flush()
        {
            if (_outputBuffer is object && _outputBuffer.Length > 0)
            {
                byte[] buffer = new byte[_outputBuffer.Length];
                _outputBuffer.Read(buffer, 0, buffer.Length);
                _outputBuffer.Dispose();
                _outputBuffer = null;
                _outputStream.Write(buffer);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return (int)_inputStream.Read(buffer, offset, (nuint)count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_streamReady)
            {
                _outputStream.Write(buffer, offset, (nuint)count);
            }
            else
            {
                _outputBuffer.Write(buffer, offset, count);
            }
        }

        public override bool DataAvailable
        {
            get
            {
                return _inputStream.HasBytesAvailable();
            }
        }
    }
}