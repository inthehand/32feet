using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace ThirtyTwoFeet
{
	/// <summary>
	/// used to send keys
	/// </summary>
	public class RemoteControlForm : System.Windows.Forms.Form
	{
		private BluetoothClient bc;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuClose;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.ComboBox cmbDevices;
		private System.Windows.Forms.MenuItem mnuSearch;
		private System.Windows.Forms.MenuItem mnuConnect;
        private Label label1;
		//unique service identifier
		private Guid service = new Guid("{7A51FDC2-FDDF-4c9b-AFFC-98BCD91BF93B}");
		

		public RemoteControlForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteControlForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.mnuClose = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuSearch = new System.Windows.Forms.MenuItem();
            this.mnuConnect = new System.Windows.Forms.MenuItem();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.mnuClose);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // mnuClose
            // 
            this.mnuClose.Text = "Close";
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.mnuSearch);
            this.menuItem2.MenuItems.Add(this.mnuConnect);
            this.menuItem2.Text = "Menu";
            // 
            // mnuSearch
            // 
            this.mnuSearch.Text = "Search";
            this.mnuSearch.Click += new System.EventHandler(this.mnuSearch_Click);
            // 
            // mnuConnect
            // 
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // cmbDevices
            // 
            this.cmbDevices.Location = new System.Drawing.Point(8, 8);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(160, 26);
            this.cmbDevices.TabIndex = 0;
            this.cmbDevices.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(3, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 55);
            this.label1.Text = "Send a keypress: 0-9, Enter, and the Arrow-keys are supported";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RemoteControlForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 266);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDevices);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "RemoteControlForm";
            this.Text = "Remote";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RemoteControlForm_KeyUp);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() 
		{
			Application.Run(new RemoteControlForm());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{		
			//turn on bt radio
			BluetoothRadio radio = BluetoothRadio.PrimaryRadio;
			if (radio != null && radio.Mode == RadioMode.PowerOff) {
				BluetoothRadio.PrimaryRadio.Mode = RadioMode.Connectable;
			}
			bc = new BluetoothClient();
		}

        Image backgroundImage = null;

		protected override void OnPaint(PaintEventArgs e)
		{
            if (backgroundImage == null)
            {
                System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ThirtyTwoFeet.32feet.128.png");
                backgroundImage = new Bitmap(s);
            }
			e.Graphics.DrawImage(backgroundImage, (this.Width - backgroundImage.Width)/2, 24);

			base.OnPaint (e);
		}


		private void mnuSearch_Click(object sender, System.EventArgs e)
		{
			//this will take a while...
			Cursor.Current = Cursors.WaitCursor;
			BluetoothDeviceInfo[] bdi = bc.DiscoverDevices(12);
			//bind the combo
			cmbDevices.DataSource = bdi;
			cmbDevices.DisplayMember = "DeviceName";
			cmbDevices.ValueMember = "DeviceAddress";
			cmbDevices.Visible = true;
			cmbDevices.Focus();
			Cursor.Current = Cursors.Default;

			if(bdi.Length > 0)
			{
				mnuConnect.Enabled = true;
			}
		}

		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
			if(cmbDevices.SelectedValue != null)
			{
				try
				{
					bc.Connect(new BluetoothEndPoint((BluetoothAddress)cmbDevices.SelectedValue, service)); 
					mnuConnect.Enabled = false;
					this.Controls.Remove(cmbDevices);
					this.BackColor = Color.PaleGreen;
					this.Focus();
				}
				catch
				{
					//error connecting
					this.BackColor = Color.Salmon;
				}
			}
		}

		private void mnuClose_Click(object sender, System.EventArgs e)
		{
			bc.Close();
			this.Close();
		}

		private void RemoteControlForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				System.IO.Stream s = bc.GetStream();
				s.Write(BitConverter.GetBytes((short)e.KeyCode), 0, 2);
			}
			catch
			{
			}
		
		}
	}
}
