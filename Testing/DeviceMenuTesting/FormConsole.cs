#define REMOTE_CONSOLE
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic; // e.g. Interaction.InputBox, MsgBox, MsgBoxResult etc.
using System.Diagnostics;
#if NETCF
using Microsoft.WindowsCE.Forms;
#endif
using System.IO;

namespace DeviceMenuTesting
{
    class FormMenu : ConsoleMenuTesting.MenuSystem
    {
        volatile bool _disposed;
        TextBox tb;
        Label labelPause;
        Menu parentMenu;
        MenuItem itemUnpause, itemClearConsole, itemDoException;
#if !NETCF
        FormsMenuTesting.FormReadAddr _formReadAddress = new FormsMenuTesting.FormReadAddr();
#endif

        public FormMenu(TextBox tb, Menu parentMenu, Label labelPause)
        {
            this.tb = tb;
            this.tb.Disposed += new EventHandler(tb_Disposed);
            this.labelPause = labelPause;
            this.parentMenu = parentMenu;
            SetupPauseControls();
            LoadKnownAddresses();
        }

        void tb_Disposed(object sender, EventArgs e)
        {
            _disposed = true;
            SaveKnownAddresses();
        }

        Dictionary<string, MenuItem> m_subMenus = new Dictionary<string, MenuItem>();

        public override void RunMenu()
        {
            if (this.tb.InvokeRequired)
                throw new InvalidOperationException("Must be called from the main thread.");
            itemUnpause = new MenuItem();
            itemUnpause.Enabled = false;
            itemUnpause.Text = "Un-pause";
            itemUnpause.Click += Unpause_Click;
            this.parentMenu.MenuItems.Add(itemUnpause);
            //
            itemClearConsole = new MenuItem();
            itemClearConsole.Text = "Clear console";
            itemClearConsole.Click += ClearConsole_Click;
            this.parentMenu.MenuItems.Add(itemClearConsole);
            //
#if NETCF && REMOTE_CONSOLE
            MenuItem itemX = new MenuItem();
            itemX.Text = "Remote console";
            itemX.Click += RemoteConsole_Click;
            this.parentMenu.MenuItems.Add(itemX);
#else
            // Keep the compiler happy.
            _secondConsole = null;
#endif
            //
            MenuItem itemY = new MenuItem();
            itemY.Text = "Save text";
            itemY.Click += SaveText_Click;
            this.parentMenu.MenuItems.Add(itemY);
            //
            itemDoException = new MenuItem();
            itemDoException.Text = "Unhandled Exception";
            itemDoException.Click += delegate
            {
                if (!this.ReadYesNo("Throw exception", false)) return;
                throw new RankException("DeviceMenuTesting manual exception.");
            };
            this.parentMenu.MenuItems.Add(itemDoException);
            //
            foreach (ConsoleMenuTesting.Option cur in Options) {
                MenuItem item = new MenuItem();
                var label = cur.name;
                label = label.Replace('_', '&');
                item.Text = label;
                item.Click += cur.EventHandlerInvokeOnBackground;
                if (cur.SubMenu == null || cur.SubMenu == RootName) {
                    this.parentMenu.MenuItems.Add(item);
                } else {
                    if (!m_subMenus.ContainsKey(cur.SubMenu)) {
                        MenuItem newSubMenu = new MenuItem();
                        newSubMenu.Text = cur.SubMenu;
                        m_subMenus.Add(cur.SubMenu, newSubMenu);
                        this.parentMenu.MenuItems.Add(newSubMenu);
                    }
                    MenuItem subMenu = m_subMenus[cur.SubMenu];
                    subMenu.MenuItems.Add(item);
                }
            }//for
        }

        //----
#if NETCF && REMOTE_CONSOLE
        void RemoteConsole_Click(object sender, EventArgs e)
        {
            if (!ReadYesNo("Connect to Remote console (port " + RemoteConsole.Port + ")", false))
                return;
            if (_secondConsole != null) {
                var msg = "Remote Console already active!";
                SafeMsgBox(msg, "Warning", MsgBoxStyle.Information);
                return;
            }
            _secondConsole = new RemoteConsole(this);
        }
#endif

        void SaveText_Click(object sender, EventArgs e)
        {
            using (var f = System.IO.File.CreateText("output.txt")) {
                f.WriteLine(this.tb.Text);
            }
        }

        //--------
        const string NewLine = "\r\n";

        IAuxConsole _secondConsole;

