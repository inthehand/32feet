// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.AndroidNetworkStream (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;
using System.IO;

namespace InTheHand.Net.Bluetooth.Droid
{
    internal sealed class AndroidNetworkStream : NetworkStream
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

        public override bool CanWrite => _outputStream.CanWrite;

        public override long Length => _inputStream.Length;

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush()
        {
            _outputStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inputStream.Read(buffer, offset, count);
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
            _outputStream.Write(buffer, offset, count);
        }

        public override bool DataAvailable
        {
            get
            {
                try
                {
                    var js = _inputStream as Android.Runtime.InputStreamInvoker;
                    return js.BaseInputStream.Available() > 0;
                }
                catch (Java.IO.IOException)
                {
                }

                return false;
            }
        }
    }
}