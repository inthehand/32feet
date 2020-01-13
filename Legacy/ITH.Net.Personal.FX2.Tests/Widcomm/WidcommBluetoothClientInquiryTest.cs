using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using System.IO;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth;
using System.Threading;
using System.Diagnostics;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommBluetoothClientInquiryTest
    {
        class TestInquiryBtIf : IBtIf
        {
            WidcommBtInterface m_parent;
            //
            public ManualResetEvent startInquiryCalled = new ManualResetEvent(false);
            public int stopInquiryCalled;
            public bool startInquiryFails;
            public bool testDoCallbacksFromWithinStartInquiry;
            /// <summary>
            /// If not set then BondQuery throws, otherwise returns the set value.
            /// </summary>
            public bool? bondQueryResult;

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

            //
            public bool StartInquiry()
            {
                bool success = startInquiryCalled.Set();
                Assert.IsTrue(success, "startInquiryCalled.Set");
                if (startInquiryFails)
                    return false;
                if (testDoCallbacksFromWithinStartInquiry) {
                    // ???? dodgy, on the same thread????
                    InquiryEventsOne();
                }
                return true;
            }

            public void StopInquiry()
            {
                ++stopInquiryCalled;
            }

            public void InquiryEventsOne()
            {
                m_parent.HandleDeviceResponded(WidcommAddressA, DevclassXXXX, WidcommDeviceNameA, false);
                m_parent.HandleDeviceResponded(WidcommAddressB, DevclassXXXX, WidcommDeviceNameB, false);
                m_parent.HandleInquiryComplete(true, 2);
            }

            public void InquiryEventsTwoWithSecondNameEvent()
            {
                m_parent.HandleDeviceResponded(WidcommAddressA, DevclassXXXX, null, false);
                m_parent.HandleDeviceResponded(WidcommAddressB, DevclassXXXX, WidcommDeviceNameB, false);
                m_parent.HandleDeviceResponded(WidcommAddressA, DevclassXXXX, WidcommDeviceNameA, false);
                m_parent.HandleInquiryComplete(true, 2);
            }

            public void InquiryEventsOne_WithNoCompletedEvent()
            {
                m_parent.HandleDeviceResponded(WidcommAddressA, DevclassXXXX, WidcommDeviceNameA, false);
                m_parent.HandleDeviceResponded(WidcommAddressB, DevclassXXXX, WidcommDeviceNameB, false);
                //////m_parentxxxx.HandleInquiryComplete(true, 2);
            }

            //--------
            public bool StartDiscovery(BluetoothAddress address, Guid serviceGuid)
            {
                throw new NotImplementedException();
            }

            public DISCOVERY_RESULT GetLastDiscoveryResult(out BluetoothAddress address, out UInt16 p_num_recs)
            {
                throw new NotImplementedException();
            }

            public ISdpDiscoveryRecordsBuffer ReadDiscoveryRecords(BluetoothAddress address, int maxCount, ServiceDiscoveryParams args)
            {
                throw new NotImplementedException();
            }

            public bool GetLocalDeviceVersionInfo(ref DEV_VER_INFO devVerInfo)
            {
                throw new NotImplementedException();
            }

            //----
            #region Get(Next)RemoteDeviceInfo
            REM_DEV_INFO[] _RememberedDevices;
            int? _RememberedDevicesResultIndex;

            public void SetRememberedDevices(REM_DEV_INFO[] list)
            {
                if (_RememberedDevicesResultIndex != null)
                    throw new InvalidOperationException("Operation in progress.");
                _RememberedDevices = list;
            }

            public REM_DEV_INFO_RETURN_CODE GetRemoteDeviceInfo(ref REM_DEV_INFO remDevInfo, IntPtr p_rem_dev_info, int cb)
            {
                //return REM_DEV_INFO_RETURN_CODE.EOF;
                return ReturnRememberedDevice(ref remDevInfo, true);
            }

            public REM_DEV_INFO_RETURN_CODE GetNextRemoteDeviceInfo(ref REM_DEV_INFO remDevInfo, IntPtr p_rem_dev_info, int cb)
            {
                //throw new NotImplementedException();
                return ReturnRememberedDevice(ref remDevInfo, false);
            }

            private REM_DEV_INFO_RETURN_CODE ReturnRememberedDevice(ref REM_DEV_INFO remDevInfo, bool firstGet)
            {
                if (_RememberedDevices == null) {
                    Debug.Assert(_RememberedDevicesResultIndex == null);
                    if (firstGet) {
                        return REM_DEV_INFO_RETURN_CODE.EOF;
                    } else {
                        throw new InvalidOperationException("Not in progress.");
                    }
                }
                // In progress?  Must NOT for Get, MUST for Next.
                if (firstGet) {
                    if (_RememberedDevicesResultIndex != null)
                        throw new InvalidOperationException("In progress.");
                    _RememberedDevicesResultIndex = 0;
                } else {
                    if (_RememberedDevicesResultIndex == null)
                        return REM_DEV_INFO_RETURN_CODE.ERROR;
                }
                // At end?
                if (_RememberedDevicesResultIndex.Value > _RememberedDevices.Length)
                    throw new InvalidOperationException("Test infrastructure error -- out of range somehow.");
                if (_RememberedDevicesResultIndex.Value == _RememberedDevices.Length) {
                    _RememberedDevicesResultIndex = null;
                    return REM_DEV_INFO_RETURN_CODE.EOF;
                }
                Debug.Assert(_RememberedDevicesResultIndex.Value < _RememberedDevices.Length);
                // Success
                remDevInfo = _RememberedDevices[(int)_RememberedDevicesResultIndex++];
                return REM_DEV_INFO_RETURN_CODE.SUCCESS;
            }
            #endregion

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
                if (!bondQueryResult.HasValue) {
                    throw new InvalidOperationException("BondQuery not allowed here??!??");
                }
                return bondQueryResult.Value;
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
                throw new Exception("The method or operation is not implemented.");
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

        }//class

        private static void Create_BluetoothClient(out TestInquiryBtIf btIf, out BluetoothClient cli)
        {
            // Ensure this is setup, as we call it from a thread which will not work.
            TestUtilities.IsUnderTestHarness();
            //
            btIf = new TestInquiryBtIf();
            TestRfcommPort port;
            Stream strm2;
            WidcommBluetoothClientCommsTest.Create_BluetoothClient(btIf, out port, out cli, out strm2);
        }

        //--------------------------------------------------------------
        static readonly byte[] _widcommAddressA = { 0, 1, 2, 3, 4, 5 };
        static readonly byte[] _widcommAddressB = { 0x00, 0x1F, 0x2E, 0x3D, 0x4C, 0x5B };
        static readonly byte[] _devclassXXXX = { 0x02, 0x01, 0x04 };
        static readonly byte[] _widcommDeviceNameA ={ (byte)'d', (byte)'e', (byte)'v', (byte)'A' };
        static readonly byte[] _widcommDeviceNameB ={ (byte)'d', (byte)'e', (byte)'v', 0xC3, 0x89 };
        // TODO maximum length name?248/7

        public static byte[] WidcommAddressA { get { return Clone(WidcommBluetoothClientInquiryTest._widcommAddressA); } }
        public static byte[] WidcommAddressB { get { return Clone(WidcommBluetoothClientInquiryTest._widcommAddressB); } }
        public static byte[] DevclassXXXX { get { return Clone(WidcommBluetoothClientInquiryTest._devclassXXXX); } }
        public static byte[] WidcommDeviceNameA { get { return Clone(WidcommBluetoothClientInquiryTest._widcommDeviceNameA); } }
        public static byte[] WidcommDeviceNameB { get { return Clone(WidcommBluetoothClientInquiryTest._widcommDeviceNameB); } }

        private static byte[] Clone(byte[] p)
        {
            return (byte[])p.Clone();
        }

        //
        readonly BluetoothAddress AddressA = BluetoothAddress.Parse("00:01:02:03:04:05");
        readonly BluetoothAddress AddressB = BluetoothAddress.Parse("00:1F:2E:3D:4C:5B");
        const string NameA = "devA";
        const string NameB = "dev\u00C9"; // unicode E-acute
        const uint CodXXXX = 0x20104/*00*/;
        const ServiceClass CodXXXXSvcClass = ServiceClass.Network;
        const DeviceClass CodXXXXDeviceClass = DeviceClass.DesktopComputer;

        private void VerifyDevicesOne(BluetoothDeviceInfo[] devices)
        {
            Assert.AreEqual(2, devices.Length, "count");
            Assert.AreEqual(AddressA, devices[0].DeviceAddress, "addrA");
            Assert.AreEqual(NameA, devices[0].DeviceName, "nameA");
            Assert.AreEqual(AddressB, devices[1].DeviceAddress, "addrB");
            Assert.AreEqual(NameB, devices[1].DeviceName, "nameB");
            // TODO Assert DeviceClass etc etc.
            Assert.AreEqual(CodXXXXSvcClass, devices[0].ClassOfDevice.Service, "codSvcA");
            Assert.AreEqual(CodXXXXDeviceClass, devices[0].ClassOfDevice.Device, "codDevA");
            Assert.AreEqual(CodXXXX, devices[0].ClassOfDevice.Value, "codA");
        }

        [Test]
        public void One()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void StartInquiryFails_ThenTestOne()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            //
            btIf.startInquiryFails = true;
            try {
                BluetoothDeviceInfo[] devicesX = cli.DiscoverDevices();
                Assert.Fail("should have thrown!");
            } catch (System.Net.Sockets.SocketException) {
            }
            bool signalled1 = btIf.startInquiryCalled.WaitOne(0, false);
            Assert.IsTrue(signalled1, "!signalled_a!!!!!");
            //
            btIf.startInquiryCalled.Reset();
            btIf.startInquiryFails = false;
            //
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            IAsyncResult arDD = cli.BeginDiscoverDevices(255, true, true, true, true, null, null);
            TestsApmUtils.SafeNoHangWait(arDD, "DiscoverDevices");
            BluetoothDeviceInfo[] devices = cli.EndDiscoverDevices(arDD);
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void One_Remembered()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices(1000, false, true, false, false);
            Assert.AreEqual(0, devices.Length);
            Assert.AreEqual(0, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void One_SameThread()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            btIf.testDoCallbacksFromWithinStartInquiry = true;
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void TwoWithSecondNameEvent()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsTwoWithSecondNameEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void FromRegistry_Attempt()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            btIf.testDoCallbacksFromWithinStartInquiry = true;
            btIf.bondQueryResult = false;
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = true;
            try {
                BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
                // May succeed, if the current machine has Widcomm installed...
            } catch (IOException ex) {
                // May fail, if the current machine doesn't have Widcomm installed...
                Assert.IsInstanceOfType(typeof(IOException), ex, "ex.Type");
                Assert.AreEqual("Widcomm 'Devices' key not found in the Registry.", ex.Message, "ex.Message");
            }
        }

        //--------
        [Test]
        public void EventsOccurWithNoPrecedingDiscoverDevicesCall()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
        }

        void _EventsOccurWithNoPrecedingDiscoverDevicesCall(
            TestInquiryBtIf btIf, BluetoothClient cli)
        {
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(0, false);
                Assert.IsFalse(signalled, "!signalled_A!!!!!");
                btIf.InquiryEventsOne();
                signalled = btIf.startInquiryCalled.WaitOne(0, false);
                Assert.IsFalse(signalled, "!signalled_B!!!!!");
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void EventsOccurWithNoPrecedingDiscoverDevicesCall_WithNoCompletedEvent()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
        }

        void _EventsOccurWithNoPrecedingDiscoverDevicesCall_WithNoCompletedEvent(
            TestInquiryBtIf btIf, BluetoothClient cli)
        {
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(0, false);
                Assert.IsFalse(signalled, "!signalled_A!!!!!");
                btIf.InquiryEventsOne_WithNoCompletedEvent();
                signalled = btIf.startInquiryCalled.WaitOne(0, false);
                Assert.IsFalse(signalled, "!signalled_B!!!!!");
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            Assert.AreEqual(0, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void EventsOccurWithNoPrecedingDiscoverDevicesCall_BothRepeated()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            Create_BluetoothClient(out btIf, out cli);
            //
            _EventsOccurWithNoPrecedingDiscoverDevicesCall_WithNoCompletedEvent(btIf, cli);
            _EventsOccurWithNoPrecedingDiscoverDevicesCall_WithNoCompletedEvent(btIf, cli);
            _EventsOccurWithNoPrecedingDiscoverDevicesCall(btIf, cli);
            btIf.stopInquiryCalled = 0;
            _EventsOccurWithNoPrecedingDiscoverDevicesCall(btIf, cli);
        }


        // With default non-infinite InquiryLength, test now no correct.[Test]
        public void One_CompletedEventLateOrNeverComes()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne_WithNoCompletedEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            Assert.IsTrue(cli.InquiryLength.CompareTo(TimeSpan.Zero) < 0, "Excepted infinite InquiryLength, but was: " + cli.InquiryLength);
            IAsyncResult arDD = cli.BeginDiscoverDevices(255, true, true, true, false, null, null);
            bool completed = arDD.AsyncWaitHandle.WaitOne(1 * 1000, false);
            Assert.IsFalse(completed, "Expected DD to time-out.");
            // NON USEFUL/WOULD BLOCK
            //BluetoothDeviceInfo[] devices = cli.EndDiscoverDevices(arDD);
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            //VerifyDevicesOne(devices);
            //Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }


        [Test]
        public void One_CompletedEventLateOrNeverComes_ButHasTimeout()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            cli.InquiryLength = TimeSpan.FromMilliseconds(750);
            //
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne_WithNoCompletedEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            Assert.IsTrue(cli.InquiryLength.CompareTo(TimeSpan.Zero) > 0, "blehhhhhhhhhhhhhhhhhhhhhh");
            IAsyncResult arDD = cli.BeginDiscoverDevices(255, true, true, true, false, null, null);
            // Timeout Killer occurs 1.5 times the InquiryLength, 1125ms
            bool completed = arDD.AsyncWaitHandle.WaitOne(1500, false);
            Assert.IsTrue(completed, "Unexpected DD time-out.");
            BluetoothDeviceInfo[] devices = cli.EndDiscoverDevices(arDD);
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void One_TimeoutSet_ButCompletesBeforeTimeout()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            cli.InquiryLength = TimeSpan.FromSeconds(1);
            //
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            BluetoothDeviceInfo[] devices = cli.DiscoverDevices();
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
            // Wait around, trying to see any exception on the timeout thread.
            Thread.Sleep(1650);
        }


        [Test]
        public void TwoDiscosConcurrently_lattersGetFirstsInquiredList_basedOnTestOne()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsOne();
            };
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            // We really should test with two BtCli instances but that's a bit hard to set-up.
            IAsyncResult ar1 = cli.BeginDiscoverDevices(255, true, true, true, false, null, null);
            IAsyncResult ar2 = cli.BeginDiscoverDevices(255, true, true, true, false, null, null);
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            bool signalledAr = _WaitAll(new WaitHandle[] { ar1.AsyncWaitHandle, ar2.AsyncWaitHandle }, 15000);
            Assert.IsTrue(signalledAr, "signalledAr");
            BluetoothDeviceInfo[] devices1 = cli.EndDiscoverDevices(ar1);
            BluetoothDeviceInfo[] devices2 = cli.EndDiscoverDevices(ar2);
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devices1);
            VerifyDevicesOne(devices2);
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

