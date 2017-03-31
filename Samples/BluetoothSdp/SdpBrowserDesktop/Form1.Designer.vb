' 32feet.NET - Personal Area Networking for .NET
'
' InTheHand.Net.Bluetooth.BluetoothRadio
' 
' Copyright (c) 2007 In The Hand Ltd, All rights reserved.
' Copyright (c) 2007 Alan J McFarlane.
' This source code is licensed under the In The Hand Community License - see License.txt

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
        Me.components = New System.ComponentModel.Container
        Dim MenuItemQueryOpp As System.Windows.Forms.ToolStripMenuItem
        Dim MenuItemQueryInstalled As System.Windows.Forms.ToolStripMenuItem
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPageSdp = New System.Windows.Forms.TabPage
        Me.LabelDiscoveringState = New System.Windows.Forms.Label
        Me.ComboBoxDevices = New System.Windows.Forms.ComboBox
        Me.BluetoothDeviceInfoBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ButtonNameAndChannelOfLastRecords = New System.Windows.Forms.Button
        Me.ButtonAllOverL2cap = New System.Windows.Forms.Button
        Me.ButtonBytesOfLastRecords = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ButtonGetOppRecord = New System.Windows.Forms.Button
        Me.ButtonGetInstalledServices = New System.Windows.Forms.Button
        Me.TabPageClient = New System.Windows.Forms.TabPage
        Me.LabelClientDiscoveringState = New System.Windows.Forms.Label
        Me.ButtonSetSvcF = New System.Windows.Forms.Button
        Me.ButtonSetSvcT = New System.Windows.Forms.Button
        Me.ButtonTestDiscoveryUi = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBoxClientDevices = New System.Windows.Forms.ComboBox
        Me.CheckBoxUsePin = New System.Windows.Forms.CheckBox
        Me.TextBoxPin = New System.Windows.Forms.TextBox
        Me.CheckBoxEncrypt = New System.Windows.Forms.CheckBox
        Me.CheckBoxAuthenticate = New System.Windows.Forms.CheckBox
        Me.ComboBoxServices = New System.Windows.Forms.ComboBox
        Me.label7 = New System.Windows.Forms.Label
        Me.labelState = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.buttonDisconnect = New System.Windows.Forms.Button
        Me.comboBoxEncoding = New System.Windows.Forms.ComboBox
        Me.labelSendPduLength = New System.Windows.Forms.Label
        Me.label6 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.textBoxReceive = New System.Windows.Forms.TextBox
        Me.buttonSend = New System.Windows.Forms.Button
        Me.textBoxSend = New System.Windows.Forms.TextBox
        Me.buttonConnect = New System.Windows.Forms.Button
        Me.TabPageServer = New System.Windows.Forms.TabPage
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBoxServerCodServices = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.CheckBoxServerLoopback = New System.Windows.Forms.CheckBox
        Me.CheckBoxServerUsePin = New System.Windows.Forms.CheckBox
        Me.TextBoxServerPin = New System.Windows.Forms.TextBox
        Me.CheckBoxServerEncrypt = New System.Windows.Forms.CheckBox
        Me.CheckBoxServerAuthenticate = New System.Windows.Forms.CheckBox
        Me.ComboBoxServerServices = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.LabelServerState = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ButtonServerDisconnect = New System.Windows.Forms.Button
        Me.ComboBoxServerEncoding = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextBoxServerReceive = New System.Windows.Forms.TextBox
        Me.ButtonServerSend = New System.Windows.Forms.Button
        Me.TextBoxServerSend = New System.Windows.Forms.TextBox
        Me.ButtonServerListen = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.DiscoveryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDiscoverRememberedOnly = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDiscoverAuthenticatedOnly = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDiscoverAll = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDiscoverNewOnly = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDiscoverDiscoverableOnly = New System.Windows.Forms.ToolStripMenuItem
        Me.AsyncDiscoOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.MenuItemDiscoverDumpsRssi = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemAddDeviceAddress = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemMenuSdp = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemQueryAllL2cap = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemQuerySelectedClass = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.MenuItemDumpRawRecordsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemDumpRecordsNameChannelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemMenuSetService = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSetSvcTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSetSvcFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TestsotherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectDialogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemAddTestDevices = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemRadioInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1Sep = New System.Windows.Forms.ToolStripSeparator
        Me.MenuItemNewBtCli = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemBtCliDiscover = New System.Windows.Forms.ToolStripMenuItem
        Me.DiscoverAsyncToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.MenuItemSetRadioName = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSetRadioMode = New System.Windows.Forms.ToolStripMenuItem
        Me.SecurityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSecurityPairRequest = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSecurityRemoveDevice = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemWin32Auth = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSecuritySetPin = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSecurityRevokePin = New System.Windows.Forms.ToolStripMenuItem
        MenuItemQueryOpp = New System.Windows.Forms.ToolStripMenuItem
        MenuItemQueryInstalled = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1.SuspendLayout()
        Me.TabPageSdp.SuspendLayout()
        CType(Me.BluetoothDeviceInfoBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageClient.SuspendLayout()
        Me.TabPageServer.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuItemQueryOpp
        '
        MenuItemQueryOpp.Name = "MenuItemQueryOpp"
        MenuItemQueryOpp.Size = New System.Drawing.Size(236, 22)
        MenuItemQueryOpp.Text = "OPP record"
        AddHandler MenuItemQueryOpp.Click, AddressOf Me.ButtonGetOppRecord_Click
        '
        'MenuItemQueryInstalled
        '
        MenuItemQueryInstalled.Name = "MenuItemQueryInstalled"
        MenuItemQueryInstalled.Size = New System.Drawing.Size(236, 22)
        MenuItemQueryInstalled.Text = "InstalledServices"
        AddHandler MenuItemQueryInstalled.Click, AddressOf Me.ButtonGetInstalledServices_Click
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageSdp)
        Me.TabControl1.Controls.Add(Me.TabPageClient)
        Me.TabControl1.Controls.Add(Me.TabPageServer)
        Me.TabControl1.Location = New System.Drawing.Point(0, 27)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(388, 499)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageSdp
        '
        Me.TabPageSdp.Controls.Add(Me.LabelDiscoveringState)
        Me.TabPageSdp.Controls.Add(Me.ComboBoxDevices)
        Me.TabPageSdp.Controls.Add(Me.ButtonNameAndChannelOfLastRecords)
        Me.TabPageSdp.Controls.Add(Me.ButtonAllOverL2cap)
        Me.TabPageSdp.Controls.Add(Me.ButtonBytesOfLastRecords)
        Me.TabPageSdp.Controls.Add(Me.TextBox1)
        Me.TabPageSdp.Controls.Add(Me.ButtonGetOppRecord)
        Me.TabPageSdp.Controls.Add(Me.ButtonGetInstalledServices)
        Me.TabPageSdp.Location = New System.Drawing.Point(4, 22)
        Me.TabPageSdp.Name = "TabPageSdp"
        Me.TabPageSdp.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageSdp.Size = New System.Drawing.Size(380, 473)
        Me.TabPageSdp.TabIndex = 0
        Me.TabPageSdp.Text = "SDP"
        Me.TabPageSdp.UseVisualStyleBackColor = True
        '
        'LabelDiscoveringState
        '
        Me.LabelDiscoveringState.Location = New System.Drawing.Point(8, 12)
        Me.LabelDiscoveringState.Name = "LabelDiscoveringState"
        Me.LabelDiscoveringState.Size = New System.Drawing.Size(76, 18)
        Me.LabelDiscoveringState.TabIndex = 0
        Me.LabelDiscoveringState.Text = "De&vices:"
        '
        'ComboBoxDevices
        '
        Me.ComboBoxDevices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxDevices.DataSource = Me.BluetoothDeviceInfoBindingSource
        Me.ComboBoxDevices.DisplayMember = "DeviceName"
        Me.ComboBoxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDevices.FormattingEnabled = True
        Me.ComboBoxDevices.Location = New System.Drawing.Point(90, 9)
        Me.ComboBoxDevices.Name = "ComboBoxDevices"
        Me.ComboBoxDevices.Size = New System.Drawing.Size(282, 21)
        Me.ComboBoxDevices.TabIndex = 1
        '
        'BluetoothDeviceInfoBindingSource
        '
        Me.BluetoothDeviceInfoBindingSource.DataSource = GetType(InTheHand.Net.Sockets.BluetoothDeviceInfo)
        '
        'ButtonNameAndChannelOfLastRecords
        '
        Me.ButtonNameAndChannelOfLastRecords.Location = New System.Drawing.Point(8, 64)
        Me.ButtonNameAndChannelOfLastRecords.Name = "ButtonNameAndChannelOfLastRecords"
        Me.ButtonNameAndChannelOfLastRecords.Size = New System.Drawing.Size(177, 23)
        Me.ButtonNameAndChannelOfLastRecords.TabIndex = 5
        Me.ButtonNameAndChannelOfLastRecords.Text = "Name and Channel of last records"
        Me.ButtonNameAndChannelOfLastRecords.UseVisualStyleBackColor = True
        '
        'ButtonAllOverL2cap
        '
        Me.ButtonAllOverL2cap.Location = New System.Drawing.Point(192, 36)
        Me.ButtonAllOverL2cap.Name = "ButtonAllOverL2cap"
        Me.ButtonAllOverL2cap.Size = New System.Drawing.Size(139, 23)
        Me.ButtonAllOverL2cap.TabIndex = 4
        Me.ButtonAllOverL2cap.Text = "All Services (over L2CAP)"
        Me.ButtonAllOverL2cap.UseVisualStyleBackColor = True
        '
        'ButtonBytesOfLastRecords
        '
        Me.ButtonBytesOfLastRecords.Location = New System.Drawing.Point(192, 65)
        Me.ButtonBytesOfLastRecords.Name = "ButtonBytesOfLastRecords"
        Me.ButtonBytesOfLastRecords.Size = New System.Drawing.Size(139, 23)
        Me.ButtonBytesOfLastRecords.TabIndex = 6
        Me.ButtonBytesOfLastRecords.Text = "Byte Array of last records"
        Me.ButtonBytesOfLastRecords.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(3, 94)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(374, 376)
        Me.TextBox1.TabIndex = 7
        '
        'ButtonGetOppRecord
        '
        Me.ButtonGetOppRecord.Location = New System.Drawing.Point(9, 36)
        Me.ButtonGetOppRecord.Name = "ButtonGetOppRecord"
        Me.ButtonGetOppRecord.Size = New System.Drawing.Size(75, 23)
        Me.ButtonGetOppRecord.TabIndex = 2
        Me.ButtonGetOppRecord.Text = "OPP record"
        Me.ButtonGetOppRecord.UseVisualStyleBackColor = True
        '
        'ButtonGetInstalledServices
        '
        Me.ButtonGetInstalledServices.Location = New System.Drawing.Point(90, 36)
        Me.ButtonGetInstalledServices.Name = "ButtonGetInstalledServices"
        Me.ButtonGetInstalledServices.Size = New System.Drawing.Size(95, 23)
        Me.ButtonGetInstalledServices.TabIndex = 3
        Me.ButtonGetInstalledServices.Text = "InstalledServices"
        Me.ButtonGetInstalledServices.UseVisualStyleBackColor = True
        '
        'TabPageClient
        '
        Me.TabPageClient.Controls.Add(Me.LabelClientDiscoveringState)
        Me.TabPageClient.Controls.Add(Me.ButtonSetSvcF)
        Me.TabPageClient.Controls.Add(Me.ButtonSetSvcT)
        Me.TabPageClient.Controls.Add(Me.ButtonTestDiscoveryUi)
        Me.TabPageClient.Controls.Add(Me.Label2)
        Me.TabPageClient.Controls.Add(Me.ComboBoxClientDevices)
        Me.TabPageClient.Controls.Add(Me.CheckBoxUsePin)
        Me.TabPageClient.Controls.Add(Me.TextBoxPin)
        Me.TabPageClient.Controls.Add(Me.CheckBoxEncrypt)
        Me.TabPageClient.Controls.Add(Me.CheckBoxAuthenticate)
        Me.TabPageClient.Controls.Add(Me.ComboBoxServices)
        Me.TabPageClient.Controls.Add(Me.label7)
        Me.TabPageClient.Controls.Add(Me.labelState)
        Me.TabPageClient.Controls.Add(Me.label8)
        Me.TabPageClient.Controls.Add(Me.buttonDisconnect)
        Me.TabPageClient.Controls.Add(Me.comboBoxEncoding)
        Me.TabPageClient.Controls.Add(Me.labelSendPduLength)
        Me.TabPageClient.Controls.Add(Me.label6)
        Me.TabPageClient.Controls.Add(Me.label5)
        Me.TabPageClient.Controls.Add(Me.textBoxReceive)
        Me.TabPageClient.Controls.Add(Me.buttonSend)
        Me.TabPageClient.Controls.Add(Me.textBoxSend)
        Me.TabPageClient.Controls.Add(Me.buttonConnect)
        Me.TabPageClient.Location = New System.Drawing.Point(4, 22)
        Me.TabPageClient.Name = "TabPageClient"
        Me.TabPageClient.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageClient.Size = New System.Drawing.Size(380, 473)
        Me.TabPageClient.TabIndex = 1
        Me.TabPageClient.Text = "Client"
        Me.TabPageClient.UseVisualStyleBackColor = True
        '
        'LabelClientDiscoveringState
        '
        Me.LabelClientDiscoveringState.Location = New System.Drawing.Point(8, 9)
        Me.LabelClientDiscoveringState.Name = "LabelClientDiscoveringState"
        Me.LabelClientDiscoveringState.Size = New System.Drawing.Size(76, 18)
        Me.LabelClientDiscoveringState.TabIndex = 0
        Me.LabelClientDiscoveringState.Text = "De&vices:"
        '
        'ButtonSetSvcF
        '
        Me.ButtonSetSvcF.Location = New System.Drawing.Point(217, 128)
        Me.ButtonSetSvcF.Name = "ButtonSetSvcF"
        Me.ButtonSetSvcF.Size = New System.Drawing.Size(60, 23)
        Me.ButtonSetSvcF.TabIndex = 21
        Me.ButtonSetSvcF.Text = "SetSvc &F"
        Me.ButtonSetSvcF.UseVisualStyleBackColor = True
        '
        'ButtonSetSvcT
        '
        Me.ButtonSetSvcT.Location = New System.Drawing.Point(150, 128)
        Me.ButtonSetSvcT.Name = "ButtonSetSvcT"
        Me.ButtonSetSvcT.Size = New System.Drawing.Size(61, 23)
        Me.ButtonSetSvcT.TabIndex = 20
        Me.ButtonSetSvcT.Text = "SetSvc &T"
        Me.ButtonSetSvcT.UseVisualStyleBackColor = True
        '
        'ButtonTestDiscoveryUi
        '
        Me.ButtonTestDiscoveryUi.Location = New System.Drawing.Point(290, 128)
        Me.ButtonTestDiscoveryUi.Name = "ButtonTestDiscoveryUi"
        Me.ButtonTestDiscoveryUi.Size = New System.Drawing.Size(82, 23)
        Me.ButtonTestDiscoveryUi.TabIndex = 19
        Me.ButtonTestDiscoveryUi.Text = "Discovery UI"
        Me.ButtonTestDiscoveryUi.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Service &UUID:"
        '
        'ComboBoxClientDevices
        '
        Me.ComboBoxClientDevices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxClientDevices.DataSource = Me.BluetoothDeviceInfoBindingSource
        Me.ComboBoxClientDevices.DisplayMember = "DeviceName"
        Me.ComboBoxClientDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxClientDevices.FormattingEnabled = True
        Me.ComboBoxClientDevices.Location = New System.Drawing.Point(90, 6)
        Me.ComboBoxClientDevices.Name = "ComboBoxClientDevices"
        Me.ComboBoxClientDevices.Size = New System.Drawing.Size(282, 21)
        Me.ComboBoxClientDevices.TabIndex = 1
        '
        'CheckBoxUsePin
        '
        Me.CheckBoxUsePin.AutoSize = True
        Me.CheckBoxUsePin.Location = New System.Drawing.Point(155, 71)
        Me.CheckBoxUsePin.Name = "CheckBoxUsePin"
        Me.CheckBoxUsePin.Size = New System.Drawing.Size(63, 17)
        Me.CheckBoxUsePin.TabIndex = 6
        Me.CheckBoxUsePin.Text = "Use &Pin"
        Me.CheckBoxUsePin.UseVisualStyleBackColor = True
        '
        'TextBoxPin
        '
        Me.TextBoxPin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxPin.Enabled = False
        Me.TextBoxPin.Location = New System.Drawing.Point(224, 69)
        Me.TextBoxPin.Name = "TextBoxPin"
        Me.TextBoxPin.Size = New System.Drawing.Size(150, 20)
        Me.TextBoxPin.TabIndex = 7
        '
        'CheckBoxEncrypt
        '
        Me.CheckBoxEncrypt.AutoSize = True
        Me.CheckBoxEncrypt.Location = New System.Drawing.Point(87, 71)
        Me.CheckBoxEncrypt.Name = "CheckBoxEncrypt"
        Me.CheckBoxEncrypt.Size = New System.Drawing.Size(62, 17)
        Me.CheckBoxEncrypt.TabIndex = 5
        Me.CheckBoxEncrypt.Text = "&Encrypt"
        Me.CheckBoxEncrypt.UseVisualStyleBackColor = True
        '
        'CheckBoxAuthenticate
        '
        Me.CheckBoxAuthenticate.AutoSize = True
        Me.CheckBoxAuthenticate.Location = New System.Drawing.Point(5, 71)
        Me.CheckBoxAuthenticate.Name = "CheckBoxAuthenticate"
        Me.CheckBoxAuthenticate.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxAuthenticate.TabIndex = 4
        Me.CheckBoxAuthenticate.Text = "&Authenticate"
        Me.CheckBoxAuthenticate.UseVisualStyleBackColor = True
        '
        'ComboBoxServices
        '
        Me.ComboBoxServices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxServices.FormattingEnabled = True
        Me.ComboBoxServices.Location = New System.Drawing.Point(90, 35)
        Me.ComboBoxServices.Name = "ComboBoxServices"
        Me.ComboBoxServices.Size = New System.Drawing.Size(284, 21)
        Me.ComboBoxServices.TabIndex = 3
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(2, 97)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(79, 13)
        Me.label7.TabIndex = 8
        Me.label7.Text = "Text &Encoding:"
        '
        'labelState
        '
        Me.labelState.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelState.Location = New System.Drawing.Point(48, 160)
        Me.labelState.Name = "labelState"
        Me.labelState.Size = New System.Drawing.Size(172, 13)
        Me.labelState.TabIndex = 13
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(3, 160)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(35, 13)
        Me.label8.TabIndex = 12
        Me.label8.Text = "State:"
        '
        'buttonDisconnect
        '
        Me.buttonDisconnect.AutoSize = True
        Me.buttonDisconnect.Location = New System.Drawing.Point(71, 128)
        Me.buttonDisconnect.Name = "buttonDisconnect"
        Me.buttonDisconnect.Size = New System.Drawing.Size(73, 23)
        Me.buttonDisconnect.TabIndex = 11
        Me.buttonDisconnect.Text = "&Disconnect"
        '
        'comboBoxEncoding
        '
        Me.comboBoxEncoding.FormattingEnabled = True
        Me.comboBoxEncoding.Items.AddRange(New Object() {"x-IA5", "iso-8859-1", "utf-8", "ASCII"})
        Me.comboBoxEncoding.Location = New System.Drawing.Point(87, 94)
        Me.comboBoxEncoding.Name = "comboBoxEncoding"
        Me.comboBoxEncoding.Size = New System.Drawing.Size(91, 21)
        Me.comboBoxEncoding.TabIndex = 9
        '
        'labelSendPduLength
        '
        Me.labelSendPduLength.AutoSize = True
        Me.labelSendPduLength.Location = New System.Drawing.Point(223, 230)
        Me.labelSendPduLength.Name = "labelSendPduLength"
        Me.labelSendPduLength.Size = New System.Drawing.Size(31, 13)
        Me.labelSendPduLength.TabIndex = 16
        Me.labelSendPduLength.Text = "9999"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(81, 227)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(132, 13)
        Me.label6.TabIndex = 16
        Me.label6.Text = "Maximum IrLMP send size:"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(3, 239)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(47, 13)
        Me.label5.TabIndex = 17
        Me.label5.Text = "&Receive"
        '
        'textBoxReceive
        '
        Me.textBoxReceive.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.textBoxReceive.Location = New System.Drawing.Point(3, 260)
        Me.textBoxReceive.Multiline = True
        Me.textBoxReceive.Name = "textBoxReceive"
        Me.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textBoxReceive.Size = New System.Drawing.Size(371, 210)
        Me.textBoxReceive.TabIndex = 18
        '
        'buttonSend
        '
        Me.buttonSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonSend.AutoSize = True
        Me.buttonSend.Location = New System.Drawing.Point(294, 187)
        Me.buttonSend.Name = "buttonSend"
        Me.buttonSend.Size = New System.Drawing.Size(78, 23)
        Me.buttonSend.TabIndex = 15
        Me.buttonSend.Text = "&Send"
        '
        'textBoxSend
        '
        Me.textBoxSend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.textBoxSend.Location = New System.Drawing.Point(3, 187)
        Me.textBoxSend.Multiline = True
        Me.textBoxSend.Name = "textBoxSend"
        Me.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textBoxSend.Size = New System.Drawing.Size(285, 36)
        Me.textBoxSend.TabIndex = 14
        '
        'buttonConnect
        '
        Me.buttonConnect.AutoSize = True
        Me.buttonConnect.Location = New System.Drawing.Point(3, 128)
        Me.buttonConnect.Name = "buttonConnect"
        Me.buttonConnect.Size = New System.Drawing.Size(62, 23)
        Me.buttonConnect.TabIndex = 10
        Me.buttonConnect.Text = "&Connect"
        '
        'TabPageServer
        '
        Me.TabPageServer.Controls.Add(Me.Label12)
        Me.TabPageServer.Controls.Add(Me.TextBoxServerCodServices)
        Me.TabPageServer.Controls.Add(Me.Label11)
        Me.TabPageServer.Controls.Add(Me.CheckBoxServerLoopback)
        Me.TabPageServer.Controls.Add(Me.CheckBoxServerUsePin)
        Me.TabPageServer.Controls.Add(Me.TextBoxServerPin)
        Me.TabPageServer.Controls.Add(Me.CheckBoxServerEncrypt)
        Me.TabPageServer.Controls.Add(Me.CheckBoxServerAuthenticate)
        Me.TabPageServer.Controls.Add(Me.ComboBoxServerServices)
        Me.TabPageServer.Controls.Add(Me.Label1)
        Me.TabPageServer.Controls.Add(Me.LabelServerState)
        Me.TabPageServer.Controls.Add(Me.Label3)
        Me.TabPageServer.Controls.Add(Me.ButtonServerDisconnect)
        Me.TabPageServer.Controls.Add(Me.ComboBoxServerEncoding)
        Me.TabPageServer.Controls.Add(Me.Label4)
        Me.TabPageServer.Controls.Add(Me.Label9)
        Me.TabPageServer.Controls.Add(Me.Label10)
        Me.TabPageServer.Controls.Add(Me.TextBoxServerReceive)
        Me.TabPageServer.Controls.Add(Me.ButtonServerSend)
        Me.TabPageServer.Controls.Add(Me.TextBoxServerSend)
        Me.TabPageServer.Controls.Add(Me.ButtonServerListen)
        Me.TabPageServer.Location = New System.Drawing.Point(4, 22)
        Me.TabPageServer.Name = "TabPageServer"
        Me.TabPageServer.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageServer.Size = New System.Drawing.Size(380, 473)
        Me.TabPageServer.TabIndex = 2
        Me.TabPageServer.Text = "Server"
        Me.TabPageServer.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(165, 122)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(75, 13)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "CoD Services:"
        '
        'TextBoxServerCodServices
        '
        Me.TextBoxServerCodServices.Location = New System.Drawing.Point(246, 119)
        Me.TextBoxServerCodServices.Name = "TextBoxServerCodServices"
        Me.TextBoxServerCodServices.ReadOnly = True
        Me.TextBoxServerCodServices.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxServerCodServices.TabIndex = 19
        Me.TextBoxServerCodServices.Text = "..."
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 23)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(76, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Service &UUID:"
        '
        'CheckBoxServerLoopback
        '
        Me.CheckBoxServerLoopback.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxServerLoopback.AutoSize = True
        Me.CheckBoxServerLoopback.Location = New System.Drawing.Point(295, 217)
        Me.CheckBoxServerLoopback.Name = "CheckBoxServerLoopback"
        Me.CheckBoxServerLoopback.Size = New System.Drawing.Size(74, 17)
        Me.CheckBoxServerLoopback.TabIndex = 13
        Me.CheckBoxServerLoopback.Text = "Loop&back"
        Me.CheckBoxServerLoopback.UseVisualStyleBackColor = True
        '
        'CheckBoxServerUsePin
        '
        Me.CheckBoxServerUsePin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxServerUsePin.AutoSize = True
        Me.CheckBoxServerUsePin.Location = New System.Drawing.Point(165, 70)
        Me.CheckBoxServerUsePin.Name = "CheckBoxServerUsePin"
        Me.CheckBoxServerUsePin.Size = New System.Drawing.Size(63, 17)
        Me.CheckBoxServerUsePin.TabIndex = 4
        Me.CheckBoxServerUsePin.Text = "Use &Pin"
        Me.CheckBoxServerUsePin.UseVisualStyleBackColor = True
        '
        'TextBoxServerPin
        '
        Me.TextBoxServerPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxServerPin.Enabled = False
        Me.TextBoxServerPin.Location = New System.Drawing.Point(234, 68)
        Me.TextBoxServerPin.Name = "TextBoxServerPin"
        Me.TextBoxServerPin.Size = New System.Drawing.Size(138, 20)
        Me.TextBoxServerPin.TabIndex = 5
        '
        'CheckBoxServerEncrypt
        '
        Me.CheckBoxServerEncrypt.AutoSize = True
        Me.CheckBoxServerEncrypt.Location = New System.Drawing.Point(97, 70)
        Me.CheckBoxServerEncrypt.Name = "CheckBoxServerEncrypt"
        Me.CheckBoxServerEncrypt.Size = New System.Drawing.Size(62, 17)
        Me.CheckBoxServerEncrypt.TabIndex = 3
        Me.CheckBoxServerEncrypt.Text = "&Encrypt"
        Me.CheckBoxServerEncrypt.UseVisualStyleBackColor = True
        '
        'CheckBoxServerAuthenticate
        '
        Me.CheckBoxServerAuthenticate.AutoSize = True
        Me.CheckBoxServerAuthenticate.Location = New System.Drawing.Point(5, 71)
        Me.CheckBoxServerAuthenticate.Name = "CheckBoxServerAuthenticate"
        Me.CheckBoxServerAuthenticate.Size = New System.Drawing.Size(86, 17)
        Me.CheckBoxServerAuthenticate.TabIndex = 2
        Me.CheckBoxServerAuthenticate.Text = "&Authenticate"
        Me.CheckBoxServerAuthenticate.UseVisualStyleBackColor = True
        '
        'ComboBoxServerServices
        '
        Me.ComboBoxServerServices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxServerServices.FormattingEnabled = True
        Me.ComboBoxServerServices.Location = New System.Drawing.Point(90, 20)
        Me.ComboBoxServerServices.Name = "ComboBoxServerServices"
        Me.ComboBoxServerServices.Size = New System.Drawing.Size(282, 21)
        Me.ComboBoxServerServices.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Text &Encoding:"
        '
        'LabelServerState
        '
        Me.LabelServerState.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelServerState.Location = New System.Drawing.Point(48, 160)
        Me.LabelServerState.Name = "LabelServerState"
        Me.LabelServerState.Size = New System.Drawing.Size(172, 13)
        Me.LabelServerState.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "State:"
        '
        'ButtonServerDisconnect
        '
        Me.ButtonServerDisconnect.AutoSize = True
        Me.ButtonServerDisconnect.Location = New System.Drawing.Point(82, 128)
        Me.ButtonServerDisconnect.Name = "ButtonServerDisconnect"
        Me.ButtonServerDisconnect.Size = New System.Drawing.Size(73, 23)
        Me.ButtonServerDisconnect.TabIndex = 9
        Me.ButtonServerDisconnect.Text = "&Disconnect"
        '
        'ComboBoxServerEncoding
        '
        Me.ComboBoxServerEncoding.FormattingEnabled = True
        Me.ComboBoxServerEncoding.Items.AddRange(New Object() {"x-IA5", "iso-8859-1", "utf-8", "ASCII"})
        Me.ComboBoxServerEncoding.Location = New System.Drawing.Point(87, 94)
        Me.ComboBoxServerEncoding.Name = "ComboBoxServerEncoding"
        Me.ComboBoxServerEncoding.Size = New System.Drawing.Size(91, 21)
        Me.ComboBoxServerEncoding.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(223, 230)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "9999"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(81, 227)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(132, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Maximum IrLMP send size:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 239)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "&Receive"
        '
        'TextBoxServerReceive
        '
        Me.TextBoxServerReceive.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxServerReceive.Location = New System.Drawing.Point(3, 260)
        Me.TextBoxServerReceive.Multiline = True
        Me.TextBoxServerReceive.Name = "TextBoxServerReceive"
        Me.TextBoxServerReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxServerReceive.Size = New System.Drawing.Size(371, 210)
        Me.TextBoxServerReceive.TabIndex = 17
        '
        'ButtonServerSend
        '
        Me.ButtonServerSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonServerSend.AutoSize = True
        Me.ButtonServerSend.Location = New System.Drawing.Point(294, 187)
        Me.ButtonServerSend.Name = "ButtonServerSend"
        Me.ButtonServerSend.Size = New System.Drawing.Size(78, 23)
        Me.ButtonServerSend.TabIndex = 12
        Me.ButtonServerSend.Text = "&Send"
        '
        'TextBoxServerSend
        '
        Me.TextBoxServerSend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxServerSend.Location = New System.Drawing.Point(3, 187)
        Me.TextBoxServerSend.Multiline = True
        Me.TextBoxServerSend.Name = "TextBoxServerSend"
        Me.TextBoxServerSend.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxServerSend.Size = New System.Drawing.Size(285, 36)
        Me.TextBoxServerSend.TabIndex = 11
        '
        'ButtonServerListen
        '
        Me.ButtonServerListen.AutoSize = True
        Me.ButtonServerListen.Location = New System.Drawing.Point(3, 128)
        Me.ButtonServerListen.Name = "ButtonServerListen"
        Me.ButtonServerListen.Size = New System.Drawing.Size(73, 23)
        Me.ButtonServerListen.TabIndex = 8
        Me.ButtonServerListen.Text = "&Listen"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DiscoveryToolStripMenuItem, Me.MenuItemMenuSdp, Me.MenuItemMenuSetService, Me.TestsotherToolStripMenuItem, Me.SecurityToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(388, 24)
        Me.MenuStrip1.TabIndex = 8
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DiscoveryToolStripMenuItem
        '
        Me.DiscoveryToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemDiscoverRememberedOnly, Me.MenuItemDiscoverAuthenticatedOnly, Me.MenuItemDiscoverAll, Me.MenuItemDiscoverNewOnly, Me.MenuItemDiscoverDiscoverableOnly, Me.AsyncDiscoOnlyToolStripMenuItem, Me.ToolStripSeparator2, Me.MenuItemDiscoverDumpsRssi, Me.MenuItemAddDeviceAddress})
        Me.DiscoveryToolStripMenuItem.Name = "DiscoveryToolStripMenuItem"
        Me.DiscoveryToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.DiscoveryToolStripMenuItem.Text = "&Discover"
        '
        'MenuItemDiscoverRememberedOnly
        '
        Me.MenuItemDiscoverRememberedOnly.Name = "MenuItemDiscoverRememberedOnly"
        Me.MenuItemDiscoverRememberedOnly.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverRememberedOnly.Text = "&Remembered only"
        '
        'MenuItemDiscoverAuthenticatedOnly
        '
        Me.MenuItemDiscoverAuthenticatedOnly.Name = "MenuItemDiscoverAuthenticatedOnly"
        Me.MenuItemDiscoverAuthenticatedOnly.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverAuthenticatedOnly.Text = "Authenticated only"
        Me.MenuItemDiscoverAuthenticatedOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MenuItemDiscoverAll
        '
        Me.MenuItemDiscoverAll.Name = "MenuItemDiscoverAll"
        Me.MenuItemDiscoverAll.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverAll.Text = "Discover &All"
        '
        'MenuItemDiscoverNewOnly
        '
        Me.MenuItemDiscoverNewOnly.Name = "MenuItemDiscoverNewOnly"
        Me.MenuItemDiscoverNewOnly.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverNewOnly.Text = "&New only"
        '
        'MenuItemDiscoverDiscoverableOnly
        '
        Me.MenuItemDiscoverDiscoverableOnly.Name = "MenuItemDiscoverDiscoverableOnly"
        Me.MenuItemDiscoverDiscoverableOnly.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverDiscoverableOnly.Text = "&Discoverable only"
        '
        'AsyncDiscoOnlyToolStripMenuItem
        '
        Me.AsyncDiscoOnlyToolStripMenuItem.Name = "AsyncDiscoOnlyToolStripMenuItem"
        Me.AsyncDiscoOnlyToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.AsyncDiscoOnlyToolStripMenuItem.Text = "As&ync DiscoOnly"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(167, 6)
        '
        'MenuItemDiscoverDumpsRssi
        '
        Me.MenuItemDiscoverDumpsRssi.CheckOnClick = True
        Me.MenuItemDiscoverDumpsRssi.Name = "MenuItemDiscoverDumpsRssi"
        Me.MenuItemDiscoverDumpsRssi.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemDiscoverDumpsRssi.Text = "Include Rssi lookup"
        '
        'MenuItemAddDeviceAddress
        '
        Me.MenuItemAddDeviceAddress.Name = "MenuItemAddDeviceAddress"
        Me.MenuItemAddDeviceAddress.Size = New System.Drawing.Size(170, 22)
        Me.MenuItemAddDeviceAddress.Text = "Add Device Address"
        '
        'MenuItemMenuSdp
        '
        Me.MenuItemMenuSdp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {MenuItemQueryOpp, MenuItemQueryInstalled, Me.MenuItemQueryAllL2cap, Me.MenuItemQuerySelectedClass, Me.ToolStripMenuItem1, Me.MenuItemDumpRawRecordsToolStripMenuItem, Me.MenuItemDumpRecordsNameChannelToolStripMenuItem})
        Me.MenuItemMenuSdp.Name = "MenuItemMenuSdp"
        Me.MenuItemMenuSdp.Size = New System.Drawing.Size(38, 20)
        Me.MenuItemMenuSdp.Text = "&SDP"
        '
        'MenuItemQueryAllL2cap
        '
        Me.MenuItemQueryAllL2cap.Name = "MenuItemQueryAllL2cap"
        Me.MenuItemQueryAllL2cap.Size = New System.Drawing.Size(236, 22)
        Me.MenuItemQueryAllL2cap.Text = "All Services (over L2CAP)"
        '
        'MenuItemQuerySelectedClass
        '
        Me.MenuItemQuerySelectedClass.Name = "MenuItemQuerySelectedClass"
        Me.MenuItemQuerySelectedClass.Size = New System.Drawing.Size(236, 22)
        Me.MenuItemQuerySelectedClass.Text = "Selected Service Class"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(233, 6)
        '
        'MenuItemDumpRawRecordsToolStripMenuItem
        '
        Me.MenuItemDumpRawRecordsToolStripMenuItem.Name = "MenuItemDumpRawRecordsToolStripMenuItem"
        Me.MenuItemDumpRawRecordsToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.MenuItemDumpRawRecordsToolStripMenuItem.Text = "Byte Array of last records"
        '
        'MenuItemDumpRecordsNameChannelToolStripMenuItem
        '
        Me.MenuItemDumpRecordsNameChannelToolStripMenuItem.Name = "MenuItemDumpRecordsNameChannelToolStripMenuItem"
        Me.MenuItemDumpRecordsNameChannelToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.MenuItemDumpRecordsNameChannelToolStripMenuItem.Text = "Name and Channel of last records"
        '
        'MenuItemMenuSetService
        '
        Me.MenuItemMenuSetService.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemSetSvcTToolStripMenuItem, Me.MenuItemSetSvcFToolStripMenuItem})
        Me.MenuItemMenuSetService.Name = "MenuItemMenuSetService"
        Me.MenuItemMenuSetService.Size = New System.Drawing.Size(70, 20)
        Me.MenuItemMenuSetService.Text = "SetSer&vice"
        '
        'MenuItemSetSvcTToolStripMenuItem
        '
        Me.MenuItemSetSvcTToolStripMenuItem.Name = "MenuItemSetSvcTToolStripMenuItem"
        Me.MenuItemSetSvcTToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.MenuItemSetSvcTToolStripMenuItem.Text = "Enable"
        '
        'MenuItemSetSvcFToolStripMenuItem
        '
        Me.MenuItemSetSvcFToolStripMenuItem.Name = "MenuItemSetSvcFToolStripMenuItem"
        Me.MenuItemSetSvcFToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.MenuItemSetSvcFToolStripMenuItem.Text = "Disable"
        '
        'TestsotherToolStripMenuItem
        '
        Me.TestsotherToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectDialogToolStripMenuItem, Me.MenuItemAddTestDevices, Me.MenuItemRadioInfo, Me.ToolStripMenuItem1Sep, Me.MenuItemNewBtCli, Me.MenuItemBtCliDiscover, Me.DiscoverAsyncToolStripMenuItem, Me.ToolStripSeparator1, Me.MenuItemSetRadioName, Me.MenuItemSetRadioMode})
        Me.TestsotherToolStripMenuItem.Name = "TestsotherToolStripMenuItem"
        Me.TestsotherToolStripMenuItem.Size = New System.Drawing.Size(74, 20)
        Me.TestsotherToolStripMenuItem.Text = "Tests &other"
        '
        'SelectDialogToolStripMenuItem
        '
        Me.SelectDialogToolStripMenuItem.Name = "SelectDialogToolStripMenuItem"
        Me.SelectDialogToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.SelectDialogToolStripMenuItem.Text = "&SelectDialog"
        '
        'MenuItemAddTestDevices
        '
        Me.MenuItemAddTestDevices.Name = "MenuItemAddTestDevices"
        Me.MenuItemAddTestDevices.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemAddTestDevices.Text = "Add dummy devices"
        '
        'MenuItemRadioInfo
        '
        Me.MenuItemRadioInfo.Name = "MenuItemRadioInfo"
        Me.MenuItemRadioInfo.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemRadioInfo.Text = "&Radio info"
        '
        'ToolStripMenuItem1Sep
        '
        Me.ToolStripMenuItem1Sep.Name = "ToolStripMenuItem1Sep"
        Me.ToolStripMenuItem1Sep.Size = New System.Drawing.Size(166, 6)
        '
        'MenuItemNewBtCli
        '
        Me.MenuItemNewBtCli.Name = "MenuItemNewBtCli"
        Me.MenuItemNewBtCli.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemNewBtCli.Text = "new BtCli"
        '
        'MenuItemBtCliDiscover
        '
        Me.MenuItemBtCliDiscover.Name = "MenuItemBtCliDiscover"
        Me.MenuItemBtCliDiscover.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemBtCliDiscover.Text = "BtCli.Discover"
        '
        'DiscoverAsyncToolStripMenuItem
        '
        Me.DiscoverAsyncToolStripMenuItem.Name = "DiscoverAsyncToolStripMenuItem"
        Me.DiscoverAsyncToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.DiscoverAsyncToolStripMenuItem.Text = "DiscoverAsync"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(166, 6)
        '
        'MenuItemSetRadioName
        '
        Me.MenuItemSetRadioName.Name = "MenuItemSetRadioName"
        Me.MenuItemSetRadioName.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemSetRadioName.Text = "Set Radio &Name"
        '
        'MenuItemSetRadioMode
        '
        Me.MenuItemSetRadioMode.Name = "MenuItemSetRadioMode"
        Me.MenuItemSetRadioMode.Size = New System.Drawing.Size(169, 22)
        Me.MenuItemSetRadioMode.Text = "Set Radio &Mode"
        '
        'SecurityToolStripMenuItem
        '
        Me.SecurityToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemSecurityPairRequest, Me.MenuItemSecurityRemoveDevice, Me.MenuItemWin32Auth, Me.MenuItemSecuritySetPin, Me.MenuItemSecurityRevokePin})
        Me.SecurityToolStripMenuItem.Name = "SecurityToolStripMenuItem"
        Me.SecurityToolStripMenuItem.Size = New System.Drawing.Size(58, 20)
        Me.SecurityToolStripMenuItem.Text = "Security"
        '
        'MenuItemSecurityPairRequest
        '
        Me.MenuItemSecurityPairRequest.Name = "MenuItemSecurityPairRequest"
        Me.MenuItemSecurityPairRequest.Size = New System.Drawing.Size(152, 22)
        Me.MenuItemSecurityPairRequest.Text = "Pair Request"
        '
        'MenuItemSecurityRemoveDevice
        '
        Me.MenuItemSecurityRemoveDevice.Name = "MenuItemSecurityRemoveDevice"
        Me.MenuItemSecurityRemoveDevice.Size = New System.Drawing.Size(152, 22)
        Me.MenuItemSecurityRemoveDevice.Text = "Remove Device"
        '
        'MenuItemWin32Auth
        '
        Me.MenuItemWin32Auth.Name = "MenuItemWin32Auth"
        Me.MenuItemWin32Auth.Size = New System.Drawing.Size(152, 22)
        Me.MenuItemWin32Auth.Text = "Win32Auth"
        '
        'MenuItemSecuritySetPin
        '
        Me.MenuItemSecuritySetPin.Name = "MenuItemSecuritySetPin"
        Me.MenuItemSecuritySetPin.Size = New System.Drawing.Size(152, 22)
        Me.MenuItemSecuritySetPin.Text = "Set PIN"
        '
        'MenuItemSecurityRevokePin
        '
        Me.MenuItemSecurityRevokePin.Name = "MenuItemSecurityRevokePin"
        Me.MenuItemSecurityRevokePin.Size = New System.Drawing.Size(152, 22)
        Me.MenuItemSecurityRevokePin.Text = "Revoke PIN"
        Me.MenuItemSecurityRevokePin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(388, 526)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "SdpBrowser"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageSdp.ResumeLayout(False)
        Me.TabPageSdp.PerformLayout()
        CType(Me.BluetoothDeviceInfoBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageClient.ResumeLayout(False)
        Me.TabPageClient.PerformLayout()
        Me.TabPageServer.ResumeLayout(False)
        Me.TabPageServer.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageSdp As System.Windows.Forms.TabPage
    Friend WithEvents TabPageClient As System.Windows.Forms.TabPage
    Friend WithEvents ButtonNameAndChannelOfLastRecords As System.Windows.Forms.Button
    Friend WithEvents ButtonAllOverL2cap As System.Windows.Forms.Button
    Friend WithEvents ButtonBytesOfLastRecords As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonGetOppRecord As System.Windows.Forms.Button
    Friend WithEvents ButtonGetInstalledServices As System.Windows.Forms.Button
    Private WithEvents labelState As System.Windows.Forms.Label
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents buttonDisconnect As System.Windows.Forms.Button
    Private WithEvents comboBoxEncoding As System.Windows.Forms.ComboBox
    Private WithEvents labelSendPduLength As System.Windows.Forms.Label
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents textBoxReceive As System.Windows.Forms.TextBox
    Private WithEvents buttonSend As System.Windows.Forms.Button
    Private WithEvents textBoxSend As System.Windows.Forms.TextBox
    Private WithEvents buttonConnect As System.Windows.Forms.Button
    Private WithEvents label7 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxEncrypt As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxAuthenticate As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxServices As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxPin As System.Windows.Forms.TextBox
    Friend WithEvents TabPageServer As System.Windows.Forms.TabPage
    Friend WithEvents CheckBoxUsePin As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxServerUsePin As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxServerPin As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxServerEncrypt As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxServerAuthenticate As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxServerServices As System.Windows.Forms.ComboBox
    Private WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents LabelServerState As System.Windows.Forms.Label
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents ButtonServerDisconnect As System.Windows.Forms.Button
    Private WithEvents ComboBoxServerEncoding As System.Windows.Forms.ComboBox
    Private WithEvents Label4 As System.Windows.Forms.Label
    Private WithEvents Label9 As System.Windows.Forms.Label
    Private WithEvents Label10 As System.Windows.Forms.Label
    Private WithEvents TextBoxServerReceive As System.Windows.Forms.TextBox
    Private WithEvents ButtonServerSend As System.Windows.Forms.Button
    Private WithEvents TextBoxServerSend As System.Windows.Forms.TextBox
    Private WithEvents ButtonServerListen As System.Windows.Forms.Button
    Friend WithEvents CheckBoxServerLoopback As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxDevices As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxClientDevices As System.Windows.Forms.ComboBox
    Friend WithEvents BluetoothDeviceInfoBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ButtonTestDiscoveryUi As System.Windows.Forms.Button
    Friend WithEvents ButtonSetSvcF As System.Windows.Forms.Button
    Friend WithEvents ButtonSetSvcT As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents TestsotherToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectDialogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemAddTestDevices As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRadioInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1Sep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuItemNewBtCli As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemBtCliDiscover As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemMenuSdp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemQueryAllL2cap As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuItemDumpRawRecordsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDumpRecordsNameChannelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemMenuSetService As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSetSvcTToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSetSvcFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiscoveryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDiscoverNewOnly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDiscoverAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextBoxServerCodServices As System.Windows.Forms.TextBox
    Friend WithEvents SecurityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSecurityPairRequest As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSecurityRemoveDevice As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuItemSetRadioName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemQuerySelectedClass As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDiscoverAuthenticatedOnly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDiscoverRememberedOnly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDiscoverDiscoverableOnly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuItemDiscoverDumpsRssi As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiscoverAsyncToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelDiscoveringState As System.Windows.Forms.Label
    Friend WithEvents LabelClientDiscoveringState As System.Windows.Forms.Label
    Friend WithEvents AsyncDiscoOnlyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemWin32Auth As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemAddDeviceAddress As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSetRadioMode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSecuritySetPin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSecurityRevokePin As System.Windows.Forms.ToolStripMenuItem

End Class
