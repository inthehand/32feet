' 32feet.NET - Personal Area Networking for .NET
'
' Sample code
'
' Copyright (c) 2011 In The Hand Ltd.
' Copyright (c) 2011 Alan J. McFarlane.
' This source code is licensed under the In The Hand Community License - see License.txt

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports InTheHand.Net
Imports InTheHand.Net.Sockets
Imports InTheHand.Windows.Forms
Imports System.Net.Sockets
Imports InTheHand.Net.Bluetooth
Imports System.Net

'Namespace Chat2

Partial Class Form1

    Private ReadOnly OurServiceClassId As Guid = New Guid("{29913A2D-EB93-40cf-BBB8-DEEE26452197}")
    Shared ReadOnly OurServiceName As String = "32feet.NET Chat2"
    '
    'TODO !!!! volatile 
    Private _closing As Boolean
    Private _connWtr As TextWriter
    Private _lsnr As BluetoothListener

    '--------

    ' We need one connection to a remote application.  The bidirectional
    ' chat messages are sent and received on that one connection, each on
    ' a new-line (terminated with CR+LF).
    ' We start a listener to accept incoming connections.  We have a
    ' menu-option to connect to a remote device.  If another connection
    ' is open then we will disallow a user's attempt to connect outwards
    ' and will discard any incoming connections.

#Region "Bluetooth start/Connect/Listen"
    Private Sub StartBluetooth()
        Try
            Dim tmp = New BluetoothClient()
        Catch ex As Exception
            Dim msg = "Bluetooth init failed: " & ex.ToString
            MessageBox.Show(msg)
            Throw New InvalidOperationException(msg, ex)
        End Try
        ' TODO Check radio?
        '
        ' Always run server?
        StartListener()
    End Sub

    Function BluetoothSelect() As BluetoothAddress
        Dim dlg = New SelectBluetoothDeviceDialog()
        Dim rslt = dlg.ShowDialog()
        If rslt <> DialogResult.OK Then
            AddMessage(MessageSource.Info, "Cancelled select device.")
            Return Nothing
        End If
        Dim addr = dlg.SelectedDevice.DeviceAddress
        Return addr
    End Function

    Sub BluetoothConnect(ByVal addr As BluetoothAddress)
        Dim cli = New BluetoothClient()
        Try
            cli.Connect(addr, OurServiceClassId)
            Dim peer = cli.GetStream()
            SetConnection(peer, True, cli.RemoteEndPoint)
            ThreadPool.QueueUserWorkItem(AddressOf ReadMessagesToEnd_Runner, peer)
        Catch ex As SocketException
            ' Try to give a explanation reason by checking what error-code.
            ' http:'32feet.codeplex.com/wikipage?title=Errors
            ' Note the error codes used on MSFT+WM are not the same as on
            ' MSFT+Win32 so don't expect much there, we try to use the
            ' same error codes on the other platforms where possible.
            ' e.g. Widcomm doesn't match well, Bluetopia does.
            ' http:'32feet.codeplex.com/wikipage?title=Feature%20support%20table
            Dim reason As String
            Select Case (ex.ErrorCode)
                Case 10048 ' SocketError.AddressAlreadyInUse
                    ' RFCOMM only allow _one_ connection to a remote service from each device.
                    reason = "There is an existing connection to the remote Chat2 Service"
                Case 10049 ' SocketError.AddressNotAvailable
                    reason = "Chat2 Service not running on remote device"
                Case 10064 ' SocketError.HostDown
                    reason = "Chat2 Service not using RFCOMM (huh!!!)"
                Case 10013 ' SocketError.AccessDenied:
                    reason = "Authentication required"
                Case 10060 ' SocketError.TimedOut:
                    reason = "Timed-out"
                Case Else
                    reason = Nothing
            End Select
            reason &= " (" & ex.ErrorCode.ToString() & ") -- "
            '
            Dim msg = "Bluetooth connection failed: " & MakeExceptionMessage(ex)
            msg = reason & msg
            AddMessage(MessageSource.Error, msg)
            MessageBox.Show(msg)
        Catch ex As Exception
            Dim msg = "Bluetooth connection failed: " & MakeExceptionMessage(ex)
            AddMessage(MessageSource.Error, msg)
            MessageBox.Show(msg)
        End Try
    End Sub

    Private Sub StartListener()
        Dim lsnr = New BluetoothListener(OurServiceClassId)
        lsnr.ServiceName = OurServiceName
        lsnr.Start()
        _lsnr = lsnr
        ThreadPool.QueueUserWorkItem(AddressOf ListenerAccept_Runner, lsnr)
    End Sub

    Sub ListenerAccept_Runner(ByVal state As Object)
        Dim lsnr = CType(_lsnr, BluetoothListener)
        ' We will accept only one incoming connection at a time. So just
        ' accept the connection and loop until it closes.
        ' To handle multiple connections we would need one threads for
        ' each or async code.
        While True
            Dim conn = lsnr.AcceptBluetoothClient()
            Dim peer = conn.GetStream()
            SetConnection(peer, False, conn.RemoteEndPoint)
            ReadMessagesToEnd(peer)
        End While
    End Sub
