using System;
using System.IO;

namespace InTheHand.Net.Tests.BlueSoleil
{
    class MostlyErrorsStream : Stream
    {
        const string ObjName = "MostlyErrorsStream";
        //
        volatile bool _disposed;

        internal MostlyErrorsStream()
        {
        }

        //----
        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }

        //----
        public override bool CanRead
        {
            get
            {
                return true;
                throw new NotImplementedException();
            }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
                throw new NotImplementedException();
            }
        }

        public override void Flush() // Done
        {
            if (_disposed)
                throw new ObjectDisposedException(ObjName);
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                throw new ObjectDisposedException("MostlyErrorsStream");
            throw new NotImplementedException();
        }

    }
}