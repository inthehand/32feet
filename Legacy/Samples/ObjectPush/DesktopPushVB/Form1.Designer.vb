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
        Me.ofdFileToBeam = New System.Windows.Forms.OpenFileDialog
        Me.btnBeam = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ofdFileToBeam
        '
        Me.ofdFileToBeam.Filter = "AllFiles|*.*"
        '
        'btnBeam
        '
        Me.btnBeam.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBeam.Location = New System.Drawing.Point(143, 100)
        Me.btnBeam.Name = "btnBeam"
        Me.btnBeam.Size = New System.Drawing.Size(75, 23)
        Me.btnBeam.TabIndex = 0
        Me.btnBeam.Text = "Beam File"
        Me.btnBeam.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(230, 135)
        Me.Controls.Add(Me.btnBeam)
        Me.Name = "Form1"
        Me.Text = "OBEX Push"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofdFileToBeam As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnBeam As System.Windows.Forms.Button

End Class
