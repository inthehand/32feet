using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using System.Diagnostics;
using NUnit.Framework;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestSdpService2 : ISdpService
    {
        int m_countCommitRecord, m_countDispose;
        //
        int m_countAddAttributeCalls, m_countWellKnownCalls;
        List<SdpServiceExpectedCall> m_actualAddAttr = new List<SdpServiceExpectedCall>();
        StringBuilder log = new StringBuilder();

        public void AssertCalls(string expected)
        {
            try {
                Assert.AreEqual(expected, log.ToString(), "SdpService function calls");
            } catch {
                //Console.WriteLine(log.ToString());
#if !NETCF
                //System.Windows.Forms.MessageBox.Show(log.ToString());
#endif
                throw;
            }
        }

        internal void AssertAreZeroAddAttributeCalls()
        {
            Debug.Assert(m_actualAddAttr.Count == m_countAddAttributeCalls, "count and list count");
            Assert.AreEqual(0, m_countAddAttributeCalls, "Expected Count AddAttr calls");
        }

        internal void AssertAreZeroWellKnownCalls()
        {
            Assert.AreEqual(0, m_countWellKnownCalls, "Expected Count WellKnown calls");
        }


        internal void AssertAddAttribute(SdpServiceExpectedCall expected)
        {
            Assert.AreEqual(1, m_actualAddAttr.Count, "Expected Count AddAttr Calls");
            SdpServiceExpectedCall cur = m_actualAddAttr[0];
            Assert.AreEqual(expected.attrId, cur.attrId, "attrId");
            Assert.AreEqual(expected.attrType, cur.attrType, "attrType");
            Assert.AreEqual(expected.val, cur.val, "byte[] val");
            Assert.AreEqual(expected.attrLen, cur.attrLen, "attrLen");
            Assert.AreEqual(0, m_countWellKnownCalls, "m_countWellKnownCalls");
        }

        internal void AssertAddAttribute(SdpServiceExpectedCall[] expected)
        {
            for (int i = 0; i < Math.Min(expected.Length, m_actualAddAttr.Count); ++i) {
                Assert.AreEqual(expected[i].attrId, m_actualAddAttr[i].attrId, "attrId");
                Assert.AreEqual(expected[i].attrType, m_actualAddAttr[i].attrType, "attrType");
                Assert.AreEqual(expected[i].val, m_actualAddAttr[i].val, "byte[] val");
                Assert.AreEqual(expected[i].attrLen, m_actualAddAttr[i].attrLen, "attrLen");
            }
            Assert.AreEqual(1, m_actualAddAttr.Count, "Expected Count AddAttr Calls");
            Assert.AreEqual(0, m_countWellKnownCalls, "m_countWellKnownCalls");
        }

        #region ISdpService Members

        void ISdpService.AddServiceClassIdList(IList<Guid> serviceClasses)
        {
            string all = StringUtilities.String_Join(serviceClasses);
            log.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "AddServiceClassIdList: <{0}>\r\n", all);
            ++m_countWellKnownCalls;
        }

        void ISdpService.AddServiceClassIdList(Guid serviceClass)
        {
            log.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "AddServiceClassIdList: {0}\r\n", serviceClass);
            ++m_countWellKnownCalls;
        }

        void ISdpService.AddRFCommProtocolDescriptor(byte scn)
        {
            log.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "AddRFCommProtocolDescriptor: {0}\r\n", scn);
            ++m_countWellKnownCalls;
        }

        void ISdpService.AddServiceName(string serviceName)
        {
            log.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "AddServiceName: {0}\r\n", serviceName);
            ++m_countWellKnownCalls;
        }

        void ISdpService.AddAttribute(ushort id, SdpService.DESC_TYPE dt, int valLen, byte[] val)
        {
            log.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "AddAttribute: id: 0x{0:X4}, dt: {1}, len: {2}, val: {3}\r\n",
                id, dt, valLen,
                val == null ? "(null)" : BitConverter.ToString(val));
            m_actualAddAttr.Add(new SdpServiceExpectedCall(id, dt, checked((uint)valLen), val));
            ++m_countAddAttributeCalls;
            Debug.Assert(m_actualAddAttr.Count == m_countAddAttributeCalls, "count and list count");
        }

        void ISdpService.CommitRecord()
        {
            ++m_countCommitRecord;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            ++m_countDispose;
        }

        public int NumDisposeCalls
        {
            get { return m_countDispose; }
        }

        #endregion

    }

}
