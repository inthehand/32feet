using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth.Widcomm;

namespace InTheHand.Net.Tests.Widcomm
{
    class TestWcLsnrManyBluetoothFactory : TestWcLsnrBluetoothFactory
    {
        internal override IRfCommIf GetWidcommRfCommIf()
        {
            return new TestLsnrRfCommIf();
        }

        internal override IRfcommPort GetWidcommRfcommPort()
        {
            return new TestLsnrRfcommPort();
        }

        internal override ISdpService GetWidcommSdpService()
        {
            return new TestSdpService2();
        }
    }
}
