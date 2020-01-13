using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth.Widcomm;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommUtilsTest
    {
        [Test]
        public void FromBluetoothAddress()
        {
            BluetoothAddress addr = BluetoothAddress.Parse("00:11:22:33:44:55");
            byte[] bdaddr = WidcommUtils.FromBluetoothAddress(addr);
            Assert.AreEqual(new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55 }, bdaddr ,"1");
            addr = BluetoothAddress.Parse("0F:1E:2D:3C:4B:5A");
            bdaddr = WidcommUtils.FromBluetoothAddress(addr);
            Assert.AreEqual(new byte[] { 0x0F, 0x1E, 0x2D, 0x3C, 0x4B, 0x5A }, bdaddr, "2");
        }

        [Test]
        public void ToBluetoothAddress()
        {
            byte[] bdaddr = new byte[] { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55 };
            BluetoothAddress addr = WidcommUtils.ToBluetoothAddress(bdaddr);
            Assert.AreEqual(BluetoothAddress.Parse("00:11:22:33:44:55"), addr, "1");
            bdaddr = new byte[] { 0x0F, 0x1E, 0x2D, 0x3C, 0x4B, 0x5A };
            addr = WidcommUtils.ToBluetoothAddress(bdaddr);
            Assert.AreEqual(BluetoothAddress.Parse("0F:1E:2D:3C:4B:5A"), addr, "2");
        }
    }
}
