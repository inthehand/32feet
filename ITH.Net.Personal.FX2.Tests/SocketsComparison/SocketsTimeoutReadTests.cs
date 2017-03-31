using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Net.Sockets;
using InTheHand.Net.Tests.Widcomm;
using System.IO;

namespace InTheHand.Net.Tests.SocketsComparison
{
    [TestFixture, Explicit]
    public class SocketsTimeoutReadTests : SocketsTimeoutTests
    {
        public SocketsTimeoutReadTests()
            : base(StreamOpTest.Read)
        {
        }

        // EoF in Read, 10057 in Write??
        protected override IAsyncResult BeginTimedRead_APreviousOpTimedOut(Stream stream)
        {
            return base.BeginTimedRead_NoError(stream);
        }
    }



    //[TestFixture, Explicit]
    //public class SocketsTimeoutWriteTests : SocketsTimeoutTests
    //{
    //    public SocketsTimeoutWriteTests()
    //        : base(StreamOpTest.Write)
    //    {
    //    }
    //}



    public abstract class SocketsTimeoutTests : NetworkStreamTimeoutTests
    {
        public SocketsTimeoutTests(StreamOpTest testType)
            : base(testType)
        {
        }

        //----
        public override void Between(int value, int min, int max, string message)
        {
            max += 550;
            base.Between(value, min, max, message);
        }


        TestSocketPair _tmpPair;

        protected override void Create(out System.IO.Stream peer, out ISocketPortBuffers socketBuffer)
        {
            TestSocketPair pair = new TestSocketPair();
            TestSocketPair was = Interlocked.Exchange(ref _tmpPair, pair);
            if (was != null) { was.Dispose(); }
            //
            peer = pair.StreamA;
            socketBuffer = new SocketBuffers(pair.SocketB);
        }

        class SocketBuffers : ISocketPortBuffers
        {
            Socket _sckFarEnd;

            public SocketBuffers(Socket farEnd)
            {
                _sckFarEnd = farEnd;
            }

            void ISocketPortBuffers.NewReceive(byte[] data)
            {
                _sckFarEnd.Send(data);
            }

            void ISocketPortBuffers.AllowWrite(ushort len)
            {
                throw new NotImplementedException();
            }
            void ISocketPortBuffers.AllowWrite(byte[] data)
            {
                ((ISocketPortBuffers)this).AllowWrite(checked((ushort)data.Length));
            }

            void IDisposable.Dispose()
            {
            }

        }

    }
}
