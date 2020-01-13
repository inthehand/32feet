using System;
using System.ComponentModel;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;

namespace Chat2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            Form_Shown(sender, e);
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Form_Closing(sender, e);
        }

    }
}