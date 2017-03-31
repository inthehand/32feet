using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using System.IO;
using InTheHand.Net.Bluetooth.Widcomm;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace InTheHand.Net.Tests.Widcomm
{

    partial class WidcommBluetoothClientCommsTest
    {
        const PORT_EV EventThatFreesPendingWrite = PORT_EV.TXEMPTY;

        class TestRfcommPort_WritePartials_AcceptHalf : TestRfcommPort
        {
            const ushort Limit = 20;

            public override InTheHand.Net.Bluetooth.Widcomm.PORT_RETURN_CODE Write(byte[] p_data, ushort len_to_write, out ushort p_len_written)
            {
                ushort len_to_write2 = LimitValue(len_to_write);
                return base.Write(p_data, len_to_write2, out p_len_written);
            }

            internal static ushort LimitValue(ushort len_to_write)
            {
                ushort len_to_write2;
                if (len_to_write > Limit) {
                    len_to_write2 = Math.Max(checked((ushort)(len_to_write / 2)),
                        Math.Min(Limit, len_to_write));
                } else {
                    len_to_write2 = len_to_write;
                }
                return len_to_write2;
            }
            internal static ushort LimitValue(int len_to_write)
            {
                ushort us = checked((ushort)Math.Min(len_to_write, ushort.MaxValue));
                return LimitValue(us);
            }
        }//class2

        class TestRfcommStream_WriteEventAfterEachBeginWrite : WidcommRfcommStreamBase
        {
            TestRfcommPort m_port;

            internal TestRfcommStream_WriteEventAfterEachBeginWrite(TestRfcommPort port, IRfCommIf iface, WidcommBluetoothFactoryBase fcty)
                : base(port, iface, fcty)
            {
                m_port = port;
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                IAsyncResult ar = base.BeginWrite(buffer, offset, count, callback, state);
                m_port.NewEvent(EventThatFreesPendingWrite);
                m_port.NewEvent(EventThatFreesPendingWrite);
                m_port.NewEvent(EventThatFreesPendingWrite);
                m_port.NewEvent(EventThatFreesPendingWrite);
                m_port.NewEvent(EventThatFreesPendingWrite);
                return ar;
            }
        }


        private void Create_ConnectedBluetoothClient_WritePartialsAcceptHalf(
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient_WritePartialsAcceptHalfA(out port, out cli, out strm2);
            ConnectBluetoothClient(port, cli);
        }

        private static void Create_BluetoothClient_WritePartialsAcceptHalfA(
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient_WritePartialsAcceptHalfB(null, out port, out cli, out strm2);
        }

        private static void Create_BluetoothClient_WritePartialsAcceptHalfB(WidcommBtInterface btIface,
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            WidcommFactoryGivenInstances fcty = new WidcommFactoryGivenInstances();
            port = new TestRfcommPort_WritePartials_AcceptHalf();
            TestRfCommIf rfCommIf = new TestRfCommIf();
            WidcommRfcommStreamBase strm = new WidcommRfcommStream(port, rfCommIf, fcty);
            fcty.AddRfcommStream(strm);
            fcty.SetBtInterface(btIface);
            WidcommBluetoothClient wcli = new WidcommBluetoothClient(fcty);
            cli = new BluetoothClient(wcli);
            strm2 = strm;
        }

        //--
        private void Create_ConnectedBluetoothClient_WritePartialsAcceptHalf_WriteEventAfterEachWrite(
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient_WritePartialsAcceptHalfA_WriteEventAfterEachWrite(out port, out cli, out strm2);
            ConnectBluetoothClient(port, cli);
        }

        private static void Create_BluetoothClient_WritePartialsAcceptHalfA_WriteEventAfterEachWrite(
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient_WritePartialsAcceptHalfB_WriteEventAfterEachWrite(null, out port, out cli, out strm2);
        }

        private static void Create_BluetoothClient_WritePartialsAcceptHalfB_WriteEventAfterEachWrite(WidcommBtInterface btIface,
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            WidcommFactoryGivenInstances fcty = new WidcommFactoryGivenInstances();
            port = new TestRfcommPort_WritePartials_AcceptHalf();
            TestRfCommIf rfCommIf = new TestRfCommIf();
            WidcommRfcommStreamBase strm = new TestRfcommStream_WriteEventAfterEachBeginWrite(port, rfCommIf, fcty);
            fcty.AddRfcommStream(strm);
            fcty.SetBtInterface(btIface);
            WidcommBluetoothClient wcli = new WidcommBluetoothClient(fcty);
            cli = new BluetoothClient(wcli);
            strm2 = strm;
        }

        private static void ConnectBluetoothClient(TestRfcommPort port, BluetoothClient cli)
        {
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumber = 5;
            //
            IAsyncResult ar;
            //
            // Success
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 1");
            Assert.IsTrue(ar.IsCompleted, "Connect 1 completed");
            cli.EndConnect(ar);
        }

        //--------------------------------------------------------------
        byte[] data30_, data30Offset5_;

        [TestFixtureSetUp]
        public void FixtureSetup_WidcommBluetoothWritePartials()
        {
            data30_ = new byte[30];
            new Random().NextBytes(data30_);
        }

        byte[] Data30 { get { return (byte[])data30_.Clone(); } }

        const int Data30_offset5_Offset = 5;
        byte[] Data30_offset5
        {
            get
            {
                if (data30Offset5_ == null) {
                    byte[] tmp = new byte[data30_.Length + Data30_offset5_Offset];
                    Array.Copy(data30_, 0, tmp, Data30_offset5_Offset, data30_.Length);
                    data30Offset5_ = tmp;
                }
                return (byte[])data30Offset5_.Clone();
            }
        }

        [Test]
        public void WritePartials_BeginWrite()
        {
            // xxQueue the un-accepted data, and send one block per TXEMPTY event, we 
            // xxshould really try sending until 
            //
            TestRfcommPort port = new TestRfcommPort_WritePartials_AcceptHalf();
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient_WritePartialsAcceptHalf(out port, out cli, out strm);
            //
            // 'Long', zero offset.
            IAsyncResult ar = strm.BeginWrite(Data30, 0, Data30.Length, null, null);
            Assert.IsFalse(ar.IsCompleted, "IsComplete before empty event(s)");
            port.AssertWrittenContent("20 of 30", First(Data30, 20));
            Assert.IsFalse(ar.IsCompleted, "IsComplete before empty event(s)");
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar.IsCompleted, "IsComplete after empty event(s)");
            byte[][] chunks = SplitToLimits(Data30);
            port.AssertWrittenContent("all of 30", chunks);
            Assert.IsFalse(ar.CompletedSynchronously, "CompletedSynchronously");
            strm.EndWrite(ar);
            port.ClearWrittenContent();
            ar = null;
            //
            // 'Long', zero offset. Twice! Whilst first un-transmitted
            IAsyncResult ar1 = strm.BeginWrite(Data30, 0, Data30.Length, null, null); //**
            Assert.IsFalse(ar1.IsCompleted, "Twice--IsComplete before #1");
            port.AssertWrittenContent("20 of 30", First(Data30, 20));
            Assert.IsFalse(ar1.IsCompleted, "Twice--IsComplete before #2");
            IAsyncResult ar2 = strm.BeginWrite(Data30, 0, Data30.Length, null, null); //**
            Assert.IsFalse(ar2.IsCompleted, "Twice--IsComplete before #3");
            port.AssertWrittenContent("20 of 30", First(Data30, 20)); // No change
            Assert.IsFalse(ar1.IsCompleted, "Twice--IsComplete before #4");
            Assert.IsFalse(ar2.IsCompleted, "Twice--IsComplete before #5");
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar1.IsCompleted, "IsComplete after empty event(s)");
            chunks = SplitToLimits(Data30, Data30);
            port.AssertWrittenContent("all of 30", chunks);
            Assert.IsFalse(ar1.CompletedSynchronously, "CompletedSynchronously ar1");
            Assert.IsFalse(ar2.CompletedSynchronously, "CompletedSynchronously ar2");
            strm.EndWrite(ar1);
            strm.EndWrite(ar2);
            port.ClearWrittenContent();
            ar1 = ar2 = null;
            //==========================================================
            // 'Long', non-zero offset.
            ar = strm.BeginWrite(Data30_offset5, Data30_offset5_Offset, Data30.Length, null, null);
            Assert.IsFalse(ar.IsCompleted, "long/offset--IsComplete before #1");
            port.AssertWrittenContent("30_offset5", SplitToLimits(Data30)[0]);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar.IsCompleted, "30/offset--IsComplete after empty event(s)");
            port.AssertWrittenContent("30/offset", SplitToLimits(Data30));
            Assert.IsFalse(ar.CompletedSynchronously, "CompletedSynchronously ar");
            strm.EndWrite(ar);
            port.ClearWrittenContent();
            ar = null;
            //
            // 'Long', large non-zero offset.
            int newOffset = UInt16.MaxValue + 20;
            byte[] data30_OffsetOver16k = ShiftToOffset(Data30, newOffset);
            ar = strm.BeginWrite(data30_OffsetOver16k, newOffset, Data30.Length, null, null);
            Assert.IsFalse(ar.IsCompleted, "long/offset--IsComplete before #1");
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar.IsCompleted, "long/offset--IsComplete after empty event(s)");
            port.AssertWrittenContent("long/offset", SplitToLimits(Data30));
            Assert.IsFalse(ar.CompletedSynchronously, "CompletedSynchronously ar");
            strm.EndWrite(ar);
            port.ClearWrittenContent();
            ar = null;
            //
            // Large (zero offset).
            byte[] dataOver16K = CreateData(UInt16.MaxValue + 100);
            byte[] dataOver16K_writer = (byte[])dataOver16K.Clone();
            ar = strm.BeginWrite(dataOver16K_writer, 0, dataOver16K_writer.Length, null, null);
            Assert.IsFalse(ar.IsCompleted, "dataOver16K--IsComplete before #1");
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar.IsCompleted, "dataOver16K--IsComplete after empty event(s)");
            strm.EndWrite(ar);
            // check data written
            List<byte[]> chunksList = new List<byte[]>();
            chunksList.AddRange(SplitToLimits(dataOver16K));
            chunks = chunksList.ToArray();
            port.AssertWrittenContentAndClear("Over16k", chunks);
        }

        private void Assert_AreEqual_ArrayArray<T>(T[][] expected, T[][] actual, string description)
        {
            bool sameNumber = (expected.Length == actual.Length);
            for (int i = 0; i < Math.Min(expected.Length, actual.Length); ++i) {
                Assert.AreEqual(expected[i], actual[i], description + " --at " + i);
            } //for
            Assert.AreEqual(expected.Length, actual.Length, description + " --number of blocks");

        }

        private byte[][] SplitToLimits(params byte[][] dataArrayArray)
        {
            List<byte[]> list = new List<byte[]>();
            foreach (byte[] data in dataArrayArray) {
                int pos = 0;
                while (pos < data.Length) {
                    ushort curLen_ = TestRfcommPort_WritePartials_AcceptHalf.LimitValue(data.Length - pos);
                    byte[] chunk = new byte[curLen_];
                    Array.Copy(data, pos, chunk, 0, chunk.Length);
                    list.Add(chunk);
                    pos += chunk.Length;
                }
            }
            return list.ToArray();
        }

        [Test]
        public void WritePartials_Write()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient_WritePartialsAcceptHalf_WriteEventAfterEachWrite(out port, out cli, out strm);
            //
            // 'Long', zero offset.
            strm.Write(Data30, 0, Data30.Length);
            port.AssertWrittenContentAndClear("30", SplitToLimits(Data30));
            port.ClearWrittenContent();
            //
            /*            // 'Long', non-zero offset.
                        strm.Write(Data50_offset5, Data50_offset5_Offset, dataA.Length);
                        port.AssertWrittenContentAndClear("50_offset5", dataA);
                        port.ClearWrittenContent();     */
            // TODO 'Long', large non-zero offset.
            //int newOffset = UInt16.MaxValue + 20;
            //byte[] data30_OffsetOver16k = ShiftToOffset(dataA, newOffset);
            //strm.Write(data30_OffsetOver16k, newOffset, dataA.Length);
            //port.AssertWrittenContentAndClear("30_offsetOver16k", dataA);
            //// Large (zero offset).
            //byte[] dataOver16K = CreateData(UInt16.MaxValue + 100);
            //strm.Write(dataOver16K, 0, dataOver16K.Length);
            //byte[] b1 = new byte[UInt16.MaxValue];
            //byte[] b2 = new byte[dataOver16K.Length - UInt16.MaxValue];
            //Array.Copy(dataOver16K, 0, b1, 0, UInt16.MaxValue);
            //Array.Copy(dataOver16K, UInt16.MaxValue, b2, 0, dataOver16K.Length - UInt16.MaxValue);
            //port.AssertWrittenContentAndClear("Over16k", b1, b2);
            //
            byte[][] chunks;
            //
            // Large (zero offset).
            byte[] dataOver16K = CreateData(UInt16.MaxValue + 100);
            byte[] dataOver16K_writer = (byte[])dataOver16K.Clone();
            ThreadStart action = delegate {
                strm.Write(dataOver16K_writer, 0, dataOver16K_writer.Length);
            };
            IAsyncResult ar = Delegate2.BeginInvoke(action, null, null);
            Assert.IsFalse(ar.IsCompleted, "dataOver16K--IsComplete before #1");
            Thread.Sleep(200); // do we have to wrry that the Write thread not been called yet
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite); // TODO remove duplicates
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            port.NewEvent(EventThatFreesPendingWrite);
            Assert.IsTrue(ar.IsCompleted, "dataOver16K--IsComplete after empty event(s)");
            Delegate2.EndInvoke(action, ar);
            // check data written
            // TO CHECK assert checks data correctly:: dataOver16K[5] = 0;
            List<byte[]> chunksList = new List<byte[]>();
            chunksList.AddRange(SplitToLimits(dataOver16K));
            chunks = chunksList.ToArray();
            port.AssertWrittenContentAndClear("Over16k", chunks);
        }

        //--------
        [Test]
        public void WritesQueuedAtClose_Hard_Local()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtClose_Hard_Init(out port, out strm, out ar1, out ar2);
            strm.Close();
            WritesQueuedAtClose_Hard_Finish(port, strm, ar1, ar2);
        }

        [Test]
        public void WritesQueuedAtClose_Hard_FromPeer()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtClose_Hard_Init(out port, out strm, out ar1, out ar2);
            port.NewEvent(PORT_EV.CONNECT_ERR);
            WritesQueuedAtClose_Hard_Finish(port, strm, ar1, ar2);
        }

        private void WritesQueuedAtClose_Hard_Init(out TestRfcommPort port, out Stream strm, out IAsyncResult ar1, out IAsyncResult ar2)
        {
            BluetoothClient cli;
            Create_ConnectedBluetoothClient_WritePartialsAcceptHalf(out port, out cli, out strm);
            // Set Linger mode "Hard".
            cli.LingerState = new LingerOption(true, 0);
            //
            ar1 = strm.BeginWrite(Data30, 0, Data30.Length, null, null);
            ar2 = strm.BeginWrite(Data30, 0, Data30.Length, null, null);
            Assert.IsFalse(ar1.IsCompleted, "WritesQueuedAtClose--IsComplete ar1");
            port.AssertWrittenContent("WritesQueuedAtClose--written--before", First(Data30, 20));
            Assert.IsFalse(ar2.IsCompleted, "WritesQueuedAtClose--IsComplete ar2");
        }

        private void WritesQueuedAtClose_Hard_Finish(TestRfcommPort port, Stream strm, IAsyncResult ar1, IAsyncResult ar2)
        {
            Assert.IsTrue(ar1.IsCompleted, "WritesQueuedAtClose--IsComplete ar1 After close");
            Assert.IsTrue(ar2.IsCompleted, "WritesQueuedAtClose--IsComplete ar2 After close");
            port.AssertWrittenContent("WritesQueuedAtClose--written--after close", First(Data30, 20));
            try {
                strm.EndWrite(ar1);
                Assert.Fail("should have thrown -- ar1");
            } catch (IOException ioex) {
                Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
                SocketException ex = (SocketException)ioex.InnerException;
                Assert.AreEqual(SocketError_NotConnected, ex.ErrorCode);
            }
            try {
                strm.EndWrite(ar2);
                Assert.Fail("should have thrown -- ar2");
            } catch (IOException ioex) {
                Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
                SocketException ex = (SocketException)ioex.InnerException;
                Assert.AreEqual(SocketError_NotConnected, ex.ErrorCode);
            }
        }

        //--------
        [Test]
        public void WritesQueuedAtClose_NoLinger_Local()
        {
            BluetoothClient cli;
            TestRfcommPort port;
            Stream strm2;
            Create_ConnectedBluetoothClient_WritePartialsAcceptHalf(out port, out cli, out strm2);
            try {
                // Set Linger mode "no linger".
                cli.LingerState = new LingerOption(false, 0);
                Assert.Fail("should have thrown--1");
            } catch (ArgumentException) {
            }
            cli.Close();
        }



        //--------
        const int Test_LingerTimeSeconds = 10;
        const int Test_LingerTimeTicks = Test_LingerTimeSeconds * 1000;

        [Test]
        [Category("Slow")] // 4secs
        public void WritesQueuedAtCloseLinger_Local_Completes()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtCloseLinger_Init(out port, out strm, out ar1, out ar2);
            int ticks;
            OneEventFirer firer1 = new OneEventFirer(port, 2000);
            firer1.Run(PORT_EV.TXEMPTY);
            OneEventFirer firer2 = new OneEventFirer(port, 2000);
            firer2.Run(PORT_EV.TXEMPTY);
            OneEventFirer firer3 = new OneEventFirer(port, 2000);
            firer3.Run(PORT_EV.TXEMPTY);
            TimeThis(out ticks, delegate(Stream strm2) {
                strm2.Close();
            }, strm);
            WritesQueuedAtCloseLinger_AllSent_Finish(port, strm, ar1, ar2);
            Assert.Less(ticks, Test_LingerTimeTicks);
            firer1.Complete();
            firer2.Complete();
            firer3.Complete();
        }

        [Test]
        [Category("Slow")] // 14secs
        public void WritesQueuedAtCloseLinger_Local_TimesOut()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtCloseLinger_Init(out port, out strm, out ar1, out ar2);
            int ticks, start = Environment.TickCount;
            try {
                strm.Close();
                Assert.Fail("should have thrown");
            } catch (Exception ex) {
                Assert.AreEqual("Linger time-out FIXME", ex.Message, "ex.Message");
            } finally {
                ticks = Environment.TickCount - start;
            }
            WritesQueuedAtClose_Hard_Finish(port, strm, ar1, ar2);
            Assert.Greater(ticks + 1, Test_LingerTimeTicks, "ticks");
        }

        [Test]
        [Category("Slow")] // 14secs
        public void WritesQueuedAtCloseLinger_Local_NotAllCompletes()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtCloseLinger_Init(out port, out strm, out ar1, out ar2);
            OneEventFirer firer1 = new OneEventFirer(port, 2000);
            firer1.Run(PORT_EV.TXEMPTY);
            int ticks, start = Environment.TickCount;
            try {
                strm.Close();
                Assert.Fail("should have thrown");
            } catch (AssertionException) {
                throw;
            } catch (Exception ex) {
                Assert.AreEqual("Linger time-out FIXME", ex.Message, "ex.Message");
            } finally {
                ticks = Environment.TickCount - start;
            }
            WritesQueuedAtCloseLinger_OneMoreSent_Finish(port, strm, ar1, ar2);
            Assert.Greater(ticks + 1, Test_LingerTimeTicks, "ticks");
            firer1.Complete();
        }

        [Test]
        public void CloseLinger_Stream() { CloseLingerX(true, false); }

        [Test]
        public void CloseLinger_Client() { CloseLingerX(false, true); }

        [Test]
        public void CloseLinger_Both() { CloseLingerX(true, true); }

        [Test]
        public void CloseLinger_Neither_SoFinalize() { CloseLingerX(false, false); }

        void CloseLingerX(bool closeStream, bool closeClient)
        {
            LingerOption lingerOption = new LingerOption(true, Test_LingerTimeSeconds);
            CloseX(closeStream, closeClient, lingerOption);
        }

        [Test]
        public void CloseLingerNoWrites()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm2;
            Create_ConnectedBluetoothClient(out port, out cli, out strm2);
            cli.LingerState = new LingerOption(true, Test_LingerTimeSeconds);
            //
            Stream strm=cli.GetStream();
            int ticks;
            TimeThis(out ticks, delegate {
                strm.Close();
            });
            Assert.Less(ticks, 1000/*ms*/, "ticks");
        }

        [Test]
        public void WritesQueuedAtCloseLinger_FromPeer()
        {
            // Linger doesn't apply in this case.
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1;
            IAsyncResult ar2;
            WritesQueuedAtCloseLinger_Init(out port, out strm, out ar1, out ar2);
            port.NewEvent(PORT_EV.CONNECT_ERR);
            WritesQueuedAtClose_Hard_Finish(port, strm, ar1, ar2); // Note NOT Linger!!!
        }

        private void WritesQueuedAtCloseLinger_Init(out TestRfcommPort port, out Stream strm, out IAsyncResult ar1, out IAsyncResult ar2)
        {
            BluetoothClient cli;
            Create_ConnectedBluetoothClient_WritePartialsAcceptHalf(out port, out cli, out strm);
            cli.LingerState = new LingerOption(true, Test_LingerTimeSeconds);
            //
            ar1 = strm.BeginWrite(Data30, 0, Data30.Length, null, null);
            ar2 = strm.BeginWrite(Data30, 0, Data30.Length, null, null);
            Assert.IsFalse(ar1.IsCompleted, "WritesQueuedAtClose--IsComplete ar1");
            port.AssertWrittenContent("WritesQueuedAtClose--written--before", First(Data30, 20));
            Assert.IsFalse(ar2.IsCompleted, "WritesQueuedAtClose--IsComplete ar2");
        }

        private void WritesQueuedAtCloseLinger_AllSent_Finish(TestRfcommPort port, Stream strm, IAsyncResult ar1, IAsyncResult ar2)
        {
            Assert.IsTrue(ar1.IsCompleted, "WritesQueuedAtClose--IsComplete ar1 After close");
            Assert.IsTrue(ar2.IsCompleted, "WritesQueuedAtClose--IsComplete ar2 After close");
            port.AssertWrittenContent("WritesQueuedAtClose--written--after close",
                First(Data30, 20), ExceptFirst(Data30, 20),
                First(Data30, 20), ExceptFirst(Data30, 20)
                );
            strm.EndWrite(ar1);
            strm.EndWrite(ar2);
        }

        private void WritesQueuedAtCloseLinger_OneMoreSent_Finish(TestRfcommPort port, Stream strm, IAsyncResult ar1, IAsyncResult ar2)
        {
            Assert.IsTrue(ar1.IsCompleted, "WritesQueuedAtClose--IsComplete ar1 After close");
            Assert.IsTrue(ar2.IsCompleted, "WritesQueuedAtClose--IsComplete ar2 After close");
            port.AssertWrittenContent("WritesQueuedAtClose--written--after close",
                First(Data30, 20), ExceptFirst(Data30, 20),
                First(Data30, 20)
                );
            strm.EndWrite(ar1);
            try {
                strm.EndWrite(ar2);
                Assert.Fail("should have thrown");
            } catch (IOException ioex) {
                Assert.IsInstanceOfType(typeof(SocketException), ioex.InnerException);
                SocketException ex = (SocketException)ioex.InnerException;
                Assert.AreEqual(SocketError_NotConnected, ex.ErrorCode);
            }
        }

        //--------
        private byte[] First(byte[] buf, int count)
        {
            byte[] ret = new byte[count];
            Array.Copy(buf, ret, ret.Length);
            return ret;
        }

        private byte[] ExceptFirst(byte[] buf, int count)
        {
            byte[] ret = new byte[buf.Length - count];
            Array.Copy(buf, count, ret, 0, ret.Length);
            return ret;
        }

        void TimeThis<T>(out int ticks, Action<T> code, T args)
        {
            int start = Environment.TickCount;
            try {
                code(args);
            } finally {
                ticks = Environment.TickCount - start;
            }
        }


        void TimeThis(out int ticks, System.Threading.ThreadStart code)
        {
            int start = Environment.TickCount;
            try {
                code();
            } finally {
                ticks = Environment.TickCount - start;
            }
        }


    }
}