#End Region

#Region "Connection Set/Close"
    Private Sub SetConnection(ByVal peerStream As Stream, ByVal outbound As Boolean, ByVal remoteEndPoint As BluetoothEndPoint)
        If _connWtr Is Nothing Then
            AddMessage(MessageSource.Error, "Already Connected!")
            Return
        End If
        _closing = False
        Dim connWtr = New StreamWriter(peerStream)
        connWtr.NewLine = vbCrLf ' Want CR+LF even on UNIX/Mac etc.
        _connWtr = connWtr
        ClearScreen()
        ' Can't guarantee that the Port is set, so just print the address.
        ' For more info see the docs on BluetoothClient.RemoteEndPoint.
        AddMessage(MessageSource.Info, _
            CType(IIf(outbound, "Connected to ", "Connection from "), String) _
            & remoteEndPoint.Address.ToString())
    End Sub

    Private Sub ConnectionCleanup()
        _closing = True
        Dim wtr = _connWtr
        '_connStrm = Nothing
        _connWtr = Nothing
        If wtr IsNot Nothing Then
            Try
                wtr.Close()
            Catch ex As Exception
                Debug.WriteLine("ConnectionCleanup close ex: " & MakeExceptionMessage(ex))
            End Try
        End If
    End Sub

    Sub BluetoothDisconnect()
        AddMessage(MessageSource.Info, "Disconnecting")
        ConnectionCleanup()
    End Sub
#End Region

#Region "Connection I/O"
    Private Function Send(ByVal message As String) As Boolean
        If (_connWtr Is Nothing) Then
            MessageBox.Show("No connection.")
            Return False
        End If
        Try
            _connWtr.WriteLine(message)
            _connWtr.Flush()
            Return True
        Catch ex As Exception
            MessageBox.Show("Connection lost! (" & MakeExceptionMessage(ex) & ")")
            ConnectionCleanup()
            Return False
        End Try
    End Function

    Private Sub ReadMessagesToEnd_Runner(ByVal state As Object)
        Dim peer = CType(state, Stream)
        ReadMessagesToEnd(peer)
    End Sub

    Private Sub ReadMessagesToEnd(ByVal peer As Stream)
        Dim rdr = New StreamReader(peer)
        While True
            Dim line As String
            Try
                line = rdr.ReadLine()
            Catch ioex As IOException
                If (_closing) Then
                    ' Ignore the error that occurs when we're in a Read
                    ' and _we_ close the connection.
                Else
                    AddMessage(MessageSource.Error, "Connection was closed hard (read).  " _
                        & MakeExceptionMessage(ioex))
                End If
                Exit While
            End Try
            If (line Is Nothing) Then
                AddMessage(MessageSource.Info, "Connection was closed (read).")
                Exit While
            End If
            AddMessage(MessageSource.Remote, line)
        End While
        ConnectionCleanup()
    End Sub
#End Region

#Region "Radio"
    Sub SetRadioMode(ByVal mode As RadioMode)
        Try
            BluetoothRadio.PrimaryRadio.Mode = mode
        Catch tmp As NotSupportedException
            MessageBox.Show("Setting Radio.Mode not supported on this Bluetooth stack.")
        End Try
    End Sub

    Shared Sub DisplayPrimaryBluetoothRadio(ByVal wtr As TextWriter)
        Dim myRadio = BluetoothRadio.PrimaryRadio
        If (myRadio Is Nothing) Then
            wtr.WriteLine("No radio hardware or unsupported software stack")
            Return
        End If
        Dim mode = myRadio.Mode
        ' Warning: LocalAddress is null if the radio is powered-off.
        wtr.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress)
        wtr.WriteLine("Mode: " & mode.ToString())
        wtr.WriteLine("Name: " & myRadio.Name)
        wtr.WriteLine("HCI Version: " & myRadio.HciVersion _
            & ", Revision: " & myRadio.HciRevision)
        wtr.WriteLine("LMP Version: " & myRadio.LmpVersion _
            & ", Subversion: " & myRadio.LmpSubversion)
        wtr.WriteLine("ClassOfDevice: " & myRadio.ClassOfDevice.ToString() _
            & ", device: " & myRadio.ClassOfDevice.Device _
            & " / service: " & myRadio.ClassOfDevice.Service)
        wtr.WriteLine("S/W Manuf: " & myRadio.SoftwareManufacturer)
        wtr.WriteLine("H/W Manuf: " & myRadio.Manufacturer)
    End Sub
#End Region


#Region "Menu items etc"
    Sub Form_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Shown
        StartBluetooth()
        AddMessage(MessageSource.Info, _
            "Connect to another remote device running the app." _
            & "  Each person can then enter text in the box at the bottom" _
            & " and hit return to send it." _
            & "  Of course the radio on the target device will have to be" _
            & " in connectable and/or discoverable mode.")
        Me.textBox1.Select(0, 0) ' Unselect the text.
        ' Focus to the input-box.
#If Not NETCF Then
        Me.textBoxInput.Select()
#Else
        Me.textBoxInput.Focus()
