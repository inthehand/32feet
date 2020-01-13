// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2013 Alan J.McFarlane, All rights reserved.
// Copyright (c) 2013 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConsoleMenuTesting;
using InTheHand.Net;

namespace AndroidMenuTesting
{
    abstract class NullMenuSystem:MenuSystem
    {
        protected abstract void SafeSetText(string txt);
        

        public /*override*/ void Clear()
        {
            SafeSetText(string.Empty);
        }


        //        public override void WriteLine(string msg)
//        public override void Write(string msg);
        public override void WriteLine (object arg0)
        {
            WriteLine (string.Format ("{0}", arg0));
        }

        public override void WriteLine (string fmt, params object[] args)
        {
            WriteLine (string.Format (fmt, args));
        }

        public override void Write (string fmt, params object[] args)
        {
            Write (string.Format (fmt, args));
        }
        
        //---------------------
        public abstract string SafeReadLine(string prompt, string title);

        public override string ReadLine(string prompt)
        {
            return SafeReadLine(prompt, "ReadLine");
        }
        
        public override int ReadInteger(string prompt)
        {
            while (true) {
                int value;
                string txt = SafeReadLine(prompt, "ReadInteger");
                try {
                    value = int.Parse(txt);
                } catch (FormatException) {
                    continue;
                }
                return value;
            }
        }
        
        public override int? ReadOptionalInteger(string prompt)
        {
            while (true) {
                int value;
                string txt = SafeReadLine(prompt, "ReadOptionalInteger");
                if (string.IsNullOrEmpty(txt)) {
                    return null;
                } else {
                    try {
                        value = int.Parse(txt);
                    } catch (FormatException) {
                        continue;
                    }
                    return value;
                }
            }
        }
        
        public override int? ReadOptionalIntegerHexadecimal(string prompt)
        {
            while (true) {
                int value;
                string txt = SafeReadLine(prompt, "ReadOptionalIntegerHexadecimal");
                if (string.IsNullOrEmpty(txt)) {
                    return null;
                } else {
                    try {
                        value = int.Parse(txt, System.Globalization.NumberStyles.HexNumber);
                    } catch (FormatException) {
                        continue;
                    }
                    return value;
                }
            }
        }
        
        public override InTheHand.Net.BluetoothAddress ReadBluetoothAddress (string prompt)
        {
            return ReadBluetoothAddress (prompt, false);
        }

        public override InTheHand.Net.BluetoothAddress ReadOptionalBluetoothAddress (string prompt)
        {
            return ReadBluetoothAddress (prompt, true);
        }

        public BluetoothAddress ReadBluetoothAddress (string prompt, bool optional)
        {
            //return BluetoothAddress.Parse ("00:00:00:ab:cd:ef");
            while (true) {
                InTheHand.Net.BluetoothAddress value;
                //string txt = SafeReadBluetoothAddress(prompt, "ReadBluetoothAddress");
                string txt = SafeReadLine(prompt, "ReadBluetoothAddress");
                if (string.IsNullOrEmpty (txt) && optional) {
                    return null;
                }
                if (InTheHand.Net.BluetoothAddress.TryParse (txt, out value)) {
                    AddKnownAddress (value);
                    return value;
                }
            }//while
        }

        void AddKnownAddress (BluetoothAddress value)
        {//TO-DO Does Android have suggested input feature?
        }

        public override Guid? ReadOptionalBluetoothUuid (string prompt, Guid? promptDefault)
        {
            while (true) {
                Guid value;
                if (promptDefault != null) {
                    prompt += " (default: " + promptDefault.ToString() + ")";
                }
                string txt = SafeReadLine(prompt, "ReadOptionalIntegerHexadecimal");
                if (string.IsNullOrEmpty(txt)) {
                    return null;
                } else {
                    if (base.BluetoothService_TryParseIncludingShortForm(txt, out value))
                        return value;
                    // Continue to give user another chance to input a good value.
                }
            }
        }

        [Obsolete("TODO")]
        public override string GetFilename()
        {
            return null;
        }

        void ClearConsole_Click(object sender, EventArgs e)
        {
            Clear();
        }

        //
//        public abstract void UiInvoke(EventHandler dlgt);
//        public abstract bool? InvokeRequired { get; }
//        public abstract object InvokeeControl{ get; }
//        //
//        /// <summary>
//        /// Run the menu system, may or may not be blocking (e.g. ConsoleMenu blocking, Forms not).
//        /// </summary>
//        public abstract void RunMenu();
    }
}

