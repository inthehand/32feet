using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using InTheHand.Net;

namespace ThirtyTwoFeet
{
	/// <summary>
	/// Listens for Object pushes.
	/// </summary>
	public class ListenerForm : System.Windows.Forms.Form
	{
		private ObexListener ol;


		private System.Windows.Forms.ListBox listBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ListenerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

            /*
            InTheHand.Net.Forms.SelectBluetoothDeviceDialog sbdd = new InTheHand.Net.Forms.SelectBluetoothDeviceDialog();
            sbdd.Info = "Oh just pick one, they are all the same!";
            if (sbdd.ShowDialog() == DialogResult.OK)
            {


                byte[] rec = sbdd.SelectedDevice.GetServiceRecord(InTheHand.Net.Bluetooth.BluetoothService.SerialPort);

            }*/

            			
            ol = new ObexListener(ObexTransport.Bluetooth);
			ol.Start();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ListenerForm));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(292, 264);
			this.listBox1.TabIndex = 0;
			// 
			// ListenerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.listBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ListenerForm";
			this.Opacity = 0.8;
			this.Text = "Obex Listener";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Closed += new System.EventHandler(this.ListenerForm_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new ListenerForm());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DealWithRequest));
			t.Start();
		}

		public delegate void AddToListDelegate(string message);

		private void AddToList(string message)
		{
			listBox1.Items.Add(DateTime.Now.ToString() + ": " + message);
		}

		public void DealWithRequest()
		{
			while(ol.IsListening)
			{
				try
				{
					ObexListenerContext olc = ol.GetContext();
					ObexListenerRequest olr = olc.Request;
					string filename = Uri.UnescapeDataString(olr.RawUrl.TrimStart(new char[]{'/'}));
					olr.WriteFile(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\" + DateTime.Now.ToString("yyMMddHHmmss") + " " + filename);

					this.Invoke(new AddToListDelegate(AddToList), new object[] {filename});
				}
				catch(Exception)
				{
					break;
				}
				
				
			}
		}

		private void ListenerForm_Closed(object sender, System.EventArgs e)
		{
			ol.Stop();
		}
	}
}
