using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using System.Threading;
using InTheHand.SystemCore;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommCommsReadTimeoutTests : WidcommCommsTimeoutTests
    {
        public WidcommCommsReadTimeoutTests()
            : base(StreamOpTest.Read)
        {
        }

        // EoF in Read, 10057 in Write??
        protected override IAsyncResult BeginTimedRead_APreviousOpTimedOut(Stream stream)
        {
            return base.BeginTimedRead_NoError(stream);
        }
    }


    [TestFixture]
    public class WidcommCommsWriteTimeoutTests : WidcommCommsTimeoutTests
    {
        public WidcommCommsWriteTimeoutTests()
            : base(StreamOpTest.Write)
        {
        }

        // EoF in Read, 10057 in Write??
        protected override IAsyncResult BeginTimedRead_APreviousOpTimedOut(Stream stream)
        {
            return base.BeginTimedRead_(stream, true, WSAENOTCONN);
        }
    }



    public abstract class WidcommCommsTimeoutTests : NetworkStreamTimeoutTests
    {
        public WidcommCommsTimeoutTests(StreamOpTest testType)
            : base(testType)
        {
        }

        //----
        internal void Create_ConnectedBluetoothClient(out TestRfCommIf rfcommIf, out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            WidcommBluetoothClientCommsTest.Create_ConnectedBluetoothClient(
                out rfcommIf, out port, out cli, out strm2);
        }

        protected override void Create(out Stream peer, out ISocketPortBuffers socketBuffer)
        {
            TestRfCommIf iface;
            BluetoothClient cli;
            Stream tmpStrm2;
            TestRfcommPort port;
            WidcommBluetoothClientCommsTest.Create_ConnectedBluetoothClient(out iface, out port, out cli, out tmpStrm2);
            peer = cli.GetStream();
            socketBuffer = new WidcommPortBuffers(port);
            // Start of with no capacity for write.
            socketBuffer.AllowWrite(0);
        }

        class WidcommPortBuffers : ISocketPortBuffers
        {
            TestRfcommPort _port;

            public WidcommPortBuffers(TestRfcommPort port)
            {
                _port = port;
            }
            void ISocketPortBuffers.NewReceive(byte[] data)
            {
                _port.NewReceive(data);
            }

            void ISocketPortBuffers.AllowWrite(byte[] data)
            {
                ((ISocketPortBuffers)this).AllowWrite(checked((ushort)data.Length));
            }
            void ISocketPortBuffers.AllowWrite(ushort len)
            {
                _port.SetWriteResult(len);
                // In case there's any writers already pending we need to release them.
                _port.NewEvent(InTheHand.Net.Bluetooth.Widcomm.PORT_EV.TXEMPTY);
            }

            void IDisposable.Dispose()
            {
                ushort? remaining = _port.GetWriteCapacity();
                Assert.AreEqual(0, remaining, "remaining");
            }
        }

    }

    public abstract class NetworkStreamTimeoutTests
    {
        public enum StreamOpTest { Read, Write }

        public const int WSAETIMEDOUT = 10060;
        public const int WSAENOTCONN = 10057;
        public const int WSAEWOULDBLOCK = 10035;
        //
        const int FudgeShorter = 3;
        const int FudgeLonger = 20;
        //
        static byte[] buf1 = new byte[1];
        //
        Converter<Stream, int> _theStreamOperation;
        Converter<Stream, IAsyncResult> _theBeginVersionOfTheStreamOperation;
        delegate void Action<T1, T2>(T1 p1, T2 p2);
        Action<ISocketPortBuffers, byte[]> _theReleaseOperation;
        System.Reflection.PropertyInfo _setTimeoutPi;

        //--------
        public NetworkStreamTimeoutTests(StreamOpTest testType)
        {
            if (testType == StreamOpTest.Write) {
                _theStreamOperation = TheWriteOperationUnderTest;
                _theBeginVersionOfTheStreamOperation = TheBeginVersionOfTheWriteOperation;
                _theReleaseOperation = ReleaseWriteOperation;
                _setTimeoutPi = typeof(Stream).GetProperty("WriteTimeout");
            } else {
                _theStreamOperation = TheReadOperationUnderTest;
                _theBeginVersionOfTheStreamOperation = TheBeginVersionOfTheReadOperation;
                _theReleaseOperation = ReleaseReadOperation;
                _setTimeoutPi = typeof(Stream).GetProperty("ReadTimeout");
            }
        }

        //--------
        void SetTimeoutValue(Stream strm, int timeout)
        {
            _setTimeoutPi.SetValue(strm, timeout, null);
        }

        int GetTimeoutValue(Stream strm)
        {
            object v = _setTimeoutPi.GetValue(strm, null);
            int value = (int)v;
            return value;
        }

        //--
        protected static int TheReadOperationUnderTest(Stream strm)
        {
            int readLen = strm.Read(buf1, 0, buf1.Length);
            return readLen;
        }

        protected static IAsyncResult TheBeginVersionOfTheReadOperation(Stream strm)
        {
            IAsyncResult ar = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            return ar;
        }

        protected static void ReleaseReadOperation(ISocketPortBuffers port, byte[] data)
        {
            port.NewReceive(data);
        }

        //--
        protected static int TheWriteOperationUnderTest(Stream strm)
        {
            strm.Write(buf1, 0, buf1.Length);
            return 1;
        }

        protected static IAsyncResult TheBeginVersionOfTheWriteOperation(Stream strm)
        {
            IAsyncResult ar = strm.BeginWrite(buf1, 0, buf1.Length, null, null);
            return ar;
        }

        protected static void ReleaseWriteOperation(ISocketPortBuffers port, byte[] data)
        {
            port.AllowWrite(data);
        }

        //--------
        protected void Create(out Stream peer)
        {
            ISocketPortBuffers port;
            Create(out peer, out port);
        }

        abstract protected void Create(out Stream peer, out ISocketPortBuffers socketBuffer);

        /// <summary>
        /// So we can have the RfcommPort/Socket receive more data (TODO , and allow write of some data).
        /// </summary>
        public interface ISocketPortBuffers : IDisposable
        {
            void NewReceive(byte[] data);
            void AllowWrite(ushort len);
            void AllowWrite(byte[] data);
        }

        //--------
        const int FiftyMsec = 50;
        const int TwoHundredMsec = 200;

        private void Read_FiveTimesOne_NoneTimeout(Stream peer)
        {
            Stopwatch timer = new Stopwatch();
            int readLen;
            for (int i = 0; i < 5; ++i) {
                timer.Reset();
                timer.Start();
                try {
                    readLen = _theStreamOperation(peer);
                    timer.Stop();
                    Assert.AreEqual(1, readLen, "readLen");
                } catch (IOException ioex) {
                    timer.Stop();
                    Assert.Fail("should NOT have thrown -- #" + i);
                    SocketException sex = (SocketException)ioex.InnerException;
                    //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                    Assert.AreEqual(WSAETIMEDOUT, sex.ErrorCode, "ErrorCode -- #" + i);
                }
                timer.Stop();
                Between(timer, 0, FiftyMsec / 2, "Between -- #" + i);
            }//for
        }

        private void Read_WillTimeout(Stream peer, int expectedTimeout)
        {
            int readLen;
            Stopwatch timer = new Stopwatch();
            //
            Assert.IsTrue(peer.CanRead, "canread 0");
            Assert.IsTrue(peer.CanWrite, "canwrite 0");
            //
            timer.Reset();
            timer.Start();
            try {
                readLen = _theStreamOperation(peer);
                timer.Stop();
                Assert.Fail("should have thrown");
            } catch (IOException ioex) {
                timer.Stop();
                SocketException sex = (SocketException)ioex.InnerException;
                //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                Assert.AreEqual(WSAETIMEDOUT, sex.ErrorCode, "ErrorCode");
            }
            Between(timer, expectedTimeout - FudgeShorter, FudgeLonger + expectedTimeout, "Between");
            //
            SetTimeoutValue(peer, -1);
            //
            readLen = -1;
            timer.Reset();
            timer.Start();
            //try {
            readLen = peer.Read(buf1, 0, buf1.Length);
            timer.Stop();
            Assert.AreEqual(0, readLen);
            //} catch (IOException ioex) { // SOCKETS throws a WSAEWOULDBLOCK here!
            //    timer.Stop();
            //    SocketException sex = (SocketException)ioex.InnerException;
            //    //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
            //    Assert.AreEqual(WSAEWOULDBLOCK, sex.ErrorCode, "ErrorCode");
            //}
            Assert.Less(timer.ElapsedMilliseconds, 20.0);
            Assert.IsTrue(peer.CanRead, "canread aR");
            Assert.IsTrue(peer.CanWrite, "canwrite aR");
            //
            timer.Reset();
            timer.Start();
            try {
                peer.Write(buf1, 0, buf1.Length);
                timer.Stop();
                Assert.Fail("should have thrown");
            } catch (IOException ioex) {
                timer.Stop();
                SocketException sex = (SocketException)ioex.InnerException;
                //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                Assert.AreEqual(WSAENOTCONN, sex.ErrorCode, "ErrorCode");
            }
            Assert.Less(timer.ElapsedMilliseconds, 20.0);
            Assert.IsFalse(peer.CanRead, "canread aW");
            Assert.IsFalse(peer.CanWrite, "canwrite aW");
            //
            readLen = -1;
            timer.Reset();
            timer.Start();
            try {
                readLen = peer.Read(buf1, 0, buf1.Length);
                timer.Stop();
                Assert.AreEqual(0, readLen);
                Assert.Fail("should have thrown");
            } catch (IOException ioex) {
                timer.Stop();
                SocketException sex = (SocketException)ioex.InnerException;
                //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                Assert.AreEqual(WSAENOTCONN, sex.ErrorCode, "ErrorCode");
            }
            Assert.Less(timer.ElapsedMilliseconds, 20.0);
            Assert.IsFalse(peer.CanRead, "canread aR2");
            Assert.IsFalse(peer.CanWrite, "canwrite aR2");
        }

        private void Read_FiveTimesOne_AllTimeoutXX(Stream peer, int expectedTimeout)
        {
            int readLen;
            Stopwatch timer = new Stopwatch();
            //
            for (int i = 0; i < 5; ++i) {
                timer.Reset();
                timer.Start();
                try {
                    readLen = _theStreamOperation(peer);
                    timer.Stop();
#if PLAY_CLOSED_AFTER_FIRST_TIMEOUT
                    if (readLen == 0) {
                        break; /*for*/
                    }
#endif
                    Assert.Fail("should have thrown -- #" + i);
                } catch (IOException ioex) {
                    timer.Stop();
                    SocketException sex = (SocketException)ioex.InnerException;
                    //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                    Assert.AreEqual(WSAETIMEDOUT, sex.ErrorCode, "ErrorCode -- #" + i);
                }
#if PLAY_CLOSED_AFTER_FIRST_TIMEOUT
                //!!
                Assert.Less(i, 2);
                //!!
#endif
                Between(timer, expectedTimeout - FudgeShorter, FudgeLonger + expectedTimeout, "Between -- #" + i);
            }//for
        }

        [Test]
        public void Read_Timeout50()
        {
            Stream peer;
            ISocketPortBuffers port;
            Create(out peer, out port);
            SetTimeoutValue(peer, FiftyMsec);
            //
            Read_WillTimeout(peer, FiftyMsec);
            peer.Close();
            port.Dispose();
        }

        [Test, Category("Slow")]
        public void Read_Timeout200()
        {
            Stream peer;
            ISocketPortBuffers port;
            Create(out peer, out port);
            SetTimeoutValue(peer, TwoHundredMsec);
            //
            Read_WillTimeout(peer, TwoHundredMsec);
            peer.Close();
            //
            port.Dispose();
        }

        [Test]
        public void Read_DataWasThere()
        {
            ISocketPortBuffers port;
            Stream peer;
            Create(out peer, out port);
            Assert.AreEqual(System.Threading.Timeout.Infinite, GetTimeoutValue(peer));
            //Assert.AreEqual(System.Threading.Timeout.Infinite, peer.WriteTimeout);
            SetTimeoutValue(peer, FiftyMsec);
            Assert.AreEqual(FiftyMsec, GetTimeoutValue(peer));
            //Assert.AreEqual(System.Threading.Timeout.Infinite, peer.WriteTimeout);
            //
            _theReleaseOperation(port, new byte[5]);
            //
            Read_FiveTimesOne_NoneTimeout(peer);
            //
            Read_WillTimeout(peer, FiftyMsec);
            //
            peer.Close();
            port.Dispose();
        }


        [Test]
        public void Read_DataArrivesBeforeExpiry()
        {
            ISocketPortBuffers port;
            Stream peer;
            Create(out peer, out port);
            SetTimeoutValue(peer, FiftyMsec);
            //
            IAsyncResult arBR = BeginTimedRead_NoError(peer);
            //
            Thread.Sleep(30);
            _theReleaseOperation(port, new byte[1]);
            Thread.Sleep(20);
            Assert.IsTrue(arBR.IsCompleted, "b");
            int time = EndTimedRead(arBR);
            Between(time, 30 - FudgeShorter, 50, "TR");
            //
            port.Dispose();
        }


        [Test]
        public void Read_BlockedByEarlierAsyncOp()
        {
            ISocketPortBuffers port;
            Stream peer;
            Create(out peer, out port);
            SetTimeoutValue(peer, FiftyMsec);
            //
            // Block the blocking read timeout
            IAsyncResult ar = _theBeginVersionOfTheStreamOperation(peer);
            //
            //int x = peer.ReadByte();
            IAsyncResult arBR = BeginTimedRead_Timesout(peer);
            Thread.Sleep(200);
            Assert.IsFalse(arBR.IsCompleted, "First async read unexpectedly IS completed");
            //
            _theReleaseOperation(port, new byte[1]);
            Thread.Sleep(60);
            Assert.IsTrue(ar.IsCompleted, "First async read not completed");
            Assert.IsTrue(arBR.IsCompleted, "Second Read not completed");
            int time = EndTimedRead(arBR);
            Between(time, 200, 200 + 2 * FiftyMsec, "time of Read");
            //
            port.Dispose();
        }

        //--
        // Both start at exactly(!) the same time so both should timeout
        // at the same time and thus both want to complete themselves...
        // But the first completes all of the ops and so the second can't
        // complete itself!  So originally bang(!!!) with: 
        //    "System.InvalidOperationException : Queue empty."
        [Test]
        public void TwoWaiting_SameTimeout__hopefully()
        {
            ISocketPortBuffers port;
            Stream peer;
            Create(out peer, out port);
            SetTimeoutValue(peer, FiftyMsec);
            //
            IAsyncResult arBrAa = BeginTimedRead_Timesout(peer);
            IAsyncResult arBrBb = BeginTimedRead_APreviousOpTimedOut(peer);
            //
            Thread.Sleep(200);
            Assert.IsTrue(arBrAa.IsCompleted, "aa");
            Assert.IsTrue(arBrBb.IsCompleted, "bb");
            try {
                int timeAa = EndTimedRead(arBrAa);
            } catch (IOException ioex) {
                AssertTimeoutException(ioex, "aa");
            }
            try {
                int timeBb = EndTimedRead(arBrBb);
                // REALLY (for Read) should check the return value is zero (EOF)!!
                //
                // Or do we really want to the code to complete all the
                // operations with TimedOut?....
                // Answer: Probably not see the case below:
                //   MultipleOpsWaiting_SecondNotTimesout
            } catch (IOException ioex) {
                AssertTimeoutException(ioex, "bb");
            }
            //
            port.Dispose();
        }

        [Test]
        public void TwoWaiting_SecondNotTimesout()
        {
            ISocketPortBuffers port;
            Stream peer;
            Create(out peer, out port);
            SetTimeoutValue(peer, FiftyMsec);
            //
            IAsyncResult arBrAa = BeginTimedRead_Timesout(peer);
            // So now the second don't timeout and thus is safely completed
            // by the first and no contention over the completion of it.
            Thread.Sleep(100);
            IAsyncResult arBrBb = BeginTimedRead_APreviousOpTimedOut(peer);
            //
            Thread.Sleep(500);
            Assert.IsTrue(arBrAa.IsCompleted, "aa");
            Assert.IsTrue(arBrBb.IsCompleted, "bb");
            try {
                int timeAa = EndTimedRead(arBrAa);
            } catch (IOException ioex) {
                AssertTimeoutException(ioex, "aa");
            }
            try {
                int timeBb = EndTimedRead(arBrBb);
                // REALLY (for Read) should check the return value is zero (EOF)!!
                //
                // Or do we really want to the code to complete all the
                // operations with TimedOut?....
            } catch (IOException ioex) {
                AssertTimeoutException(ioex, "bb");
            }
            //Between(time0, 30 - FudgeShorter, 50, "TR");
            //
            port.Dispose();
        }

        //===================================
        //delegate TResult Func<T1, T2, TResult>(T1 p1, T2 p2);

        protected IAsyncResult BeginTimedRead_(Stream stream, bool expectedToThrow, int? expectedError)
        {
            Trace.Assert(expectedError.HasValue == expectedToThrow);
            Func<Stream, bool, int?, int> dlgt = TimedRead_Runner;
            Func<int> dlgt0 = delegate { return dlgt(stream, expectedToThrow, expectedError); };
            return Delegate2.BeginInvoke(dlgt0, null, dlgt0);
        }

        protected IAsyncResult BeginTimedRead_Timesout(Stream stream)
        {
            return BeginTimedRead_(stream, true, WSAETIMEDOUT);
        }
        protected IAsyncResult BeginTimedRead_NoError(Stream stream)
        {
            return BeginTimedRead_(stream, false, null);
        }
        // EoF in Read, 10057 in Write??
        protected abstract IAsyncResult BeginTimedRead_APreviousOpTimedOut(Stream stream);

        int EndTimedRead(object ar)
        {
#if false
            IAsyncResult ar2 = (IAsyncResult)ar;
            Func<Stream, bool, int> dlgt = (Func<Stream, bool, int>)ar2.AsyncState;
            int period = dlgt.EndInvoke(ar2);
            return period;
#else
            IAsyncResult ar2 = (IAsyncResult)ar;
            //Func<Stream, bool, int> dlgt = (Func<Stream, bool, int>)ar2.AsyncState;
            //Func<int> dlgt0 = null;
            Func<int> dlgt0 = (Func<int>)ar2.AsyncState;
            int period = Delegate2.EndInvoke(dlgt0, ar2);
            return period;
#endif
        }
        int TimedRead_Runner(Stream stream, bool expectedToThrow, int? expectedError)
        {
            Trace.Assert(expectedError.HasValue == expectedToThrow);
            Stopwatch timer = new Stopwatch();
            try {
                timer.Start();
                int x = _theStreamOperation(stream);
                timer.Stop();
                if (expectedToThrow) {
                    Assert.Fail("should have thrown -- #" + "TimedRead");
                }
            } catch (IOException ioex) {
                timer.Stop();
                if (!expectedToThrow) {
                    Assert.Fail("should NOT have thrown -- #" + "TimedRead");
                }
                SocketException sex = (SocketException)ioex.InnerException;
                //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
                Assert.AreEqual(expectedError, sex.ErrorCode, "ErrorCode -- #" + "TimedRead");
            }
            return checked((int)timer.ElapsedMilliseconds);
        }

        private void AssertTimeoutException(IOException ioex, string name)
        {
            SocketException sex = (SocketException)ioex.InnerException;
            //no NETCF: Assert.AreEqual(SocketError.TimedOut, sex.SocketErrorCode);
            Assert.AreEqual(WSAETIMEDOUT, sex.ErrorCode, "ErrorCode -- #" + "TimedRead -- " + name);
        }


        //--------
        public void Between(Stopwatch timer, int min, int max, string message)
        {
            Between(checked((int)timer.ElapsedMilliseconds), min, max, message);
        }

        public virtual void Between(int value, int min, int max, string message)
        {
            Assert.Greater(value, min - 1, "min -- " + message);
            Assert.Less(value, max + 1, "max -- " + message);
        }

#if NETCF
        public class Stopwatch
        {
            int _start;
            int _elapsed;

            public Stopwatch()
            {
            }

            public void Start()
            {
                _start = Environment.TickCount;
            }

            public void Stop()
            {
                int stop = Environment.TickCount;
                int curElapsed = stop -_start;
                _elapsed += curElapsed;
            }

            public void Reset()
            {
                _elapsed = 0;
                _start = 0; //????
            }

            public long ElapsedMilliseconds { get { return _elapsed; } }
        }
#endif

    }
}
