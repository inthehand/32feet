// 32feet.NET - Personal Area Networking for .NET
//
// Sample code
//
// Copyright (c) 2011 In The Hand Ltd.
// Copyright (c) 2011 Alan J. McFarlane.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using System.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Net;

namespace Chat2
{
    partial class Form1
    {
        readonly Guid OurServiceClassId = new Guid("{29913A2D-EB93-40cf-BBB8-DEEE26452197}");
        readonly string OurServiceName = "32feet.NET Chat2";
        //
        volatile bool _closing;
        TextWriter _connWtr;
        BluetoothListener _lsnr;

        //--------

        // We need one connection to a remote application.  The bidirectional
        // chat messages are sent and received on that one connection, each on
        // a new-line (terminated with CR+LF).
        // We start a listener to accept incoming connections.  We have a
        // menu-option to connect to a remote device.  If another connection
        // is open then we will disallow a user's attempt to connect outwards
        // and will discard any incoming connections.

        #region Bluetooth start/Connect/Listen
        private void StartBluetooth()
        {
            try {
                new BluetoothClient();
            } catch (Exception ex) {
                var msg = "Bluetooth init failed: " + ex;
                MessageBox.Show(msg);
                throw new InvalidOperationException(msg, ex);
            }
            // TODO Check radio?
            //
            // Always run server?
            StartListener();
        }

        BluetoothAddress BluetoothSelect()
        {
            var dlg = new SelectBluetoothDeviceDialog();
            var rslt = dlg.ShowDialog();
            if (rslt != DialogResult.OK) {
                AddMessage(MessageSource.Info, "Cancelled select device.");
                return null;
            }
            var addr = dlg.SelectedDevice.DeviceAddress;
            return addr;
        }

        void BluetoothConnect(BluetoothAddress addr)
        {
            var cli = new BluetoothClient();
            try {
                cli.Connect(addr, OurServiceClassId);
                var peer = cli.GetStream();
                SetConnection(peer, true, cli.RemoteEndPoint);
                ThreadPool.QueueUserWorkItem(ReadMessagesToEnd_Runner, peer);
            } catch (SocketException ex) {
                // Try to give a explanation reason by checking what error-code.
                // http://32feet.codeplex.com/wikipage?title=Errors
                // Note the error codes used on MSFT+WM are not the same as on
                // MSFT+Win32 so don't expect much there, we try to use the
                // same error codes on the other platforms where possible.
                // e.g. Widcomm doesn't match well, Bluetopia does.
                // http://32feet.codeplex.com/wikipage?title=Feature%20support%20table
                string reason;
                switch (ex.ErrorCode) {
                    case 10048: // SocketError.AddressAlreadyInUse
                        // RFCOMM only allow _one_ connection to a remote service from each device.
                        reason = "There is an existing connection to the remote Chat2 Service";
                        break;
                    case 10049: // SocketError.AddressNotAvailable
                        reason = "Chat2 Service not running on remote device";
                        break;
                    case 10064: // SocketError.HostDown
                        reason = "Chat2 Service not using RFCOMM (huh!!!)";
                        break;
                    case 10013: // SocketError.AccessDenied:
                        reason = "Authentication required";
                        break;
                    case 10060: // SocketError.TimedOut:
                        reason = "Timed-out";
                        break;
                    default:
                        reason = null;
                        break;
                }
                reason += " (" + ex.ErrorCode.ToString() + ") -- ";
                //
                var msg = "Bluetooth connection failed: " + MakeExceptionMessage(ex);
                msg = reason + msg;
                AddMessage(MessageSource.Error, msg);
                MessageBox.Show(msg);
            } catch (Exception ex) {
                var msg = "Bluetooth connection failed: " + MakeExceptionMessage(ex);
                AddMessage(MessageSource.Error, msg);
                MessageBox.Show(msg);
            }
        }

        private void StartListener()
        {
            var lsnr = new BluetoothListener(OurServiceClassId);
            lsnr.ServiceName = OurServiceName;
            lsnr.Start();
            _lsnr = lsnr;
            ThreadPool.QueueUserWorkItem(ListenerAccept_Runner, lsnr);
        }

