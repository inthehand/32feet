// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

#if ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using Android.Bluetooth;
//
using A_DeviceClass = Android.Bluetooth.DeviceClass;
using A_ServiceClass = Android.Bluetooth.ServiceClass;
using T_DeviceClass = InTheHand.Net.Bluetooth.DeviceClass;
using T_ServiceClass = InTheHand.Net.Bluetooth.ServiceClass;


namespace TtfTests.Droid
{
    [TestFixture]
    public class AndroidCoDTests
    {
        static BluetoothClass Make(A_DeviceClass dc, A_ServiceClass sc)
        {
            return SimpleBluetoothClassMock.Create(dc, sc);
        }

        void DoTest(T_DeviceClass expectedDC, T_ServiceClass expectedSC,
            A_DeviceClass testDC, A_ServiceClass TestSC)
        {
            var abc = Make(testDC, TestSC);
            var cod = AndroidBthUtils.ConvertCoDs(abc);
            Assert.AreEqual(new ClassOfDevice(expectedDC, expectedSC), cod);
        }

        //--------
        [Test]
        public void Aa()
        {
            DoTest(T_DeviceClass.AudioVideoHiFi,
                T_ServiceClass.LimitedDiscoverableMode | T_ServiceClass.ObjectTransfer,
                A_DeviceClass.AudioVideoHifiAudio,
                A_ServiceClass.LimitedDiscoverability | A_ServiceClass.ObjectTransfer);
        }

        [Test]
        public void Ab()
        {
            DoTest(T_DeviceClass.ToyVehicle, T_ServiceClass.Network,
                A_DeviceClass.ToyVehicle, A_ServiceClass.Networking);
        }

        [Test]
        public void ZeroSC()
        {
            DoTest(T_DeviceClass.Computer, T_ServiceClass.None,
                A_DeviceClass.ComputerUncategorized, (A_ServiceClass)0);
            //
            var abc = Make(A_DeviceClass.ComputerUncategorized,
                (A_ServiceClass)0);
            var cod = AndroidBthUtils.ConvertCoDs(abc);
            Assert.AreEqual(new ClassOfDevice(T_DeviceClass.Computer,
                T_ServiceClass.None), cod);
        }

        [Test]
        public void ZeroBoth()
        {
            DoTest(T_DeviceClass.Miscellaneous, T_ServiceClass.None,
                (A_DeviceClass)0, (A_ServiceClass)0);
            //
            var abc = Make(A_DeviceClass.ComputerUncategorized,
                (A_ServiceClass)0);
            var cod = AndroidBthUtils.ConvertCoDs(abc);
            Assert.AreEqual(new ClassOfDevice(T_DeviceClass.Computer,
                T_ServiceClass.None), cod);
        }

    }
}
#endif
