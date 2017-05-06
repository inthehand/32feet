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
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Bluetooth.Factory;
using NUnit.Framework;

namespace TtfTests.Droid
{
    [TestFixture]
    public class AndroidRadioTests
    {
        const string LocalAddrTxt = "00:11:22:AA:BB:CC";
        const string LocalName = "MyRadio1";
        const int CoD = 0;
        const RadioMode Mode = RadioMode.Connectable;
        const RadioModes Modes = RadioModes.PowerOn | RadioModes.Connectable;
#pragma warning disable 618 // warning CS0618: '...' is obsolete: '...'
        const Manufacturer Manu = Manufacturer.AndroidXxxx;
#pragma warning restore 618

        //-----------------------------------------
        [Test]
        public void RadioMisc()
        {
            var f = AndroidTestInfra.Init();
            var rList = f.DoGetAllRadios();
            Assert.AreEqual(1, rList.Length, "rList.Length");
            var r = rList[0];
            Assert.AreEqual(BluetoothAddress.Parse(LocalAddrTxt), r.LocalAddress, "r.LocalAddress");
            Assert.AreEqual(LocalName, r.Name, "r.Name");
            Assert.AreEqual(Manu, r.SoftwareManufacturer, "r.SoftwareManufacturer"); // HA-CK
            Assert.AreEqual(Manufacturer.Unknown, r.Manufacturer, "r.Manufacturer");
            Assert.AreEqual(new ClassOfDevice(CoD), r.ClassOfDevice, "r.Manufacturer");
            //
            Assert.AreEqual(Mode, r.Mode, "r.Mode");
            Assert.AreEqual(Modes, r.Modes, "r.Modes");
            Assert.AreEqual(HardwareStatus.Running, r.HardwareStatus, "r.HardwareStatus");
            //
            Assert.AreEqual(HciVersion.Unknown, r.HciVersion, "r.HciVersion");
            Assert.AreEqual(0, r.HciRevision, "r.HciRevision");
            //
            Assert.AreEqual(IntPtr.Zero, r.Handle, "r.Handle");
        }

        [Test]
        public void Radio2_OffConno()
        {
            var values = new AndroidMockValues
            {
                Radio_Address = "10:23:45:67:89:ab",
                Radio_Name = "radio2222",
                Radio_State = Android.Bluetooth.State.Off,
                Radio_ScanMode = Android.Bluetooth.ScanMode.Connectable,
            };
            var f = AndroidTestInfra.Init(values);
            var r = f.DoGetPrimaryRadio();
            Assert.AreEqual(BluetoothAddress.Parse(values.Radio_Address), r.LocalAddress, "r.LocalAddress");
            Assert.AreEqual(values.Radio_Name, r.Name, "r.Name");
            Assert.AreEqual(Manu, r.SoftwareManufacturer, "r.SoftwareManufacturer"); // HA-CK
            Assert.AreEqual(Manufacturer.Unknown, r.Manufacturer, "r.Manufacturer");
            Assert.AreEqual(new ClassOfDevice(0), r.ClassOfDevice, "r.Manufacturer");
            //
            Assert.AreEqual(RadioMode.PowerOff, r.Mode, "r.Mode");
            Assert.AreEqual(RadioModes.Connectable | RadioModes.PowerOff,
                r.Modes, "r.Modes");
            Assert.AreEqual(HardwareStatus.Shutdown, r.HardwareStatus, "r.HardwareStatus");
        }


        //---
        IBluetoothRadio TestRadioStates(RadioModes expectedModes, RadioMode expectedMode, HardwareStatus expectedStatus,
            Android.Bluetooth.State state, Android.Bluetooth.ScanMode scanMode)
        {
            var values = new AndroidMockValues
            {
                Radio_Address = "10:23:45:67:89:ab",
                Radio_Name = "radio2222",
                Radio_State = state,
                Radio_ScanMode = scanMode,
            };
            var f = AndroidTestInfra.Init(values);
            var r = f.DoGetPrimaryRadio();
            //
            Assert.AreEqual(expectedMode, r.Mode, "r.Mode");
            Assert.AreEqual(expectedModes, r.Modes, "r.Modes");
            Assert.AreEqual(expectedStatus, r.HardwareStatus, "r.HardwareStatus");
            return r;
        }

