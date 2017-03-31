using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Bluetooth.Msft;

namespace InTheHand.Net.Tests.Widcomm
{
    [TestFixture]
    public class WidcommBluetoothDeviceInfoTest
    {
        WidcommBluetoothFactoryBase m_factory;

        public WidcommBluetoothDeviceInfoTest()
        {
            m_factory = WidcommFactoryNotImpl.Instance;
        }


        BluetoothAddress addr1_ = BluetoothAddress.Parse("10:22:33:44:55:66");
        BluetoothAddress addr2_ = BluetoothAddress.Parse("02:F3:E4:D5:C6:B7");
        public BluetoothAddress Addr1 { get { return (BluetoothAddress)addr1_.Clone(); } }
        public BluetoothAddress Addr2 { get { return (BluetoothAddress)addr2_.Clone(); } }
        byte[] addr1BigendianBytes = { 0x10, 0x22, 0x33, 0x44, 0x55, 0x66 };
        byte[] addr2BigendianBytes = { 0x02, 0xF3, 0xE4, 0xD5, 0xC6, 0xB7 };
        //
        const string name1 = "alAn";
        byte[] name1Bytes = { (byte)'a', (byte)'l', (byte)'A', (byte)'n', };
        const string name2 = "al\u00F6an";  // U+00F6 is o_umlaut
        byte[] name2Bytes = { (byte)'a', (byte)'l', 0xC3, 0xB6, (byte)'a', (byte)'n', };
        byte[] devClass1Bytes = { 3, 2, 4, };
        int devClass1 = 0x30204;
        byte[] devClass2Bytes = { 0xE, 0x1D, 0xEC, };
        int devClass2 = 0xE1DEC;

        //----
        private BluetoothDeviceInfo Create1_FromAddress()
        {
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(WidcommBluetoothDeviceInfo.CreateFromGivenAddressNoLookup(Addr1, m_factory));
            return bdi;
        }

        [Test]
        public void FromAddress()
        {
            BluetoothDeviceInfo bdi = Create1_FromAddress();
            Assert.IsFalse(bdi.Connected, "Connected");
            Assert.IsFalse(bdi.Remembered, "Remembered");
            Assert.IsFalse(bdi.Authenticated, "Authenticated");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            Assert.AreEqual(0, bdi.ClassOfDevice.ValueAsInt32, "ClassOfDevice");
            Assert.AreEqual(Addr1, bdi.DeviceAddress, "DeviceAddress");
            Assert.AreEqual(Addr1.ToString("C"), bdi.DeviceName, "DeviceName");
            //
            //
            bdi.Refresh(); // Refresh has no effect on Widcomm (yet).
            Assert.AreEqual(Addr1.ToString("C"), bdi.DeviceName, "DeviceName after Refresh");
            //
            bdi.DeviceName = "foobar";
            Assert.AreEqual("foobar", bdi.DeviceName, "DeviceName after set");
            //
            bdi.Refresh(); // Refresh has no effect on Widcomm (yet).
            Assert.AreEqual("foobar", bdi.DeviceName, "DeviceName after set&Refresh");
        }

        //----
        private BluetoothDeviceInfo Create1_FromInquiry_Connected()
        {
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(WidcommBluetoothDeviceInfo.CreateFromHandleDeviceResponded(
                WidcommUtils.FromBluetoothAddress(Addr1), name1Bytes, devClass1Bytes, true, m_factory));
            return bdi;
        }

        [Test]
        public void FromInquiry_Connected()
        {
            BluetoothDeviceInfo bdi = Create1_FromInquiry_Connected();
            Assert.IsTrue(bdi.Connected, "Connected");
            Assert.IsFalse(bdi.Remembered, "Remembered");
            Assert.IsFalse(bdi.Authenticated, "Authenticated");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            Assert.AreEqual(devClass1, bdi.ClassOfDevice.ValueAsInt32, "ClassOfDevice");
            // TO-DO check the CoD components (device major, minor and services)
            Assert.AreEqual(Addr1, bdi.DeviceAddress, "DeviceAddress");
            Assert.AreEqual(name1, bdi.DeviceName, "DeviceName");
        }

        //----
        private BluetoothDeviceInfo Create2_FromInquiry_NotConnected()
        {
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(WidcommBluetoothDeviceInfo.CreateFromHandleDeviceResponded(
                WidcommUtils.FromBluetoothAddress(Addr2), name2Bytes, devClass2Bytes, false, m_factory));
            return bdi;
        }

