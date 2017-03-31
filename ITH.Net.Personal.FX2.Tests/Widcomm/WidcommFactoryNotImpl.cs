using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.Widcomm
{
    class WidcommFactoryNotImpl : WidcommBluetoothFactoryBase
    {
        private static WidcommBluetoothFactoryBase s_Instance;

        public static WidcommBluetoothFactoryBase Instance
        {
            get
            {
                lock (typeof(WidcommFactoryNotImpl)) {
                    if (s_Instance == null)
                        s_Instance = new WidcommFactoryNotImpl();
                }
                return s_Instance;
            }
        }

        protected WidcommFactoryNotImpl()
        {
        }

        //----
        internal override WidcommBtInterface GetWidcommBtInterface()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal override WidcommRfcommStreamBase GetWidcommRfcommStream()
        {
            throw new Exception("The method or operation is not implemented.");
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
            throw new NotImplementedException();
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
