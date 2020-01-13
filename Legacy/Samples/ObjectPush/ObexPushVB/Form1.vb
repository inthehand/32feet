Imports InTheHand.net
Imports InTheHand.windows.forms


Public Class Form1
    Inherits System.Windows.Forms.Form
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    '
    ' TODO ******************** argh FileDialog *****************

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        MyBase.Dispose(disposing)
    End Sub

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnBeam As System.Windows.Forms.Button
    Friend WithEvents mnuBeam As System.Windows.Forms.MenuItem
    Friend WithEvents mnuQuit As System.Windows.Forms.MenuItem
    Friend WithEvents ofdFileToBeam As System.Windows.Forms.OpenFileDialog
    Private Sub InitializeComponent()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuBeam = New System.Windows.Forms.MenuItem
        Me.mnuQuit = New System.Windows.Forms.MenuItem
        Me.ofdFileToBeam = New System.Windows.Forms.OpenFileDialog
        Me.btnBeam = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.Add(Me.mnuBeam)
        Me.MainMenu1.MenuItems.Add(Me.mnuQuit)
        '
        'mnuBeam
        '
        Me.mnuBeam.Text = "&Beam File"
        '
        'mnuQuit
        '
        Me.mnuQuit.Text = "&Quit"
        '
        'ofdFileToBeam
        '
        Me.ofdFileToBeam.Filter = "AllFiles|*.*"
        '
        'btnBeam
        '
        Me.btnBeam.Location = New System.Drawing.Point(152, 120)
        Me.btnBeam.Name = "btnBeam"
        Me.btnBeam.Size = New System.Drawing.Size(80, 24)
        Me.btnBeam.TabIndex = 0
        Me.btnBeam.Text = "Beam File"
        '
        'Form1
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(240, 160)
        Me.Controls.Add(Me.btnBeam)
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.Text = "Object Push"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnBeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBeam.Click, mnuBeam.Click

        Dim sbdd As New SelectBluetoothDeviceDialog
        sbdd.ShowAuthenticated = True
        sbdd.ShowRemembered = True
        sbdd.ShowUnknown = True

        If sbdd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            If ofdFileToBeam.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                Cursor.Current = Cursors.WaitCursor
                Dim theuri As New Uri("obex://" + sbdd.SelectedDevice.DeviceAddress.ToString() + "/" + System.IO.Path.GetFileName(ofdFileToBeam.FileName))
                Dim request As New ObexWebRequest(theuri)
                request.ReadFile(ofdFileToBeam.FileName)

                Dim response As ObexWebResponse = CType(request.GetResponse(), ObexWebResponse)
                MessageBox.Show(response.StatusCode.ToString())
                response.Close()

                Cursor.Current = Cursors.Default
            End If

        End If
    End Sub

    Private Sub mnuQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuQuit.Click
        Me.Close()
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Dim br As Bluetooth.BluetoothRadio = Bluetooth.BluetoothRadio.PrimaryRadio
        If Not br Is Nothing Then
            If br.Mode = Bluetooth.RadioMode.PowerOff Then
                br.Mode = Bluetooth.RadioMode.Connectable
            End If
        Else
            MessageBox.Show("Your device uses an unsupported Bluetooth software stack")
            btnBeam.Enabled = False
        End If
    End Sub

    Private backgroundImage As Image

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        If backgroundImage Is Nothing Then
            Dim s As System.IO.Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ThirtyTwoFeet.32feet.128.png")
            backgroundImage = New Bitmap(s)

        End If

        e.Graphics.DrawImage(backgroundImage, 0, 0)

    End Sub

End Class
