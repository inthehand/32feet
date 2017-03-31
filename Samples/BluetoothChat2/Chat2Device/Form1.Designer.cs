namespace Chat2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            System.Windows.Forms.MenuItem menuItem3;
            System.Windows.Forms.MenuItem menuItem4;
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemMenu = new System.Windows.Forms.MenuItem();
            this.menuItemConnectBySelect = new System.Windows.Forms.MenuItem();
            this.menuItemConnectByAddress = new System.Windows.Forms.MenuItem();
            this.menuItemDisconnect = new System.Windows.Forms.MenuItem();
            this.menuItemShowRadioInfo = new System.Windows.Forms.MenuItem();
            this.menuItemSetRadioModeMenu = new System.Windows.Forms.MenuItem();
            this.menuItemModeNeither = new System.Windows.Forms.MenuItem();
            this.menuItemModeConnectable = new System.Windows.Forms.MenuItem();
            this.menuItemModeDiscoverable = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            menuItem3 = new System.Windows.Forms.MenuItem();
            menuItem4 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // menuItem3
            // 
            menuItem3.Text = "-";
            // 
            // menuItem4
            // 
            menuItem4.Text = "-";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItemMenu);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "null";
            // 
            // menuItemMenu
            // 
            this.menuItemMenu.MenuItems.Add(this.menuItemConnectBySelect);
            this.menuItemMenu.MenuItems.Add(this.menuItemConnectByAddress);
            this.menuItemMenu.MenuItems.Add(this.menuItemDisconnect);
            this.menuItemMenu.MenuItems.Add(menuItem3);
            this.menuItemMenu.MenuItems.Add(this.menuItemShowRadioInfo);
            this.menuItemMenu.MenuItems.Add(this.menuItemSetRadioModeMenu);
            this.menuItemMenu.MenuItems.Add(menuItem4);
            this.menuItemMenu.MenuItems.Add(this.menuItemExit);
            this.menuItemMenu.Text = "&Menu";
            // 
            // menuItemConnectBySelect
            // 
            this.menuItemConnectBySelect.Text = "&Connect by Select";
            this.menuItemConnectBySelect.Click += new System.EventHandler(this.menuItemConnectBySelect_Click);
            // 
            // menuItemConnectByAddress
            // 
            this.menuItemConnectByAddress.Text = "Connect by &Address";
            this.menuItemConnectByAddress.Click += new System.EventHandler(this.menuItemConnectByAddress_Click);
            // 
            // menuItemDisconnect
            // 
            this.menuItemDisconnect.Text = "&Disconnect";
            this.menuItemDisconnect.Click += new System.EventHandler(this.menuItemDisconnect_Click);
            // 
            // menuItemShowRadioInfo
            // 
            this.menuItemShowRadioInfo.Text = "&Radio info";
            this.menuItemShowRadioInfo.Click += new System.EventHandler(this.menuItemShowRadioInfo_Click);
            // 
            // menuItemSetRadioModeMenu
            // 
            this.menuItemSetRadioModeMenu.MenuItems.Add(this.menuItemModeNeither);
            this.menuItemSetRadioModeMenu.MenuItems.Add(this.menuItemModeConnectable);
            this.menuItemSetRadioModeMenu.MenuItems.Add(this.menuItemModeDiscoverable);
            this.menuItemSetRadioModeMenu.Text = "Set Radio &Mode";
            // 
            // menuItemModeNeither
            // 
            this.menuItemModeNeither.Text = "Neither";
            this.menuItemModeNeither.Click += new System.EventHandler(this.menuItemModeNeither_Click);
            // 
            // menuItemModeConnectable
            // 
            this.menuItemModeConnectable.Text = "Connectable";
            this.menuItemModeConnectable.Click += new System.EventHandler(this.menuItemModeConnectable_Click);
            // 
            // menuItemModeDiscoverable
            // 
            this.menuItemModeDiscoverable.Text = "Discoverable";
            this.menuItemModeDiscoverable.Click += new System.EventHandler(this.menuItemModeDiscoverable_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(240, 247);
            this.textBox1.TabIndex = 1;
            // 
            // textBoxInput
            // 
            this.textBoxInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxInput.Location = new System.Drawing.Point(0, 247);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(240, 21);
            this.textBoxInput.TabIndex = 2;
            this.textBoxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxInput_KeyPress);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBoxInput);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Chat2 32feet.NET";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemMenu;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.MenuItem menuItemConnectBySelect;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemDisconnect;
        private System.Windows.Forms.MenuItem menuItemConnectByAddress;
        private System.Windows.Forms.MenuItem menuItemSetRadioModeMenu;
        private System.Windows.Forms.MenuItem menuItemModeNeither;
        private System.Windows.Forms.MenuItem menuItemModeConnectable;
        private System.Windows.Forms.MenuItem menuItemModeDiscoverable;
        private System.Windows.Forms.MenuItem menuItemShowRadioInfo;
    }
}

