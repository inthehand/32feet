#if BLUETOPIA
using System;
using NUnit.Framework;
using NMock2;
using NMock2.Actions;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.StonestreetOne;
using InTheHand.Net.Tests.Infra;

namespace InTheHand.Net.Tests.BluetopiaTests
{
    [TestFixture]
    public class BluetopiaRadioTests
    {
        [Test]
        public void Hack_TestAlignments()
        {
            new Structs.GAP_Authentication_Event_Data__Status(new Version());
            new Structs.GAP_Authentication_Information(StackConsts.GAP_Authentication_Type_t.atLinkKey);
        }


        class StuffRadio : StuffStackBluetopia
        {
            public IBluetoothRadio Radio { get; set; }
        }

        private class Params
        {
            public bool AccessesVersionsOrManufacturer { get; set; }
        }

        static StuffRadio Create_Radio_A(Params p)
        {
            var stuff = new StuffRadio();
            //.
            const uint StackId = 1;
            var addrBytes = new byte[] { 0xA4, 0x24, 0x4C, 0x98, 0x24, 0x08 };
            var ExpectedAddress = BluetoothAddress.Parse("08:24:98:4C:24:A4");
            var nameBytes = new byte[] { (byte)'A', (byte)'b', (byte)'c', 0 };
            var ExpectedName = "Abc";
            var codStruct = (uint)0x20104;
            var ExpectedCod = new ClassOfDevice(0x20104);
            //
            BluetopiaTesting.InitMockery(stuff, StackId);
            Expect.Once.On(stuff.MockedApi).Method("GAP_Query_Local_BD_ADDR")
                .With(StackId, new byte[6])
                .Will(
                    FillArrayIndexedParameterAction.Fill(1, addrBytes, true),
                    Return.Value(BluetopiaError.OK));
            Expect.Once.On(stuff.MockedApi).Method("GAP_Query_Local_Device_Name")
                .With(StackId, 248, new byte[248])
                .Will(
                    FillArrayIndexedParameterAction.Fill(2, nameBytes, false),
                    Return.Value(BluetopiaError.OK));
            Expect.Once.On(stuff.MockedApi).Method("GAP_Query_Class_Of_Device")
                .With(StackId, Is.Out)
                .Will(
                    new SetIndexedParameterAction(1, codStruct),
                    Return.Value(BluetopiaError.OK));
            if (p.AccessesVersionsOrManufacturer) {
                Expect.Once.On(stuff.MockedApi).Method("HCI_Read_Local_Version_Information")
                    .With(StackId,
                        Is.Out, Is.Out, Is.Out, Is.Out, Is.Out, Is.Out)
                    .Will(
                        new SetIndexedParameterAction(1, (StackConsts.HCI_ERROR_CODE)0),
                        new SetIndexedParameterAction(2, (HciVersion)0),
                        new SetIndexedParameterAction(3, (UInt16)0),
                        new SetIndexedParameterAction(4, (LmpVersion)0),
                        new SetIndexedParameterAction(5, Manufacturer.Zeevo),
                        new SetIndexedParameterAction(6, (UInt16)0),
                        Return.Value(BluetopiaError.OK));
            }
            //--
            BluetopiaTesting.HackAllowShutdownCall(stuff.MockedApi);
            BluetopiaFactory fcty = new BluetopiaFactory(stuff.MockedApi);
            IBluetoothRadio r = stuff.Radio = fcty.DoGetPrimaryRadio();
            Assert.AreEqual(Manufacturer.StonestreetOne, r.SoftwareManufacturer, "SoftwareManufacturer");
            Assert.AreEqual(ExpectedAddress, r.LocalAddress, "LocalAddress");
            Assert.AreEqual(ExpectedName, r.Name, "Name");
            Assert.AreEqual(ExpectedCod, r.ClassOfDevice, "ClassOfDevice");
            //
            return stuff;
        }

