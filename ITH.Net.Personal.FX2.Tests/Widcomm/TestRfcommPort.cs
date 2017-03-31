using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using NUnit.Framework;
using System.Diagnostics;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestRfcommPort : IRfcommPort
    {
        WidcommRfcommStreamBase parent;
        // Write
        List<byte[]> m_writtenContent = new List<byte[]>();
        // Connect
        int m_OpenClientCalled, m_CloseCalled;
        System.Threading.ManualResetEvent m_OpenClientCalledEvent = new System.Threading.ManualResetEvent(false);
        PORT_RETURN_CODE m_OpenClientResult;
        int m_DestroyRfcommPortCalled;
        int m_ocScn;
        byte[] m_ocAddress;


        //--------
        public void SetParentStream(InTheHand.Net.Bluetooth.Widcomm.WidcommRfcommStreamBase parent)
        {
            this.parent = parent;
        }

        internal void TestHackClearParentStream()
        {
            this.parent = null;
        }

        //--------
        public void AssertWrittenContentAndClear(string name, params byte[][] expectedData)
        {
            AssertWrittenContent(name, expectedData);
            ClearWrittenContent();
        }

        public void AssertWrittenContent(string name, params byte[][] expectedData)
        {
            bool sameNumber = (expectedData.Length == m_writtenContent.Count);
            for (int i = 0; i < Math.Min(expectedData.Length, m_writtenContent.Count); ++i) {
                Assert.AreEqual(expectedData[i], m_writtenContent[i], name + " -- at " + i);
                //ManualAssertByteArraysEqual(expectedData[i], m_writtenContent[i]);
            } //for
            Assert.AreEqual(expectedData.Length, m_writtenContent.Count, name + " -- number of blocks");
        }

        private void ManualAssertByteArraysEqual(byte[] expected, byte[] actual)
        {
            if (expected.Length != actual.Length)
                throw new AssertionException("Array lengths differ, expected " + expected.Length
                    + " but was " + actual.Length);
            for (int i = 0; i < expected.Length; ++i) {
                if (expected[i] != actual[i])
                    throw new AssertionException("Arrays differ at index " + i
                        + " expected " + expected[i] + " but was " + actual[i]);
            }
        }

        internal void ClearWrittenContent()
        {
            m_writtenContent.Clear();
        }

        PORT_RETURN_CODE? m_writeResult;
        ushort? m_writeAcceptLength;
        object _lock = new object();

        public virtual InTheHand.Net.Bluetooth.Widcomm.PORT_RETURN_CODE Write(
            byte[] p_data, ushort len_to_write, out ushort p_len_written)
        {
            lock(_lock) {
                Debug.Assert(len_to_write <= p_data.Length, "len_to_write too long");
                if (m_writeResult != null) { // Failure result
                    Debug.Assert(m_writeResult != PORT_RETURN_CODE.SUCCESS);
                    p_len_written = 9999;
                    Debug.Assert(m_writeAcceptLength == null, "we don't use it here...");
                    return m_writeResult.Value;
                } else {
                    ushort writeAcceptLength ;
                    if (m_writeAcceptLength != null) {
                        writeAcceptLength = Math.Min(len_to_write, m_writeAcceptLength.Value);
                        int remains = m_writeAcceptLength.Value - writeAcceptLength;
                        m_writeAcceptLength = checked((ushort)remains);
                    } else {
                        writeAcceptLength = len_to_write;
                    }
                    byte[] acceptedData = new byte[writeAcceptLength];
                    Array.Copy(p_data, acceptedData, acceptedData.Length);
                    m_writtenContent.Add(acceptedData);
                    p_len_written = checked((ushort)acceptedData.Length);
                    return PORT_RETURN_CODE.SUCCESS;
                }
            }
        }

        internal void SetWriteResult(PORT_RETURN_CODE result)
        {
            lock (_lock) {
                m_writeResult = result;
                m_writeAcceptLength = null;
            }
        }

        internal void SetWriteResult(ushort amountToAccept)
        {
            lock (_lock) {
                m_writeResult = null;
                m_writeAcceptLength = amountToAccept;
            }
        }

        internal ushort? GetWriteCapacity()
        {
            lock (_lock) { return m_writeAcceptLength; }
        }

        internal PORT_RETURN_CODE? GetWriteResultStatus()
        {
            lock (_lock) { return m_writeResult; }
        }

        //--------
        internal void NewEvent(PORT_EV eventId)
        {
            parent.HandlePortEvent(eventId, this);
        }

        public void NewReceive(byte[] data)
        {
            parent.HandlePortReceive(data, this);
        }

        //--------
        public PORT_RETURN_CODE OpenClient(int scn, byte[] address)
        {
            Debug.Assert(m_OpenClientCalled == 0, "called twice?! pre-count: " + m_OpenClientCalled);
            ++m_OpenClientCalled;
            m_OpenClientCalledEvent.Set();
            Debug.Assert(m_ocAddress == null, "called twice?!");
            m_ocScn = scn;
            m_ocAddress = (byte[])address.Clone();
            return m_OpenClientResult;
        }

        public void WaitOpenClientCalled()
        {
            bool signalled = m_OpenClientCalledEvent.WaitOne(30 * 1000, false);
            Assert.IsTrue(signalled, "Test Timeout");
        }

        public void SetOpenClientResult(PORT_RETURN_CODE ret)
        {
            m_OpenClientResult = ret;
        }

        public void AssertOpenClientNotCalled()
        {
            Assert.AreEqual(0, m_OpenClientCalled, "NOT OpenClientCalled");
        }

        public void AssertOpenClientCalledAndClear(byte[] address, byte channel)
        {
            Assert.AreEqual(1, m_OpenClientCalled, "OpenClientCalled");
            Assert.AreEqual(address, m_ocAddress, "Address");
            Assert.AreEqual(channel, m_ocScn, "Channel");
            //
            m_OpenClientCalled = 0;
            //...
        }

        //--------
        public PORT_RETURN_CODE Close()
        {
            ++m_CloseCalled;
            return PORT_RETURN_CODE.SUCCESS;
        }

        public void AssertCloseCalledOnce(string name)
        {
            Assert.AreEqual(1, m_CloseCalled, "CloseCalled -- " + name);
            //Assert.AreEqual(1, m_DestroyRfcommPortCalled, "DestroyRfcommPortCalled -- " + name);
        }

        public void AssertCloseCalledAtLeastOnce(string name)
        {
            Assert.Greater(m_CloseCalled, 0, "CloseCalled -- " + name);
            Assert.AreEqual(1, m_DestroyRfcommPortCalled, "DestroyRfcommPortCalled -- " + name);
        }

        //--------
        public void Create()
        {
            m_debugId = System.Threading.Interlocked.Increment(ref s_nextDebugId);
            //Console.WriteLine("TestRfcommPort.Created: " + DebugId);
        }

        static int s_nextDebugId;
        int m_debugId;

        public string DebugId { get { return m_debugId.ToString(); } }

        public void Destroy()
        {
            ++m_DestroyRfcommPortCalled;
        }

        //--------
        public virtual PORT_RETURN_CODE OpenServer(int scn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //--------
        bool m_IsConnectedReturnsFalse, m_IsConnectedThrows;

        public void SetIsConnectedReturnsFalse()
        {
            m_IsConnectedReturnsFalse = true;
        }

        public void SetIsConnectedThrows()
        {
            m_IsConnectedThrows = true;
        }

        public virtual bool IsConnected(out BluetoothAddress p_remote_bdaddr)
        {
            if (m_IsConnectedThrows)
                throw new RankException("SetIsConnectedThrows");
            else if (m_IsConnectedReturnsFalse) {
                p_remote_bdaddr = BluetoothAddress.Parse("99:99:99:99:99:99");
                return false;
            } else {
                p_remote_bdaddr = BluetoothAddress.Parse("00:11:22:33:44:55");
                return true;
            }
        }

    }//class2

}
