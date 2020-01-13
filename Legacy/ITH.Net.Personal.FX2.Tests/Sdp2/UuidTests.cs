using System;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;

namespace InTheHand.Net.Tests.Sdp2
{
    [TestFixture]
    public class UuidTests
    {

        [Test]
        public void NotExists()
        {
            Guid value = new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsNull(BluetoothService.GetName(value));
        }

        [Test]
        public void Empty()
        {
            // Should it special-case Empty and return null?
            Assert.AreEqual("Empty",
            //Assert.IsNull(
                BluetoothService.GetName(Guid.Empty));
        }

        [Test]
        public void OPP()
        {
            //Guid uuid = BluetoothService.ObexObjectPush;
            Guid uuid = new Guid(0x1105, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5f, 0x9b, 0x34, 0xfb);
            Assert.AreEqual("ObexObjectPush", BluetoothService.GetName(uuid));
        }

        [Test]
        public void VideoDistribution()
        {
            //Guid uuid = BluetoothService.VideoDistribution ;
            Guid uuid = new Guid(0x1305, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5f, 0x9b, 0x34, 0xfb);
            Assert.AreEqual("VideoDistribution", BluetoothService.GetName(uuid));
        }

        [Test]
        public void OPPu32()
        {
            UInt32 value = 0x1105;
            Assert.AreEqual("ObexObjectPush", BluetoothService.GetName(value));
        }

        [Test]
        public void OPPs32()
        {
            Int32 value = 0x1105;
            Assert.AreEqual("ObexObjectPush", BluetoothService.GetName(value));
        }

        [Test]
        public void OPPu16()
        {
            UInt16 value = 0x1105;
            Assert.AreEqual("ObexObjectPush", BluetoothService.GetName(value));
        }

        [Test]
        public void OPPs16()
        {
            Int16 value = 0x1105;
            Assert.AreEqual("ObexObjectPush", BluetoothService.GetName(value));
        }

    }//class

}
