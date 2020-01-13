namespace IrDAServiceClient
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemDiscover = new System.Windows.Forms.MenuItem();
            this.menuItemConnect = new System.Windows.Forms.MenuItem();
            this.menuItemDisconnect = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemLSR_OC = new System.Windows.Forms.MenuItem();
            this.menuItemLSR_OO = new System.Windows.Forms.MenuItem();
            this.menuItemLSR_NC = new System.Windows.Forms.MenuItem();
            this.menuItemLSR_NO = new System.Windows.Forms.MenuItem();
            this.menuItemQuit = new System.Windows.Forms.MenuItem();
            this.menuItemSend = new System.Windows.Forms.MenuItem();
            this.comboBoxProtocolMode = new System.Windows.Forms.ComboBox();
            this.comboBoxWellKnowServices = new System.Windows.Forms.ComboBox();
            this.labelSendPduLength = new System.Windows.Forms.Label();
            this.listBoxDevices = new System.Windows.Forms.ListBox();
            this.labelState = new System.Windows.Forms.Label();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.textBoxServiceName = new System.Windows.Forms.TextBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDiscovery = new System.Windows.Forms.TabPage();
            this.tabPageConnected = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageDiscovery.SuspendLayout();
            this.tabPageConnected.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItemSend);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItemDiscover);
            this.menuItem1.MenuItems.Add(this.menuItemConnect);
            this.menuItem1.MenuItems.Add(this.menuItemDisconnect);
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.MenuItems.Add(this.menuItemQuit);
            this.menuItem1.Text = "Menu";
            // 
            // menuItemDiscover
            // 
            this.menuItemDiscover.Text = "Discover";
            this.menuItemDiscover.Click += new System.EventHandler(this.buttonDiscover_Click);
            // 
            // menuItemConnect
            // 
            this.menuItemConnect.Text = "Connect";
            this.menuItemConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // menuItemDisconnect
            // 
            this.menuItemDisconnect.Text = "Disconnect";
            this.menuItemDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.menuItem3);
            this.menuItem2.Text = "Tests";
            // 
            // menuItem3
            // 
            this.menuItem3.MenuItems.Add(this.menuItemLSR_OC);
            this.menuItem3.MenuItems.Add(this.menuItemLSR_OO);
            this.menuItem3.MenuItems.Add(this.menuItemLSR_NC);
            this.menuItem3.MenuItems.Add(this.menuItemLSR_NO);
            this.menuItem3.Text = "LsapSel range";
            // 
            // menuItemLSR_OC
            // 
            this.menuItemLSR_OC.Text = "OBEX closing";
            this.menuItemLSR_OC.Click += new System.EventHandler(this.menuItemLSR_OC_Click);
            // 
            // menuItemLSR_OO
            // 
            this.menuItemLSR_OO.Text = "OBEX leave";
            this.menuItemLSR_OO.Click += new System.EventHandler(this.menuItemLSR_OO_Click);
            // 
            // menuItemLSR_NC
            // 
            this.menuItemLSR_NC.Text = "NotExist closing";
            this.menuItemLSR_NC.Click += new System.EventHandler(this.menuItemLSR_NC_Click);
            // 
            // menuItemLSR_NO
            // 
            this.menuItemLSR_NO.Text = "NotExist leave";
            this.menuItemLSR_NO.Click += new System.EventHandler(this.menuItemLSR_NO_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Text = "Quit";
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // menuItemSend
            // 
            this.menuItemSend.Text = "Send";
            this.menuItemSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // comboBoxProtocolMode
            // 
            this.comboBoxProtocolMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProtocolMode.Location = new System.Drawing.Point(120, 174);
            this.comboBoxProtocolMode.Name = "comboBoxProtocolMode";
            this.comboBoxProtocolMode.Size = new System.Drawing.Size(61, 22);
            this.comboBoxProtocolMode.TabIndex = 3;
            // 
            // comboBoxWellKnowServices
            // 
            this.comboBoxWellKnowServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWellKnowServices.Location = new System.Drawing.Point(4, 148);
            this.comboBoxWellKnowServices.Name = "comboBoxWellKnowServices";
            this.comboBoxWellKnowServices.Size = new System.Drawing.Size(233, 22);
            this.comboBoxWellKnowServices.TabIndex = 1;
            this.comboBoxWellKnowServices.SelectedIndexChanged += new System.EventHandler(this.comboBoxWKS_SelectedIndexChanged);
            // 
            // labelSendPduLength
            // 
            this.labelSendPduLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSendPduLength.Location = new System.Drawing.Point(168, 248);
            this.labelSendPduLength.Name = "labelSendPduLength";
            this.labelSendPduLength.Size = new System.Drawing.Size(33, 20);
            this.labelSendPduLength.Text = "9999";
            // 
            // listBoxDevices
            // 
            this.listBoxDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDevices.Location = new System.Drawing.Point(4, 0);
            this.listBoxDevices.Name = "listBoxDevices";
            this.listBoxDevices.Size = new System.Drawing.Size(233, 142);
            this.listBoxDevices.TabIndex = 0;
            // 
            // labelState
            // 
            this.labelState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelState.Location = new System.Drawing.Point(42, 228);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(169, 20);
            this.labelState.Text = "label2";
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEncoding.Items.Add("x-IA5");
            this.comboBoxEncoding.Items.Add("iso-8859-1");
            this.comboBoxEncoding.Items.Add("utf-8");
            this.comboBoxEncoding.Items.Add("ASCII");
            this.comboBoxEncoding.Location = new System.Drawing.Point(187, 174);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(50, 22);
            this.comboBoxEncoding.TabIndex = 4;
            // 
            // textBoxServiceName
            // 
            this.textBoxServiceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServiceName.Location = new System.Drawing.Point(4, 174);
            this.textBoxServiceName.Name = "textBoxServiceName";
            this.textBoxServiceName.Size = new System.Drawing.Size(111, 21);
            this.textBoxServiceName.TabIndex = 2;
            // 
            // textBoxSend
            // 
            this.textBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSend.Location = new System.Drawing.Point(0, -3);
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSend.Size = new System.Drawing.Size(232, 44);
            this.textBoxSend.TabIndex = 11;
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceive.HideSelection = false;
            this.textBoxReceive.Location = new System.Drawing.Point(0, 39);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReceive.Size = new System.Drawing.Size(232, 157);
            this.textBoxReceive.TabIndex = 12;
            this.textBoxReceive.Text = "Using the TabControl is clearly a hack here, the discover+connect functionality s" +
                "hould be on a child form, but time is short...";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageDiscovery);
            this.tabControl1.Controls.Add(this.tabPageConnected);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.None;
            this.tabControl1.Location = new System.Drawing.Point(0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(240, 222);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPageDiscovery
            // 
            this.tabPageDiscovery.Controls.Add(this.textBoxServiceName);
            this.tabPageDiscovery.Controls.Add(this.comboBoxEncoding);
            this.tabPageDiscovery.Controls.Add(this.listBoxDevices);
            this.tabPageDiscovery.Controls.Add(this.comboBoxProtocolMode);
            this.tabPageDiscovery.Controls.Add(this.comboBoxWellKnowServices);
            this.tabPageDiscovery.Location = new System.Drawing.Point(0, 0);
            this.tabPageDiscovery.Name = "tabPageDiscovery";
            this.tabPageDiscovery.Size = new System.Drawing.Size(240, 199);
            this.tabPageDiscovery.Text = "Discovery";
            // 
            // tabPageConnected
            // 
            this.tabPageConnected.Controls.Add(this.textBoxReceive);
            this.tabPageConnected.Controls.Add(this.textBoxSend);
            this.tabPageConnected.Location = new System.Drawing.Point(0, 0);
            this.tabPageConnected.Name = "tabPageConnected";
            this.tabPageConnected.Size = new System.Drawing.Size(232, 196);
            this.tabPageConnected.Text = "Connected";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.Location = new System.Drawing.Point(4, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.Text = "Maximum IrLMP send size:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.Location = new System.Drawing.Point(4, 228);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.Text = "State:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.labelSendPduLength);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "IrDAServiceClientCF2";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageDiscovery.ResumeLayout(false);
            this.tabPageConnected.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemDiscover;
        private System.Windows.Forms.ComboBox comboBoxProtocolMode;
        private System.Windows.Forms.ComboBox comboBoxWellKnowServices;
        private System.Windows.Forms.Label labelSendPduLength;
        private System.Windows.Forms.ListBox listBoxDevices;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.TextBox textBoxServiceName;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.TextBox textBoxReceive;
        private System.Windows.Forms.MenuItem menuItemConnect;
        private System.Windows.Forms.MenuItem menuItemSend;
        private System.Windows.Forms.MenuItem menuItemDisconnect;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDiscovery;
        private System.Windows.Forms.TabPage tabPageConnected;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MenuItem menuItemQuit;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItemLSR_OC;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItemLSR_OO;
        private System.Windows.Forms.MenuItem menuItemLSR_NC;
        private System.Windows.Forms.MenuItem menuItemLSR_NO;
    }
}

