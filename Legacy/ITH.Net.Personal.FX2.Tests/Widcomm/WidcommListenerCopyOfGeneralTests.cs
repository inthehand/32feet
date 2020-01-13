using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Factory;

namespace InTheHand.Net.Tests.Widcomm
{

    [Explicit]
    public class WcListenerMisc : InTheHand.Net.Tests.Bluetooth.ListenerMisc
    {
        public override void Init()
        {
            BluetoothFactory.SetFactory(new TestWcLsnrManyBluetoothFactory());
        }
    }

    [Explicit]
    public class WcListenerSdpRecordAuto : InTheHand.Net.Tests.Bluetooth.ListenerSdpRecordAuto
    {
        public override void Init()
        {
            BluetoothFactory.SetFactory(new TestWcLsnrManyBluetoothFactory());
        }
    }

    [Explicit]
    public class WcListenerSdpRecordByteArrayGiven : InTheHand.Net.Tests.Bluetooth.ListenerSdpRecordByteArrayGiven
    {
        public override void Init()
        {
            BluetoothFactory.SetFactory(new TestWcLsnrManyBluetoothFactory());
        }
    }

    [Explicit]
    public class WcListenerSdpRecordServiceRecordGiven : InTheHand.Net.Tests.Bluetooth.ListenerSdpRecordServiceRecordGiven
    {
        public override void Init()
        {
            BluetoothFactory.SetFactory(new TestWcLsnrManyBluetoothFactory());
        }
    }

}
