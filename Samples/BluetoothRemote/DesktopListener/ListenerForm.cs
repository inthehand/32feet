using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Net.Sockets;

namespace ThirtyTwoFeet
{
	/// <summary>
	/// Processes incoming commands
	/// </summary>
	public class RemoteListenerForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private BluetoothListener bl;

		//unique service identifier
		private Guid service = new Guid("{7A51FDC2-FDDF-4c9b-AFFC-98BCD91BF93B}");
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblVer;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.PictureBox pictureBox1;
		private bool listening = true;

		public RemoteListenerForm()
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteListenerForm));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblVer = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Bluetooth Remote";
            this.trayIcon.Visible = true;
            this.trayIcon.Click += new System.EventHandler(this.trayIcon_Click);
            // 
            // lblVer
            // 
            this.lblVer.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVer.Location = new System.Drawing.Point(64, 8);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(232, 24);
            this.lblVer.TabIndex = 0;
            this.lblVer.Text = "32feet.NET Bluetooth Remote v3.0";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // RemoteListenerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(312, 46);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblVer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RemoteListenerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bluetooth Remote";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RemoteListenerForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new RemoteListenerForm());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			BluetoothRadio br = BluetoothRadio.PrimaryRadio;
            if (br == null) {
                MessageBox.Show("No supported Bluetooth radio/stack found.");
            } else if (br.Mode != InTheHand.Net.Bluetooth.RadioMode.Discoverable) {
                DialogResult rslt = MessageBox.Show("Make BluetoothRadio Discoverable?", "Bluetooth Remote Listener", MessageBoxButtons.YesNo);
                if (rslt == DialogResult.Yes) {
                    br.Mode = RadioMode.Discoverable;
                }
            }

			bl = new BluetoothListener(service);
			bl.Start();

			System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(ListenLoop));
			t.Start();
		}

		private void RemoteListenerForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			listening = false;
			bl.Stop();
		}

		private void ListenLoop()
		{
			byte[] buffer = new byte[4];
			int received = 0;

			while(listening)
			{
				BluetoothClient bc;
				System.IO.Stream ns;
				try
				{
					bc = bl.AcceptBluetoothClient();
					ns = bc.GetStream();
				}
				catch
				{
					break;
				}
				

				//keep connection open
				while(listening)
				{
					try
					{
						received = ns.Read(buffer, 0, 2);
					}
					catch
					{
						break;
					}
				
					if(received > 0)
					{
						string command = "";

						Keys keycode = (Keys)BitConverter.ToInt16(buffer, 0);
						
						switch(keycode)
						{
							case Keys.Up:
								command = "{UP}";
								break;
							case Keys.Down:
								command = "{DOWN}";
								break;
							case Keys.Left:
								command = "{LEFT}";
								break;
							case Keys.Right:
								command = "{RIGHT}";
								break;
							case Keys.Enter:
								command = "{ENTER}";
								break;
							case Keys.D1:
								command = "1";
								break;
							case Keys.D2:
								command = "2";
								break;
							case Keys.D3:
								command = "3";
								break;
							case Keys.D4:
								command = "4";
								break;
							case Keys.D5:
								command = "5";
								break;
							case Keys.D6:
								command = "6";
								break;
							case Keys.D7:
								command = "7";
								break;
							case Keys.D8:
								command = "8";
								break;
							case Keys.D9:
								command = "9";
								break;
							case Keys.D0:
								command = "0";
								break;
						}
						System.Diagnostics.Trace.WriteLine(@"SendWait(""" + command + @""")");
						System.Windows.Forms.SendKeys.SendWait(command);
					}
					else
					{
						//connection lost
						break;
					}
				}

				try
				{
					bc.Close();
				}
				catch
				{
				}
			}

		}

		private void trayIcon_Click(object sender, System.EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
		}
	}
}