        void SafeAppendText(string txt)
        {
            if (_secondConsole != null) {
                _secondConsole.AppendText(txt);
            }
            //
            EventHandler dlgt = delegate
            {
                tb.Text += txt;
            };
            SafeInvoke(tb, dlgt);
        }

        void SafeSetText(string txt)
        {
            EventHandler dlgt = delegate
            {
                tb.Text = txt;
            };
            SafeInvoke(tb, dlgt);
        }

        private void SafeInvoke(Control c, EventHandler dlgt)
        {
            if (_disposed) {
                return;
            }
            if (c.InvokeRequired) {
                c.Invoke(dlgt);
            } else {
                dlgt.Invoke(c, EventArgs.Empty);
            }
        }

        public /*override*/ void Clear()
        {
            SafeSetText(string.Empty);
        }


        public override void WriteLine(string msg)
        {
            SafeAppendText(msg + NewLine);
        }
        public override void Write(string msg)
        {
            SafeAppendText(msg);
        }

        public override void WriteLine(object arg0)
        {
            WriteLine(arg0.ToString());
        }

        public override void WriteLine(string fmt, params object[] args)
        {
            WriteLine(string.Format(fmt, args));
        }
        public override void Write(string fmt, params object[] args)
        {
            Write(string.Format(fmt, args));
        }

        string SafeReadLine(string prompt, string title)
        {
            string txt = "eeeeeeh";
            EventHandler dlgt = delegate
            {
                txt = Interaction.InputBox(prompt, title,
                null, -1, -1);
            };
            SafeInvoke(this.tb, dlgt);
            return txt;
        }

        string SafeReadBluetoothAddress(string prompt, string title)
        {
#if NETCF
            return SafeReadLine(prompt, title);
#else
            string txt = "eeeeeeh";
            EventHandler dlgt = delegate {
                _formReadAddress.SetPrompt(prompt, title);
                var rslt = _formReadAddress.ShowDialog();
                if (rslt == DialogResult.OK) {
                    txt = _formReadAddress.Line;
                } else {
                    txt = null;
                }
            };
            SafeInvoke(this.tb, dlgt);
            return txt;
#endif
        }

        MsgBoxResult SafeMsgBox(string prompt, string title,
            MsgBoxStyle buttons)
        {
            MsgBoxResult result = MsgBoxResult.Cancel;
            EventHandler dlgt = delegate
            {
                result = Interaction.MsgBox(prompt, buttons,
                    title);
            };
            SafeInvoke(this.tb, dlgt);
            return result;
        }

        //--------
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

        public override InTheHand.Net.BluetoothAddress ReadBluetoothAddress(string prompt)
        {
            return ReadBluetoothAddress(prompt, false);
        }

        public override InTheHand.Net.BluetoothAddress ReadOptionalBluetoothAddress(string prompt)
        {
            return ReadBluetoothAddress(prompt, true);
        }

        InTheHand.Net.BluetoothAddress ReadBluetoothAddress(string prompt, bool optional)
        {
            while (true) {
                InTheHand.Net.BluetoothAddress value;
                string txt = SafeReadBluetoothAddress(prompt, "ReadBluetoothAddress");
                if (string.IsNullOrEmpty(txt) && optional) {
                    return null;
                }
                if (InTheHand.Net.BluetoothAddress.TryParse(txt, out value)) {
                    AddKnownAddress(value);
                    return value;
                }
            }//while
        }

        public override string GetFilename()
        {
            return GetFilenameWinForms(this);
        }

        void ClearConsole_Click(object sender, EventArgs e)
        {
            Clear();
        }

        System.Threading.ManualResetEvent m_pause = new System.Threading.ManualResetEvent(false);

        public override void Pause(string prompt)
        {
            m_pause.Reset();
            ShowPause(true, prompt);
            bool x = m_pause.WaitOne();
        }

        private void Unpause_Click(object sender, EventArgs e)
        {
            ShowPause(false, null);
            m_pause.Set();
        }

        private void SetupPauseControls()
        {
            this.labelPause.SendToBack();
            ShowPause(false, null);
        }

        private void ShowPause(bool pausing, string prompt)
        {
            System.Threading.ThreadStart dlgt = delegate
            {
                if (pausing) {
                    WriteLine(prompt);
                    this.labelPause.Text = "Menu->Un-pause: " + prompt;
                    this.labelPause.Visible = true;
                    Debug.Assert(itemUnpause != null, "init'd in RunMenu so should be valid!");
                } else {
                    this.labelPause.Visible = false;
                }
                if (itemUnpause != null) {
                    itemUnpause.Enabled = pausing;
                }
            };
            if (this.tb.InvokeRequired) {
                this.tb.BeginInvoke(dlgt);
            } else {
                dlgt();
            }
        }

