<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents MenuItemMenuSdp As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQueryOpp As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQueryInstalled As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQueryAllL2cap As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6Sep As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuTestsSdp As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemTestRecordSdp As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDumpRecordsNameChannel As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemTestRecordOpp As System.Windows.Forms.MenuItem
    Friend WithEvents PanelClient As System.Windows.Forms.Panel
    Friend WithEvents PanelSdp As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ComboBoxDevices As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxUsePin As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxPin As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxEncrypt As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxAuthenticate As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxServices As System.Windows.Forms.ComboBox
    Private WithEvents comboBoxEncoding As System.Windows.Forms.ComboBox
    Private WithEvents labelState As System.Windows.Forms.Label
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents textBoxSend As System.Windows.Forms.TextBox
    Private WithEvents textBoxReceive As System.Windows.Forms.TextBox
    Friend WithEvents MenuItemTestDiscoveryUi As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQuit As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuSetService As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetSvcT As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetSvcF As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3Sep As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemAddTestDevices As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuTestsOther As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDumpRawRecords As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemRadioInfo As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSdpSaveText As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemNewBtCli As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemBtCliDiscover As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7Sep As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSdpCopy As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiscoverAll As System.Windows.Forms.MenuItem
    Private WithEvents MenuItemDiscoverNewOnly As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSeMmv100 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSecurityRemoveDevice As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSecurityPairRequest As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1Sepa As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuSecurity As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemQuerySelectedClass As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetRadioName As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiscoverAuthenticatedOnly As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiscoverRememberedOnly As System.Windows.Forms.MenuItem
    Friend WithEvents PanelServer As System.Windows.Forms.Panel
    Private WithEvents TextBoxServerReceive As System.Windows.Forms.TextBox
    Private WithEvents LabelServerState As System.Windows.Forms.Label
    Private WithEvents Label2 As System.Windows.Forms.Label
    Private WithEvents TextBoxServerSend As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxServerUsePin As System.Windows.Forms.CheckBox
    Friend WithEvents TextBoxServerPin As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxServerEncrypt As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxServerAuthenticate As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxServerServices As System.Windows.Forms.ComboBox
    Private WithEvents ComboBoxServerEncoding As System.Windows.Forms.ComboBox
    Friend WithEvents MenuItemMenuServer As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemServerListen As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemServerDisconnect As System.Windows.Forms.MenuItem
    Friend WithEvents CheckBoxServerLoopback As System.Windows.Forms.CheckBox
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemClientConnect As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemClientDisconnect As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuDiscover As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiscoverDiscoverableOnly As System.Windows.Forms.MenuItem
    Private WithEvents MenuItem4Sep As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemMenuLocal As System.Windows.Forms.MenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LabelDiscoveringState As System.Windows.Forms.Label
    Friend WithEvents MenuItemMenuView As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemViewSdp As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemViewClient As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemViewServer As System.Windows.Forms.MenuItem
    Friend WithEvents LabelTabSdp As System.Windows.Forms.Label
    Friend WithEvents LabelTabClient As System.Windows.Forms.Label
    Friend WithEvents LabelTabServer As System.Windows.Forms.Label
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiscoverDumpsRssi As System.Windows.Forms.MenuItem
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Me.MenuItemSetRadioName = New System.Windows.Forms.MenuItem
        Me.MenuItemDumpRawRecords = New System.Windows.Forms.MenuItem
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItemMenuView = New System.Windows.Forms.MenuItem
        Me.MenuItemViewSdp = New System.Windows.Forms.MenuItem
        Me.MenuItemViewClient = New System.Windows.Forms.MenuItem
        Me.MenuItemViewServer = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuDiscover = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverRememberedOnly = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverAuthenticatedOnly = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverAll = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverNewOnly = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverDiscoverableOnly = New System.Windows.Forms.MenuItem
        Me.MenuItemAsyncDiscoOnly = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItemDiscoverDumpsRssi = New System.Windows.Forms.MenuItem
        Me.MenuItemAddDeviceAddress = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuSdp = New System.Windows.Forms.MenuItem
        Me.MenuItemQueryOpp = New System.Windows.Forms.MenuItem
        Me.MenuItemQueryInstalled = New System.Windows.Forms.MenuItem
        Me.MenuItemQueryAllL2cap = New System.Windows.Forms.MenuItem
        Me.MenuItemQuerySelectedClass = New System.Windows.Forms.MenuItem
        Me.MenuItem6Sep = New System.Windows.Forms.MenuItem
        Me.MenuItemDumpRecordsNameChannel = New System.Windows.Forms.MenuItem
        Me.MenuItem7Sep = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuTestsSdp = New System.Windows.Forms.MenuItem
        Me.MenuItemTestRecordSdp = New System.Windows.Forms.MenuItem
        Me.MenuItemTestRecordOpp = New System.Windows.Forms.MenuItem
        Me.MenuItemSeMmv100 = New System.Windows.Forms.MenuItem
        Me.MenuItem4Sep = New System.Windows.Forms.MenuItem
        Me.MenuItemSdpSaveText = New System.Windows.Forms.MenuItem
        Me.MenuItemSdpCopy = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuLocal = New System.Windows.Forms.MenuItem
        Me.MenuItemRadioInfo = New System.Windows.Forms.MenuItem
        Me.MenuItemSetRadioMode = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuSetService = New System.Windows.Forms.MenuItem
        Me.MenuItemSetSvcT = New System.Windows.Forms.MenuItem
        Me.MenuItemSetSvcF = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuSecurity = New System.Windows.Forms.MenuItem
        Me.MenuItemSecurityPairRequest = New System.Windows.Forms.MenuItem
        Me.MenuItemSecurityRemoveDevice = New System.Windows.Forms.MenuItem
        Me.MenuItemSecuritySetPin = New System.Windows.Forms.MenuItem
        Me.MenuItemSecurityRevokePin = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.MenuItemClientConnect = New System.Windows.Forms.MenuItem
        Me.MenuItemClientDisconnect = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuServer = New System.Windows.Forms.MenuItem
        Me.MenuItemServerListen = New System.Windows.Forms.MenuItem
        Me.MenuItemServerDisconnect = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItemMenuTestsOther = New System.Windows.Forms.MenuItem
        Me.MenuItemTestDiscoveryUi = New System.Windows.Forms.MenuItem
        Me.MenuItemAddTestDevices = New System.Windows.Forms.MenuItem
        Me.MenuItem3Sep = New System.Windows.Forms.MenuItem
        Me.MenuItemNewBtCli = New System.Windows.Forms.MenuItem
        Me.MenuItemBtCliDiscover = New System.Windows.Forms.MenuItem
        Me.MenuItem1Sepa = New System.Windows.Forms.MenuItem
        Me.MenuItemQuit = New System.Windows.Forms.MenuItem
        Me.LabelTabSdp = New System.Windows.Forms.Label
        Me.PanelSdp = New System.Windows.Forms.Panel
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.PanelClient = New System.Windows.Forms.Panel
        Me.ComboBoxServices = New System.Windows.Forms.ComboBox
        Me.CheckBoxAuthenticate = New System.Windows.Forms.CheckBox
        Me.CheckBoxEncrypt = New System.Windows.Forms.CheckBox
        Me.CheckBoxUsePin = New System.Windows.Forms.CheckBox
        Me.TextBoxPin = New System.Windows.Forms.TextBox
        Me.comboBoxEncoding = New System.Windows.Forms.ComboBox
        Me.textBoxSend = New System.Windows.Forms.TextBox
        Me.textBoxReceive = New System.Windows.Forms.TextBox
        Me.LabelTabClient = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.labelState = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.PanelServer = New System.Windows.Forms.Panel
        Me.LabelTabServer = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBoxServerServices = New System.Windows.Forms.ComboBox
        Me.CheckBoxServerAuthenticate = New System.Windows.Forms.CheckBox
        Me.CheckBoxServerEncrypt = New System.Windows.Forms.CheckBox
        Me.CheckBoxServerUsePin = New System.Windows.Forms.CheckBox
        Me.TextBoxServerPin = New System.Windows.Forms.TextBox
        Me.ComboBoxServerEncoding = New System.Windows.Forms.ComboBox
        Me.CheckBoxServerLoopback = New System.Windows.Forms.CheckBox
        Me.TextBoxServerSend = New System.Windows.Forms.TextBox
        Me.TextBoxServerReceive = New System.Windows.Forms.TextBox
        Me.LabelServerState = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBoxDevices = New System.Windows.Forms.ComboBox
        Me.LabelDiscoveringState = New System.Windows.Forms.Label
        Me.PanelSdp.SuspendLayout()
        Me.PanelClient.SuspendLayout()
        Me.PanelServer.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuItemSetRadioName
        '
        Me.MenuItemSetRadioName.Text = "Set Radio Name"
        '
        'MenuItemDumpRawRecords
        '
        Me.MenuItemDumpRawRecords.Text = "Byte Array of last records"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.Add(Me.MenuItemMenuView)
        Me.MainMenu1.MenuItems.Add(Me.MenuItem14)
        '
        'MenuItemMenuView
        '
        Me.MenuItemMenuView.MenuItems.Add(Me.MenuItemViewSdp)
        Me.MenuItemMenuView.MenuItems.Add(Me.MenuItemViewClient)
        Me.MenuItemMenuView.MenuItems.Add(Me.MenuItemViewServer)
        Me.MenuItemMenuView.Text = "View"
        '
        'MenuItemViewSdp
        '
        Me.MenuItemViewSdp.Text = "SDP"
        '
        'MenuItemViewClient
        '
        Me.MenuItemViewClient.Text = "Client"
        '
        'MenuItemViewServer
        '
        Me.MenuItemViewServer.Text = "Server"
        '
        'MenuItem14
        '
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuDiscover)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuSdp)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuLocal)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuSecurity)
        Me.MenuItem14.MenuItems.Add(Me.MenuItem3)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuServer)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemMenuTestsOther)
        Me.MenuItem14.MenuItems.Add(Me.MenuItemQuit)
        Me.MenuItem14.Text = "Menu"
        '
        'MenuItemMenuDiscover
        '
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverRememberedOnly)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverAuthenticatedOnly)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverAll)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverNewOnly)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverDiscoverableOnly)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemAsyncDiscoOnly)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItem1)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemDiscoverDumpsRssi)
        Me.MenuItemMenuDiscover.MenuItems.Add(Me.MenuItemAddDeviceAddress)
        Me.MenuItemMenuDiscover.Text = "Discover"
        '
        'MenuItemDiscoverRememberedOnly
        '
        Me.MenuItemDiscoverRememberedOnly.Text = "&Remembered only"
        '
        'MenuItemDiscoverAuthenticatedOnly
        '
        Me.MenuItemDiscoverAuthenticatedOnly.Text = "&Authenticated only"
        '
        'MenuItemDiscoverAll
        '
        Me.MenuItemDiscoverAll.Text = "Discover &All"
        '
        'MenuItemDiscoverNewOnly
        '
        Me.MenuItemDiscoverNewOnly.Text = "&New only"
        '
        'MenuItemDiscoverDiscoverableOnly
        '
        Me.MenuItemDiscoverDiscoverableOnly.Text = "&Discoverable Only"
        '
        'MenuItemAsyncDiscoOnly
        '
        Me.MenuItemAsyncDiscoOnly.Text = "As&ync DiscoOnly"
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "-"
        '
        'MenuItemDiscoverDumpsRssi
        '
        Me.MenuItemDiscoverDumpsRssi.Text = "Include RSSI lookup"
        '
        'MenuItemAddDeviceAddress
        '
        Me.MenuItemAddDeviceAddress.Text = "Add Device Address"
        '
        'MenuItemMenuSdp
        '
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemQueryOpp)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemQueryInstalled)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemQueryAllL2cap)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemQuerySelectedClass)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItem6Sep)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemDumpRawRecords)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemDumpRecordsNameChannel)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItem7Sep)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemMenuTestsSdp)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItem4Sep)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemSdpSaveText)
        Me.MenuItemMenuSdp.MenuItems.Add(Me.MenuItemSdpCopy)
        Me.MenuItemMenuSdp.Text = "SDP"
        '
        'MenuItemQueryOpp
        '
        Me.MenuItemQueryOpp.Text = "OPP record"
        '
        'MenuItemQueryInstalled
        '
        Me.MenuItemQueryInstalled.Text = "InstalledServices"
        '
        'MenuItemQueryAllL2cap
        '
        Me.MenuItemQueryAllL2cap.Text = "All Services (over L2CAP)"
        '
        'MenuItemQuerySelectedClass
        '
        Me.MenuItemQuerySelectedClass.Text = "Selected Service Class"
        '
        'MenuItem6Sep
        '
        Me.MenuItem6Sep.Text = "-"
        '
        'MenuItemDumpRecordsNameChannel
        '
        Me.MenuItemDumpRecordsNameChannel.Text = "Name and Channel of last records"
        '
        'MenuItem7Sep
        '
        Me.MenuItem7Sep.Text = "-"
        '
        'MenuItemMenuTestsSdp
        '
        Me.MenuItemMenuTestsSdp.MenuItems.Add(Me.MenuItemTestRecordSdp)
        Me.MenuItemMenuTestsSdp.MenuItems.Add(Me.MenuItemTestRecordOpp)
        Me.MenuItemMenuTestsSdp.MenuItems.Add(Me.MenuItemSeMmv100)
        Me.MenuItemMenuTestsSdp.Text = "Tests SDP"
        '
        'MenuItemTestRecordSdp
        '
        Me.MenuItemTestRecordSdp.Text = "XP Sdp"
        '
        'MenuItemTestRecordOpp
        '
        Me.MenuItemTestRecordOpp.Text = "XP Opp"
        '
        'MenuItemSeMmv100
        '
        Me.MenuItemSeMmv100.Text = "MMV Bip etc"
        '
        'MenuItem4Sep
        '
        Me.MenuItem4Sep.Text = "-"
        '
        'MenuItemSdpSaveText
        '
        Me.MenuItemSdpSaveText.Text = "Save..."
        '
        'MenuItemSdpCopy
        '
        Me.MenuItemSdpCopy.Text = "Copy"
        '
        'MenuItemMenuLocal
        '
        Me.MenuItemMenuLocal.MenuItems.Add(Me.MenuItemRadioInfo)
        Me.MenuItemMenuLocal.MenuItems.Add(Me.MenuItemSetRadioMode)
        Me.MenuItemMenuLocal.MenuItems.Add(Me.MenuItemSetRadioName)
        Me.MenuItemMenuLocal.MenuItems.Add(Me.MenuItemMenuSetService)
        Me.MenuItemMenuLocal.Text = "Local"
        '
        'MenuItemRadioInfo
        '
        Me.MenuItemRadioInfo.Text = "Radio info"
        '
        'MenuItemSetRadioMode
        '
        Me.MenuItemSetRadioMode.Text = "Set Radio Mode"
        '
        'MenuItemMenuSetService
        '
        Me.MenuItemMenuSetService.MenuItems.Add(Me.MenuItemSetSvcT)
        Me.MenuItemMenuSetService.MenuItems.Add(Me.MenuItemSetSvcF)
        Me.MenuItemMenuSetService.Text = "SetService"
        '
        'MenuItemSetSvcT
        '
        Me.MenuItemSetSvcT.Text = "Enable"
        '
        'MenuItemSetSvcF
        '
        Me.MenuItemSetSvcF.Text = "Disable"
        '
        'MenuItemMenuSecurity
        '
        Me.MenuItemMenuSecurity.MenuItems.Add(Me.MenuItemSecurityPairRequest)
        Me.MenuItemMenuSecurity.MenuItems.Add(Me.MenuItemSecurityRemoveDevice)
        Me.MenuItemMenuSecurity.MenuItems.Add(Me.MenuItemSecuritySetPin)
        Me.MenuItemMenuSecurity.MenuItems.Add(Me.MenuItemSecurityRevokePin)
        Me.MenuItemMenuSecurity.Text = "Security"
        '
        'MenuItemSecurityPairRequest
        '
        Me.MenuItemSecurityPairRequest.Text = "Pair Request"
        '
        'MenuItemSecurityRemoveDevice
        '
        Me.MenuItemSecurityRemoveDevice.Text = "Remove Device"
        '
        'MenuItemSecuritySetPin
        '
        Me.MenuItemSecuritySetPin.Text = "Set PIN"
        '
        'MenuItemSecurityRevokePin
        '
        Me.MenuItemSecurityRevokePin.Text = "Revoke PIN"
        '
        'MenuItem3
        '
        Me.MenuItem3.MenuItems.Add(Me.MenuItemClientConnect)
        Me.MenuItem3.MenuItems.Add(Me.MenuItemClientDisconnect)
        Me.MenuItem3.MenuItems.Add(Me.MenuItem6)
        Me.MenuItem3.Text = "Client"
        '
        'MenuItemClientConnect
        '
        Me.MenuItemClientConnect.Text = "Connect"
        '
        'MenuItemClientDisconnect
        '
        Me.MenuItemClientDisconnect.Text = "Disconnect"
        '
        'MenuItem6
        '
        Me.MenuItem6.Text = "Send"
        '
        'MenuItemMenuServer
        '
        Me.MenuItemMenuServer.MenuItems.Add(Me.MenuItemServerListen)
        Me.MenuItemMenuServer.MenuItems.Add(Me.MenuItemServerDisconnect)
        Me.MenuItemMenuServer.MenuItems.Add(Me.MenuItem2)
        Me.MenuItemMenuServer.Text = "Server"
        '
        'MenuItemServerListen
        '
        Me.MenuItemServerListen.Text = "Listen"
        '
        'MenuItemServerDisconnect
        '
        Me.MenuItemServerDisconnect.Text = "Disconnect"
        '
        'MenuItem2
        '
        Me.MenuItem2.Text = "Send"
        '
        'MenuItemMenuTestsOther
        '
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItemTestDiscoveryUi)
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItem3Sep)
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItemNewBtCli)
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItemBtCliDiscover)
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItem1Sepa)
        Me.MenuItemMenuTestsOther.MenuItems.Add(Me.MenuItemAddTestDevices)
        Me.MenuItemMenuTestsOther.Text = "Tests other"
        '
        'MenuItemTestDiscoveryUi
        '
        Me.MenuItemTestDiscoveryUi.Text = "SelectDialog"
        '
        'MenuItemAddTestDevices
        '
        Me.MenuItemAddTestDevices.Text = "Add dummy devices"
        '
        'MenuItem3Sep
        '
        Me.MenuItem3Sep.Text = "-"
        '
        'MenuItemNewBtCli
        '
        Me.MenuItemNewBtCli.Text = "new BtCli"
        '
        'MenuItemBtCliDiscover
        '
        Me.MenuItemBtCliDiscover.Text = "BtCli.Discover"
        '
        'MenuItem1Sepa
        '
        Me.MenuItem1Sepa.Text = "-"
        '
        'MenuItemQuit
        '
        Me.MenuItemQuit.Text = "Exit"
        '
        'LabelTabSdp
        '
        Me.LabelTabSdp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTabSdp.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTabSdp.Location = New System.Drawing.Point(0, 217)
        Me.LabelTabSdp.Name = "LabelTabSdp"
        Me.LabelTabSdp.Size = New System.Drawing.Size(50, 20)
        Me.LabelTabSdp.Text = "SDP"
        '
        'PanelSdp
        '
        Me.PanelSdp.Controls.Add(Me.TextBox1)
        Me.PanelSdp.Controls.Add(Me.LabelTabSdp)
        Me.PanelSdp.Location = New System.Drawing.Point(0, 0)
        Me.PanelSdp.Name = "PanelSdp"
        Me.PanelSdp.Size = New System.Drawing.Size(240, 237)
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.TextBox1.Location = New System.Drawing.Point(3, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(233, 211)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "TextBox1"
        '
        'PanelClient
        '
        Me.PanelClient.Controls.Add(Me.ComboBoxServices)
        Me.PanelClient.Controls.Add(Me.CheckBoxAuthenticate)
        Me.PanelClient.Controls.Add(Me.CheckBoxEncrypt)
        Me.PanelClient.Controls.Add(Me.CheckBoxUsePin)
        Me.PanelClient.Controls.Add(Me.TextBoxPin)
        Me.PanelClient.Controls.Add(Me.comboBoxEncoding)
        Me.PanelClient.Controls.Add(Me.textBoxSend)
        Me.PanelClient.Controls.Add(Me.textBoxReceive)
        Me.PanelClient.Controls.Add(Me.LabelTabClient)
        Me.PanelClient.Controls.Add(Me.Label3)
        Me.PanelClient.Controls.Add(Me.labelState)
        Me.PanelClient.Controls.Add(Me.label8)
        Me.PanelClient.Location = New System.Drawing.Point(20, 20)
        Me.PanelClient.Name = "PanelClient"
        Me.PanelClient.Size = New System.Drawing.Size(240, 237)
        '
        'ComboBoxServices
        '
        Me.ComboBoxServices.Location = New System.Drawing.Point(3, 3)
        Me.ComboBoxServices.Name = "ComboBoxServices"
        Me.ComboBoxServices.Size = New System.Drawing.Size(226, 22)
        Me.ComboBoxServices.TabIndex = 0
        '
        'CheckBoxAuthenticate
        '
        Me.CheckBoxAuthenticate.Location = New System.Drawing.Point(3, 31)
        Me.CheckBoxAuthenticate.Name = "CheckBoxAuthenticate"
        Me.CheckBoxAuthenticate.Size = New System.Drawing.Size(55, 17)
        Me.CheckBoxAuthenticate.TabIndex = 1
        Me.CheckBoxAuthenticate.Text = "&Authenticate"
        '
        'CheckBoxEncrypt
        '
        Me.CheckBoxEncrypt.Location = New System.Drawing.Point(64, 31)
        Me.CheckBoxEncrypt.Name = "CheckBoxEncrypt"
        Me.CheckBoxEncrypt.Size = New System.Drawing.Size(62, 17)
        Me.CheckBoxEncrypt.TabIndex = 2
        Me.CheckBoxEncrypt.Text = "&Encrypt"
        '
        'CheckBoxUsePin
        '
        Me.CheckBoxUsePin.Location = New System.Drawing.Point(132, 31)
        Me.CheckBoxUsePin.Name = "CheckBoxUsePin"
        Me.CheckBoxUsePin.Size = New System.Drawing.Size(45, 17)
        Me.CheckBoxUsePin.TabIndex = 3
        Me.CheckBoxUsePin.Text = "&Pin"
        '
        'TextBoxPin
        '
        Me.TextBoxPin.Enabled = False
        Me.TextBoxPin.Location = New System.Drawing.Point(183, 27)
        Me.TextBoxPin.Name = "TextBoxPin"
        Me.TextBoxPin.Size = New System.Drawing.Size(46, 21)
        Me.TextBoxPin.TabIndex = 4
        '
        'comboBoxEncoding
        '
        Me.comboBoxEncoding.Items.Add("x-IA5")
        Me.comboBoxEncoding.Items.Add("iso-8859-1")
        Me.comboBoxEncoding.Items.Add("utf-8")
        Me.comboBoxEncoding.Items.Add("ASCII")
        Me.comboBoxEncoding.Location = New System.Drawing.Point(3, 54)
        Me.comboBoxEncoding.Name = "comboBoxEncoding"
        Me.comboBoxEncoding.Size = New System.Drawing.Size(83, 22)
        Me.comboBoxEncoding.TabIndex = 5
        '
        'textBoxSend
        '
        Me.textBoxSend.Location = New System.Drawing.Point(3, 98)
        Me.textBoxSend.Multiline = True
        Me.textBoxSend.Name = "textBoxSend"
        Me.textBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textBoxSend.Size = New System.Drawing.Size(174, 23)
        Me.textBoxSend.TabIndex = 6
        '
        'textBoxReceive
        '
        Me.textBoxReceive.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.textBoxReceive.Location = New System.Drawing.Point(3, 127)
        Me.textBoxReceive.Multiline = True
        Me.textBoxReceive.Name = "textBoxReceive"
        Me.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textBoxReceive.Size = New System.Drawing.Size(226, 83)
        Me.textBoxReceive.TabIndex = 7
        '
        'LabelTabClient
        '
        Me.LabelTabClient.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTabClient.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTabClient.Location = New System.Drawing.Point(0, 217)
        Me.LabelTabClient.Name = "LabelTabClient"
        Me.LabelTabClient.Size = New System.Drawing.Size(50, 20)
        Me.LabelTabClient.Text = "Client"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(183, 101)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 20)
        Me.Label3.Text = ": Send"
        '
        'labelState
        '
        Me.labelState.Location = New System.Drawing.Point(48, 82)
        Me.labelState.Name = "labelState"
        Me.labelState.Size = New System.Drawing.Size(172, 13)
        '
        'label8
        '
        Me.label8.Location = New System.Drawing.Point(3, 82)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(39, 13)
        Me.label8.Text = "State:"
        '
        'PanelServer
        '
        Me.PanelServer.Controls.Add(Me.LabelTabServer)
        Me.PanelServer.Controls.Add(Me.Label1)
        Me.PanelServer.Controls.Add(Me.ComboBoxServerServices)
        Me.PanelServer.Controls.Add(Me.CheckBoxServerAuthenticate)
        Me.PanelServer.Controls.Add(Me.CheckBoxServerEncrypt)
        Me.PanelServer.Controls.Add(Me.CheckBoxServerUsePin)
        Me.PanelServer.Controls.Add(Me.TextBoxServerPin)
        Me.PanelServer.Controls.Add(Me.ComboBoxServerEncoding)
        Me.PanelServer.Controls.Add(Me.CheckBoxServerLoopback)
        Me.PanelServer.Controls.Add(Me.TextBoxServerSend)
        Me.PanelServer.Controls.Add(Me.TextBoxServerReceive)
        Me.PanelServer.Controls.Add(Me.LabelServerState)
        Me.PanelServer.Controls.Add(Me.Label2)
        Me.PanelServer.Location = New System.Drawing.Point(40, 40)
        Me.PanelServer.Name = "PanelServer"
        Me.PanelServer.Size = New System.Drawing.Size(240, 237)
        '
        'LabelTabServer
        '
        Me.LabelTabServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelTabServer.BackColor = System.Drawing.SystemColors.Control
        Me.LabelTabServer.Location = New System.Drawing.Point(0, 217)
        Me.LabelTabServer.Name = "LabelTabServer"
        Me.LabelTabServer.Size = New System.Drawing.Size(50, 20)
        Me.LabelTabServer.Text = "Server"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(185, 101)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 20)
        Me.Label1.Text = ": Send"
        '
        'ComboBoxServerServices
        '
        Me.ComboBoxServerServices.Location = New System.Drawing.Point(5, 4)
        Me.ComboBoxServerServices.Name = "ComboBoxServerServices"
        Me.ComboBoxServerServices.Size = New System.Drawing.Size(226, 22)
        Me.ComboBoxServerServices.TabIndex = 2
        '
        'CheckBoxServerAuthenticate
        '
        Me.CheckBoxServerAuthenticate.Location = New System.Drawing.Point(5, 32)
        Me.CheckBoxServerAuthenticate.Name = "CheckBoxServerAuthenticate"
        Me.CheckBoxServerAuthenticate.Size = New System.Drawing.Size(55, 17)
        Me.CheckBoxServerAuthenticate.TabIndex = 3
        Me.CheckBoxServerAuthenticate.Text = "&Authenticate"
        '
        'CheckBoxServerEncrypt
        '
        Me.CheckBoxServerEncrypt.Location = New System.Drawing.Point(66, 32)
        Me.CheckBoxServerEncrypt.Name = "CheckBoxServerEncrypt"
        Me.CheckBoxServerEncrypt.Size = New System.Drawing.Size(62, 17)
        Me.CheckBoxServerEncrypt.TabIndex = 4
        Me.CheckBoxServerEncrypt.Text = "&Encrypt"
        '
        'CheckBoxServerUsePin
        '
        Me.CheckBoxServerUsePin.Location = New System.Drawing.Point(134, 32)
        Me.CheckBoxServerUsePin.Name = "CheckBoxServerUsePin"
        Me.CheckBoxServerUsePin.Size = New System.Drawing.Size(45, 17)
        Me.CheckBoxServerUsePin.TabIndex = 5
        Me.CheckBoxServerUsePin.Text = "&Pin"
        '
        'TextBoxServerPin
        '
        Me.TextBoxServerPin.AcceptsReturn = True
        Me.TextBoxServerPin.Enabled = False
        Me.TextBoxServerPin.Location = New System.Drawing.Point(185, 28)
        Me.TextBoxServerPin.Name = "TextBoxServerPin"
        Me.TextBoxServerPin.Size = New System.Drawing.Size(46, 21)
        Me.TextBoxServerPin.TabIndex = 6
        '
        'ComboBoxServerEncoding
        '
        Me.ComboBoxServerEncoding.Items.Add("x-IA5")
        Me.ComboBoxServerEncoding.Items.Add("iso-8859-1")
        Me.ComboBoxServerEncoding.Items.Add("utf-8")
        Me.ComboBoxServerEncoding.Items.Add("ASCII")
        Me.ComboBoxServerEncoding.Location = New System.Drawing.Point(5, 55)
        Me.ComboBoxServerEncoding.Name = "ComboBoxServerEncoding"
        Me.ComboBoxServerEncoding.Size = New System.Drawing.Size(83, 22)
        Me.ComboBoxServerEncoding.TabIndex = 7
        '
        'CheckBoxServerLoopback
        '
        Me.CheckBoxServerLoopback.Location = New System.Drawing.Point(131, 57)
        Me.CheckBoxServerLoopback.Name = "CheckBoxServerLoopback"
        Me.CheckBoxServerLoopback.Size = New System.Drawing.Size(100, 20)
        Me.CheckBoxServerLoopback.TabIndex = 8
        Me.CheckBoxServerLoopback.Text = "Loopback"
        '
        'TextBoxServerSend
        '
        Me.TextBoxServerSend.Location = New System.Drawing.Point(5, 99)
        Me.TextBoxServerSend.Multiline = True
        Me.TextBoxServerSend.Name = "TextBoxServerSend"
        Me.TextBoxServerSend.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxServerSend.Size = New System.Drawing.Size(174, 23)
        Me.TextBoxServerSend.TabIndex = 9
        '
        'TextBoxServerReceive
        '
        Me.TextBoxServerReceive.AcceptsReturn = True
        Me.TextBoxServerReceive.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxServerReceive.Location = New System.Drawing.Point(5, 128)
        Me.TextBoxServerReceive.Multiline = True
        Me.TextBoxServerReceive.Name = "TextBoxServerReceive"
        Me.TextBoxServerReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxServerReceive.Size = New System.Drawing.Size(226, 83)
        Me.TextBoxServerReceive.TabIndex = 10
        '
        'LabelServerState
        '
        Me.LabelServerState.Location = New System.Drawing.Point(50, 83)
        Me.LabelServerState.Name = "LabelServerState"
        Me.LabelServerState.Size = New System.Drawing.Size(172, 13)
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(5, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.Text = "State:"
        '
        'ComboBoxDevices
        '
        Me.ComboBoxDevices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxDevices.Location = New System.Drawing.Point(82, 243)
        Me.ComboBoxDevices.Name = "ComboBoxDevices"
        Me.ComboBoxDevices.Size = New System.Drawing.Size(155, 22)
        Me.ComboBoxDevices.TabIndex = 4
        '
        'LabelDiscoveringState
        '
        Me.LabelDiscoveringState.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelDiscoveringState.Location = New System.Drawing.Point(0, 245)
        Me.LabelDiscoveringState.Name = "LabelDiscoveringState"
        Me.LabelDiscoveringState.Size = New System.Drawing.Size(76, 20)
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.PanelServer)
        Me.Controls.Add(Me.PanelClient)
        Me.Controls.Add(Me.PanelSdp)
        Me.Controls.Add(Me.LabelDiscoveringState)
        Me.Controls.Add(Me.ComboBoxDevices)
        Me.Menu = Me.MainMenu1
        Me.Name = "Form1"
        Me.Text = "SdpBrowser"
        Me.PanelSdp.ResumeLayout(False)
        Me.PanelClient.ResumeLayout(False)
        Me.PanelServer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuItemAsyncDiscoOnly As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemAddDeviceAddress As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSetRadioMode As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSecuritySetPin As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemSecurityRevokePin As System.Windows.Forms.MenuItem


End Class
