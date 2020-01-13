// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

//using TextBox = Gtk.Label;
using TextBox = Gtk.TextView;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using DeviceMenuTesting;
using System.Diagnostics;
using System.Threading;

namespace GtkMenuTesting
{
    public enum MsgBoxResult
    {
        Abort = 3,
        Cancel = 2,
        Ignore = 5,
        No = 7,
        Ok = 1,
        Retry = 4,
        Yes = 6
    }

    [Flags]
    public enum MsgBoxStyle
    {
        OkOnly = 0,
        OkCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5,
        //
        Critical = 0x10,
        Question = 0x20,
        Exclamation = 0x30,
        Information = 0x40,
        // 0x300
        DefaultButton1 = 0,
        DefaultButton2 = 0x100,
        DefaultButton3 = 0x200,
        //
        ApplicationModal = 0,
        SystemModal = 0x1000,
        //
        MsgBoxHelp = 0x4000,
        MsgBoxRight = 0x80000,
        MsgBoxRtlReading = 0x100000,
        MsgBoxSetForeground = 0x10000,
    }

    //===============

    class WindowMenuSystem : ConsoleMenuTesting.MenuSystem
    {
        public bool DoInvokeRequired(Widget c)
        {
            var cur = Thread.CurrentThread;
            if (_uiThread.Equals(cur)) {
                return false;
            } else {
                return true;
            }
        }

        void AddItem(Menu parentMenu, MenuItem item)
        {
            // SWF parentMenu.MenuItems.Add(item);
            parentMenu.Append(item);
        }

        //--
        MenuItem CreateMenuItem(string label, EventHandler handler)
        {
            return CreateMenuItem(label, handler, true);
        }

        MenuItem CreateMenuItem(string label, EventHandler handler, bool enabled)
        {
            if (label.IndexOf('_') == -1) {
                label = '_' + label;
            }
            var mi = new MenuItem(label);
            mi.Activated += (o, e) => Console.WriteLine("[clicked: " + label + "]");
            mi.Activated += handler;
            //TODO mi.Enabled -= enabled;
            return mi;
        }

        //===
        volatile bool _disposed;
        TextBox tb;
        Label labelPause;
        Menu parentMenu;
        MenuItem itemUnpause, itemClearConsole, itemDoException;
        Window _parent;
        //#if !NETCF
        //        FormsMenuTesting.FormReadAddr _formReadAddress = new FormsMenuTesting.FormReadAddr();
        //#endif
        System.Threading.Thread _uiThread;

        public WindowMenuSystem(TextBox tb, Menu parentMenu, Label labelPause,
            Window window)
        {
            _parent = window;
            this.tb = tb;
            //TODO this.tb.Disposed += new EventHandler(tb_Disposed);
            this.labelPause = labelPause;
            this.parentMenu = parentMenu;
            //
            _uiThread = System.Threading.Thread.CurrentThread;
            SynchronizationContext.SetSynchronizationContext(
                new GtkSynchronizationContext(_parent));
            //
            SetupPauseControls();
            LoadKnownAddresses();
        }

        void tb_Disposed(object sender, EventArgs e)
        {
            _disposed = true;
            SaveKnownAddresses();
        }

        Dictionary<string, Menu> m_subMenus = new Dictionary<string, Menu>();

