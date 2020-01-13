using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace InTheHand.Net.Tests.CommonBt
{
    [TestFixture]
    public class CommonLsnrTests
    {
        const int FakePort = 25;

        //----
        [TestFixtureSetUp]
        [TestFixtureTearDown]
        [TearDown]
        public void ForceFinalization()
        {
            // Force all Listeners to be cleaned-up and show us any errors.
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //----
        class TestData
        {
            public BluetoothListener Lsnr { get; set; }
            public NullBluetoothFactory Fcty { get; set; }
        }

        private static BluetoothListener _SetupListener(out NullBluetoothFactory fcty)
        {
            fcty = new NullBluetoothFactory();
            var lsnr = new BluetoothListener(fcty, BluetoothService.AdvancedAudioDistribution);
            return lsnr;
        }

        private static BluetoothListener SetupListener()
        {
            NullBluetoothFactory fcty;
            var lsnr = _SetupListener(out fcty);
            fcty.TheBtLsnr.AddPortSettings(new NullBtListener.LsnrSetting[] { NullBtListener.LsnrSetting.None });
            return lsnr;
        }

        private static TestData SetupListenerWithPorts(params NullBtListener.LsnrSetting[] settings)
        {
            var data = new TestData();
            NullBluetoothFactory fcty;
            data.Lsnr = _SetupListener(out fcty);
            data.Fcty = fcty;
            fcty.TheBtLsnr.AddPortSettings(settings);
            return data;
        }

        //----
        [Test]
        public void StartStop()
        {
            var lsnr = SetupListener();
            Assert.IsFalse(lsnr.Pending());
            Assert.IsNull(lsnr.LocalEndPoint);
            //
            lsnr.Start();
            Assert.IsFalse(lsnr.Pending());
            Assert.IsNotNull(lsnr.LocalEndPoint);
            Assert.AreEqual(FakePort, lsnr.LocalEndPoint.Port);
            //
            lsnr.Stop();
            Assert.IsFalse(lsnr.Pending());
            Assert.IsNotNull(lsnr.LocalEndPoint);
            Assert.AreEqual(FakePort, lsnr.LocalEndPoint.Port);
        }

        //----
        /*  Waiting acceptors are errored when the listener is stopped.
         */
        [Test]
        public void AcceptThenStop_Async()
        {
            var lsnr = SetupListener();
            lsnr.Start();
            var ar = lsnr.BeginAcceptBluetoothClient(null, null);
            lsnr.Stop();
            try {
                lsnr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
        }

        //----
        /*  Error if Accept is called before Start.
         */

        //[Test]
        [Explicit]
        public void AcceptBeforeStart_Sync()
        {
            var lsnr = SetupListener();
            // BROKEN !!!
            var cli = lsnr.AcceptBluetoothClient();
        }

        [Test]
        public void AcceptBeforeStart_Async()
        {
            var lsnr = SetupListener();
            var ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept");
            try {
                lsnr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException ex) {
                Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
            }
        }

        [Test]
        [Explicit]
        public void AcceptBeforeStart_TryTcpListener()
        {
            System.Net.Sockets.TcpListener lsnr1 = null;
            System.Net.Sockets.TcpListener lsnr2 = null;
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                //-- NO start --
                lsnr2 = new System.Net.Sockets.TcpListener(ep);
                try {
                    var ar2 = lsnr2.BeginAcceptTcpClient(null, null);
                    Assert.Fail("should have thrown");
                } catch (InvalidOperationException ex) {
                    Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
                    // System.InvalidOperationException : 
                    // "Not listening. You must call the Start() method before calling this method."
                }
                // With Start
                lsnr1 = new System.Net.Sockets.TcpListener(ep);
                lsnr1.Start();
                var ar1 = lsnr1.BeginAcceptTcpClient(null, null);
            } finally {
                if (lsnr1 != null) lsnr1.Stop();
                if (lsnr2 != null) lsnr2.Stop();
            }
        }

        [Test]
        [Explicit]
        public void AcceptBeforeStart_TryTcpSocket()
        {
            System.Net.Sockets.Socket lsnr1 = null;
            System.Net.Sockets.Socket lsnr2 = null;
            Converter<System.Net.IPEndPoint, System.Net.Sockets.Socket> createListener
                = delegate(System.Net.IPEndPoint epX)
                {
                    var s = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Unspecified);
                    s.Bind(epX);
                    return s;
                };
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                //-- NO start --
                lsnr2 = createListener(ep);
                try {
                    var ar2 = lsnr2.BeginAccept(null, null);
                    Assert.Fail("should have thrown");
                } catch (InvalidOperationException ex) {
                    Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
                    // System.InvalidOperationException : 
                    // "You must call the Listen method before performing this operation."
                }
                // With Start
                lsnr1 = createListener(ep);
                lsnr1.Listen(1);
                var ar1 = lsnr1.BeginAccept(null, null);
            } finally {
                if (lsnr1 != null) lsnr1.Close();
                if (lsnr2 != null) lsnr2.Close();
            }
        }

        //----
        /*  After Stop is called errors on Accept.
         */

        //[Test]
        [Explicit]
        public void AcceptAfterStop_Sync()
        {
            var lsnr = SetupListener();
            lsnr.Start();
            lsnr.Stop();
            // BROKEN !!!
            var cli = lsnr.AcceptBluetoothClient();
        }

        [Test]
        public void AcceptAfterStop_Async()
        {
            var lsnr = SetupListener();
            lsnr.Start();
            lsnr.Stop();
            var ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept");
            try {
                lsnr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (InvalidOperationException ex) {
                Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
            }
        }

        [Test]
        [Explicit]
        public void AcceptAfterStop_TryTcpListener()
        {
            System.Net.Sockets.TcpListener lsnr = null;
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                //-- NO start --
                lsnr = new System.Net.Sockets.TcpListener(ep);
                lsnr.Start();
                // *
                lsnr.Stop();
                try {
                    var arAccept = lsnr.BeginAcceptTcpClient(null, null);
                    Assert.Fail("should have thrown");
                } catch (InvalidOperationException ex) {
                    Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
                    // System.InvalidOperationException : 
                    // "Not listening. You must call the Start() method before calling this method."
                }
            } finally {
                if (lsnr != null) lsnr.Stop();
            }
        }

        [Test]
        [Explicit]
        public void AcceptAfterStop_TryTcpSocket()
        {
            System.Net.Sockets.Socket lsnr = null;
            Converter<System.Net.IPEndPoint, System.Net.Sockets.Socket> createListener
                = delegate(System.Net.IPEndPoint epX)
                {
                    var s = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Unspecified);
                    s.Bind(epX);
                    return s;
                };
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                lsnr = createListener(ep);
                lsnr.Listen(1);
                // *
                lsnr.Close();
                try {
                    var arAccept = lsnr.BeginAccept(null, null);
                    Assert.Fail("should have thrown");
                } catch (ObjectDisposedException ex) {
                    Assert.AreEqual(typeof(ObjectDisposedException), ex.GetType());
                }
            } finally {
                if (lsnr != null) lsnr.Close();
            }
        }

        //----
        /*  When Stop is called outstanding Accepts receive error.
         */

        [Test]
        public void AcceptOverStop_Async()
        {
            var lsnr = SetupListener();
            lsnr.Start();
            var ar = lsnr.BeginAcceptBluetoothClient(null, null);
            lsnr.Stop();
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept");
            try {
                lsnr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (ObjectDisposedException ex) {
                Assert.AreEqual(typeof(ObjectDisposedException), ex.GetType());
            }
        }


        [Test]
        [Explicit]
        public void AcceptOverStop_TryTcpListener()
        {
            System.Net.Sockets.TcpListener lsnr = null;
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                lsnr = new System.Net.Sockets.TcpListener(ep);
                lsnr.Start();
                var arAccept = lsnr.BeginAcceptTcpClient(null, null);
                // *
                lsnr.Stop();
                TestsApmUtils.SafeNoHangWaitShort(arAccept, "EndAccept");
                try {
                    lsnr.EndAcceptTcpClient(arAccept);
                    Assert.Fail("should have thrown");
                } catch (ObjectDisposedException ex) {
                    Assert.AreEqual(typeof(ObjectDisposedException), ex.GetType());
                }
            } finally {
                if (lsnr != null) lsnr.Stop();
            }
        }

        [Test]
        [Explicit]
        public void AcceptOverStop_TryTcpSocket()
        {
            System.Net.Sockets.Socket lsnr = null;
            Converter<System.Net.IPEndPoint, System.Net.Sockets.Socket> createListener
                = delegate(System.Net.IPEndPoint epX)
                {
                    var s = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Unspecified);
                    s.Bind(epX);
                    return s;
                };
            var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0);
            try {
                //-- NO start --
                lsnr = createListener(ep);
                lsnr.Listen(1);
                var arAccept = lsnr.BeginAccept(null, null);
                lsnr.Close();
                TestsApmUtils.SafeNoHangWaitShort(arAccept, "EndAccept");
                try {
                    lsnr.EndAccept(arAccept);
                    Assert.Fail("should have thrown");
                } catch (ObjectDisposedException ex) {
                    Assert.AreEqual(typeof(ObjectDisposedException), ex.GetType());
                }
            } finally {
                if (lsnr != null) lsnr.Close();
            }
        }

        //----
        [Test]
        public void OneConnectionTwoAccepts()
        {
            IAsyncResult ar;
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.None).Lsnr;
            lsnr.Start();
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-1");
            lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNotCompletesShort(ar, "EndAccept-2");
        }

        [Test]
        [Explicit] // Wierd threading issues?????
        public void AcceptsWithStopStartInBetween()
        {
            IAsyncResult ar;
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately
                ).Lsnr;
            lsnr.Start();
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-1");
            lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-2");
            lsnr.EndAcceptBluetoothClient(ar);
            //
            lsnr.Stop();
            ForceFinalization();
            lsnr.Start();
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-3");
            lsnr.EndAcceptBluetoothClient(ar);
            ForceFinalization();
        }

        [Test]
        public void TenAcceptsOnTenPorts()
        {
            int N = 10;
            IAsyncResult ar;
            var settings = new List<NullBtListener.LsnrSetting>();
            for (int i = 0; i < N; ++i)
                settings.Add(NullBtListener.LsnrSetting.ConnectsImmediately);
            var lsnr = SetupListenerWithPorts(settings.ToArray()).Lsnr;
            lsnr.Start();
            //
            for (int i = 0; i < N; ++i) {
                ar = lsnr.BeginAcceptBluetoothClient(null, null);
                TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-#" + i.ToString());
                lsnr.EndAcceptBluetoothClient(ar);
            }
        }

        [Test]
        public void TenAcceptsOnTenPortsPlusLastOneNotComplete()
        {
            int N = 10;
            IAsyncResult ar;
            var settings = new List<NullBtListener.LsnrSetting>();
            for (int i = 0; i < N; ++i)
                settings.Add(NullBtListener.LsnrSetting.ConnectsImmediately);
            settings.Add(NullBtListener.LsnrSetting.None);
            var lsnr = SetupListenerWithPorts(settings.ToArray()).Lsnr;
            lsnr.Start();
            //
            for (int i = 0; i < N; ++i) {
                ar = lsnr.BeginAcceptBluetoothClient(null, null);
                TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-#" + i.ToString());
                lsnr.EndAcceptBluetoothClient(ar);
            }
            // Not completes
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNotCompletesMiddling(ar, "EndAccept-Last");
        }

        //----
        [Test]
        public void SecondPortCreateFails_BeginAcceptAfterFirstConn()
        {
            IAsyncResult ar;
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ErrorOnOpenServer).Lsnr;
            lsnr.Start();
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-good");
            lsnr.EndAcceptBluetoothClient(ar);
            //
            ar = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar, "EndAccept-Error");
            try {
                lsnr.EndAcceptBluetoothClient(ar);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
        }

        [Test]
        public void SecondPortCreateFails_BeginAcceptBeforeFirstConn()
        {
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ErrorOnOpenServer).Lsnr;
            lsnr.Start();
            //
            var ar1 = lsnr.BeginAcceptBluetoothClient(null, null);
            var ar2 = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar1, "EndAccept-good");
            lsnr.EndAcceptBluetoothClient(ar1);
            //
            TestsApmUtils.SafeNoHangWaitShort(ar2, "EndAccept-Error");
            try {
                lsnr.EndAcceptBluetoothClient(ar2);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
        }

        [Test]
        public void SecondPortCreateFails_ThenOkButServerStaysError_BeginAcceptBeforeFirstConn()
        {
            /*  The server is dead after any DoOpenServer etc port error.
             */
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ErrorOnOpenServer,
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately).Lsnr;
            lsnr.Start();
            //
            var ar1 = lsnr.BeginAcceptBluetoothClient(null, null);
            var ar2 = lsnr.BeginAcceptBluetoothClient(null, null);
            var ar3 = lsnr.BeginAcceptBluetoothClient(null, null);
            //
            TestsApmUtils.SafeNoHangWaitShort(ar1, "EndAccept-good");
            lsnr.EndAcceptBluetoothClient(ar1);
            //
            TestsApmUtils.SafeNoHangWaitShort(ar2, "EndAccept-Error");
            try {
                lsnr.EndAcceptBluetoothClient(ar2);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
            //
            TestsApmUtils.SafeNoHangWaitShort(ar3, "EndAccept-good-3");
            try {
                lsnr.EndAcceptBluetoothClient(ar3);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
        }

        [Test]
        public void SecondPortCreateFails_ThenOkButServerStaysError_BeginAcceptAfterFirstConn()
        {
            /*  The server is dead after any DoOpenServer etc port error.
             */
            var lsnr = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ErrorOnOpenServer,
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately).Lsnr;
            lsnr.Start();
            //
            var ar1 = lsnr.BeginAcceptBluetoothClient(null, null);
            //
            TestsApmUtils.SafeNoHangWaitShort(ar1, "EndAccept-good");
            lsnr.EndAcceptBluetoothClient(ar1);
            //
            var ar2 = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar2, "EndAccept-Error");
            try {
                lsnr.EndAcceptBluetoothClient(ar2);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
            //
            var ar3 = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar3, "EndAccept-good-3");
            try {
                lsnr.EndAcceptBluetoothClient(ar3);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
        }

        [Test]
        /*  We don't explicitly support Start-Stop-Start in CmnBtLsnr.
         *  So even less so after a failure!
         */
        [Explicit]
        public void SecondPortCreateFails_ThenStopStart()
        {
            var data = SecondPortCreateFails_ThenStop__();
            //
            data.Fcty.TheBtLsnr.Start();
            //
            //var ar3 = lsnr.BeginAcceptBluetoothClient(null, null);
            //TestsApmUtils.SafeNoHangWaitShort(ar3, "EndAccept-good-3");
            //lsnr.EndAcceptBluetoothClient(ar3);
            //try {
            //    Assert.Fail("should have thrown!");
            //} catch (Exception) {
            //}
        }

        [Test]
        public void SecondPortCreateFails_ThenStop()
        {
            SecondPortCreateFails_ThenStop__();
        }

        TestData SecondPortCreateFails_ThenStop__()
        {
            /*  The server is dead after any DoOpenServer etc port error.
             */
            var data = SetupListenerWithPorts(
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ErrorOnOpenServer,
                NullBtListener.LsnrSetting.ConnectsImmediately,
                NullBtListener.LsnrSetting.ConnectsImmediately);
            var lsnr = data.Lsnr;
            Assert.IsFalse(data.Fcty.TheBtLsnr.AllDisposed, "disposed 1 NOT");
            lsnr.Start();
            Assert.IsFalse(data.Fcty.TheBtLsnr.AllDisposed, "disposed 2 NOT");
            //
            var ar1 = lsnr.BeginAcceptBluetoothClient(null, null);
            //
            TestsApmUtils.SafeNoHangWaitShort(ar1, "EndAccept-good");
            lsnr.EndAcceptBluetoothClient(ar1);
            //
            var ar2 = lsnr.BeginAcceptBluetoothClient(null, null);
            TestsApmUtils.SafeNoHangWaitShort(ar2, "EndAccept-Error");
            try {
                lsnr.EndAcceptBluetoothClient(ar2);
                Assert.Fail("should have thrown!");
            } catch (Exception) {
            }
            //
            lsnr.Stop();
            Assert.IsTrue(data.Fcty.TheBtLsnr.AllDisposed);
            //
            return data;
        }

    }
}
