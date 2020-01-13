using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace PairingAndPortsCF
{
    public partial class Form1 : Form
    {
        BluetoothClient bc = new BluetoothClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = bc.DiscoverDevices(32, false, false, true);
            listBox1.DisplayMember = "DeviceName";
            listBox2.DataSource = bc.DiscoverDevices(64, true, false, false);
            listBox2.DisplayMember = "DeviceName";
        }

        private void mnuPair_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                BluetoothSecurity.PairRequest(((BluetoothDeviceInfo)listBox1.SelectedItem).DeviceAddress, "1234");

            }
        }
    }
}