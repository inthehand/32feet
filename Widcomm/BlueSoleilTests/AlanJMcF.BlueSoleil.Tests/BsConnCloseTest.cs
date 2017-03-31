using System;
using System.Text;
using System.Collections.Generic;
#if NUNIT
using NUnit.Framework;
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestAttribute;
using TestContext = System.Version; //dummy
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using InTheHand.Net.Bluetooth.BlueSoleil;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace InTheHand.Net.Tests.BlueSoleil
{

    /// <summary>
    /// Summary description for BsConnClose
    /// </summary>
    [TestClass]
    public class BsConnCloseTest
    {
        public BsConnCloseTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //--------
        const int WeeWait = 200;

        private static BlueSoleilSerialPortNetworkStream CreateConnectedStream(out BsTestNullApi api)
        {
            api = new BsTestNullApi();
            api.SetIsOpen();
            var fcty = new BluesoleilFactory(api);
            TestSerialPortWrapper port = new TestSerialPortWrapper();
            UInt32 hConn = 0;
            var cli = (BluesoleilClient)fcty.DoGetBluetoothClient();
            var conn = new BlueSoleilSerialPortNetworkStream(port, hConn, cli, fcty);
            return conn;
        }

        private void RaiseNetworkClose(BlueSoleilSerialPortNetworkStream conn, BsTestNullApi api)
        {
            api.SetIsNotOpen();
            ((IBluesoleilConnection)conn).CloseNetworkOrInternal();
        }

        private void AssertIsOpen(BlueSoleilSerialPortNetworkStream conn, bool open, string name)
        {
            Assert.AreEqual(open, conn.Connected, "conn.Connected -- " + name);
            //Assert.IsTrue(conn.CanRead, "conn.CanRead");
            //Assert.IsTrue(conn.CanWrite, "conn.CanWrite");
        }

        //--------
        [TestMethod]
        public void LocalClose_Reads()
        {
            BsTestNullApi api;
            var conn = CreateConnectedStream(out api);
            AssertIsOpen(conn, true, "top");
            int readLen;
            var buf = new byte[10];
            var action = (Converter<Version, int>)delegate {
                int readLenInner = conn.Read(buf, 0, buf.Length);
                return readLenInner;
            };
            var arList = new IAsyncResult[3];
            for (int i = 0; i < arList.Length; ++i) {
                arList[i] = action.BeginInvoke(null, null, null);
            }
            Thread.Sleep(WeeWait);
            for (int i = 0; i < arList.Length; ++i) {
                Assert.IsFalse(arList[i].IsCompleted, "before: IsCompleted [" + i + "]");
            }
            AssertIsOpen(conn, true, "before");
            conn.Close(); // ************
            // TODO This should be 'true' -- till an I/O op fails: AssertIsOpen(conn, true, "after");
            AssertIsOpen(conn, false, "after");
            for (int i = 0; i < arList.Length; ++i) {
                TestsApmUtils.SafeNoHangWaitShort(arList[i], "after: completed [" + i + "]");
                Assert.IsTrue(arList[i].IsCompleted, "after: IsCompleted [" + i + "]");
            }
            for (int i = 0; i < arList.Length; ++i) {
                try {
                    readLen = action.EndInvoke(arList[i]);
                    Assert.Fail("should have thrown");
                } catch (IOException ioex) {
                    Exception ex = ioex.InnerException;
                    Assert.IsInstanceOfType(typeof(SocketException), ex, "[" + i + "] InInstance");
                    var soex = (SocketException)ex;
                    Assert.AreEqual(SocketError.Interrupted, soex.SocketErrorCode, "[" + i + "] SocketErrorCode");
                } catch (ObjectDisposedException) {
                    Assert.Greater(i, 0, "[" + i + "] For first pending Read the error must be IOEx(SocketEx((WSAEINTR))).");
                } catch (Exception ex) {
                    Assert.Fail("[" + i + "] Bad exception from Read: " + ex);
                }
            }
            AssertIsOpen(conn, false, "bottom");
            //
            var arAfter = action.BeginInvoke(null, null, null);
            TestsApmUtils.SafeNoHangWaitShort(arAfter, "after after");
            Assert.IsTrue(arAfter.IsCompleted, "after after: IsCompleted");
            try {
                readLen = action.EndInvoke(arAfter);
                Assert.Fail("should have thrown");
            } catch (ObjectDisposedException) {
                //IOException ioex) {
                //Exception ex = ioex.InnerException;
                //Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex);
            } catch (Exception ex) {
                Assert.Fail("Bad exception from Read: " + ex);
            }
            AssertIsOpen(conn, false, "bottom-bottom");
            //
            conn.Close();
            conn.Close();
            conn.Close();
        }

        [TestMethod]
        public void LocalClose_WriteAfter()
        {
            BsTestNullApi api;
            var conn = CreateConnectedStream(out api);
            AssertIsOpen(conn, true, "top");
            conn.Flush();
            var buf = new byte[10];
            var action = (Action<Version>)delegate {
                conn.Write(buf, 0, buf.Length);
            };
            //var arList = new IAsyncResult[3];
            //for (int i = 0; i < arList.Length; ++i) {
            //    arList[i] = action.BeginInvoke(null, null, null);
            //}
            //Thread.Sleep(WeeWait);
            //for (int i = 0; i < arList.Length; ++i) {
            //    Assert.IsFalse(arList[i].IsCompleted, "before: IsCompleted [" + i + "]");
            //}
            AssertIsOpen(conn, true, "before");
            conn.Close(); // ************
            // TODO This should be 'true' -- till an I/O op fails: AssertIsOpen(conn, true, "after");
            AssertIsOpen(conn, false, "after");
            //Thread.Sleep(WeeWait);
            //for (int i = 0; i < arList.Length; ++i) {
            //    Assert.IsTrue(arList[i].IsCompleted, "after: IsCompleted [" + i + "]");
            //}
            //for (int i = 0; i < arList.Length; ++i) {
            //    action.EndInvoke(arList[i]);
            //}
            //
            var arAfter = action.BeginInvoke(null, null, null);
            TestsApmUtils.SafeNoHangWaitShort(arAfter, "after after");
            Assert.IsTrue(arAfter.IsCompleted, "after after: IsCompleted");
            try {
                action.EndInvoke(arAfter);
                Assert.Fail("should have thrown!");
            } catch (ObjectDisposedException) {
            //} catch (IOException ioex) {
            //    Exception ex = ioex.InnerException;
            //    Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex);
            } catch (Exception ex) {
                Assert.Fail("Bad exception from Write: " + ex);
            }
            AssertIsOpen(conn, false, "bottom");
            // NetworkStream doesn't use Flush, so it doesn't throw on connection
            // close, thus we need to duplicate that. :-(
            conn.Flush();
            //
            conn.Close();
            conn.Close();
            conn.Close();
        }

        [TestMethod]
        public void NetworkClose_Reads()
        {
            BsTestNullApi api;
            var conn = CreateConnectedStream(out api);
            AssertIsOpen(conn, true, "top");
            int readLen;
            var buf = new byte[10];
            var action = (Converter<Version, int>)delegate {
                int readLenInner = conn.Read(buf, 0, buf.Length);
                return readLenInner;
            };
            var arList = new IAsyncResult[3];
            for (int i = 0; i < arList.Length; ++i) {
                arList[i] = action.BeginInvoke(null, null, null);
            }
            Thread.Sleep(WeeWait);
            for (int i = 0; i < arList.Length; ++i) {
                Assert.IsFalse(arList[i].IsCompleted, "before: IsCompleted [" + i + "]");
            }
            AssertIsOpen(conn, true, "before");
            RaiseNetworkClose(conn, api); // ************
            // TODO This should be 'true' -- till an I/O op fails: AssertIsOpen(conn, true, "after");
            AssertIsOpen(conn, false, "after");
            for (int i = 0; i < arList.Length; ++i) {
                TestsApmUtils.SafeNoHangWaitShort(arList[i], "after: completed [" + i + "]");
                Assert.IsTrue(arList[i].IsCompleted, "after: IsCompleted [" + i + "]");
            }
            for (int i = 0; i < arList.Length; ++i) {
                readLen = action.EndInvoke(arList[i]);
                Assert.AreEqual(0, readLen, "after: readLen [" + i + "]");
            }
            AssertIsOpen(conn, false, "bottom");
            //
            var arAfter = action.BeginInvoke(null, null, null);
            TestsApmUtils.SafeNoHangWaitShort(arAfter, "after after");
            Assert.IsTrue(arAfter.IsCompleted, "after after: IsCompleted");
            readLen = action.EndInvoke(arAfter);
            Assert.AreEqual(0, readLen, "after after result: readLen");
            AssertIsOpen(conn, false, "bottom-bottom");
            //
            conn.Close();
            conn.Close();
            conn.Close();
        }

        [TestMethod]
        public void NetworkClose_WriteAfter()
        {
            BsTestNullApi api;
            var conn = CreateConnectedStream(out api);
            AssertIsOpen(conn, true, "top");
            conn.Flush();
            var buf = new byte[10];
            var action = (Action<Version>)delegate {
                conn.Write(buf, 0, buf.Length);
            };
            //var arList = new IAsyncResult[3];
            //for (int i = 0; i < arList.Length; ++i) {
            //    arList[i] = action.BeginInvoke(null, null, null);
            //}
            //Thread.Sleep(WeeWait);
            //for (int i = 0; i < arList.Length; ++i) {
            //    Assert.IsFalse(arList[i].IsCompleted, "before: IsCompleted [" + i + "]");
            //}
            AssertIsOpen(conn, true, "before");
            RaiseNetworkClose(conn, api); // ************
            // TODO This should be 'true' -- till an I/O op fails: AssertIsOpen(conn, true, "after");
            AssertIsOpen(conn, false, "after");
            //Thread.Sleep(WeeWait);
            //for (int i = 0; i < arList.Length; ++i) {
            //    Assert.IsTrue(arList[i].IsCompleted, "after: IsCompleted [" + i + "]");
            //}
            //for (int i = 0; i < arList.Length; ++i) {
            //    action.EndInvoke(arList[i]);
            //}
            //
            var arAfter = action.BeginInvoke(null, null, null);
            TestsApmUtils.SafeNoHangWaitShort(arAfter, "after after");
            Assert.IsTrue(arAfter.IsCompleted, "after after: IsCompleted");
            try {
                action.EndInvoke(arAfter);
                Assert.Fail("should have thrown!");
            } catch (IOException ex) {
                var ex2 = ex.InnerException;
                AssertAdapter.IsInstanceOfType(ex2, typeof(SocketException));
                var soex = (SocketException)ex2;
                Assert.AreEqual(SocketError.ConnectionAborted, soex.SocketErrorCode);
            }
            AssertIsOpen(conn, false, "bottom");
            // NetworkStream doesn't use Flush, so it doesn't throw on connection
            // close, thus we need to duplicate that. :-(
            conn.Flush();
            //
            conn.Close();
            conn.Close();
            conn.Close();
        }

    }
}
