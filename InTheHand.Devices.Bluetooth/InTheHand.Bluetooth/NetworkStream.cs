// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkStream.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-18 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.IO;
using System.Threading;
#if !UNITY
using System.Threading.Tasks;
#endif

#if __ANDROID__
using Android.Bluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using Windows.Networking.Sockets;
#elif UNITY
using InTheHand.Net.Sockets;
#endif

namespace InTheHand.Networking.Sockets
{
    /// <summary>
    /// Provides the underlying stream of data for network access.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    /// <seealso cref="System.Net.Sockets.NetworkStream"/>
    public sealed class NetworkStream : Stream
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
        private StreamSocket _socket;
        private Stream _inputStream;
        private Stream _outputStream;

        /// <summary>
        /// Creates a new instance of the NetworkStream for the specified <see cref="StreamSocket"/>
        /// </summary>
        /// <param name="socket"></param>
        public NetworkStream(StreamSocket socket)
        {
            _socket = socket;
            _inputStream = _socket.InputStream.AsStreamForRead();
            _outputStream = _socket.OutputStream.AsStreamForWrite();
        }

#elif __ANDROID__
        private BluetoothSocket _socket;

        /// <summary>
        /// Creates a new instance of the NetworkStream for the specified <see cref="BluetoothSocket"/>
        /// </summary>
        /// <param name="socket"></param>
        public NetworkStream(BluetoothSocket socket)
        {
            _socket = socket;
        }
#elif UNITY
        private MonoBluetoothSocket _socket;

        public NetworkStream(MonoBluetoothSocket socket)
        {
            _socket = socket;
        }
#endif

        /// <summary>
        /// Gets a value that indicates whether the NetworkStream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _inputStream.CanRead;

#elif __ANDROID__
                return _socket.InputStream.CanRead;

#elif UNITY
                return true;

#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the stream supports seeking. This property is not currently supported.
        /// </summary>
        /// <value>This property always returns false.</value>
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the NetworkStream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _outputStream.CanWrite;

#elif __ANDROID__
                return _socket.OutputStream.CanWrite;

#elif UNITY
                return true;

#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets the length of the data available on the stream. 
        /// This property is not currently supported and always throws a NotSupportedException.
        /// </summary>
        public override long Length
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
                return _inputStream.Length;

#elif __ANDROID__
                return _socket.InputStream.Length;

#elif UNITY
                return _socket.Available;

#else
                throw new NotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets or sets the current position in the stream.
        /// This property is not currently supported and always throws a NotSupportedException.
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Flushes data from the stream.
        /// </summary>
        public override void Flush()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream.Flush();

#elif __ANDROID__
            _socket.OutputStream.Flush();
#endif
        }

#if !UNITY
        /// <summary>
        /// Asynchronously clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _outputStream.FlushAsync(cancellationToken);

#elif __ANDROID__
            return _socket.OutputStream.FlushAsync(cancellationToken);

#else
            return Task.CompletedTask;
#endif
        }
#endif

        /// <summary>
        /// Reads data from the NetworkStream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _inputStream.Read(buffer, offset, count);

#elif __ANDROID__
            return _socket.InputStream.Read(buffer, offset, count);

#elif UNITY
            return _socket.Receive(buffer, count, 0);

#else
            return 0;
#endif
        }

#if !UNITY
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _inputStream.ReadAsync(buffer, offset, count, cancellationToken);

#elif __ANDROID__
            return _socket.InputStream.ReadAsync(buffer, offset, count, cancellationToken);

#else
            return Task.FromResult<int>(0);
#endif
        }
#endif

        /// <summary>
        /// Sets the current position of the stream to the given value.
        /// This method is not currently supported and always throws a NotSupportedException.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the length of the stream. 
        /// This method always throws a NotSupportedException.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes data to the NetworkStream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream.Write(buffer, offset, count);

#elif __ANDROID__
            _socket.OutputStream.Write(buffer, offset, count);

#elif UNITY
            _socket.Send(buffer, count, 0);
#endif
        }

#if !UNITY
        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _outputStream.WriteAsync(buffer, offset, count, cancellationToken);

#elif __ANDROID__
            return _socket.OutputStream.WriteAsync(buffer, offset, count, cancellationToken);

#else
            return Task.CompletedTask;
#endif
        }
#endif

            /// <summary>
            /// Releases the unmanaged resources used by the NetworkStream and optionally releases the managed resources.
            /// </summary>
            /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            _outputStream?.Dispose();
            _outputStream = null;
            _inputStream?.Dispose();
            _outputStream = null;
            _socket?.Dispose();
            _socket = null;

#elif __ANDROID__
            _socket?.Close();
            _socket?.Dispose();
            _socket = null;

#elif UNITY
            _socket?.Dispose();
            _socket = null;
#endif

            base.Dispose(disposing);
        }
    }
}