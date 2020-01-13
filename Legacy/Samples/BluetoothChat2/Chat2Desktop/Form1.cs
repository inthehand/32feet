using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chat2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Form_Shown(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason) {
                case CloseReason.UserClosing:
                    break;
                //
                case CloseReason.WindowsShutDown:
                    return;
                case CloseReason.ApplicationExitCall:
                    return;
                case CloseReason.FormOwnerClosing:
                    return;
                case CloseReason.MdiFormClosing:
                    return;
                //
                case CloseReason.None:
                    break;
                case CloseReason.TaskManagerClosing:
                    break;
                default:
                    break;
            }
            Form_Closing(sender, e);
        }

    }
}
