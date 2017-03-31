using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Sockets;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using InTheHand.Net.Bluetooth.Factory;
using System.Threading;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommBluetoothListenerTest
    {
        private static TestLsnrRfcommPort Init_OneListener()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            //
            return port0;
        }

        private static TestLsnrRfcommPort Init_OneListenerStartedTwice()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            f.MaxSdpServices = 2;
            // For Start()
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            // For Start
            TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port1);
            // For Client Finalize
            TestLsnrRfcommPort port2 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port2);
            //
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            port1.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            port2.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            //
            return port0;
        }

        [Test]
        public void StopNoStart()
        {
            TestLsnrRfcommPort port0 = Init_OneListener();
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            lsnr.Stop();
        }

        [Test]
        public void StopStartStop()
        {
            TestLsnrRfcommPort port0 = Init_OneListenerStartedTwice();
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            lsnr.Start();
            lsnr.Stop();
            lsnr.Start();
            //
            lsnr = null;
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //----
        [Test]
        public void OneConnection()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            TestLsnrRfcommPort port1 = AddSomeCreatablePorts(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            lsnr.Start();
            IAsyncResult ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port0.AssertOpenServerCalledAndClear(29);//adter Start?
            port0.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli = lsnr.EndAcceptBluetoothClient(ar);
            TestSdpService2 sdpSvc = f.GetTestSdpService();
            Assert.AreEqual(0, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            lsnr.Stop();
            Assert.AreEqual(1, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            //
            Assert.IsTrue(cli.Connected, "cli.Connected");
            Assert.AreEqual(0, f.queueIRfCommPort.Count, "Used both ports");
            port1.AssertCloseCalledOnce("second acceptor closed");
            cli.Close();
            port0.AssertCloseCalledOnce("first accepted connection now closed");
            //
            BluetoothEndPoint lep = lsnr.LocalEndPoint;
            sdpSvc.AssertCalls(
                "AddServiceClassIdList: 00001303-0000-1000-8000-00805f9b34fb" + NewLine
                + "AddRFCommProtocolDescriptor: " + lep.Port + NewLine
                );
        }

        private static TestLsnrRfcommPort AddACreatablePort(TestWcLsnrBluetoothFactory f, PORT_RETURN_CODE result)
        {
            TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
            port1.SetOpenServerResult(result); // now begun immediately
            f.queueIRfCommPort.Enqueue(port1);
            return port1;
        }

        private static TestLsnrRfcommPort AddSomeCreatablePorts(TestWcLsnrBluetoothFactory f)
        {
            TestLsnrRfcommPort port1 = AddACreatablePort(f, PORT_RETURN_CODE.SUCCESS);
            //AddACreatablePort(f, PORT_RETURN_CODE.SUCCESS);
            //AddACreatablePort(f, PORT_RETURN_CODE.SUCCESS);
            //AddACreatablePort(f, PORT_RETURN_CODE.SUCCESS);
            return port1;
        }

        [Test]
        public void OneConnection_PendingBeforeSyncAccept()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            try {
                Assert.IsFalse(lsnr.Pending(), "!Pending before Start");
                lsnr.Start();
                port0.AssertOpenServerCalledAndClear(29);//adter Start?
                TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
                port1.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS); // now begun immediately
                f.queueIRfCommPort.Enqueue(port1);
                Assert.IsFalse(lsnr.Pending(), "!Pending before");
                port0.NewEvent(PORT_EV.CONNECTED);
                Thread.Sleep(100);
                Assert.IsTrue(lsnr.Pending(), "Pending");
                BluetoothClient cli = lsnr.AcceptBluetoothClient();
                TestSdpService2 sdpSvc = f.GetTestSdpService();
                Assert.AreEqual(0, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
                lsnr.Stop();
                Assert.AreEqual(1, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
                //
                Assert.IsTrue(cli.Connected, "cli.Connected");
                //TODOAssert.AreEqual(0, f.queueIRfCommPort.Count, "Used both ports");
                //TODO port1.AssertCloseCalledOnce("second acceptor closed");
                cli.Close();
                port0.AssertCloseCalledOnce("first accepted connection now closed");
                //
                BluetoothEndPoint lep = lsnr.LocalEndPoint;
                sdpSvc.AssertCalls(
                    "AddServiceClassIdList: 00001303-0000-1000-8000-00805f9b34fb" + NewLine
                    + "AddRFCommProtocolDescriptor: " + lep.Port + NewLine
                    );
                //
                //
            } finally {
                lsnr.Stop(); // See errors that might otherwise occur on the Finalizer.
            }
        }

        [Test]
        public void OneConnection_PeerImmediatelyCloses()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            lsnr.Start();
            IAsyncResult ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port0.AssertOpenServerCalledAndClear(29);//adter Start?
            TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port1);
            port1.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS); // now begun immediately
            FireOpenReceiveCloseEvents firer = new FireOpenReceiveCloseEvents(port0);
            firer.Run(); //port0.NewEvent(PORT_EV.CONNECTED);
            //Assert.IsFalse(ar.IsCompleted, "Connect 1 completed"); // 100ms later...
            firer.Complete();
            port0.AssertCloseCalledOnce("first accepted connection now closed");
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli = lsnr.EndAcceptBluetoothClient(ar);
            lsnr.Stop();
            //
            //TODO ! Assert.IsTrue(cli.Connected, "cli.Connected");
            Assert.AreEqual(0, f.queueIRfCommPort.Count, "Used both ports");
            port1.AssertCloseCalledOnce("second acceptor closed");
            //
            Stream peer = cli.GetStream();
            byte[] buf = new byte[10];
            int readLen = TestsApmUtils.SafeNoHangRead(peer, buf, 0, buf.Length);
            Assert.AreEqual(1, readLen, "readLen");
            cli.Close();
            port0.AssertCloseCalledAtLeastOnce("first accepted connection now closed");
        }

        [Test]
        public void ZeroConnections()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            lsnr.Start();
            IAsyncResult ar = lsnr.BeginAcceptBluetoothClient(null, null);
            lsnr.Stop();
            Assert.IsTrue(ar.IsCompleted, ".IsCompleted");
            try {
                BluetoothClient cli = lsnr.EndAcceptBluetoothClient(ar);
            } catch (ObjectDisposedException) {
            }
            //
            Assert.AreEqual(0, f.queueIRfCommPort.Count, "Used all ports");
            port0.AssertCloseCalledOnce("acceptor closed");
        }

        [Test]
        public void OneFailedIncomingConnection()
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSink);
            lsnr.ServiceName = "weeee";
            lsnr.Start();
            IAsyncResult ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port1);
            port1.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS); // now begun immediately
            port0.NewEvent(PORT_EV.CONNECT_ERR);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            port0.AssertOpenServerCalledAndClear(29);
            try {
                try {
                    BluetoothClient cli = lsnr.EndAcceptBluetoothClient(ar);
                } catch (System.IO.IOException ioexShouldNotWrapSEx) { //HACK ioexShouldNotWrapSEx
                    throw ioexShouldNotWrapSEx.InnerException;
                }
                Assert.Fail("should have thrown!");
            } catch (SocketException) {
            }
            TestSdpService2 sdpSvc = f.GetTestSdpService();
            Assert.AreEqual(0, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            lsnr.Stop();
            Assert.AreEqual(1, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            //
            Assert.AreEqual(0, f.queueIRfCommPort.Count, "Used both ports");
            port1.AssertCloseCalledOnce("second acceptor closed");
            //port0.AssertCloseCalledOnce("first failed connection now closed");
            //
            BluetoothEndPoint lep = lsnr.LocalEndPoint;
            sdpSvc.AssertCalls(
                "AddServiceClassIdList: 00001304-0000-1000-8000-00805f9b34fb" + NewLine
                + "AddRFCommProtocolDescriptor: " + lep.Port + NewLine
                + "AddServiceName: weeee" + NewLine
                );
        }

        [Test]
        public void MultipleConnection()
        {
            MultipleConnection_(false, false, BTM_SEC.NONE);
        }
        [Test]
        public void MultipleConnection_AuthNotEncrypt()
        {
            MultipleConnection_(true, false, BTM_SEC.IN_AUTHENTICATE);
        }
        [Test]
        public void MultipleConnection_NotAuthEncrypt()
        {
            MultipleConnection_(false, true, BTM_SEC.IN_ENCRYPT | BTM_SEC.IN_AUTHENTICATE);
        }
        [Test]
        public void MultipleConnection_AuthEncrypt()
        {
            MultipleConnection_(true, true, BTM_SEC.IN_ENCRYPT | BTM_SEC.IN_AUTHENTICATE);
        }

        void MultipleConnection_(bool auth, bool encrypt, 
            BTM_SEC expectedSecurityLevel)
        {
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothFactory.SetFactory(f);
            //
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource);
            if (auth)
                lsnr.Authenticate = true;
            if (encrypt)
                lsnr.Encrypt = true;
            Assert.AreEqual(auth, lsnr.Authenticate, ".Authenticate 1");
            Assert.AreEqual(encrypt, lsnr.Encrypt, ".Encrypt 1");
            lsnr.Start();
            IAsyncResult ar;
            commIf.AssertSetSecurityLevel(expectedSecurityLevel, true);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port0.AssertOpenServerCalledAndClear(29);//adter Start?
            TestLsnrRfcommPort port1 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port1);
            port1.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            port0.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli0 = lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port1.AssertOpenServerCalledAndClear(29);//adter Start?
            TestLsnrRfcommPort port2 = new TestLsnrRfcommPort();
            port2.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            f.queueIRfCommPort.Enqueue(port2);
            port1.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli1 = lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port2.AssertOpenServerCalledAndClear(29);//adter Start?
            TestLsnrRfcommPort port3 = new TestLsnrRfcommPort();
            port3.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            f.queueIRfCommPort.Enqueue(port3);
            port2.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli2 = lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            port3.AssertOpenServerCalledAndClear(29);//adter Start?
            TestLsnrRfcommPort port4 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port4);
            port4.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            port3.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            BluetoothClient cli3 = lsnr.EndAcceptBluetoothClient(ar);
            //
            TestSdpService2 sdpSvc = f.GetTestSdpService();
            Assert.AreEqual(0, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            lsnr.Stop();
            Assert.AreEqual(1, sdpSvc.NumDisposeCalls, "NumDisposeCalls");
            Assert.AreEqual(auth, lsnr.Authenticate, ".Authenticate 2");
            Assert.AreEqual(encrypt, lsnr.Encrypt, ".Encrypt 2");
            //
            Assert.IsTrue(cli0.Connected, "0 cli.Connected");
            Assert.AreEqual(0, f.queueIRfCommPort.Count, "Used both ports");
            port4.AssertCloseCalledOnce("4 acceptor closed");
            cli0.Close();
            port0.AssertCloseCalledOnce("0 accepted connection now closed");
            //
            Assert.IsTrue(cli1.Connected, "1 cli.Connected");
            cli1.Close();
            port1.AssertCloseCalledOnce("1 accepted connection now closed");
            //
            Assert.IsTrue(cli2.Connected, "2 cli.Connected");
            cli2.Close();
            port2.AssertCloseCalledOnce("2 accepted connection now closed");
            //
            Assert.IsTrue(cli3.Connected, "3 cli.Connected");
            cli3.Close();
            port3.AssertCloseCalledOnce("3 accepted connection now closed");
        }


        delegate TResult MyFunc<T0, TResult>(T0 p0);
        delegate TResult MyFunc<T0, T1, TResult>(T0 p0, T1 p1);

        private static ServiceRecord CreateAVariousRecord()
        {
            MyFunc<ElementType, ServiceAttributeId> createId = delegate(ElementType etX) {
                return (ServiceAttributeId)0x4000 + checked((byte)etX);
            };
            ServiceRecordBuilder bldr = new ServiceRecordBuilder();
            bldr.AddServiceClass(BluetoothService.HealthDevice);
            bldr.ServiceName = "alan";
            IList<ServiceAttribute> attrList = new List<ServiceAttribute>();
            ElementType et_;
#if SUPPORT_NIL
            et_ = ElementType.Nil;
            attrList.Add(new ServiceAttribute(createId(et_), new ServiceElement(et_, null)));
#endif
            et_ = ElementType.Boolean;
            attrList.Add(new ServiceAttribute(createId(et_), new ServiceElement(et_, true)));
            ElementType[] weee = {
                ElementType.UInt8,  ElementType.UInt16, ElementType.UInt32, //UInt64, UInt128,
                ElementType.Int8,   ElementType.Int16,  ElementType.Int32, //Int64, Int128,
            };
            foreach (ElementType et in weee) {
                attrList.Add(new ServiceAttribute(
                    createId(et),
                    ServiceElement.CreateNumericalServiceElement(et, (uint)et)));
            }
            et_ = ElementType.Uuid16;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, (UInt16)et_)));
            et_ = ElementType.Uuid32;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, (UInt32)et_)));
            et_ = ElementType.Uuid128;
            attrList.Add(new ServiceAttribute(createId(et_),
                new ServiceElement(et_, BluetoothService.CreateBluetoothUuid((int)et_))));
            bldr.AddCustomAttributes(attrList);
            bldr.AddCustomAttributes(ElementsAndVariableAndFixedInDeepTree1());
            ServiceRecord record = bldr.ServiceRecord;
            return record;
        }

        public static IList<ServiceAttribute> ElementsAndVariableAndFixedInDeepTree1()
        {
            IList<ServiceAttribute> attrs = new List<ServiceAttribute>();
            //
            String str = InTheHand.Net.Tests.Sdp2.Data_SdpCreator_SingleElementTests.RecordBytes_OneString_StringValue;
            ServiceElement itemStr1 = new ServiceElement(ElementType.TextString, str);
            ServiceElement itemStr2 = new ServiceElement(ElementType.TextString, str);
            //
            Uri uri = new Uri("http://example.com/foo.txt");
            ServiceElement itemUrl = new ServiceElement(ElementType.Url, uri);
            //
            ServiceElement itemF1 = new ServiceElement(ElementType.UInt16, (UInt16)0xfe12);
            ServiceElement itemF2 = new ServiceElement(ElementType.UInt16, (UInt16)0x1234);
            //
            IList<ServiceElement> leaves2 = new List<ServiceElement>();
            leaves2.Add(itemStr1);
            leaves2.Add(itemUrl);
            leaves2.Add(itemF1);
            ServiceElement e2 = new ServiceElement(ElementType.ElementSequence, leaves2);
            //
            ServiceElement e1 = new ServiceElement(ElementType.ElementSequence, e2);
            //
            IList<ServiceElement> leaves0 = new List<ServiceElement>();
            leaves0.Add(e1);
            leaves0.Add(itemStr2);
            leaves0.Add(itemF2);
            attrs.Add(
                new ServiceAttribute(0x0401, new ServiceElement(ElementType.ElementAlternative,
                        leaves0)));
            return attrs;
        }

        [Test]
        public void TestCreateAVariousRecord()
        {
            ServiceRecord record = CreateAVariousRecord();
#if false
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            BluetoothFactory.SetFactory(f);
            //----
            int port = 19;
            ServiceRecordHelper.SetRfcommChannelNumber(record, port);
            //
            ISdpService x0 = SdpService.CreateCustom(record);
            TestSdpService2 x = (TestSdpService2)x0;
#endif
            TestWcLsnrBluetoothFactory f = new TestWcLsnrBluetoothFactory();
            TestLsnrRfCommIf commIf = new TestLsnrRfCommIf();
            f.queueIRfCommIf.Enqueue(commIf);
            TestLsnrRfcommPort port0 = new TestLsnrRfcommPort();
            f.queueIRfCommPort.Enqueue(port0);
            BluetoothFactory.SetFactory(f);
            //
            port0.SetOpenServerResult(PORT_RETURN_CODE.SUCCESS);
            BluetoothListener lsnr = new BluetoothListener(BluetoothService.VideoSource,
                record);
            lsnr.Start();
            int port = lsnr.LocalEndPoint.Port;
            lsnr.Stop();
            //
            TestSdpService2 x = f.GetTestSdpService();
            x.AssertCalls(
                // Well-known
                "AddServiceClassIdList: <00001400-0000-1000-8000-00805f9b34fb>" + NewLine
                + "AddRFCommProtocolDescriptor: " + port + NewLine
                // Eeeeeeech, how to detect all possible LangOffset_WellKnown strings!!!!
                + "AddAttribute: id: 0x0006, dt: DATA_ELE_SEQ, len: 9, val: "
                +   "09-65-6E-" + "09-00-6A-" + "09-01-00" + NewLine
                + "AddAttribute: id: 0x0100, dt: TEXT_STR, len: 4, val: 61-6C-61-6E" + NewLine
                + "AddAttribute: id: 0x0401, dt: DATA_ELE_ALT, len: 66, val: "
                    + "35-2F-35-2D-25-0C-61-62-63-64-C3-A9-66-67-68-C4-"
                    + "AD-6A-45-1A-68-74-74-70-3A-2F-2F-65-78-61-6D-70-"
                    + "6C-65-2E-63-6F-6D-2F-66-6F-6F-2E-74-78-74-09-FE-"
                    + "12-25-0C-61-62-63-64-C3-A9-66-67-68-C4-AD-6A-09-"
                    + "12-34" + NewLine
                + "AddAttribute: id: 0x4015, dt: UINT, len: 1, val: 15" + NewLine
                + "AddAttribute: id: 0x4016, dt: UINT, len: 2, val: 00-16" + NewLine
                + "AddAttribute: id: 0x4017, dt: UINT, len: 4, val: 00-00-00-17" + NewLine
                + "AddAttribute: id: 0x401E, dt: TWO_COMP_INT, len: 1, val: 1E" + NewLine
                + "AddAttribute: id: 0x401F, dt: TWO_COMP_INT, len: 2, val: 00-1F" + NewLine
                + "AddAttribute: id: 0x4020, dt: TWO_COMP_INT, len: 4, val: 00-00-00-20" + NewLine
                + "AddAttribute: id: 0x4028, dt: UUID, len: 2, val: 00-28" + NewLine
                + "AddAttribute: id: 0x4029, dt: UUID, len: 4, val: 00-00-00-29" + NewLine
                + "AddAttribute: id: 0x402A, dt: UUID, len: 16, val: 00-00-00-2A-00-00-10-00-80-00-00-80-5F-9B-34-FB" + NewLine
                + "AddAttribute: id: 0x402C, dt: BOOLEAN, len: 1, val: 01" + NewLine
                );

        }

        const string NewLine = "\r\n";
    }
}
