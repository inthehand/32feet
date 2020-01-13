using System;
using InTheHand.Net.Bluetooth;
using NUnit.Framework;

namespace InTheHand.Net.Tests.BluetoothTesting
{
    [TestFixture]
    public class ClassOfDeviceTests
    {
        [Test]
        public void Ctor2_A()
        {
            var cod = new ClassOfDevice(DeviceClass.CellPhone, ServiceClass.Audio);
            Assert.AreEqual(DeviceClass.CellPhone, cod.Device, "D");
            Assert.AreEqual(ServiceClass.Audio, cod.Service, "S");
            //
            Assert.AreEqual(DeviceClass.Phone, cod.MajorDevice, "MD");
        }

        [Test]
        public void Ctor2_NoSc()
        {
            var cod = new ClassOfDevice(DeviceClass.CellPhone, ServiceClass.None);
            Assert.AreEqual(DeviceClass.CellPhone, cod.Device, "D");
            Assert.AreEqual(ServiceClass.None, cod.Service, "S");
            //
            Assert.AreEqual(DeviceClass.Phone, cod.MajorDevice, "MD");
        }

    }
}