        void Pause__old()
        {
            //bool hc = this.tb.IsHandleCreated;
            bool ir = this.tb.InvokeRequired;
            if (ir) { //DEBUG
            }
            MessageBox.Show("Pause" + (ir ? " InvokeRequired!!" : " (is on UI thread)"), "Pause",
                MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }

        public override bool ReadYesNo(string prompt, bool defaultYes)
        {
            MsgBoxStyle style = MsgBoxStyle.YesNo;
            style |= (defaultYes ? MsgBoxStyle.DefaultButton1 : MsgBoxStyle.DefaultButton2);
            while (true) {
                MsgBoxResult val = SafeMsgBox(prompt, "ReadYesNo", style);
                Debug.Assert(val == MsgBoxResult.Yes || val == MsgBoxResult.No);
                return (val == MsgBoxResult.Yes); // true==Yes, false==No
            }
        }

        public override bool? ReadYesNoCancel(string prompt, bool? defaultYes)
        {
            MsgBoxStyle style = MsgBoxStyle.YesNoCancel;
            style |= (defaultYes == true ? MsgBoxStyle.DefaultButton1
                : defaultYes == false ? MsgBoxStyle.DefaultButton2
                    : MsgBoxStyle.DefaultButton3);
            var twoChoicesSupportedOnly = false;
#if NETCF
            twoChoicesSupportedOnly = SystemSettings.Platform == WinCEPlatform.Smartphone;

#endif
            if (twoChoicesSupportedOnly) {
                bool result;
                result = ReadYesNo("Choose Yes or No/Cancel -- " + prompt, defaultYes == null);
                if (result)
                    return true;
                result = ReadYesNo("Choose No or Cancel -- " + prompt, defaultYes == null);
                if (result)
                    return false;
                else
                    return null;
            }
            while (true) {
                MsgBoxResult val = SafeMsgBox(prompt, "ReadYesNoCancel", style);
                Debug.Assert(val == MsgBoxResult.Yes || val == MsgBoxResult.No
                     || val == MsgBoxResult.Cancel);
                // true==Yes, false==No
                if (val == MsgBoxResult.Yes)
                    return true;
                else if (val == MsgBoxResult.No)
                    return false;
                else return null;
            }
        }

        public override Guid? ReadOptionalBluetoothUuid(string prompt, Guid? promptDefault)
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


        private void Log(string msg)
        {
            SafeAppendText(msg + "\r\n");
        }

        //--
        public override void UiInvoke(EventHandler dlgt)
        {
            SafeInvoke(tb, dlgt);
        }

        public override bool? InvokeRequired
        {
            get { return tb.InvokeRequired; }
        }

        public override object InvokeeControl
        {
            get { return tb; }
        }

        //----
#if !NETCF
        AutoCompleteStringCollection _knownAddresses;

        private string _knownAddressesPath
        {
            get
            {
                var dir = Path.GetDirectoryName(EntryAssemblyPath);
                return Path.Combine(dir, "knownAddresses.txt");
            }
        }

        private string EntryAssemblyPath
        {
            get
            {
                var cb = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
                var u = new Uri(cb);
                var dir = u.LocalPath;
                return dir;
            }
        }

        void LoadKnownAddresses()
        {
            _knownAddresses = new AutoCompleteStringCollection();
            _formReadAddress.SetKnownAddressList(_knownAddresses);
            string[] lines;
            try {
                lines = File.ReadAllLines(_knownAddressesPath);
            } catch (FileNotFoundException) {
                return;
            }
            _knownAddresses.AddRange(lines);
        }

        void SaveKnownAddresses()
        {
            string[] arr = new string[_knownAddresses.Count];
            _knownAddresses.CopyTo(arr, 0);
            System.IO.File.WriteAllLines(_knownAddressesPath, arr);
        }

        void AddKnownAddress(InTheHand.Net.BluetoothAddress value)
        {
            var text = value.ToString("C");
            if (!_knownAddresses.Contains(text)) {
                _knownAddresses.Add(text);
            }
        }
#else
        void LoadKnownAddresses() { }
        void SaveKnownAddresses() { }
        void AddKnownAddress(InTheHand.Net.BluetoothAddress value) { }
#endif
    }
}
