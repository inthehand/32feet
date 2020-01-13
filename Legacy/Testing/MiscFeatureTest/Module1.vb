Imports InTheHand.Net.Bluetooth
Imports InTheHand.Net
Imports System.IO
Imports InTheHand.Net.Sockets
Imports InTheHand.Net.Bluetooth.AttributeIds
Imports ObexWebRequest2 = InTheHand.Net.ObexWebRequest
Imports System.Windows.Forms

Module Module1

    Public Sub Main()
        Misc.FromJsr82ServerUri()
        '
#If LIVE_DISCO Then
        LiveDisco.One()
#End If
        '
        Documentation.SdpListenerRecord()
        Documentation.BluetoothListener2ServiceName()
        Documentation.ObexPutFile()
        Documentation.ObexPutJpegFile()
        Documentation.ObexPutData()
        Documentation.ObexGetData()
        Documentation.LoopUsing()
        'Documentation.DisplayBluetoothRadio(Console.Out)
        Documentation.ConnectByServiceName()
        Documentation.CreateOnSpecificRadioStack()
        '
        Security.BtCli_SetPin()
        Misc.GetPrimaryRadio()
        Misc.Discovery()
        Misc.SelectBluetoothDeviceDialog()
        '
        Misc.ObexListen()
        '
        Try
            Using x As New BluetoothWin32Authentication(BluetoothAddress.Parse("001122334455"), "ckzdknasdnkankzsbkn213ih1")
                Console.Write("Hit return to continue>")
                Console.ReadLine()
            End Using
        Catch ex As Exception
            Console.WriteLine("Exception in BluetoothWin32AuthenticationEx:")
            Console.WriteLine(ex)
        End Try
        '--
        Dim inst As New Tests()
        inst.Win32AuthCallbackTwoSeparateAuthentications()
        inst.Win32AuthCallbackInitialBadPasscodeAndRetry()
        '
        Misc.ObexPushAddressString("127.0.0.1")
        Misc.ObexPushAddressString("000a3a6865bb")
        Misc.ObexPushBluetoothUi()
    End Sub
End Module

Class Documentation
    ' ==== Discovery Async ====
    Public Sub DiscoDevicesAsync()
        Dim bco As New BluetoothComponent()
        AddHandler bco.DiscoverDevicesProgress, AddressOf HandleDiscoDevicesProgress
        AddHandler bco.DiscoverDevicesComplete, AddressOf HandleDiscoDevicesComplete
        bco.DiscoverDevicesAsync(255, True, True, True, False, 99)
    End Sub

    Private Sub HandleDiscoDevicesProgress(ByVal sender As Object, ByVal e As DiscoverDevicesEventArgs)
        Console.WriteLine("DiscoDevicesAsync Progress found {0} devices.", e.Devices.Length)
    End Sub

    Private Sub HandleDiscoDevicesComplete(ByVal sender As Object, ByVal e As DiscoverDevicesEventArgs)
        Debug.Assert(CInt(e.UserState) = 99)
        If e.Cancelled Then
            Console.WriteLine("DiscoDevicesAsync cancelled.")
        ElseIf e.Error IsNot Nothing Then
            Console.WriteLine("DiscoDevicesAsync error: {0}.", e.Error.Message)
        Else
            Console.WriteLine("DiscoDevicesAsync complete found {0} devices.", e.Devices.Length)
        End If
    End Sub



    ' ==== Bluetooth Radio ====
    Public Shared Sub DisplayBluetoothRadio(ByVal wtr As System.IO.TextWriter)
        Dim myRadio As BluetoothRadio = BluetoothRadio.PrimaryRadio
        If myRadio Is Nothing Then
            wtr.WriteLine("No radio hardware or unsupported software stack")
            Return
        End If
        Dim mode As RadioMode = myRadio.Mode
        ' Warning: LocalAddress can be null if the radio is powered-off.
        wtr.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress)
        wtr.WriteLine("Mode: " & mode.ToString())
        wtr.WriteLine("Name: " & myRadio.Name)
        wtr.WriteLine("HCI Version: " & myRadio.HciVersion _
            & ", Revision: " & myRadio.HciRevision)
        wtr.WriteLine("LMP Version: " & myRadio.LmpVersion _
            & ", Subversion: " & myRadio.LmpSubversion)
        wtr.WriteLine("ClassOfDevice: " & myRadio.ClassOfDevice.ToString() _
            & ", device: " & myRadio.ClassOfDevice.Device.ToString() _
            & " / service: " & myRadio.ClassOfDevice.Service.ToString())
        '
        '
        ' Enable discoverable mode