        [Test]
        public void A()
        {
            var stuff = Create_Radio_A(new Params { AccessesVersionsOrManufacturer = true });
            Assert.AreEqual((IntPtr)1, stuff.Radio.Handle, "Handle");
            Assert.AreEqual(HardwareStatus.Running, stuff.Radio.HardwareStatus, "HardwareStatus");
            Assert.AreEqual(Manufacturer.Zeevo, stuff.Radio.Manufacturer, "Manufacturer");
            Assert.AreEqual(null, stuff.Radio.Remote, "Remote");
            Assert.AreEqual(0, stuff.Radio.LmpSubversion, "LmpSubversion");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void A_GetMode_YesNo()
        {
            A_GetMode_(StackConsts.GAP_Connectability_Mode.ConnectableMode,
                StackConsts.GAP_Discoverability_Mode.NonDiscoverableMode,
                RadioMode.Connectable);
        }

        [Test]
        public void A_GetMode_YesLimited()
        {
            A_GetMode_(StackConsts.GAP_Connectability_Mode.ConnectableMode,
                StackConsts.GAP_Discoverability_Mode.LimitedDiscoverableMode,
                RadioMode.Connectable);
        }

        [Test]
        public void A_GetMode_YesYes()
        {
            A_GetMode_(StackConsts.GAP_Connectability_Mode.ConnectableMode,
                StackConsts.GAP_Discoverability_Mode.GeneralDiscoverableMode,
                RadioMode.Discoverable);
        }

        [Test]
        public void A_GetMode_NoYes()
        {
            A_GetMode_(StackConsts.GAP_Connectability_Mode.NonConnectableMode,
                StackConsts.GAP_Discoverability_Mode.GeneralDiscoverableMode,
                RadioMode.PowerOff);
        }

        [Test]
        public void A_GetMode_NoNo()
        {
            A_GetMode_(StackConsts.GAP_Connectability_Mode.NonConnectableMode,
                StackConsts.GAP_Discoverability_Mode.NonDiscoverableMode,
                RadioMode.PowerOff);
        }

        static void A_GetMode_(StackConsts.GAP_Connectability_Mode conno,
            StackConsts.GAP_Discoverability_Mode disco,
            RadioMode expectedMode)
        {
            var stuff = Create_Radio_A(new Params { });
            //
            Expect.Once.On(stuff.MockedApi).Method("GAP_Query_Connectability_Mode")
                .With(stuff.StackId, Is.Out)
                .Will(
                    new SetIndexedParameterAction(1, conno),
                    Return.Value(BluetopiaError.OK));
            if (conno == StackConsts.GAP_Connectability_Mode.ConnectableMode) {
                // (We don't call GAP_Query_Discoverability_Mode when not conno).
                Expect.Once.On(stuff.MockedApi).Method("GAP_Query_Discoverability_Mode")
                    .With(stuff.StackId, Is.Out, Is.Out)
                    .Will(
                        new SetIndexedParameterAction(1, disco),
                        new SetIndexedParameterAction(2, (uint)0),
                        Return.Value(BluetopiaError.OK));
            }
            //
            Assert.AreEqual(expectedMode, stuff.Radio.Mode, "Mode");
            //
            stuff.Mockery_VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void InitError()
        {
            var mocks = new Mockery();
            var api = mocks.NewMock<IBluetopiaApi>();
            // Second time after try-kill BTExplorer.exe
            Expect.Exactly(2).On(api).Method("BSC_Initialize")
                .WithAnyArguments()  // TO-DO
                .Will(Return.Value((int)BluetopiaError.UNSUPPORTED_PLATFORM_ERROR));
            //----
            try {
                BluetopiaTesting.HackAllowShutdownCall(api);
                BluetopiaFactory fcty = new BluetopiaFactory(api);
                Assert.Fail("should have thrown!");
            } catch (BluetopiaSocketException ex) {
                Assert.AreEqual((int)BluetopiaError.UNSUPPORTED_PLATFORM_ERROR, ex.BluetopiaErrorCode, "BluetopiaErrorCode");
                Assert.AreEqual(BluetopiaError.UNSUPPORTED_PLATFORM_ERROR.ToString(), ex.BluetopiaError, "BluetopiaError");
                Assert.AreEqual(
                    // was: "An operation was attempted on something that is not a socket"
                    "The attempted operation is not supported for the type of object referenced"
                    + " (Bluetopia: UNSUPPORTED_PLATFORM_ERROR (-101)).",
                    ex.Message, "Message");
            }
            //
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        //[Explicit("Causes Debug.Assert")]
        public void InitZeroHandle()
        {
            const uint BadZeroStackId = 0;
            var mocks = new Mockery();
            var api = mocks.NewMock<IBluetopiaApi>();
            Expect.Exactly(2).On(api).Method("BSC_Initialize")
                .WithAnyArguments()  // TO-DO
                .Will(Return.Value((int)BadZeroStackId));
            //----
            try {
                BluetopiaTesting.HackAllowShutdownCall(api);
                BluetopiaFactory fcty = new BluetopiaFactory(api);
                Assert.Fail("should have thrown!");
            } catch (BluetopiaSocketException ex) {
                Assert.AreEqual((int)BluetopiaError.OK, ex.BluetopiaErrorCode, "BluetopiaErrorCode");
                Assert.AreEqual(BluetopiaError.OK.ToString(), ex.BluetopiaError, "BluetopiaError");
            }
            //
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

    }
}
#endif
