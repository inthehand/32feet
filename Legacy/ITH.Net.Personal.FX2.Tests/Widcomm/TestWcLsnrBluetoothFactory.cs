using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.Widcomm
{
    internal class TestWcLsnrBluetoothFactory : WidcommBluetoothFactoryBase
    {
        int _maxSdpService = 1;

        public int MaxSdpServices { get { return _maxSdpService; } set { _maxSdpService = value; } }

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
            return new WidcommBluetoothClient((WidcommRfcommStreamBase)acceptedStream, this);
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
            return new WidcommBluetoothListener(this);
        }

        protected override IBluetoothRadio GetPrimaryRadio()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override IBluetoothRadio[] GetAllRadios()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        //

        internal override WidcommBtInterface GetWidcommBtInterface()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override WidcommRfcommStreamBase GetWidcommRfcommStream()
        {
            return new WidcommRfcommStream(GetWidcommRfcommPort(), GetWidcommRfCommIf(), this);
        }

        internal override WidcommRfcommStreamBase GetWidcommRfcommStreamWithoutRfcommIf()
        {
            return new WidcommRfcommStream(GetWidcommRfcommPort(), null, this);
        }

        public Queue<IRfcommPort> queueIRfCommPort = new Queue<IRfcommPort>();
        internal override IRfcommPort GetWidcommRfcommPort()
        {
            lock (queueIRfCommPort) { // Is there any chance this is called concurrently.
                return queueIRfCommPort.Dequeue();
            }
        }

        public Queue<IRfCommIf> queueIRfCommIf = new Queue<IRfCommIf>();
        internal override IRfCommIf GetWidcommRfCommIf()
        {
            lock (queueIRfCommIf) { // Is there any chance this is called concurrently.
                return queueIRfCommIf.Dequeue();
            }
        }

        //----------------
        TestSdpService2 m_sdpSvc;
        int _countSdpService;

        internal override ISdpService GetWidcommSdpService()
        {
            if (_countSdpService >= _maxSdpService) {
                throw new InvalidOperationException("Uses more SdpService instances than expected.");
            }
            if (m_sdpSvc != null) {
                NUnit.Framework.Assert.AreEqual(1, m_sdpSvc.NumDisposeCalls, "TestSdpService2 NumDisposeCalls");
            }
            m_sdpSvc = new TestSdpService2();
            ++_countSdpService;
            return m_sdpSvc;
        }

        internal TestSdpService2 GetTestSdpService()
        {
            return m_sdpSvc;
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
