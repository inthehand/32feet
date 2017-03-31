#region Copyright (c) 2007, Andy Hume <andyhume32@yahoo.co.uk>
/************************************************************************************
'
' Copyright © 2007, Andy Hume
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Copyright © 2007, Andy Hume
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NetcfTestRunner
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemRunAll = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemRunAll2 = new System.Windows.Forms.MenuItem();
            this.menuItemRunSelected = new System.Windows.Forms.MenuItem();
            this.menuItemStop = new System.Windows.Forms.MenuItem();
            this.menuItemShow = new System.Windows.Forms.MenuItem();
            this.menuItemListAssemblies = new System.Windows.Forms.MenuItem();
            this.menuItemLoadAssembly = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemQuit = new System.Windows.Forms.MenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.Location = new System.Drawing.Point(4, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(232, 22);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.GotFocus += new System.EventHandler(this.comboBox1_GotFocus);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 20);
            this.label1.Text = "label1";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItemRunAll);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItemRunAll
            // 
            this.menuItemRunAll.Text = "Run all";
            this.menuItemRunAll.Click += new System.EventHandler(this.buttonAllClasses_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuItemShow);
            this.menuItem2.MenuItems.Add(this.menuItemRunAll2);
            this.menuItem2.MenuItems.Add(this.menuItemRunSelected);
            this.menuItem2.MenuItems.Add(this.menuItemStop);
            this.menuItem2.MenuItems.Add(this.menuItemListAssemblies);
            this.menuItem2.MenuItems.Add(this.menuItemLoadAssembly);
            this.menuItem2.MenuItems.Add(this.menuItem1);
            this.menuItem2.MenuItems.Add(this.menuItemQuit);
            this.menuItem2.Text = "Menu";
            // 
            // menuItemRunAll2
            // 
            this.menuItemRunAll2.Text = "Run &All";
            this.menuItemRunAll2.Click += new System.EventHandler(this.buttonAllClasses_Click);
            // 
            // menuItemRunSelected
            // 
            this.menuItemRunSelected.Text = "Run &Selected";
            this.menuItemRunSelected.Click += new System.EventHandler(this.buttonSelectedClass_Click);
            // 
            // menuItemStop
            // 
            this.menuItemStop.Text = "Sto&p";
            this.menuItemStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // menuItemShow
            // 
            this.menuItemShow.Text = "S&how...";
            this.menuItemShow.Click += new System.EventHandler(this.buttonShowSelectedMessage_Click);
            // 
            // menuItemListAssemblies
            // 
            this.menuItemListAssemblies.Text = "List Assemblies";
            this.menuItemListAssemblies.Click += new System.EventHandler(this.menuItemListAssemblies_Click);
            // 
            // menuItemLoadAssembly
            // 
            this.menuItemLoadAssembly.Text = "Load...";
            this.menuItemLoadAssembly.Click += new System.EventHandler(this.menuItemLoadAssembly_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Save...";
            this.menuItem1.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Text = "&Quit";
            this.menuItemQuit.Click += new System.EventHandler(this.menuQuit_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(4, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(233, 211);
            this.listView1.TabIndex = 6;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "NetcfTestRunner";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItemLoadAssembly;
        private System.Windows.Forms.MenuItem menuItemListAssemblies;
        internal System.Windows.Forms.MenuItem menuItemRunSelected;
        private System.Windows.Forms.MenuItem menuItemShow;
        internal System.Windows.Forms.MenuItem menuItemStop;
        internal System.Windows.Forms.MenuItem menuItemRunAll;
        internal System.Windows.Forms.MenuItem menuItemRunAll2;
        internal System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemQuit;
    }
}

