using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Net.Sockets;

namespace IrDAServiceClient
{
    public partial class Form1 : Form
    {

#if NETCF
        internal static class MessageBox
        {
            internal static void Show(Control ownerIgnored, string text)
            {
                System.Windows.Forms.MessageBox.Show(text);
            }
        }
#endif

        readonly string NewLine 
#if NETCF
            = "\r\n";
#else
            = Environment.NewLine;
#endif
        
        //--------------------------------------------------------------
        //--------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------
        IrDAClient m_cli;
        IrDADeviceInfo[] m_devices;
        System.Net.Sockets.NetworkStream m_strm;
        System.IO.StreamWriter m_wtr;
        Encoding m_encoding;
        bool m_connecting;
        bool m_disconnecting;


        //--------------------------------------------------------------
        private void newIrdaClient()
        {
            m_connecting = false;
            m_disconnecting = true;
            UiInvoke(this.labelState, delegate { this.labelState.Text = "Disconnecting..."; });
            if (m_wtr != null) {
                m_wtr.Close();
                // Closing the writer should have closed this too.
                System.Diagnostics.Debug.Assert(!m_cli.Connected);
                // But we'll close anyway...
            }
            m_cli.Close();
            m_cli = new IrDAClient();
            UiInvoke(this.labelState, delegate { this.labelState.Text = "Disconnected."; });
        }

        private static void UiInvoke(Control labelState, EventHandler uiUpdate)
        {
            if (labelState.InvokeRequired) {
                labelState.Invoke(uiUpdate);
            } else {
                uiUpdate(labelState, new EventArgs());
            }
        }

        //--------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxProtocolMode.DataSource = new IrProtocol[]{
                // Want to exclude .None, so don't use: Enum.GetValues(typeof(IrProtocol));
                IrProtocol.TinyTP, IrProtocol.IrCOMM, IrProtocol.IrLMP,
            };
            comboBoxProtocolMode.SelectedIndex = 0;
            //
            comboBoxWellKnowServices.BeginUpdate();
            comboBoxWellKnowServices.Items.Clear();
            foreach (WellKnownIrdaSvc svcCur in WellKnownIrdaSvc.s_wellknownServices) {
                comboBoxWellKnowServices.Items.Add(svcCur);
            }//for
            comboBoxWellKnowServices.EndUpdate();
            //
#if NETCF
            // Encoding IA5 isn't supported on my PPC.
            comboBoxEncoding.SelectedIndex = 3;
#else
            comboBoxEncoding.SelectedIndex = 0;
#endif
            //
            labelState.Text = "Disconnected";
            labelSendPduLength.Text = "";
            //
            m_cli = new IrDAClient();
        }

#if ! PocketPC
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
#else
        private void Form1_Closing(object sender, CancelEventArgs e)