#If False Then
        wtr.WriteLine()
        myRadio.Mode = RadioMode.Discoverable
        wtr.WriteLine("Radio Mode now: " & myRadio.Mode.ToString())
#End If
    End Sub

    Public Shared Sub ObexPutFile()
        ' The host part of the URI is the device address, e.g. IrDAAddress.ToString(),
        ' and the file part is the OBEX object name.
        Dim addr As String = "112233445566"
        Dim uri As New Uri("obex://" & addr & "/HelloWorld.txt")
        Dim req As New ObexWebRequest(uri)
        req.ReadFile("Hello World.txt")
        Dim rsp As ObexWebResponse = CType(req.GetResponse(), ObexWebResponse)
        Console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode)
    End Sub

    Public Shared Sub ObexPutJpegFile()
        ' The host part of the URI is the device address, e.g. IrDAAddress.ToString(),
        ' and the file part is the OBEX object name.
        Dim uri As New Uri("obex://000e079502/image.jpg")
        Dim req As New ObexWebRequest(uri)
        req.ReadFile("D:\Documents and Settings\alan\My Documents\My Pictures\foo.PNG")
        Dim rsp As ObexWebResponse = CType(req.GetResponse(), ObexWebResponse)
        Console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode)
    End Sub

    Public Shared Sub ObexPutData()
        ' The host part of the URI is the device address, e.g. IrDAAddress.ToString(),
        ' and the file part is the OBEX object name.
        Dim addr As String = "112233445566"
        Dim uri As New Uri("obex://" & addr & "/HelloWorld2.txt")
        Dim req As New ObexWebRequest(uri)
        Using content As Stream = req.GetRequestStream()
            ' Using a StreamWriter to write text to the stream...
            Using wtr As New StreamWriter(content)
                wtr.WriteLine("Hello World GetRequestStream")
                wtr.WriteLine("Hello World GetRequestStream 2")
                wtr.Flush()
                ' Set the Length header value
                req.ContentLength = content.Length
            End Using
        End Using
        Dim rsp As ObexWebResponse = CType(req.GetResponse(), ObexWebResponse)
        Console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode)
    End Sub

    Public Shared Sub ObexGetData()
        ' The host part of the URI is the device address, e.g. IrDAAddress.ToString(),
        ' and the file part is the OBEX object name.
        Dim addr As String = "112233445566"
        Dim uri As New Uri("obex-ftp://" & addr & "/HelloWorld.txt")
        Dim req As New ObexWebRequest(uri)
        req.Method = "GET"
        Dim rsp As ObexWebResponse = CType(req.GetResponse(), ObexWebResponse)
        Console.WriteLine("Response Code: {0} (0x{0:X})", rsp.StatusCode)
        Using content As Stream = rsp.GetResponseStream()
            ' Using a StreamReader to read text from the stream...
            Using rdr As New StreamReader(content)
                While True
                    Dim line As String = rdr.ReadLine()
                    If line Is Nothing Then Exit While
                    Console.WriteLine(line)
                End While
            End Using
        End Using
    End Sub

    ' ==== BluetoothClient ====
    Shared Sub LoopUsing()
        Dim something As Boolean = True
        Dim ep As New BluetoothEndPoint(BluetoothAddress.None, BluetoothService.SerialPort)
        '
        While something
            Using cli As New BluetoothClient() ' create a new instance for each connection
                cli.Connect(ep)
                ' ... ...
                ' ... ...
            End Using
        End While
    End Sub

    ' ==== BluetoothListener ====
    Shared Sub BluetoothListener1()
        'Class MyConsts
        'Shared         ReadOnly 
        Dim _
        MyConsts___MyServiceUuid As Guid _
          = New Guid("{00112233-4455-6677-8899-aabbccddeeff}")
        'End Class

        '...
        Dim lsnr As New BluetoothListener(MyConsts___MyServiceUuid)
        lsnr.Start()
        ' Now accept new connections, perhaps using the thread pool to handle each
        Dim conn As BluetoothClient = lsnr.AcceptBluetoothClient()
        Dim peerStream As Stream = conn.GetStream()
        '
        '
        Console.ReadLine()
    End Sub

    Shared Sub BluetoothListener2ServiceName()
        'Class MyConsts
        'Shared         ReadOnly 
        Dim _
        MyConsts___MyServiceUuid As Guid _
          = New Guid("{00112233-4455-6677-8899-aabbccddeeff}")
        'End Class

        '...
        Dim lsnr As New BluetoothListener(MyConsts___MyServiceUuid)
        lsnr.ServiceName = "32feet.NET MiscFeatureTest"
        lsnr.Start()
        ' Now accept new connections, perhaps using the thread pool to handle each
        Dim conn As BluetoothClient = lsnr.AcceptBluetoothClient()
        Dim peerStream As Stream = conn.GetStream()
        '
        '
        Console.ReadLine()
    End Sub

    ' ==== Bluetooth Stacks/Radios ====
    Shared Sub CreateOnSpecificRadioStack()
        Dim radioList() As BluetoothRadio = BluetoothRadio.AllRadios
        If radioList.Length < 2 Then
            Trace.Fail("Only one radio!!")
        End If
        '
        Dim radioB As BluetoothRadio = BluetoothRadio.AllRadios(1)
        Dim cli As BluetoothClient = radioB.StackFactory.CreateBluetoothClient()
        Dim lsnr As BluetoothListener = radioB.StackFactory.CreateBluetoothListener(BluetoothService.Wap)
        Console.WriteLine("Successfully used StackFactory to create client and listener.")
    End Sub

    ' ==== Bluetooth SDP -- Read ====
    Public Shared Sub ConnectByServiceName()
        Dim expectedSvcName As String = "FooBar"
        Dim addrS As String = "001122334455"
        Dim commonClass As Guid = BluetoothService.SerialPort
        commonClass = BluetoothService.L2CapProtocol ' TESTING
        Dim addr As BluetoothAddress = BluetoothAddress.Parse(addrS)
        '
        Dim bdi As New BluetoothDeviceInfo(addr)
        Dim rcdList() As ServiceRecord = bdi.GetServiceRecords(commonClass)
        Dim curSvcName As String
        Dim portInteger As Integer = -1
        For Each record As ServiceRecord In rcdList
            portInteger = ServiceRecordHelper.GetRfcommChannelNumber(record)
            Try
                curSvcName = record.GetPrimaryMultiLanguageStringAttributeById(UniversalAttributeId.ServiceName)
            Catch ex As KeyNotFoundException ' No ServiceName
                Continue For
            End Try
            Debug.Assert(curSvcName IsNot Nothing, "null ServiceName!?")
            If expectedSvcName.Equals(curSvcName, StringComparison.InvariantCulture) Then
                If portInteger = -1 Then
                    Throw New InvalidOperationException("Selected Service is not RFCOMM")
                End If
                Exit For
            End If
        Next
        If portInteger = -1 Then
            Throw New InvalidOperationException("No Service found with the given ServiceName")
        End If
        Dim port As Byte = CByte(portInteger) ' convert to byte now we know that it's valid
        Dim rep As New BluetoothEndPoint(addr, BluetoothService.Empty, port)
        Dim cli As New BluetoothClient()
        cli.Connect(rep)
        cli.Close()
    End Sub

    ' ==== Bluetooth SDP -- Difficult Create====
    Shared Sub SdpListenerRecord()
        Dim serviceClassUuid As New Guid("{D91ED51C-B8A1-4ce7-B76A-3637327C5552}")
        '
        '
        Dim pdl As ServiceElement = ServiceRecordHelper.CreateRfcommProtocolDescriptorList()
        Dim classList As ServiceElement = New ServiceElement(ElementType.ElementSequence, _
          New ServiceElement(ElementType.Uuid128, serviceClassUuid))
        Dim recordXX As ServiceRecord = New ServiceRecord( _
          New ServiceAttribute(UniversalAttributeId.ServiceClassIdList, classList), _
          New ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl), _
          New ServiceAttribute(&H4000, New ServiceElement(ElementType.TextString, "foo")), _
          New ServiceAttribute(&H4001, ServiceElement.CreateNumericalServiceElement(ElementType.UInt16, 69)) _
        )
        '
        '
        Dim str As ServiceElement = New ServiceElement(ElementType.TextString, "hello world")
        Dim langBaseList As ServiceElement = CreateEnglishUtf8PrimaryLanguageServiceElement()
        '
        '...
        Dim record As ServiceRecord = New ServiceRecord( _
            New ServiceAttribute(UniversalAttributeId.ServiceClassIdList, classList), _
            New ServiceAttribute(UniversalAttributeId.ProtocolDescriptorList, pdl), _
 _
            New ServiceAttribute(UniversalAttributeId.LanguageBaseAttributeIdList, langBaseList), _
            New ServiceAttribute(ServiceRecord.CreateLanguageBasedAttributeId( _
                    UniversalAttributeId.ServiceName, _
                    LanguageBaseItem.PrimaryLanguageBaseAttributeId), _
            str) _
        )

        Dim lsnr As New BluetoothListener(serviceClassUuid, recordXX)
        lsnr.Start()
    End Sub

    Private Shared Function CreateEnglishUtf8PrimaryLanguageServiceElement() As ServiceElement
        Dim englishUtf8PrimaryLanguage As ServiceElement = LanguageBaseItem.CreateElementSequenceFromList( _
                New LanguageBaseItem() { _
                    New LanguageBaseItem("en", LanguageBaseItem.Utf8EncodingId, _
                         LanguageBaseItem.PrimaryLanguageBaseAttributeId) _
        })
        Return englishUtf8PrimaryLanguage
    End Function

    '====
    Sub BtWin32Auth()
        Using pairer As New BluetoothWin32Authentication(AddressOf Win32AuthCallbackHandler)
            Console.WriteLine("Hit Return to stop authenticating")
            Console.ReadLine()
        End Using
        ' ...
    End Sub

    Sub Win32AuthCallbackHandler(ByVal sender As Object, ByVal e As InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs)
        ' Note we assume here that 'Legacy' pairing is being used,
        ' and thus we only set the Pin property!
        Dim address As String = e.Device.DeviceAddress.ToString()
        Console.WriteLine("Received an authentication request from address " + address)
        '
        ' compare the first 8 hex numbers, this is just a special case because in the
        ' used scenario the model of the devices can be identified by the first 8 hex
        ' numbers, the last 4 numbers being the device specific part.
        If address.Substring(0, 8).Equals("0099880D") OrElse _
                address.Substring(0, 8).Equals("0099880E") Then
            ' send authentication response
            e.Pin = "5276"
        ElseIf (address.Substring(0, 8).Equals("00997788")) Then
            ' send authentication response
            e.Pin = "ásdfghjkl"
        End If
    End Sub


    ' untested!
    Sub HandlerWithSsp(ByVal sender As Object, ByVal e As InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs)
        If e.AuthenticationMethod = BluetoothAuthenticationMethod.Legacy Then
            ' Call the old method above
            Win32AuthCallbackHandler(sender, e)
        ElseIf e.JustWorksNumericComparison = True Then
            Dim rslt As DialogResult = MessageBox.Show("Allow device with address " & e.Device.DeviceAddress.ToString() & " to pair?")
            If rslt = DialogResult.Yes Then
                e.Confirm = True
            End If
        ElseIf e.AuthenticationMethod = BluetoothAuthenticationMethod.NumericComparison Then
            Dim rslt As DialogResult = MessageBox.Show("Device with address " & e.Device.DeviceAddress.ToString() & " is wanting to pair." & _
                  " Confirm that it is displaying this six-digit number on screen: " & e.NumberOrPasskeyAsString)
            If rslt = DialogResult.Yes Then
                e.Confirm = True
            End If
        ElseIf e.AuthenticationMethod = BluetoothAuthenticationMethod.Passkey Then
            Dim line As String = MyInputBox.Show("Device with address " & e.Device.DeviceAddress.ToString & " is wanting to pair." & _
                  " Please enter the six digit number that it is displaying on screen.")
            If line IsNot Nothing Then
                Dim pk As Integer = Integer.Parse(line)
                If pk >= 0 AndAlso pk < 1000000 Then
                    e.ResponseNumberOrPasskey = pk
                    e.Confirm = True
                End If
            End If
        Else
            ' TODO
        End If
    End Sub

    '--
    Class MyInputBox
        Public Shared Function Show(ByVal prompt As String) As String
            Throw New NotSupportedException
        End Function
    End Class

