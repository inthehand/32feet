#region Using directives

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using InTheHand.Net;

#endregion

namespace DeviceListener
{
    /// <summary>
    /// Summary description for form.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form
    {
        private ListBox listBox1;
        private ObexListener listener;
        private ArrayList items;
        /// <summary>
        /// Main menu for the form.
        /// </summary>
        private System.Windows.Forms.MainMenu mainMenu1;

        public Form1()
        {
            InitializeComponent();
            InTheHand.Net.Bluetooth.BluetoothRadio br = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio;
            if (br == null) {
                MessageBox.Show("No support Bluetooth radio/stack found.");
            } else if (br.Mode != InTheHand.Net.Bluetooth.RadioMode.Discoverable) {
                DialogResult rslt = MessageBox.Show("Make BluetoothRadio Discoverable?", "DeviceListener", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (rslt == DialogResult.Yes) {
                    br.Mode = InTheHand.Net.Bluetooth.RadioMode.Discoverable;
                }
            }
            listener = new ObexListener(ObexTransport.Bluetooth);
            items = new ArrayList();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.listBox1 = new System.Windows.Forms.ListBox();
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(14, 14);
            this.listBox1.Size = new System.Drawing.Size(212, 212);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.listBox1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);

        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Application.Run(new Form1());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listener.Start();

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DealWithRequest));
            t.Start();
        }

        void RedrawList(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = items;
        }

        public void DealWithRequest()
        {
            while (listener.IsListening)
            {
                try
                {
                    ObexListenerContext olc = listener.GetContext();
                    ObexListenerRequest olr = olc.Request;
                    string filename = Uri.UnescapeDataString(olr.RawUrl.TrimStart(new char[] { '/' }));
                    olr.WriteFile("\\My Documents\\" + DateTime.Now.ToString("yyMMddHHmmss") + " " + filename);

                    items.Add(filename);
                }
                catch (Exception)
                {
                    break;
                }

                this.Invoke(new EventHandler(RedrawList));

            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            listener.Stop();

            base.OnClosing(e);
        }

    }
}

