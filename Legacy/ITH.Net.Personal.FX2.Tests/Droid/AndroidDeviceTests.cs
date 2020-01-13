// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

#if ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using NUnit.Framework;
using Moq;
using InTheHand.Net.Bluetooth.Droid;

namespace TtfTests.Droid
{
    [TestFixture]
    public class AndroidDeviceTests
    {
        const string Dev1AddrTxt = "00:91:A2:2A:3B:4C";
        const string Dev1Name = "TheRemoteDeviceName";
        static readonly ClassOfDevice Dev1CoD = new ClassOfDevice(DeviceClass.MedicalPulseOximeter,
            ServiceClass.LimitedDiscoverableMode | ServiceClass.ObjectTransfer);

        //-----------------------------------------
        [Test]
        public void Dev1()
        {
            var f = AndroidTestInfra.Init();
            var bdi = f.DoGetBluetoothDeviceInfo(BluetoothAddress.Parse(Dev1AddrTxt));
            //
            Assert.AreEqual(BluetoothAddress.Parse(Dev1AddrTxt), bdi.DeviceAddress, "r.LocalAddress");
            Assert.AreEqual(Dev1Name, bdi.DeviceName, "r.DeviceName");
            Assert.AreEqual(Dev1CoD, bdi.ClassOfDevice, "r.ClassOfDevice");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "r.LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "r.LastUsed");
            Assert.AreEqual(false, bdi.Remembered, "r.Remembered");
            Assert.AreEqual(false, bdi.Authenticated, "r.Authenticated");
            Assert.AreEqual(false, bdi.Connected, "r.Connected");
            try { bdi.GetVersions(); } catch (NotSupportedException) { }
            Assert.AreEqual(int.MinValue, bdi.Rssi, "r.Rssi");
        }

        [Test]
        public void Dev2_RadioDisabledThusNullDeviceBluetoothClass()
        {
            var dMock = new Mock<Android.Bluetooth.BluetoothDevice>();
            var dev = dMock.Object;
            //
            var aMock = new Mock<Android.Bluetooth.BluetoothAdapter>();
            aMock.Setup(x => x.GetRemoteDevice(It.IsAny<String>()))
                .Returns(dev);
            //
            // Radio init
            aMock.Setup(x => x.Address).Returns("0000000000aa");
            if (true) {
                // Device Init
                // TODO Have AndroidBtCli not need BondState/Class
                dMock.Setup(x => x.Address).Returns(Dev1AddrTxt);
                dMock.Setup(x => x.BondState).Returns((Android.Bluetooth.Bond)999);
                dMock.Setup(x => x.BluetoothClass).Returns((Android.Bluetooth.BluetoothClass)null);
                // Adapter init
                aMock.Setup(x => x.GetRemoteDevice(
                                    It.IsAny<string>())) //TODO
                    .Returns(dMock.Object);
            }
            //
            var a = aMock.Object;
            //
            //
            //
            //var f = AndroidTestInfra.Init(/*getsBluetoothClass:*/ false);
            var fMock = new Mock<AndroidBthFactoryBase>(MockBehavior.Loose, a) { CallBase = true, };
            var f = fMock.Object;
            var bdi = f.DoGetBluetoothDeviceInfo(BluetoothAddress.Parse(Dev1AddrTxt));
            //
            Assert.AreEqual(BluetoothAddress.Parse(Dev1AddrTxt), bdi.DeviceAddress, "r.LocalAddress");
            //Assert.AreEqual(Dev1Name, bdi.DeviceName, "r.DeviceName");
            Assert.AreEqual(new ClassOfDevice(0), bdi.ClassOfDevice, "r.ClassOfDevice");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "r.LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "r.LastUsed");
            Assert.AreEqual(false, bdi.Remembered, "r.Remembered");
            Assert.AreEqual(false, bdi.Authenticated, "r.Authenticated");
            Assert.AreEqual(false, bdi.Connected, "r.Connected");
            try { bdi.GetVersions(); } catch (NotSupportedException) { }
            Assert.AreEqual(int.MinValue, bdi.Rssi, "r.Rssi");
        }

    }
}
#endif