#endif
        {
            m_disconnecting = true;
            if (m_wtr != null) {
                m_wtr.Close();
                // Closing the writer should have closed this too.
                System.Diagnostics.Debug.Assert(!m_cli.Connected);
                // But we'll close anyway...
            }
            m_cli.Close();
        }


        //--------------------------------------------------------------
        private void buttonDiscover_Click(object sender, EventArgs e)
        {
            // DiscoverDevices is fast on IrDA so no need for background running.
            m_devices = m_cli.DiscoverDevices();
            listBoxDevices.BeginUpdate();
            listBoxDevices.Items.Clear();
            foreach (IrDADeviceInfo curDev in m_devices) {
                string text;
                if (Environment.OSVersion.Platform == PlatformID.WinCE) {
                    text = "" + curDev.DeviceName.PadRight(22) + curDev.DeviceAddress
                        + "  " + curDev.Hints;
                } else {
                    text = "" + curDev.DeviceName + "\t" + curDev.DeviceAddress
                        + "\t" + curDev.Hints;
                }
                listBoxDevices.Items.Add(text);
            }//for
            listBoxDevices.EndUpdate();
            if (listBoxDevices.Items.Count > 0) {
                listBoxDevices.SelectedIndex = 0;
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (m_connecting) {
                // During asynchronous Connect let the disconnect button be used 
                // to cancel the connect attempt.  We must not delete the extant 
                // IrDAClient until the connect attempt has completed (so don't
                // use newIrdaClient()). It will instead be called in the 
                // EndConnect callback when the cancel is actioned.
                m_cli.Close();
            } else {
                m_disconnecting = true;
                //labelState.Text = "Disconnecting...";
                newIrdaClient();
                //labelState.Text = "Disconnected";
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (m_cli.Connected) {
                MessageBox.Show(this, "Already connected.");
                return;
            }
            if (m_connecting) {
#if ! PocketPC
                Console.Beep();
#endif
                return;
            }
            //----------------------------------------------
            // Gather input from user controls
            //----------------------------------------------
            int index = listBoxDevices.SelectedIndex;
            if (index == -1) {
                MessageBox.Show(this, "Please choose a device to connect to.");
                return;
            }
            IrDADeviceInfo selectedDevice = m_devices[index];
            //
            String serviceName = textBoxServiceName.Text;
            if (serviceName.Length == 0) {
                MessageBox.Show(this, "Please enter a Service Name to connect to.");
                return;
            }
            //
            IrProtocol selectedMode = (IrProtocol)comboBoxProtocolMode.SelectedItem;
            if (selectedMode == IrProtocol.IrLMP) {
                m_cli.Client.SetSocketOption(
                    IrDASocketOptionLevel.IrLmp, IrDASocketOptionName.IrLptMode,
                    1); // NETCF doesn't have Boolean overload.
            } else if (selectedMode == IrProtocol.IrCOMM) {
                m_cli.Client.SetSocketOption(
                    IrDASocketOptionLevel.IrLmp, IrDASocketOptionName.NineWireMode,
                    1);
            }
            //
            try {
                m_encoding = Encoding.GetEncoding(comboBoxEncoding.Text);
            } catch (ArgumentException) {
                MessageBox.Show(this, "Unknown Encoding.");
                newIrdaClient();
                return;
            }
#if NETCF
            catch(PlatformNotSupportedException){
                MessageBox.Show(this, "Encoding not supported on this platform.");
                newIrdaClient();
                return;
            }
#endif
            //
            IrDAEndPoint ep = new IrDAEndPoint(selectedDevice.DeviceAddress, serviceName);

            //----------------------------------------------
            // Prepare the UI
            //----------------------------------------------
            textBoxReceive.Text = null; //.Clear()

            //----------------------------------------------
            // Connect etc
            //----------------------------------------------
            m_disconnecting = false;
            labelState.Text = "Connecting...";
            try {
#if NON_ASYNC_CONNECT
                m_cli.Connect(ep);
                DoneConnect(selectedMode);
#else
                AsyncCallback cbk = new AsyncCallback(ConnectCallback);
                m_connecting = true;
                m_cli.BeginConnect(ep, cbk, selectedMode);
#endif
            } catch (System.Net.Sockets.SocketException sex) {
                String msg = "Connect failed: "
#if ! PocketPC
                    + sex.SocketErrorCode.ToString()
#endif
                    + " (" + sex.ErrorCode.ToString("D") + "); "
                    + sex.Message;
                MessageBox.Show(this, msg);
                labelState.Text = msg;
                newIrdaClient();
                return;
            }
        }

        void ConnectCallback(IAsyncResult ar)
        {
            EventHandler uiUpdate;
            try {
                m_cli.EndConnect(ar);
                m_connecting = false;
                // Using goto is ok in error handling situations...
                goto connectSuccess;
            } catch (NullReferenceException) {
                // Close() on IrDAClient sets its socket to null, so EndConnect throws...
                //System.Diagnostics.Debug.Assert(nrex.Message == "foo");
                uiUpdate = delegate {
                    labelState.Text = "Connect cancelled.";
                };
            } catch (ObjectDisposedException) {
                // Just before the client's internal socket instance is set to null 
                // (see the catch above) is is Disposes so catch the resultant error.
                uiUpdate = delegate {
                    labelState.Text = "Connect cancelled.";
                };
            } catch (System.Net.Sockets.SocketException sex) {
                uiUpdate = delegate {
                    String msg = "Connect failed: "
#if ! PocketPC
                        + sex.SocketErrorCode.ToString()
#endif
                        + " (" + sex.ErrorCode.ToString("D") + ")"
                        + NewLine + "    '" + sex.Message + "'";
                    if (Environment.OSVersion.Platform != PlatformID.WinCE) {
                        //    On NETCF Socket.LocalEndPoint is null if connect failed! :-(
                        // Check if experiencing XP bug...
                        const byte MinLsapSel = 1;
                        const byte MaxLsapSel = 0x6F; // 111
                        IrDAEndPoint epLocal = (IrDAEndPoint)m_cli.Client.LocalEndPoint;
                        int lsapSel = GetNumericalLsapSel(epLocal);
                        System.Diagnostics.Debug.Assert(lsapSel != -1, "lsapSel != -1, should alway be numeric in this case");
                        if ((lsapSel < MinLsapSel || lsapSel > MaxLsapSel) && lsapSel != -1) {
                            msg += NewLine + NewLine + string.Format(
                                "Note: the local machine is using an illegal port number (LSAP-Sel): {0}=0x{0:X2}!", lsapSel);
                            // See "Illegal maximum LSAP Selectors used" at
                            // http://www.alanjmcf.me.uk/comms/infrared/Apparent%20bugs%20in%20Windows%20IrDA.html
                        }
                    }
                    MessageBox.Show(this, msg);
                    labelState.Text = msg;
                };
            }
            // Dropped out of one of the exception handlers, exit after updating...
            //UI thread-safe
            if (labelState.InvokeRequired) {
                labelState.Invoke(uiUpdate);
            } else {
                uiUpdate(labelState, null);
            }
            newIrdaClient();
            return;
        //-------------------------------
        // Successful Connection.
        connectSuccess:
            IrProtocol selectedMode = (IrProtocol)ar.AsyncState;
            DoneConnect(selectedMode);
        }

        private int GetNumericalLsapSel(IrDAEndPoint epLocal)
        {
            const string NumericalPrefix = "LSAP-SEL";
            string svcName = epLocal.ServiceName;
            if (!svcName.StartsWith(NumericalPrefix, StringComparison.Ordinal)) {
                return -1;
            }
            string numStr = svcName.Substring(NumericalPrefix.Length);
            byte num = byte.Parse(numStr);
            return num;
        }

        private void DoneConnect(IrProtocol selectedMode)
        {
            m_strm = m_cli.GetStream();
            m_wtr = new System.IO.StreamWriter(m_strm, m_encoding);

            //----------------------------------------------
            // Update the UI
            //----------------------------------------------
            EventHandler uiUpdate = delegate {
                labelState.Text = "Connected";
                //==
                //try {
                //    IrDAEndPoint lep = (IrDAEndPoint)GetLocalEndPoint(m_cli.Client, new IrDAEndPoint(IrDAAddress.None, "prototype"));
                //    labelState.Text += "; lep=" + lep;
                //} catch (SocketException sex) {
                //    labelState.Text += "; " + sex.ErrorCode + "from GLEP.";
                //}
                //==
                // Display the maximum send size where relevant.
                if (selectedMode == IrProtocol.IrLMP) {
                    object objValue = m_cli.Client.GetSocketOption(
                        IrDASocketOptionLevel.IrLmp, IrDASocketOptionName.SendPduLength);
                    int sendPduLength = (int)objValue;
                    labelSendPduLength.Text = sendPduLength.ToString();
                } else {
                    labelSendPduLength.Text = "N/A";
                }
#if NETCF
                this.tabControl1.SelectedIndex = 1;
#endif
                textBoxSend.Focus();
            };
            //UI thread-safe
            if (labelState.InvokeRequired) {
                labelState.Invoke(uiUpdate);
            } else {
                uiUpdate(this, new EventArgs());
            }

            //----------------------------------------------
            // Start the receive thread.
            //----------------------------------------------
            System.Threading.ThreadPool.QueueUserWorkItem(receiveThreadFn);
        }

        private void comboBoxWKS_SelectedIndexChanged(object sender, EventArgs e)
        {
            WellKnownIrdaSvc svc = (WellKnownIrdaSvc)comboBoxWellKnowServices.SelectedItem;
            if (svc == null) {
                System.Diagnostics.Debug.Fail("huh?");
                return;
            }
            textBoxServiceName.Text = svc.serviceName;
            //
            if (svc.protocolType != IrProtocol.None) {
#if DEBUG
                // Ensure that the items in the combobox and in the field in the 
                // data items are of the same type.
                Type protoType = comboBoxProtocolMode.SelectedItem.GetType();
                System.Diagnostics.Debug.Assert(protoType == svc.protocolType.GetType());
#endif
                // Set it
                comboBoxProtocolMode.SelectedItem = svc.protocolType;
#if DEBUG
                // Check that the set took effect (and there aren't say particular 
                // items missing from the combobox).
                object itemRaw = comboBoxProtocolMode.SelectedItem;
                IrProtocol item = (IrProtocol)itemRaw; // Safe due to the Assert above.
                System.Diagnostics.Debug.Assert(item == svc.protocolType);
#endif
            }//if
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (m_wtr == null
                || m_wtr.BaseStream == null
                || !m_wtr.BaseStream.CanWrite) {
                MessageBox.Show(this, "Not connected.");
            } else {
                // Assume v.25ter, so use a CR.
                String str = textBoxSend.Text + "\r";
                try {
                    m_wtr.Write(str);
                    m_wtr.Flush();
                } catch (System.IO.IOException ioex) {
                    SocketException sex = ioex.InnerException as SocketException;
                    if (sex != null) {
                        receiveAppend("!! Send SocketException: "
#if ! PocketPC
                            + sex.SocketErrorCode.ToString()
#endif
                            + " (" + sex.ErrorCode.ToString("D")
                            + "); " + ioex.Message);
                    } else {
                        receiveAppend("!! Send IOException: " + ioex.Message);
                    }
                    newIrdaClient();
                }
            }
        }

        void receiveThreadFn(object state) { receiveThreadFn(); }
        void receiveThreadFn()
        {
            if (!m_cli.Connected
                || m_strm == null
                || !m_strm.CanRead) {
#if ! PocketPC
                Console.Beep();
#endif
                return;
            }

            System.IO.StreamReader rdr = new System.IO.StreamReader(m_strm, m_encoding);
            char[] buf = new char[100];
            try {
                while (true) {
                    // We don't use ReadLine because we then don't get to see the 
                    // CR and LF characters.  And we often get the series \r\r\n 
                    // which should appear as one new line, but would appear as two 
                    // if we did textBox.Append("\n") each ReadLine.
                    int numRead = rdr.Read(buf, 0, buf.Length);
                    if (numRead == 0) {
                        break;
                    }
                    String str = new String(buf, 0, numRead);
                    receiveAppend(str);
                }//while
            } catch (System.IO.IOException ioex) {
                if (!m_disconnecting) {
                    SocketException sex = ioex.InnerException as SocketException;
                    if (sex != null) {
                        receiveAppend("!! SocketException: "
#if ! PocketPC
                            + sex.SocketErrorCode.ToString()
#endif
                            + " (" + sex.ErrorCode.ToString("D")
                            + "); " + ioex.Message);
                    } else {
                        receiveAppend("!! IOException: " + ioex.Message);
                    }
                }
            }
            newIrdaClient();
        }

        delegate void ReceiveAppendCallback(String str);

        // UI thread-safe updating.
        void receiveAppend(String str)
        {
            if (this.textBoxReceive.InvokeRequired) {
                ReceiveAppendCallback d = new ReceiveAppendCallback(receiveAppend);
                this.Invoke(d, new object[] { str });
            } else {
                textBoxReceive.Text += str; //.AppendText()
                // This doesn't work.  Do we need to give focus, or not set ReadOnly
                // or ...?
                ScrollToEnd(textBoxReceive);
            }
        }

        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessageWin32(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [System.Runtime.InteropServices.DllImport("coredll.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessageWinCE(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        void ScrollToEnd(TextBox textBox)
        {
            const int WM_VSCROLL = 0x115;
            const int SB_BOTTOM = 7;

            // Scroll to the bottom, but don't move the caret position.
            // In our case, we aren't worried about the caret position, but instead
            // because we're ReadOnly setting .SelectionStart has no effect.
            if (Environment.OSVersion.Platform == PlatformID.WinCE) {
                SendMessageWinCE(textBox.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
            } else if (Environment.OSVersion.Platform == PlatformID.Win32Windows) {
                SendMessageWin32(textBox.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
            }
        }

        void TestLsapSelRange(string serviceName, bool leaveOpen)
        {
#if NETCF
            this.tabControl1.SelectedIndex = 1;
#endif
            textBoxReceive.Text = "Test LsapSel range";
            Application.DoEvents();
            //
            IrDAClient cliX = new IrDAClient();
            IrDADeviceInfo[] devices = cliX.DiscoverDevices(1);
            if (devices.Length == 0) {
                textBoxReceive.Text += "No device in range.";
                return;
            }
            IrDAEndPoint ep = new IrDAEndPoint(devices[0].DeviceAddress, "OBEX");
            //
            for (int i = 0; i < 120; ++i) {
                try {
                    textBoxReceive.Text += " | ";
                    IrDAClient cli = new IrDAClient();
                    cli.Connect(ep);
                    textBoxReceive.Text += GetNumericalLsapSel((IrDAEndPoint)cli.Client.LocalEndPoint);
                    Application.DoEvents();
                    if (!leaveOpen) {
                        cli.Dispose();
                    }
                } catch (SocketException sex) {
                    textBoxReceive.Text += "SEX: " + sex.ErrorCode;
                }
            }//for
            textBoxReceive.Text += " | END";
        }

        private void menuItemLSR_OC_Click(object sender, EventArgs e)
        {
            TestLsapSelRange("OBEX", false);
        }
        private void menuItemLSR_OO_Click(object sender, EventArgs e)
        {
            TestLsapSelRange("OBEX", true);
        }

        private void menuItemLSR_NC_Click(object sender, EventArgs e)
        {
            TestLsapSelRange("NotExist", false);
        }

        private void menuItemLSR_NO_Click(object sender, EventArgs e)
        {
            TestLsapSelRange("NotExist", true);
        }

#if NETCF
        [System.Runtime.InteropServices.DllImport("ws2.dll", EntryPoint = "getsockname"/*"@376"*/, SetLastError = true)]
        public static extern int getsockname(IntPtr s, byte[] name, ref int namelen);

        // NOT WORKING
        // NOT WORKING// NOT WORKING// NOT WORKING// NOT WORKING
        // NOT WORKING
        // NOT WORKING
        // NOT WORKING
        // Fails with 10038
        System.Net.EndPoint GetLocalEndPoint(Socket socket, System.Net.EndPoint prototype)
        {
            System.Net.SocketAddress sa = prototype.Serialize();
            byte[] buf = new byte[sa.Size];
            IntPtr handle = socket.Handle;
            int len = buf.Length;
            int ret = getsockname(handle, buf, ref len);
            if (ret == -1) {
                int gle = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                throw new SocketException(gle);
            }
            System.Diagnostics.Debug.Assert(len == buf.Length, "length out == length in");
            for (int i = 0; i < buf.Length; ++i) {
                byte cur = buf[i];
                sa[i] = cur;
            }
            System.Net.EndPoint ep = prototype.Create(sa);
            return ep;
        }
#endif

    }//class

}
