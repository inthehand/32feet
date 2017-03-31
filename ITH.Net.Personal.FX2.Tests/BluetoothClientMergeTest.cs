using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using List_IBluetoothDeviceInfo = System.Collections.Generic.List<InTheHand.Net.Bluetooth.Factory.IBluetoothDeviceInfo>;
using System.Diagnostics;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests
{
    [TestFixture]
    public class BluetoothClientMergeTest
    {
        internal static List_IBluetoothDeviceInfo BluetoothClient_DiscoverDevicesMerge(
            bool authenticated, bool remembered, bool unknown,
            List_IBluetoothDeviceInfo knownDevices,
            List_IBluetoothDeviceInfo discoverableDevices, DateTime discoTime)
        {
#pragma warning disable 618 // Obsolete
            return BluetoothClient.DiscoverDevicesMerge(
                authenticated, remembered, unknown,
                knownDevices, discoverableDevices, discoTime);
#pragma warning restore 618 // Obsolete
        }

        const string _addr1 = "000000" + "111111";
        const string _addr2 = "000000" + "222222";
        const string _addr3 = "000000" + "333333";
        const string _addr4 = "000000" + "444444";
        const string _addr5 = "000000" + "555555";
        const string _addr6 = "000000" + "666666";
        const string _addr7 = "000000" + "777777";
        static BluetoothAddress Addr1 { get { return BluetoothAddress.Parse(_addr1); } }
        static BluetoothAddress Addr2 { get { return BluetoothAddress.Parse(_addr2); } }
        static BluetoothAddress Addr3 { get { return BluetoothAddress.Parse(_addr3); } }
        static BluetoothAddress Addr4 { get { return BluetoothAddress.Parse(_addr4); } }
        static BluetoothAddress Addr5 { get { return BluetoothAddress.Parse(_addr5); } }
        static BluetoothAddress Addr6 { get { return BluetoothAddress.Parse(_addr6); } }
        static BluetoothAddress Addr7 { get { return BluetoothAddress.Parse(_addr7); } }

        private static void CreateDeviceListsTwoFourOneOverlap(out List_IBluetoothDeviceInfo known, out List_IBluetoothDeviceInfo discovered)
        {
            known = new List_IBluetoothDeviceInfo();
            known.Add(new TestBluetoothDeviceInfo(Addr1, true, true));
            known.Add(new TestBluetoothDeviceInfo(Addr2, true, true));
            known.Add(new TestBluetoothDeviceInfo(Addr6, true, false));
            known.Add(new TestBluetoothDeviceInfo(Addr7, true, false));
            //
            discovered = new List_IBluetoothDeviceInfo();
            discovered.Add(new TestBluetoothDeviceInfo(Addr2));
            discovered.Add(new TestBluetoothDeviceInfo(Addr3));
            discovered.Add(new TestBluetoothDeviceInfo(Addr4));
            discovered.Add(new TestBluetoothDeviceInfo(Addr5));
            discovered.Add(new TestBluetoothDeviceInfo(Addr6));
        }


        [Test]
        public void ARU()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            authenticated = remembered = true;
            unknown = true;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(7, result.Count, "A .Count");
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Remembered, "Remembered");
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr3).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr3).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr4).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr4).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr5).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr5).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr6).Authenticated, "Authenticated 6");
            Assert.IsTrue(GetEntryOrThrow(result, Addr6).Remembered, "Remembered 6");
            Assert.IsFalse(GetEntryOrThrow(result, Addr7).Authenticated, "Authenticated 7");
            Assert.IsTrue(GetEntryOrThrow(result, Addr7).Remembered, "Remembered 7");
        }

        [Test]
        public void xxU()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            authenticated = remembered = false;
            unknown = true;
            discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(3, result.Count, "B .Count");
            Assert.IsFalse(GetEntryOrThrow(result, Addr3).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr3).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr4).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr4).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr5).Authenticated);
            Assert.IsFalse(GetEntryOrThrow(result, Addr5).Remembered, "Remembered");
        }

        [Test]
        public void ARx()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            authenticated = remembered = true;
            unknown = false;
            discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(4, result.Count, "C-RA .Count");
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Remembered, "Remembered");
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr6).Authenticated, "Authenticated 6");
            Assert.IsTrue(GetEntryOrThrow(result, Addr6).Remembered, "Remembered 6");
            Assert.IsFalse(GetEntryOrThrow(result, Addr7).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr7).Remembered, "Remembered 7");
        }

        [Test]
        public void xRx()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            remembered = true;
            authenticated = false;
            unknown = false;
            discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(4, result.Count, "C-Ra .Count");
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Remembered, "Remembered");
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Remembered, "Remembered");
            Assert.IsFalse(GetEntryOrThrow(result, Addr6).Authenticated, "Authenticated 6");
            Assert.IsTrue(GetEntryOrThrow(result, Addr6).Remembered, "Remembered 6");
            Assert.IsFalse(GetEntryOrThrow(result, Addr7).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr7).Remembered, "Remembered 7");
        }

        [Test]
        public void Axx()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            remembered = false;
            authenticated = true;
            unknown = false;
            discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(2, result.Count, "C-rA .Count");
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr1).Remembered, "Remembered");
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Authenticated);
            Assert.IsTrue(GetEntryOrThrow(result, Addr2).Remembered, "Remembered");
        }

        [Test]
        public void xxx()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            DateTime discoTime = DateTime.UtcNow;
            //
            CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
            authenticated = remembered = false;
            unknown = false;
            discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(0, result.Count, "D .Count");
        }

        private static void CreateDeviceListsDuplicates(out List_IBluetoothDeviceInfo known, out List_IBluetoothDeviceInfo discovered)
        {
            known = new List_IBluetoothDeviceInfo();
            known.Add(new TestBluetoothDeviceInfo(Addr1, true, true));
            known.Add(new TestBluetoothDeviceInfo(Addr2, true, true));
            //
            discovered = new List_IBluetoothDeviceInfo();
            discovered.Add(new TestBluetoothDeviceInfo(Addr2));
            discovered.Add(new TestBluetoothDeviceInfo(Addr3));
            discovered.Add(new TestBluetoothDeviceInfo(Addr2));
            discovered.Add(new TestBluetoothDeviceInfo(Addr3));
            discovered.Add(new TestBluetoothDeviceInfo(Addr4));
            discovered.Add(new TestBluetoothDeviceInfo(Addr5));
        }

        [Test]
        [Explicit]
        public void Duplicates()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            //
            CreateDeviceListsDuplicates(out known, out discovered);
            authenticated = remembered = true;
            unknown = true;
            DateTime discoTime = DateTime.UtcNow;
            result = BluetoothClient_DiscoverDevicesMerge(
                authenticated, remembered, unknown, known, discovered, discoTime);
            Assert.AreEqual(7, result.Count, ".Count");
            //Assert.AreEqual(5, result.Count, ".Count");
        }

        [Test]
        public void DiscoverableOnly()
        {
            bool authenticated, remembered, unknown;
            List_IBluetoothDeviceInfo known, discovered, result;
            //
            int flagSource = 0;
            while (true) {
                authenticated = (flagSource & 1) != 0;
                remembered = (flagSource & 2) != 0;
                unknown = (flagSource & 4) != 0;
                //
                CreateDeviceListsTwoFourOneOverlap(out known, out discovered);
                result = BluetoothClient.DiscoverDevicesMerge(
                    authenticated, remembered, unknown, known, discovered,
                    true, DateTime.UtcNow);
                Assert.AreEqual(5, result.Count, ".Count" + " [" + flagSource + "]");
                Assert.IsTrue(GetEntryOrThrow(result, Addr2).Authenticated,
                    "2A" + " [" + flagSource + "]");
                Assert.IsTrue(GetEntryOrThrow(result, Addr2).Remembered,
                    "2R" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr3).Authenticated,
                    "3a" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr3).Remembered,
                    "3r" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr4).Authenticated,
                    "4a" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr4).Remembered,
                    "4r" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr5).Authenticated,
                    "5a" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr5).Remembered,
                    "5r" + " [" + flagSource + "]");
                Assert.IsFalse(GetEntryOrThrow(result, Addr6).Authenticated,
                    "6a" + " [" + flagSource + "]");
                Assert.IsTrue(GetEntryOrThrow(result, Addr6).Remembered,
                    "6R" + " [" + flagSource + "]");
                if (authenticated && remembered && unknown)
                    break; // Have tested all cases of the original three flags.
                ++flagSource;
            }//while
        }

        private IBluetoothDeviceInfo GetEntryOrThrow(List_IBluetoothDeviceInfo result, BluetoothAddress address)
        {
            IBluetoothDeviceInfo bdi = result.Find(delegate(IBluetoothDeviceInfo cur) {
                return cur.DeviceAddress == address;
            });
            if (bdi == null)
                throw new KeyNotFoundException("No BDI for " + address + ".");
            return bdi;
        }

        //====
        class TestBluetoothDeviceInfo : IBluetoothDeviceInfo
        {
            BluetoothAddress m_addr;
            bool m_rembd, m_authd;
            DateTime m_lastSeen;

            internal TestBluetoothDeviceInfo(BluetoothAddress addr)
            {
                m_addr = addr;
            }

            internal TestBluetoothDeviceInfo(BluetoothAddress addr, bool rembd, bool authd)
                : this(addr)
            {
                m_rembd = rembd;
                m_authd = authd;
            }

            public void Merge(IBluetoothDeviceInfo other)
            {
                m_rembd = other.Remembered;
                m_authd = other.Authenticated;
            }

            public void SetDiscoveryTime(DateTime dt)
            {
                if (m_lastSeen != DateTime.MinValue)
                    throw new InvalidOperationException("LastSeen is already set.");
                m_lastSeen = dt;
            }

            #region IBluetoothDeviceInfo Members

            public bool Authenticated
            {
                get { return m_authd; }
            }

            public global::InTheHand.Net.Bluetooth.ClassOfDevice ClassOfDevice
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public bool Connected
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public BluetoothAddress DeviceAddress
            {
                get { return m_addr; }
            }

            public string DeviceName
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public bool HasDeviceName
            {
                get
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            public global::InTheHand.Net.Bluetooth.ServiceRecord[] GetServiceRecords(Guid service)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public IAsyncResult BeginGetServiceRecords(Guid service, AsyncCallback callback, object state)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public InTheHand.Net.Bluetooth.ServiceRecord[] EndGetServiceRecords(IAsyncResult asyncResult)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public byte[][] GetServiceRecordsUnparsed(Guid service)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public Guid[] InstalledServices
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public DateTime LastSeen
            {
                get { return m_lastSeen; }
            }

            public DateTime LastUsed
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public void Refresh()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public bool Remembered
            {
                get { return m_rembd; }
            }

            public int Rssi
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public void SetServiceState(Guid service, bool state, bool throwOnError)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void SetServiceState(Guid service, bool state)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void ShowDialog()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public void Update()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            //--------
            public override bool Equals(object obj)
            {
                Debug.Fail("who calls this????");
                IBluetoothDeviceInfo bdiO = obj as IBluetoothDeviceInfo;
                Debug.Assert(bdiO != null, "CHANGED/-ING Bdi->IBdi");
                if (bdiO != null) {
                    return this.DeviceAddress.Equals(bdiO.DeviceAddress);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.DeviceAddress.GetHashCode();
            }

            public int CompareTo(object obj)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            InTheHand.Net.Bluetooth.RadioVersions IBluetoothDeviceInfo.GetVersions()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }

}
