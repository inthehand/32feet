// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.iOS.ExternalAccessoryNetworkStream (iOS)
// 
// Copyright (c) 2018-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using ExternalAccessory;
using Foundation;
using System;
using System.IO;

namespace InTheHand.Net.Sockets.iOS
{
    internal class ExternalAccessoryNetworkStream : NonSocketNetworkStream
    {
        private readonly NSInputStream _inputStream;
        private readonly NSOutputStream _outputStream;
        private MemoryStream _outputBuffer = null;
        private int _outputBufferOffset = 0;
        private readonly EAStreamDelegate _delegate;
        private bool _streamReady = false;
        private readonly object _lockObject = new object();

        internal ExternalAccessoryNetworkStream(EASession session)
        {
            _delegate = new EAStreamDelegate(this);
            _inputStream = session.InputStream;
            _outputStream = session.OutputStream;

            _inputStream.Open();
            _outputStream.Delegate = _delegate;
            _outputStream.Schedule(NSRunLoop.Current, NSRunLoopMode.Default);
            _outputStream.Open();
        }

        internal class EAStreamDelegate : NSStreamDelegate
        {
            private readonly ExternalAccessoryNetworkStream _stream;

            internal EAStreamDelegate(ExternalAccessoryNetworkStream stream)
            {
                _stream = stream;
            }

            public override void HandleEvent(NSStream theStream, NSStreamEvent streamEvent)
            {
                if (theStream == _stream._outputStream)
                {
                    switch (streamEvent)
                    {
                        case NSStreamEvent.OpenCompleted:
                            _stream._streamReady = true;
                            break;
                            
                        case NSStreamEvent.HasSpaceAvailable:
                            //Make sure that nobody else changes the _outputBuffer while writing to the accessory.
                            lock (_stream._lockObject)
                            {
                              _stream.WriteToAccessory();
                            }
                            break;

                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(_inputStream != null)
            {
                _inputStream.Close();
                _inputStream.Dispose();
            }

            if(_outputStream != null)
            {
                _outputStream.Close();
                _outputStream.Unschedule(NSRunLoop.Current, NSRunLoopMode.Default);
                _outputStream.Dispose();
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
          // As this method can be called by external code, it should not contain any
          // functions that affect the _outputStream. 
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
            // Make sure that the delegate does not change the _outputBuffer while the
            // application layer is writing data to it.
            lock (_lockObject)
            {
                if (_outputBuffer == null)
                {
                    _outputBuffer = new MemoryStream();
                }
                else
                {
                    throw new InvalidDataException("InTheHand - Write: The previous request is still in progress.");
                }
                // Write the incoming data to the _outputBuffer and set the offset to 0.
                // This is the initial state.
                _outputBuffer.Write(buffer, offset, count);
                _outputBufferOffset = 0;

                // If no request is being handled and the output stream is writable,
                // write the data directly to the output stream
                if (_streamReady)
                {
                    if (_outputStream.HasSpaceAvailable())
                    {
                      // Set the following flag to “false” to ensure that each subsequent request
                      // waits until this request is completed.
                      _streamReady = false;
                      WriteToAccessory();
                    }
                    else
                    {
                        throw new InvalidDataException("InTheHand - Write: Stream is ready but there is no space available!");
                    }
                }
            }
        }

        /// <summary>
        /// According to Apple's Stream Programming Guide
        /// (https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/Streams/Articles/WritingOutputStreams.html#//apple_ref/doc/uid/20002274-1002233)
        /// This method can be called either by the application layer or by the delegate.
        /// If it is called by the delegate, this means that the output stream is ready to send more data.
        /// If the data is too large to be sent in one go, it can be sent in several packets. As soon as
        /// no more data is written to the output stream in the context of the delegate, it stops sending
        /// the “HasSpaceAvailable” event. At this point, the _streamReady flag is set to true so that the
        /// application layer can write to the output stream again.
        /// </summary>
        internal void WriteToAccessory()
        {
            if (_outputBuffer != null && _outputBuffer.Length > 0)
            {
                // Get the amount of data to write to the accessory.
                var bufferLength = _outputBuffer.Length - _outputBufferOffset;
                // Create a buffer with the capacity of the amount of the data to be written.
                byte[] buffer = new byte[bufferLength];
                // Set the position in the _outputBuffer to the current offset.
                _outputBuffer.Seek(_outputBufferOffset, SeekOrigin.Begin);
                // Copy the data from the _outputBuffer to the previously created buffer.
                _outputBuffer.Read(buffer, 0, buffer.Length);
                // Write the buffer to the output stream of the accessory.
                var lengthWritten = _outputStream.Write(buffer);
                // Recalculate the offset of the data already written to the output stream for the _outputBuffer. 
                _outputBufferOffset += (int)lengthWritten;
                // If all data of the _outputBuffer has been written to the output stream, the _outputBuffer is
                // disposed.
                if (_outputBufferOffset >= _outputBuffer.Length)
                {
                    _outputBuffer.Dispose();
                    _outputBuffer = null;
                }
            }
            else
            {
              // Set following flag to true to enable the application layer to write further data.
              _streamReady = true;
            }
        }

        public override bool DataAvailable => _inputStream.HasBytesAvailable();
    }
}