        [Test]
        public void RadioStatus_Off_Disco()
        {
            var r = TestRadioStates(RadioModes.PowerOff | RadioModes.Connectable | RadioModes.Discoverable,
                RadioMode.PowerOff, HardwareStatus.Shutdown,
                Android.Bluetooth.State.Off, Android.Bluetooth.ScanMode.ConnectableDiscoverable);
            // Other
            Assert.AreEqual(BluetoothAddress.Parse("10:23:45:67:89:ab"), r.LocalAddress, "r.LocalAddress");
            Assert.AreEqual("radio2222", r.Name, "r.Name");
            Assert.AreEqual(Manu, r.SoftwareManufacturer, "r.SoftwareManufacturer"); // HA-CK
            Assert.AreEqual(Manufacturer.Unknown, r.Manufacturer, "r.Manufacturer");
            Assert.AreEqual(new ClassOfDevice(0), r.ClassOfDevice, "r.Manufacturer");
        }

        [Test]
        public void RadioStatus_Off_Neither()
        {
            var r = TestRadioStates(RadioModes.PowerOff,
                RadioMode.PowerOff, HardwareStatus.Shutdown,
                Android.Bluetooth.State.Off, Android.Bluetooth.ScanMode.None);
        }

        [Test]
        public void RadioStatus_Wierd_Conno()
        {
            var r = TestRadioStates(RadioModes.PowerOff | RadioModes.Connectable,
                RadioMode.PowerOff, HardwareStatus.Unknown,
                (Android.Bluetooth.State)99, Android.Bluetooth.ScanMode.Connectable);
        }

        [Test]
        public void RadioStatus_On_Conno()
        {
            var r = TestRadioStates(RadioModes.PowerOn | RadioModes.Connectable,
                RadioMode.Connectable, HardwareStatus.Running,
                Android.Bluetooth.State.On, Android.Bluetooth.ScanMode.Connectable);
        }

        [Test]
        public void RadioStatus_On_Disco()
        {
            var r = TestRadioStates(RadioModes.PowerOn | RadioModes.Connectable | RadioModes.Discoverable,
                RadioMode.Discoverable, HardwareStatus.Running,
                Android.Bluetooth.State.On, Android.Bluetooth.ScanMode.ConnectableDiscoverable);
        }

        [Test]
        public void RadioStatus_On_Neither()
        {
            var r = TestRadioStates(RadioModes.PowerOn,
                RadioMode.PowerOff, // No good answer.
                HardwareStatus.Running,
                Android.Bluetooth.State.On, (Android.Bluetooth.ScanMode)99);
        }

        [Test]
        public void RadioStatus_On_Wierd()
        {
            var r = TestRadioStates(RadioModes.PowerOn,
                RadioMode.PowerOff, // No good answer.
                HardwareStatus.Running,
                Android.Bluetooth.State.On, (Android.Bluetooth.ScanMode)99);
        }

        [Test]
        public void Radio_TurningOff_Conno()
        {
            var r = TestRadioStates(RadioModes.PowerOff | RadioModes.Connectable,
                RadioMode.PowerOff,
                HardwareStatus.Shutdown,
                Android.Bluetooth.State.TurningOff, Android.Bluetooth.ScanMode.Connectable);
        }

        [Test]
        public void Radio_TurningOn_Disco()
        {
            var r = TestRadioStates(RadioModes.PowerOff | RadioModes.Connectable | RadioModes.Discoverable,
                RadioMode.PowerOff,
                HardwareStatus.Initializing,
                Android.Bluetooth.State.TurningOn, Android.Bluetooth.ScanMode.ConnectableDiscoverable);
        }

    }
}
#endif
