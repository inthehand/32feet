using Console = System.ValueType; // Block the use of Console, must use "console".
using System;
using System.Collections.Generic;
using System.Text;
using InTheHand.Net.Bluetooth;

namespace ConsoleMenuTesting
{
    partial class BluetoothTesting
    {
#if !NETCF
        [MenuItem, SubMenu("Win32Auth")]
        public void BtWin32Auth()
        {
            bool useMsgBox = console.ReadYesNo("Auth callback prompt dialog", false);
            var hndlr = useMsgBox
                ? (EventHandler<BluetoothWin32AuthenticationEventArgs>)HandlerWithMsgBoxInclSsp //HndlrWithMsgBox
                : HndlrNoRespond;
            using (BluetoothWin32Authentication ar = new BluetoothWin32Authentication(hndlr)) {
                console.Pause("Continue to stop authenticating>");
            }
        }

        //[MenuItem]
        //public void BtWin32AuthEx()
        //{
        //    using (BluetoothWin32AuthenticationEx ar = new BluetoothWin32AuthenticationEx(Hndlr)) {
        //        console.Write("Hit return to break>");
        //        Console.ReadLine();
        //    }
        //}

        void HndlrNoRespond(object sender, BluetoothWin32AuthenticationEventArgs e)
        {
            console.WriteLine("Win32Auth hndlr, peer: {0} {1}, m: {2}, req: {3}, io: {4}, n_pk: {5}."
                + " Attempt# {6}, PrevErr: {7}",
                e.Device.DeviceAddress, e.Device.DeviceName,
                e.AuthenticationMethod, e.AuthenticationRequirements,
                e.IoCapability, e.NumberOrPasskey,
                e.AttemptNumber, e.PreviousNativeErrorCode);
        }

        void HndlrWithMsgBox(object sender, BluetoothWin32AuthenticationEventArgs e)
        {
            HndlrNoRespond(sender, e);
            var msg = string.Format(
                    "Auth {0} {1} {2} {6} jw: {3}, n/pk: {4:G6} (default confirm: {5})",
                    e.Device.DeviceAddress, e.Device.DeviceName,
                    e.AuthenticationMethod, e.JustWorksNumericComparison,
                    e.NumberOrPasskey, e.Confirm,
                    e.AuthenticationRequirements);
            if (e.AuthenticationMethod == BluetoothAuthenticationMethod.Legacy) {
                e.Pin = console.ReadLine(msg + "\r\n"
                    + "PIN?");
                console.WriteLine("Using Pin: {0}", ToStringQuotedOrNull(e.Pin));
            } else {
                var rsp = console.ReadYesNoCancel(msg, null);
                e.Confirm = rsp;
            }
        }

        void HandlerWithMsgBoxInclSsp(object sender, InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs e)
        {
            HndlrNoRespond(sender, e);
            if (e.AuthenticationMethod == BluetoothAuthenticationMethod.Legacy) {
                var line = console.ReadLine("Device with address " + e.Device.DeviceAddress.ToString()
                    + " is wanting to pair."
                    + " Please enter the PIN/passphrase.");
                if (line != null) {
                    e.Pin = line;
                }
            } else if (e.JustWorksNumericComparison == true) {
                var rslt = console.ReadYesNo("Allow device with address " + e.Device.DeviceAddress.ToString()
                    + " to pair?", false);
                if (rslt) {
                    e.Confirm = true;
                }
            } else if (e.AuthenticationMethod == BluetoothAuthenticationMethod.NumericComparison) {
                var rslt = console.ReadYesNo("Device with address " + e.Device.DeviceAddress.ToString()
                    + " is wanting to pair."
                    + " Confirm that it is displaying this six-digit number on screen: "
                    + e.NumberOrPasskeyAsString, false);
                if (rslt) {
                    e.Confirm = true;
                }
            } else if (e.AuthenticationMethod == BluetoothAuthenticationMethod.Passkey) {
                var line = console.ReadLine("Device with address " + e.Device.DeviceAddress.ToString()
                    + " is wanting to pair."
                    + " Please enter the six digit number that it is displaying on the other device's screen.");
                if (line != null) {
                    int pk;
                    if (Int32.TryParse(line, out pk) && pk >= 0 && pk < 1000000) {
                        e.ResponseNumberOrPasskey = pk;
                        e.Confirm = true;
                    } else {
                        console.WriteLine("Invalid format Passkey entered.");
                    }
                }
            } else if (e.AuthenticationMethod == BluetoothAuthenticationMethod.PasskeyNotification) {
                var rslt = console.ReadYesNo("Device with address " + e.Device.DeviceAddress.ToString()
                    + " is wanting to pair."
                    // Please type this passkey on the other device...
                    + " Confirm this Passkey on the other machine: "
                    + e.NumberOrPasskeyAsString, false);
                if (rslt) {
                    e.Confirm = true;
                }
            } else if (e.AuthenticationMethod == BluetoothAuthenticationMethod.OutOfBand) {
                console.WriteLine("What to do with authentication method " + e.AuthenticationMethod + "??");
            } else {
                console.WriteLine("Unknown authentication method: " + e.AuthenticationMethod);
            }
            var callbackWithResult = console.ReadYesNo("Callback with Result", true);
            e.CallbackWithResult = callbackWithResult;
        }

