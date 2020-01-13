namespace IrDAServiceClient
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
        protected override void Dispose (bool disposing)
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
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelState = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSendPduLength = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxWellKnowServices = new System.Windows.Forms.ComboBox();
            this.comboBoxProtocolMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxServiceName = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.listBoxDevices = new System.Windows.Forms.ListBox();
            this.buttonDiscover = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelState
            // 
            this.labelState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelState.Location = new System.Drawing.Point(56, 232);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(286, 13);
            this.labelState.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "State:";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.AutoSize = true;
            this.buttonDisconnect.Location = new System.Drawing.Point(90, 200);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(73, 23);
            this.buttonDisconnect.TabIndex = 33;
            this.buttonDisconnect.Text = "&Disconnect";
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Items.AddRange(new object[] {
            "x-IA5",
            "iso-8859-1",
            "utf-8",
            "ASCII"});
            this.comboBoxEncoding.Location = new System.Drawing.Point(95, 166);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(91, 21);
            this.comboBoxEncoding.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 169);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Text &Encoding:";
            // 
            // labelSendPduLength
            // 
            this.labelSendPduLength.AutoSize = true;
            this.labelSendPduLength.Location = new System.Drawing.Point(227, 299);
            this.labelSendPduLength.Name = "labelSendPduLength";
            this.labelSendPduLength.Size = new System.Drawing.Size(31, 13);
            this.labelSendPduLength.TabIndex = 39;
            this.labelSendPduLength.Text = "9999";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(89, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Maximum IrLMP send size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 311);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "&Receive";
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceive.Location = new System.Drawing.Point(11, 332);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReceive.Size = new System.Drawing.Size(331, 101);
            this.textBoxReceive.TabIndex = 41;
            // 
            // buttonSend
            // 
            this.buttonSend.AutoSize = true;
            this.buttonSend.Location = new System.Drawing.Point(264, 257);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(78, 23);
            this.buttonSend.TabIndex = 37;
            this.buttonSend.Text = "&Send";
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(11, 259);
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSend.Size = new System.Drawing.Size(247, 36);
            this.textBoxSend.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "&List of devices in range:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "&Well known services:";
            // 
            // comboBoxWellKnowServices
            // 
            this.comboBoxWellKnowServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWellKnowServices.DropDownWidth = 200;
            this.comboBoxWellKnowServices.FormattingEnabled = true;
            this.comboBoxWellKnowServices.Location = new System.Drawing.Point(124, 112);
            this.comboBoxWellKnowServices.MaxDropDownItems = 10;
            this.comboBoxWellKnowServices.Name = "comboBoxWellKnowServices";
            this.comboBoxWellKnowServices.Size = new System.Drawing.Size(218, 21);
            this.comboBoxWellKnowServices.TabIndex = 25;
            this.comboBoxWellKnowServices.SelectedIndexChanged += new System.EventHandler(this.comboBoxWKS_SelectedIndexChanged);
            // 
            // comboBoxProtocolMode
            // 
            this.comboBoxProtocolMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProtocolMode.FormattingEnabled = true;
            this.comboBoxProtocolMode.Location = new System.Drawing.Point(272, 141);
            this.comboBoxProtocolMode.Name = "comboBoxProtocolMode";
            this.comboBoxProtocolMode.Size = new System.Drawing.Size(70, 21);
            this.comboBoxProtocolMode.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "&Protocol mode:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Service &Name:";
            // 
            // textBoxServiceName
            // 
            this.textBoxServiceName.AcceptsTab = true;
            this.textBoxServiceName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxServiceName.Location = new System.Drawing.Point(95, 141);
            this.textBoxServiceName.Name = "textBoxServiceName";
            this.textBoxServiceName.Size = new System.Drawing.Size(91, 20);
            this.textBoxServiceName.TabIndex = 27;
            // 
            // buttonConnect
            // 
            this.buttonConnect.AutoSize = true;
            this.buttonConnect.Location = new System.Drawing.Point(11, 200);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(73, 23);
            this.buttonConnect.TabIndex = 32;
            this.buttonConnect.Text = "&Connect";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // listBoxDevices
            // 
            this.listBoxDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDevices.FormattingEnabled = true;
            this.listBoxDevices.Location = new System.Drawing.Point(11, 61);
            this.listBoxDevices.Name = "listBoxDevices";
            this.listBoxDevices.Size = new System.Drawing.Size(331, 43);
            this.listBoxDevices.TabIndex = 23;
            // 
            // buttonDiscover
            // 
            this.buttonDiscover.AutoSize = true;
            this.buttonDiscover.Location = new System.Drawing.Point(11, 13);
            this.buttonDiscover.Name = "buttonDiscover";
            this.buttonDiscover.Size = new System.Drawing.Size(108, 23);
            this.buttonDiscover.TabIndex = 21;
            this.buttonDiscover.Text = "Disco&ver devices";
            this.buttonDiscover.Click += new System.EventHandler(this.buttonDiscover_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 446);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.comboBoxEncoding);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelSendPduLength);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxReceive);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxSend);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxWellKnowServices);
            this.Controls.Add(this.comboBoxProtocolMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxServiceName);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.listBoxDevices);
            this.Controls.Add(this.buttonDiscover);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "IrDA Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSendPduLength;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxReceive;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxWellKnowServices;
        private System.Windows.Forms.ComboBox comboBoxProtocolMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxServiceName;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ListBox listBoxDevices;
        private System.Windows.Forms.Button buttonDiscover;

    }
}

