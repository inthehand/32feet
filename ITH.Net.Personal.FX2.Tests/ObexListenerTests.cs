using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using InTheHand.Net.Tests.Infra;
using InTheHand.Net.Tests.ObexRequest;
using System.IO;
using InTheHand.Net.Tests.Sdp2;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace InTheHand.Net.Tests.ObexListenerTests
{
    [TestFixture]
    public class ObexListenerTests_ExceptionOnEos_AvailablePropertyThrows : ObexListenerTests_AvailablePropertyThrows
    {
        internal override SocketAdapter CreateSocketAdapter(Stream peer)
        {
            Stream p2 = new ExceptionAtReadEosStream(peer);
            return base.CreateSocketAdapter(p2);
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public override void SimplePut3_ConnectionTruncatedBeforeFinal_OutsidePacket()
        {
            base.SimplePut3_ConnectionTruncatedBeforeFinal_OutsidePacket();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public override void SimplePut3_ConnectionTruncated_InPacketInHeader()
        {
            base.SimplePut3_ConnectionTruncated_InPacketInHeader();
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public override void SimplePut3_ConnectionTruncated_InPacketAfterHeader()
        {
            base.SimplePut3_ConnectionTruncated_InPacketAfterHeader();
        }
    }


    [TestFixture]
    public class ObexListenerTests_ByteByByteRead_AvailablePropertyThrows : ObexListenerTests_AvailablePropertyThrows
    {
        protected override Stream ModifyReadStream(MemoryStream strm)
        {
            return new ByteByByteReadStream(strm);
        }
    }

    [TestFixture]
    public class ObexListenerTests_ByteByByteRead_AvailableReturns1Arrrgggh : ObexListenerTests_AvailableReturns1Arrrgggh
    {
        protected override Stream ModifyReadStream(MemoryStream strm)
        {
            return new ByteByByteReadStream(strm);
        }
    }


    [TestFixture]
    public class ObexListenerTests_AvailablePropertyThrows : ObexListenerTests
    {
        internal override SocketAdapter CreateSocketAdapter(Stream peer)
        {
            SocketAdapter s = new SocketStreamAdapterAvailablePropertyThrows(peer);
            return s;
        }
    }

    [TestFixture]
    public class ObexListenerTests_AvailableReturns1Arrrgggh : ObexListenerTests
    {
        internal override SocketAdapter CreateSocketAdapter(Stream peer)
        {
            SocketAdapter s = new SocketStreamAdapterAvailableOneByte(peer);
            return s;
        }
    }


    [TestFixture]
    public class ObexListenerMiscTests
    {
        [Test]
        [Category("Need Bluetooth device/stack")]
        public void StartStopStart()
        {
            ObexListener lsnr;
            lsnr = new ObexListener(ObexTransport.Tcp);
            DoTestStartStopStart(lsnr);
            lsnr = new ObexListener(ObexTransport.Bluetooth);
            DoTestStartStopStart(lsnr);
            // With Window's Irmon service running we can't run another IrDA OBEX service.
            //lsnr = new ObexListener(ObexTransport.IrDA);
            //DoTestStartStopStart(lsnr);
        }

        private void DoTestStartStopStart(ObexListener lsnr)
        {
            Action<ObexListener> dlgt = Foo;
            lsnr.Start();
            IAsyncResult ar0 = dlgt.BeginInvoke(lsnr, null, null);
            Assert.IsFalse(ar0.IsCompleted);
            lsnr.Stop();
            System.Threading.Thread.Sleep(50);
            Assert.IsTrue(ar0.IsCompleted);
            lsnr.Start();
            try {
                dlgt.EndInvoke(ar0);
            } catch (InvalidOperationException) {
            }
            lsnr.Stop();
        }

        void Foo(ObexListener lsnr)
        {
            ObexListenerContext ctx = lsnr.GetContext();
            if (ctx != null) {
                ObexListenerRequest req = ctx.Request;
            }
        }
    }


    public abstract class ObexListenerTests
    {
        [Test]
        public void SimplePut1()
        {
            byte[] requests = NewWholeProcessTests_Data.SimplePut1_ExpectedRequests;
            byte[] expectedResponses = SimplePut1_Responses;
            //
            ObexListenerContext ctx = DoTest(requests, expectedResponses);
            //
            AssertHeader(ctx, "NAME", "aaaa.txt");
            AssertHeader(ctx, "LENGTH", "4");
            Assert.AreEqual(2, ctx.Request.Headers.Count, "Headers.Count");
            //Console.WriteLine(String.Join(",", ctx.Request.Headers.AllKeys));
            //
            Stream content = new MemoryStream();
            Stream_CopyTo(ctx.Request.InputStream, content);
            Assert.AreEqual(4, content.Length, "content.Length");
        }

        private void AssertHeader(ObexListenerContext ctx, string headerName, string expectedValue)
        {
            Assert.AreEqual(expectedValue, ctx.Request.Headers[headerName], "Headers['" + headerName + "']");
        }

        [Test]
        public void TwoPuts_SecondIsDisallowed()
        {
            byte[] requests = NewWholeProcessTests_Data.TwoPuts_ExpectedRequests;
            byte[] expectedResponses = TwoPuts_DisallowFirstAndCloses_Responses;
            //
            ObexListenerContext ctx = DoTest(requests, expectedResponses);
            AssertHeader(ctx, "NAME", "aaaa.txt");
            AssertHeader(ctx, "LENGTH", "4");
            //
            Stream content = new MemoryStream();
            Stream_CopyTo(ctx.Request.InputStream, content);
            Assert.AreEqual(4, content.Length, "content.Length");
        }

        private void Stream_CopyTo(Stream stream, Stream content)
        {
            byte[] buf = new byte[1024];
            int readLen;
            while ((readLen = stream.Read(buf, 0, buf.Length)) > 0) {
                content.Write(buf, 0, readLen);
            }
            content.Position = 0;
        }

        //--------
        private ObexListenerContext DoTest(byte[] requests, byte[] expectedResponses)
        {
            Exception ex;
            return DoTest(requests, expectedResponses, false, out ex);
        }

        private ObexListenerContext DoTest(byte[] requests, byte[] expectedResponses,
            bool expectError, out Exception resultException)
        {
            MemoryStream peerRequests_ = new MemoryStream(requests, false);
            Stream peerRequests = ModifyReadStream(peerRequests_);
            MemoryStream ourResponses = new MemoryStream();
            TwoWayStream peer = new TwoWayStream(peerRequests, ourResponses);
            //
            SocketAdapter s = CreateSocketAdapter(peer);
            ObexListenerContext ctx;
            try {
                ctx = new ObexListenerContext(s);
                resultException = null;
            } catch (Exception ex) {
                // If test error expected we DO want to check that the
                // response packets are correct.  Otherwise just fail.
                if (expectError) {
                    resultException = ex;
                    ctx = null;
                } else {
                    // Unexpected!
                    throw;
                }
            }
            //
            Assert.AreEqual(expectedResponses, ourResponses.ToArray(), "ourResponses");
            if (resultException != null) { Debug.Assert(ctx == null); }
            return ctx;
        }

        internal abstract SocketAdapter CreateSocketAdapter(Stream peer);

        protected virtual Stream ModifyReadStream(MemoryStream strm)
        {
            return strm;
        }

        //--------
        [Test]
        public void WidcommMruxxB8()
        {
            byte[] requests = SimplePut1_ExpectedRequests_WidcommMru0FB8;
            byte[] expectedResponses = SimplePut1_Responses;
            //
            MemoryStream peerRequests_ = new MemoryStream(requests, false);
            Stream peerRequests = ModifyReadStream(peerRequests_);
            MemoryStream ourResponses = new MemoryStream();
            TwoWayStream peer = new TwoWayStream(peerRequests, ourResponses);
            //
            SocketAdapter s = CreateSocketAdapter(peer);
            ObexListenerContext ctx = new ObexListenerContext(s);
            //
            Assert.AreEqual(expectedResponses, ourResponses.ToArray(), "ourResponses");
        }

        public static readonly byte[] SimplePut1_Responses ={ // !! Copied from NewWholeProcessTests_Data
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, //changed to suit ObexListener
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };

        public static readonly byte[] SimplePut1_ExpectedRequests_WidcommMru0FB8 ={
            //Connect
            0x80, 0x00, 0x07, 0x10, 0x00, 0x0f, 0xb8,
            //Put
            0x02, 0x00, 0x1D, 0x01, 0x00, 0x15, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x61, 0x00, 0x2E,
                0x00, 0x74, 0x00, 0x78, 0x00, 0x74, 0x00, 0x00, 0xC3, 0x00, 0x00, 0x00, 0x04, 
            //Put
            0x82, 0x00, 0x0A, 0x49, 0x00, 0x07, 0x61, 0x62, 0x63, 0x64,
            //Disconnect
            0x81, 0x00, 0x03
        };

        const byte StatusForbidden = 0xC3;
        public static readonly byte[] TwoPuts_AllowsButShouldNot_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] TwoPuts_DisallowsEach_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            // Subsequent response(s)
            StatusForbidden, 0,3,
            StatusForbidden, 0,3,
            //
            0xA0, 0,3,
        };
        public static readonly byte[] TwoPuts_DisallowFirstAndCloses_Responses ={
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, // MTU=0x800=2048
            // Subsequent response(s)
            0x90, 0,3,
            0xA0, 0,3,
            // Subsequent response(s)
            StatusForbidden, 0,3,
        };

        //--
        public static readonly byte[] SimpleGet1_Responses_NotImplemented ={ // !! Copied from NewWholeProcessTests_Data
            // Connect response
            0xA0, 0,7, 0x10, 0x00, 0x20, 0x00, //changed to suit ObexListener
            // Subsequent response(s)
          0x7F & /*hack*/ 0xD1, 0,3, // NotImplemented = 0x51
          0x7F & /*hack*/ 0xD1, 0,3,
            //
            0xA0, 0,3,
        };

        //--
        [Test]
        public void SimplePut1_WithContentLength()
        {
            const int expectedContentLengthHeaderValue = 4;
            DoTest_Put_ContentLength(NewWholeProcessTests_Data.SimplePut1_ExpectedRequests,
                expectedContentLengthHeaderValue);
        }

        private void DoTest_Put_ContentLength(byte[] requests, int? expectedContentLengthHeaderValue)
        {
            DoTest_Put_ContentLength(requests, SimplePut1_Responses,
                expectedContentLengthHeaderValue);
        }

        private void DoTest_Put_ContentLength(byte[] requests, byte[] expectedResponses,
            int? expectedContentLengthHeaderValue)
        {
            ObexListenerContext ctx = DoTest(requests, expectedResponses);
            ObexListenerRequest req = ctx.Request;
            //
            if (expectedContentLengthHeaderValue.HasValue)
                Assert.AreEqual(expectedContentLengthHeaderValue.Value, req.ContentLength64, "ContentLength64");
            //
            AreEqualBuffers(NewWholeProcessTests_Data.SimplePut1_Data, (MemoryStream)req.InputStream);
            //
            MemoryStream data2 = new MemoryStream();
            System.Threading.ThreadStart dlgt = delegate {
                req.WriteFile(data2);
            };
            IAsyncResult ar = dlgt.BeginInvoke(null, null);
            bool signalled = ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
            if (!signalled) {
                // Need to kill the WriteFile thread or there'll be big CPU usage!
                data2.Close(); // Cause WriteFile to fail!
                bool signalledAfter = ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                Assert.IsTrue(signalledAfter, "WriteFile aborted but still hung");
                try {
                    dlgt.EndInvoke(ar);
                } catch (ObjectDisposedException) {
                }
            } else {
                Assert.IsTrue(signalled, "WriteFile hung");
                dlgt.EndInvoke(ar);
            }
            AreEqualBuffers(NewWholeProcessTests_Data.SimplePut1_Data, data2);
            Assert.IsTrue(signalled, "WriteFile hung [end]");
        }

        [Test]
        public void SimplePut1_WithTooLongContentLength()
        {
            const int expectedContentLengthHeaderValue = 0x9004;
            DoTest_Put_ContentLength(NewWholeProcessTests_Data.SimplePut1_TooLongContentLength_ExpectedRequests,
                expectedContentLengthHeaderValue);
        }

        [Test]
        public void SimplePut1_WithTooShortContentLength()
        {
            const int expectedContentLengthHeaderValue = 1;
            DoTest_Put_ContentLength(NewWholeProcessTests_Data.SimplePut1_TooShortContentLength_ExpectedRequests,
                expectedContentLengthHeaderValue);
        }

        [Test]
        public void SimplePut1_WithNoContentLength()
        {
            const int expectedContentLengthHeaderValue = -1;
            DoTest_Put_ContentLength(NewWholeProcessTests_Data.SimplePut1_NoContentLength_ExpectedRequests,
                expectedContentLengthHeaderValue);
        }

        void AreEqualBuffers(byte[] expected, MemoryStream ms)
        {
            byte[] data = ms.ToArray();
            Assert.AreEqual(NewWholeProcessTests_Data.SimplePut1_Data, data, "data via InputStream");
        }

        //------------
        [Test]
        public void SimplePut3_EmptyEob()
        {
            DoTest(NewWholeProcessTests_Data.SimplePut3EmptyEob_ExpectedRequests,
                NewWholeProcessTests_Data.SimplePut3_Responses);
        }

        [Test]
        [ExpectedException(typeof(System.Net.ProtocolViolationException))]
        public virtual void SimplePut3_ConnectionTruncatedBeforeFinal_OutsidePacket()
        {
            DoTest(NewWholeProcessTests_Data.SimplePut3_ConnectionTruncatedBeforeFinal_ExpectedRequests,
                NewWholeProcessTests_Data.SimplePut3_Truncated_Responses);
            Console.WriteLine("SimplePut3_ConnectionTruncatedBeforeFinal error SHOULD"
                + " be reported somehow.  The connection closed before completion!!!");
        }

        [Test]
        [ExpectedException(typeof(EndOfStreamException))]
        public virtual void SimplePut3_ConnectionTruncated_InPacketInHeader()
        {
            DoTest(NewWholeProcessTests_Data.SimplePut3_ConnectionTruncatedBeforeFinal_TruncatedInPacketInHeader_ExpectedRequests,
                NewWholeProcessTests_Data.SimplePut3_Truncated_Responses);
            Console.WriteLine("SimplePut3_ConnectionTruncatedBeforeFinal error SHOULD"
                + "be reported somehow.  The connection closed before completion!!!");
        }

        [Test]
        [ExpectedException(typeof(EndOfStreamException))]
        public virtual void SimplePut3_ConnectionTruncated_InPacketAfterHeader()
        {
            DoTest(NewWholeProcessTests_Data.SimplePut3_ConnectionTruncatedBeforeFinal_TruncatedInPacketAfterHeader_ExpectedRequests,
                NewWholeProcessTests_Data.SimplePut3_Truncated_Responses);
            Console.WriteLine("SimplePut3_ConnectionTruncatedBeforeFinal error SHOULD"
                + "be reported somehow.  The connection closed before completion!!!");
        }

        [Test]
        public void GetNotSupport()
        {
            byte[] requests = NewWholeProcessTests_Data.SimpleGet1_ExpectedRequests;
            byte[] expectedResponses = SimpleGet1_Responses_NotImplemented;
            Exception ex;
            ObexListenerContext ctx = DoTest(requests, expectedResponses, true, out ex);
            //
            // We check above that we get error codes from the server.
            // We also want the user to know that the client didn't do a PUT.
            Assert.IsAssignableFrom(typeof(ProtocolViolationException), ex);
            Assert.AreEqual("No PutFinal received.", ex.Message);
        }

        [Test]
        public void UnknownVerbNotSupport()
        {
            byte[] requests = NewWholeProcessTests_Data.SimpleUnknownVerb_ExpectedRequests;
            byte[] expectedResponses = SimpleGet1_Responses_NotImplemented;
            Exception ex;
            ObexListenerContext ctx = DoTest(requests, expectedResponses, true, out ex);
            //
            // We check above that we get error codes from the server.
            // We also want the user to know that the client didn't do a PUT.
            Assert.IsAssignableFrom(typeof(ProtocolViolationException), ex);
            Assert.AreEqual("No PutFinal received.", ex.Message);
        }

    }
}
