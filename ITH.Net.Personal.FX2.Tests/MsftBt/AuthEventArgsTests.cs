using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Msft;

namespace InTheHand.Net.Tests.MsftBt
{
    [TestFixture]
    public class AuthEventArgsTests
    {
        BluetoothAddress addr1 = BluetoothAddress.Parse("00112233445566");

        private BluetoothWin32AuthenticationEventArgs Create_EA(BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS cbParams)
        {
            BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS? cbParamsRef = cbParams;
            var dev = new BLUETOOTH_DEVICE_INFO(addr1);
            var e = new BluetoothWin32AuthenticationEventArgs(dev, ref cbParamsRef);
            return e;
        }

        //----
        void DoTestSsbProps(BluetoothAuthenticationMethod am, BluetoothAuthenticationRequirements req,
            BluetoothIoCapability ioCapa, bool? expectedJustWorksValue)
        {
            var cbParams = new BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS();
            cbParams.authenticationMethod = am;
            cbParams.authenticationRequirements = req;
            cbParams.ioCapability = ioCapa;
            var e = Create_EA(cbParams);
            //
            Assert.AreEqual(am, e.AuthenticationMethod, "am");
            Assert.AreEqual(req, e.AuthenticationRequirements, "req");
            Assert.AreEqual(ioCapa, e.IoCapability, "ioCapa");
            Assert.AreEqual(expectedJustWorksValue, e.JustWorksNumericComparison, "jw");
        }

        //----

        [Test]
        public void NcHandler()
        {
            BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS cbParams
                = new BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS();
            cbParams.authenticationMethod = BluetoothAuthenticationMethod.NumericComparison;
            cbParams.authenticationRequirements = BluetoothAuthenticationRequirements.MITMProtectionRequired;
            cbParams.Numeric_Value_Passkey = 0;
            //
            BluetoothWin32AuthenticationEventArgs e;
            //
            cbParams.Numeric_Value_Passkey = 123456;
            e = Create_EA(cbParams);
            Assert.AreEqual(123456, e.NumberOrPasskey, "Num 123456");
            Assert.AreEqual("123 456", e.NumberOrPasskeyAsString, "NumToS6 123456");
            //
            cbParams.Numeric_Value_Passkey = 987654;
            e = Create_EA(cbParams);
            Assert.AreEqual(987654, e.NumberOrPasskey, "Num 987654");
            Assert.AreEqual("987 654", e.NumberOrPasskeyAsString, "NumToS6 987654");
            //
            cbParams.Numeric_Value_Passkey = 0;
            e = Create_EA(cbParams);
            Assert.AreEqual(0, e.NumberOrPasskey, "Num 0");
            Assert.AreEqual("000 000", e.NumberOrPasskeyAsString, "NumToS6 0");
            //
            //
            e = new BluetoothWin32AuthenticationEventArgs();
            Assert.IsNull(e.NumberOrPasskey, "Num ea..ctor()");
            Assert.IsNull(e.NumberOrPasskeyAsString, "NumToS6 ea..ctor()");
        }

        [Test]
        public void OobHandler()
        {
            BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS cbParams
                = new BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS();
            cbParams.authenticationMethod = BluetoothAuthenticationMethod.OutOfBand;
            //
            var e = Create_EA(cbParams);
            //
            e.Confirm = null;
            e.ConfirmOob(null, null);
            Assert.AreEqual(true, e.Confirm);
            Assert.IsNull(e.OobC, "OobC");
            Assert.IsNull(e.OobR, "OobR");
            //
            e.Confirm = null;
            e.ConfirmOob(null, new byte[16]);
            Assert.AreEqual(true, e.Confirm);
            Assert.IsNull(e.OobC, "OobC");
            Assert.IsNotNull(e.OobR, "OobR");
            Assert.AreEqual(16, e.OobR.Length, "OobR");
            //
            e.Confirm = null;
            e.ConfirmOob(new byte[16], null);
            Assert.AreEqual(true, e.Confirm);
            Assert.IsNotNull(e.OobC, "OobC");
            Assert.AreEqual(16, e.OobC.Length, "OobC.Len");
            Assert.IsNull(e.OobR, "OobR");
            //
            e.Confirm = null;
            e.ConfirmOob(new byte[16], new byte[16]);
            Assert.AreEqual(true, e.Confirm);
            Assert.IsNotNull(e.OobC, "OobC");
            Assert.AreEqual(16, e.OobC.Length, "OobC.Len");
            Assert.IsNotNull(e.OobR, "OobR");
            Assert.AreEqual(16, e.OobR.Length, "OobR");
            //
            try {
                e.Confirm = null;
                e.ConfirmOob(null, new byte[5]);
                Assert.Fail("should have thrown!");
            } catch (ArgumentException) { }
            Assert.AreEqual(null, e.Confirm);
            //
            try {
                e.Confirm = null;
                e.ConfirmOob(new byte[17], null);
                Assert.Fail("should have thrown!");
            } catch (ArgumentException) { }
            Assert.AreEqual(null, e.Confirm);
            //
            // TODO Test mutation protection
        }

        [Test]
        public void SsbPropertiesIncludingJustWorksFlag()
        {
            DoTestSsbProps(BluetoothAuthenticationMethod.Legacy,
                BluetoothAuthenticationRequirements.MITMProtectionNotDefined,
                BluetoothIoCapability.Undefined,
                null);
            DoTestSsbProps(BluetoothAuthenticationMethod.NumericComparison,
                BluetoothAuthenticationRequirements.MITMProtectionRequired,
                BluetoothIoCapability.DisplayYesNo,
                false);
            DoTestSsbProps(BluetoothAuthenticationMethod.NumericComparison,
                BluetoothAuthenticationRequirements.MITMProtectionNotRequired,
                BluetoothIoCapability.DisplayYesNo,
                true);
            DoTestSsbProps(BluetoothAuthenticationMethod.NumericComparison,
                (BluetoothAuthenticationRequirements)99,
                BluetoothIoCapability.DisplayYesNo,
                false);
            // ...
            // ...
            // ...
        }

    }//class

}
