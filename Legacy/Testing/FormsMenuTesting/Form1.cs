using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsoleMenuTesting;
using System.Diagnostics;
using DeviceMenuTesting;

namespace FormsMenuTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MainMenu x = new MainMenu();
            this.textBox1.Text = string.Empty;
            MenuSystem console = new FormMenu(this.textBox1, this.menuItemMenuMenu, this.labelPause);
            BluetoothTesting program = new BluetoothTesting();
            program.console = console;
            console.AddMenus(program);
            console.RunMenu();
            //
            this.Text = this.Text + " -- " + GetCpuBitSize();
        }

        string GetCpuBitSize()
        {
            switch (IntPtr.Size) {
                case 8:
                    return "64-bit";
                case 4:
                    return "32-bit";
                default:
                    return (8 * IntPtr.Size).ToString();
            }
        }

        //--------------------------------------------------------------
        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            DialogResult rslt = MessageBox.Show("Quit?", "DeviceMenuTesting",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (rslt != DialogResult.Yes) {
                Debug.Assert(rslt == DialogResult.No, "Neither Yes nor No!");
                e.Cancel = true;
            }
        }
    }
}
