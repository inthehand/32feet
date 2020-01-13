#if BLUETOPIA
using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.StonestreetOne;
using NMock2;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using NMock2.Actions;
using System.Diagnostics;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaDeviceInfoTests
    {
        private void ExpectQueryRemoteName(StuffStackBluetopia stuff,
            BluetoothAddress expectedAddr, BluetopiaError whatRet)
        {
            Int64 expectedAddrBytes = expectedAddr.ToInt64();
            Expect.Once.On(stuff.MockedApi)
                .Method("GAP_Query_Remote_Device_Name")
                .With(stuff.StackId, expectedAddrBytes, Is.Anything, Is.Anything)
                .Will(Return.Value(whatRet));
        }

        //-------------------
        [Test]
        public void NoAccessToDeviceName()
        {
            var addr = BluetoothAddress.Parse("001122334455");
            //
            var stuff = new StuffStackBluetopia();
            BluetopiaTesting.InitMockery(stuff, 2);
            //
            var bdi = stuff.GetFactory().DoGetBluetoothDeviceInfo(
                (BluetoothAddress)addr.Clone());
            //
            Assert.AreEqual(addr, bdi.DeviceAddress, "DeviceAddress");
            Assert.IsFalse(bdi.Remembered, "Remembered");
            Assert.IsFalse(bdi.Authenticated, "Authenticated");
            Assert.IsFalse(bdi.Connected, "Connected");
            Assert.AreEqual(new ClassOfDevice(0), bdi.ClassOfDevice, "ClassOfDevice");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void DeviceNameErrorOnQuery_NoCallback_DoesntRefresh()
        {
            var addr = BluetoothAddress.Parse("001122334455");
            var addrBytes8 = addr.ToByteArray();
            var addrBytes = new byte[6];
            Array.Copy(addrBytes8, 0, addrBytes, 0, addrBytes.Length); // To-Do what endianism?
            //
            var stuff = new StuffStackBluetopia();
            BluetopiaTesting.InitMockery(stuff, 2);
            //
            var bdi = stuff.GetFactory().DoGetBluetoothDeviceInfo(
                (BluetoothAddress)addr.Clone());
            //
            Assert.AreEqual(addr, bdi.DeviceAddress, "DeviceAddress");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ExpectQueryRemoteName(stuff, addr, BluetopiaError.OK);
            Assert.AreEqual(addr.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            // When we get an error we set the name to the address and DO NOT
            // call GAP_Query_Remote_Device_Name again.
            Assert.AreEqual(addr.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        [Explicit]
        public void DeviceNameErrorOnQuery_NOCALLBACK_DoRefresh()
        {
            var addr = BluetoothAddress.Parse("001122334455");
            //
            var stuff = new StuffStackBluetopia();
            BluetopiaTesting.InitMockery(stuff, 2);
            //
            var bdi = stuff.GetFactory().DoGetBluetoothDeviceInfo((BluetoothAddress)addr.Clone());
            //
            Assert.AreEqual(addr, bdi.DeviceAddress, "DeviceAddress");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ExpectQueryRemoteName(stuff, addr, BluetopiaError.OK);
            Assert.AreEqual(addr.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            // When we get an error we set the name to the address and DO NOT
            // call GAP_Query_Remote_Device_Name again.
            // Unless we hit Refresh....
            bdi.Refresh();
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            // TO-DO This fails because the previous lookup is still pending
            // (no callback occurred) so we don't start another query!)
            ExpectQueryRemoteName(stuff, addr, BluetopiaError.OK);
            Assert.AreEqual(addr.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void One()
        {
            var addr = BluetoothAddress.Parse("001122334455");
            //
            var stuff = new StuffStackBluetopia();
            BluetopiaTesting.InitMockery(stuff, 2);
            //
            var bdi = stuff.GetFactory().DoGetBluetoothDeviceInfo(
                (BluetoothAddress)addr.Clone());
            //
            Assert.AreEqual(addr, bdi.DeviceAddress, "DeviceAddress");
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
            ExpectQueryRemoteName(stuff, addr, BluetopiaError.OK);
            Assert.AreEqual(addr.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

    }
}
#endif