End Class



Class Misc
    Shared Sub FromJsr82ServerUri()
        Dim s As String = "btspp://localhost:00112233445566778899AABBCCDDEEFF;name=MyServiceName"
        Dim sr As ServiceRecord = ServiceRecordBuilder.FromJsr82ServerUri(s).ServiceRecord
        ServiceRecordUtilities.Dump(Console.Out, sr)
    End Sub

    '----------------
    Shared Sub GetPrimaryRadio()
        Console.WriteLine("---- GetPrimaryRadio ----")
        Dim myRadio As BluetoothRadio = BluetoothRadio.PrimaryRadio
        If myRadio Is Nothing Then
            Console.WriteLine("No radio hardware or unsupported software stack")
            Return
        End If
        Dim myAddr As BluetoothAddress = myRadio.LocalAddress
        Console.WriteLine("Local radio address is " & myAddr.ToString())
    End Sub

    '----------------
    Shared Sub Discovery()
        Console.WriteLine("---- Discovery ----")
        Dim startTime As DateTime = DateTime.UtcNow
        Dim deviceList() As BluetoothDeviceInfo = New BluetoothClient().DiscoverDevices()
        Dim endTime As DateTime = DateTime.UtcNow
        Dim len As Integer = deviceList.Length
        Console.WriteLine("found {0} devices", len)
        Console.WriteLine("----")
        DumpDeviceInfo(deviceList, startTime, endTime)
        Console.WriteLine("----")
    End Sub
    '-----------------
    Private Shared Sub DumpDeviceInfo(ByVal devices() As BluetoothDeviceInfo, _
            ByVal startTime As DateTime, ByVal endTime As DateTime)
        Dim sb As New System.Text.StringBuilder
        Dim localTime As DateTime = startTime.ToLocalTime
        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
            "Discovery process started at {0} UTC, {1} local, and ended at {2} UTC.", _
            startTime, localTime, endTime)
        sb.Append(vbCrLf + vbCrLf)
        Console.WriteLine(sb.ToString())
        For Each curDevice As BluetoothDeviceInfo In devices
            sb.Length = 0
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
                "* {0}", curDevice.DeviceName)
            sb.Append(vbCrLf)
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
                "Address: {0}", curDevice.DeviceAddress)
            sb.Append(vbCrLf)
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
                "Remembered: {2}, Authenticated: {0}, Connected: {1}", _
                curDevice.Authenticated, curDevice.Connected, curDevice.Remembered)
            sb.Append(vbCrLf)
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
                "LastSeen: {0}, LastUsed: {1}", _
                curDevice.LastSeen, curDevice.LastUsed)
            sb.Append(vbCrLf)
            DumpCodInfo(curDevice.ClassOfDevice, sb)
            sb.Append(vbCrLf)
            Console.WriteLine(sb.ToString())
        Next
    End Sub

    Shared Sub DumpCodInfo(ByVal cod As ClassOfDevice, ByVal sb As System.Text.StringBuilder)
        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
            "CoD: (0x{0:X6})", cod.Value, cod)
        sb.Append(vbCrLf)
        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
            " Device:  {0} (0x{1:X2}) / {2} (0x{3:X4})", cod.MajorDevice, CInt(cod.MajorDevice), cod.Device, CInt(cod.Device))
        sb.Append(vbCrLf)
        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, _
            " Service: {0} (0x{1:X2})", cod.Service, CInt(cod.Service))
        sb.Append(vbCrLf)
    End Sub
    '-----------------

    Shared Sub SelectBluetoothDeviceDialog()
        Console.WriteLine("---- SelectBluetoothDeviceDialog ----")
        Dim dlg As New InTheHand.Windows.Forms.SelectBluetoothDeviceDialog
        ShowSelectBluetoothDeviceDialog(dlg)
        '
        Dim f As Boolean
        f = dlg.ShowUnknown
        f = dlg.ShowRemembered
        f = dlg.ShowAuthenticated
        dlg.Reset()
        dlg.ShowUnknown = False
        ShowSelectBluetoothDeviceDialog(dlg)
    End Sub

    Shared Sub ShowSelectBluetoothDeviceDialog(ByVal dlg As InTheHand.Windows.Forms.SelectBluetoothDeviceDialog)
        Dim rslt As System.Windows.Forms.DialogResult = dlg.ShowDialog()
        Console.WriteLine("dialog result: {0}", rslt)
        Console.WriteLine("selected device: {0}", dlg.SelectedDevice)
        If Not dlg.SelectedDevice Is Nothing Then
            Console.WriteLine("  address: {0}", dlg.SelectedDevice.DeviceAddress)
            Console.WriteLine("  name   : {0}", dlg.SelectedDevice.DeviceName)
        End If
    End Sub

    Shared Sub SelectBluetoothDeviceDialog_WithFilter()
        '......
        Dim dlg As New InTheHand.Windows.Forms.SelectBluetoothDeviceDialog()
        dlg.DeviceFilter = AddressOf FilterDevice
        Dim rslt As DialogResult = dlg.ShowDialog()
        '...... 
    End Sub

    Shared Function FilterDevice(ByVal dev As BluetoothDeviceInfo) As Boolean
        Dim rslt As DialogResult = MessageBox.Show("Include this device " & dev.DeviceAddress.ToString & " " & dev.DeviceName, "FilterDevice", MessageBoxButtons.YesNo)
        Dim ret As Boolean = (DialogResult.Yes = rslt)
        Return ret
    End Function



    '----------------
    Shared Sub ObexListen()
        Dim lstnr As New ObexListener(ObexTransport.Bluetooth)
        lstnr.Start()
        Try
            Console.WriteLine("OBEX listening")
            Console.WriteLine("Hit Return to complete")
            Console.ReadLine()
        Finally
            lstnr.Stop()
        End Try
    End Sub

    '----------------
    Shared Sub ObexPushBluetoothUi()
        Dim dlg As New InTheHand.Windows.Forms.SelectBluetoothDeviceDialog
        Dim rslt As System.Windows.Forms.DialogResult = dlg.ShowDialog()
        If rslt = Windows.Forms.DialogResult.OK Then
            Dim device As BluetoothDeviceInfo = dlg.SelectedDevice
            Dim addr As String = device.DeviceAddress.ToString()
            ObexPushAddressString(addr)
        End If
    End Sub
    Shared Sub ObexPushAddressString(ByVal addressAsString As String)
        Dim uri As New Uri("obex-push://" & addressAsString & "/alan.txt")
        Dim owreq As ObexWebRequest = New ObexWebRequest(uri)
        Using reqContent As Stream = owreq.GetRequestStream()
            Using wtr As StreamWriter = New StreamWriter(reqContent)
                wtr.WriteLine("alan is cool")
            End Using
        End Using
        Dim rsp As ObexWebResponse = DirectCast(owreq.GetResponse(), ObexWebResponse)
    End Sub

