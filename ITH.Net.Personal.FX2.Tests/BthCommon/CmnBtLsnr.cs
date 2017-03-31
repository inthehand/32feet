// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Tests.BluetopiaTests;  // HACK refactor
using Moq;
using NUnit.Framework;

namespace InTheHand.Net.Tests.BthCommon
{
    [TestFixture]
    public class CmnBtLsnrTests
    {

        //--------
        private static MyCommonBluetoothListener CreateLsnr(MyCommonRfcommStream[] portList, MockBehavior b)
        {
            byte serverPort = 5;
            var outBep = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.Empty, serverPort);
            //
            var ports = new Queue<MyCommonRfcommStream>(portList);
            //---------------------
            var mockF = new Mock<MyBluetoothFactory>();
            var fcty = mockF.Object;
            //---------------------
            var ctorArgs = new object[] { fcty };
            var mockS = new Mock<MyCommonBluetoothListener>(b, ctorArgs);
            //
            SetupLsnrDisposeMethods(mockS);
            int j = 0;
            mockS.Setup(x => x.tSetupListener(It.IsAny<BluetoothEndPoint>(), It.IsAny<Int32>()))
                //.Callback<BluetoothEndPoint, int, BluetoothEndPoint>((epX, pX, epX2) => ++j)
                .Returns(outBep)
                ;
            mockS.Setup<CommonRfcommStream>(s => s.tGetNewPort())
                .Returns(ports.Dequeue);
            //
            var svr = mockS.Object;
            return svr;
        }

        private static void SetupLsnrDisposeMethods(Mock<MyCommonBluetoothListener> mockS)
        {
            bool server_disposed = false;
            //int debugJ = 0;
            mockS.SetupGet<bool>(s => s.tIsDisposed)
                //.Callback(() => debugJ++)
                .Returns(() => server_disposed);
            mockS.Setup(s => s.tOtherDispose(true))
                .Callback(delegate(bool x) { server_disposed = true; });
            mockS.Setup(s => s.tOtherDisposeMore()); // NOP
        }

        //class StuffBtLsnr
        //{
        //}

        //--------
        [Test]
        public void GetNewPortFailsFirstTime_toBeRefactored()
        {
            var outBep = new BluetoothEndPoint(BluetoothAddress.None, BluetoothService.WspProtocol);
            //---------------------
            var ctorArgs = new object[] { null };
            MockBehavior b = MockBehavior.Strict;
            var mockS = new Mock<MyCommonBluetoothListener>(b, ctorArgs);
            //
            SetupLsnrDisposeMethods(mockS);
            mockS.Setup(x => x.tSetupListener(It.IsAny<BluetoothEndPoint>(), It.IsAny<Int32>()))
                .Returns(outBep);
            mockS.Setup(s => s.tGetNewPort())
                .Throws<System.Net.Sockets.SocketException>(); //!!
            //
            var svr = mockS.Object;
            svr.Construct(BluetoothService.CordlessTelephony);
            try {
                svr.Start();
                Assert.Fail("should have thrown!");
            } catch (SocketException ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void GetNewPortFailsFirstTime2()
        {
            var ports = new MyCommonRfcommStream[0];
            var svr = CreateLsnr(ports, MockBehavior.Strict);
            svr.Construct(BluetoothService.CordlessTelephony);
            try {
                svr.Start();
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException ex) {
                Assert.AreEqual("Queue empty.", ex.Message, "ex.Message (LANG)");
            }
        }

        //--------
        [Test]
        public void HandleCONNECTERR()
        {
            var mockP = new Mock<MyCommonRfcommStream>();
            var p = mockP.Object;
            var ports = new MyCommonRfcommStream[] { p };
            //---------------------
            var svr = CreateLsnr(ports, MockBehavior.Loose);
            svr.Construct(BluetoothService.CordlessTelephony); // ****
            svr.Start(); // ****
            var ar = svr.BeginAcceptBluetoothClient(null, null);
            Assert.IsFalse(ar.IsCompleted, "isc0");
            //--
            p.HandleCONNECT_ERR("Weeeee", null); // ****
            ClientTesting.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "isc1");
            try {
                svr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (SocketException ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void HandleCONNECTED()
        {
            Action<CommonBluetoothListener> doConstruct
                = svr => svr.Construct(BluetoothService.CordlessTelephony);
            _HandleCONNECTED(doConstruct);
        }

        [Test]
        public void HandleCONNECTED_localPortGiven()
        {
            Action<CommonBluetoothListener> doConstruct
                = svr => svr.Construct(new BluetoothEndPoint(BluetoothAddress.None,
                    BluetoothService.CordlessTelephony, 6));
            _HandleCONNECTED(doConstruct);
        }

        void _HandleCONNECTED(Action<CommonBluetoothListener> doConstruct)
        {
            var mockP = new Mock<MyCommonRfcommStream>();
            var p = mockP.Object;
            //
            var ports = new MyCommonRfcommStream[] {
                p,
                new Mock<MyCommonRfcommStream>().Object };
            //--
            var svr = CreateLsnr(ports, MockBehavior.Loose);
            doConstruct(svr); // ****
            //
            svr.Start(); // ****
            var ar = svr.BeginAcceptBluetoothClient(null, null);
            Assert.IsFalse(ar.IsCompleted, "isc0");
            //--
            p.HandleCONNECTED("Weeeee"); // ****
            ClientTesting.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "isc1");
            svr.EndAcceptBluetoothClient(ar);
        }

        //-------------
        [Test]
        public void NextGetPortFails_localPortGiven()
        {
            Action<CommonBluetoothListener> doConstruct
                = svr => svr.Construct(new BluetoothEndPoint(BluetoothAddress.None,
                    BluetoothService.CordlessTelephony, 6));
            NextGetPortFails(doConstruct);
        }

        void NextGetPortFails(Action<CommonBluetoothListener> doConstruct)
        {
            var mockP = new Mock<MyCommonRfcommStream>();
            var p = mockP.Object;
            var ports = new MyCommonRfcommStream[] {
                p };
            //--
            var svr = CreateLsnr(ports, MockBehavior.Loose);
            doConstruct(svr); // ****
            //
            svr.Start(); // ****
            var ar = svr.BeginAcceptBluetoothClient(null, null);
            var ar2 = svr.BeginAcceptBluetoothClient(null, null);
            Assert.IsFalse(ar.IsCompleted, "isc0");
            //--
            p.HandleCONNECTED("Weeeee"); // ****
            ClientTesting.SafeWait(ar);
            Assert.IsTrue(ar.IsCompleted, "isc1");
            svr.EndAcceptBluetoothClient(ar);
            try {
                svr.EndAcceptBluetoothClient(ar2);
                Assert.Fail("should have thrown!");
            } catch (Exception ex) {
            }
            //
            try {
                var ar3 = svr.BeginAcceptBluetoothClient(null, null);
                Assert.Fail("should have thrown!");
                //svr.EndAcceptBluetoothClient(ar3);
            } catch (Exception ex) {
            }
        }

    }
}
