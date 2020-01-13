using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace DiscoveryAuthenticationSample
{
    public partial class Form1 : Form
    {

        private InTheHand.Net.Bluetooth.BluetoothWin32Authentication bwa;

        public Form1()
        {
            InitializeComponent();

            bwa = new InTheHand.Net.Bluetooth.BluetoothWin32Authentication(new EventHandler<InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs>(AuthHandler));
            InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio.Name = "Something Snazzy";
        }


        void AuthHandler(object sender, InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs e)
        {
            InTheHand.Net.Sockets.BluetoothDeviceInfo bdi = e.Device;

        }
    }

    public class BluetoothRadioEvents
    {
#pragma warning disable 67
        public event BluetoothRadioEventHandler DeviceArrived;
        public event BluetoothRadioEventHandler DeviceUpdated;
        public event BluetoothRadioEventHandler DeviceDeparted;
#pragma warning restore 67
    }

    public delegate void BluetoothRadioEventHandler(object sender, BluetoothRadioEventArgs e);

    public class BluetoothRadioEventArgs : EventArgs
    {
        internal BluetoothRadioEventArgs(BluetoothAddress deviceAddress)
        {
            this.Address = deviceAddress;
        }

        public BluetoothAddress Address
        {
            get;
            private set;
        }
    }
}