#End If
    End Sub

    Private Sub Form_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles MyBase.FormClosing
        Dim result = MessageBox.Show("Quit?", "Quit?", _
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If (result <> DialogResult.Yes) Then
            e.Cancel = True
        End If
    End Sub
    Private Sub menuItemExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub menuItemConnectBySelect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles connectBySelectToolStripMenuItem.Click
        Dim addr = BluetoothSelect()
        If (addr Is Nothing) Then
            Return
        End If
        BluetoothConnect(addr)
    End Sub

    Private Sub menuItemConnectByAddress_Click(ByVal sender As Object, ByVal e As EventArgs) Handles connectByAddressToolStripMenuItem.Click
        Dim addr = BluetoothAddress.Parse("002233445566")
        Dim line = Microsoft.VisualBasic.Interaction.InputBox("Target Address", "Chat2", Nothing, -1, -1)
        If (String.IsNullOrEmpty(line)) Then
            Return
        End If
        line = line.Trim()
        If (Not BluetoothAddress.TryParse(line, addr)) Then
            MessageBox.Show("Invalid address.")
            Return
        End If
        BluetoothConnect(addr)
    End Sub

    Private Sub menuItemDisconnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles disconnectToolStripMenuItem.Click
        BluetoothDisconnect()
    End Sub

    Private Sub textBoxInput_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles textBoxInput.KeyPress
        Dim cr As Boolean = (e.KeyChar = vbCr) '\r'
        Dim lf As Boolean = (e.KeyChar = vbLf) '\n'
        If (cr OrElse lf) Then
            e.Handled = True
            SendMessage()
        End If
    End Sub

    Private Sub SendMessage()
        Dim message = Me.textBoxInput.Text
        Dim successSend As Boolean = Send(message)
        If (successSend) Then
            AddMessage(MessageSource.Local, message)
            Me.textBoxInput.Text = String.Empty
        End If
    End Sub

    '--
    Private Sub menuItemModeDiscoverable_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetRadioMode(RadioMode.Discoverable)
    End Sub

    Private Sub menuItemModeConnectable_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetRadioMode(RadioMode.Connectable)
    End Sub

    Private Sub menuItemModeNeither_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetRadioMode(RadioMode.PowerOff)
    End Sub

    Private Sub menuItemShowRadioInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles showRadioInfoToolStripMenuItem.Click
        Using wtr = New StringWriter()
            DisplayPrimaryBluetoothRadio(wtr)
            AddMessage(MessageSource.Info, wtr.ToString())
        End Using
    End Sub
#End Region

#Region "Chat Log"
    Private Sub ClearScreen()
        Dim action As EventHandler = AddressOf ClearScreen_Delegate
        ThreadSafeRun(action)
    End Sub
    Private Sub ClearScreen_Delegate(ByVal sender As Object, ByVal e As EventArgs)
        AssertOnUiThread()
        Me.textBox1.Text = String.Empty
    End Sub

    Enum MessageSource
        Local
        Remote
        Info
        [Error]
    End Enum

    Sub AddMessage(ByVal source As MessageSource, ByVal message As String)
        Dim action As EventHandler = AddressOf AddMessage_Delegate
        Monitor.Enter(_key)
        Try
            _msgList.Enqueue(New MsgListItem(source, message))
        Finally
            Monitor.Exit(_key)
        End Try
        ThreadSafeRun(action)
    End Sub
    Private Sub AddMessage_Delegate(ByVal sender As Object, ByVal e As EventArgs)
        Monitor.Enter(_key)
        Dim source As MessageSource
        Dim message As String
        Try
            Dim item = _msgList.Dequeue()
            source = item._source
            message = item._message
        Finally
            Monitor.Exit(_key)
        End Try
        '--
        Dim prefix As String
        Select Case source
            Case MessageSource.Local
                prefix = "Me: "
            Case MessageSource.Remote
                prefix = "You: "
            Case MessageSource.Info
                prefix = "Info: "
            Case MessageSource.Error
                prefix = "Error: "
            Case Else
                prefix = "???:"
        End Select
        AssertOnUiThread()
        Me.textBox1.Text = _
            prefix & message & vbCrLf _
            & Me.textBox1.Text
    End Sub

    Private _key As New Object
    Private _msgList As New Queue(Of MsgListItem)

    Private Class MsgListItem
        Public ReadOnly _source As MessageSource
        Public ReadOnly _message As String
        '
        Sub New(ByVal source As MessageSource, ByVal message As String)
            _source = source
            _message = message
        End Sub
    End Class

    Private Sub ThreadSafeRun(ByVal action As EventHandler)
        Dim c As Control = Me.textBox1
        If c.InvokeRequired Then
            c.BeginInvoke(action)
        Else
            action(Nothing, Nothing)
        End If
    End Sub
#End Region

    Private Sub AssertOnUiThread()
        Debug.Assert(Not Me.textBox1.InvokeRequired, "UI access from non UI thread!")
    End Sub

    Private Shared Function MakeExceptionMessage(ByVal ex As Exception) As String
#If Not NETCF Then
        Return ex.Message
#Else
        ' Probably no messages in NETCF.
        Return ex.GetType().Name
#End If
    End Function

End Class
'End Namespace