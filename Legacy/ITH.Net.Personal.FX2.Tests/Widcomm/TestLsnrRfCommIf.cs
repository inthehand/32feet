using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestLsnrRfCommIf : TestRfCommIf
    {
        static object m_lock = new object();
        static int m_lastScn = 20;

        public override int GetScn()
        {
            lock (m_lock) {
                ++m_lastScn;
                if (m_lastScn > 30)
                    m_lastScn = 1;
                return m_lastScn;
            }
        }
    }
}