        void ListenerAccept_Runner(object state)
        {
            var lsnr = (BluetoothListener)_lsnr;
            // We will accept only one incoming connection at a time. So just
            // accept the connection and loop until it closes.
            // To handle multiple connections we would need one threads for
            // each or async code.
            while (true) {
                var conn = lsnr.AcceptBluetoothClient();
                var peer = conn.GetStream();
                SetConnection(peer, false, conn.RemoteEndPoint);
                ReadMessagesToEnd(peer);
            }
        }
        #endregion

        #region Connection Set/Close
        private void SetConnection(Stream peerStream, bool outbound, BluetoothEndPoint remoteEndPoint)
        {
            if (_connWtr != null) {
                AddMessage(MessageSource.Error, "Already Connected!");
                return;
            }
            _closing = false;
            var connWtr = new StreamWriter(peerStream);
            connWtr.NewLine = "\r\n"; // Want CR+LF even on UNIX/Mac etc.
            _connWtr = connWtr;
            ClearScreen();
            AddMessage(MessageSource.Info,
                (outbound ? "Connected to " : "Connection from ")
                // Can't guarantee that the Port is set, so just print the address.
                // For more info see the docs on BluetoothClient.RemoteEndPoint.
                + remoteEndPoint.Address);
        }

        private void ConnectionCleanup()
        {
            _closing = true;
            var wtr = _connWtr;
            //_connStrm = null;
            _connWtr = null;
            if (wtr != null) {
                try {
                    wtr.Close();
                } catch (Exception ex) {
                    Debug.WriteLine("ConnectionCleanup close ex: " + MakeExceptionMessage(ex));
                }
            }
        }

        void BluetoothDisconnect()
        {
            AddMessage(MessageSource.Info, "Disconnecting");
            ConnectionCleanup();
        }
        #endregion

        #region Connection I/O
        private bool Send(string message)
        {
            if (_connWtr == null) {
                MessageBox.Show("No connection.");
                return false;
            }
            try {
                _connWtr.WriteLine(message);
                _connWtr.Flush();
                return true;
            } catch (Exception ex) {
                MessageBox.Show("Connection lost! (" + MakeExceptionMessage(ex) + ")");
                ConnectionCleanup();
                return false;
            }
        }

        private void ReadMessagesToEnd_Runner(object state)
        {
            Stream peer = (Stream)state;
            ReadMessagesToEnd(peer);
        }

        private void ReadMessagesToEnd(Stream peer)
        {
            var rdr = new StreamReader(peer);
            while (true) {
                string line;
                try {
                    line = rdr.ReadLine();
                } catch (IOException ioex) {
                    if (_closing) {
                        // Ignore the error that occurs when we're in a Read
                        // and _we_ close the connection.
                    } else {
                        AddMessage(MessageSource.Error, "Connection was closed hard (read).  "
                            + MakeExceptionMessage(ioex));
                    }
                    break;
                }
                if (line == null) {
                    AddMessage(MessageSource.Info, "Connection was closed (read).");
                    break;
                }
                AddMessage(MessageSource.Remote, line);
            }//while
            ConnectionCleanup();
        }
        #endregion

        #region Radio
        void SetRadioMode(RadioMode mode)
        {
            try {
                BluetoothRadio.PrimaryRadio.Mode = mode;
            } catch (NotSupportedException) {
                MessageBox.Show("Setting Radio.Mode not supported on this Bluetooth stack.");
            }
        }

        static void DisplayPrimaryBluetoothRadio(TextWriter wtr)
        {
            var myRadio = BluetoothRadio.PrimaryRadio;
            if (myRadio == null) {
                wtr.WriteLine("No radio hardware or unsupported software stack");
                return;
            }
            var mode = myRadio.Mode;
            // Warning: LocalAddress is null if the radio is powered-off.
            wtr.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress);
            wtr.WriteLine("Mode: " + mode.ToString());
            wtr.WriteLine("Name: " + myRadio.Name);
            wtr.WriteLine("HCI Version: " + myRadio.HciVersion
                + ", Revision: " + myRadio.HciRevision);
            wtr.WriteLine("LMP Version: " + myRadio.LmpVersion
                + ", Subversion: " + myRadio.LmpSubversion);
            wtr.WriteLine("ClassOfDevice: " + myRadio.ClassOfDevice
                + ", device: " + myRadio.ClassOfDevice.Device
                + " / service: " + myRadio.ClassOfDevice.Service);
            wtr.WriteLine("S/W Manuf: " + myRadio.SoftwareManufacturer);
            wtr.WriteLine("H/W Manuf: " + myRadio.Manufacturer);
        }
#endregion


