using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace IOBluetooth
{
    public enum CompanyIdentifer : uint
    {
        EricssonTechnologyLicensing = 0,
        NokiaMobilePhones = 1,
        Intel = 2,
        Ibm = 3,
        Toshiba = 4,
        kBluetoothCompanyIdentifer3Com = 5,
        Microsoft = 6,
        Lucent = 7,
        Motorola = 8,
        InfineonTechnologiesAG = 9,
        CambridgeSiliconRadio = 10,
        SiliconWave = 11,
        DigianswerAS = 12,
        TexasInstruments = 13,
        ParthusTechnologies = 14,
        Broadcom = 15,
        MitelSemiconductor = 16,
        Widcomm = 17,
        Zeevo = 18,
        Atmel = 19,
        MistubishiElectric = 20,
        RTXTelecom = 21,
        KCTechnology = 22,
        Newlogic = 23,
        Transilica = 24,
        RohdeandSchwarz = 25,
        TTPCom = 26,
        SigniaTechnologies = 27,
        ConexantSystems = 28,
        Qualcomm = 29,
        Inventel = 30,
        AVMBerlin = 31,
        Bandspeed = 32,
        Mansella = 33,
        Nec = 34,
        WavePlusTechnology = 35,
        Alcatel = 36,
        PhilipsSemiconductor = 37,
        CTechnologies = 38,
        OpenInterface = 39,
        RFCMicroDevices = 40,
        Hitachi = 41,
        SymbolTechnologies = 42,
        Tenovis = 43,
        MacronixInternational = 44,
        GCTSemiconductor = 45,
        NorwoodSystems = 46,
        MewTelTechnology = 47,
        STMicroelectronics = 48,
        Synopsys = 49,
        RedMCommunications = 50,
        Commil = 51,
        Catc = 52,
        Eclipse = 53,
        RenesasTechnology = 54,
        Mobilian = 55,
        Terax = 56,
        IntegratedSystemSolution = 57,
        MatsushitaElectricIndustrial = 58,
        Gennum = 59,
        ResearchInMotion = 60,
        IPextreme = 61,
        SystemsAndChips = 62,
        BluetoothSIG = 63,
        SeikoEpson = 64,
        IntegratedSiliconSolution = 65,
        CONWISETechnology = 66,
        ParrotSA = 67,
        SocketCommunications = 68,
        AtherosCommunications = 69,
        MediaTek = 70,
        Bluegiga = 71,
        MarvellTechnologyGroup = 72,
        kBluetoothCompanyIdentifer3DSP = 73,
        AccelSemiconductor = 74,
        ContinentialAutomotiveSystems = 75,
        Apple = 76,
        StaccatoCommunications = 77,
        AvagoTechnologies = 78,
        Apt = 79,
        SiRFTechnology = 80,
        TZeroTechnologies = 81,
        JandM = 82,
        Free2Move = 83,
        kBluetoothCompanyIdentifer3DiJoy = 84,
        Plantronics = 85,
        SonyEricssonMobileCommunications = 86,
        HarmonInternational = 87,
        Visio = 88,
        NordicSemiconductor = 89,
        EMMicroElectronicMarin = 90,
        RalinkTechnology = 91,
        BelkinInternational = 92,
        RealtekSemiconductor = 93,
        StonestreetOne = 94,
        Wicentric = 95,
        RivieraWaves = 96,
        RDAMicroelectronics = 97,
        GibsonGuitars = 98,
        MiCommand = 99,
        BandXIInternational = 100,
        HewlettPackard = 101,
        kBluetoothCompanyIdentifer9SolutionsOy = 102,
        GNNetcom = 103,
        GeneralMotors = 104,
        AAndDEngineering = 105,
        MindTree = 106,
        PolarElectroOY = 107,
        BeautifulEnterprise = 108,
        BriarTek = 109,
        SummitDataCommunications = 110,
        SoundID = 111,
        Monster = 112,
        ConnectBlueAB = 113,
        ShangHaiSuperSmartElectronics = 114,
        GroupSense = 115,
        Zomm = 116,
        SamsungElectronics = 117,
        CreativeTechnology = 118,
        LairdTechnologies = 119,
        Nike = 120,
        LessWire = 121,
        MStarTechnologies = 122,
        HanlynnTechnologies = 123,
        AAndRCambridge = 124,
        SeersTechnology = 125,
        SportsTrackingTechnologies = 126,
        AutonetMobile = 127,
        DeLormePublishingCompany = 128,
        WuXiVimicro = 129,
        SennheiserCommunications = 130,
        TimeKeepingSystems = 131,
        LudusHelsinki = 132,
        BlueRadios = 133,
        Equinux = 134,
        GarminInternational = 135,
        Ecotest = 136,
        GNResound = 137,
        Jawbone = 138,
        TopconPositioningSystems = 139,
        Gimbal = 140,
        ZscanSoftware = 141,
        Quintic = 142,
        TelitWirelessSolutions = 143,
        FunaiElectric = 144,
        AdvancedPANMOBILSystems = 145,
        ThinkOptics = 146,
        UniversalElectriconics = 147,
        AirohaTechnology = 148,
        NECLightning = 149,
        ODMTechnology = 150,
        ConnecteDevice = 151,
        Zero1TV = 152,
        ITechDynamicGlobalDistribution = 153,
        Alpwise = 154,
        JiangsuToppowerAutomotiveElectronics = 155,
        Colorfy = 156,
        Geoforce = 157,
        Bose = 158,
        SuuntoOy = 159,
        KensingtonComputerProductsGroup = 160,
        SRMedizinelektronik = 161,
        Vertu = 162,
        MetaWatch = 163,
        Linak = 164,
        OTLDynamics = 165,
        PandaOcean = 166,
        Visteon = 167,
        ARPDevicesUnlimited = 168,
        MagnetiMarelli = 169,
        CaenRFID = 170,
        IngenieurSystemgruppeZahn = 171,
        GreenThrottleGames = 172,
        PeterSystemtechnik = 173,
        Omegawave = 174,
        Cinetix = 175,
        PassifSemiconductor = 176,
        SarisCyclingGroup = 177,
        Bekey = 178,
        ClarinoxTechnologies = 179,
        BDETechnology = 180,
        SwirlNetworks = 181,
        MesoInternational = 182,
        TreLab = 183,
        QualcommInnovationCenter = 184,
        JohnsonControls = 185,
        StarkeyLaboratories = 186,
        SPowerElectronics = 187,
        AceSensor = 188,
        Aplix = 189,
        AAMPofAmerica = 190,
        StalmartTechnology = 191,
        AMICCOMElectronics = 192,
        ShenzhenExcelsecuDataTechnology = 193,
        Geneq = 194,
        Adidas = 195,
        LGElectronics = 196,
        OnsetComputer = 197,
        SelflyBV = 198,
        Quupa = 199,
        GeLo = 200,
        Evluma = 201,
        Mc10 = 202,
        BinauricSE = 203,
        BeatsElectronics = 204,
        MicrochipTechnology = 205,
        ElgatoSystems = 206,
        Archos = 207,
        Dexcom = 208,
        PolarElectroEurope = 209,
        DialogSemiconductor = 210,
        TaixingbangTechnology = 211,
        Kawantech = 212,
        AustcoCommunicationsSystems = 213,
        TimexGroup = 214,
        QualcommTechnologies = 215,
        QualcommConnectedExperiences = 216,
        VoyetraTurtleBeach = 217,
        txtrGMBH = 218,
        Biosentronics = 219,
        ProctorAndGamble = 220,
        Hosiden = 221,
        Musik = 222,
        MisfitWearables = 223,
        Google = 224,
        Danlers = 225,
        Semilink = 226,
        InMusicBrands = 227,
        LSResearch = 228,
        EdenSoftwareConsultants = 229,
        Freshtemp = 230,
        KSTechnologies = 231,
        ACTSTechnologies = 232,
        VtrackSystems = 233,
        NielsenKellerman = 234,
        ServerTechnology = 235,
        BioResearchAssociates = 236,
        JollyLogic = 237,
        AboveAverageOutcomes = 238,
        Bitsplitters = 239,
        PayPal = 240,
        WitronTechnology = 241,
        MorseProject = 242,
        KentDisplays = 243,
        Nautilus = 244,
        Smartifier = 245,
        Elcometer = 246,
        VSNTechnologies = 247,
        AceUni = 248,
        StickNFind = 249,
        CrystalCode = 250,
        Koukamm = 251,
        Delphi = 252,
        ValenceTech = 253,
        StanleyBlackAndDecker = 254,
        TypeProducts = 255,
        TomTomInternational = 256,
        FuGoo = 257,
        Keiser = 258,
        BangAndOlufson = 259,
        PLUSLocationSystems = 260,
        UbiquitousComputingTechnology = 261,
        InnovativeYachtterSolutions = 262,
        WilliamDemantHolding = 263,
        InteropIdentifier = 65535
    }

    [Native]
    public enum ServiceClassMajor : ulong
    {
        LimitedDiscoverableMode = 1,
        Reserved1 = 2,
        Reserved2 = 4,
        Positioning = 8,
        Networking = 16,
        Rendering = 32,
        Capturing = 64,
        ObjectTransfer = 128,
        Audio = 256,
        Telephony = 512,
        Information = 1024,
        Any = 707406378,
        None = 1852796517,
        End
    }

    [Native]
    public enum DeviceClassMajor : ulong
    {
        Miscellaneous = 0,
        Computer = 1,
        Phone = 2,
        LANAccessPoint = 3,
        Audio = 4,
        Peripheral = 5,
        Imaging = 6,
        Wearable = 7,
        Toy = 8,
        Health = 9,
        Unclassified = 31,
        Any = 707406378,
        None = 1852796517,
    }

    [Native]
    public enum DeviceClassMinor : ulong
    {
        ComputerUnclassified = 0,
        ComputerDesktopWorkstation = 1,
        ComputerServer = 2,
        ComputerLaptop = 3,
        ComputerHandheld = 4,
        ComputerPalmSized = 5,
        ComputerWearable = 6,
        PhoneUnclassified = 0,
        PhoneCellular = 1,
        PhoneCordless = 2,
        PhoneSmartPhone = 3,
        PhoneWiredModemOrVoiceGateway = 4,
        PhoneCommonISDNAccess = 5,
        AudioUnclassified = 0,
        AudioHeadset = 1,
        AudioHandsFree = 2,
        AudioReserved1 = 3,
        AudioMicrophone = 4,
        AudioLoudspeaker = 5,
        AudioHeadphones = 6,
        AudioPortable = 7,
        AudioCar = 8,
        AudioSetTopBox = 9,
        AudioHiFi = 10,
        AudioVCR = 11,
        AudioVideoCamera = 12,
        AudioCamcorder = 13,
        AudioVideoMonitor = 14,
        AudioVideoDisplayAndLoudspeaker = 15,
        AudioVideoConferencing = 16,
        AudioReserved2 = 17,
        AudioGamingToy = 18,
        Peripheral1Keyboard = 16,
        Peripheral1Pointing = 32,
        Peripheral1Combo = 48,
        Peripheral2Unclassified = 0,
        Peripheral2Joystick = 1,
        Peripheral2Gamepad = 2,
        Peripheral2RemoteControl = 3,
        Peripheral2SensingDevice = 4,
        Peripheral2DigitizerTablet = 5,
        Peripheral2CardReader = 6,
        Peripheral2DigitalPen = 7,
        Peripheral2HandheldScanner = 8,
        Peripheral2GesturalInputDevice = 9,
        Peripheral2AnyPointing = 1886349678,
        Imaging1Display = 4,
        Imaging1Camera = 8,
        Imaging1Scanner = 16,
        Imaging1Printer = 32,
        Imaging2Unclassified = 0,
        WearableWristWatch = 1,
        WearablePager = 2,
        WearableJacket = 3,
        WearableHelmet = 4,
        WearableGlasses = 5,
        ToyRobot = 1,
        ToyVehicle = 2,
        ToyDollActionFigure = 3,
        ToyController = 4,
        ToyGame = 5,
        HealthUndefined = 0,
        HealthBloodPressureMonitor = 1,
        HealthThermometer = 2,
        HealthScale = 3,
        HealthGlucoseMeter = 4,
        HealthPulseOximeter = 5,
        HealthHeartRateMonitor = 6,
        HealthDataDisplay = 7,
        Any = 707406378,
        None = 1852796517
    }

    public enum GAPAppearance : uint
    {
        Unknown = 0,
        GenericPhone = 64,
        GenericComputer = 128,
        GenericWatch = 192,
        GenericClock = 256,
        GenericDisplay = 320,
        GenericRemoteControl = 384,
        GenericEyeGlasses = 448,
        GenericTag = 512,
        GenericKeyring = 576,
        GenericMediaPlayer = 640,
        GenericBarcodeScanner = 704,
        GenericThermometer = 768,
        GenericHeartrateSensor = 832,
        GenericBloodPressure = 896,
        GenericHumanInterfaceDevice = 960,
        HumanInterfaceDeviceKeyboard = 961,
        HumanInterfaceDeviceMouse = 962,
        HumanInterfaceDeviceJoystick = 963,
        HumanInterfaceDeviceGamepad = 964,
        HumanInterfaceDeviceDigitizerTablet = 965,
        HumanInterfaceDeviceCardReader = 966,
        HumanInterfaceDeviceDigitalPen = 967,
        HumanInterfaceDeviceBarcodeScanner = 968,
        GenericGlucoseMeter = 1024,
        GenericRunningWalkingSensor = 1088,
        GenericCycling = 1152
    }

    public enum L2CapPsm : uint
    {
        None = 0,
        Sdp = 1,
        Rfcomm = 3,
        TcsBin = 5,
        TcsBinCordless = 7,
        Bnep = 15,
        HIDControl = 17,
        HIDInterrupt = 19,
        Avctp = 23,
        Avdtp = 25,
        AvctpBrowsing = 27,
        UID_C_Plane = 29,
        Att = 31,
        ReservedStart = 1,
        ReservedEnd = 4096,
        DynamicStart = 4097,
        Aacp = 4097,
        D2d = 4111,
        DynamicEnd = 65535,
    }

    public enum BluetoothSdpUuid16 : uint
    {
        Base = 0,
        Sdp = 1,
        Udp = 2,
        Rfcomm = 3,
        Tcp = 4,
        Tcsbin = 5,
        Tcsat = 6,
        Obex = 8,
        Ip = 9,
        Ftp = 10,
        Http = 12,
        Wsp = 14,
        Bnep = 15,
        Upnp = 16,
        Hidp = 17,
        HardcopyControlChannel = 18,
        HardcopyDataChannel = 20,
        HardcopyNotification = 22,
        Avctp = 23,
        Avdtp = 25,
        Cmpt = 27,
        UDI_C_Plane = 29,
        MCAPControlChannel = 30,
        MCAPDataChannel = 31,
        L2Cap = 256
    }

    public enum SdpServiceClasses : uint
    {
        ServiceDiscoveryServer = 4096,
        BrowseGroupDescriptor = 4097,
        PublicBrowseGroup = 4098,
        SerialPort = 4353,
        LANAccessUsingPPP = 4354,
        DialupNetworking = 4355,
        IrMCSync = 4356,
        OBEXObjectPush = 4357,
        OBEXFileTransfer = 4358,
        IrMCSyncCommand = 4359,
        Headset = 4360,
        CordlessTelephony = 4361,
        AudioSource = 4362,
        AudioSink = 4363,
        AVRemoteControlTarget = 4364,
        AdvancedAudioDistribution = 4365,
        AVRemoteControl = 4366,
        AVRemoteControlController = 4367,
        Intercom = 4368,
        Fax = 4369,
        HeadsetAudioGateway = 4370,
        Wap = 4371,
        WAPClient = 4372,
        Panu = 4373,
        Nap = 4374,
        Gn = 4375,
        DirectPrinting = 4376,
        ReferencePrinting = 4377,
        Imaging = 4378,
        ImagingResponder = 4379,
        ImagingAutomaticArchive = 4380,
        ImagingReferencedObjects = 4381,
        HandsFree = 4382,
        HandsFreeAudioGateway = 4383,
        DirectPrintingReferenceObjectsService = 4384,
        ReflectedUI = 4385,
        BasicPrinting = 4386,
        PrintingStatus = 4387,
        HumanInterfaceDeviceService = 4388,
        HardcopyCableReplacement = 4389,
        HCR_Print = 4390,
        HCR_Scan = 4391,
        CommonISDNAccess = 4392,
        VideoConferencingGW = 4393,
        UdiMt = 4394,
        UdiTa = 4395,
        AudioVideo = 4396,
        SIM_Access = 4397,
        PhonebookAccess_PCE = 4398,
        PhonebookAccess_PSE = 4399,
        PhonebookAccess = 4400,
        Headset_HS = 4401,
        MessageAccessServer = 4402,
        MessageNotificationServer = 4403,
        MessageAccessProfile = 4404,
        GlobalNavigationSatelliteSystem = 4405,
        GlobalNavigationSatelliteSystemServer = 4406,
        PnPInformation = 4608,
        GenericNetworking = 4609,
        GenericFileTransfer = 4610,
        GenericAudio = 4611,
        GenericTelephony = 4612,
        VideoSource = 4867,
        VideoSink = 4868,
        VideoDistribution = 4869,
        HealthDevice = 5120,
        HealthDeviceSource = 5121,
        HealthDeviceSink = 5122
    }

    public enum SdpAttributeIdentifierCodes : uint
    {
        ServiceRecordHandle = 0,
        ServiceClassIDList = 1,
        ServiceRecordState = 2,
        ServiceID = 3,
        ProtocolDescriptorList = 4,
        BrowseGroupList = 5,
        LanguageBaseAttributeIDList = 6,
        ServiceInfoTimeToLive = 7,
        ServiceAvailability = 8,
        BluetoothProfileDescriptorList = 9,
        DocumentationURL = 10,
        ClientExecutableURL = 11,
        IconURL = 12,
        AdditionalProtocolsDescriptorList = 13,
        VersionNumberList = 512,
        ServiceDatabaseState = 513,
        GroupID = 512,
        IPSubnet = 512,
        HIDReleaseNumber = 512,
        HIDParserVersion = 513,
        HIDDeviceSubclass = 514,
        HIDCountryCode = 515,
        HIDVirtualCable = 516,
        HIDReconnectInitiate = 517,
        HIDDescriptorList = 518,
        HIDLangIDBaseList = 519,
        HIDSDPDisable = 520,
        HIDBatteryPower = 521,
        HIDRemoteWake = 522,
        HIDProfileVersion = 523,
        HIDSupervisionTimeout = 524,
        HIDNormallyConnectable = 525,
        HIDBootDevice = 526,
        HIDSSRHostMaxLatency = 527,
        HIDSSRHostMinTimeout = 528,
        ServiceVersion = 768,
        ExternalNetwork = 769,
        Network = 769,
        SupportedDataStoresList = 769,
        FaxClass1Support = 770,
        RemoteAudioVolumeControl = 770,
        FaxClass2_0Support = 771,
        SupporterFormatsList = 771,
        FaxClass2Support = 772,
        AudioFeedbackSupport = 773,
        NetworkAddress = 774,
        WAPGateway = 775,
        HomepageURL = 776,
        WAPStackType = 777,
        SecurityDescription = 778,
        NetAccessType = 779,
        MaxNetAccessRate = 780,
        SupportedCapabilities = 784,
        SupportedFeatures = 785,
        SupportedFunctions = 786,
        TotalImagingDataCapacity = 787,
        ServiceName = 0,
        ServiceDescription = 1,
        ProviderName = 2
    }

    public enum SdpAttributeDeviceIdentificationRecord : uint
    {
        ServiceDescription = 1,
        DocumentationURL = 10,
        ClientExecutableURL = 11,
        SpecificationID = 512,
        VendorID = 513,
        ProductID = 514,
        Version = 515,
        PrimaryRecord = 516,
        VendorIDSource = 517,
        ReservedRangeStart = 518,
        ReservedRangeEnd = 767
    }

    public enum ProtocolParameters : uint
    {
        L2CapPsm = 1,
        RFCommChannel = 1,
        TcpPort = 1,
        UdpPort = 1,
        BNEPVersion = 1,
        BNEPSupportedNetworkPacketTypeList = 2
    }

    public enum BluetoothHCIExtendedInquiryResponseDataTypes : uint
    {
        Flags = 1,
        kBluetoothHCIExtendedInquiryResponseDataType16BitServiceClassUUIDsWithMoreAvailable = 2,
        kBluetoothHCIExtendedInquiryResponseDataType16BitServiceClassUUIDsCompleteList = 3,
        kBluetoothHCIExtendedInquiryResponseDataType32BitServiceClassUUIDsWithMoreAvailable = 4,
        kBluetoothHCIExtendedInquiryResponseDataType32BitServiceClassUUIDsCompleteList = 5,
        kBluetoothHCIExtendedInquiryResponseDataType128BitServiceClassUUIDsWithMoreAvailable = 6,
        kBluetoothHCIExtendedInquiryResponseDataType128BitServiceClassUUIDsCompleteList = 7,
        ShortenedLocalName = 8,
        CompleteLocalName = 9,
        TransmitPowerLevel = 10,
        SSPOOBClassOfDevice = 13,
        SSPOOBSimplePairingHashC = 14,
        SSPOOBSimplePairingRandomizerR = 15,
        DeviceID = 16,
        SecurityManagerTKValue = 16,
        SecurityManagerOOBFlags = 17,
        SlaveConnectionIntervalRange = 18,
        ServiceSolicitation16BitUUIDs = 20,
        ServiceSolicitation128BitUUIDs = 21,
        ServiceData = 22,
        PublicTargetAddress = 23,
        RandomTargetAddress = 24,
        Appearance = 25,
        AdvertisingInterval = 26,
        LEBluetoothDeviceAddress = 27,
        LERole = 28,
        SimplePairingHash = 29,
        SimplePairingRandomizer = 30,
        ServiceSolicitation32BitUUIDs = 31,
        ServiceData32BitUUID = 32,
        ServiceData128BitUUID = 33,
        SecureConnectionsConfirmationValue = 34,
        SecureConnectionsRandomValue = 35,
        Uri = 36,
        IndoorPositioning = 37,
        TransportDiscoveryData = 38,
        kBluetoothHCIExtendedInquiryResponseDataType3DInformationData = 61,
        ManufacturerSpecificData = 255
    }

    public enum BluetoothHciVersion : uint
    {
        CoreSpecification1_0b = 0,
        CoreSpecification1_1 = 1,
        CoreSpecification1_2 = 2,
        CoreSpecification2_0EDR = 3,
        CoreSpecification2_1EDR = 4,
        CoreSpecification3_0HS = 5,
        CoreSpecification4_0 = 6,
        CoreSpecification4_1 = 7,
        CoreSpecification4_2 = 8,
        CoreSpecification5_0 = 9
    }

    public enum BluetoothLmpVersion : uint
    {
        CoreSpecification1_0b = 0,
        CoreSpecification1_1 = 1,
        CoreSpecification1_2 = 2,
        CoreSpecification2_0EDR = 3,
        CoreSpecification2_1EDR = 4,
        CoreSpecification3_0HS = 5,
        CoreSpecification4_0 = 6,
        CoreSpecification4_1 = 7,
        CoreSpecification4_2 = 8,
        CoreSpecification5_0 = 9
    }

    //TODO: this should be a const
    public enum BluetoothConnectionHandle : uint
    {
        None = 65535
    }

    public enum BluetoothEncryptionEnable : uint
    {
        Off = 0,
        On = 1,
        Bredre0 = 1,
        Leaesccm = 1,
        Bredraesccm = 2
    }

    public enum BluetoothKeyFlag : uint
    {
        SemiPermanent = 0,
        Temporary = 1
    }

    public enum BluetoothKeyType : uint
    {
        Combination = 0,
        LocalUnit = 1,
        RemoteUnit = 2,
        DebugCombination = 3,
        UnauthenticatedCombination = 4,
        AuthenticatedCombination = 5,
        ChangedCombination = 6
    }

    [Flags]
    public enum BluetoothPacketType : uint
    {
        Reserved1 = 1,
        BluetoothPacketType2DH1Omit = 2,
        BluetoothPacketType3DH1Omit = 4,
        Dm1 = 8,
        Dh1 = 16,
        Hv1 = 32,
        Hv2 = 64,
        Hv3 = 128,
        Dv = 256,
        BluetoothPacketType2DH3Omit = 256,
        BluetoothPacketType3DH3Omit = 512,
        Aux = 512,
        Dm3 = 1024,
        Dh3 = 2048,
        BluetoothPacketType2DH5Omit = 4096,
        BluetoothPacketType3DM5Omit = 8192,
        Dm5 = 16384,
        Dh5 = 32768
    }

    [Flags]
    public enum BluetoothSynchronousConnectionPacketType : uint
    {
        None = 0,
        Hv1 = 1,
        Hv2 = 2,
        Hv3 = 4,
        Ev3 = 8,
        Ev4 = 16,
        Ev5 = 32,
        BluetoothSynchronousConnectionPacketType2EV3Omit = 64,
        BluetoothSynchronousConnectionPacketType3EV3Omit = 128,
        BluetoothSynchronousConnectionPacketType2EV5Omit = 256,
        BluetoothSynchronousConnectionPacketType3EV5Omit = 512,
        FutureUse = 64512,
        All = 65535
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum kBluetooth : uint
    //{
    //	GeneralInquiryAccessCodeIndex = 0,
    //	GeneralInquiryAccessCodeLAPValue = 10390323,
    //	LimitedInquiryAccessCodeIndex = 1,
    //	LimitedInquiryAccessCodeLAPValue = 10390272,
    //	LimitedInquiryAccessCodeEnd
    //}

    public enum BluetoothPageScanRepetitionMode : uint
    {
        R0 = 0,
        R1 = 1,
        R2 = 2
    }

    public enum BluetoothPageScanPeriodMode : uint
    {
        P0 = 0,
        P1 = 1,
        P2 = 2
    }

    public enum BluetoothPageScanMode : uint
    {
        Mandatory = 0,
        Optional1 = 1,
        Optional2 = 2,
        Optional3 = 3
    }

    public enum BluetoothHciPageScanTypes : uint
    {
        Standard = 0,
        Interlaced = 1,
        ReservedStart = 2,
        ReservedEnd = 255
    }

    public enum BluetoothHciErroneousDataReporting : uint
    {
        Disabled = 0,
        Enabled = 1,
        ReservedStart = 2,
        ReservedEnd = 255
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothDeviceAddress
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothKey
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothIRK
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothPINCode
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;
    }

    public static class BluetoothConstants
    {
        public const uint BluetoothDeviceNameMaxLength = 248;
        public const uint BluetoothL2CAPMaxPacketSize = 65535;
        public const uint BluetoothL2CAPInfoTypeMaxConnectionlessMTUSize = 1;
        public const uint BluetoothL2CAPPacketHeaderSize = 4;
    }


    public enum BluetoothRoleSwitch : uint
    {
        DontAllowRoleSwitch = 0,
        AllowRoleSwitch = 1
    }

    public enum BluetoothRole : uint
    {
        BecomeMaster = 0,
        RemainSlave = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothSetEventMask
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data;
    }

    public enum BluetoothACLLogicalChannel : uint
    {

        Reserved = 0,
        L2CAPContinue = 1,
        L2CAPStart = 2,
        LMP = 3
    }

    public enum BluetoothL2CAPChannel : uint
    {
        Null = 0,
        Signalling = 1,
        ConnectionLessData = 2,
        AMPManagerProtocol = 3,
        AttributeProtocol = 4,
        LESignalling = 5,
        SecurityManager = 6,
        ReservedStart = 7,
        Leap = 42,
        Leas = 43,
        MagicPairing = 48,
        Magnet = 58,
        ReservedEnd = 63,
        DynamicStart = 64,
        DynamicEnd = 65535
    }

    public enum BluetoothL2CAPCommandCode : uint
    {
        Reserved = 0,
        CommandReject = 1,
        ConnectionRequest = 2,
        ConnectionResponse = 3,
        ConfigureRequest = 4,
        ConfigureResponse = 5,
        DisconnectionRequest = 6,
        DisconnectionResponse = 7,
        EchoRequest = 8,
        EchoResponse = 9,
        InformationRequest = 10,
        InformationResponse = 11,
        CreateChannelRequest = 12,
        CreateChannelResponse = 13,
        MoveChannelRequest = 14,
        MoveChannelResponse = 15,
        MoveChannelConfirmation = 16,
        MoveChannelConfirmationResponse = 17,
        ConnectionParameterUpdateRequest = 18,
        ConnectionParameterUpdateResponse = 19,
        LECreditBasedConnectionRequest = 20,
        LECreditBasedConnectionResponse = 21,
        LEFlowControlCredit = 22
    }

    public enum BluetoothL2CAPCommandRejectReason : uint
    {
        CommandNotUnderstood = 0,
        SignallingMTUExceeded = 1,
        InvalidCIDInRequest = 2
    }

    public enum BluetoothL2CAPFlushTimeout : uint
    {
        UseExisting = 0,
        Immediate = 1,
        Forever = 65535
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothL2CAPQualityOfServiceOptions
    {
        public byte flags;

        public byte serviceType;

        public uint tokenRate;

        public uint tokenBucketSize;

        public uint peakBandwidth;

        public uint latency;

        public uint delayVariation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothL2CAPRetransmissionAndFlowControlOptions
    {
        public byte flags;

        public byte txWindowSize;

        public byte maxTransmit;

        public ushort retransmissionTimeout;

        public ushort monitorTimeout;

        public ushort maxPDUPayloadSize;
    }

    public enum BluetoothL2CAPSegmentationAndReassembly : uint
    {
        UnsegmentedSDU = 0,
        StartOfSDU = 1,
        EndOfSDU = 2,
        ContinuationOfSDU = 3
    }

    public enum BluetoothL2CAPConnectionResult : uint
    {
        Successful = 0,
        Pending = 1,
        RefusedPSMNotSupported = 2,
        RefusedSecurityBlock = 3,
        RefusedNoResources = 4,
        RefusedReserved = 5,
        RefusedInvalidSourceCID = 6,
        RefusedSourceCIDAlreadyAllocated = 7
    }

    public enum BluetoothL2CAPConnectionStatus : uint
    {
        NoInfoAvailable = 0,
        AuthenticationPending = 1,
        AuthorizationPending = 2
    }

    public enum BluetoothL2CAPConfigurationResult : uint
    {
        Success = 0,
        UnacceptableParams = 1,
        Rejected = 2,
        UnknownOptions = 3
    }

    public enum BluetoothL2CAPConfigurationOption : uint
    {
        Mtu = 1,
        FlushTimeout = 2,
        QoS = 3,
        RetransmissionAndFlowControl = 4,
        FrameCheckSequence = 5,
        ExtendedFlowSpecification = 6,
        ExtendedWindowSize = 7
    }

    public enum BluetoothL2CAPConfigurationOptionLength : uint
    {
        MTULength = 2,
        FlushTimeoutLength = 2,
        QoSLength = 22,
        RetransmissionAndFlowControlLength = 9
    }

    public enum BluetoothL2CAPConfigurationRetransmissionAndFlowControlFlags : uint
    {
        BasicL2CAPModeFlag = 0,
        RetransmissionModeFlag = 1,
        FlowControlModeFlag = 2,
        EnhancedRetransmissionMode = 3,
        StreamingMode = 4
    }

    public enum BluetoothL2CAPInformationType : uint
    {
        ConnectionlessMTU = 1,
        ExtendedFeatures = 2,
        FixedChannelsSupported = 3
    }

    public enum BluetoothL2CAPInformationResult : uint
    {
        Success = 0,
        NotSupported = 1
    }

    public enum BluetoothL2CAPInformationExtendedFeaturesMask : uint
    {
        InformationNoExtendedFeatures = 0,
        InformationFlowControlMode = 1,
        InformationRetransmissionMode = 2,
        InformationBidirectionalQoS = 4,
        InformationEnhancedRetransmissionMode = 8,
        InformationStreamingMode = 16,
        InformationFCSOption = 32,
        InformationExtendedFlowSpecification = 64,
        InformationFixedChannels = 128,
        InformationExtendedWindowSize = 256,
        UnicastConnectionlessDataReception = 512
    }

    public enum BluetoothL2CAPQoSType : uint
    {
        NoTraffic = 0,
        BestEffort = 1,
        Guaranteed = 2
    }

    public enum BluetoothL2CAPSupervisoryFuctionType : uint
    {
        ReceiverReady = 0,
        Reject = 1,
        ReceiverNotReady = 2,
        SelectiveReject = 3
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum BluetoothLETX : uint
    //{
    //	TimeMin = 328,
    //	TimeDefault = 328,
    //	TimeMax = 2120,
    //	OctetsMin = 27,
    //	OctetsDefault = 27,
    //	OctetsMax = 251
    //}

    //[Verify (InferredFromMemberPrefix)]
    //public enum BluetoothL2CAP : uint
    //{
    //	MTULowEnergyDefault = kBluetoothLETXOctetsMin,
    //	MTULowEnergyMax = kBluetoothLETXOctetsMax,
    //	MTUMinimum = 48,
    //	MTUDefault = 1017,
    //	MTUMaximum = 65535,
    //	MTUStart = 32767,
    //	Mtusig = 48,
    //	FlushTimeoutDefault = kBluetoothL2CAPFlushTimeoutForever,
    //	QoSFlagsDefault = 0,
    //	QoSTypeDefault = kBluetoothL2CAPQoSTypeBestEffort,
    //	QoSTokenRateDefault = 0,
    //	QoSTokenBucketSizeDefault = 0,
    //	QoSPeakBandwidthDefault = 0,
    //	QoSLatencyDefault = 4294967295u,
    //	QoSDelayVariationDefault = 4294967295u
    //}

    //public enum BluetoothLEMaxTX : uint
    //{
    //	TimeMin = 328,
    //	TimeDefault = kBluetoothL2CAPMTULowEnergyDefault,
    //	TimeMax = 2120,
    //	OctetsMin = 27,
    //	OctetsDefault = 128,
    //	OctetsMax = 251
    //}

    //public enum BluetoothLESecurityManagerKeyDistributionFormat : uint
    //{
    //	EncryptionKey = (1 << 0),
    //	IDKey = (1 << 1),
    //	SignKey = (1 << 2),
    //	LinkKey = (1 << 3)
    //}

    //public enum BluetoothLESecurityManagerCommandCode : uint
    //{
    //	Reserved = 0,
    //	PairingRequest = 1,
    //	PairingResponse = 2,
    //	PairingConfirm = 3,
    //	PairingRandom = 4,
    //	PairingFailed = 5,
    //	EncryptionInfo = 6,
    //	MasterIdentification = 7,
    //	IdentityInfo = 8,
    //	IdentityAddressInfo = 9,
    //	SigningInfo = 10,
    //	SecurityRequest = 11,
    //	PairingPublicKey = 12,
    //	PairingDHKeyCheck = 13,
    //	PairingKeypressNotification = 14,
    //	ReservedStart = 15,
    //	ReservedEnd = 255
    //}

    //public enum BluetoothLESecurityManagerUserInputCapability : uint
    //{
    //	NoInput = 1,
    //	YesNo = 2,
    //	Keyboard = 3
    //}

    //public enum BluetoothLESecurityManagerUserOutputCapability : uint
    //{
    //	oOutput = 1,
    //	umericOutput = 2
    //}

    //public enum BluetoothLESecurityManagerIOCapability : uint
    //{
    //	DisplayOnly = 0,
    //	DisplayYesNo = 1,
    //	KeyboardOnly = 2,
    //	NoInputNoOutput = 3,
    //	KeyboardDisplay = 4,
    //	ReservedStart = 5,
    //	ReservedEnd = 255
    //}

    //public enum BluetoothLESecurityManagerOOBData : uint
    //{
    //	AuthenticationDataNotPresent = 0,
    //	AuthenticationDataPresent = 1,
    //	DataReservedStart = 2,
    //	DataReservedEnd = 255
    //}

    //public enum BluetoothLESecurityManager : uint
    //{
    //	NoBonding = 0,
    //	Bonding = 1,
    //	ReservedStart = 2,
    //	ReservedEnd = 3
    //}

    //public enum BluetoothLESecurityManagerPairingFailedReasonCode : uint
    //{
    //	Reserved = 0,
    //	PasskeyEntryFailed = 1,
    //	OOBNotAvailbale = 2,
    //	AuthenticationRequirements = 3,
    //	ConfirmValueFailed = 4,
    //	PairingNotSupported = 5,
    //	EncryptionKeySize = 6,
    //	CommandNotSupported = 7,
    //	UnspecifiedReason = 8,
    //	RepeatedAttempts = 9,
    //	InvalidParameters = 10,
    //	DHKeyCheckFailed = 11,
    //	NumericComparisonFailed = 12,
    //	BREDRPairingInProgress = 13,
    //	CrossTransportKeyDerivationGenerationNotAllowed = 14,
    //	ReservedStart = 15,
    //	ReservedEnd = 255
    //}

    //public enum BluetoothLESecurityManagerKeypressNotificationType : uint
    //{
    //	PasskeyEntryStarted = 0,
    //	PasskeyDigitEntered = 1,
    //	PasskeyDigitErased = 2,
    //	PasskeyCleared = 3,
    //	PasskeyEntryCompleted = 4,
    //	ReservedStart = 5,
    //	ReservedEnd = 255
    //}

    public enum BluetoothAMPManagerCode : uint
    {
        Reserved = 0,
        AMPCommandReject = 1,
        AMPDiscoverRequest = 2,
        AMPDiscoverResponse = 3,
        AMPChangeNotify = 4,
        AMPChangeResponse = 5,
        AMPGetInfoRequest = 6,
        AMPGetInfoResponse = 7,
        AMPGetAssocRequest = 8,
        AMPGetAssocResponse = 9,
        AMPCreatePhysicalLinkRequest = 10,
        AMPCreatePhysicalLinkResponse = 11,
        AMPDisconnectPhysicalLinkRequest = 12,
        AMPDisconnectPhysicalLinkResponse = 13
    }

    public enum BluetoothAMPCommandRejectReason : uint
    {
        BluetoothAMPManagerCommandRejectReasonCommandNotRecognized = 0
    }

    public enum BluetoothAMPDiscoverResponseControllerStatus : uint
    {
        PoweredDown = 0,
        BluetoothOnly = 1,
        NoCapacity = 2,
        LowCapacity = 3,
        MediumCapacity = 4,
        HighCapacity = 5,
        FullCapacity = 6
    }

    public enum BluetoothAMPGetInfoResponseStatus : uint
    {
        Success = 0,
        InvalidControllerID = 1
    }

    public enum BluetoothAMPGetAssocResponseStatus : uint
    {
        Success = 0,
        InvalidControllerID = 1
    }

    public enum BluetoothAMPCreatePhysicalLinkResponseStatus : uint
    {
        Success = 0,
        InvalidControllerID = 1,
        UnableToStartLinkCreation = 2,
        CollisionOccurred = 3,
        AMPDisconnectedPhysicalLinkRequestReceived = 4,
        PhysicalLinkAlreadyExists = 5,
        SecurityViolation = 6
    }

    public enum BluetoothAMPDisconnectPhysicalLinkResponseStatus : uint
    {
        Success = 0,
        InvalidControllerID = 1,
        NoPhysicalLink = 2
    }

    public enum BluetoothHCI : uint
    {
        OpCodeNoOp = 0,
        CommandGroupNoOp = 0,
        CommandNoOp = 0,
        CommandGroupLinkControl = 1,
        CommandInquiry = 1,
        CommandInquiryCancel = 2,
        CommandPeriodicInquiryMode = 3,
        CommandExitPeriodicInquiryMode = 4,
        CommandCreateConnection = 5,
        CommandDisconnect = 6,
        CommandAddSCOConnection = 7,
        CommandCreateConnectionCancel = 8,
        CommandAcceptConnectionRequest = 9,
        CommandRejectConnectionRequest = 10,
        CommandLinkKeyRequestReply = 11,
        CommandLinkKeyRequestNegativeReply = 12,
        CommandPINCodeRequestReply = 13,
        CommandPINCodeRequestNegativeReply = 14,
        CommandChangeConnectionPacketType = 15,
        CommandAuthenticationRequested = 17,
        CommandSetConnectionEncryption = 19,
        CommandChangeConnectionLinkKey = 21,
        CommandMasterLinkKey = 23,
        CommandRemoteNameRequest = 25,
        CommandReadRemoteSupportedFeatures = 27,
        CommandReadRemoteExtendedFeatures = 28,
        CommandReadRemoteVersionInformation = 29,
        CommandReadClockOffset = 31,
        CommandRemoteNameRequestCancel = 26,
        CommandReadLMPHandle = 32,
        CommandSetupSynchronousConnection = 40,
        CommandAcceptSynchronousConnectionRequest = 41,
        CommandRejectSynchronousConnectionRequest = 42,
        CommandIOCapabilityRequestReply = 43,
        CommandUserConfirmationRequestReply = 44,
        CommandUserConfirmationRequestNegativeReply = 45,
        CommandUserPasskeyRequestReply = 46,
        CommandUserPasskeyRequestNegativeReply = 47,
        CommandRemoteOOBDataRequestReply = 48,
        CommandRemoteOOBDataRequestNegativeReply = 51,
        CommandIOCapabilityRequestNegativeReply = 52,
        CommandEnhancedSetupSynchronousConnection = 61,
        CommandEnhancedAcceptSynchronousConnectionRequest = 62,
        CommandTruncatedPage = 63,
        CommandTruncatedPageCancel = 64,
        CommandSetConnectionlessSlaveBroadcast = 65,
        CommandSetConnectionlessSlaveBroadcastReceive = 66,
        CommandStartSynchronizationTrain = 67,
        CommandReceiveSynchronizationTrain = 68,
        CommandRemoteOOBExtendedDataRequestReply = 69,
        CommandGroupLinkPolicy = 2,
        CommandHoldMode = 1,
        CommandSniffMode = 3,
        CommandExitSniffMode = 4,
        CommandParkMode = 5,
        CommandExitParkMode = 6,
        CommandQoSSetup = 7,
        CommandRoleDiscovery = 9,
        CommandSwitchRole = 11,
        CommandReadLinkPolicySettings = 12,
        CommandWriteLinkPolicySettings = 13,
        CommandReadDefaultLinkPolicySettings = 14,
        CommandWriteDefaultLinkPolicySettings = 15,
        CommandFlowSpecification = 16,
        CommandSniffSubrating = 17,
        CommandAcceptSniffRequest = 49,
        CommandRejectSniffRequest = 50,
        CommandGroupHostController = 3,
        CommandSetEventMask = 1,
        CommandReset = 3,
        CommandSetEventFilter = 5,
        CommandFlush = 8,
        CommandReadPINType = 9,
        CommandWritePINType = 10,
        CommandCreateNewUnitKey = 11,
        CommandReadStoredLinkKey = 13,
        CommandWriteStoredLinkKey = 17,
        CommandDeleteStoredLinkKey = 18,
        CommandChangeLocalName = 19,
        CommandReadLocalName = 20,
        CommandReadConnectionAcceptTimeout = 21,
        CommandWriteConnectionAcceptTimeout = 22,
        CommandReadPageTimeout = 23,
        CommandWritePageTimeout = 24,
        CommandReadScanEnable = 25,
        CommandWriteScanEnable = 26,
        CommandReadPageScanActivity = 27,
        CommandWritePageScanActivity = 28,
        CommandReadInquiryScanActivity = 29,
        CommandWriteInquiryScanActivity = 30,
        CommandReadAuthenticationEnable = 31,
        CommandWriteAuthenticationEnable = 32,
        CommandReadEncryptionMode = 33,
        CommandWriteEncryptionMode = 34,
        CommandReadClassOfDevice = 35,
        CommandWriteClassOfDevice = 36,
        CommandReadVoiceSetting = 37,
        CommandWriteVoiceSetting = 38,
        CommandReadAutomaticFlushTimeout = 39,
        CommandWriteAutomaticFlushTimeout = 40,
        CommandReadNumberOfBroadcastRetransmissions = 41,
        CommandWriteNumberOfBroadcastRetransmissions = 42,
        CommandReadHoldModeActivity = 43,
        CommandWriteHoldModeActivity = 44,
        CommandReadTransmitPowerLevel = 45,
        CommandReadSCOFlowControlEnable = 46,
        CommandWriteSCOFlowControlEnable = 47,
        CommandSetHostControllerToHostFlowControl = 49,
        CommandHostBufferSize = 51,
        CommandHostNumberOfCompletedPackets = 53,
        CommandReadLinkSupervisionTimeout = 54,
        CommandWriteLinkSupervisionTimeout = 55,
        CommandReadNumberOfSupportedIAC = 56,
        CommandReadCurrentIACLAP = 57,
        CommandWriteCurrentIACLAP = 58,
        CommandReadPageScanPeriodMode = 59,
        CommandWritePageScanPeriodMode = 60,
        CommandReadPageScanMode = 61,
        CommandWritePageScanMode = 62,
        CommandSetAFHClassification = 63,
        CommandReadInquiryScanType = 66,
        CommandWriteInquiryScanType = 67,
        CommandReadInquiryMode = 68,
        CommandWriteInquiryMode = 69,
        CommandReadPageScanType = 70,
        CommandWritePageScanType = 71,
        CommandReadAFHChannelAssessmentMode = 72,
        CommandWriteAFHChannelAssessmentMode = 73,
        CommandReadExtendedInquiryResponse = 81,
        CommandWriteExtendedInquiryResponse = 82,
        CommandRefreshEncryptionKey = 83,
        CommandReadSimplePairingMode = 85,
        CommandWriteSimplePairingMode = 86,
        CommandReadLocalOOBData = 87,
        CommandReadInquiryResponseTransmitPower = 88,
        CommandWriteInquiryResponseTransmitPower = 89,
        CommandSendKeypressNotification = 96,
        CommandReadDefaultErroneousDataReporting = 90,
        CommandWriteDefaultErroneousDataReporting = 91,
        CommandEnhancedFlush = 95,
        CommandReadLogicalLinkAcceptTimeout = 97,
        CommandWriteLogicalLinkAcceptTimeout = 98,
        CommandSetEventMaskPageTwo = 99,
        CommandReadLocationData = 100,
        CommandWriteLocationData = 101,
        CommandReadFlowControlMode = 102,
        CommandWriteFlowControlMode = 103,
        CommandReadEnhancedTransmitPowerLevel = 104,
        CommandReadBestEffortFlushTimeout = 105,
        CommandWriteBestEffortFlushTimeout = 106,
        CommandShortRangeMode = 107,
        CommandReadLEHostSupported = 108,
        CommandWriteLEHostSupported = 109,
        CommandSetMWSChannelParameters = 110,
        CommandSetExternalFrameConfiguration = 111,
        CommandSetMWSSignaling = 112,
        CommandSetMWSTransportLayer = 113,
        CommandSetMWSScanFrequencyTable = 114,
        CommandSetMWSPATTERNConfiguration = 115,
        CommandSetReservedLTADDR = 116,
        CommandDeleteReservedLTADDR = 117,
        CommandSetConnectionlessSlaveBroadcastData = 118,
        CommandReadSynchronizationTrainParameters = 119,
        CommandWriteSynchronizationTrainParameters = 120,
        CommandReadSecureConnectionsHostSupport = 121,
        CommandWriteSecureConnectionsHostSupport = 122,
        CommandReadAuthenticatedPayloadTimeout = 123,
        CommandWriteAuthenticatedPayloadTimeout = 124,
        CommandReadLocalOOBExtendedData = 125,
        CommandReadExtendedPageTimeout = 126,
        CommandWriteExtendedPageTimeout = 127,
        CommandReadExtendedInquiryLength = 128,
        CommandWriteExtendedInquiryLength = 129,
        CommandGroupInformational = 4,
        CommandReadLocalVersionInformation = 1,
        CommandReadLocalSupportedCommands = 2,
        CommandReadLocalSupportedFeatures = 3,
        CommandReadLocalExtendedFeatures = 4,
        CommandReadBufferSize = 5,
        CommandReadCountryCode = 7,
        CommandReadDeviceAddress = 9,
        CommandReadDataBlockSize = 10,
        CommandReadLocalSupportedCodecs = 11,
        CommandGroupStatus = 5,
        CommandReadFailedContactCounter = 1,
        CommandResetFailedContactCounter = 2,
        CommandGetLinkQuality = 3,
        CommandReadRSSI = 5,
        CommandReadAFHMappings = 6,
        CommandReadClock = 7,
        CommandReadEncryptionKeySize = 8,
        CommandReadLocalAMPInfo = 9,
        CommandReadLocalAMPASSOC = 10,
        CommandWriteRemoteAMPASSOC = 11,
        CommandGetMWSTransportLayerConfiguration = 12,
        CommandSetTriggeredClockCapture = 13,
        CommandGroupTesting = 6,
        CommandReadLoopbackMode = 1,
        CommandWriteLoopbackMode = 2,
        CommandEnableDeviceUnderTestMode = 3,
        CommandWriteSimplePairingDebugMode = 4,
        CommandEnableAMPReceiverReports = 7,
        CommandAMPTestEnd = 8,
        CommandAMPTest = 9,
        CommandGroupLowEnergy = 8,
        CommandLESetEventMask = 1,
        CommandLEReadBufferSize = 2,
        CommandLEReadLocalSupportedFeatures = 3,
        CommandLESetRandomAddress = 5,
        CommandLESetAdvertisingParameters = 6,
        CommandLEReadAdvertisingChannelTxPower = 7,
        CommandLESetAdvertisingData = 8,
        CommandLESetScanResponseData = 9,
        CommandLESetAdvertiseEnable = 10,
        CommandLESetScanParameters = 11,
        CommandLESetScanEnable = 12,
        CommandLECreateConnection = 13,
        CommandLECreateConnectionCancel = 14,
        CommandLEReadWhiteListSize = 15,
        CommandLEClearWhiteList = 16,
        CommandLEAddDeviceToWhiteList = 17,
        CommandLERemoveDeviceFromWhiteList = 18,
        CommandLEConnectionUpdate = 19,
        CommandLESetHostChannelClassification = 20,
        CommandLEReadChannelMap = 21,
        CommandLEReadRemoteUsedFeatures = 22,
        CommandLEEncrypt = 23,
        CommandLERand = 24,
        CommandLEStartEncryption = 25,
        CommandLELongTermKeyRequestReply = 26,
        CommandLELongTermKeyRequestNegativeReply = 27,
        CommandLEReadSupportedStates = 28,
        CommandLEReceiverTest = 29,
        CommandLETransmitterTest = 30,
        CommandLETestEnd = 31,
        CommandLERemoteConnectionParameterRequestReply = 32,
        CommandLERemoteConnectionParameterRequestNegativeReply = 33,
        CommandLESetDataLength = 34,
        CommandLEReadSuggestedDefaultDataLength = 35,
        CommandLEWriteSuggestedDefaultDataLength = 36,
        CommandLEReadLocalP256PublicKey = 37,
        CommandLEGenerateDHKey = 38,
        CommandLEAddDeviceToResolvingList = 39,
        CommandLERemoveDeviceFromResolvingList = 40,
        CommandLEClearResolvingList = 41,
        CommandLEReadResolvingListSize = 42,
        CommandLEReadPeerResolvableAddress = 43,
        CommandLEReadLocalResolvableAddress = 44,
        CommandLESetAddressResolutionEnable = 45,
        CommandLESetResolvablePrivateAddressTimeout = 46,
        CommandLEReadMaximumDataLength = 47,
        CommandLEReadPhy = 48,
        CommandLESetDefaultPhy = 49,
        CommandLESetPhy = 50,
        CommandLEEnhancedReceiverTest = 51,
        CommandLEEnhancedTransmitterTest = 52,
        CommandGroupLogoTesting = 62,
        CommandGroupVendorSpecific = 63,
        CommandGroupMax = 64,
        CommandMax = 1023
    }

    public enum BluetoothHCIConnectionModes : uint
    {
        ActiveMode = 0,
        HoldMode = 1,
        SniffMode = 2,
        ParkMode = 3,
        ModeReservedForFutureUse = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCISupportedCommands
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCILEUsedFeatures
    {
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIExtendedFeaturesInfo
    {
        public byte page;

        public byte maxPage;

        public byte[] data;
    }

    public enum BluetoothLEFeatureBits : uint
    {
        LEEncryption = (1 << 0),
        ConnectionParamsRequestProcedure = (1 << 1),
        ExtendedRejectIndication = (1 << 2),
        SlaveInitiatedFeaturesExchange = (1 << 3),
        LEPing = (1 << 4),
        LEDataPacketLengthExtension = (1 << 5),
        LLPrivacy = (1 << 6),
        ExtendedScannerFilterPolicies = (1 << 7)
    }

    public enum BluetoothFeatureBits : uint
    {
        ThreeSlotPackets = (1 << 0),
        FiveSlotPackets = (1 << 1),
        Encryption = (1 << 2),
        SlotOffset = (1 << 3),
        TimingAccuracy = (1 << 4),
        SwitchRoles = (1 << 5),
        HoldMode = (1 << 6),
        SniffMode = (1 << 7),
        ParkMode = (1 << 0),
        Rssi = (1 << 1),
        PowerControlRequests = (1 << 1),
        ChannelQuality = (1 << 2),
        SCOLink = (1 << 3),
        HV2Packets = (1 << 4),
        HV3Packets = (1 << 5),
        ULawLog = (1 << 6),
        ALawLog = (1 << 7),
        Cvsd = (1 << 0),
        PagingScheme = (1 << 1),
        PowerControl = (1 << 2),
        TransparentSCOData = (1 << 3),
        FlowControlLagBit0 = (1 << 4),
        FlowControlLagBit1 = (1 << 5),
        FlowControlLagBit2 = (1 << 6),
        BroadcastEncryption = (1 << 7),
        ScatterMode = (1 << 0),
        EnhancedDataRateACL2MbpsMode = (1 << 1),
        EnhancedDataRateACL3MbpsMode = (1 << 2),
        EnhancedInquiryScan = (1 << 3),
        InterlacedInquiryScan = (1 << 4),
        InterlacedPageScan = (1 << 5),
        RSSIWithInquiryResult = (1 << 6),
        ExtendedSCOLink = (1 << 7),
        EV4Packets = (1 << 0),
        EV5Packets = (1 << 1),
        AbsenceMasks = (1 << 2),
        AFHCapableSlave = (1 << 3),
        AFHClassificationSlave = (1 << 4),
        AliasAuhentication = (1 << 5),
        LESupportedController = (1 << 6),
        kBluetoothFeature3SlotEnhancedDataRateACLPackets = (1 << 7),
        kBluetoothFeature5SlotEnhancedDataRateACLPackets = (1 << 0),
        SniffSubrating = (1 << 1),
        PauseEncryption = (1 << 2),
        AFHCapableMaster = (1 << 3),
        AFHClassificationMaster = (1 << 4),
        EnhancedDataRateeSCO2MbpsMode = (1 << 5),
        EnhancedDataRateeSCO3MbpsMode = (1 << 6),
        kBluetoothFeature3SlotEnhancedDataRateeSCOPackets = (1 << 7),
        ExtendedInquiryResponse = (1 << 0),
        SecureSimplePairing = (1 << 3),
        EncapsulatedPDU = (1 << 4),
        ErroneousDataReporting = (1 << 5),
        NonFlushablePacketBoundaryFlag = (1 << 6),
        LinkSupervisionTimeoutChangedEvent = (1 << 0),
        InquiryTransmissionPowerLevel = (1 << 1),
        ExtendedFeatures = (1 << 7),
        SimpleSecurePairingHostMode = (1 << 0)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothEventFilterCondition
    {
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciFailedContactInfo
    {
        public ushort count;

        public ushort handle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciRssiInfo
    {
        public ushort handle;

        public sbyte RssiValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciLinkQualityInfo
    {
        public ushort handle;

        public byte qualityValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciRoleInfo
    {
        public byte role;

        public ushort handle;
    }

    public enum HciRole : uint
    {
        MasterRole = 0,
        SlaveRole = 1
    }

    public enum HciLinkPolicySettingsValues : uint
    {
        DisableAllLMModes = 0,
        EnableMasterSlaveSwitch = 1,
        EnableHoldMode = 2,
        EnableSniffMode = 4,
        EnableParkMode = 8,
        ReservedForFutureUse = 16
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciLinkPolicySettingsInfo
    {
        public ushort settings;

        public ushort handle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciQualityOfServiceSetupParams
    {
        public byte flags;

        public byte serviceType;

        public uint tokenRate;

        public uint peakBandwidth;

        public uint latency;

        public uint delayVariation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciSetupSynchronousConnectionParams
    {
        public uint transmitBandwidth;

        public uint receiveBandwidth;

        public ushort maxLatency;

        public ushort voiceSetting;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciAcceptSynchronousConnectionRequestParams
    {
        public uint transmitBandwidth;

        public uint receiveBandwidth;

        public ushort maxLatency;

        public ushort contentFormat;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciEnhancedSetupSynchronousConnectionParams
    {
        public uint transmitBandwidth;

        public uint receiveBandwidth;

        public ulong transmitCodingFormat;

        public ulong receiveCodingFormat;

        public byte transmitCodecFrameSize;

        public byte receiveCodecFrameSize;

        public ushort inputBandwidth;

        public ushort outputBandwidth;

        public ulong inputCodingFormat;

        public ulong outputCodingFormat;

        public ushort inputCodedDataSize;

        public ushort outputCodedDataSize;

        public byte inputPCMDataFormat;

        public byte outputPCMDataFormat;

        public byte inputPCMSamplePayloadMSBPosition;

        public byte outputPCMSamplePayloadMSBPosition;

        public byte inputDataPath;

        public byte outputDataPath;

        public byte inputTransportUnitSize;

        public byte outputTransportUnitSize;

        public ushort maxLatency;

        public ushort voiceSetting;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciEnhancedAcceptSynchronousConnectionRequestParams
    {
        public uint transmitBandwidth;

        public uint receiveBandwidth;

        public ulong transmitCodingFormat;

        public ulong receiveCodingFormat;

        public byte transmitCodecFrameSize;

        public byte receiveCodecFrameSize;

        public ushort inputBandwidth;

        public ushort outputBandwidth;

        public ulong inputCodingFormat;

        public ulong outputCodingFormat;

        public ushort inputCodedDataSize;

        public ushort outputCodedDataSize;

        public byte inputPCMDataFormat;

        public byte outputPCMDataFormat;

        public byte inputPCMSamplePayloadMSBPosition;

        public byte outputPCMSamplePayloadMSBPosition;

        public byte inputDataPath;

        public byte outputDataPath;

        public byte inputTransportUnitSize;

        public byte outputTransportUnitSize;

        public ushort maxLatency;

        public ushort voiceSetting;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    public enum HciLoopbackMode : uint
    {
        Off = 0,
        Local = 1,
        Remote = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothReadClockInfo
    {
        public ushort handle;

        public uint clock;

        public ushort accuracy;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciEventFlowSpecificationData
    {
        public ushort connectionHandle;

        public byte flags;

        public byte flowDirection;

        public byte serviceType;

        public uint tokenRate;

        public uint tokenBucketSize;

        public uint peakBandwidth;

        public uint accessLatency;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciVersionInfo
    {
        public ushort manufacturerName;

        public byte lmpVersion;

        public ushort lmpSubVersion;

        public byte hciVersion;

        public ushort hciRevision;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciBufferSize
    {
        public ushort ACLDataPacketLength;

        public byte SCODataPacketLength;

        public ushort totalNumACLDataPackets;

        public ushort totalNumSCODataPackets;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciLEBufferSize
    {
        public ushort ACLDataPacketLength;

        public byte totalNumACLDataPackets;
    }

    public enum HciTimeoutValues : uint
    {
        DefaultPageTimeout = 10000
    }

    public enum HciDeleteStoredLinkKeyFlags : uint
    {
        KeyForSpecifiedDeviceOnly = 0,
        AllStoredLinkKeys = 1
    }

    public enum HciReadStoredLinkKeysFlags : uint
    {
        turnLinkKeyForSpecifiedDeviceOnly = 0,
        adAllStoredLinkKeys = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciStoredLinkKeysInfo
    {
        public ushort numLinkKeysRead;

        public ushort maxNumLinkKeysAllowedInDevice;
    }

    public enum HciPageScanModes : uint
    {
        MandatoryPageScanMode = 0,
        OptionalPageScanMode1 = 1,
        OptionalPageScanMode2 = 2,
        OptionalPageScanMode3 = 3
    }

    public enum HciPageScanPeriodModes : uint
    {
        kP0Mode = 0,
        kP1Mode = 1,
        kP2Mode = 2
    }

    public enum HciPageScanEnableStates : uint
    {
        NoScansEnabled = 0,
        InquiryScanEnabledPageScanDisabled = 1,
        InquiryScanDisabledPageScanEnabled = 2,
        InquiryScanEnabledPageScanEnabled = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciScanActivity
    {
        public ushort scanInterval;

        public ushort scanWindow;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciInquiryAccessCode
    {
        public byte[] data;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothHCICurrentInquiryAccessCodes
    //{
    //	public byte count;

    //	public unsafe BluetoothHCIInquiryAccessCode* codes;

    //    public const uint MaximumNumberOfInquiryAccessCodes = 64;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct HciCurrentInquiryAccessCodesForWrite
    {
        public byte count;

        public byte[] codes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciLinkSupervisionTimeout
    {
        public ushort handle;

        public ushort timeout;
    }

    public enum HciScoFlowControlStates : uint
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum HciGeneralFlowControlStates : uint
    {
        ostControllerToHostFlowControlOff = 0,
        CIACLDataPacketsOnHCISCODataPacketsOff = 1,
        CIACLDataPacketsOffHCISCODataPacketsOn = 2,
        CIACLDataPacketsOnHCISCODataPacketsOn = 3
    }

    public enum HciTransmitReadPowerLevelTypes : uint
    {
        CurrentTransmitPowerLevel = 0,
        MaximumTransmitPowerLevel = 1
    }

    public enum HciAFHChannelAssessmentModes : uint
    {
        Disabled = 0,
        Enabled = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciTransmitPowerLevelInfo
    {
        public ushort handle;

        public sbyte level;
    }

    public enum HciHoldModeActivityStates : uint
    {
        MaintainCurrentPowerState = 0,
        SuspendPageScan = 1,
        SuspendInquiryScan = 2,
        SuspendPeriodicInquiries = 3
    }

    public enum HciAuthentionEnableModes : uint
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum HciEncryptionMode : byte
    {
        Disabled = 0,
        OnlyForPointToPointPackets = 1,
        ForBothPointToPointAndBroadcastPackets = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciAutomaticFlushTimeoutInfo
    {
        public ushort handle;

        public ushort timeout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothTransportInfo
    {
        public uint productID;

        public uint vendorID;

        public uint type;

        public sbyte[] productName;

        public sbyte[] vendorName;

        public ulong totalDataBytesSent;

        public ulong totalSCOBytesSent;

        public ulong totalDataBytesReceived;

        public ulong totalSCOBytesReceived;
    }

    public enum BluetoothTransportType : uint
    {
        Usb = 1,
        PCCard = 2,
        PciCard = 3,
        Uart = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciInquiryResult
    {
        public BluetoothDeviceAddress deviceAddress;

        public byte pageScanRepetitionMode;

        public byte pageScanPeriodMode;

        public byte pageScanMode;

        public uint classOfDevice;

        public ushort clockOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciInquiryResults
    {
        public HciInquiryResult[] results;

        public uint count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciInquiryWithRssiResult
    {
        public BluetoothDeviceAddress deviceAddress;

        public byte pageScanRepetitionMode;

        public byte reserved;

        public uint classOfDevice;

        public ushort clockOffset;

        public sbyte RSSIValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciInquiryWithRssiResults
    {
        public HciInquiryWithRssiResult[] results;

        public uint count;
    }

    public enum HciFecRequiredValues : uint
    {
        Required = 0,
        NotRequired = 1
    }

    public enum HciInquiryMode : uint
    {
        Standard = 0,
        WithRssi = 1,
        WithRssiOrExtendedInquiryResultFormat = 2
    }

    public enum HciInquiryScanType : uint
    {
        Standard = 0,
        Interlaced = 1,
        ReservedStart = 2,
        ReservedEnd = 255
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciExtendedInquiryResponse
    {
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciReadExtendedInquiryResponseResults
    {
        public byte outFECRequired;

        public HciExtendedInquiryResponse extendedInquiryResponse;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciExtendedInquiryResult
    {
        public byte numberOfReponses;

        public BluetoothDeviceAddress deviceAddress;

        public byte pageScanRepetitionMode;

        public byte reserved;

        public uint classOfDevice;

        public ushort clockOffset;

        public sbyte RssiValue;

        public HciExtendedInquiryResponse extendedInquiryResponse;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciReadLmpHandleResults
    {
        public ushort handle;

        public byte lmp_handle;

        public uint reserved;
    }

    public enum HciSimplePairingMode : uint
    {
        NotSet = 0,
        Enabled = 1
    }

    public enum SimplePairingDebugMode : uint
    {
        Disabled = 0,
        Enabled = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciSimplePairingOobData
    {
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HciReadLocalOobDataResults
    {
        public HciSimplePairingOobData hash;

        public HciSimplePairingOobData randomizer;
    }

    public enum BluetoothIOCapabilities : uint
    {
        DisplayOnly = 0,
        DisplayYesNo = 1,
        KeyboardOnly = 2,
        NoInputNoOutput = 3
    }

    public enum BluetoothOOBDataPresenceValues : uint
    {
        NotPresent = 0,
        FromRemoteDevicePresent = 1
    }

    public enum BluetoothAuthenticationRequirementsValues : uint
    {
        NotRequired = 0,
        Required = 1,
        NotRequiredNoBonding = 0,
        RequiredNoBonding = 1,
        NotRequiredDedicatedBonding = 2,
        RequiredDedicatedBonding = 3,
        NotRequiredGeneralBonding = 4,
        RequiredGeneralBonding = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothIOCapabilityResponse
    {
        public BluetoothDeviceAddress deviceAddress;

        public byte ioCapability;

        public byte OOBDataPresence;

        public byte authenticationRequirements;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothUserPasskeyNotification
    {
        public BluetoothDeviceAddress deviceAddress;

        public uint passkey;
    }

    public enum BluetoothKeypressNotificationTypes : uint
    {
        EntryStarted = 0,
        DigitEntered = 1,
        DigitErased = 2,
        Cleared = 3,
        EntryCompleted = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothKeypressNotification
    {
        public BluetoothDeviceAddress deviceAddress;

        public byte notificationType;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothRemoteHostSupportedFeaturesNotification
    //{
    //	public BluetoothDeviceAddress deviceAddress;

    //	public BluetoothHCISupportedFeatures hostSupportedFeatures;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothAFHHostChannelClassification
    {
        public byte[] data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothAFHResults
    {
        public ushort handle;

        public byte mode;

        public byte[] afhMap;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothUserConfirmationRequest
    {
        public BluetoothDeviceAddress deviceAddress;

        public uint numericValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventSimplePairingCompleteResults
    {
        public BluetoothDeviceAddress deviceAddress;
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum kBluetoothHCI : uint
    //{
    //	CommandPacketHeaderSize = 3,
    //	CommandPacketMaxDataSize = 255,
    //	MaxCommandPacketSize = CommandPacketHeaderSize + CommandPacketMaxDataSize,
    //	EventPacketHeaderSize = 2,
    //	EventPacketMaxDataSize = 255,
    //	MaxEventPacketSize = EventPacketHeaderSize + EventPacketMaxDataSize,
    //	DataPacketHeaderSize = 4,
    //	DataPacketMaxDataSize = 65535,
    //	MaxDataPacketSize = DataPacketHeaderSize + DataPacketMaxDataSize
    //}

    public enum BluetoothLinkType : byte
    {
        SCOConnection = 0,
        ACLConnection = 1,
        ESCOConnection = 2,
        LinkTypeNone = 255
    }

    public enum BluetoothVoiceSettingInputCoding : uint
    {
        Mask = 768,
        LinearInputCoding = 0,
        ULawInputCoding = 256,
        ALawInputCoding = 512
    }

    public enum BluetoothVoiceSettingInputDataFormat : uint
    {
        Mask = 192,
        kBluetoothVoiceSettingInputDataFormat1sComplement = 0,
        kBluetoothVoiceSettingInputDataFormat2sComplement = 64,
        SignMagnitude = 128,
        Unsigned = 192
    }

    public enum BluetoothVoiceSettingInputSampleSize : uint
    {
        Mask = 32,
        kBluetoothVoiceSettingInputSampleSize8Bit = 0,
        kBluetoothVoiceSettingInputSampleSize16Bit = 32
    }

    public enum BluetoothVoiceSettingPCMBitPosition : uint
    {
        BluetoothVoiceSettingPCMBitPositionMask = 28
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum kBluetoothVoiceSettingAirCodingFormat : uint
    //{
    //	Mask = 3,
    //	Cvsd = 0,
    //	ULaw = 1,
    //	ALaw = 2,
    //	TransparentData = 3
    //}

    public enum HciRetransmissionEffortType : uint
    {
        None = 0,
        AtLeastOneAndOptimizeForPower = 1,
        AtLeastOneAndOptimizeLinkQuality = 2,
        DontCare = 255
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum kBluetoothAirMode : uint
    //{
    //	ULawLog = 0,
    //	ALawLog = 1,
    //	Cvsd = 2,
    //	TransparentData = 3
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothSynchronousConnectionInfo
    {
        public uint transmitBandWidth;

        public uint receiveBandWidth;

        public ushort maxLatency;

        public ushort voiceSetting;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothEnhancedSynchronousConnectionInfo
    {
        public uint transmitBandWidth;

        public uint receiveBandWidth;

        public ulong transmitCodingFormat;

        public ulong receiveCodingFormat;

        public ushort transmitCodecFrameSize;

        public ushort receiveCodecFrameSize;

        public uint inputBandwidth;

        public uint outputBandwidth;

        public ulong inputCodingFormat;

        public ulong outputCodingFormat;

        public ushort inputCodedDataSize;

        public ushort outputCodedDataSize;

        public byte inputPCMDataFormat;

        public byte outputPCMDataFormat;

        public byte inputPCMSampelPayloadMSBPosition;

        public byte outputPCMSampelPayloadMSBPosition;

        public byte inputDataPath;

        public byte outputDataPath;

        public byte inputTransportUnitSize;

        public byte outputTransportUnitSize;

        public ushort maxLatency;

        public ushort voiceSetting;

        public byte retransmissionEffort;

        public ushort packetType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventSynchronousConnectionCompleteResults
    {
        public ushort connectionHandle;

        public BluetoothDeviceAddress deviceAddress;

        public byte linkType;

        public byte transmissionInterval;

        public byte retransmissionWindow;

        public ushort receivePacketLength;

        public ushort transmitPacketLength;

        public byte airMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventSynchronousConnectionChangedResults
    {
        public ushort connectionHandle;

        public byte transmissionInterval;

        public byte retransmissionWindow;

        public ushort receivePacketLength;

        public ushort transmitPacketLength;
    }

    public enum HciEvent : uint
    {
        InquiryComplete = 1,
        InquiryResult = 2,
        ConnectionComplete = 3,
        ConnectionRequest = 4,
        DisconnectionComplete = 5,
        AuthenticationComplete = 6,
        BluetoothHCIEventRemoteNameRequestComplete = 7,
        BluetoothHCIEventEncryptionChange = 8,
        BluetoothHCIEventChangeConnectionLinkKeyComplete = 9,
        BluetoothHCIEventMasterLinkKeyComplete = 10,
        BluetoothHCIEventReadRemoteSupportedFeaturesComplete = 11,
        BluetoothHCIEventReadRemoteVersionInformationComplete = 12,
        BluetoothHCIEventQoSSetupComplete = 13,
        BluetoothHCIEventCommandComplete = 14,
        BluetoothHCIEventCommandStatus = 15,
        BluetoothHCIEventHardwareError = 16,
        BluetoothHCIEventFlushOccurred = 17,
        BluetoothHCIEventRoleChange = 18,
        BluetoothHCIEventNumberOfCompletedPackets = 19,
        BluetoothHCIEventModeChange = 20,
        BluetoothHCIEventReturnLinkKeys = 21,
        BluetoothHCIEventPINCodeRequest = 22,
        BluetoothHCIEventLinkKeyRequest = 23,
        BluetoothHCIEventLinkKeyNotification = 24,
        BluetoothHCIEventLoopbackCommand = 25,
        BluetoothHCIEventDataBufferOverflow = 26,
        BluetoothHCIEventMaxSlotsChange = 27,
        BluetoothHCIEventReadClockOffsetComplete = 28,
        BluetoothHCIEventConnectionPacketType = 29,
        BluetoothHCIEventQoSViolation = 30,
        BluetoothHCIEventPageScanModeChange = 31,
        BluetoothHCIEventPageScanRepetitionModeChange = 32,
        BluetoothHCIEventFlowSpecificationComplete = 33,
        BluetoothHCIEventInquiryResultWithRSSI = 34,
        BluetoothHCIEventReadRemoteExtendedFeaturesComplete = 35,
        BluetoothHCIEventSynchronousConnectionComplete = 44,
        BluetoothHCIEventSynchronousConnectionChanged = 45,
        BluetoothHCIEventSniffSubrating = 46,
        BluetoothHCIEventExtendedInquiryResult = 47,
        BluetoothHCIEventEncryptionKeyRefreshComplete = 48,
        BluetoothHCIEventIOCapabilityRequest = 49,
        BluetoothHCIEventIOCapabilityResponse = 50,
        BluetoothHCIEventUserConfirmationRequest = 51,
        BluetoothHCIEventUserPasskeyRequest = 52,
        BluetoothHCIEventRemoteOOBDataRequest = 53,
        BluetoothHCIEventSimplePairingComplete = 54,
        BluetoothHCIEventLinkSupervisionTimeoutChanged = 56,
        BluetoothHCIEventEnhancedFlushComplete = 57,
        BluetoothHCIEventUserPasskeyNotification = 59,
        BluetoothHCIEventKeypressNotification = 60,
        BluetoothHCIEventRemoteHostSupportedFeaturesNotification = 61,
        BluetoothHCIEventLEMetaEvent = 62,
        BluetoothHCISubEventLEConnectionComplete = 1,
        BluetoothHCISubEventLEAdvertisingReport = 2,
        BluetoothHCISubEventLEConnectionUpdateComplete = 3,
        BluetoothHCISubEventLEReadRemoteUsedFeaturesComplete = 4,
        BluetoothHCISubEventLELongTermKeyRequest = 5,
        BluetoothHCISubEventLERemoteConnectionParameterRequest = 6,
        BluetoothHCISubEventLEDataLengthChange = 7,
        BluetoothHCISubEventLEReadLocalP256PublicKeyComplete = 8,
        BluetoothHCISubEventLEGenerateDHKeyComplete = 9,
        BluetoothHCISubEventLEEnhancedConnectionComplete = 10,
        BluetoothHCISubEventLEDirectAdvertisingReport = 11,
        BluetoothHCISubEventLEPhyUpdateComplete = 12,
        BluetoothHCISubEventLEExtendedAdvertising = 13,
        BluetoothHCISubEventLEPeriodicAdvertisingSyncEstablished = 14,
        BluetoothHCISubEventLEPeriodicAdvertisingReport = 15,
        BluetoothHCISubEventLEPeriodicAdvertisingSyncLost = 16,
        BluetoothHCISubEventLEScanTimeout = 17,
        BluetoothHCISubEventLEAdvertisingSetTerminated = 18,
        BluetoothHCISubEventLEScanRequestReceived = 19,
        BluetoothHCISubEventLEChannelSelectionAlgorithm = 20,
        LeExtendedAdvertisingReportSubevent = 13,
        LePeriodicAdvertisingSyncEstablishedSubevent = 14,
        LePeriodicAdvertisingReportSubevent = 15,
        LePeriodicAdvertisingSyncLostSubevent = 16,
        LeScanTimeoutSubevent = 17,
        LeAdvertisingSetTerminatedSubevent = 18,
        LeScanRequestReceivedSubevent = 19,
        LeChannelSelectionAlgorithmSubevent = 20,
        BluetoothHCIEventPhysicalLinkComplete = 64,
        BluetoothHCIEventChannelSelected = 65,
        BluetoothHCIEventDisconnectionPhysicalLinkComplete = 66,
        BluetoothHCIEventPhysicalLinkLossEarlyWarning = 67,
        BluetoothHCIEventPhysicalLinkRecovery = 68,
        BluetoothHCIEventLogicalLinkComplete = 69,
        BluetoothHCIEventDisconnectionLogicalLinkComplete = 70,
        BluetoothHCIEventFlowSpecModifyComplete = 71,
        BluetoothHCIEventNumberOfCompletedDataBlocks = 72,
        BluetoothHCIEventShortRangeModeChangeComplete = 76,
        BluetoothHCIEventAMPStatusChange = 77,
        BluetoothHCIEventAMPStartTest = 73,
        BluetoothHCIEventAMPTestEnd = 74,
        BluetoothHCIEventAMPReceiverReport = 75,
        BluetoothHCIEventLogoTesting = 254,
        BluetoothHCIEventVendorSpecific = 255
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum BluetoothHCIEventMask : uint
    //{
    //	None = 0,
    //	InquiryComplete = 1,
    //	InquiryResult = 2,
    //	ConnectionComplete = 4,
    //	ConnectionRequest = 8,
    //	DisconnectionComplete = 16,
    //	AuthenticationComplete = 32,
    //	RemoteNameRequestComplete = 64,
    //	EncryptionChange = 128,
    //	ChangeConnectionLinkKeyComplete = 256,
    //	MasterLinkKeyComplete = 512,
    //	ReadRemoteSupportedFeaturesComplete = 1024,
    //	ReadRemoteVersionInformationComplete = 2048,
    //	QoSSetupComplete = 4096,
    //	CommandComplete = 8192,
    //	CommandStatus = 16384,
    //	HardwareError = 32768,
    //	FlushOccurred = 65536,
    //	RoleChange = 131072,
    //	NumberOfCompletedPackets = 262144,
    //	ModeChange = 524288,
    //	ReturnLinkKeys = 1048576,
    //	PINCodeRequest = 2097152,
    //	LinkKeyRequest = 4194304,
    //	LinkKeyNotification = 8388608,
    //	LoopbackCommand = 16777216,
    //	DataBufferOverflow = 33554432,
    //	MaxSlotsChange = 67108864,
    //	ReadClockOffsetComplete = 134217728,
    //	ConnectionPacketTypeChanged = 268435456,
    //	QoSViolation = 536870912,
    //	PageScanModeChange = 1073741824,
    //	PageScanRepetitionModeChange = 2147483648u,
    //	All = 4294967295u,
    //	Default = All
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventConnectionCompleteResults
    {
        public ushort ConnectionHandle;

        public BluetoothDeviceAddress DeviceAddress;

        public BluetoothLinkType LinkType;

        public byte EncryptionMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventLEConnectionCompleteResults
    {
        public ushort connectionHandle;

        public byte role;

        public byte peerAddressType;

        public BluetoothDeviceAddress peerAddress;

        public ushort connInterval;

        public ushort connLatency;

        public ushort supervisionTimeout;

        public byte masterClockAccuracy;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventLEConnectionUpdateCompleteResults
    {
        public ushort connectionHandle;

        public ushort connInterval;

        public ushort connLatency;

        public ushort supervisionTimeout;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothHCIEventLEReadRemoteUsedFeaturesCompleteResults
    //{
    //	public ushort connectionHandle;

    //	public BluetoothHCISupportedFeatures usedFeatures;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventDisconnectionCompleteResults
    {
        public ushort connectionHandle;

        public byte reason;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothHCIEventReadSupportedFeaturesResults
    //{
    //	public ushort connectionHandle;

    //	public BluetoothHCISupportedFeatures supportedFeatures;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventReadExtendedFeaturesResults
    {
        public ushort connectionHandle;

        public BluetoothHCIExtendedFeaturesInfo supportedFeaturesInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventReadRemoteVersionInfoResults
    {
        public ushort connectionHandle;

        public byte lmpVersion;

        public ushort manufacturerName;

        public ushort lmpSubversion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventRemoteNameRequestResults
    {
        public BluetoothDeviceAddress deviceAddress;

        public byte[] deviceName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventReadClockOffsetResults
    {
        public ushort connectionHandle;

        public ushort clockOffset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventConnectionRequestResults
    {
        public BluetoothDeviceAddress DeviceAddress;

        public uint ClassOfDevice;

        public BluetoothLinkType LinkType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventLinkKeyNotificationResults
    {
        public BluetoothDeviceAddress DeviceAddress;

        public BluetoothKey LinkKey;

        public byte KeyType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventMaxSlotsChangeResults
    {
        public ushort ConnectionHandle;

        public byte MaxSlots;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventModeChangeResults
    {
        public ushort ConnectionHandle;

        public byte Mode;

        public ushort ModeInterval;
    }

    /*[StructLayout (LayoutKind.Sequential)]
    public struct BluetoothHCIEventReturnLinkKeysResults
    {
        public byte numLinkKeys;

        [StructLayout (LayoutKind.Sequential)]
        public struct 
        {
            public BluetoothDeviceAddress deviceAddress;

            public BluetoothKey linkKey;
        }


        public [anonymous Record: struct BluetoothHCIEventReturnLinkKeysResults::(anonymous at /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX.sdk/System/Library/Frameworks/IOBluetooth.framework/Headers/Bluetooth.h:2457:2)][] linkKeys;
    }*/

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventAuthenticationCompleteResults
    {
        public ushort ConnectionHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventEncryptionChangeResults
    {
        public ushort ConnectionHandle;

        public byte Enable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventChangeConnectionLinkKeyCompleteResults
    {
        public ushort ConnectionHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventMasterLinkKeyCompleteResults
    {
        public ushort ConnectionHandle;

        public byte keyFlag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventQoSSetupCompleteResults
    {
        public ushort ConnectionHandle;

        public HciQualityOfServiceSetupParams setupParams;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventHardwareErrorResults
    {
        public byte Error;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventFlushOccurredResults
    {
        public ushort ConnectionHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventRoleChangeResults
    {
        public ushort ConnectionHandle;

        public BluetoothDeviceAddress DeviceAddress;

        public byte Role;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventDataBufferOverflowResults
    {
        public BluetoothLinkType LinkType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventConnectionPacketTypeResults
    {
        public ushort ConnectionHandle;

        public ushort PacketType;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothHCIEventReadRemoteSupportedFeaturesResults
    //{
    //	public byte error;

    //	public ushort connectionHandle;

    //	public BluetoothHCISupportedFeatures lmpFeatures;
    //}

    //[StructLayout (LayoutKind.Sequential)]
    //public struct BluetoothHCIEventReadRemoteExtendedFeaturesResults
    //{
    //	public byte error;

    //	public ushort connectionHandle;

    //	public byte page;

    //	public byte maxPage;

    //	public BluetoothHCISupportedFeatures lmpFeatures;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventQoSViolationResults
    {
        public ushort ConnectionHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventPageScanModeChangeResults
    {
        public BluetoothDeviceAddress DeviceAddress;

        public byte PageScanMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventPageScanRepetitionModeChangeResults
    {
        public BluetoothDeviceAddress DeviceAddress;

        public byte PageScanRepetitionMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventVendorSpecificResults
    {
        public byte Length;

        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventEncryptionKeyRefreshCompleteResults
    {
        public ushort ConnectionHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventSniffSubratingResults
    {
        public ushort ConnectionHandle;

        public ushort MaxTransmitLatency;

        public ushort MaxReceiveLatency;

        public ushort MinRemoteTimeout;

        public ushort MinLocalTimeout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventLEMetaResults
    {
        public byte Length;

        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIEventLELongTermKeyRequestResults
    {
        public ushort ConnectionHandle;

        public byte[] randomNumber;

        public ushort ediv;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluetoothHCIRequestCallbackInfo
    {
        public ulong userCallback;

        public ulong userRefCon;

        public ulong internalRefCon;

        public ulong asyncIDRefCon;

        public ulong reserved;
    }

    public enum BluetoothHCIError : uint
    {
        Success = 0,
        UnknownHCICommand = 1,
        NoConnection = 2,
        HardwareFailure = 3,
        PageTimeout = 4,
        AuthenticationFailure = 5,
        KeyMissing = 6,
        MemoryFull = 7,
        ConnectionTimeout = 8,
        MaxNumberOfConnections = 9,
        MaxNumberOfSCOConnectionsToADevice = 10,
        ACLConnectionAlreadyExists = 11,
        CommandDisallowed = 12,
        HostRejectedLimitedResources = 13,
        HostRejectedSecurityReasons = 14,
        HostRejectedRemoteDeviceIsPersonal = 15,
        HostTimeout = 16,
        UnsupportedFeatureOrParameterValue = 17,
        InvalidHCICommandParameters = 18,
        OtherEndTerminatedConnectionUserEnded = 19,
        OtherEndTerminatedConnectionLowResources = 20,
        OtherEndTerminatedConnectionAboutToPowerOff = 21,
        ConnectionTerminatedByLocalHost = 22,
        RepeatedAttempts = 23,
        PairingNotAllowed = 24,
        UnknownLMPPDU = 25,
        UnsupportedRemoteFeature = 26,
        SCOOffsetRejected = 27,
        SCOIntervalRejected = 28,
        SCOAirModeRejected = 29,
        InvalidLMPParameters = 30,
        UnspecifiedError = 31,
        UnsupportedLMPParameterValue = 32,
        RoleChangeNotAllowed = 33,
        LMPResponseTimeout = 34,
        LMPErrorTransactionCollision = 35,
        LMPPDUNotAllowed = 36,
        EncryptionModeNotAcceptable = 37,
        UnitKeyUsed = 38,
        QoSNotSupported = 39,
        InstantPassed = 40,
        PairingWithUnitKeyNotSupported = 41,
        HostRejectedUnacceptableDeviceAddress = 15,
        DifferentTransactionCollision = 42,
        QoSUnacceptableParameter = 44,
        QoSRejected = 45,
        ChannelClassificationNotSupported = 46,
        InsufficientSecurity = 47,
        ParameterOutOfMandatoryRange = 48,
        RoleSwitchPending = 49,
        ReservedSlotViolation = 52,
        RoleSwitchFailed = 53,
        ExtendedInquiryResponseTooLarge = 54,
        SecureSimplePairingNotSupportedByHost = 55,
        HostBusyPairing = 56,
        ConnectionRejectedDueToNoSuitableChannelFound = 57,
        ControllerBusy = 58,
        UnacceptableConnectionInterval = 59,
        DirectedAdvertisingTimeout = 60,
        ConnectionTerminatedDueToMICFailure = 61,
        ConnectionFailedToBeEstablished = 62,
        MACConnectionFailed = 63,
        CoarseClockAdjustmentRejected = 64,
        Max = 64,
        PowerIsOFF = (Max + 1),
    }

    public enum BluetoothHCIPowerState : uint
    {
        Off = 0,
        On = 1,
        Unintialized = 255
    }

    public enum BluetoothHCITransportUSB : uint
    {
        ClassCode = 224,
        SubClassCode = 1,
        ProtocolCode = 1
    }

    public enum BluetoothL2CAPTCIEventID : uint
    {
        Reserved = 0,
        L2CA_ConnectInd = 1,
        L2CA_ConfigInd = 2,
        L2CA_DisconnectInd = 3,
        L2CA_QoSViolationInd = 4,
        L2CA_TimeOutInd = 5
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum kBluetoothL2CAPTCICommand : uint
    //{
    //	Reserved = 0,
    //	L2CA_ConnectReq = 1,
    //	L2CA_DisconnectReq = 2,
    //	L2CA_ConfigReq = 3,
    //	L2CA_DisableCLT = 4,
    //	L2CA_EnableCLT = 5,
    //	L2CA_GroupCreate = 6,
    //	L2CA_GroupClose = 7,
    //	L2CA_GroupAddMember = 8,
    //	L2CA_GroupRemoveMember = 9,
    //	L2CA_GroupMembership = 10,
    //	L2CA_WriteData = 11,
    //	L2CA_ReadData = 12,
    //	L2CA_Ping = 13,
    //	L2CA_GetInfo = 14,
    //	L2CA_Reserved1 = 15,
    //	L2CA_Reserved2 = 16,
    //	L2CA_ConnectResp = 17,
    //	L2CA_DisconnectResp = 18,
    //	L2CA_ConfigResp = 19
    //}

    public enum RFCommParityType : uint
    {
        NoParity = 0,
        OddParity,
        EvenParity,
        MaxParity
    }

    public enum RFCommLineStatus : uint
    {
        NoError = 0,
        OverrunError,
        ParityError,
        FramingError
    }

    //[Verify (InferredFromMemberPrefix)]
    //public enum BluetoothSDPPDUID : uint
    //{
    //	Reserved = 0,
    //	ErrorResponse = 1,
    //	ServiceSearchRequest = 2,
    //	ServiceSearchResponse = 3,
    //	ServiceAttributeRequest = 4,
    //	ServiceAttributeResponse = 5,
    //	ServiceSearchAttributeRequest = 6,
    //	ServiceSearchAttributeResponse = 7
    //}

    public enum BluetoothSDPErrorCode : uint
    {
        Success = 0,
        Reserved = 0,
        InvalidSDPVersion = 1,
        InvalidServiceRecordHandle = 2,
        InvalidRequestSyntax = 3,
        InvalidPDUSize = 4,
        InvalidContinuationState = 5,
        InsufficientResources = 6,
        ReservedStart = 7,
        ReservedEnd = 65535
    }

    [Native]
    public enum BluetoothSDPDataElementType : ulong
    {
        Nil = 0,
        UnsignedInt = 1,
        SignedInt = 2,
        Uuid = 3,
        String = 4,
        Boolean = 5,
        DataElementSequence = 6,
        DataElementAlternative = 7,
        Url = 8,
        ReservedStart = 9,
        ReservedEnd = 31
    }

    public enum BluetoothLEScanType : uint
    {
        Passive = 0,
        Active = 1
    }

    public enum BluetoothLEAddressType : uint
    {
        Public = 0,
        Random = 1
    }

    public enum BluetoothLEScanFilter : uint
    {
        None = 0,
        Whitelist = 1
    }

    public enum BluetoothLEScan : uint
    {
        Disable = 0,
        Enable = 1
    }

    public enum BluetoothLEConnectionInterval : uint
    {
        @in = 6,
        ax = 3200
    }

    public enum BluetoothLEScanDuplicateFilter : uint
    {
        Disable = 0,
        Enable = 1
    }

    public enum BluetoothLEAdvertisingType : uint
    {
        ConnectableUndirected = 0,
        ConnectableDirected = 1,
        DiscoverableUndirected = 2,
        NonConnectableUndirected = 3,
        ScanResponse = 4
    }

    //static class CFunctions
    //{
    //	// extern void IOBluetoothIgnoreHIDDevice (IOBluetoothDeviceRef device);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe void IOBluetoothIgnoreHIDDevice (IOBluetoothDeviceRef* device);

    //	// extern void IOBluetoothRemoveIgnoredHIDDevice (IOBluetoothDeviceRef device);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe void IOBluetoothRemoveIgnoredHIDDevice (IOBluetoothDeviceRef* device);

    //	// extern void IOBluetoothUserNotificationUnregister (IOBluetoothUserNotificationRef notificationRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe void IOBluetoothUserNotificationUnregister (IOBluetoothUserNotificationRef* notificationRef);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRegisterForDeviceConnectNotifications (IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRegisterForDeviceConnectNotifications (IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothDeviceRegisterForDisconnectNotification (IOBluetoothDeviceRef inDevice, IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothDeviceRegisterForDisconnectNotification (IOBluetoothDeviceRef* inDevice, IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRegisterForL2CAPChannelOpenNotifications (IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRegisterForL2CAPChannelOpenNotifications (IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRegisterForFilteredL2CAPChannelOpenNotifications (IOBluetoothUserNotificationCallback callback, void *inRefCon, BluetoothL2CAPPSM inPSM, IOBluetoothUserNotificationChannelDirection inDirection);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRegisterForFilteredL2CAPChannelOpenNotifications (IOBluetoothUserNotificationCallback* callback, void* inRefCon, ushort inPSM, IOBluetoothUserNotificationChannelDirection inDirection);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothL2CAPChannelRegisterForChannelCloseNotification (IOBluetoothL2CAPChannelRef channel, IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothL2CAPChannelRegisterForChannelCloseNotification (IOBluetoothL2CAPChannelRef* channel, IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRegisterForRFCOMMChannelOpenNotifications (IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRegisterForRFCOMMChannelOpenNotifications (IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRegisterForFilteredRFCOMMChannelOpenNotifications (IOBluetoothUserNotificationCallback callback, void *inRefCon, BluetoothRFCOMMChannelID channelID, IOBluetoothUserNotificationChannelDirection inDirection);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRegisterForFilteredRFCOMMChannelOpenNotifications (IOBluetoothUserNotificationCallback* callback, void* inRefCon, byte channelID, IOBluetoothUserNotificationChannelDirection inDirection);

    //	// extern IOBluetoothUserNotificationRef IOBluetoothRFCOMMChannelRegisterForChannelCloseNotification (IOBluetoothRFCOMMChannelRef inChannel, IOBluetoothUserNotificationCallback callback, void *inRefCon);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe IOBluetoothUserNotificationRef* IOBluetoothRFCOMMChannelRegisterForChannelCloseNotification (IOBluetoothRFCOMMChannelRef* inChannel, IOBluetoothUserNotificationCallback* callback, void* inRefCon);

    //	// extern IOReturn IOBluetoothNSStringToDeviceAddress (NSString *inNameString, BluetoothDeviceAddress *outDeviceAddress);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int IOBluetoothNSStringToDeviceAddress (NSString inNameString, BluetoothDeviceAddress* outDeviceAddress);

    //	// extern NSString * IOBluetoothNSStringFromDeviceAddress (const BluetoothDeviceAddress *deviceAddress);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe NSString IOBluetoothNSStringFromDeviceAddress (BluetoothDeviceAddress* deviceAddress);

    //	// extern NSString * IOBluetoothNSStringFromDeviceAddressColon (const BluetoothDeviceAddress *deviceAddress);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe NSString IOBluetoothNSStringFromDeviceAddressColon (BluetoothDeviceAddress* deviceAddress);

    //	// extern Boolean IOBluetoothIsFileAppleDesignatedPIMData (NSString *inFileName);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern byte IOBluetoothIsFileAppleDesignatedPIMData (NSString inFileName);

    //	// extern NSString * IOBluetoothGetUniqueFileNameAndPath (NSString *inName, NSString *inPath);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern NSString IOBluetoothGetUniqueFileNameAndPath (NSString inName, NSString inPath);

    //	// extern long IOBluetoothPackData (void *ioBuffer, const char *inFormat, ...);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe nint IOBluetoothPackData (void* ioBuffer, sbyte* inFormat, IntPtr varArgs);

    //	// extern long IOBluetoothPackDataList (void *ioBuffer, const char *inFormat, struct __va_list_tag *inArgs);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe nint IOBluetoothPackDataList (void* ioBuffer, sbyte* inFormat, va_list[] inArgs);

    //	// extern long IOBluetoothUnpackData (ByteCount inBufferSize, const void *inBuffer, const char *inFormat, ...);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe nint IOBluetoothUnpackData (nuint inBufferSize, void* inBuffer, sbyte* inFormat, IntPtr varArgs);

    //	// extern long IOBluetoothUnpackDataList (ByteCount inBufferSize, const void *inBuffer, const char *inFormat, struct __va_list_tag *inArgs);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe nint IOBluetoothUnpackDataList (nuint inBufferSize, void* inBuffer, sbyte* inFormat, va_list[] inArgs);

    //	// extern long IOBluetoothNumberOfAvailableHIDDevices ();
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern nint IOBluetoothNumberOfAvailableHIDDevices ();

    //	// extern long IOBluetoothNumberOfPointingHIDDevices ();
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern nint IOBluetoothNumberOfPointingHIDDevices ();

    //	// extern long IOBluetoothNumberOfKeyboardHIDDevices ();
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern nint IOBluetoothNumberOfKeyboardHIDDevices ();

    //	// extern long IOBluetoothNumberOfTabletHIDDevices ();
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern nint IOBluetoothNumberOfTabletHIDDevices ();

    //	// extern long IOBluetoothFindNumberOfRegistryEntriesOfClassName (const char *deviceType);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe nint IOBluetoothFindNumberOfRegistryEntriesOfClassName (sbyte* deviceType);

    //	// extern CFDictionaryRef OBEXGetHeaders (const void *inData, size_t inDataSize) __attribute__((cf_returns_retained));
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe CFDictionaryRef* OBEXGetHeaders (void* inData, nuint inDataSize);

    //	// extern CFMutableDataRef OBEXHeadersToBytes (CFDictionaryRef dictionaryOfHeaders);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe CFMutableDataRef* OBEXHeadersToBytes (CFDictionaryRef* dictionaryOfHeaders);

    //	// extern OBEXError OBEXAddNameHeader (CFStringRef name, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddNameHeader (CFStringRef* name, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddDescriptionHeader (CFStringRef description, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddDescriptionHeader (CFStringRef* description, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddCountHeader (uint32_t count, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddCountHeader (uint count, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddTime4ByteHeader (uint32_t time4Byte, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddTime4ByteHeader (uint time4Byte, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddLengthHeader (uint32_t length, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddLengthHeader (uint length, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddTypeHeader (CFStringRef type, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddTypeHeader (CFStringRef* type, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddTimeISOHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddTimeISOHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddTargetHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddTargetHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddHTTPHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddHTTPHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddBodyHeader (const void *inHeaderData, uint32_t inHeaderDataLength, Boolean isEndOfBody, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddBodyHeader (void* inHeaderData, uint inHeaderDataLength, byte isEndOfBody, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddWhoHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddWhoHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddConnectionIDHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddConnectionIDHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddApplicationParameterHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddApplicationParameterHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddByteSequenceHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddByteSequenceHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddObjectClassHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddObjectClassHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddAuthorizationChallengeHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddAuthorizationChallengeHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddAuthorizationResponseHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddAuthorizationResponseHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //	// extern OBEXError OBEXAddUserDefinedHeader (const void *inHeaderData, uint32_t inHeaderDataLength, CFMutableDictionaryRef dictRef);
    //	[DllImport ("__Internal")]
    //	[Verify (PlatformInvoke)]
    //	static extern unsafe int OBEXAddUserDefinedHeader (void* inHeaderData, uint inHeaderDataLength, CFMutableDictionaryRef* dictRef);

    //}

    public enum DeviceSearchOptionsBits : uint
    {
        None = 0,
        AlwaysStartInquiry = (1 << 0),
        DiscardCachedResults = (1 << 1)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceSearchDeviceAttributes
    {
        public BluetoothDeviceAddress Address;

        public byte[] Name;

        public ServiceClassMajor ServiceClassMajor;

        public DeviceClassMajor DeviceClassMajor;

        public DeviceClassMinor DeviceClassMinor;
    }

    //[StructLayout (LayoutKind.Sequential)]
    //public struct DeviceSearchAttributes
    //{
    //	public uint options;

    //	public uint maxResults;

    //	public uint deviceAttributeCount;

    //	public unsafe DeviceSearchDeviceAttributes* attributeList;
    //}

    [Flags]
    public enum DeviceSearchType : uint
    {
        Classic = 1,
        LE = 2
    }

    public enum UserNotificationChannelDirection : uint
    {
        Any = 0,
        Incoming = 1,
        Outgoing = 2
    }

    public enum L2CAPChannelEventType : uint
    {
        Data = 1,
        OpenComplete = 2,
        Closed = 3,
        Reconfigured = 4,
        WriteComplete = 5,
        QueueSpaceAvailable = 6
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct L2CAPChannelDataBlock
    {
        public unsafe void* dataPtr;

        public nuint dataSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct L2CAPChannelEvent
    {
        public L2CAPChannelEventType eventType;

        [StructLayout(LayoutKind.Explicit)]
        public struct U
        {
            [FieldOffset(0)]
            public L2CAPChannelDataBlock data;

            [FieldOffset(0)]
            public unsafe void* writeRefCon;

            [FieldOffset(0)]
            public byte[] padding;
        }


        public U u;

        public int status;
    }

    public enum OBEXErrorCodes
    {
        ErrorRangeMin = -21850,
        ErrorRangeMax = -21899,
        Success = 0,
        GeneralError = -21850,
        NoResourcesError = -21851,
        UnsupportedError = -21852,
        InternalError = -21853,
        BadArgumentError = -21854,
        TimeoutError = -21855,
        BadRequestError = -21856,
        CancelledError = -21857,
        ForbiddenError = -21858,
        UnauthorizedError = -21859,
        NotAcceptableError = -21860,
        ConflictError = -21861,
        MethodNotAllowedError = -21862,
        NotFoundError = -21863,
        NotImplementedError = -21864,
        PreconditionFailedError = -21865,
        SessionBusyError = -21875,
        SessionNotConnectedError = -21876,
        SessionBadRequestError = -21877,
        SessionBadResponseError = -21878,
        SessionNoTransportError = -21879,
        SessionTransportDiedError = -21880,
        SessionTimeoutError = -21881,
        SessionAlreadyConnectedError = -21882
    }

    public enum OBEXHeaderIdentifiers : uint
    {
        Name = 1,
        Description = 5,
        ReservedRangeStart = 16,
        ReservedRangeEnd = 47,
        UserDefinedRangeStart = 48,
        UserDefinedRangeEnd = 63,
        Type = 66,
        TimeISO = 68,
        Target = 70,
        Http = 71,
        Body = 72,
        EndOfBody = 73,
        Who = 74,
        AppParameters = 76,
        AuthorizationChallenge = 77,
        AuthorizationResponse = 78,
        ObjectClass = 79,
        Count = 192,
        Length = 195,
        Time4Byte = 196,
        ConnectionID = 203,
        Obex13wanuuid = 80,
        OBEX13ObjectClass = 81,
        OBEX13SessionParameters = 82,
        OBEX13SessionSequenceNumber = 147,
        OBEX13CreatorID = 207
    }

    public enum OBEXOpCodeResponseValues : uint
    {
        ReservedRangeStart = 0,
        ReservedRangeEnd = 15,
        Continue = 16,
        ContinueWithFinalBit = 144,
        Success = 32,
        SuccessWithFinalBit = 160,
        Created = 33,
        CreatedWithFinalBit = 161,
        Accepted = 34,
        AcceptedWithFinalBit = 162,
        NonAuthoritativeInfo = 35,
        NonAuthoritativeInfoWithFinalBit = 163,
        NoContent = 36,
        NoContentWithFinalBit = 164,
        ResetContent = 37,
        ResetContentWithFinalBit = 165,
        PartialContent = 38,
        PartialContentWithFinalBit = 166,
        MultipleChoices = 48,
        MultipleChoicesWithFinalBit = 176,
        MovedPermanently = 49,
        MovedPermanentlyWithFinalBit = 177,
        MovedTemporarily = 50,
        MovedTemporarilyWithFinalBit = 178,
        SeeOther = 51,
        SeeOtherWithFinalBit = 179,
        NotModified = 52,
        NotModifiedWithFinalBit = 180,
        UseProxy = 53,
        UseProxyWithFinalBit = 181,
        BadRequest = 64,
        BadRequestWithFinalBit = 192,
        Unauthorized = 65,
        UnauthorizedWithFinalBit = 193,
        PaymentRequired = 66,
        PaymentRequiredWithFinalBit = 194,
        Forbidden = 67,
        ForbiddenWithFinalBit = 195,
        NotFound = 68,
        NotFoundWithFinalBit = 196,
        MethodNotAllowed = 69,
        MethodNotAllowedWithFinalBit = 197,
        NotAcceptable = 70,
        NotAcceptableWithFinalBit = 198,
        ProxyAuthenticationRequired = 71,
        ProxyAuthenticationRequiredWithFinalBit = 199,
        RequestTimeOut = 72,
        RequestTimeOutWithFinalBit = 200,
        Conflict = 73,
        ConflictWithFinalBit = 201,
        Gone = 74,
        GoneWithFinalBit = 202,
        LengthRequired = 75,
        LengthRequiredFinalBit = 203,
        PreconditionFailed = 76,
        PreconditionFailedWithFinalBit = 204,
        RequestedEntityTooLarge = 77,
        RequestedEntityTooLargeWithFinalBit = 205,
        RequestURLTooLarge = 78,
        RequestURLTooLargeWithFinalBit = 206,
        UnsupportedMediaType = 79,
        UnsupportedMediaTypeWithFinalBit = 207,
        InternalServerError = 80,
        InternalServerErrorWithFinalBit = 208,
        NotImplemented = 81,
        NotImplementedWithFinalBit = 209,
        BadGateway = 82,
        BadGatewayWithFinalBit = 210,
        ServiceUnavailable = 83,
        ServiceUnavailableWithFinalBit = 211,
        GatewayTimeout = 84,
        GatewayTimeoutWithFinalBit = 212,
        HTTPVersionNotSupported = 85,
        HTTPVersionNotSupportedWithFinalBit = 213,
        DatabaseFull = 96,
        DatabaseFullWithFinalBit = 224,
        DatabaseLocked = 97,
        DatabaseLockedWithFinalBit = 225
    }

    public enum OBEXOpCodeCommandValues : uint
    {
        Reserved = 4,
        Connect = 128,
        Disconnect = 129,
        Put = 2,
        PutWithHighBitSet = 130,
        Get = 3,
        GetWithHighBitSet = 131,
        ReservedWithHighBitSet = 132,
        SetPath = 133,
        Abort = 255,
        ReservedRangeStart = 6,
        ReservedRangeEnd = 15,
        UserDefinedStart = 16,
        UserDefinedEnd = 31
    }

    public enum OBEXConnectFlagValues : uint
    {
        None = 0,
        SupportMultipleItLMPConnections = (1 << 0),
        OBEXConnectFlag1Reserved = (1 << 1),
        OBEXConnectFlag2Reserved = (1 << 2),
        OBEXConnectFlag3Reserved = (1 << 3),
        OBEXConnectFlag4Reserved = (1 << 4),
        OBEXConnectFlag5Reserved = (1 << 5),
        OBEXConnectFlag6Reserved = (1 << 6),
        OBEXConnectFlag7Reserved = (1 << 7)
    }

    public enum OBEXPutFlagValues : uint
    {
        None = (0 << 0),
        GoToParentDirFirst = (1 << 0),
        DontCreateDirectory = (1 << 1),
        OBEXPutFlag2Reserved = (1 << 2),
        OBEXPutFlag3Reserved = (1 << 3),
        OBEXPutFlag4Reserved = (1 << 4),
        OBEXPutFlag5Reserved = (1 << 5),
        OBEXPutFlag6Reserved = (1 << 6),
        OBEXPutFlag7Reserved = (1 << 7)
    }

    public enum OBEXNonceFlagValues : uint
    {
        None = (0 << 0),
        SendUserIDInResponse = (1 << 0),
        AccessModeReadOnly = (1 << 1),
        OBEXNonceFlag2Reserved = (1 << 2),
        OBEXNonceFlag3Reserved = (1 << 3),
        OBEXNonceFlag4Reserved = (1 << 4),
        OBEXNonceFlag5Reserved = (1 << 5),
        OBEXNonceFlag6Reserved = (1 << 6),
        OBEXNonceFlag7Reserved = (1 << 7)
    }

    public enum OBEXRealmValues : uint
    {
        Ascii = 0,
        Iso88591 = 1,
        Iso88592 = 2,
        Iso88593 = 3,
        Iso88594 = 4,
        Iso88595 = 5,
        Iso88596 = 6,
        Iso88597 = 7,
        Iso88598 = 8,
        Iso88599 = 9,
        Unicode = 255
    }

    public enum OBEXOpCodeSessionValues : uint
    {
        CreateSession = 0,
        CloseSession = 1,
        SuspendSession = 2,
        ResumeSession = 3,
        SetTimeout = 4
    }

    public enum OBEXSessionParameterTags : uint
    {
        DeviceAddress = 0,
        Nonce = 1,
        SessionID = 2,
        NextSequenceNumber = 3,
        Timeout = 4,
        SessionOpcode = 5
    }

    public enum OBEXVersions : uint
    {
        OBEXVersion10 = 16
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXConnectCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;

        public ushort maxPacketSize;

        public byte version;

        public byte flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXDisconnectCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXPutCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXGetCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXSetPathCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;

        public byte flags;

        public byte constants;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXAbortCommandResponseData
    {
        public byte serverResponseOpCode;

        public unsafe void* headerDataPtr;

        public nuint headerDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXConnectCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint headerDataLength;

        public ushort maxPacketSize;

        public byte version;

        public byte flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXDisconnectCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint HeaderDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXPutCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint headerDataLength;

        public nuint bodyDataLeftToSend;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXGetCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint HeaderDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXSetPathCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint headerDataLength;

        public byte flags;

        public byte constants;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXAbortCommandData
    {
        public unsafe void* headerDataPtr;

        public nuint headerDataLength;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXErrorData
    {
        public int Error;

        public unsafe void* dataPtr;

        public nuint DataLength;
    }

    public enum OBEXSessionEventTypes : uint
    {
        ConnectCommandResponseReceived = 1329808707,
        DisconnectCommandResponseReceived = 1329808708,
        PutCommandResponseReceived = 1329808720,
        GetCommandResponseReceived = 1329808711,
        SetPathCommandResponseReceived = 1329808723,
        AbortCommandResponseReceived = 1329808705,
        ConnectCommandReceived = 1330857283,
        DisconnectCommandReceived = 1330857284,
        PutCommandReceived = 1330857296,
        GetCommandReceived = 1330857287,
        SetPathCommandReceived = 1330857299,
        AbortCommandReceived = 1330857281,
        Error = 1330070853
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXSessionEvent
    {
        public uint type;

        //public unsafe OBEXSessionRef* session;

        public unsafe void* refCon;

        public byte isEndOfEventData;

        public unsafe void* reserved1;

        public unsafe void* reserved2;

        [StructLayout(LayoutKind.Explicit)]
        public struct U
        {
            [FieldOffset(0)]
            public OBEXConnectCommandResponseData connectCommandResponseData;

            [FieldOffset(0)]
            public OBEXDisconnectCommandResponseData disconnectCommandResponseData;

            [FieldOffset(0)]
            public OBEXPutCommandResponseData putCommandResponseData;

            [FieldOffset(0)]
            public OBEXGetCommandResponseData getCommandResponseData;

            [FieldOffset(0)]
            public OBEXSetPathCommandResponseData setPathCommandResponseData;

            [FieldOffset(0)]
            public OBEXAbortCommandResponseData abortCommandResponseData;

            [FieldOffset(0)]
            public OBEXConnectCommandData connectCommandData;

            [FieldOffset(0)]
            public OBEXDisconnectCommandData disconnectCommandData;

            [FieldOffset(0)]
            public OBEXPutCommandData putCommandData;

            [FieldOffset(0)]
            public OBEXGetCommandData getCommandData;

            [FieldOffset(0)]
            public OBEXSetPathCommandData setPathCommandData;

            [FieldOffset(0)]
            public OBEXAbortCommandData abortCommandData;

            [FieldOffset(0)]
            public OBEXErrorData errorData;
        }


        public U u;
    }

    public enum OBEXTransportEventTypes : uint
    {
        DataReceived = 1147237441,
        Status = 1400136020
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OBEXTransportEvent
    {
        public uint Type;

        public int Status;

        public unsafe void* dataPtr;

        public nuint DataLength;
    }

    public enum FTSFileType : uint
    {
        Folder = 1,
        File = 2
    }

    public enum HandsFreeDeviceFeatures : uint
    {
        None = 0,
        ECAndOrNRFunction = (1 << 0),
        ThreeWayCalling = (1 << 1),
        CLIPresentation = (1 << 2),
        VoiceRecognition = (1 << 3),
        RemoteVolumeControl = (1 << 4),
        EnhancedCallStatus = (1 << 5),
        EnhancedCallControl = (1 << 6),
        CodecNegotiation = (1 << 7)
    }

    public enum HandsFreeAudioGatewayFeatures : uint
    {
        None = 0,
        ThreeWayCalling = (1 << 0),
        ECAndOrNRFunction = (1 << 1),
        VoiceRecognition = (1 << 2),
        InBandRingTone = (1 << 3),
        AttachedNumberToVoiceTag = (1 << 4),
        RejectCallCapability = (1 << 5),
        EnhancedCallStatus = (1 << 6),
        EnhancedCallControl = (1 << 7),
        ExtendedErrorResultCodes = (1 << 8),
        CodecNegotiation = (1 << 9)
    }

    [Native]
    public enum HandsFreeCallHoldModes : ulong
    {
        Mode0 = 1 << 0,
        Mode1 = 1 << 1,
        Mode1idx = 1 << 2,
        HoldMode2 = 1 << 3,
        Mode2idx = 1 << 4,
        Mode3 = 1 << 5,
        Mode4 = 1 << 6
    }

    public enum HandsFreeCodecID : byte
    {
        Cvsd = 1,
        mSBC = 2,
        Aaceld = 128
    }

    [Native]
    public enum SMSMode : ulong
    {
        Pdu,
        Text
    }

    [Native]
    public enum HandsFreeSMSSupport : ulong
    {
        Phase2SMSSupport = 1 << 0,
        Phase2pSMSSupport = 1 << 1,
        ManufactureSpecificSMSSupport = 1 << 2
    }

    [Native]
    public enum HandsFreePDUMessageStatus : ulong
    {
        RecUnread = 0,
        RecRead = 1,
        StoUnsent = 2,
        StoSent = 3,
        All = 4
    }
}