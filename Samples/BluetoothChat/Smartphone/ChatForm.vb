Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports InTheHand.Net.Sockets
Imports InTheHand.Net.Bluetooth
Imports InTheHand.Net

Public Class ChatForm
    Inherits System.Windows.Forms.Form
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents cboDevices As System.Windows.Forms.ComboBox
    Friend WithEvents txtMessagesArchive As System.Windows.Forms.TextBox
    Friend WithEvents MainMenu2 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuSend As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSearch As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu

    Const MAX_MESSAGE_SIZE As Integer = 128
    Const MAX_TRIES As Integer = 3

    Private ServiceName As New Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}")

    Dim btClient As New BluetoothClient
    Dim btListener As BluetoothListener

    Private listening As Boolean = True

    'holds the incoming message
    Dim str As String

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
    Private Sub InitializeComponent()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.cboDevices = New System.Windows.Forms.ComboBox
        Me.txtMessagesArchive = New System.Windows.Forms.TextBox
        Me.MainMenu2 = New System.Windows.Forms.MainMenu
        Me.mnuSend = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.mnuSearch = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(8, 8)
        Me.txtMessage.Size = New System.Drawing.Size(160, 25)
        Me.txtMessage.Text = ""
        '
        'cboDevices
        '
        Me.cboDevices.Location = New System.Drawing.Point(8, 40)
        Me.cboDevices.Size = New System.Drawing.Size(160, 26)
        '
        'txtMessagesArchive
        '
        Me.txtMessagesArchive.Location = New System.Drawing.Point(8, 72)
        Me.txtMessagesArchive.Multiline = True
        Me.txtMessagesArchive.Size = New System.Drawing.Size(160, 104)
        Me.txtMessagesArchive.Text = ""
        '
        'MainMenu2
        '
        Me.MainMenu2.MenuItems.Add(Me.mnuSend)
        Me.MainMenu2.MenuItems.Add(Me.MenuItem2)
        '
        'mnuSend
        '
        Me.mnuSend.Text = "Send"
        '
        'MenuItem2
        '
        Me.MenuItem2.MenuItems.Add(Me.mnuSearch)
        Me.MenuItem2.MenuItems.Add(Me.mnuExit)
        Me.MenuItem2.Text = "Menu"
        '
        'mnuSearch
        '
        Me.mnuSearch.Text = "Search again"
        '
        'mnuExit
        '
        Me.mnuExit.Text = "Exit"
        '
        'ChatForm
        '
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.cboDevices)
        Me.Controls.Add(Me.txtMessagesArchive)
        Me.Menu = Me.MainMenu2
        Me.Text = "Bluetooth Chat"

    End Sub

#End Region

    Private Sub sendMessage(ByVal NumRetries As Integer, _
                            ByVal Buffer() As Byte, _
                            ByVal BufferLen As Integer)


        Dim client As BluetoothClient = Nothing
        Dim CurrentTries As Integer = 0
        Do
            Try
                client = New BluetoothClient
                client.Connect(New BluetoothEndPoint(CType(cboDevices.SelectedItem, BluetoothDeviceInfo).DeviceAddress, ServiceName))


            Catch se As SocketException
                If (CurrentTries >= NumRetries) Then
                    Throw se
                End If
                client = Nothing
            End Try
            CurrentTries = CurrentTries + 1

        Loop While client Is Nothing And _
             CurrentTries < NumRetries

        If (client Is Nothing) Then
            'timeout occurred
            MsgBox("Error establishing contact")
            Return
        End If

        Dim stream As System.IO.Stream = Nothing
        Try
            stream = client.GetStream()
            stream.Write(Buffer, 0, BufferLen)
        Catch e As Exception
            MsgBox("Error sending")
        Finally
            If (Not stream Is Nothing) Then
                stream.Close()
            End If
            If (Not client Is Nothing) Then
                client.Close()
            End If
        End Try
    End Sub

    Private Function receiveMessage(ByVal BufferLen As Integer) _
        As String
        Dim bytesRead As Integer = 0
        Dim client As BluetoothClient = Nothing
        Dim stream As System.IO.Stream = Nothing
        Dim Buffer(MAX_MESSAGE_SIZE) As Byte

        Try

            client = btListener.AcceptBluetoothClient()  ' blocking call
            stream = client.GetStream()
            bytesRead = stream.Read(Buffer, 0, BufferLen)

            str = client.RemoteMachineName + "->" + _
                    System.Text.Encoding.Unicode.GetString(Buffer, 0, bytesRead) + vbCrLf

        Catch e As Exception
            'dont display error if we are ending the listener
            If listening Then
                MsgBox("Error listening to incoming message")
            End If

        Finally
            If (Not stream Is Nothing) Then
                stream.Close()
            End If
            If (Not client Is Nothing) Then
                client.Close()
            End If

        End Try
        Return str
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, _
                           ByVal e As System.EventArgs) _
                           Handles MyBase.Load

        Dim t1 As System.Threading.Thread
        t1 = New Threading.Thread(AddressOf receiveLoop)
        t1.Start()

        btClient = New BluetoothClient


        Dim bdi As BluetoothDeviceInfo() = btClient.DiscoverDevices()

        cboDevices.DataSource = bdi
        cboDevices.DisplayMember = "DeviceName"


    End Sub

    Public Sub receiveLoop()
        Dim strReceived As String
        btListener = New BluetoothListener(ServiceName)
        btListener.Start()

        strReceived = receiveMessage(MAX_MESSAGE_SIZE)
        While listening '---keep on listening for new message
            If strReceived <> "" Then
                Me.Invoke(New EventHandler(AddressOf UpdateTextBox))

                strReceived = receiveMessage(MAX_MESSAGE_SIZE)
            End If
        End While

    End Sub


    Private Sub UpdateTextBox(ByVal sender As Object, ByVal e As EventArgs)
        '---delegate to update the textbox control
        txtMessagesArchive.Text += str
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        'stop receive loop
        listening = False
        'stop listening service
        btListener.Stop()

        Application.Exit()


    End Sub

    Private Sub mnuSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSend.Click

        sendMessage(MAX_TRIES, _
                    System.Text.Encoding.Unicode.GetBytes(txtMessage.Text), _
                    txtMessage.Text.Length * 2)

    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub mnuSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSearch.Click

        Cursor.Current = Cursors.WaitCursor

        Dim bdi As BluetoothDeviceInfo() = btClient.DiscoverDevices()
        cboDevices.DataSource = bdi

        Cursor.Current = Cursors.Default

    End Sub

End Class
