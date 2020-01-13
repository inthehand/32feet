using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.IO;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;

namespace ObjectPushApplication
{
	/// <summary>
	/// Used to push objects.
	/// </summary>
	public class ObjectPushForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cbDevices;
		private System.Windows.Forms.Button btnBeam;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.Button btnIr;

		private BluetoothClient bc;
		private System.Windows.Forms.Button btnBluetooth;
		private System.Windows.Forms.OpenFileDialog ofdFileToBeam;
		private System.Windows.Forms.SaveFileDialog sfdReceivedFile;
        private Button button1;
		private IrDAClient ic;

		public ObjectPushForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			//InTheHand.Net.Bluetooth.Broadcom.BtIf i = new InTheHand.Net.Bluetooth.Broadcom.BtIf();

			BluetoothRadio br = BluetoothRadio.PrimaryRadio;
			if(br!=null)
			{
				if (br.Mode == RadioMode.PowerOff) {
					br.Mode = RadioMode.Connectable;
				}
			}
			else
			{
				MessageBox.Show("Your device uses an unsupported Bluetooth software stack");
				btnBluetooth.Enabled = false;
			}

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.btnBeam = new System.Windows.Forms.Button();
            this.btnBluetooth = new System.Windows.Forms.Button();
            this.btnIr = new System.Windows.Forms.Button();
            this.ofdFileToBeam = new System.Windows.Forms.OpenFileDialog();
            this.sfdReceivedFile = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            // 
            // cbDevices
            // 
            this.cbDevices.Location = new System.Drawing.Point(56, 160);
            this.cbDevices.Size = new System.Drawing.Size(120, 22);
            // 
            // btnBeam
            // 
            this.btnBeam.Location = new System.Drawing.Point(184, 160);
            this.btnBeam.Size = new System.Drawing.Size(48, 24);
            this.btnBeam.Text = "Beam";
            this.btnBeam.Click += new System.EventHandler(this.btnBeam_Click);
            // 
            // btnBluetooth
            // 
            this.btnBluetooth.Location = new System.Drawing.Point(8, 128);
            this.btnBluetooth.Size = new System.Drawing.Size(168, 24);
            this.btnBluetooth.Text = "Beam object via Bluetooth";
            this.btnBluetooth.Click += new System.EventHandler(this.btnBluetooth_Click);
            // 
            // btnIr
            // 
            this.btnIr.Location = new System.Drawing.Point(8, 160);
            this.btnIr.Size = new System.Drawing.Size(40, 24);
            this.btnIr.Text = "IR";
            this.btnIr.Click += new System.EventHandler(this.btnIr_Click);
            // 
            // ofdFileToBeam
            // 
            this.ofdFileToBeam.Filter = "All Files|*.*";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(184, 128);
            this.button1.Size = new System.Drawing.Size(48, 20);
            this.button1.Text = "button1";
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ObjectPushForm
            // 
            this.ClientSize = new System.Drawing.Size(240, 192);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnIr);
            this.Controls.Add(this.btnBluetooth);
            this.Controls.Add(this.btnBeam);
            this.Controls.Add(this.cbDevices);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Text = "Object Push";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new ObjectPushForm());
		}

		protected override void OnLoad(EventArgs e)
		{
			try
			{
				bc = new BluetoothClient();
			}
			catch
			{
				btnBluetooth.Enabled = false;
			}
			try
			{
				ic = new IrDAClient();
			}
			catch
			{
				btnIr.Enabled = false;
			}

			base.OnLoad (e);
		}

        private Image backgroundImage = null;

		protected override void OnPaint(PaintEventArgs e)
		{
            if (backgroundImage == null)
            {
                System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ThirtyTwoFeet.32feet.128.png");
                backgroundImage = new Bitmap(s);
            }
            e.Graphics.DrawImage(backgroundImage, 0,0);

			base.OnPaint (e);
		}


		private void btnBeam_Click(object sender, System.EventArgs e)
		{

			if(cbDevices.SelectedIndex > -1)
			{

				IrDADeviceInfo idi = (IrDADeviceInfo)cbDevices.SelectedItem;
					
					if(ofdFileToBeam.ShowDialog()==DialogResult.OK)
					{
						Cursor.Current = Cursors.WaitCursor;
						System.Uri uri = new Uri("obex://" + idi.DeviceAddress.ToString() + "/" + System.IO.Path.GetFileName(ofdFileToBeam.FileName));
						ObexWebRequest request =  new ObexWebRequest(uri);
						Stream requestStream = request.GetRequestStream();
						FileStream fs = File.OpenRead(ofdFileToBeam.FileName);
						byte[] buffer = new byte[1024];
						int readBytes = 1;
						while (readBytes != 0)
						{
							readBytes = fs.Read(buffer, 0, buffer.Length);
							requestStream.Write(buffer, 0, readBytes);
						}
						requestStream.Close();
						ObexWebResponse response = (ObexWebResponse)request.GetResponse();
						MessageBox.Show(response.StatusCode.ToString());
						response.Close();

						Cursor.Current = Cursors.Default;
					}

			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bc.Dispose();
		}

		private void btnBluetooth_Click(object sender, System.EventArgs e)
		{
			// use the new select bluetooth device dialog
			SelectBluetoothDeviceDialog sbdd = new SelectBluetoothDeviceDialog();
			sbdd.ShowAuthenticated = true;
			sbdd.ShowRemembered = true;
			sbdd.ShowUnknown = true;
			if(sbdd.ShowDialog()==DialogResult.OK)
			{
                if(ofdFileToBeam.ShowDialog()==DialogResult.OK)
				{
					Cursor.Current = Cursors.WaitCursor;
					System.Uri uri = new Uri("obex://" + sbdd.SelectedDevice.DeviceAddress.ToString() + "/" + System.IO.Path.GetFileName(ofdFileToBeam.FileName));
					ObexWebRequest request =  new ObexWebRequest(uri);
					request.ReadFile(ofdFileToBeam.FileName);
					
					ObexWebResponse response = (ObexWebResponse)request.GetResponse();
                    MessageBox.Show(response.StatusCode.ToString());
					response.Close();

					Cursor.Current = Cursors.Default;
				}
			}
		}

		private void btnIr_Click(object sender, System.EventArgs e)
		{
           

			Cursor.Current = Cursors.WaitCursor;

			cbDevices.DisplayMember = "DeviceName";
			cbDevices.ValueMember = "DeviceAddress";
			cbDevices.DataSource = ic.DiscoverDevices();
			Cursor.Current = Cursors.Default;
		}

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ObexWebRequest does not support GET.");
            //
            //
            SelectBluetoothDeviceDialog sbdd = new SelectBluetoothDeviceDialog();
			sbdd.ShowAuthenticated = true;
			sbdd.ShowRemembered = true;
			sbdd.ShowUnknown = true;
            if (sbdd.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                System.Uri uri = new Uri("obex://" + sbdd.SelectedDevice.DeviceAddress.ToString());
                ObexWebRequest request = new ObexWebRequest(uri);
                request.Method = "GET";
                request.ContentType = InTheHand.Net.Mime.MediaTypeNames.ObjectExchange.Capabilities;
                //request.ReadFile("C:\\t4s.log");
                //request.ContentType = InTheHand.Net.Mime.MediaTypeNames.Text.Plain;
                
                ObexWebResponse response = (ObexWebResponse)request.GetResponse();

                response.Close();

                Cursor.Current = Cursors.Default;
            }
        }

       
	}
}
