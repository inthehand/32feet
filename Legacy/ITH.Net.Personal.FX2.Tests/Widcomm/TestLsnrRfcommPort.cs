using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using NUnit.Framework;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestLsnrRfcommPort : TestRfcommPort
    {
        int m_OpenServerCalled;
        PORT_RETURN_CODE m_OpenServerResult = (PORT_RETURN_CODE)99;

        public override PORT_RETURN_CODE OpenServer(int scn)
        {
            ++m_OpenServerCalled;
            return m_OpenServerResult;
        }

        public void SetOpenServerResult(PORT_RETURN_CODE ret)
        {
            m_OpenServerResult = ret;
        }

        public void AssertOpenServerNotCalled()
        {
            Assert.AreEqual(0, m_OpenServerCalled, "NOT OpenServerCalled");
        }

        public void AssertOpenServerCalledAndClear(byte channel)
        {
            Assert.AreEqual(1, m_OpenServerCalled, "OpenServerCalled");
            //Assert.AreEqual(address, m_ocAddress, "Address");
            //Assert.AreEqual(channel, m_ocScn, "Channel");
            //
            m_OpenServerCalled = 0;
            //...
        }

    }
}