        [Test]
        public void FromInquiry_NotConnected()
        {
            BluetoothDeviceInfo bdi = Create2_FromInquiry_NotConnected();
            Assert.IsFalse(bdi.Connected, "Connected");
            Assert.IsFalse(bdi.Remembered, "Remembered");
            Assert.IsFalse(bdi.Authenticated, "Authenticated");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            Assert.AreEqual(devClass2, bdi.ClassOfDevice.ValueAsInt32, "ClassOfDevice");
            Assert.AreEqual(Addr2, bdi.DeviceAddress, "DeviceAddress");
            Assert.AreEqual(name2, bdi.DeviceName, "DeviceName");
            //
            //
            bdi.Refresh(); // Refresh has no effect on Widcomm (yet).
            Assert.AreEqual(name2, bdi.DeviceName, "DeviceName after set&Refresh");
        }

        //----
        private BluetoothDeviceInfo Create1_FromStoredRemoteDeviceInfo_ConnectedPairedSetFalse()
        {
            REM_DEV_INFO rdi;
            rdi.bda = addr1BigendianBytes;
            rdi.b_connected = true;
            rdi.b_paired = false;
            rdi.bd_name = name1Bytes;
            rdi.dev_class = devClass1Bytes;
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(WidcommBluetoothDeviceInfo.CreateFromStoredRemoteDeviceInfo(
                rdi, m_factory));
            return bdi;
        }

        [Test]
        public void FromStoredRemoteDeviceInfo_ConnectedPairedSetFalse()
        {
            BluetoothDeviceInfo bdi = Create1_FromStoredRemoteDeviceInfo_ConnectedPairedSetFalse();
            Assert.IsTrue(bdi.Connected, "Connected");
            Assert.IsTrue(bdi.Remembered, "Remembered");
            Assert.IsFalse(bdi.Authenticated, "Authenticated");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            Assert.AreEqual(devClass1, bdi.ClassOfDevice.ValueAsInt32, "ClassOfDevice");
            // TO-DO check the CoD components (device major, minor and services)
            Assert.AreEqual(Addr1, bdi.DeviceAddress, "DeviceAddress");
            Assert.AreEqual(name1, bdi.DeviceName, "DeviceName");
        }

        //----
        private BluetoothDeviceInfo Create2_FromStoredRemoteDeviceInfo_NotConnectedPairedSetTrue()
        {
            REM_DEV_INFO rdi;
            rdi.bda = addr2BigendianBytes;
            rdi.b_connected = false;
            rdi.b_paired = true;
            rdi.bd_name = name2Bytes;
            rdi.dev_class = devClass2Bytes;
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(WidcommBluetoothDeviceInfo.CreateFromStoredRemoteDeviceInfo(
                rdi, m_factory));
            return bdi;
        }

        [Test]
        public void FromStoredRemoteDeviceInfo_NotConnectedPairedSetTrue()
        {
            BluetoothDeviceInfo bdi = Create2_FromStoredRemoteDeviceInfo_NotConnectedPairedSetTrue();
            Assert.IsFalse(bdi.Connected, "Connected");
            Assert.IsTrue(bdi.Remembered, "Remembered");
            Assert.IsTrue(bdi.Authenticated, "Authenticated");
            Assert.AreEqual(DateTime.MinValue, bdi.LastSeen, "LastSeen");
            Assert.AreEqual(DateTime.MinValue, bdi.LastUsed, "LastUsed");
            Assert.AreEqual(devClass2, bdi.ClassOfDevice.ValueAsInt32, "ClassOfDevice");
            Assert.AreEqual(Addr2, bdi.DeviceAddress, "DeviceAddress");
            Assert.AreEqual(name2, bdi.DeviceName, "DeviceName");
        }

        [Test]
        public void Equals()
        {
            Assert.AreEqual(Create1_FromAddress(), Create1_FromInquiry_Connected(), "1_addr_inquiry");
            Assert.AreEqual(Create1_FromAddress(), Create1_FromStoredRemoteDeviceInfo_ConnectedPairedSetFalse(),
                "1_addr_stored");
            Assert.AreEqual(Create1_FromInquiry_Connected(), Create1_FromStoredRemoteDeviceInfo_ConnectedPairedSetFalse(),
                "1_inquiry_stored");
            Assert.AreNotEqual(Create1_FromAddress(), Create2_FromInquiry_NotConnected(), "1addr_2inquiry");
            Assert.AreNotEqual(Create1_FromAddress(), Create2_FromStoredRemoteDeviceInfo_NotConnectedPairedSetTrue(),
                "1addr_2stored");
            //
            Assert.AreEqual(Create1Windows_FromAddress(), Create1_FromInquiry_Connected(), "1_addrWindows_inquiry");
            Assert.AreNotEqual(Create1Windows_FromAddress(), Create2_FromInquiry_NotConnected(), "1_addrWindows_inquiry");
        }

        //--------

        private BluetoothDeviceInfo Create1Windows_FromAddress()
        {
            BluetoothDeviceInfo bdi = new BluetoothDeviceInfo(new WindowsBluetoothDeviceInfo(Addr1));
            return bdi;
        }

    }
}