        //----
        [MenuItem, SubMenu("Win32Auth")]
        public void Win32AuthCallbackTwoSeparateAuthentications()
        {
            console.WriteLine("Authenticate two devices");
            console.WriteLine("Passcode respectively: '{0}', '{1}', '{2}'",
                "1234", "9876", "ásdfghjkl");
            Win32AuthCallback__(Win32AuthCallbackTwoSeparateAuthenticationsHandler);
        }

        Int32 Win32AuthCallbackTwoSeparateAuthentications_count;

        void Win32AuthCallbackTwoSeparateAuthenticationsHandler(Object sender, InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs e)
        {
            console.WriteLine("Authenticating {0} {1}", e.Device.DeviceAddress, e.Device.DeviceName);
            console.WriteLine("  Attempt# {0}, Last error code {1}", e.AttemptNumber, e.PreviousNativeErrorCode);
            //
            if (Win32AuthCallbackTwoSeparateAuthentications_count == 0) {
                e.Pin = "1234";
            } else if (Win32AuthCallbackTwoSeparateAuthentications_count == 1) {
                e.Pin = "9876";
            } else if (Win32AuthCallbackTwoSeparateAuthentications_count == 2) {
                e.Pin = "ásdfghjkl";
            }
            console.WriteLine("Using '{0}'", e.Pin);
            Win32AuthCallbackTwoSeparateAuthentications_count += 1;
        }

        void Win32AuthCallbackJohoHandler(Object sender, InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs e)
        {
            String address = e.Device.DeviceAddress.ToString();
            console.WriteLine("Received an authentication request from address " + address);

            //compare the first 8 hex numbers, this is just a special case because in the
            //used scenario the model of the devices can be identified by the first 8 hex
            //numbers, the last 4 numbers being the device specific part.
            if (address.Substring(0, 8).Equals("0099880D")
                    || address.Substring(0, 8).Equals("0099880E")) {
                //send authentication response
                e.Pin = "5276";
            } else if (address.Substring(0, 8).Equals("00997788")) {
                //send authentication response
                e.Pin = "1423";
            }
        }


        //The output of a standard run is the following.  Note that the error code from
        //the initial wrong response is error code 1244 which is ErrorNotAuthenticated.
        //However when we try another attempt we get error code 1167 which is
        //ErrorDeviceNotConnected.  So for the devices I've tested it's apparently not
        //worth trying a second response.  If the user attempts to send another PIN-
        //response (by setting the Pin field in the event args) then it will be ignored
        //(we probably should have the event args throw when Pin is set in that case).
        //
        //[[
        //Local radio address is 008099244999
        //Authenticate two devices
        //Passcode respectively: '1234', '9876', 'ásdfghjkl'
        //Making PC discoverable
        //Hit Return to complete
        //
        //Complete
        //Authenticate one device -- with wrong passcode here the first two times.
        //Passcode respectively: 'BAD-x', 'BAD-y', '9876'
        //Making PC discoverable
        //Hit Return to complete
        //Authenticating 000A99686999 Win2kBelkin
        //  Attempt# 0, Last error code 0
        //Using '0.88516242889927'
        //Authenticating 000A99686999 Win2kBelkin
        //  Attempt# 1, Last error code 1244
        //Using '0.59947050996100'
        //Authenticating 000A99686999 Win2kBelkin
        //  Attempt# 2, Last error code 1167
        //Using '9876'
        //]]
        [MenuItem, SubMenu("Win32Auth")]
        public void Win32AuthCallbackInitialBadPasscodeAndRetry()
        {
            console.WriteLine("Authenticate one device -- with wrong passcode here the first two times.");
            console.WriteLine("Passcode respectively: '{0}', '{1}', '{2}'",
                "BAD-x", "BAD-y", "9876");
            Win32AuthCallback__(Win32AuthCallbackInitialBadPasscodeAndRetryHandler);
        }


