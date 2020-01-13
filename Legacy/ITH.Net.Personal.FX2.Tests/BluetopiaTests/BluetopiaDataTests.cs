#if BLUETOPIA
using System;
using System.Threading;
using NMock2;
using NUnit.Framework;
using InTheHand.Net.Tests.Infra;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaDataTests
    {

        #region Write
        // int SPP_Data_Write(uint BluetoothStackID, uint SerialPortID, Word DataLength, byte[] DataBuffer);

        [Test]
        public void WriteAllAaaa()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            Expect.Once.On(stuff.MockedApi).Method("SPP_Data_Write")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId(),
                    (ushort)10, new byte[10])
                .Will(Return.Value(10));
            byte[] buf = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA };
            buf = new byte[10];
            stuff.DutConn.Write(buf, 0, buf.Length);
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }

        [Test]
        public void WritePartAaaa()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            byte[] buf = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA };
            Expect.Once.On(stuff.MockedApi).Method("SPP_Data_Write")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId(),
                    (ushort)buf.Length, buf)
                .Will(Return.Value(5));
            var ar = stuff.DutConn.BeginWrite(buf, 0, buf.Length, null, null);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            Assert.IsFalse(ar.IsCompleted, "IsCompleted mid");
            //
            // Spp_Write is blocked
            Thread.Sleep(100);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            byte[] buf2 = { 6, 7, 8, 9, 0xA };
            ClientTestingBluetopia.ExpectWrite(stuff, buf2, buf2.Length);
            using (var ctor = new SppEventCreator()) {
                var eventData = ctor.CreateWriteEmpty(stuff.DutConn.Testing_GetPortId());
                ClientTestingBluetopia.RaiseSppEvent(stuff, eventData);
            }
            Thread.Sleep(100);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted end");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }

        [Test]
        public void WritePartBbbb()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            byte[] buf1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA };
            ClientTestingBluetopia.ExpectWrite(stuff, buf1, 5);
            var ar1 = stuff.DutConn.BeginWrite(buf1, 0, buf1.Length, null, null);
            byte[] buf2 = new byte[20];
            var ar2 = stuff.DutConn.BeginWrite(buf2, 0, buf2.Length, null, null);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            Assert.IsFalse(ar1.IsCompleted, "IsCompleted 1 mid");
            Assert.IsFalse(ar2.IsCompleted, "IsCompleted 2 mid");
            //
            // Spp_Write is blocked
            Thread.Sleep(20);
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            //
            byte[] buf1Part2 = { 6, 7, 8, 9, 0xA };
            ClientTestingBluetopia.ExpectWrite(stuff, buf1Part2, buf1Part2.Length);
            ClientTestingBluetopia.ExpectWrite(stuff, buf2, buf2.Length);
            //Expect.Once.On(stuff.MockedApi).Method("SPP_Data_Write")
            //    .With(stuff.StackId, stuff.DutConn.Testing_GetPortId(),
            //        (ushort)buf2.Length, buf2)
            //    .Will(Return.Value(buf2.Length));
            using (var ctor = new SppEventCreator()) {
                var eventData = ctor.CreateWriteEmpty(stuff.DutConn.Testing_GetPortId());
                ClientTestingBluetopia.RaiseSppEvent(stuff, eventData);
            }
            Thread.Sleep(100);
            Assert.IsTrue(ar1.IsCompleted, "IsCompleted 1 end");
            Assert.IsTrue(ar2.IsCompleted, "IsCompleted 2 end");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }
        #endregion

        #region Receive
        // int SPP_Data_Read(uint BluetoothStackID, uint SerialPortID, Word DataBufferSize, byte[] DataBuffer);

        [Test]
        public void ReadAfter()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            byte[] srcBuf = SendReceiveTen(stuff);
            //
            byte[] buf = new byte[100];
            int readLen = stuff.DutConn.Read(buf, 0, buf.Length);
            Assert.AreEqual(10, readLen, "readLen");
            Assert2.AreEqual_Buffers(srcBuf, buf, 0, readLen, "buf content");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }

        [Test]
        public void ReadBeginBefore()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            byte[] buf = new byte[100];
            var ar = stuff.DutConn.BeginRead(buf, 0, buf.Length, null, null);
            Assert.IsFalse(ar.IsCompleted);
            //
            byte[] srcBuf = SendReceiveTen(stuff);
            //
            Assert.IsTrue(ar.IsCompleted);
            int readLen = stuff.DutConn.EndRead(ar);
            Assert.AreEqual(10, readLen, "readLen");
            Assert2.AreEqual_Buffers(srcBuf, buf, 0, readLen, "buf content");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }

        [Test]
        public void ReadBeginBeforeTwoParts()
        {
            var stuff = ClientTestingBluetopia.Open();
            Assert.IsNotNull(stuff.DutClient, "DutClient");
            Assert.IsNotNull(stuff.DutConn, "DutConn");
            //
            byte[] buf = new byte[100];
            byte[] buf2 = new byte[100];
            var ar = stuff.DutConn.BeginRead(buf, 0, 5, null, null);
            var ar2 = stuff.DutConn.BeginRead(buf2, 0, buf2.Length, null, null);
            Assert.IsFalse(ar.IsCompleted);
            Assert.IsFalse(ar2.IsCompleted);
            //
            byte[] srcBuf = SendReceiveTen(stuff);
            //
            byte[] srcBufPart1 = new byte[5];
            Array.Copy(srcBuf, 0, srcBufPart1, 0, srcBufPart1.Length);
            byte[] srcBufPart2 = new byte[5];
            Array.Copy(srcBuf, srcBufPart1.Length, srcBufPart2, 0, srcBufPart2.Length);
            //
            Assert.IsTrue(ar.IsCompleted);
            Assert.IsTrue(ar2.IsCompleted);
            int readLen = stuff.DutConn.EndRead(ar);
            Assert.AreEqual(5, readLen, "readLen");
            Assert2.AreEqual_Buffers(srcBufPart1, buf, 0, readLen, "buf content Part1");
            int readLen2 = stuff.DutConn.EndRead(ar);
            Assert.AreEqual(5, readLen, "readLen");
            Assert2.AreEqual_Buffers(srcBufPart2, buf2, 0, readLen2, "buf content Part2");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ClientTestingBluetopia.Close(stuff);
        }

        private static byte[] SendReceiveTen(StuffClientBluetopia stuff)
        {
            byte[] srcBuf = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA };
            bool isMustBeSameLength = false;
            int sizeOfReaderBuffer = 10;
            Expect.Once.On(stuff.MockedApi).Method("SPP_Data_Read")
                .With(stuff.StackId, stuff.DutConn.Testing_GetPortId(),
                    (ushort)sizeOfReaderBuffer, new byte[sizeOfReaderBuffer])
                .Will(Return.Value(srcBuf.Length),
                    FillArrayIndexedParameterAction.Fill(3, srcBuf, isMustBeSameLength));
            using (var ctor = new SppEventCreator()) {
                var eventData = ctor.CreateDataIndication(stuff.DutConn.Testing_GetPortId(), srcBuf.Length);
                ClientTestingBluetopia.RaiseSppEvent(stuff, eventData);
            }
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            return srcBuf;
        }
        #endregion

    }
}
#endif