#if !NETCF
        // TODO ! Need test with 'remembered' devices...

        [Test]
        public void Ebap_TwoWithSecondNameEvent()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsTwoWithSecondNameEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            var bc = new BluetoothComponent(cli);
            var devicesSteps = new List<BluetoothDeviceInfo[]>();
            BluetoothDeviceInfo[] devicesResult = null;
            var complete = new ManualResetEvent(false);
            bc.DiscoverDevicesProgress += delegate(object sender, DiscoverDevicesEventArgs e) {
                devicesSteps.Add(e.Devices);
            };
            bc.DiscoverDevicesComplete += delegate(object sender, DiscoverDevicesEventArgs e) {
                try {
                    devicesResult = e.Devices;
                } finally {
                    complete.Set();
                }
            };
            bc.DiscoverDevicesAsync(255, true, true, true, false, 999);
            bool signalled2 = complete.WaitOne(15000);
            Assert.IsTrue(signalled2, "signalled2");
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devicesResult);
            Assert.AreEqual(3, devicesSteps.Count);
            Assert.AreEqual(1, devicesSteps[0].Length, "[0].Len");
            Assert.AreEqual(AddressA, devicesSteps[0][0].DeviceAddress, "[0][0].Addr");
            Assert.AreEqual(AddressA.ToString("C"), devicesSteps[0][0].DeviceName, "[0][0].Name"); // no name
            Assert.AreEqual(1,        devicesSteps[1].Length, "[1].Len");
            Assert.AreEqual(AddressB, devicesSteps[1][0].DeviceAddress, "[1][0].Addr");
            Assert.AreEqual(NameB,    devicesSteps[1][0].DeviceName, "[1][0].Name");
            Assert.AreEqual(1,        devicesSteps[2].Length, "[2].Len");
            Assert.AreEqual(AddressA, devicesSteps[2][0].DeviceAddress, "[2][0].Addr");
            Assert.AreEqual(NameA,    devicesSteps[2][0].DeviceName, "[2][0].Name");
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void Ebap_WithRememberedDuplicateDevices_From_TwoWithSecondNameEvent()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsTwoWithSecondNameEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            var bc = new BluetoothComponent(cli);
            var devicesSteps = new List<BluetoothDeviceInfo[]>();
            BluetoothDeviceInfo[] devicesResult = null;
            var complete = new ManualResetEvent(false);
            bc.DiscoverDevicesProgress += delegate(object sender, DiscoverDevicesEventArgs e) {
                devicesSteps.Add(e.Devices);
            };
            bc.DiscoverDevicesComplete += delegate(object sender, DiscoverDevicesEventArgs e) {
                try {
                    devicesResult = e.Devices;
                } finally {
                    complete.Set();
                }
            };
            //
            REM_DEV_INFO[] remembered = {
                CreateREM_DEV_INFO(NameB, AddressB),
                CreateREM_DEV_INFO(NameA, AddressA),
            };
            btIf.SetRememberedDevices(remembered);
            //
            bc.DiscoverDevicesAsync(255, true, true, true, false, 999);
            bool signalled2 = complete.WaitOne(15000);
            Assert.IsTrue(signalled2, "signalled2");
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devicesResult);
            Assert.AreEqual(4, devicesSteps.Count);
            int step = 0;
            Assert.AreEqual(2, devicesSteps[step].Length, "[0].Len");
            Assert.AreEqual(AddressB, devicesSteps[step][0].DeviceAddress, "[0][1].Addr");
            Assert.AreEqual(NameB,    devicesSteps[step][0].DeviceName, "[0][1].Name"); // no name
            Assert.AreEqual(AddressA, devicesSteps[step][1].DeviceAddress, "[0][0].Addr");
            Assert.AreEqual(NameA,    devicesSteps[step][1].DeviceName, "[0][0].Name"); // no name
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[0].Len");
            Assert.AreEqual(AddressA, devicesSteps[step][0].DeviceAddress, "[0][0].Addr");
            Assert.AreEqual(AddressA.ToString("C"), devicesSteps[step][0].DeviceName, "[0][0].Name"); // no name
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[1].Len");
            Assert.AreEqual(AddressB, devicesSteps[step][0].DeviceAddress, "[1][0].Addr");
            Assert.AreEqual(NameB, devicesSteps[step][0].DeviceName, "[1][0].Name");
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[2].Len");
            Assert.AreEqual(AddressA, devicesSteps[step][0].DeviceAddress, "[2][0].Addr");
            Assert.AreEqual(NameA, devicesSteps[step][0].DeviceName, "[2][0].Name");
            ++step;
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        [Test]
        public void Ebap_WithRememberedOnePartDuplicateDevice_From_TwoWithSecondNameEvent()
        {
            TestInquiryBtIf btIf;
            BluetoothClient cli;
            //
            Create_BluetoothClient(out btIf, out cli);
            Assert.IsFalse(btIf.testDoCallbacksFromWithinStartInquiry, "tdcfwsi");
            ThreadStart dlgt = delegate {
                bool signalled = btIf.startInquiryCalled.WaitOne(10000, false);
                Assert.IsTrue(signalled, "!signalled!!!!!");
                btIf.InquiryEventsTwoWithSecondNameEvent();
            };
            IAsyncResult arDlgt = Delegate2.BeginInvoke(dlgt, null, null);
            //
            WidcommBluetoothClient.ReadKnownDeviceFromTheRegistry = false;
            var bc = new BluetoothComponent(cli);
            var devicesSteps = new List<BluetoothDeviceInfo[]>();
            BluetoothDeviceInfo[] devicesResult = null;
            var complete = new ManualResetEvent(false);
            bc.DiscoverDevicesProgress += delegate(object sender, DiscoverDevicesEventArgs e) {
                devicesSteps.Add(e.Devices);
            };
            bc.DiscoverDevicesComplete += delegate(object sender, DiscoverDevicesEventArgs e) {
                try {
                    devicesResult = e.Devices;
                } finally {
                    complete.Set();
                }
            };
            //
            REM_DEV_INFO[] remembered = {
                CreateREM_DEV_INFO(null, AddressB),
            };
            btIf.SetRememberedDevices(remembered);
            //
            bc.DiscoverDevicesAsync(255, true, true, true, false, 999);
            bool signalled2 = complete.WaitOne(15000);
            Assert.IsTrue(signalled2, "signalled2");
            Delegate2.EndInvoke(dlgt, arDlgt); // any exceptions?
            VerifyDevicesOne(devicesResult);
            Assert.AreEqual(4, devicesSteps.Count);
            int step = 0;
            Assert.AreEqual(1, devicesSteps[step].Length, "[0].Len");
            Assert.AreEqual(AddressB, devicesSteps[step][0].DeviceAddress, "[0][1].Addr");
            Assert.AreEqual(AddressB.ToString("C"), devicesSteps[step][0].DeviceName, "[0][1].Name"); // no name
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[0].Len");
            Assert.AreEqual(AddressA, devicesSteps[step][0].DeviceAddress, "[0][0].Addr");
            Assert.AreEqual(AddressA.ToString("C"), devicesSteps[step][0].DeviceName, "[0][0].Name"); // no name
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[1].Len");
            Assert.AreEqual(AddressB, devicesSteps[step][0].DeviceAddress, "[1][0].Addr");
            Assert.AreEqual(NameB, devicesSteps[step][0].DeviceName, "[1][0].Name");
            ++step;
            Assert.AreEqual(1, devicesSteps[step].Length, "[2].Len");
            Assert.AreEqual(AddressA, devicesSteps[step][0].DeviceAddress, "[2][0].Addr");
            Assert.AreEqual(NameA, devicesSteps[step][0].DeviceName, "[2][0].Name");
            ++step;
            Assert.AreEqual(1, btIf.stopInquiryCalled, "stopInquiryCalled");
        }

        private REM_DEV_INFO CreateREM_DEV_INFO(string name, BluetoothAddress addr)
        {
            var dev = new REM_DEV_INFO();
            if (name != null)
                dev.bd_name = Encoding.UTF8.GetBytes(name + "\0");
            if (addr != null)
                dev.bda = WidcommUtils.FromBluetoothAddress(addr);
            return dev;
        }
#endif

        //------------------------------------------------------------
        static bool _WaitAll(WaitHandle[] events, int timeout)
        {
#if false && !NETCF
            return WaitHandle.WaitAll(events, timeout);
#else
            var final = DateTime.UtcNow.AddMilliseconds(timeout);
            foreach (var curE in events) {
                TimeSpan curTo = final.Subtract(DateTime.UtcNow);
                if (curTo.CompareTo(TimeSpan.Zero) <= 0) {
                    Debug.Fail("Should we expect to get here?  Normally exit at !signalled below...");
                    return false;
                }
                bool signalled = curE.WaitOne((int)curTo.TotalMilliseconds, false);
                if (!signalled) {
                    return false;
                }
            }//for
            return true;
#endif
        }

    }
}
