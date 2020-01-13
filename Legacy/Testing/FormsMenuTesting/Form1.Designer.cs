namespace FormsMenuTesting
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelPause = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemMenuMenu = new System.Windows.Forms.MenuItem();
            this.menuItemQuit = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(472, 532);
            this.textBox1.TabIndex = 0;
            // 
            // labelPause
            // 
            this.labelPause.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPause.Location = new System.Drawing.Point(0, 0);
            this.labelPause.Name = "labelPause";
            this.labelPause.Size = new System.Drawing.Size(472, 13);
            this.labelPause.TabIndex = 1;
            this.labelPause.Text = "label1";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemMenuMenu,
            this.menuItemQuit});
            // 
            // menuItemMenuMenu
            // 
            this.menuItemMenuMenu.Index = 0;
            this.menuItemMenuMenu.Text = "Menu";
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Index = 1;
            this.menuItemQuit.Text = "Quit";
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 545);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelPause);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "FormsMenuTesting";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelPause;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemMenuMenu;
        private System.Windows.Forms.MenuItem menuItemQuit;
    }
}

