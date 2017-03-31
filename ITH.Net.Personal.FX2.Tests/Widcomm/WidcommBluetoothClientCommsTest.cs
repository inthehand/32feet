using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Widcomm;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using InTheHand.Net.Sockets;
using System.Diagnostics;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public partial class WidcommBluetoothClientCommsTest
    {
        private static byte[] CanonicalOrderBytes(BluetoothAddress addr)
        {
            byte[] x = addr.ToByteArray();
            Array_Resize(ref x, 6);
            Array.Reverse(x);
            return x;
        }

        // no Array.Resize in NETCF
        static void Array_Resize<T>(ref T[] arr, int length)
        {
            if (length == arr.Length) {
                Debug.Fail("Don't call when not resizing!");
                return;
            }
            Debug.Assert(arr != null, "We don't handle the null input case.");
            T[] arrNew = new T[length];
            int overlap = Math.Min(arr.Length, length);
            Array.Copy(arr, arrNew, overlap);
            arr = arrNew;
        }

        //====
        class TestBtIf_StartDiscoverySyncCallback : IBtIf
        {
            WidcommBtInterface m_parent;
            int[] m_portsToReturn;
            bool m_asyncHdc;
            //
            bool m_startDiscovery_NeverFiresCompletion;
            bool m_startDiscovery_ReturnsFalse;
            bool m_startDiscovery_Throws;
            DISCOVERY_RESULT? m_GetLastDiscoveryResult_Result;
            bool m_GetLastDiscoveryResult_ReturnsWrongAddress;
            bool m_GetLastDiscoveryResult_Throws;
            bool m_ReadDiscoveryRecords_Throws;
            int? m_ExtendedError;
            //
            ManualResetEvent m_DiscoveryComplete = new ManualResetEvent(false);
            string m_unhandledException;


            public TestBtIf_StartDiscoverySyncCallback(bool startDiscovery_ReturnsFalse, bool startDiscovery_Throws, 
                DISCOVERY_RESULT? GetLastDiscoveryResult_Result, 
                bool GetLastDiscoveryResult_ReturnsWrongAddress, bool GetLastDiscoveryResult_Throws,
                bool ReadDiscoveryRecords_Throws, bool discovery_ManualDiscoveryComplete, params int[] portsToReturn)
                : this(portsToReturn)
            {
                m_startDiscovery_ReturnsFalse = startDiscovery_ReturnsFalse;
                if (m_startDiscovery_ReturnsFalse) {
                    m_ExtendedError = 0;
                }
                m_startDiscovery_Throws = startDiscovery_Throws;
                m_GetLastDiscoveryResult_Result = GetLastDiscoveryResult_Result;
                m_GetLastDiscoveryResult_ReturnsWrongAddress = GetLastDiscoveryResult_ReturnsWrongAddress;
                m_GetLastDiscoveryResult_Throws = GetLastDiscoveryResult_Throws;
                m_ReadDiscoveryRecords_Throws = ReadDiscoveryRecords_Throws;
                m_startDiscovery_NeverFiresCompletion = discovery_ManualDiscoveryComplete;
            }

            public TestBtIf_StartDiscoverySyncCallback(params int[] portsToReturn)
            {
                m_portsToReturn = portsToReturn;
            }

            public bool AsyncDiscovery
            {
                get { return m_asyncHdc; }
                set { m_asyncHdc = value; }
            }

            //----
            public void Assert_(string testName)
            {
                if (!m_startDiscovery_NeverFiresCompletion) {
                    WaitDiscoveryComplete();
                }
                if (m_unhandledException != null) {
                    //DEBUG
                }
                Assert.IsNull(m_unhandledException, m_unhandledException + " -- " + testName);
            }

            //----
            public void SetParent(WidcommBtInterface parent)
            {
                m_parent = parent;
            }

            public void Create()
            {
            }

            public void Destroy(bool disposing)
            {
            }

            //----
            public bool StartInquiry()
            {
                throw new NotImplementedException();
            }

            public void StopInquiry()
            {
                //TODO (REMOVED throw new NotImplementedException();)
            }

            //----
            const int GetLastDiscoveryResult_AlwaysReturns1 = 1;
            BluetoothAddress m_sdpDiscoveryAddress;

            public bool StartDiscovery(BluetoothAddress address, Guid serviceGuid)
            {
                if (m_startDiscovery_ReturnsFalse) {
                    m_DiscoveryComplete.Set();
                    return false;
                }
                if (m_startDiscovery_Throws) {
                    m_DiscoveryComplete.Set();
                    throw new RankException("TEST induced failure.");
                }
                m_sdpDiscoveryAddress = address;
                // Fire!
                if (!m_startDiscovery_NeverFiresCompletion) {
                    FireServiceDiscoverCompleteNow();
                }
                return true;
            }

            public void FireServiceDiscoverCompleteNow()
            {
                Debug.Assert(!m_DiscoveryComplete.WaitOne(0, false), "once please!!");
                object args = new ThreadStart(m_parent.HandleDiscoveryComplete);
                if (m_asyncHdc) {
                    ThreadPool.QueueUserWorkItem(Thread_Runner, args);
                } else {
                    Thread_Runner(args);
                }
            }

            void Thread_Runner(object state)
            {
                try {
                    ThreadStart dlgt = (ThreadStart)state;
                    dlgt();
                } catch (Exception ex) {
                    const string prefix = "Unhandled exception on widcomm thread: ";
                    Console.WriteLine("Unhandled exception on widcomm thread: " + ex);
                    //Trace.Assert(false, prefix + ex.ToString());
                    m_unhandledException = prefix + ex.Message;
                } finally {
                    m_DiscoveryComplete.Set();
                }
            }

            public void WaitDiscoveryComplete()
            {
                bool signalled = m_DiscoveryComplete.WaitOne(30 * 1000, false);
                Assert.IsTrue(signalled, "Test Timeout");
            }

            public DISCOVERY_RESULT GetLastDiscoveryResult(out BluetoothAddress address, out UInt16 p_num_recs)
            {
                if (m_GetLastDiscoveryResult_Result != null) {
                    address = new BluetoothAddress(-1);
                    p_num_recs = 0;
                    return m_GetLastDiscoveryResult_Result.Value;
                }
                if (m_GetLastDiscoveryResult_ReturnsWrongAddress) {
                    address = ToggleAddressBytes(m_sdpDiscoveryAddress);
                    p_num_recs = GetLastDiscoveryResult_AlwaysReturns1;
                    return DISCOVERY_RESULT.SUCCESS;
                }
                if (m_GetLastDiscoveryResult_Throws)
                    throw new RankException("TEST induced failure.");
                // SUCCESS
                Debug.Assert(m_sdpDiscoveryAddress != null, "GetLastDiscoveryResult null address return!");
                address = m_sdpDiscoveryAddress;
                p_num_recs = GetLastDiscoveryResult_AlwaysReturns1;
                return DISCOVERY_RESULT.SUCCESS;
            }

            private BluetoothAddress ToggleAddressBytes(BluetoothAddress address)
            {
                byte[] b = (byte[])address.ToByteArray().Clone();
                for (int i = 0; i < b.Length; ++i)
                    b[i] = (byte)(b[i] ^ 0xFF);
                BluetoothAddress addr2 = new BluetoothAddress(b);
                Debug.Assert(address != addr2, "NOT: " + address + " != " + addr2);
                return addr2;
            }

            public ISdpDiscoveryRecordsBuffer ReadDiscoveryRecords(BluetoothAddress address, int maxRecords, ServiceDiscoveryParams args)
            {
                if (m_ReadDiscoveryRecords_Throws)
                    throw new RankException("TEST induced failure.");
                return new TestISdpDiscoveryRecordsBuffer(m_portsToReturn);
            }

            //----------
            public int GetSupportsGetRemoteDeviceInfo()
            {
                throw new NotImplementedException();
            }

            public REM_DEV_INFO_RETURN_CODE GetRemoteDeviceInfo(ref REM_DEV_INFO remDevInfo, 
                IntPtr p_rem_dev_info, int cb)
            {
                throw new NotImplementedException();
            }

            public REM_DEV_INFO_RETURN_CODE GetNextRemoteDeviceInfo(ref REM_DEV_INFO remDevInfo, 
                IntPtr p_rem_dev_info, int cb)
            {
                throw new NotImplementedException();
            }

            //----
            public bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO devVerInfo)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool GetLocalDeviceName(byte[] bdName)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void IsDeviceConnectableDiscoverable(out bool conno, out bool disco)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool GetLocalDeviceInfoBdAddr(byte[] bdAddr)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public int GetRssi(byte[] bd_addr)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool BondQuery(byte[] bd_addr)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public BOND_RETURN_CODE Bond(BluetoothAddress address, string passphrase)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool UnBond(BluetoothAddress address)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public WBtRc GetExtendedError()
            {
                return (WBtRc)m_ExtendedError.Value;
            }

            public SDK_RETURN_CODE IsRemoteDevicePresent(byte[] bd_addr)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public bool IsRemoteDeviceConnected(byte[] bd_addr)
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }

            public void IsStackUpAndRadioReady(out bool stackServerUp, out bool deviceReady)
            {
                throw new NotImplementedException();
            }

            public void SetDeviceConnectableDiscoverable(bool connectable, bool pairedOnly, bool discoverable)
            {
                throw new NotImplementedException();
            }

        }

        class TestISdpDiscoveryRecordsBuffer : ISdpDiscoveryRecordsBuffer
        {
            int[] m_portsToReturn;
            bool m_disposed;

            public TestISdpDiscoveryRecordsBuffer(params int[] portsToReturn)
            {
                if (portsToReturn == null)
                    m_portsToReturn = new int[0];
                else
                    m_portsToReturn = portsToReturn;
            }

            //--------
            #region ISdpDiscoveryRecordsBuffer Members

            public int RecordCount
            {
                get { EnsureNonDisposed(); return m_portsToReturn.Length; }
            }

            public int[] Hack_GetPorts()
            {
                EnsureNonDisposed();
                return m_portsToReturn;
            }

            public int[] Hack_GetPsms()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public InTheHand.Net.Bluetooth.ServiceRecord[] GetServiceRecords()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                m_disposed = true;
            }

            void EnsureNonDisposed()
            {
                if (m_disposed)
                    throw new ObjectDisposedException("TestISdpDiscoveryRecordsBuffer");
            }
            #endregion
        }

        //----------------------
        private static void Create_BluetoothClient(out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient(null, out port, out cli, out strm2);
        }

        private static void Create_BluetoothClient(out TestRfCommIf rfCommIf,
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            Create_BluetoothClient__(null, out rfCommIf, out port, out cli, out strm2);
        }

        internal static void Create_BluetoothClient(IBtIf btIface,
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            TestRfCommIf rfCommIf;
            Create_BluetoothClient__(btIface, out rfCommIf, out port, out cli, out strm2);
        }

        private static void Create_BluetoothClient__(IBtIf btIf,
            out TestRfCommIf rfCommIf,
            out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            WidcommFactoryGivenInstances fcty = new WidcommFactoryGivenInstances();
            WidcommBtInterface btIface;
            if (btIf != null) {
                btIface = new WidcommBtInterface(btIf, fcty);
                fcty.SetBtInterface(btIface);
            } else {
                btIface = null;
            }
            rfCommIf = new TestRfCommIf ();
            port = new TestRfcommPort();
            WidcommRfcommStreamBase strm = new WidcommRfcommStream(port, rfCommIf, fcty);
            fcty.AddRfcommStream(strm);
            //
            WidcommBluetoothClient wcli = new WidcommBluetoothClient(fcty);
            cli = new BluetoothClient(wcli);
            strm2 = strm;
        }

        BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
            InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);

        private void Create_ConnectedBluetoothClient(out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            TestRfCommIf rfcommIf;
            Create_ConnectedBluetoothClient(out rfcommIf, out port, out cli, out strm2);
        }

        internal static void Create_ConnectedBluetoothClient(out TestRfCommIf rfcommIf, out TestRfcommPort port, out BluetoothClient cli, out Stream strm2)
        {
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumber = 5;
            //
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out rfcommIf, out port, out cli, out strm2);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Accept");
            Assert.IsTrue(ar.IsCompleted, "Connect 1 completed");
            cli.EndConnect(ar);
        }

        //--------------------------------------------------------------
        const PORT_EV EventClosed = PORT_EV.CONNECT_ERR; //=99;

        const int SocketError_IsConnected = 10056;
        const int SocketError_NotConnected = 10057;
        const int SocketError_NetworkDown = 10050;

        //--------------------------------------------------------------
        static IAsyncResult BeginConnect(WidcommRfcommStreamBase rfcommStream, 
            BluetoothEndPoint bep,
            AsyncCallback asyncCallback, Object state)
        {
            return rfcommStream.BeginConnect(bep, null, //false, false,
                asyncCallback, state);
        }

        [Test]
        public void Misc()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm2;
            //
            Create_BluetoothClient(out port, out cli, out strm2);
            Assert_StreamIsNonSeekable(strm2);
            //

            BluetoothAddress addrA = BluetoothAddress.Parse("00:11:22:33:44:55");
            WidcommRfcommStreamBase strmRfcomm = (WidcommRfcommStreamBase)strm2;
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap), null, null);
            } catch (ArgumentException) {
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, 0), null, null);
            } catch (ArgumentException) {
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, -1), null, null);
            } catch (ArgumentException) {
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, -10), null, null);
            } catch (ArgumentException) {
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, 31), null, null);
            } catch (ArgumentException) {
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, 100), null, null);
            } catch (ArgumentException) {
            }
            //
            AsyncResultNoResult arFake = new AsyncResultNoResult(null, null);
            try {
                strmRfcomm.EndConnect(arFake);
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex); // and not a sub-type
            }
            //
            Create_ConnectedBluetoothClient(out port, out cli, out strm2);
            Assert.IsTrue(cli.Connected, "cli.Connected open");
            Assert.IsTrue(strm2.CanRead, "strm2.CanRead open");
            Assert.IsTrue(strm2.CanWrite, "strm2.CanWrite open");
            cli.Close();
            Assert.IsFalse(cli.Connected, "cli.Connected closed");
            Assert.IsFalse(strm2.CanRead, "strm2.CanRead closed");
            Assert.IsFalse(strm2.CanWrite, "strm2.CanWrite closed");
            try {
                Stream strm = cli.GetStream();
            } catch (Exception) {
                // TODO Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex);
            }
            try {
                BeginConnect(strmRfcomm, new BluetoothEndPoint(addrA, BluetoothService.Wap, 5), null, null);
            } catch (ObjectDisposedException) {
            }
        }

        public static void Assert_StreamIsNonSeekable(Stream strm)
        {
            long y;
            try {
                y = strm.Length;
                Assert.Fail("should have thrown -- " + "Length");
            } catch (NotSupportedException) { }
            try {
                y = strm.Position;
                Assert.Fail("should have thrown -- " + "Position");
            } catch (NotSupportedException) { }
            try {
                strm.Position = 111;
                Assert.Fail("should have thrown -- " + "Position");
            } catch (NotSupportedException) { }
            try {
                strm.SetLength(111);
                Assert.Fail("should have thrown -- " + "SetLength");
            } catch (NotSupportedException) { }
            try {
                strm.Seek(111, SeekOrigin.Current);
                Assert.Fail("should have thrown -- " + "Seek");
            } catch (NotSupportedException) { }
        }

        public static void Assert_StreamIsTimeoutable(Stream strm)
        {
            Assert.IsTrue(strm.CanTimeout, "CanTimeout");
            int x;
            x = strm.ReadTimeout;
            x = strm.WriteTimeout;
            strm.ReadTimeout = 111;
            strm.WriteTimeout = 111;
        }

        public static void Assert_StreamIsNonWriteTimeoutable(Stream strm)
        {
            Assert.IsFalse(strm.CanTimeout, "CanTimeout");
            int x;
            //try {
            //    x = strm.ReadTimeout;
            //    Assert.Fail("should have thrown -- " + "get_ReadTimeout");
            //} catch (InvalidOperationException) { }
            try {
                x = strm.WriteTimeout;
                Assert.Fail("should have thrown -- " + "get_WriteTimeout");
            } catch (InvalidOperationException) { }
            //try {
            //    strm.ReadTimeout = 111;
            //    Assert.Fail("should have thrown -- " + "set_ReadTimeout");
            //} catch (InvalidOperationException) { }
            try {
                strm.WriteTimeout = 111;
                Assert.Fail("should have thrown -- " + "set_WriteTimeout");
            } catch (InvalidOperationException) { }
        }

        public static void Assert_StreamIsNonTimeoutable(Stream strm)
        {
            Assert.IsFalse(strm.CanTimeout, "CanTimeout");
            int x;
            try {
                x = strm.ReadTimeout;
                Assert.Fail("should have thrown -- " + "get_ReadTimeout");
            } catch (InvalidOperationException) { }
            try {
                x = strm.WriteTimeout;
                Assert.Fail("should have thrown -- " + "get_WriteTimeout");
            } catch (InvalidOperationException) { }
            try {
                strm.ReadTimeout = 111;
                Assert.Fail("should have thrown -- " + "set_ReadTimeout");
            } catch (InvalidOperationException) { }
            try {
                strm.WriteTimeout = 111;
                Assert.Fail("should have thrown -- " + "set_WriteTimeout");
            } catch (InvalidOperationException) { }
        }

        //--------------------------------------------------------------
        [Test]
        public void Connect_SameThread__IsThisObsolete_WeNeverUseSameThreadForCallbacksNow()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 1");
            Assert.IsTrue(ar.IsCompleted, "Connect 1 completed");
            cli.EndConnect(ar);
            //
            // Failure from OpenClient call.
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.ALREADY_OPENED);
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 2");
            } catch (Exception ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                //TODO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
            //
            // Failure from failure event.
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            port.NewEvent(PORT_EV.CONNECT_ERR);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 3");
            Assert.IsTrue(ar.IsCompleted, "Connect 3 completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown" + " @EndConnect 3");
            } catch (Exception ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                //TODO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
            //
            // Call BeginConnect twice without EndConnect in between!
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            Assert.IsFalse(ar.IsCompleted, "Connect 4a completed");
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 4b");
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
            //
            // Call BeginConnect twice without EndConnect in between, but first has completed!
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 5a");
            Assert.IsTrue(ar.IsCompleted, "Connect 5a completed");
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 5b");
            } catch (Exception ex) {
                //Exception ex = ex0.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                Assert.AreEqual(SocketError_IsConnected, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
            port.AssertOpenClientNotCalled();
            //
            // Connect twice!
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 6a");
            Assert.IsTrue(ar.IsCompleted, "Connect 6a completed");
            cli.EndConnect(ar);
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 6b");
            } catch (Exception ex) {
                //Exception ex = ex0.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                //Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
            port.AssertOpenClientNotCalled();
            //
            // Use wrong IAsyncResult
            IAsyncResult wrongAr = ar;
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.NewEvent(PORT_EV.CONNECTED);
            try {
                cli.EndConnect(wrongAr);
                Assert.Fail("should have thrown" + " @BeginConnect 7b");
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            } catch (Exception ex) {
                //Assert.IsInstanceOfType(typeof(SocketException), ex0.InnerException, "ex typeof SEX");
                Assert.IsInstanceOfType(typeof(SocketException), ex, "ex typeof SEX");
                SocketException sex = (SocketException)ex;
                Assert.AreEqual(SocketError_IsConnected, sex.ErrorCode, "SEX error code");
            }
        }

        private static void GetConnectData(out BluetoothEndPoint bep, out byte[] addressInWidcomm, out byte channelNumber)
        {
            bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);
            addressInWidcomm = CanonicalOrderBytes(bep.Address);
            channelNumber = 5;
        }

        [Test]
        public void Connect___Success()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            OneEvent100msFirer firer = new OneEvent100msFirer(port);
            firer.Run(PORT_EV.CONNECTED);
            Assert.IsFalse(ar.IsCompleted, "Connect 1 completed"); // 100ms later...
            cli.EndConnect(ar);
            firer.Complete(); // any exceptions?
            // Misc
            Assert.IsFalse(cli.Authenticate, ".Authenticate");
            Assert.IsFalse(cli.Encrypt, ".Encrypt");
        }

        [Test]
        public void Connect_CloseInternalAfterOpenClientButBeforeEvent()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            //
            ((WidcommRfcommStreamBase)strm).CloseInternalAndAbort_willLock();
            //
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect");
            Assert.IsTrue(ar.IsCompleted, "Connect completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown!");
            } catch (SocketException ex) {
                //Assert.AreEqual(SocketError.NetworkDown, ex.SocketErrorCode, "SocketErrorCode");
                Assert.AreEqual(SocketError_NetworkDown, ex.ErrorCode, "ErrorCode");
            }
        }

        [Test]
        public void Connect_DisposeAfterOpenClientButBeforeEvent()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            //
            cli.Close();
            //
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect");
            Assert.IsTrue(ar.IsCompleted, "Connect completed"); // 100ms later...
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown!");
            } catch (ObjectDisposedException) {
            }
        }

        [Test]
        public void Connect___Failure_from_OpenClient_call()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Failure from OpenClient call.
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.ALREADY_OPENED);
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 2");
            } catch (SocketException ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                //TODO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
        }

        [Test]
        public void Connect___Failure_from_failure_event()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Failure from failure event.
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            OneEvent100msFirer firer = new OneEvent100msFirer(port);
            firer.Run(PORT_EV.CONNECT_ERR); // 100ms later...
            Assert.IsFalse(ar.IsCompleted, "Connect 3 completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown" + " @EndConnect 3");
            } catch (Exception ex) {
                Assert.IsInstanceOfType(typeof(SocketException), ex);
                //TODO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            }
            firer.Complete(); // any exceptions?
        }

        [Test]
        public void Connect___Call_BeginConnect_twice_without_EndConnect_in_between()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep;
            byte[] AddressInWidcomm;
            byte ChannelNumber;
            GetConnectData(out bep, out AddressInWidcomm, out ChannelNumber);
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Call BeginConnect twice without EndConnect in between!
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            Assert.IsFalse(ar.IsCompleted, "Connect 4a completed");
            try {
                ar = cli.BeginConnect(bep, null, null);
                cli.EndConnect(ar); // Now BeginConnect calls through to OpenClient asynchronously!
                Assert.Fail("should have thrown" + " @BeginConnect 4b");
            } catch (InvalidOperationException ex) {
                Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            }
            //
            //TODO ?// Call BeginConnect twice without EndConnect in between, but first has completed!
            //Create_BluetoothClient(out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            //ar = cli.BeginConnect(bep, null, null);
            //port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            //port.NewEvent(PORT_EV.CONNECTED);
            //Assert.IsTrue(ar.IsCompleted, "Connect 5a completed");
            //try {
            //    ar = cli.BeginConnect(bep, null, null);
            //    Assert.Fail("should have thrown" + " @BeginConnect 5b");
            //} catch (SocketException ex) {
            //    Assert.IsInstanceOfType(typeof(SocketException), ex);
            //    Assert.AreEqual(SocketError_IsConnected, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            //}
            //port.AssertOpenClientNotCalled();
            ////
            //// Connect twice!
            //Create_BluetoothClient(out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            //ar = cli.BeginConnect(bep, null, null);
            //port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5);
            //port.NewEvent(PORT_EV.CONNECTED);
            //Assert.IsTrue(ar.IsCompleted, "Connect 6a completed");
            //cli.EndConnect(ar);
            //try {
            //    ar = cli.BeginConnect(bep, null, null);
            //    Assert.Fail("should have thrown" + " @BeginConnect 6b");
            //} catch (SocketException ex) {
            //    Assert.IsInstanceOfType(typeof(SocketException), ex);
            //    //Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
            //}
            //port.AssertOpenClientNotCalled();
            ////
            //// Use wrong IAsyncResult
            //IAsyncResult wrongAr = ar;
            //Create_BluetoothClient(out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            //ar = cli.BeginConnect(bep, null, null);
            //port.NewEvent(PORT_EV.CONNECTED);
            //try {
            //    cli.EndConnect(wrongAr);
            //    Assert.Fail("should have thrown" + " @BeginConnect 7b");
            //} catch (InvalidOperationException ex) {
            //    Assert.IsInstanceOfType(typeof(InvalidOperationException), ex);
            //}
        }

        [Test]
        public void Connect_NotAuthNotEncrypt()
        {
            Connect_AuthEncrypt_(false, false, BTM_SEC.NONE);
        }
        [Test]
        [Explicit]
        public void Connect_NotAuthEncrypt()
        {
            Connect_AuthEncrypt_(false, true, BTM_SEC.OUT_ENCRYPT | BTM_SEC.OUT_AUTHENTICATE);
        }
        [Test]
        [Explicit]
        public void Connect_AuthNotEncrypt()
        {
            Connect_AuthEncrypt_(true, false, BTM_SEC.OUT_AUTHENTICATE);
        }
        [Test]
        [Explicit]
        public void Connect_AuthEncrypt()
        {
            Connect_AuthEncrypt_(true, true, BTM_SEC.OUT_ENCRYPT | BTM_SEC.OUT_AUTHENTICATE);
        }

        void Connect_AuthEncrypt_(bool auth, bool encrypt, BTM_SEC expectedSecurityLevel)
        {
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumber = 5;
            //
            //
            TestRfCommIf rfCommIf;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out rfCommIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            // **
            //if (auth)
            //    cli.Authenticate = true;
            //if (encrypt)
            //    cli.Encrypt = true;
            Assert.AreEqual(auth, cli.Authenticate, ".Encrypt");
            Assert.AreEqual(encrypt, cli.Encrypt, ".Encrypt");
            //
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            rfCommIf.AssertSetSecurityLevel(expectedSecurityLevel, false);  // ****
            OneEvent100msFirer firer = new OneEvent100msFirer(port);
            firer.Run(PORT_EV.CONNECTED);
            Assert.IsFalse(ar.IsCompleted, "Connect 1 completed"); // 100ms later...
            cli.EndConnect(ar);
            firer.Complete(); // any exceptions?
            //
            //Assert.AreEqual(auth, cli.Authenticate, ".Authenticate End");
            //Assert.AreEqual(encrypt, cli.Encrypt, ".Encrypt End");
        }

        [Test]
        public void ConnectPortLookup_TestSuccessAndAllPossibleFailures_Async()
        {
            ConnectPortLookup_TestSuccessAndAllPossibleFailures_(true);
        }

        [Test]
        public void ConnectPortLookup_TestSuccessAndAllPossibleFailures_Sync()
        {
            ConnectPortLookup_TestSuccessAndAllPossibleFailures_(false);
        }

        private void ConnectPortLookup_TestSuccessAndAllPossibleFailures_(bool @async)
        {
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Wap);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumberLookedUp = 7;
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            TestBtIf_StartDiscoverySyncCallback btIf;
            //
            // Success
            btIf = new TestBtIf_StartDiscoverySyncCallback(ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 1 completed -- before event");
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 1");
            Assert.IsTrue(ar.IsCompleted, "Connect 1 completed");
            cli.EndConnect(ar);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumberLookedUp);
            btIf.Assert_("Connect 1");
            //
            // Multiple rfcomm records, use the most recent...
            btIf = new TestBtIf_StartDiscoverySyncCallback(new int[] { 1, 2, 3, 4, 5 });
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 2 completed -- before event");
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 2");
            Assert.IsTrue(ar.IsCompleted, "Connect 2 completed");
            cli.EndConnect(ar);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, 5 /* the last record's port is used */);
            btIf.Assert_("Connect 2");
            //
            //--------
            // Zero records
            btIf = new TestBtIf_StartDiscoverySyncCallback(new int[0]);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, false, "Zero records");
            //
            // Multiple records (zero ports e.g. not rfcomm records)
            btIf = new TestBtIf_StartDiscoverySyncCallback(new int[] { -1, -1 });
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, false, "No RFCOMM Ports");
            //
            // SD returns false
            btIf = new TestBtIf_StartDiscoverySyncCallback(true, false, null, false, false, false, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, false, "SD returns false");
            //
            // SD throws
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, true, null, false, false, false, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, true, "SD throws");
            //
            // GLDR returns error
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, DISCOVERY_RESULT.CONNECT_ERR, false, false, false, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, false, "GLDR returns error");
            //
            // GLDR returns different address
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, true, false, false, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, true, "GLDR returns different address");
            //
            // GLDR throws
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, false, true, false, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, true, "GLDR throws");
            //
            // RDR throws
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, false, false, true, false);
            btIf.AsyncDiscovery = @async;
            Assert_ConnectFailsBeforeOpenClient(bep, btIf, true, "RDR throws");
            //----
            // IsConnected throws
            btIf = new TestBtIf_StartDiscoverySyncCallback(ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            port.SetIsConnectedThrows();
            ar = cli.BeginConnect(bep, null, null);
            port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 4 completed -- before event");
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 4");
            Assert.IsTrue(ar.IsCompleted, "Connect 4 completed");
            cli.EndConnect(ar);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumberLookedUp);
            //
            // IsConnected returns false
            btIf = new TestBtIf_StartDiscoverySyncCallback(ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            port.SetIsConnectedReturnsFalse();
            ar = cli.BeginConnect(bep, null, null);
            port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 3 completed -- before event");
            port.NewEvent(PORT_EV.CONNECTED);
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 3");
            Assert.IsTrue(ar.IsCompleted, "Connect 3 completed");
            cli.EndConnect(ar);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumberLookedUp);
        }

        private static void Assert_ConnectFailsBeforeOpenClient(BluetoothEndPoint bep, TestBtIf_StartDiscoverySyncCallback btIf, bool nonSocketException, string testName)
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            //
            IAsyncResult ar;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            try {
                ar = cli.BeginConnect(bep, null, null);
                btIf.Assert_(testName);
            } catch (SocketException sex) {
                btIf.Assert_(testName);
                Assert.IsFalse(nonSocketException, "Expected nonSocketException" + sex.Message + " -- " + testName);
                //Assert.IsInstanceOfType(typeof(SocketException), sex, "BeginConnect Exception Type -- " + testName);
                return;
            } catch (Exception ex) {
                btIf.Assert_(testName);
                //Debug.Fail(ex.ToString());
                Assert.IsTrue(nonSocketException, "Unexpected nonSocketException: " + ex.Message + " -- " + testName);
                return;
            }
            Assert.IsNotNull(ar, "ar isNull -- " + testName);
            bool signalled = ar.AsyncWaitHandle.WaitOne(100, false);
            Assert.IsTrue(signalled, "Connect signalled -- " + testName);
            Assert.IsTrue(ar.IsCompleted, "Connect completed -- " + testName);
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown" + " @EndConnect -- " + testName);
            } catch (SocketException sex) {
                Assert.IsFalse(nonSocketException, "Expected nonSocketException" + sex.Message + " -- " + testName);
                // Assert.IsInstanceOfType(typeof(SocketException), sex, "EndConnect Exception Type -- " + testName);
            } catch (Exception ex) {
                Assert.IsTrue(nonSocketException, "Unexpected nonSocketException: " + ex.Message + " -- " + testName);
                return;
            }
            port.AssertOpenClientNotCalled();
        }


        [Test]
        public void Connect_DisposeDuringPortLookup_Async()
        {
            Connect_DisposeDuringPortLookup_(true);
        }

        [Test]
        public void Connect_DisposeDuringPortLookup_Sync()
        {
            Connect_DisposeDuringPortLookup_(false);
        }

        private void Connect_DisposeDuringPortLookup_(bool @async)
        {
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Wap);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumberLookedUp = 7;
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            TestBtIf_StartDiscoverySyncCallback btIf;
            //
            // Aborted (and SDP never completes)
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, false, false, false,
                true, ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            //port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 1 completed -- before event");
            //port.NewEvent(PORT_EV.CONNECTED);
            Thread.Sleep(50);
            cli.Close();
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 1");
            Assert.IsTrue(ar.IsCompleted, "Connect 1 completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown!");
            } catch {
            }
            port.AssertOpenClientNotCalled();
            btIf.Assert_("Connect 1");
            //
            // Aborted and SDP then completes
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, false, false, false,
                true, ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            //port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 2 completed -- before event");
            //port.NewEvent(PORT_EV.CONNECTED);
            Thread.Sleep(50);
            cli.Close();
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 2");
            Assert.IsTrue(ar.IsCompleted, "Connect 2 completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown!");
            } catch {
            }
            btIf.FireServiceDiscoverCompleteNow();
            btIf.WaitDiscoveryComplete();
            port.AssertOpenClientNotCalled();
            btIf.Assert_("Connect 2");
            //----------
            // Aborted and SDP then completes *incorrectly*
            btIf = new TestBtIf_StartDiscoverySyncCallback(false, false, null, false, true, false,
                true, ChannelNumberLookedUp);
            btIf.AsyncDiscovery = @async;
            Create_BluetoothClient(btIf, out port, out cli, out strm);
            //port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            //port.WaitOpenClientCalled();
            Assert.IsFalse(ar.IsCompleted, "Connect 3 completed -- before event");
            //port.NewEvent(PORT_EV.CONNECTED);
            Thread.Sleep(50);
            cli.Close();
            TestsApmUtils.SafeNoHangWaitShort(ar, "Connect 3");
            Assert.IsTrue(ar.IsCompleted, "Connect 3 completed");
            try {
                cli.EndConnect(ar);
                Assert.Fail("should have thrown!");
            } catch {
            }
            btIf.FireServiceDiscoverCompleteNow();
            btIf.WaitDiscoveryComplete();
            port.AssertOpenClientNotCalled();
            btIf.Assert_("Connect 3");
        }


        [Test]
        public void Connect_PeerImmediatelyCloses()
        {
            //CONNECTED    = 0x00000200,  /* RFCOMM connection established */
            //CONNECT_ERR  = 0x00008000,  /* Was not able to establish connection */
            //
            BluetoothEndPoint bep = new BluetoothEndPoint(BluetoothAddress.Parse("00:1F:2E:3D:4C:5B"),
                InTheHand.Net.Bluetooth.BluetoothService.Empty, 5);
            byte[] AddressInWidcomm = CanonicalOrderBytes(bep.Address);
            const int ChannelNumber = 5;
            //
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            IAsyncResult ar;
            //
            // Success
            Create_BluetoothClient(out port, out cli, out strm);
            port.SetOpenClientResult(PORT_RETURN_CODE.SUCCESS);
            ar = cli.BeginConnect(bep, null, null);
            port.AssertOpenClientCalledAndClear(AddressInWidcomm, ChannelNumber);
            FireOpenReceiveCloseEvents firer = new FireOpenReceiveCloseEvents(port);
            firer.Run();
            //Assert.IsFalse(ar.IsCompleted, "Connect 1 completed"); // 100ms later...
            firer.Complete(); // any exceptions?
            cli.EndConnect(ar);
            // Misc
            Assert.IsFalse(cli.Authenticate, ".Authenticate");
            Assert.IsFalse(cli.Encrypt, ".Encrypt");
            Stream peer = cli.GetStream();
            byte[] buf = new byte[10];
            int readLen = TestsApmUtils.SafeNoHangRead(peer, buf, 0, buf.Length);
            Assert.AreEqual(1, readLen, "readLen");
            peer.Close();
        }

        //--------------------------------------------------------------
        [Test]
        public void Close_Stream() { CloseX(true, false); }

        [Test]
        public void Close_Client() { CloseX(false, true); }

        [Test]
        public void Close_Both() { CloseX(true, true); }

        [Test]
        public void Close_Neither_SoFinalize() { CloseX(false, false); }

        void CloseX(bool closeStream, bool closeClient)
        {
            CloseX(closeStream, closeClient, null);
        }

        void CloseX(bool closeStream, bool closeClient, LingerOption lingerOption)
        {
            TestRfcommPort port;
            TestRfCommIf commIf;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out commIf, out port, out cli, out strm);
            if (lingerOption != null) {
                cli.LingerState = lingerOption;
            }
            Assert.IsTrue(cli.Connected, "isConnected 1");
            Assert.IsTrue(strm.CanRead, "CanRead 1");
            Assert.IsTrue(strm.CanWrite, "CanWrite 1");
            Assert.IsFalse(strm.CanSeek, "CanSeek 1");
            if (closeStream) {
                strm.Close(); //***
                port.AssertCloseCalledOnce("after Stream.Close");
                commIf.AssertDestroyCalledOnce();
            }
            if (closeClient) {
                cli.Close(); //***
                port.AssertCloseCalledOnce("after Client.Close");
                commIf.AssertDestroyCalledOnce();
            }
            if (!closeStream && !closeClient) {
                cli = null;
                strm = null;
                port.TestHackClearParentStream();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                port.AssertCloseCalledOnce("after finalizer");
                commIf.AssertDestroyCalledOnce();
            }
            //
            // Check the objects' state, but can't when they're null'd for Finalize test!
            if (closeStream || closeClient) {
                Assert.IsFalse(cli.Connected, "Connected 2");
                Assert.IsFalse(strm.CanRead, "CanRead 2");
                Assert.IsFalse(strm.CanWrite, "CanWrite 2");
                Assert.IsFalse(strm.CanSeek, "CanSeek 2");
                IAsyncResult ar;
                try {
                    ar = strm.BeginWrite(new byte[] { 1, 2, 3 }, 0, 3, null, null);
                    Assert.Fail("should have thrown" + " @BeginWrite");
                } catch (IOException ioex) {
                    Exception ex = ioex.InnerException;
                    Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex);
                    //TO-DO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
                }
                try {
                    byte[] buf = new byte[3];
                    ar = strm.BeginRead(buf, 0, buf.Length, null, null);
                    Assert.Fail("should have thrown" + " @BeginRead");
                } catch (IOException ioex) {
                    Exception ex = ioex.InnerException;
                    Assert.IsInstanceOfType(typeof(ObjectDisposedException), ex);
                    //TO-DO Assert.AreEqual(99, ((SocketException)ex).ErrorCode, "ErrorCode" + " @BeginConnect 2");
                }
            }
            //
            cli = null;
            strm = null;
            port.TestHackClearParentStream();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            port.AssertCloseCalledOnce("after 'second' finalizer");
            commIf.AssertDestroyCalledOnce();
        }

        private void SendEvent(TestRfcommPort port, PORT_EV eventId)
        {
            port.NewEvent(eventId);
        }

        [Test]
        public void PeerClose_PeerCloseWrite()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            //----
            byte[] buf1;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            SendEvent(port, PORT_EV.CONNECT_ERR);
            buf1 = new byte[10];
            try {
                strm.Write(buf1, 0, buf1.Length);
                Assert.Fail("should have thrown -- " + "Write");
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void PeerClose_PeerCloseBeginWrite()
        {
            TestRfCommIf commIf;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            //----
            byte[] buf1;
            Create_ConnectedBluetoothClient(out commIf, out port, out cli, out strm);
            SendEvent(port, PORT_EV.CONNECT_ERR);
            port.AssertCloseCalledOnce("after Peer close i.e. CONNECT_ERR");
            buf1 = new byte[10];
            try {
                IAsyncResult arR1 = strm.BeginWrite(buf1, 0, buf1.Length, null, null);
                Assert.Fail("should have thrown -- " + "BeginWrite");
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
            //
            strm.Close();
            port.AssertCloseCalledAtLeastOnce("after Stream.Close");
            commIf.AssertDestroyCalledOnce();
        }

        [Test]
        public void PeerClose_BeginReadOnePeerClose()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            //----
            byte[] buf1;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            buf1 = new byte[10];
            IAsyncResult arR1 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            Assert.IsFalse(arR1.IsCompleted, "before R1 !completed");
            SendEvent(port, PORT_EV.CONNECT_ERR);
            Assert.IsTrue(arR1.IsCompleted, "after R1 !completed");
            int readLen = strm.EndRead(arR1);
            Assert.AreEqual(0, readLen, "readLen");
            //
            strm.Close();
        }

        [Test]
        public void PeerClose_BeginReadMultiplePeerClose()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            byte[] buf1, buf2, buf3, buf4;
            //----
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            buf1 = new byte[10];
            IAsyncResult arR1 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            buf2 = new byte[10];
            IAsyncResult arR2 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            buf3 = new byte[10];
            IAsyncResult arR3 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            buf4 = new byte[10];
            IAsyncResult arR4 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            Assert.IsFalse(arR1.IsCompleted, "before R1 !completed");
            Assert.IsFalse(arR2.IsCompleted, "before R2 !completed");
            Assert.IsFalse(arR3.IsCompleted, "before R3 !completed");
            Assert.IsFalse(arR4.IsCompleted, "before R4 !completed");
            //
            SendEvent(port, PORT_EV.CONNECT_ERR);// ****
            Assert.IsTrue(arR1.IsCompleted, "after R1 !completed");
            Assert.IsTrue(arR2.IsCompleted, "after R2 !completed");
            Assert.IsTrue(arR3.IsCompleted, "after R3 !completed");
            Assert.IsTrue(arR4.IsCompleted, "after R4 !completed");
            Assert.AreEqual(0, strm.EndRead(arR1), "readLen 1");
            Assert.AreEqual(0, strm.EndRead(arR2), "readLen 2");
            Assert.AreEqual(0, strm.EndRead(arR3), "readLen 3");
            Assert.AreEqual(0, strm.EndRead(arR4), "readLen 4");
            //
            strm.Close();
        }

        //--------------------------------------------------------------
        [Test]
        public void ReadsQueuedAtCloseFromPeer()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1, ar2, ar3;
            ReadsQueuedAtClose_Init(out port, out strm, out ar1, out ar2, out ar3);
            port.NewEvent(PORT_EV.CONNECT_ERR); //****
            ReadsQueuedAtClose_Finish(strm, ar1, ar2, ar3);
        }

        [Test]
        public void ReadsQueuedAtCloseLocal()
        {
            TestRfcommPort port;
            Stream strm;
            IAsyncResult ar1, ar2, ar3;
            ReadsQueuedAtClose_Init(out port, out strm, out ar1, out ar2, out ar3);
            strm.Close(); //****
            ReadsQueuedAtClose_Finish(strm, ar1, ar2, ar3);
        }

        private void ReadsQueuedAtClose_Init(out TestRfcommPort port, out Stream strm, out IAsyncResult ar1, out IAsyncResult ar2, out IAsyncResult ar3)
        {
            BluetoothClient cli;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            byte[] buf1 = new byte[10];
            byte[] buf2 = new byte[10];
            byte[] buf3 = new byte[10];
            ar1 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            ar2 = strm.BeginRead(buf2, 0, buf2.Length, null, null);
            ar3 = strm.BeginRead(buf3, 0, buf3.Length, null, null);
            Assert.IsFalse(ar1.IsCompleted, "IsC 1 before");
            Assert.IsFalse(ar2.IsCompleted, "IsC 2 before");
            Assert.IsFalse(ar3.IsCompleted, "IsC 3 before");
        }

        private static void ReadsQueuedAtClose_Finish(Stream strm, IAsyncResult ar1, IAsyncResult ar2, IAsyncResult ar3)
        {
            Assert.IsTrue(ar1.IsCompleted, "IsC 1 after");
            Assert.IsTrue(ar2.IsCompleted, "IsC 2 after");
            Assert.IsTrue(ar3.IsCompleted, "IsC 3 after");
            int readLen;
            readLen = strm.EndRead(ar1);
            Assert.AreEqual(0, readLen, "readLen 1");
            readLen = strm.EndRead(ar2);
            Assert.AreEqual(0, readLen, "readLen 2");
            readLen = strm.EndRead(ar3);
            Assert.AreEqual(0, readLen, "readLen 3");
            //
            strm.Close();
        }

        [Test]
        public void MultipleBeginReadOutstandingThenGetDataOneChunk()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            int len = dataB9.Length / 3;
            Assert.AreEqual(dataB9.Length, 3 * len, "quotient!");
            byte[] buf1 = new byte[len];
            byte[] buf2 = new byte[len];
            byte[] buf3 = new byte[len];
            IAsyncResult ar1 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            IAsyncResult ar2 = strm.BeginRead(buf2, 0, buf2.Length, null, null);
            IAsyncResult ar3 = strm.BeginRead(buf3, 0, buf3.Length, null, null);
            Assert.IsFalse(ar1.IsCompleted, "IsC 1 before");
            Assert.IsFalse(ar2.IsCompleted, "IsC 2 before");
            Assert.IsFalse(ar3.IsCompleted, "IsC 3 before");
            port.NewReceive(dataB9); //****
            Assert.IsTrue(ar1.IsCompleted, "IsC 1 after");
            Assert.IsTrue(ar2.IsCompleted, "IsC 2 after");
            Assert.IsTrue(ar3.IsCompleted, "IsC 3 after");
            int readLen;
            readLen = strm.EndRead(ar1);
            Assert.AreEqual(buf1.Length, readLen, "readLen 1");
            readLen = strm.EndRead(ar2);
            Assert.AreEqual(buf2.Length, readLen, "readLen 2");
            readLen = strm.EndRead(ar3);
            Assert.AreEqual(buf3.Length, readLen, "readLen 3");
        }

        [Test]
        public void MultipleBeginReadOutstandingThenGetDataOneChunkPerRead()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            byte[] buf1 = new byte[dataA.Length];
            byte[] buf2 = new byte[dataA.Length];
            byte[] buf3 = new byte[dataA.Length];
            IAsyncResult ar1 = strm.BeginRead(buf1, 0, buf1.Length, null, null);
            IAsyncResult ar2 = strm.BeginRead(buf2, 0, buf2.Length, null, null);
            IAsyncResult ar3 = strm.BeginRead(buf3, 0, buf3.Length, null, null);
            Assert.IsFalse(ar1.IsCompleted, "IsC 1 before");
            Assert.IsFalse(ar2.IsCompleted, "IsC 2 before");
            Assert.IsFalse(ar3.IsCompleted, "IsC 3 before");
            port.NewReceive(dataA); //****
            port.NewReceive(dataA); //****
            port.NewReceive(dataA); //****
            Assert.IsTrue(ar1.IsCompleted, "IsC 1 after");
            Assert.IsTrue(ar2.IsCompleted, "IsC 2 after");
            Assert.IsTrue(ar3.IsCompleted, "IsC 3 after");
            int readLen;
            readLen = strm.EndRead(ar1);
            Assert.AreEqual(buf1.Length, readLen, "readLen 1");
            readLen = strm.EndRead(ar2);
            Assert.AreEqual(buf2.Length, readLen, "readLen 2");
            readLen = strm.EndRead(ar3);
            Assert.AreEqual(buf3.Length, readLen, "readLen 3");
        }

        //--------------------------------------------------------------
        [Test]
        public void ReceiveBeforehand_BigTargetBuffer()
        {
            ReceiveBeforehand_BigTargetBuffer_withOffset(0);
        }

        [Test]
        public void ReceiveBeforehand_BigTargetBuffer_offset()
        {
            ReceiveBeforehand_BigTargetBuffer_withOffset(10);
        }

        public void ReceiveBeforehand_BigTargetBuffer_withOffset(int theOffset)
        {
            TestRfcommPort port;
            Stream strm;
            BluetoothClient cli;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            port.NewReceive(dataA);
            port.NewReceive(dataA);
            port.NewReceive(dataA);
            port.NewEvent(EventClosed);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_0");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "0");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_1");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "1");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_2");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "2");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(0, readLen, "end");
        }

        [Test]
        public void ReceiveBeforehand_SmallTargetBuffer()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            port.NewReceive(dataA);
            port.NewReceive(dataA);
            port.NewReceive(dataA);
            port.NewEvent(EventClosed);
            //
            Assert.AreEqual(3 * dataA.Length, cli.Available, ".Available");
            Assert.IsTrue(((WidcommRfcommStreamBase)strm).DataAvailable, ".Available");
            int readLen;
            byte[] bufSmall = new byte[3];
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_0a");
            Assert.AreEqual(-3 + 3 * dataA.Length, cli.Available, ".Available");
            var expectedE = (IEnumerator<byte[]>)dataAtimes3In3ByteBufsNoCoalesce.GetEnumerator();
            Assert.IsTrue(expectedE.MoveNext(), "readLen_1 MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "0 content");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_0b");
            Assert.IsTrue(expectedE.MoveNext(), "readLen_0b MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "0b content");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_1a");
            Assert.IsTrue(expectedE.MoveNext(), "readLen_1a MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "1a content");
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_1b");
            Assert.IsTrue(expectedE.MoveNext(), "readLen_1b MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "1b content");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_2a");
            Assert.IsTrue(expectedE.MoveNext(), "readLen_2a MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "2a content");
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_2b");
            Assert.IsTrue(expectedE.MoveNext(), "readLen_2b MN");
            Assert_AreEqual_Buffers(expectedE.Current, bufSmall, 0, readLen, "2b content");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(0, readLen, "end");
            Assert.AreEqual(0, cli.Available, ".Available @end");
            Assert.IsFalse(((WidcommRfcommStreamBase)strm).DataAvailable, ".DataAvailable @end");
        }

        [Test]
        public void BeginReadArriveAfterBegin()
        {
            BeginReadArriveAfterBegin_withOffset(0);
        }
        [Test]
        public void BeginReadArriveAfterBegin_offset20()
        {
            BeginReadArriveAfterBegin_withOffset(20);
        }
        public void BeginReadArriveAfterBegin_withOffset(int theOffset)
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            IAsyncResult ar = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, null, null);
            Assert.IsFalse(ar.IsCompleted, "IsCompleted before");
            port.NewReceive(dataA); // **
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after");
            readLen = strm.EndRead(ar);
            Assert.AreEqual(dataA.Length, readLen, "readLen_0");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "0");
        }

        [Test]
        public void BeginReadArriveAfterBegin_GetClosed()
        {
            int theOffset = 0;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            IAsyncResult ar = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, null, null);
            Assert.IsFalse(ar.IsCompleted, "IsCompleted before");
            port.NewEvent(EventClosed);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after");
            readLen = strm.EndRead(ar);
            Assert.AreEqual(0, readLen, "readLen_0 -> 0==closed");
        }

        [Test]
        public void BeginReadArriveAfterBeginCallback_GetClosed()
        {
            int theOffset = 0;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            Exception callbackResult = null;
            ManualResetEvent complete = new ManualResetEvent(false);
            AsyncCallback cb = delegate(IAsyncResult ar) {
                try {
                    Assert.IsTrue(ar.IsCompleted, "IsCompleted after");
                    readLen = strm.EndRead(ar);
                    Assert.AreEqual(0, readLen, "readLen_0 -> 0==closed");
                } catch (Exception ex) {
                    callbackResult = ex;
                } finally {
                    complete.Set();
                }
            };
            IAsyncResult ar0 = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, cb, null);
            Assert.IsFalse(ar0.IsCompleted, "IsCompleted before");
            port.NewEvent(EventClosed);
            bool completed = complete.WaitOne(TestsUtils.TimespanToMilliseconds(new TimeSpan(0, 0, 10)), false);
            Assert.IsTrue(completed, "callback not called/completed!");
            if (callbackResult != null)
                throw callbackResult;
        }

        //--------------------------------------------------------------
        [Test]
        public void ReceiveAfter_BigTargetBuffer()
        {
            ReceiveAfter_BigTargetBuffer_withOffset(0);
        }

        [Test]
        public void ReceiveAfter_BigTargetBuffer_offset()
        {
            ReceiveAfter_BigTargetBuffer_withOffset(10);
        }

        public void ReceiveAfter_BigTargetBuffer_withOffset(int theOffset)
        {
            TestRfcommPort port;
            Stream strm;
            BluetoothClient cli;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            ReceiveAfter_FireAfterEvents(port);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_0");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "0");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_1");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "1");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(dataA.Length, readLen, "readLen_2");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "2");
            readLen = strm.Read(bufBig, theOffset, bufBig.Length - theOffset);
            Assert.AreEqual(0, readLen, "end");
        }

        private void ReceiveAfter_FireAfterEvents(TestRfcommPort port)
        {
            const int DelayBetweenEvents = 50;
            ThreadPool.QueueUserWorkItem(delegate {
                Thread.Sleep(DelayBetweenEvents);
                port.NewReceive(dataA);
                Thread.Sleep(DelayBetweenEvents);
                port.NewReceive(dataA);
                Thread.Sleep(DelayBetweenEvents);
                port.NewReceive(dataA);
                Thread.Sleep(DelayBetweenEvents);
                port.NewEvent(EventClosed);
            });
        }

        [Test]
        public void ReceiveAfter_SmallTargetBuffer()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            ReceiveAfter_FireAfterEvents(port);
            //
            int readLen;
            byte[] bufSmall = new byte[3];
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_0a");
            //Assert_AreEqual_Buffers(dataA, bufSmall, readLen, "0");
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_0b");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_1a");
            //Assert_AreEqual_Buffers(dataA, bufSmall, readLen, "1");
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_0b");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(3, readLen, "readLen_2a");
            //Assert_AreEqual_Buffers(dataA, bufSmall, readLen, "2");
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(1, readLen, "readLen_2b");
            //
            readLen = strm.Read(bufSmall, 0, bufSmall.Length);
            Assert.AreEqual(0, readLen, "end");
            Assert.AreEqual(0, cli.Available, ".Available @end");
            Assert.IsFalse(((WidcommRfcommStreamBase)strm).DataAvailable, ".DataAvailable @end");
        }

        [Test]
        public void BeginReadArriveBeforeBegin()
        {
            BeginReadArriveBeforeBegin_withOffset(0);
        }
        [Test]
        public void BeginReadArriveBeforeBegin_offset20()
        {
            BeginReadArriveBeforeBegin_withOffset(20);
        }
        public void BeginReadArriveBeforeBegin_withOffset(int theOffset)
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            port.NewReceive(dataA); // **
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            IAsyncResult ar = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, null, null);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted after");
            readLen = strm.EndRead(ar);
            Assert.AreEqual(dataA.Length, readLen, "readLen_0");
            Assert_AreEqual_Buffers(dataA, bufBig, theOffset, readLen, "0");
        }

        [Test]
        public void BeginReadArriveBeforeBegin_GetClosed()
        {
            int theOffset = 0;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            port.NewEvent(EventClosed);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            IAsyncResult ar = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, null, null);
            Assert.IsTrue(ar.IsCompleted, "IsCompleted");
            readLen = strm.EndRead(ar);
            Assert.AreEqual(0, readLen, "readLen_0 -> 0==closed");
        }

        [Test]
        public void BeginReadArriveBeforeBeginCallback_GetClosed()
        {
            int theOffset = 0;
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            port.NewEvent(EventClosed);
            //
            int readLen;
            byte[] bufBig = new byte[2 * UInt16.MaxValue];
            //
            Exception callbackResult = null;
            ManualResetEvent complete = new ManualResetEvent(false);
            AsyncCallback cb = delegate(IAsyncResult ar) {
                try {
                    Assert.IsTrue(ar.IsCompleted, "IsCompleted after");
                    readLen = strm.EndRead(ar);
                    Assert.AreEqual(0, readLen, "readLen_0 -> 0==closed");
                } catch (Exception ex) {
                    callbackResult = ex;
                } finally {
                    complete.Set();
                }
            };
            IAsyncResult ar0 = strm.BeginRead(bufBig, theOffset, bufBig.Length - theOffset, cb, null);
            Assert.IsTrue(ar0.IsCompleted, "IsCompleted");
            bool completed = complete.WaitOne(TestsUtils.TimespanToMilliseconds(new TimeSpan(0, 0, 10)), false);
            Assert.IsTrue(completed, "callback not called/completed!");
            if (callbackResult != null)
                throw callbackResult;
        }

        //--------------------------------------------------------------

        [Test]
        public void Write()
        {
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            // Short, zero offset.
            strm.Write(dataA, 0, dataA.Length);
            port.AssertWrittenContentAndClear("A", dataA);
            port.ClearWrittenContent();
            // Short, non-zero offset.
            strm.Write(dataA_offset5, dataA_offset5_Offset, dataA.Length);
            port.AssertWrittenContentAndClear("A_offset5", dataA);
            port.ClearWrittenContent();
            // Short, large non-zero offset.
            int newOffset = UInt16.MaxValue + 20;
            byte[] dataA_OffsetOver16k = ShiftToOffset(dataA, newOffset);
            strm.Write(dataA_OffsetOver16k, newOffset, dataA.Length);
            port.AssertWrittenContentAndClear("A_offsetOver16k", dataA);
            // Large (zero offset).
            byte[] dataOver16K = CreateData(UInt16.MaxValue + 100);
            strm.Write(dataOver16K, 0, dataOver16K.Length);
            byte[] b1 = new byte[UInt16.MaxValue];
            byte[] b2 = new byte[dataOver16K.Length - UInt16.MaxValue];
            Array.Copy(dataOver16K, 0, b1, 0, UInt16.MaxValue);
            Array.Copy(dataOver16K, UInt16.MaxValue, b2, 0, dataOver16K.Length - UInt16.MaxValue);
            port.AssertWrittenContentAndClear("Over16k", b1, b2);
        }

        [Test]
        public void WriteReturnsFail_InBeginWrite_ThenRead()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InBeginWrite_(out cli, out strm);
            byte[] buf = new byte[10];
            try {
                int readLen = strm.Read(buf, 0, buf.Length);
                Assert.AreEqual(0, readLen);
                Assert.Fail("expected exception...");
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void WriteReturnsFail_InBeginWrite_ThenWrite()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InBeginWrite_(out cli, out strm);
            try {
                strm.Write(dataA, 0, dataA.Length);
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void WriteReturnsFail_InBeginWrite()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InBeginWrite_(out cli, out strm);
        }

        void WriteReturnsFail_InBeginWrite_(out BluetoothClient cli, out Stream strm)
        {
            TestRfcommPort port;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            //
            port.SetWriteResult(PORT_RETURN_CODE.LINE_ERR);
            try {
                IAsyncResult ar = strm.BeginWrite(dataA, 0, dataA.Length, null, null);
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
            Assert.IsFalse(cli.Connected);
        }

        [Test]
        public void WriteReturnsFail_InEndWrite()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InEndWrite_(out cli, out strm);
        }

        [Test]
        public void WriteReturnsFail_InEndWrite_ThenWrite()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InEndWrite_(out cli, out strm);
            try {
                strm.Write(dataA, 0, dataA.Length);
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        [Test]
        public void WriteReturnsFail_InEndWrite_ThenRead()
        {
            BluetoothClient cli;
            Stream strm;
            WriteReturnsFail_InEndWrite_(out cli, out strm);
            byte[] buf = new byte[10];
            try {
                int readLen = strm.Read(buf, 0, buf.Length);
                Assert.AreEqual(0, readLen);
                Assert.Fail("expected exception...");
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
        }

        void WriteReturnsFail_InEndWrite_(out BluetoothClient cli, out Stream strm)
        {
            TestRfcommPort port;
            //BluetoothClient cli;
            //Stream strm;
            Create_ConnectedBluetoothClient(out port, out cli, out strm);
            //
            // Cause buffering of the write.
            port.SetWriteResult(1);
            IAsyncResult ar =  strm.BeginWrite(dataA, 0, dataA.Length, null, null);
            // Release it and fail the write call.
            port.SetWriteResult(PORT_RETURN_CODE.LINE_ERR);
            OneEventFirer firer = new OneEventFirer(port);
            firer.Run(PORT_EV.TXEMPTY);
            try {
                TestsApmUtils.SafeNoHangEndWrite(strm, ar);
            } catch (IOException ioex) {
                Exception ex = ioex.InnerException;
                Assert.IsInstanceOfType(typeof(SocketException), ex);
            }
            Assert.IsFalse(cli.Connected);
        }

        //--------------------------------------------------------------
        [Test]
        public void SeenEvents1()
        {
            // As seen on PC
            //
            TestRfcommPort port;
            BluetoothClient cli;
            Stream strm2;
            Create_BluetoothClient(out port, out cli, out strm2);
            /*
            BtIf_Create
            WidcommBluetoothFactory
            RfCommIf_Create
            RfCommIf_Client_AssignScnValue
            RfCommIf_SetSecurityLevel
            RfCommIf_SetSecurityLevel 034D2C00 {66-61-6b-65-33-32-66-0} 0 0
            exit RfCommIf_SetSecurityLevel: 1
            RfcommPort_OpenClient: scn: 5, addr: 0-80-98-24-4c-a4
            NativeMethods.RfcommPort_OpenClient ret: SUCCESS=0x00000000
            OpenClient ret: SUCCESS=0x00000000
            OnEventReceived: 200
            HandleEvent: 512=0x200=CONNECTED
            Connected
            Unhandled event: 'CONNECTED'=0x00000200
            OnEventReceived: 1e38
            HandleEvent: 7736=0x1E38=CTS, DSR, RLSD, CONNECTED, CTSS, DSRS, RLSDS
            Unhandled event: 'CTS, DSR, RLSD, CONNECTED, CTSS, DSRS, RLSDS'=0x00001E38
            OnEventReceived: 200
            HandleEvent: 512=0x200=CONNECTED
            Unhandled event: 'CONNECTED'=0x00000200
            OnEventReceived: 4004
            HandleEvent: 16388=0x4004=TXEMPTY, TXCHAR
            Unhandled event: 'TXEMPTY, TXCHAR'=0x00004004
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnDataReceived
            HandleReceive: len: 5
            rec 5 bytes
            {61-62-63-31-0D}
            OnEventReceived: 8038
            HandleEvent: 32824=0x8038=CTS, DSR, RLSD, CONNECT_ERR
            HandlePortEvent: closed when open.
            rec 0 bytes
            */
            IAsyncResult arC =  cli.BeginConnect(bep, null, null);
            port.NewEvent((PORT_EV)0x200); // CONNECTED
            TestsApmUtils.SafeNoHangWaitShort(arC, "Connect");
            Assert.IsTrue(arC.IsCompleted, "Connect IsCompleted");
            cli.EndConnect(arC);
            port.NewEvent((PORT_EV)0x1E38); // CTS, DSR, RLSD, CONNECTED, CTSS, DSRS, RLSDS
            port.NewEvent((PORT_EV)0x200); // CONNECTED
            port.NewEvent((PORT_EV)0x4004); // TXEMPTY, TXCHAR
            port.NewEvent((PORT_EV)0x8038); // CTS, DSR, RLSD, CONNECT_ERR
            Assert.IsFalse(((WidcommRfcommStreamBase)strm2).LiveConnected, "internally knows not connected");
            Assert.IsTrue(cli.Connected, "remember isn't updated till next user IO operation");
        }

        //--------------------------------------------------------------
        private byte[] CreateData(int length)
        {
            Random r = new Random();
            byte[] buf = new byte[length];
            r.NextBytes(buf);
            for (int i = 0; i < buf.Length; ++i) {
                // We have reports that two zeros are inserted into the stream sometimes,
                // so have no zeros for checking.
                if (buf[i] == 0)
                    buf[i] = 255;
            }
            return buf;
        }

        private byte[] ShiftToOffset(byte[] source, int newOffset)
        {
            byte[] dest = new byte[source.Length + newOffset];
            source.CopyTo(dest, newOffset);
            return dest;
        }

        byte[] dataA = { 1, 2, 0xFF, 4 };
        byte[] dataA_offset5 = { 0, 0, 0, 0, 0,/**/  1, 2, 0xFF, 4 };
        byte[] dataAtimes3 = { 1, 2, 0xFF, 4, 1, 2, 0xFF, 4, 1, 2, 0xFF, 4, };
        List<byte[]> dataAtimes3In3ByteBufsNoCoalesce = new List<byte[]>(new byte[][] {
            new byte[] { 1, 2, 0xFF }, new byte[] { 4 }, new byte[] { 1, 2, 0xFF }, new byte[] { 4 }, new byte[] { 1, 2, 0xFF }, new byte[] { 4 }});
        List<byte[]> dataAtimes3In3ByteBufsCoalesce = new List<byte[]>(new byte[][] {
            new byte[] { 1, 2, 0xFF }, new byte[] { 4, 1, 2 }, new byte[] { 0xFF, 4, 1 }, new byte[] { 2, 0xFF, 4 }, });
        const int dataA_offset5_Offset = 5;
        byte[] dataB9 = { 1, 2, 0xFF, 4, 5, 6, 7, 8, 0xF9 };

        static void Assert_AreEqual_Buffers(byte[] expectedData, byte[] buffer, int offset, int count, string name)
        {
            if (count > buffer.Length)
                throw new ArgumentException("count");
            for (int i = 0; i < Math.Min(expectedData.Length, count); ++i) {
                Assert.AreEqual(expectedData[i], buffer[i + offset], name + "-- at " + i);
            }
            Assert.AreEqual(expectedData.Length, count, name + "-- lengths");
        }

    }
}