        void Win32AuthCallback__(EventHandler<BluetoothWin32AuthenticationEventArgs> handler)
        {
            console.WriteLine("Making PC discoverable");
            InTheHand.Net.Bluetooth.BluetoothRadio radio = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio;
            InTheHand.Net.Bluetooth.RadioMode origRadioMode = radio.Mode;
            radio.Mode = InTheHand.Net.Bluetooth.RadioMode.Discoverable;
            //
            using (InTheHand.Net.Bluetooth.BluetoothWin32Authentication auther
                    = new InTheHand.Net.Bluetooth.BluetoothWin32Authentication(handler)) {
                console.Pause("Continue to stop authenticating>");
            }
            radio.Mode = origRadioMode;
        }

        int Win32AuthCallbackInitialBadPasscodeAndRetry_count;

        void Win32AuthCallbackInitialBadPasscodeAndRetryHandler(Object sender, InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs e)
        {
            console.WriteLine("Authenticating {0} {1}", e.Device.DeviceAddress, e.Device.DeviceName);
            console.WriteLine("  Attempt# {0}, Last error code {1}", e.AttemptNumber, e.PreviousNativeErrorCode);
            if (e.AttemptNumber != Win32AuthCallbackInitialBadPasscodeAndRetry_count) {
                console.WriteLine("Bad AttemptNumber!!!");
            }
            Random rnd = new Random();
            String badPasscode = rnd.NextDouble().ToString();
            badPasscode = badPasscode.Substring(0, Math.Min(16, badPasscode.Length));
            //
            if (Win32AuthCallbackInitialBadPasscodeAndRetry_count == 0
                    || Win32AuthCallbackInitialBadPasscodeAndRetry_count == 1) {
                e.Pin = badPasscode;
                e.CallbackWithResult = true;
            } else if (Win32AuthCallbackInitialBadPasscodeAndRetry_count == 2
                    || Win32AuthCallbackInitialBadPasscodeAndRetry_count == 3) {
                e.Pin = "9876";
                e.CallbackWithResult = true;
            } else if (Win32AuthCallbackInitialBadPasscodeAndRetry_count == 4) {
                e.CallbackWithResult = true; //Try to *wrongly* get another callback!
            } else {
                console.WriteLine("Unexpected callback #{0}", Win32AuthCallbackInitialBadPasscodeAndRetry_count);
            }
            console.WriteLine("Using '{0}'", (e.Pin == null ? "<null>" : e.Pin));
            Win32AuthCallbackInitialBadPasscodeAndRetry_count += 1;
        }


        [MenuItem, SubMenu("Win32Auth")]
        public void Win32AuthCallbackInduceFinalisationFault()
        {
            console.WriteLine("Authenticate a device two times");
            console.WriteLine("Making PC discoverable");
            InTheHand.Net.Bluetooth.BluetoothRadio radio = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio;
            InTheHand.Net.Bluetooth.RadioMode origRadioMode = radio.Mode;
            radio.Mode = InTheHand.Net.Bluetooth.RadioMode.Discoverable;
            //
            Object auther;
            auther = new InTheHand.Net.Bluetooth.BluetoothWin32Authentication(Win32AuthCallbackTwoSeparateAuthenticationsHandler);
            console.Pause("Continue to clear and GC");
            auther = "foo";
            GC.Collect();
            console.Pause("Continue to complete");
            radio.Mode = origRadioMode;
        }

#endif

    }
}
