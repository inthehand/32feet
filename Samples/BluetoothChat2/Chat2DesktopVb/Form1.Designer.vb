<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim toolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Dim toolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
        Me.connectByAddressToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.disconnectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.connectBySelectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.showRadioInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.menuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.textBox1 = New System.Windows.Forms.TextBox
        Me.textBoxInput = New System.Windows.Forms.TextBox
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip
        toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        toolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.menuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolStripMenuItem1
        '
        toolStripMenuItem1.Name = "toolStripMenuItem1"
        toolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'toolStripMenuItem2
        '
        toolStripMenuItem2.Name = "toolStripMenuItem2"
        toolStripMenuItem2.Size = New System.Drawing.Size(177, 6)
        '
        'connectByAddressToolStripMenuItem
        '
        Me.connectByAddressToolStripMenuItem.Name = "connectByAddressToolStripMenuItem"
        Me.connectByAddressToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.connectByAddressToolStripMenuItem.Text = "Connect by &Address"
        '
        'disconnectToolStripMenuItem
        '
        Me.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem"
        Me.disconnectToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.disconnectToolStripMenuItem.Text = "&Disconnect"
        '
        'connectBySelectToolStripMenuItem
        '
        Me.connectBySelectToolStripMenuItem.Name = "connectBySelectToolStripMenuItem"
        Me.connectBySelectToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.connectBySelectToolStripMenuItem.Text = "&Connect by Select"
        '
        'showRadioInfoToolStripMenuItem
        '
        Me.showRadioInfoToolStripMenuItem.Name = "showRadioInfoToolStripMenuItem"
        Me.showRadioInfoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.showRadioInfoToolStripMenuItem.Text = "&Radio info"
        '
        'menuToolStripMenuItem
        '
        Me.menuToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.connectBySelectToolStripMenuItem, Me.connectByAddressToolStripMenuItem, Me.disconnectToolStripMenuItem, toolStripMenuItem1, Me.showRadioInfoToolStripMenuItem, toolStripMenuItem2, Me.exitToolStripMenuItem})
        Me.menuToolStripMenuItem.Name = "menuToolStripMenuItem"
        Me.menuToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.menuToolStripMenuItem.Text = "&Menu"
        '
        'exitToolStripMenuItem
        '
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.exitToolStripMenuItem.Text = "E&xit"
        '
        'textBox1
        '
        Me.textBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.textBox1.Location = New System.Drawing.Point(0, 24)
        Me.textBox1.Multiline = True
        Me.textBox1.Name = "textBox1"
        Me.textBox1.ReadOnly = True
        Me.textBox1.Size = New System.Drawing.Size(392, 322)
        Me.textBox1.TabIndex = 4
        '
        'textBoxInput
        '
        Me.textBoxInput.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.textBoxInput.Location = New System.Drawing.Point(0, 346)
        Me.textBoxInput.Name = "textBoxInput"
        Me.textBoxInput.Size = New System.Drawing.Size(392, 20)
        Me.textBoxInput.TabIndex = 5
        '
        'menuStrip1
        '
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(392, 24)
        Me.menuStrip1.TabIndex = 6
        Me.menuStrip1.Text = "menuStrip1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 366)
        Me.Controls.Add(Me.textBox1)
        Me.Controls.Add(Me.textBoxInput)
        Me.Controls.Add(Me.menuStrip1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents connectByAddressToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents disconnectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents connectBySelectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents showRadioInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents menuToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents textBox1 As System.Windows.Forms.TextBox
    Private WithEvents textBoxInput As System.Windows.Forms.TextBox
    Private WithEvents menuStrip1 As System.Windows.Forms.MenuStrip

End Class
