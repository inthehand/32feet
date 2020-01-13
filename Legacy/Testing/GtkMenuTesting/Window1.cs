// 32feet.NET - Personal Area Networking for .NET
//
// Copyright (c) 2010-2011 In The Hand Ltd, All rights reserved.
// Copyright (c) 2010-2011 Alan J. McFarlane, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using ConsoleMenuTesting;

namespace GtkMenuTesting
{
    class Window1 : Window
    {
        public Window1()
            : base("GtkMenuTesting")
        {
            InitializeComponent();
            Form1_Load(this, EventArgs.Empty);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var x = new MenuBar();
            this.textBox1.Buffer.Text = string.Empty;
            MenuSystem console = new WindowMenuSystem(this.textBox1, this.menuItemMenuMenu, this.labelPause, this);
            BluetoothTesting program = new BluetoothTesting();
            program.console = console;
            console.AddMenus(program);
            console.RunMenu();
        }

        //--------------------------------------------------------------
        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Quit(); //HACK !
        }

        //private void Form1_Closing(object sender, CancelEventArgs e)
        //{
        //    DialogResult rslt = MessageBox.Show("Quit?", "DeviceMenuTesting",
        //        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        //    if (rslt != DialogResult.Yes) {
        //        Debug.Assert(rslt == DialogResult.No, "Neither Yes nor No!");
        //        e.Cancel = true;
        //    }
        //}

        //=================================
        private TextView textBox1;
        private Label labelPause;
        private MenuBar mainMenu1;
        private Menu menuItemMenuMenu;
        private MenuItem menuItemQuit;

        //----
        private void InitializeComponent()
        {
            SetSizeRequest(500, 600);
            //
            this.textBox1 = new TextView();
            this.labelPause = new Label();
            this.mainMenu1 = new MenuBar();
            //
            this.textBox1.Editable = false;
            this.textBox1.WrapMode = WrapMode.Word;
            this.textBox1.BorderWidth = 1;
            //
            var sw = new ScrolledWindow();
            sw.AddWithViewport(this.textBox1);
            //
            this.menuItemMenuMenu = new Menu();
            var tmpItem = new MenuItem("_Menu");
            tmpItem.Submenu = menuItemMenuMenu;
            this.mainMenu1.Append(tmpItem);
            //
            this.menuItemQuit = new MenuItem("_Quit");
            this.menuItemQuit.Activated += menuItemQuit_Click;
            this.mainMenu1.Append(menuItemQuit);
            //
            var b = new VBox();
            b.PackStart(this.mainMenu1, false, false, 0);
            b.PackStart(this.labelPause, false, false, 0);
            b.PackStart(sw, true, true, 0);
            this.Add(b);
        }
    }
}