        #region Menu items etc
        void Form_Shown(object sender, EventArgs e)
        {
            StartBluetooth();
            AddMessage(MessageSource.Info,
                "Connect to another remote device running the app."
                + "  Each person can then enter text in the box at the bottom"
                + " and hit return to send it."
                + "  Of course the radio on the target device will have to be"
                + " in connectable and/or discoverable mode.");
            this.textBox1.Select(0, 0); // Unselect the text.
            // Focus to the input-box.
#if !NETCF
            this.textBoxInput.Select();
#else
            this.textBoxInput.Focus();
#endif
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Quit?", "Quit?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes) {
                e.Cancel = true;
            }
        }
        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemConnectBySelect_Click(object sender, EventArgs e)
        {
            var addr = BluetoothSelect();
            if (addr == null) {
                return;
            }
            BluetoothConnect(addr);
        }

        private void menuItemConnectByAddress_Click(object sender, EventArgs e)
        {
            var addr = BluetoothAddress.Parse("002233445566");
            var line = Microsoft.VisualBasic.Interaction.InputBox("Target Address", "Chat2", null, -1, -1);
            if (string.IsNullOrEmpty(line)) {
                return;
            }
            line = line.Trim();
            if (!BluetoothAddress.TryParse(line, out addr)) {
                MessageBox.Show("Invalid address.");
                return;
            }
            BluetoothConnect(addr);
        }

        private void menuItemDisconnect_Click(object sender, EventArgs e)
        {
            BluetoothDisconnect();
        }

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            var cr = e.KeyChar == '\r';
            var lf = e.KeyChar == '\n';
            if (cr || lf) {
                e.Handled = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            var message = this.textBoxInput.Text;
            bool successSend = Send(message);
            if (successSend) {
                AddMessage(MessageSource.Local, message);
                this.textBoxInput.Text = string.Empty;
            }
        }

        //--
        private void menuItemModeDiscoverable_Click(object sender, EventArgs e)
        {
            SetRadioMode(RadioMode.Discoverable);
        }

        private void menuItemModeConnectable_Click(object sender, EventArgs e)
        {
            SetRadioMode(RadioMode.Connectable);
        }

        private void menuItemModeNeither_Click(object sender, EventArgs e)
        {
            SetRadioMode(RadioMode.PowerOff);
        }

        private void menuItemShowRadioInfo_Click(object sender, EventArgs e)
        {
            using (var wtr = new StringWriter()) {
                DisplayPrimaryBluetoothRadio(wtr);
                AddMessage(MessageSource.Info, wtr.ToString());
            }
        }
        #endregion

        #region Chat Log
        private void ClearScreen()
        {
            EventHandler action = delegate {
                AssertOnUiThread();
                this.textBox1.Text = string.Empty;
            };
            ThreadSafeRun(action);
        }

        enum MessageSource
        {
            Local,
            Remote,
            Info,
            Error,
        }

        void AddMessage(MessageSource source, string message)
        {
            EventHandler action = delegate {
                string prefix;
                switch (source) {
                    case MessageSource.Local:
                        prefix = "Me: ";
                        break;
                    case MessageSource.Remote:
                        prefix = "You: ";
                        break;
                    case MessageSource.Info:
                        prefix = "Info: ";
                        break;
                    case MessageSource.Error:
                        prefix = "Error: ";
                        break;
                    default:
                        prefix = "???:";
                        break;
                }
                AssertOnUiThread();
                this.textBox1.Text =
                    prefix + message + "\r\n"
                    + this.textBox1.Text;
            };
            ThreadSafeRun(action);
        }

        private void ThreadSafeRun(EventHandler action)
        {
            Control c = this.textBox1;
            if (c.InvokeRequired) {
                c.BeginInvoke(action);
            } else {
                action(null, null);
            }
        }
        #endregion

        private void AssertOnUiThread()
        {
            Debug.Assert(!this.textBox1.InvokeRequired, "UI access from non UI thread!");
        }

        private static string MakeExceptionMessage(Exception ex)
        {
#if !NETCF
            return ex.Message;
#else
            // Probably no messages in NETCF.
            return ex.GetType().Name;
#endif
        }

    }
}