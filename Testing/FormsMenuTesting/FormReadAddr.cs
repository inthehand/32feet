using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormsMenuTesting
{
    public partial class FormReadAddr : Form
    {
        public FormReadAddr()
        {
            InitializeComponent();
        }

        public string Line { get; set; }

        public void SetKnownAddressList(AutoCompleteStringCollection knownAddresses)
        {
            this.textBox1.AutoCompleteCustomSource = knownAddresses;
        }

        public void SetPrompt(string prompt, string title)
        {
            this.Text = title;
            this.label1.Text = prompt;
        }

        #region Controls
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FormReadAddr_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK) {
                Line = this.textBox1.Text;
            }
        }
        #endregion

        private void FormReadAddr_Load(object sender, EventArgs e)
        {

        }

    }
}