End Class

Class Tests
    <TestFeature()> _
    Sub Win32AuthCallbackTwoSeparateAuthentications()
        Console.WriteLine("Authenticate two devices")
        Console.WriteLine("Passcode respectively: '{0}', '{1}', '{2}'", _
            "1234", "9876", "ásdfghjkl")
        Win32AuthCallback__(AddressOf Win32AuthCallbackTwoSeparateAuthenticationsHandler)
    End Sub

    Private Win32AuthCallbackTwoSeparateAuthentications_count As Integer
    Sub Win32AuthCallbackTwoSeparateAuthenticationsHandler(ByVal sender As Object, ByVal e As InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs)
        Console.WriteLine("Authenticating {0} {1}", e.Device.DeviceAddress, e.Device.DeviceName)
        Console.WriteLine("  Attempt# {0}, Last error code {1}", e.AttemptNumber, e.PreviousNativeErrorCode)
        '
        If Win32AuthCallbackTwoSeparateAuthentications_count = 0 Then
            e.Pin = "1234"
        ElseIf Win32AuthCallbackTwoSeparateAuthentications_count = 1 Then
            e.Pin = "9876"
        ElseIf Win32AuthCallbackTwoSeparateAuthentications_count = 2 Then
            e.Pin = "ásdfghjkl"
        End If
        Console.WriteLine("Using '{0}'", e.Pin)
        Win32AuthCallbackTwoSeparateAuthentications_count += 1
    End Sub

    Sub Win32AuthCallbackJohoHandler(ByVal sender As Object, ByVal e As InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs)
        Dim address As String = e.Device.DeviceAddress.ToString()
        Console.WriteLine("Received an authentication request from address " + address)

        ' compare the first 8 hex numbers, this is just a special case because in the
        ' used scenario the model of the devices can be identified by the first 8 hex
        ' numbers, the last 4 numbers being the device specific part.
        If address.Substring(0, 8).Equals("0099880D") OrElse _
                address.Substring(0, 8).Equals("0099880E") Then
            ' send authentication response
            e.Pin = "5276"
        ElseIf (address.Substring(0, 8).Equals("00997788")) Then
            ' send authentication response
            e.Pin = "1423"
        End If
    End Sub


    ' The output of a standard run is the following.  Note that the error code from
    ' the initial wrong response is error code 1244 which is ErrorNotAuthenticated.
    ' However when we try another attempt we get error code 1167 which is
    ' ErrorDeviceNotConnected.  So for the devices I've tested it's apparently not
    ' worth trying a second response.  If the user attempts to send another PIN-
    ' response (by setting the Pin field in the event args) then it will be ignored
    ' (we probably should have the event args throw when Pin is set in that case).
    ' 
    '[[
    ' Local radio address is 008099244999
    ' Authenticate two devices
    ' Passcode respectively: '1234', '9876', 'ásdfghjkl'
    ' Making PC discoverable
    ' Hit Return to complete
    ' 
    ' Complete
    ' Authenticate one device -- with wrong passcode here the first two times.
    ' Passcode respectively: 'BAD-x', 'BAD-y', '9876'
    ' Making PC discoverable
    ' Hit Return to complete
    ' Authenticating 000A99686999 Win2kBelkin
    '   Attempt# 0, Last error code 0
    ' Using '0.88516242889927'
    ' Authenticating 000A99686999 Win2kBelkin
    '   Attempt# 1, Last error code 1244
    ' Using '0.59947050996100'
    ' Authenticating 000A99686999 Win2kBelkin
    '   Attempt# 2, Last error code 1167
    ' Using '9876'
    ']]
    <TestFeature()> _
    Sub Win32AuthCallbackInitialBadPasscodeAndRetry()
        Console.WriteLine("Authenticate one device -- with wrong passcode here the first two times.")
        Console.WriteLine("Passcode respectively: '{0}', '{1}', '{2}'", _
            "BAD-x", "BAD-y", "9876")
        Win32AuthCallback__(AddressOf Win32AuthCallbackInitialBadPasscodeAndRetryHandler)
    End Sub


    Sub Win32AuthCallback__(ByVal handler As System.EventHandler(Of BluetoothWin32AuthenticationEventArgs))
        Console.WriteLine("Making PC discoverable")
        Dim radio As InTheHand.Net.Bluetooth.BluetoothRadio = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio
        Dim origRadioMode As InTheHand.Net.Bluetooth.RadioMode = radio.Mode
        radio.Mode = InTheHand.Net.Bluetooth.RadioMode.Discoverable
        '
        Using auther As New InTheHand.Net.Bluetooth.BluetoothWin32Authentication(handler)
            Console.WriteLine("Hit Return to complete")
            Console.ReadLine()
            Console.WriteLine("Complete")
        End Using
        radio.Mode = origRadioMode
    End Sub

    Private Win32AuthCallbackInitialBadPasscodeAndRetry_count As Integer
    Sub Win32AuthCallbackInitialBadPasscodeAndRetryHandler(ByVal sender As Object, ByVal e As InTheHand.Net.Bluetooth.BluetoothWin32AuthenticationEventArgs)
        Console.WriteLine("Authenticating {0} {1}", e.Device.DeviceAddress, e.Device.DeviceName)
        Console.WriteLine("  Attempt# {0}, Last error code {1}", e.AttemptNumber, e.PreviousNativeErrorCode)
        If e.AttemptNumber <> Win32AuthCallbackInitialBadPasscodeAndRetry_count Then
            Console.WriteLine("Bad AttemptNumber!!!")
        End If
        Dim rnd As New Random()
        Dim badPasscode As String = rnd.NextDouble().ToString()
        badPasscode = badPasscode.Substring(0, Math.Min(16, badPasscode.Length))
        '
        If Win32AuthCallbackInitialBadPasscodeAndRetry_count = 0 _
                OrElse Win32AuthCallbackInitialBadPasscodeAndRetry_count = 1 Then
            e.Pin = badPasscode
            e.CallbackWithResult = True
        ElseIf Win32AuthCallbackInitialBadPasscodeAndRetry_count = 2 _
                OrElse Win32AuthCallbackInitialBadPasscodeAndRetry_count = 3 Then
            e.Pin = "9876"
            e.CallbackWithResult = True
        ElseIf Win32AuthCallbackInitialBadPasscodeAndRetry_count = 4 Then
            e.CallbackWithResult = True ' Try to *wrongly* get another callback!
        Else
            Console.WriteLine("Unexpected callback #{0}", Win32AuthCallbackInitialBadPasscodeAndRetry_count)
        End If
        Console.WriteLine("Using '{0}'", IIf(e.Pin Is Nothing, "<null>", e.Pin))
        Win32AuthCallbackInitialBadPasscodeAndRetry_count += 1
    End Sub


    <TestFeature()> _
    Sub Win32AuthCallbackInduceFinalisationFault()
        Console.WriteLine("Authenticate a device two times")
        Console.WriteLine("Making PC discoverable")
        Dim radio As InTheHand.Net.Bluetooth.BluetoothRadio = InTheHand.Net.Bluetooth.BluetoothRadio.PrimaryRadio
        Dim origRadioMode As InTheHand.Net.Bluetooth.RadioMode = radio.Mode
        radio.Mode = InTheHand.Net.Bluetooth.RadioMode.Discoverable
        '
        Dim auther As Object
        auther = New InTheHand.Net.Bluetooth.BluetoothWin32Authentication(AddressOf Win32AuthCallbackTwoSeparateAuthenticationsHandler)
        Console.WriteLine("Hit Return to clear and GC")
        Console.ReadLine()
        auther = "foo"
        GC.Collect()
        Console.WriteLine("Hit Return to complete")
        Console.ReadLine()
        Console.WriteLine("Complete")
        radio.Mode = origRadioMode
    End Sub
End Class


Module Security
    Sub BtCli_SetPin()
        Dim cli As New BluetoothClient
        cli.SetPin("pass phrase")
    End Sub
End Module

#If LIVE_DISCO Then
Module LiveDisco

    Sub One()
        AddHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
        Console.ReadLine()
        RemoveHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
    End Sub

    Sub Two()
        AddHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
        AddHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
        Console.ReadLine()
        RemoveHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
        Console.ReadLine()
        RemoveHandler BluetoothLiveDiscovery.Foo, AddressOf HandleLD
    End Sub

    Private Sub HandleLD(ByVal sender As Object, ByVal e As LiveDiscoveryEventArgs)
        Console.WriteLine()
        Console.WriteLine("{0}", e)
        Console.WriteLine("    Gained: '{0}'", e.GainedFlags, e.LostFlags)
        Console.WriteLine("    Lost:   '{1}'", e.GainedFlags, e.LostFlags)
    End Sub
End Module
#End If


Class TestFeatureAttribute
    Inherits Attribute

    Public Sub New()
    End Sub

End Class