        public override void RunMenu()
        {
            //if (DoInvokeRequired(this.tb))
            //    throw new InvalidOperationException("Must be called from the main thread.");
            itemUnpause = CreateMenuItem("_Un-pause", Unpause_Click, false);
            AddItem(this.parentMenu, itemUnpause);
            //
            itemClearConsole = CreateMenuItem("_Clear console", ClearConsole_Click);
            AddItem(this.parentMenu, itemClearConsole);
            //
#if NETCF && REMOTE_CONSOLE
            MenuItem itemX = CreateMenuItem();
            itemX.Text = "Remote console";
            itemX.Click += RemoteConsole_Click;
            this.parentMenu.MenuItems.Add(itemX);
#else
            // Keep the compiler happy.
            _secondConsole = null;
#endif
            //
            MenuItem itemY = CreateMenuItem("_Save text", SaveText_Click);
            AddItem(parentMenu, itemY);
            //
            itemDoException = CreateMenuItem("_Unhandled Exception", delegate {
                if (!this.ReadYesNo("Throw exception", false)) return;
                throw new RankException("DeviceMenuTesting manual exception.");
            });
            AddItem(this.parentMenu, itemDoException);
            //
            ConsoleMenuTesting.Option.console = this;
            foreach (ConsoleMenuTesting.Option cur in Options) {
                MenuItem item = CreateMenuItem(cur.name, cur.EventHandlerInvokeOnBackground);
                if (cur.SubMenu == null || cur.SubMenu == RootName) {
                    AddItem(this.parentMenu, item);
                } else {
                    if (!m_subMenus.ContainsKey(cur.SubMenu)) {
                        Menu newSubMenu = new Menu();
                        m_subMenus.Add(cur.SubMenu, newSubMenu);
                        MenuItem newItem = new MenuItem("_" + cur.SubMenu);
                        newItem.Submenu = newSubMenu;
                        this.parentMenu.Append(newItem);
                    }
                    Menu subMenu = m_subMenus[cur.SubMenu];
                    subMenu.Add(item);
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
                f.WriteLine(this.tb.Buffer.Text);
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
            EventHandler dlgt = delegate {
                tb.Buffer.Text += txt;
            };
            SafeInvoke(tb, dlgt);
        }

        void SafeSetText(string txt)
        {
            EventHandler dlgt = delegate {
                tb.Buffer.Text = txt;
            };
            SafeInvoke(tb, dlgt);
        }

        private void SafeInvoke(Widget c, EventHandler dlgt)
        {
            if (_disposed) {
                return;
            }
            if (DoInvokeRequired(c)) {
                // WAS WinForms: c.Invoke(dlgt);
                // TODO replace with SynchronizationContext.Current.Send
                using (var ev = new System.Threading.ManualResetEvent(false)) {
                    EventHandler dlgt2 = delegate {
                        try {
                            dlgt(c, EventArgs.Empty);
                        } finally {
                            ev.Set();
                        }
                    };
                    Application.Invoke(dlgt2);
                    ev.WaitOne();
                }//using
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
            Console.WriteLine("SafeReadLine: " + title + " / " + prompt);
            //return string.Empty;
            string txt = "eeeeeeh";
            EventHandler dlgt = delegate {
                //txt = Interaction.InputBox(prompt, title,
                //null, -1, -1);
                var dlg = new ReadLineDialog();
                dlg.Title = title;
                dlg.Prompt = prompt;
                var resultG = (Gtk.ResponseType)dlg.Run();
                if (resultG == ResponseType.Ok) {
                    txt = dlg.Text;
                } else {
                    txt = string.Empty;
                }
                dlg.Destroy();
            };
            SafeInvoke(this.tb, dlgt);
            return txt;
        }

        //class hack : TextView
        //{
        //    protected override bool OnKeyReleaseEvent(Gdk.EventKey evnt)
        //    {
        //        if (evnt.Key == Gdk.Key.Return
        //                || evnt.Key == Gdk.Key.KP_Enter) {
        //            return;
        //        } 
        //        return base.OnKeyReleaseEvent(evnt);
        //    }
        //}

        class ReadLineDialog : Dialog
        {
            Label _label;
            TextView _tv;

            public ReadLineDialog()
            {
                this.BorderWidth = 5;
                _label = new Label();
                _label.Wrap = true;
                VBox.PackStart(_label, true, true, 5);
                _tv = new TextView(); // TODO Block Return/Enter
                VBox.PackStart(_tv, false, false, 5);
                VBox.ShowAll();
                //
                this.AddButton(Stock.Ok, ResponseType.Ok);
                this.AddButton(Stock.Cancel, ResponseType.Cancel);
            }

            public string Prompt
            {
                get { return _label.Text; }
                set { _label.Text = value; }
            }

            /// <summary>
            /// Get and set the window's title.
            /// </summary>
            /// -
            /// <remarks>
            /// <para>Overridden just to appear more obvious
            /// in the documentation/Intellisense.
            /// </para>
            /// </remarks>
            public new string Title
            {
                get { return base.Title; }
                set { base.Title = value; }
            }

            public string Text
            {
                get { return _tv.Buffer.Text; }
            }
        }

        string SafeReadBluetoothAddress(string prompt, string title)
        {
#if true || NETCF //TODO
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
            ButtonsType buttons2 = Convert(buttons);
            EventHandler dlgt = delegate {
                var dlg = new MessageDialog(_parent, DialogFlags.Modal,
                    MessageType.Question, buttons2, prompt + "  [ " + GetDefaultString(buttons) + " ]");
                var dflt = GetDefaultResponse(buttons);
                dlg.DefaultResponse = dflt;
                while (true) {
                    var resultG = (Gtk.ResponseType)dlg.Run();
                    result = Convert(resultG);
                    bool isYN = (result == MsgBoxResult.Yes || result == MsgBoxResult.No);
                    bool isC = (result == MsgBoxResult.Cancel);
                    if (isYN
                        || (Enum_FlagEquals(buttons, MsgBoxStyle.YesNoCancel)
                            && isC)
                        )
                        break;
                }//while
                dlg.Destroy();
            };
            SafeInvoke(this.tb, dlgt);
            return result;
        }

        bool Enum_FlagEquals(MsgBoxStyle input, MsgBoxStyle mask)
        {
            var x = input & mask;
            return x == mask;
        }

        private ResponseType GetDefaultResponse(MsgBoxStyle buttons)
        {
            var masked = buttons & MaskDefaultButton;
            if (masked == MsgBoxStyle.DefaultButton1) //Zero!!
                return ResponseType.Yes;
            if (masked == MsgBoxStyle.DefaultButton2)
                return ResponseType.No;
            if (masked == MsgBoxStyle.DefaultButton3)
                return ResponseType.DeleteEvent;
            return ResponseType.Help;
        }
        private string GetDefaultString(MsgBoxStyle buttons)
        {
            var masked = buttons & MaskDefaultButton;
            if (masked == MsgBoxStyle.DefaultButton1) //Zero!!
                return "Yes";
            if (masked == MsgBoxStyle.DefaultButton2)
                return "No";
            if (masked == MsgBoxStyle.DefaultButton3)
                return "Cancel";
            return "NoDefault";
        }

        const MsgBoxStyle MaskDefaultButton = (MsgBoxStyle)(0x100 | 0x200);

        private ButtonsType Convert(MsgBoxStyle buttons)
        {
            var b2 = buttons & ~MaskDefaultButton;
            switch (b2) {
                case MsgBoxStyle.YesNo:
                    return ButtonsType.YesNo;
                case MsgBoxStyle.YesNoCancel:
                    return ButtonsType.YesNo;
                default:
                    throw new NotImplementedException("MsgBoxStyle: " + buttons);
            }
        }

        private MsgBoxResult Convert(ResponseType resultG)
        {
            switch (resultG) {
                case ResponseType.Yes:
                    return MsgBoxResult.Yes;
                case ResponseType.No:
                    return MsgBoxResult.No;
                case ResponseType.Cancel:
                    return MsgBoxResult.Cancel;
                //
                case ResponseType.DeleteEvent:
                    return MsgBoxResult.Cancel;
                //
                case ResponseType.Accept:
                case ResponseType.Apply:
                case ResponseType.Close:
                case ResponseType.Help:
                case ResponseType.None:
                case ResponseType.Ok:
                case ResponseType.Reject:
                default:
                    throw new NotImplementedException("ResponseType: " + resultG);
            }
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
            //TODO this.labelPause.SendToBack();
            ShowPause(false, null);
        }

        private void ShowPause(bool pausing, string prompt)
        {
            //System.Threading.ThreadStart dlgt = delegate {
            EventHandler dlgt = delegate {
                if (pausing) {
                    WriteLine(prompt);
                    this.labelPause.Text = "Menu->Un-pause: " + prompt;
                    this.labelPause.Visible = true;
                    Debug.Assert(itemUnpause != null, "init'd in RunMenu so should be valid!");
                } else {
                    this.labelPause.Visible = false;
                }
                if (itemUnpause != null) {
                    //itemUnpause.Enabled = pausing;
                }
            };
            if (DoInvokeRequired(tb)) {
                // WAS WinForms: this.tb.BeginInvoke(dlgt);
                // TODO replace with SynchronizationContext.Current.Post
                Application.Invoke(dlgt);
            } else {
                dlgt(null, null);
            }
        }

        //void Pause__old()
        //{
        //    //bool hc = this.tb.IsHandleCreated;
        //    bool ir = this.tb.InvokeRequired;
        //    if (ir) { //DEBUG
        //    }
        //    MessageBox.Show("Pause" + (ir ? " InvokeRequired!!" : " (is on UI thread)"), "Pause",
        //        MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        //}

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

        public override string GetFilename()
        {
            string fileSource;
            EventHandler action = delegate {
                var d = new FileChooserDialog("Which file", _parent, FileChooserAction.Open,
                    "Cancel", ResponseType.Cancel,
                    "Open", ResponseType.Accept);
                var rslt = (Gtk.ResponseType)d.Run();
                if (rslt == Gtk.ResponseType.Accept) {
                    fileSource = d.Filename;
                }
                d.Destroy();
            };
            fileSource = null;
            SafeInvoke(this.tb, action);
            if (fileSource == null) {
                return null;
            }
            return fileSource;
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
            get { return DoInvokeRequired(tb); }
        }

        public override object InvokeeControl
        {
            get { return tb; }
        }

        //----
#if false && !NETCF //TODO [minor]
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


