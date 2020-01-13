using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.Widcomm
{
    class WidcommFactoryGivenInstances : WidcommBluetoothFactoryBase
    {
        WidcommBtInterface m_btIf;
        Queue<WidcommRfcommStreamBase> m_strmQ = new Queue<WidcommRfcommStreamBase>();

        public void SetBtInterface(WidcommBtInterface btIf)
        {
            if (m_btIf != null)
                throw new InvalidOperationException("Only can be one WidcommBtInterface.");
            m_btIf = btIf;
        }

        public void AddRfcommStream(WidcommRfcommStreamBase strm)
        {
            m_strmQ.Enqueue(strm);
        }

        //----
        internal override WidcommBtInterface GetWidcommBtInterface()
        {
            return m_btIf;
        }

        internal override WidcommRfcommStreamBase GetWidcommRfcommStream()
        {
            return m_strmQ.Dequeue();
        }

        internal override WidcommRfcommStreamBase GetWidcommRfcommStreamWithoutRfcommIf()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override IRfcommPort GetWidcommRfcommPort()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override IRfCommIf GetWidcommRfCommIf()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override ISdpService GetWidcommSdpService()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothClient GetBluetoothClient()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothClient GetBluetoothClient(System.Net.Sockets.Socket acceptedSocket)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        protected override IBluetoothClient GetBluetoothClientForListener(CommonRfcommStream acceptedStream)
        {
            throw new NotSupportedException();
        }

        protected override IBluetoothClient GetBluetoothClient(BluetoothEndPoint localEP)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothDeviceInfo GetBluetoothDeviceInfo(BluetoothAddress address)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothListener GetBluetoothListener()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothRadio GetPrimaryRadio()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothRadio[] GetAllRadios()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothSecurity GetBluetoothSecurity()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override WidcommPortSingleThreader GetSingleThreader()
        {
            return null;
        }

        internal override bool IsWidcommSingleThread()
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        internal override void EnsureLoaded()
        {
            throw new NotImplementedException();
        }
    }
}
