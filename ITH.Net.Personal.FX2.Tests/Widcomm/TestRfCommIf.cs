using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using NUnit.Framework;
using System.Diagnostics;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestRfCommIf : IRfCommIf
    {
        int m_DestroyRfcommIfCalled;
        BTM_SEC? m_SSL_secLevel;
        bool? m_SSL_isServer;

        IntPtr IRfCommIf.PObject { get { throw new NotImplementedException(); } }

        public void Create()
        {
            //throw new NotImplementedException();
        }
        public void Destroy(bool disposing)
        {
            ++m_DestroyRfcommIfCalled;
        }
        //
        const bool SUCCESS = true;
        public bool ClientAssignScnValue(Guid p_service_guid, int scn)
        {
            //throw new NotImplementedException();
            return SUCCESS;
        }
        public bool SetSecurityLevel(byte[] p_service_name, BTM_SEC security_level, bool is_server)
        {
            Debug.Assert(!m_SSL_secLevel.HasValue, "Duplicate call to SetSecurityLevel!");
            m_SSL_secLevel = security_level;
            m_SSL_isServer = is_server;
            return SUCCESS;
        }

        public virtual int GetScn()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //----
        internal void AssertDestroyCalledOnce()
        {
            Assert.AreEqual(1, m_DestroyRfcommIfCalled, "DestroyRfcommIfCalled");
        }

        internal void AssertSetSecurityLevel(BTM_SEC expectedSecurityLevel, bool expectedIsServer)
        {
            Assert.AreEqual(expectedSecurityLevel, m_SSL_secLevel, "SetSecurityLevel-BTM_SEC");
            Assert.AreEqual(expectedIsServer, m_SSL_isServer, "SetSecurityLevel-isServer");
        }
    }

}
