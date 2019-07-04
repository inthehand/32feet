using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

//[assembly: LinkWith("IOBluetooth.framework", LinkTarget.x86_64, ForceLoad =true, Frameworks = "CoreFoundation")]

namespace IOBluetooth
{
    // @interface IOBluetoothObject : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    interface IOBluetoothObject : INSCopying
    {
    }

    // @interface IOBluetoothUserNotification : NSObject
    /// <summary>
    /// Represents a registered notification.
    /// </summary>
    [BaseType(typeof(NSObject))]
    interface UserNotification
    {
        // -(void)unregister;
        /// <summary>
        /// Called to unregister the target notification.
        /// </summary>
        [Export("unregister")]
        void Unregister();
    }

    //// @protocol IOBluetoothDeviceAsyncCallbacks
    //[Protocol, Model]
    //interface IOBluetoothDeviceAsyncCallbacks
    //{
    //    // @required -(void)remoteNameRequestComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
    //    [Abstract]
    //    [Export("remoteNameRequestComplete:status:")]
    //    void RemoteNameRequestComplete(IOBluetoothDevice device, int status);

    //    // @required -(void)connectionComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
    //    [Abstract]
    //    [Export("connectionComplete:status:")]
    //    void ConnectionComplete(IOBluetoothDevice device, int status);

    //    // @required -(void)sdpQueryComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
    //    [Abstract]
    //    [Export("sdpQueryComplete:status:")]
    //    void SdpQueryComplete(IOBluetoothDevice device, int status);
    //}

    // @interface IOBluetoothDevice : IOBluetoothObject <NSCoding, NSSecureCoding>
    /// <summary>
    /// An instance of IOBluetoothDevice represents a single remote Bluetooth device.
    /// </summary>
    [BaseType(typeof(IOBluetoothObject))]
    interface BluetoothDevice : INSCoding, INSSecureCoding
    {
        // +(IOBluetoothUserNotification *)registerForConnectNotifications:(id)observer selector:(SEL)inSelector;
        [Static]
        [Export("registerForConnectNotifications:selector:")]
        UserNotification RegisterForConnectNotifications(NSObject observer, Selector inSelector);

        // -(IOBluetoothUserNotification *)registerForDisconnectNotification:(id)observer selector:(SEL)inSelector;
        [Export("registerForDisconnectNotification:selector:")]
        UserNotification RegisterForDisconnectNotification(NSObject observer, Selector inSelector);

        // +(instancetype)deviceWithAddress:(const BluetoothDeviceAddress *)address;
        [Static]
        [Export("deviceWithAddress:")]
        BluetoothDevice DeviceWithAddress(ref BluetoothDeviceAddress address);

        // +(instancetype)deviceWithAddressString:(NSString *)address;
        /// <summary>
        /// Returns the IOBluetoothDevice object for the given BluetoothDeviceAddress.
        /// </summary>
        /// <returns>The with address string.</returns>
        /// <param name="address">Address.</param>
        [Static]
        [Export("deviceWithAddressString:")]
        BluetoothDevice DeviceWithAddressString(string address);

        // -(IOReturn)openL2CAPChannelSync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm delegate:(id)channelDelegate;
        [Export("openL2CAPChannelSync:withPSM:delegate:")]
        int OpenL2CAPChannelSync(out L2CAPChannel newChannel, ushort psm, NSObject channelDelegate);

        // -(IOReturn)openL2CAPChannelAsync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm delegate:(id)channelDelegate;
        [Export("openL2CAPChannelAsync:withPSM:delegate:")]
        int OpenL2CAPChannelAsync(out L2CAPChannel newChannel, ushort psm, NSObject channelDelegate);

        // -(IOReturn)sendL2CAPEchoRequest:(void *)data length:(UInt16)length;
        [Export("sendL2CAPEchoRequest:length:")]
        int SendL2CAPEchoRequest(NSArray data, ushort length);

        // -(IOReturn)openRFCOMMChannelSync:(IOBluetoothRFCOMMChannel **)rfcommChannel withChannelID:(BluetoothRFCOMMChannelID)channelID delegate:(id)channelDelegate;
        /// <summary>
        /// Opens a new RFCOMM channel to the target device. Returns only once the channel is open or failed to open.
        /// </summary>
        /// <returns>Returns kIOReturnSuccess if the open process was successfully started (or if an existing RFCOMM channel was found). 
        /// The channel must be released when the caller is done with it.</returns>
        /// <param name="rfcommChannel">A pointer to an IOBluetoothRFCOMMChannel object to receive the RFCOMM channel requested to be opened. 
        /// The rfcommChannel pointer will only be set if kIOReturnSuccess is returned.</param>
        /// <param name="channelID">The RFCOMM channel ID for the new channel.</param>
        /// <param name="channelDelegate">the object that will play the role of delegate for the channel. 
        /// A channel delegate is the object the rfcomm uses as target for data and events. 
        /// The developer will implement only the the methods he/she is interested in. 
        /// A list of the possible methods is at the end of the file "IOBluetoothRFCOMMChannel.h" in the definition of the protocol IOBluetoothRFCOMMChannelDelegate.</param>
        [Export("openRFCOMMChannelSync:withChannelID:delegate:")]
        int OpenRFCOMMChannelSync(out RFCOMMChannel rfcommChannel, byte channelID, NSObject channelDelegate);

        // -(IOReturn)openRFCOMMChannelAsync:(IOBluetoothRFCOMMChannel **)rfcommChannel withChannelID:(BluetoothRFCOMMChannelID)channelID delegate:(id)channelDelegate;
        [Export("openRFCOMMChannelAsync:withChannelID:delegate:")]
        int OpenRFCOMMChannelAsync(out RFCOMMChannel rfcommChannel, byte channelID, NSObject channelDelegate);

        // @property (readonly) BluetoothClassOfDevice classOfDevice;
        /// <summary>
        /// Gets the full class of device value for the remote device.
        /// </summary>
        /// <value>The class of device.</value>
        [Export("classOfDevice")]
        uint ClassOfDevice { get; }

        // @property (readonly) BluetoothServiceClassMajor serviceClassMajor;
        /// <summary>
        /// Get the major service class of the device.
        /// </summary>
        /// <value>The service class major.</value>
        [Export("serviceClassMajor")]
        BluetoothServiceClassMajor ServiceClassMajor { get; }

        // @property (readonly) BluetoothDeviceClassMajor deviceClassMajor;
        /// <summary>
        /// Get the major device class of the device.
        /// </summary>
        /// <value>The device class major.</value>
        [Export("deviceClassMajor")]
        BluetoothDeviceClassMajor DeviceClassMajor { get; }

        // @property (readonly) BluetoothDeviceClassMinor deviceClassMinor;
        /// <summary>
        /// Get the minor service class of the device.
        /// </summary>
        /// <value>The device class minor.</value>
        [Export("deviceClassMinor")]
        BluetoothDeviceClassMinor DeviceClassMinor { get; }

        // @property (readonly, copy) NSString * name;
        /// <summary>
        /// Get the human readable name of the remote device.
        /// </summary>
        /// <value>The name.</value>
        [Export("name")]
        string Name { get; }

        // @property (readonly) NSString * nameOrAddress;
        /// <summary>
        /// Get the human readable name of the remote device. 
        /// If the name is not present, it will return a string containing the device's address.
        /// </summary>
        /// <value>The name or address.</value>
        [Export("nameOrAddress")]
        string NameOrAddress { get; }

        // @property (readonly, retain) NSDate * lastNameUpdate;
        /// <summary>
        /// Get the date/time of the last successful remote name request.
        /// </summary>
        /// <value>The last name update.</value>
        [Export("lastNameUpdate", ArgumentSemantic.Retain)]
        NSDate LastNameUpdate { get; }

        // -(const BluetoothDeviceAddress *)getAddress;
        [Internal]
        [Export("getAddress")]
        IntPtr GetAddress();

        // @property (readonly) NSString * addressString;
        /// <summary>
        /// Get a string representation of the Bluetooth device address for the target device. 
        /// The format of the string is the same as returned by IOBluetoothNSStringFromDeviceAddress().
        /// </summary>
        /// <value>The address string.</value>
        [Export("addressString")]
        string AddressString { get; }

        // -(BluetoothPageScanRepetitionMode)getPageScanRepetitionMode;
        [Export("getPageScanRepetitionMode")]
        byte PageScanRepetitionMode { get; }

        // -(BluetoothPageScanPeriodMode)getPageScanPeriodMode;
        [Export("getPageScanPeriodMode")]
        byte PageScanPeriodMode { get; }

        // -(BluetoothPageScanMode)getPageScanMode;
        [Export("getPageScanMode")]
        byte PageScanMode { get; }

        // -(BluetoothClockOffset)getClockOffset;
        [Export("getClockOffset")]
        ushort ClockOffset { get; }

        // -(NSDate *)getLastInquiryUpdate;
        [Export("getLastInquiryUpdate")]
        NSDate LastInquiryUpdate { get; }

        // -(BluetoothHCIRSSIValue)RSSI __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("RSSI")]
        sbyte RSSI { get; }

        // -(BluetoothHCIRSSIValue)rawRSSI __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("rawRSSI")]
        sbyte RawRSSI { get; }

        // -(BOOL)isConnected;
        /// <summary>
        /// Indicates whether a baseband connection to the device exists.
        /// </summary>
        /// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
        [Export("isConnected")]
        bool IsConnected { get; }

        // -(IOReturn)openConnection;
        [Export("openConnection")]
        int OpenConnection();

        // -(IOReturn)openConnection:(id)target;
        [Export("openConnection:")]
        int OpenConnection(NSObject target);

        // -(IOReturn)openConnection:(id)target withPageTimeout:(BluetoothHCIPageTimeout)pageTimeoutValue authenticationRequired:(BOOL)authenticationRequired;
        [Export("openConnection:withPageTimeout:authenticationRequired:")]
        int OpenConnection(NSObject target, ushort pageTimeoutValue, bool authenticationRequired);

        // -(IOReturn)closeConnection;
        [Export("closeConnection")]
        int CloseConnection();

        // -(IOReturn)remoteNameRequest:(id)target;
        [Export("remoteNameRequest:")]
        int RemoteNameRequest(NSObject target);

        // -(IOReturn)remoteNameRequest:(id)target withPageTimeout:(BluetoothHCIPageTimeout)pageTimeoutValue;
        [Export("remoteNameRequest:withPageTimeout:")]
        int RemoteNameRequest(NSObject target, ushort pageTimeoutValue);

        // -(IOReturn)requestAuthentication;
        [Export("requestAuthentication")]
        int RequestAuthentication();

        // @property (readonly, assign) BluetoothConnectionHandle connectionHandle;
        [Export("connectionHandle")]
        ushort ConnectionHandle { get; }

        // -(BOOL)isIncoming;
        /// <summary>
        /// Returns TRUE if the device connection was generated by the remote host.
        /// </summary>
        /// <value><c>true</c> if is incoming; otherwise, <c>false</c>.</value>
        [Export("isIncoming")]
        bool IsIncoming { get; }

        // -(BluetoothLinkType)getLinkType;
        [Export("getLinkType")]
        byte LinkType { get; }

        // -(BluetoothHCIEncryptionMode)getEncryptionMode;
        [Export("getEncryptionMode")]
        BluetoothHCIEncryptionMode EncryptionMode { get; }

        // -(IOReturn)performSDPQuery:(id)target;
        [Export("performSDPQuery:")]
        int PerformSDPQuery(NSObject target);

        // -(IOReturn)performSDPQuery:(id)target uuids:(NSArray *)uuidArray __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("performSDPQuery:uuids:")]
        int PerformSDPQuery(NSObject target, SDPUUID[] uuidArray);

        // @property (readonly, retain) NSArray * services;
        [Export("services", ArgumentSemantic.Retain)]
        SDPServiceRecord[] Services { get; }

        // -(NSDate *)getLastServicesUpdate;
        /// <summary>
        /// Get the date/time of the last SDP query.
        /// </summary>
        /// <value>The last services update.</value>
        [Export("getLastServicesUpdate")]
        NSDate LastServicesUpdate { get; }

        // -(IOBluetoothSDPServiceRecord *)getServiceRecordForUUID:(IOBluetoothSDPUUID *)sdpUUID;
        [Export("getServiceRecordForUUID:")]
        SDPServiceRecord GetServiceRecordForUUID(SDPUUID sdpUUID);

        // +(NSArray *)favoriteDevices;
        /// <summary>
        /// Gets an array of the user's favorite devices.
        /// </summary>
        /// <value>The favorite devices.</value>
        [Static]
        [Export("favoriteDevices")]
        BluetoothDevice[] FavoriteDevices { get; }

        // -(BOOL)isFavorite;
        /// <summary>
        /// Reports whether the target device is a favorite for the user.
        /// </summary>
        /// <value><c>true</c> if is favorite; otherwise, <c>false</c>.</value>
        [Export("isFavorite")]
        bool IsFavorite { get; }

        // -(IOReturn)addToFavorites;
        /// <summary>
        /// Adds the target device to the user's favorite devices list.
        /// </summary>
        /// <returns>The to favorites.</returns>
        [Export("addToFavorites")]
        int AddToFavorites();

        // -(IOReturn)removeFromFavorites;
        /// <summary>
        /// Removes the target device from the user's favorite devices list.
        /// </summary>
        /// <returns>The from favorites.</returns>
        [Export("removeFromFavorites")]
        int RemoveFromFavorites();

        // +(NSArray *)recentDevices:(unsigned long)numDevices;
        /// <summary>
        /// Gets an array of recently used Bluetooth devices.
        /// </summary>
        /// <returns>Returns an array of device objects recently used by the system. 
        /// If no devices have been accessed, nil is returned.</returns>
        /// <param name="numDevices">The number of devices to return.</param>
        [Static]
        [Export("recentDevices:")]
        BluetoothDevice[] GetRecentDevices(nuint numDevices);

        // -(NSDate *)recentAccessDate;
        [Export("recentAccessDate")]
        NSDate RecentAccessDate { get; }

        // +(NSArray *)pairedDevices;
        /// <summary>
        /// Gets an array of all of the paired devices on the system.
        /// </summary>
        /// <value>The paired devices.</value>
        [Static]
        [Export("pairedDevices")]
        BluetoothDevice[] PairedDevices { get; }

        // -(BOOL)isPaired;
        /// <summary>
        /// Returns whether the target device is paired.
        /// </summary>
        /// <value><c>true</c> if is paired; otherwise, <c>false</c>.</value>
        [Export("isPaired")]
        bool IsPaired { get; }

        // -(IOReturn)setSupervisionTimeout:(UInt16)timeout;
        /// <summary>
        /// Sets the connection supervision timeout.
        /// </summary>
        /// <returns>Returns kIOReturnSuccess if it was possible to set the connection supervision timeout.</returns>
        /// <param name="timeout">A client-supplied link supervision timeout value to use to monitor the connection. 
        /// The timeout value should be specified in slots, so you can use the BluetoothGetSlotsFromSeconds macro to get the proper value. 
        /// e.g. BluetoothGetSlotsFromSeconds( 5.0 ) will give yield the proper number of slots (8000) for 5 seconds.</param>
        [Export("setSupervisionTimeout:")]
        int SetSupervisionTimeout(ushort timeout);

        // -(IOReturn)openL2CAPChannelSync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm withConfiguration:(NSDictionary *)channelConfiguration delegate:(id)channelDelegate;
        [Export("openL2CAPChannelSync:withPSM:withConfiguration:delegate:")]
        int OpenL2CAPChannelSync(out L2CAPChannel newChannel, L2CapPsm psm, NSDictionary channelConfiguration, NSObject channelDelegate);

        // -(IOReturn)openL2CAPChannelAsync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm withConfiguration:(NSDictionary *)channelConfiguration delegate:(id)channelDelegate;
        [Export("openL2CAPChannelAsync:withPSM:withConfiguration:delegate:")]
        int OpenL2CAPChannelAsync(out L2CAPChannel newChannel, L2CapPsm psm, NSDictionary channelConfiguration, NSObject channelDelegate);

        // -(id)awakeAfterUsingCoder:(NSCoder *)coder __attribute__((ns_returns_retained)) __attribute__((ns_consumes_self));
        [Export("awakeAfterUsingCoder:")]
        NSObject AwakeAfterUsingCoder(NSCoder coder);
    }

    // @interface IOBluetoothDeviceInquiry : NSObject
    [BaseType(typeof(NSObject))]
    interface IOBluetoothDeviceInquiry
    {
        [Wrap("WeakDelegate")]
        IOBluetoothDeviceInquiryDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)inquiryWithDelegate:(id)delegate;
        [Static]
        [Export("inquiryWithDelegate:")]
        IOBluetoothDeviceInquiry InquiryWithDelegate(IOBluetoothDeviceInquiryDelegate @delegate);

        // -(instancetype)initWithDelegate:(id)delegate;
        [Export("initWithDelegate:")]
        IntPtr Constructor(IOBluetoothDeviceInquiryDelegate @delegate);

        // -(IOReturn)start;
        [Export("start")]
        int Start();

        // -(IOReturn)stop;
        [Export("stop")]
        int Stop();

        // @property (assign) uint8_t inquiryLength;
        [Export("inquiryLength")]
        byte InquiryLength { get; set; }

        // @property (assign) IOBluetoothDeviceSearchTypes searchType;
        [Export("searchType")]
        uint SearchType { get; set; }

        // @property (assign) BOOL updateNewDeviceNames;
        [Export("updateNewDeviceNames")]
        bool UpdateNewDeviceNames { get; set; }

        // -(NSArray *)foundDevices;
        [Export("foundDevices")]
        BluetoothDevice[] FoundDevices { get; }

        // -(void)clearFoundDevices;
        [Export("clearFoundDevices")]
        void ClearFoundDevices();

        // -(void)setSearchCriteria:(BluetoothServiceClassMajor)inServiceClassMajor majorDeviceClass:(BluetoothDeviceClassMajor)inMajorDeviceClass minorDeviceClass:(BluetoothDeviceClassMinor)inMinorDeviceClass;
        [Export("setSearchCriteria:majorDeviceClass:minorDeviceClass:")]
        void SetSearchCriteria(uint inServiceClassMajor, uint inMajorDeviceClass, uint inMinorDeviceClass);
    }

    // @protocol IOBluetoothDeviceInquiryDelegate
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface IOBluetoothDeviceInquiryDelegate
    {
        // @optional -(void)deviceInquiryStarted:(IOBluetoothDeviceInquiry *)sender;
        [Export("deviceInquiryStarted:")]
        void DeviceInquiryStarted(IOBluetoothDeviceInquiry sender);

        // @optional -(void)deviceInquiryDeviceFound:(IOBluetoothDeviceInquiry *)sender device:(IOBluetoothDevice *)device;
        [Export("deviceInquiryDeviceFound:device:")]
        void DeviceInquiryDeviceFound(IOBluetoothDeviceInquiry sender, BluetoothDevice device);

        // @optional -(void)deviceInquiryUpdatingDeviceNamesStarted:(IOBluetoothDeviceInquiry *)sender devicesRemaining:(uint32_t)devicesRemaining;
        [Export("deviceInquiryUpdatingDeviceNamesStarted:devicesRemaining:")]
        void DeviceInquiryUpdatingDeviceNamesStarted(IOBluetoothDeviceInquiry sender, uint devicesRemaining);

        // @optional -(void)deviceInquiryDeviceNameUpdated:(IOBluetoothDeviceInquiry *)sender device:(IOBluetoothDevice *)device devicesRemaining:(uint32_t)devicesRemaining;
        [Export("deviceInquiryDeviceNameUpdated:device:devicesRemaining:")]
        void DeviceInquiryDeviceNameUpdated(IOBluetoothDeviceInquiry sender, BluetoothDevice device, uint devicesRemaining);

        // @optional -(void)deviceInquiryComplete:(IOBluetoothDeviceInquiry *)sender error:(IOReturn)error aborted:(BOOL)aborted;
        [Export("deviceInquiryComplete:error:aborted:")]
        void DeviceInquiryComplete(IOBluetoothDeviceInquiry sender, int error, bool aborted);
    }

    // @interface IOBluetoothDevicePair : NSObject
    [BaseType(typeof(NSObject))]
    interface IOBluetoothDevicePair
    {
        [Wrap("WeakDelegate")]
        IOBluetoothDevicePairDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)pairWithDevice:(IOBluetoothDevice *)device;
        [Static]
        [Export("pairWithDevice:")]
        IOBluetoothDevicePair PairWithDevice(BluetoothDevice device);

        // -(IOReturn)start;
        [Export("start")]
        int Start();

        // -(void)stop;
        [Export("stop")]
        void Stop();

        // -(IOBluetoothDevice *)device;
        // -(void)setDevice:(IOBluetoothDevice *)inDevice;
        [Export("device")]
        BluetoothDevice Device { get; set; }

        // -(void)replyPINCode:(ByteCount)PINCodeSize PINCode:(BluetoothPINCode *)PINCode;
        [Export("replyPINCode:PINCode:")]
        void ReplyPINCode(nuint PINCodeSize, IntPtr PINCode);

        // -(void)replyUserConfirmation:(BOOL)reply;
        [Export("replyUserConfirmation:")]
        void ReplyUserConfirmation(bool reply);
    }

    // @protocol IOBluetoothDevicePairDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface IOBluetoothDevicePairDelegate
    {
        // @optional -(void)devicePairingStarted:(id)sender;
        [Export("devicePairingStarted:")]
        void DevicePairingStarted(NSObject sender);

        // @optional -(void)devicePairingConnecting:(id)sender;
        [Export("devicePairingConnecting:")]
        void DevicePairingConnecting(NSObject sender);

        // @optional -(void)devicePairingPINCodeRequest:(id)sender;
        [Export("devicePairingPINCodeRequest:")]
        void DevicePairingPINCodeRequest(NSObject sender);

        // @optional -(void)devicePairingUserConfirmationRequest:(id)sender numericValue:(BluetoothNumericValue)numericValue;
        [Export("devicePairingUserConfirmationRequest:numericValue:")]
        void DevicePairingUserConfirmationRequest(NSObject sender, uint numericValue);

        // @optional -(void)devicePairingUserPasskeyNotification:(id)sender passkey:(BluetoothPasskey)passkey;
        [Export("devicePairingUserPasskeyNotification:passkey:")]
        void DevicePairingUserPasskeyNotification(NSObject sender, uint passkey);

        // @optional -(void)devicePairingFinished:(id)sender error:(IOReturn)error;
        [Export("devicePairingFinished:error:")]
        void DevicePairingFinished(NSObject sender, int error);

        // @optional -(void)deviceSimplePairingComplete:(id)sender status:(BluetoothHCIEventStatus)status;
        [Export("deviceSimplePairingComplete:status:")]
        void DeviceSimplePairingComplete(NSObject sender, byte status);
    }

    // @interface IOBluetoothHostController : NSObject
    [BaseType(typeof(NSObject))]
    interface IOBluetoothHostController
    {
        [Wrap("WeakDelegate")]
        IOBluetoothHostControllerDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)defaultController;
        [Static]
        [Export("defaultController")]
        IOBluetoothHostController DefaultController { get; }

        // @property (readonly) BluetoothHCIPowerState powerState;
        [Export("powerState")]
        uint PowerState { get; }

        // -(BluetoothClassOfDevice)classOfDevice;
        [Export("classOfDevice")]
        uint ClassOfDevice { get; }

        // -(IOReturn)setClassOfDevice:(BluetoothClassOfDevice)classOfDevice forTimeInterval:(NSTimeInterval)seconds;
        [Export("setClassOfDevice:forTimeInterval:")]
        int SetClassOfDevice(uint classOfDevice, double seconds);

        // -(NSString *)addressAsString;
        [Export("addressAsString")]
        string AddressAsString { get; }

        // -(NSString *)nameAsString;
        [Export("nameAsString")]
        string NameAsString { get; }
    }

    // @interface IOBluetoothHostControllerDelegate (NSObject)
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface IOBluetoothHostControllerDelegate
    {
        //// -(void)readRSSIForDeviceComplete:(id)controller device:(IOBluetoothDevice *)device info:(BluetoothHCIRSSIInfo *)info error:(IOReturn)error;
        //[Export ("readRSSIForDeviceComplete:device:info:error:")]
        //unsafe void ReadRSSIForDeviceComplete (NSObject controller, IOBluetoothDevice device, BluetoothHCIRSSIInfo* info, int error);

        //// -(void)readLinkQualityForDeviceComplete:(id)controller device:(IOBluetoothDevice *)device info:(BluetoothHCILinkQualityInfo *)info error:(IOReturn)error;
        //[Export ("readLinkQualityForDeviceComplete:device:info:error:")]
        //unsafe void ReadLinkQualityForDeviceComplete (NSObject controller, IOBluetoothDevice device, BluetoothHCILinkQualityInfo* info, int error);
    }

    [Static]
    partial interface Constants
    {
    	// extern NSString *const IOBluetoothHostControllerPoweredOnNotification;
        [Field ("IOBluetoothHostControllerPoweredOnNotification", LibraryName ="__Internal")]
    	NSString IOBluetoothHostControllerPoweredOnNotification { get; }

    	// extern NSString *const IOBluetoothHostControllerPoweredOffNotification;
        [Field ("IOBluetoothHostControllerPoweredOffNotification", LibraryName = "__Internal")]
    	NSString IOBluetoothHostControllerPoweredOffNotification { get; }
    }

    // @interface IOBluetoothL2CAPChannel : IOBluetoothObject <NSPortDelegate>
    [BaseType(typeof(IOBluetoothObject))]
    interface L2CAPChannel : INSPortDelegate
    {
        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector);

        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector withPSM:(BluetoothL2CAPPSM)psm direction:(IOBluetoothUserNotificationChannelDirection)inDirection;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:withPSM:direction:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector, ushort psm, uint inDirection);

        // +(instancetype)withObjectID:(IOBluetoothObjectID)objectID;
        [Static]
        [Export("withObjectID:")]
        L2CAPChannel WithObjectID(nuint objectID);

        // -(IOReturn)closeChannel;
        [Export("closeChannel")]
        int CloseChannel();

        // @property (readonly) BluetoothL2CAPMTU outgoingMTU;
        [Export("outgoingMTU")]
        ushort OutgoingMTU { get; }

        // @property (readonly) BluetoothL2CAPMTU incomingMTU;
        [Export("incomingMTU")]
        ushort IncomingMTU { get; }

        // -(IOReturn)requestRemoteMTU:(BluetoothL2CAPMTU)remoteMTU;
        [Export("requestRemoteMTU:")]
        int RequestRemoteMTU(ushort remoteMTU);

        // -(IOReturn)writeAsync:(void *)data length:(UInt16)length refcon:(void *)refcon;
        [Export("writeAsync:length:refcon:")]
        int WriteAsync(IntPtr data, ushort length, IntPtr refcon);

        // -(IOReturn)writeSync:(void *)data length:(UInt16)length;
        [Export("writeSync:length:")]
        int WriteSync(IntPtr data, ushort length);

        // -(IOReturn)setDelegate:(id)channelDelegate;
        [Export("setDelegate:")]
        int SetDelegate(L2CAPChannelDelegate channelDelegate);

        // -(IOReturn)setDelegate:(id)channelDelegate withConfiguration:(NSDictionary *)channelConfiguration;
        [Export("setDelegate:withConfiguration:")]
        int SetDelegate(L2CAPChannelDelegate channelDelegate, NSDictionary channelConfiguration);

        // -(id)delegate;
        [Export("delegate")]
        L2CAPChannelDelegate Delegate { get; }

        // @property (readonly, retain) IOBluetoothDevice * device;
        [Export("device", ArgumentSemantic.Retain)]
        BluetoothDevice Device { get; }

        // @property (readonly, assign) IOBluetoothObjectID objectID;
        [Export("objectID")]
        nuint ObjectID { get; }

        // @property (readonly, assign) BluetoothL2CAPPSM PSM;
        [Export("PSM")]
        L2CapPsm Psm { get; }

        // @property (readonly, assign) BluetoothL2CAPChannelID localChannelID;
        [Export("localChannelID")]
        ushort LocalChannelID { get; }

        // @property (readonly, assign) BluetoothL2CAPChannelID remoteChannelID;
        [Export("remoteChannelID")]
        ushort RemoteChannelID { get; }

        // -(BOOL)isIncoming;
        [Export("isIncoming")]
        bool IsIncoming { get; }

        // -(IOBluetoothUserNotification *)registerForChannelCloseNotification:(id)observer selector:(SEL)inSelector;
        [Export("registerForChannelCloseNotification:selector:")]
        UserNotification RegisterForChannelCloseNotification(NSObject observer, Selector inSelector);
    }

    // @protocol IOBluetoothL2CAPChannelDelegate
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface L2CAPChannelDelegate
    {
        // @optional -(void)l2capChannelData:(IOBluetoothL2CAPChannel *)l2capChannel data:(void *)dataPointer length:(size_t)dataLength;
        [Export("l2capChannelData:data:length:")]
        void L2CAPChannelData(L2CAPChannel l2capChannel, IntPtr dataPointer, nuint dataLength);

        // @optional -(void)l2capChannelOpenComplete:(IOBluetoothL2CAPChannel *)l2capChannel status:(IOReturn)error;
        [Export("l2capChannelOpenComplete:status:")]
        void L2CAPChannelOpenComplete(L2CAPChannel l2capChannel, int error);

        // @optional -(void)l2capChannelClosed:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelClosed:")]
        void L2CAPChannelClosed(L2CAPChannel l2capChannel);

        // @optional -(void)l2capChannelReconfigured:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelReconfigured:")]
        void L2CAPChannelReconfigured(L2CAPChannel l2capChannel);

        // @optional -(void)l2capChannelWriteComplete:(IOBluetoothL2CAPChannel *)l2capChannel refcon:(void *)refcon status:(IOReturn)error;
        [Export("l2capChannelWriteComplete:refcon:status:")]
        void L2CAPChannelWriteComplete(L2CAPChannel l2capChannel, IntPtr refcon, int error);

        // @optional -(void)l2capChannelQueueSpaceAvailable:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelQueueSpaceAvailable:")]
        void L2CAPChannelQueueSpaceAvailable(L2CAPChannel l2capChannel);
    }


    partial interface Constants
    {
    	// extern NSString *const IOBluetoothL2CAPChannelPublishedNotification;
        [Field ("IOBluetoothL2CAPChannelPublishedNotification", LibraryName = "__Internal")]
    	NSString IOBluetoothL2CAPChannelPublishedNotification { get; }

    	// extern NSString *const IOBluetoothL2CAPChannelTerminatedNotification;
        [Field ("IOBluetoothL2CAPChannelTerminatedNotification", LibraryName = "__Internal")]
    	NSString IOBluetoothL2CAPChannelTerminatedNotification { get; }
    }

    // @interface IOBluetoothRFCOMMChannel : IOBluetoothObject <NSPortDelegate>
    [BaseType(typeof(IOBluetoothObject))]
    interface RFCOMMChannel : INSPortDelegate
    {
        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector);

        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector withChannelID:(BluetoothRFCOMMChannelID)channelID direction:(IOBluetoothUserNotificationChannelDirection)inDirection;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:withChannelID:direction:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector, byte channelID, uint inDirection);

        // +(instancetype)withRFCOMMChannelRef:(IOBluetoothRFCOMMChannelRef)rfcommChannelRef;
        //[Static]
        //[Export ("withRFCOMMChannelRef:")]
        //unsafe IOBluetoothRFCOMMChannel WithRFCOMMChannelRef (IOBluetoothRFCOMMChannelRef* rfcommChannelRef);

        // +(instancetype)withObjectID:(IOBluetoothObjectID)objectID;
        [Static]
        [Export("withObjectID:")]
        RFCOMMChannel WithObjectID(nuint objectID);

        // -(IOBluetoothRFCOMMChannelRef)getRFCOMMChannelRef;
        //[Export ("getRFCOMMChannelRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothRFCOMMChannelRef* RFCOMMChannelRef { get; }

        // -(IOReturn)closeChannel;
        [Export("closeChannel")]
        int CloseChannel();

        // -(BOOL)isOpen;
        [Export("isOpen")]
        bool IsOpen { get; }

        // -(BluetoothRFCOMMMTU)getMTU;
        [Export("getMTU")]
        ushort MTU { get; }

        // -(BOOL)isTransmissionPaused;
        [Export("isTransmissionPaused")]
        bool IsTransmissionPaused { get; }

        // -(IOReturn)writeAsync:(void *)data length:(UInt16)length refcon:(void *)refcon;
        [Export("writeAsync:length:refcon:")]
        int WriteAsync(IntPtr data, ushort length, IntPtr refcon);

        // -(IOReturn)writeSync:(void *)data length:(UInt16)length;
        [Export("writeSync:length:")]
        int WriteSync(IntPtr data, ushort length);

        // -(IOReturn)setSerialParameters:(UInt32)speed dataBits:(UInt8)nBits parity:(BluetoothRFCOMMParityType)parity stopBits:(UInt8)bitStop;
        [Export("setSerialParameters:dataBits:parity:stopBits:")]
        int SetSerialParameters(uint speed, byte nBits, BluetoothRFCOMMParityType parity, byte bitStop);

        // -(IOReturn)sendRemoteLineStatus:(BluetoothRFCOMMLineStatus)lineStatus;
        [Export("sendRemoteLineStatus:")]
        int SendRemoteLineStatus(uint lineStatus);

        // -(IOReturn)setDelegate:(id)delegate;
        [Export("setDelegate:")]
        int SetDelegate(NSObject @delegate);

        // -(id)delegate;
        [Export("delegate")]
        RFCOMMChannelDelegate Delegate { get; }

        // -(BluetoothRFCOMMChannelID)getChannelID;
        [Export("getChannelID")]
        byte ChannelID { get; }

        // -(BOOL)isIncoming;
        [Export("isIncoming")]
        bool IsIncoming { get; }

        // -(IOBluetoothDevice *)getDevice;
        [Export("getDevice")]
        BluetoothDevice Device { get; }

        // -(IOBluetoothObjectID)getObjectID;
        [Export("getObjectID")]
        nuint ObjectID { get; }

        // -(IOBluetoothUserNotification *)registerForChannelCloseNotification:(id)observer selector:(SEL)inSelector;
        [Export("registerForChannelCloseNotification:selector:")]
        UserNotification RegisterForChannelCloseNotification(NSObject observer, Selector inSelector);
    }

    // @protocol IOBluetoothRFCOMMChannelDelegate
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface RFCOMMChannelDelegate
    {
        // @optional -(void)rfcommChannelData:(IOBluetoothRFCOMMChannel *)rfcommChannel data:(void *)dataPointer length:(size_t)dataLength;
        [Export("rfcommChannelData:data:length:")]
        void RfcommChannelData(RFCOMMChannel rfcommChannel, IntPtr dataPointer, nuint dataLength);

        // @optional -(void)rfcommChannelOpenComplete:(IOBluetoothRFCOMMChannel *)rfcommChannel status:(IOReturn)error;
        [Export("rfcommChannelOpenComplete:status:")]
        void RfcommChannelOpenComplete(RFCOMMChannel rfcommChannel, int error);

        // @optional -(void)rfcommChannelClosed:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelClosed:")]
        void RfcommChannelClosed(RFCOMMChannel rfcommChannel);

        // @optional -(void)rfcommChannelControlSignalsChanged:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelControlSignalsChanged:")]
        void RfcommChannelControlSignalsChanged(RFCOMMChannel rfcommChannel);

        // @optional -(void)rfcommChannelFlowControlChanged:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelFlowControlChanged:")]
        void RfcommChannelFlowControlChanged(RFCOMMChannel rfcommChannel);

        // @optional -(void)rfcommChannelWriteComplete:(IOBluetoothRFCOMMChannel *)rfcommChannel refcon:(void *)refcon status:(IOReturn)error;
        [Export("rfcommChannelWriteComplete:refcon:status:")]
        void RfcommChannelWriteComplete(RFCOMMChannel rfcommChannel, IntPtr refcon, int error);

        // @optional -(void)rfcommChannelQueueSpaceAvailable:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelQueueSpaceAvailable:")]
        void RfcommChannelQueueSpaceAvailable(RFCOMMChannel rfcommChannel);
    }

    // @interface IOBluetoothSDPDataElement : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject), Name ="IOBluetoothSDPDataElement")]
    interface SDPDataElement : INSCoding, INSSecureCoding
    {
        // +(instancetype)withElementValue:(NSObject *)element;
        [Static]
        [Export("withElementValue:")]
        SDPDataElement WithElementValue(NSObject element);

        // +(instancetype)withType:(BluetoothSDPDataElementTypeDescriptor)type sizeDescriptor:(BluetoothSDPDataElementSizeDescriptor)newSizeDescriptor size:(uint32_t)newSize value:(NSObject *)newValue;
        [Static]
        [Export("withType:sizeDescriptor:size:value:")]
        SDPDataElement WithType(BluetoothSDPDataElementType type, byte newSizeDescriptor, uint newSize, NSObject newValue);

        // +(instancetype)withSDPDataElementRef:(IOBluetoothSDPDataElementRef)sdpDataElementRef;
        //[Static]
        //[Export ("withSDPDataElementRef:")]
        //unsafe IOBluetoothSDPDataElement WithSDPDataElementRef (IOBluetoothSDPDataElementRef* sdpDataElementRef);

        // -(instancetype)initWithElementValue:(NSObject *)element;
        [Export("initWithElementValue:")]
        IntPtr Constructor(NSObject element);

        // -(instancetype)initWithType:(BluetoothSDPDataElementTypeDescriptor)newType sizeDescriptor:(BluetoothSDPDataElementSizeDescriptor)newSizeDescriptor size:(uint32_t)newSize value:(NSObject *)newValue;
        [Export("initWithType:sizeDescriptor:size:value:")]
        IntPtr Constructor(BluetoothSDPDataElementType newType, byte newSizeDescriptor, uint newSize, NSObject newValue);

        // -(IOBluetoothSDPDataElementRef)getSDPDataElementRef;
        //[Export ("getSDPDataElementRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothSDPDataElementRef* SDPDataElementRef { get; }

        // -(BluetoothSDPDataElementTypeDescriptor)getTypeDescriptor;
        [Export("getTypeDescriptor")]
        BluetoothSDPDataElementType TypeDescriptor { get; }

        // -(BluetoothSDPDataElementSizeDescriptor)getSizeDescriptor;
        [Export("getSizeDescriptor")]
        byte SizeDescriptor { get; }

        // -(uint32_t)getSize;
        [Export("getSize")]
        uint Size { get; }

        // -(NSNumber *)getNumberValue;
        [Export("getNumberValue")]
        NSNumber NumberValue { get; }

        // -(NSData *)getDataValue;
        [Export("getDataValue")]
        NSData DataValue { get; }

        // -(NSString *)getStringValue;
        [Export("getStringValue")]
        string StringValue { get; }

        // -(NSArray *)getArrayValue;
        [Export("getArrayValue")]
        NSObject[] ArrayValue { get; }

        // -(IOBluetoothSDPUUID *)getUUIDValue;
        [Export("getUUIDValue")]
        SDPUUID UUIDValue { get; }

        // -(NSObject *)getValue;
        [Export("getValue")]
        NSObject Value { get; }

        // -(BOOL)containsDataElement:(IOBluetoothSDPDataElement *)dataElement;
        [Export("containsDataElement:")]
        bool ContainsDataElement(SDPDataElement dataElement);

        // -(BOOL)containsValue:(NSObject *)cmpValue;
        [Export("containsValue:")]
        bool ContainsValue(NSObject cmpValue);
    }

    // @interface IOBluetoothSDPServiceAttribute : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject))]
    interface SDPServiceAttribute : INSCoding, INSSecureCoding
    {
        // +(instancetype)withID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElementValue:(NSObject *)attributeElementValue;
        [Static]
        [Export("withID:attributeElementValue:")]
        SDPServiceAttribute WithID(ushort newAttributeID, NSObject attributeElementValue);

        // +(instancetype)withID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElement:(IOBluetoothSDPDataElement *)attributeElement;
        [Static]
        [Export("withID:attributeElement:")]
        SDPServiceAttribute WithID(ushort newAttributeID, SDPDataElement attributeElement);

        // -(instancetype)initWithID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElementValue:(NSObject *)attributeElementValue;
        [Export("initWithID:attributeElementValue:")]
        IntPtr Constructor(ushort newAttributeID, NSObject attributeElementValue);

        // -(instancetype)initWithID:(BluetoothSDPServiceAttributeID)newAttributeID attributeElement:(IOBluetoothSDPDataElement *)attributeElement;
        [Export("initWithID:attributeElement:")]
        IntPtr Constructor(ushort newAttributeID, SDPDataElement attributeElement);

        // -(BluetoothSDPServiceAttributeID)getAttributeID;
        [Export("getAttributeID")]
        ushort AttributeID { get; }

        // -(IOBluetoothSDPDataElement *)getDataElement;
        [Export("getDataElement")]
        SDPDataElement DataElement { get; }

        // -(IOBluetoothSDPDataElement *)getIDDataElement;
        [Export("getIDDataElement")]
        SDPDataElement IDDataElement { get; }
    }

    // @interface IOBluetoothSDPServiceRecord : NSObject <NSCoding, NSSecureCoding>
    [BaseType(typeof(NSObject))]
    interface SDPServiceRecord : INSCoding, INSSecureCoding
    {
        // +(instancetype)publishedServiceRecordWithDictionary:(NSDictionary *)serviceDict;
        [Static]
        [Export("publishedServiceRecordWithDictionary:")]
        SDPServiceRecord PublishedServiceRecordWithDictionary(NSDictionary serviceDict);

        // -(IOReturn)removeServiceRecord;
        [Export("removeServiceRecord")]
        int RemoveServiceRecord();

        // +(instancetype)withServiceDictionary:(NSDictionary *)serviceDict device:(IOBluetoothDevice *)device;
        [Static]
        [Export("withServiceDictionary:device:")]
        SDPServiceRecord WithServiceDictionary(NSDictionary serviceDict, BluetoothDevice device);

        // -(instancetype)initWithServiceDictionary:(NSDictionary *)serviceDict device:(IOBluetoothDevice *)device;
        [Export("initWithServiceDictionary:device:")]
        IntPtr Constructor(NSDictionary serviceDict, BluetoothDevice device);

        // +(instancetype)withSDPServiceRecordRef:(IOBluetoothSDPServiceRecordRef)sdpServiceRecordRef;
        //[Static]
        //[Export ("withSDPServiceRecordRef:")]
        //unsafe IOBluetoothSDPServiceRecord WithSDPServiceRecordRef (IOBluetoothSDPServiceRecordRef* sdpServiceRecordRef);

        // -(IOBluetoothSDPServiceRecordRef)getSDPServiceRecordRef;
        //[Export ("getSDPServiceRecordRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothSDPServiceRecordRef* SDPServiceRecordRef { get; }

        // @property (readonly, retain) IOBluetoothDevice * device;
        [Export("device", ArgumentSemantic.Retain)]
        BluetoothDevice Device { get; }

        // @property (readonly, copy) NSDictionary * attributes;
        [Export("attributes", ArgumentSemantic.Copy)]
        NSDictionary Attributes { get; }

        // -(IOBluetoothSDPDataElement *)getAttributeDataElement:(BluetoothSDPServiceAttributeID)attributeID;
        [Export("getAttributeDataElement:")]
        SDPDataElement GetAttributeDataElement(ushort attributeID);

        // -(NSString *)getServiceName;
        [Export("getServiceName")]
        string ServiceName { get; }

        // -(IOReturn)getRFCOMMChannelID:(BluetoothRFCOMMChannelID *)rfcommChannelID;
        [Export("getRFCOMMChannelID:")]
        int GetRFCOMMChannelID(out byte rfcommChannelID);

        // -(IOReturn)getL2CAPPSM:(BluetoothL2CAPPSM *)outPSM;
        [Export("getL2CAPPSM:")]
        int GetL2CAPPSM(out ushort outPSM);

        // -(IOReturn)getServiceRecordHandle:(BluetoothSDPServiceRecordHandle *)outServiceRecordHandle;
        [Export("getServiceRecordHandle:")]
        int GetServiceRecordHandle(IntPtr outServiceRecordHandle);

        // -(BOOL)matchesUUID16:(BluetoothSDPUUID16)uuid16;
        [Export("matchesUUID16:")]
        bool MatchesUUID16(ushort uuid16);

        // -(BOOL)matchesUUIDArray:(NSArray *)uuidArray;
        [Export("matchesUUIDArray:")]
        bool MatchesUUIDArray(SDPUUID[] uuidArray);

        // -(BOOL)matchesSearchArray:(NSArray *)searchArray;
        [Export("matchesSearchArray:")]
        bool MatchesSearchArray(SDPUUID[] searchArray);

        // -(BOOL)hasServiceFromArray:(NSArray *)array;
        [Export("hasServiceFromArray:")]
        bool HasServiceFromArray(SDPUUID[] array);

        // @property (readonly, copy) NSArray * sortedAttributes;
        [Export("sortedAttributes", ArgumentSemantic.Copy)]
        SDPServiceAttribute[] SortedAttributes { get; }
    }

    // @interface IOBluetoothSDPUUID : NSData
    [BaseType(typeof(NSData))]
    interface SDPUUID
    {
        // +(instancetype)uuidWithBytes:(const void *)bytes length:(unsigned int)length;
        [Static]
        [Export("uuidWithBytes:length:")]
        SDPUUID UuidWithBytes(IntPtr bytes, uint length);

        // +(instancetype)uuidWithData:(NSData *)data;
        [Static]
        [Export("uuidWithData:")]
        SDPUUID UuidWithData(NSData data);

        // +(instancetype)uuid16:(BluetoothSDPUUID16)uuid16;
        [Static]
        [Export("uuid16:")]
        SDPUUID Uuid16(ushort uuid16);

        // +(instancetype)uuid32:(BluetoothSDPUUID32)uuid32;
        [Static]
        [Export("uuid32:")]
        SDPUUID Uuid32(uint uuid32);

        // -(instancetype)initWithUUID16:(BluetoothSDPUUID16)uuid16;
        [Export("initWithUUID16:")]
        IntPtr Constructor(ushort uuid16);

        // -(instancetype)initWithUUID32:(BluetoothSDPUUID32)uuid32;
        [Export("initWithUUID32:")]
        IntPtr Constructor(uint uuid32);

        // -(instancetype)getUUIDWithLength:(unsigned int)newLength;
        [Export("getUUIDWithLength:")]
        SDPUUID GetUUIDWithLength(uint newLength);

        // -(BOOL)isEqualToUUID:(IOBluetoothSDPUUID *)otherUUID;
        [Export("isEqualToUUID:")]
        bool IsEqualToUUID(SDPUUID otherUUID);

        // -(Class)classForCoder;
        [Export("classForCoder")]
        Class ClassForCoder { get; }

        // -(Class)classForArchiver;
        [Export("classForArchiver")]
        Class ClassForArchiver { get; }

        // -(Class)classForPortCoder;
        [Export("classForPortCoder")]
        Class ClassForPortCoder { get; }
    }

    /*[Static]
    [Verify (ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern CFStringRef kOBEXHeaderIDKeyName;
        [Field ("kOBEXHeaderIDKeyName")]
        unsafe CFStringRef* kOBEXHeaderIDKeyName { get; }

        // extern CFStringRef kOBEXHeaderIDKeyType;
        [Field ("kOBEXHeaderIDKeyType")]
        unsafe CFStringRef* kOBEXHeaderIDKeyType { get; }

        // extern CFStringRef kOBEXHeaderIDKeyDescription;
        [Field ("kOBEXHeaderIDKeyDescription")]
        unsafe CFStringRef* kOBEXHeaderIDKeyDescription { get; }

        // extern CFStringRef kOBEXHeaderIDKeyTimeISO;
        [Field ("kOBEXHeaderIDKeyTimeISO")]
        unsafe CFStringRef* kOBEXHeaderIDKeyTimeISO { get; }

        // extern CFStringRef kOBEXHeaderIDKeyTime4Byte;
        [Field ("kOBEXHeaderIDKeyTime4Byte")]
        unsafe CFStringRef* kOBEXHeaderIDKeyTime4Byte { get; }

        // extern CFStringRef kOBEXHeaderIDKeyTarget;
        [Field ("kOBEXHeaderIDKeyTarget")]
        unsafe CFStringRef* kOBEXHeaderIDKeyTarget { get; }

        // extern CFStringRef kOBEXHeaderIDKeyHTTP;
        [Field ("kOBEXHeaderIDKeyHTTP")]
        unsafe CFStringRef* kOBEXHeaderIDKeyHTTP { get; }

        // extern CFStringRef kOBEXHeaderIDKeyBody;
        [Field ("kOBEXHeaderIDKeyBody")]
        unsafe CFStringRef* kOBEXHeaderIDKeyBody { get; }

        // extern CFStringRef kOBEXHeaderIDKeyEndOfBody;
        [Field ("kOBEXHeaderIDKeyEndOfBody")]
        unsafe CFStringRef* kOBEXHeaderIDKeyEndOfBody { get; }

        // extern CFStringRef kOBEXHeaderIDKeyWho;
        [Field ("kOBEXHeaderIDKeyWho")]
        unsafe CFStringRef* kOBEXHeaderIDKeyWho { get; }

        // extern CFStringRef kOBEXHeaderIDKeyAppParameters;
        [Field ("kOBEXHeaderIDKeyAppParameters")]
        unsafe CFStringRef* kOBEXHeaderIDKeyAppParameters { get; }

        // extern CFStringRef kOBEXHeaderIDKeyAuthorizationChallenge;
        [Field ("kOBEXHeaderIDKeyAuthorizationChallenge")]
        unsafe CFStringRef* kOBEXHeaderIDKeyAuthorizationChallenge { get; }

        // extern CFStringRef kOBEXHeaderIDKeyAuthorizationResponse;
        [Field ("kOBEXHeaderIDKeyAuthorizationResponse")]
        unsafe CFStringRef* kOBEXHeaderIDKeyAuthorizationResponse { get; }

        // extern CFStringRef kOBEXHeaderIDKeyObjectClass;
        [Field ("kOBEXHeaderIDKeyObjectClass")]
        unsafe CFStringRef* kOBEXHeaderIDKeyObjectClass { get; }

        // extern CFStringRef kOBEXHeaderIDKeyCount;
        [Field ("kOBEXHeaderIDKeyCount")]
        unsafe CFStringRef* kOBEXHeaderIDKeyCount { get; }

        // extern CFStringRef kOBEXHeaderIDKeyLength;
        [Field ("kOBEXHeaderIDKeyLength")]
        unsafe CFStringRef* kOBEXHeaderIDKeyLength { get; }

        // extern CFStringRef kOBEXHeaderIDKeyConnectionID;
        [Field ("kOBEXHeaderIDKeyConnectionID")]
        unsafe CFStringRef* kOBEXHeaderIDKeyConnectionID { get; }

        // extern CFStringRef kOBEXHeaderIDKeyByteSequence;
        [Field ("kOBEXHeaderIDKeyByteSequence")]
        unsafe CFStringRef* kOBEXHeaderIDKeyByteSequence { get; }

        // extern CFStringRef kOBEXHeaderIDKeyUnknownUnicodeText;
        [Field ("kOBEXHeaderIDKeyUnknownUnicodeText")]
        unsafe CFStringRef* kOBEXHeaderIDKeyUnknownUnicodeText { get; }

        // extern CFStringRef kOBEXHeaderIDKeyUnknownByteSequence;
        [Field ("kOBEXHeaderIDKeyUnknownByteSequence")]
        unsafe CFStringRef* kOBEXHeaderIDKeyUnknownByteSequence { get; }

        // extern CFStringRef kOBEXHeaderIDKeyUnknown1ByteQuantity;
        [Field ("kOBEXHeaderIDKeyUnknown1ByteQuantity")]
        unsafe CFStringRef* kOBEXHeaderIDKeyUnknown1ByteQuantity { get; }

        // extern CFStringRef kOBEXHeaderIDKeyUnknown4ByteQuantity;
        [Field ("kOBEXHeaderIDKeyUnknown4ByteQuantity")]
        unsafe CFStringRef* kOBEXHeaderIDKeyUnknown4ByteQuantity { get; }

        // extern CFStringRef kOBEXHeaderIDKeyUserDefined;
        [Field ("kOBEXHeaderIDKeyUserDefined")]
        unsafe CFStringRef* kOBEXHeaderIDKeyUserDefined { get; }
    }*/

    // @interface OBEXSession : NSObject
    [BaseType(typeof(NSObject))]
    interface OBEXSession
    {
    //    // -(OBEXError)OBEXConnect:(OBEXFlags)inFlags maxPacketLength:(OBEXMaxPacketLength)inMaxPacketLength optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXConnect:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXConnect(byte inFlags, ushort inMaxPacketLength, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXDisconnect:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXDisconnect:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXDisconnect(byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXPut:(Boolean)isFinalChunk headersData:(void *)inHeadersData headersDataLength:(size_t)inHeadersDataLength bodyData:(void *)inBodyData bodyDataLength:(size_t)inBodyDataLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXPut:headersData:headersDataLength:bodyData:bodyDataLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXPut(byte isFinalChunk, byte[] inHeadersData, nuint inHeadersDataLength, byte[] inBodyData, nuint inBodyDataLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXGet:(Boolean)isFinalChunk headers:(void *)inHeaders headersLength:(size_t)inHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXGet:headers:headersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXGet(byte isFinalChunk, byte[] inHeaders, nuint inHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXAbort:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXAbort:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXAbort(byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXSetPath:(OBEXFlags)inFlags constants:(OBEXConstants)inConstants optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXSetPath:constants:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXSetPath(byte inFlags, byte inConstants, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXConnectResponse:(OBEXOpCode)inResponseOpCode flags:(OBEXFlags)inFlags maxPacketLength:(OBEXMaxPacketLength)inMaxPacketLength optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXConnectResponse:flags:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXConnectResponse(byte inResponseOpCode, byte inFlags, ushort inMaxPacketLength, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXDisconnectResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXDisconnectResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXDisconnectResponse(byte inResponseOpCode, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXPutResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXPutResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXPutResponse(byte inResponseOpCode, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXGetResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXGetResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXGetResponse(byte inResponseOpCode, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXAbortResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXAbortResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXAbortResponse(byte inResponseOpCode, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

    //    // -(OBEXError)OBEXSetPathResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("OBEXSetPathResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
    //    int OBEXSetPathResponse(byte inResponseOpCode, byte[] inOptionalHeaders, nuint inOptionalHeadersLength, Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

        // -(OBEXMaxPacketLength)getAvailableCommandPayloadLength:(OBEXOpCode)inOpCode;
        [Export("getAvailableCommandPayloadLength:")]
        ushort GetAvailableCommandPayloadLength(byte inOpCode);

        // -(OBEXMaxPacketLength)getAvailableCommandResponsePayloadLength:(OBEXOpCode)inOpCode;
        [Export("getAvailableCommandResponsePayloadLength:")]
        ushort GetAvailableCommandResponsePayloadLength(byte inOpCode);

        // -(OBEXMaxPacketLength)getMaxPacketLength;
        [Export("getMaxPacketLength")]
        ushort MaxPacketLength { get; }

        // -(BOOL)hasOpenOBEXConnection;
        [Export("hasOpenOBEXConnection")]
        bool HasOpenOBEXConnection { get; }

    //    // -(void)setEventCallback:(OBEXSessionEventCallback)inEventCallback;
    //    //[Export ("setEventCallback:")]
    //    //unsafe void SetEventCallback (OBEXSessionEventCallback* inEventCallback);

    //    // -(void)setEventRefCon:(void *)inRefCon;
    //    [Export("setEventRefCon:")]
    //    void SetEventRefCon(IntPtr inRefCon);

    //    // -(void)setEventSelector:(SEL)inEventSelector target:(id)inEventSelectorTarget refCon:(id)inUserRefCon;
    //    [Export("setEventSelector:target:refCon:")]
    //    void SetEventSelector(Selector inEventSelector, NSObject inEventSelectorTarget, NSObject inUserRefCon);

    //    //// -(void)serverHandleIncomingData:(OBEXTransportEvent *)event;
    //    //[Export ("serverHandleIncomingData:")]
    //    //unsafe void ServerHandleIncomingData (OBEXTransportEvent* @event);

    //    //// -(void)clientHandleIncomingData:(OBEXTransportEvent *)event;
    //    //[Export ("clientHandleIncomingData:")]
    //    //unsafe void ClientHandleIncomingData (OBEXTransportEvent* @event);

    //    // -(OBEXError)sendDataToTransport:(void *)inDataToSend dataLength:(size_t)inDataLength;
    //    [Export("sendDataToTransport:dataLength:")]
    //    int SendDataToTransport(IntPtr inDataToSend, nuint inDataLength);

    //    // -(OBEXError)openTransportConnection:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
    //    [Export("openTransportConnection:selectorTarget:refCon:")]
    //    int OpenTransportConnection(Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

        // -(Boolean)hasOpenTransportConnection;
        [Export("hasOpenTransportConnection")]
        byte HasOpenTransportConnection { get; }

        // -(OBEXError)closeTransportConnection;
        [Export("closeTransportConnection")]
        int CloseTransportConnection();
    }

    // @interface IOBluetoothOBEXSession : OBEXSession <IOBluetoothRFCOMMChannelDelegate>
    [BaseType(typeof(OBEXSession))]
    interface BluetoothOBEXSession : RFCOMMChannelDelegate
    {
    //    // +(instancetype)withSDPServiceRecord:(IOBluetoothSDPServiceRecord *)inSDPServiceRecord;
    //    [Static]
    //    [Export("withSDPServiceRecord:")]
    //    IOBluetoothOBEXSession WithSDPServiceRecord(IOBluetoothSDPServiceRecord inSDPServiceRecord);

    //    // +(instancetype)withDevice:(IOBluetoothDevice *)inDevice channelID:(BluetoothRFCOMMChannelID)inRFCOMMChannelID;
    //    [Static]
    //    [Export("withDevice:channelID:")]
    //    IOBluetoothOBEXSession WithDevice(IOBluetoothDevice inDevice, byte inRFCOMMChannelID);

    //    // +(instancetype)withIncomingRFCOMMChannel:(IOBluetoothRFCOMMChannel *)inChannel eventSelector:(SEL)inEventSelector selectorTarget:(id)inEventSelectorTarget refCon:(void *)inUserRefCon;
    //    [Static]
    //    [Export("withIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
    //    IOBluetoothOBEXSession WithIncomingRFCOMMChannel(IOBluetoothRFCOMMChannel inChannel, Selector inEventSelector, NSObject inEventSelectorTarget, IntPtr inUserRefCon);

    //    // -(instancetype)initWithSDPServiceRecord:(IOBluetoothSDPServiceRecord *)inSDPServiceRecord;
    //    [Export("initWithSDPServiceRecord:")]
    //    IntPtr Constructor(IOBluetoothSDPServiceRecord inSDPServiceRecord);

        // -(instancetype)initWithDevice:(IOBluetoothDevice *)inDevice channelID:(BluetoothRFCOMMChannelID)inChannelID;
        [Export("initWithDevice:channelID:")]
        IntPtr Constructor(BluetoothDevice inDevice, byte inChannelID);

    //    // -(instancetype)initWithIncomingRFCOMMChannel:(IOBluetoothRFCOMMChannel *)inChannel eventSelector:(SEL)inEventSelector selectorTarget:(id)inEventSelectorTarget refCon:(void *)inUserRefCon;
    //    [Export("initWithIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
    //    IntPtr Constructor(IOBluetoothRFCOMMChannel inChannel, Selector inEventSelector, NSObject inEventSelectorTarget, IntPtr inUserRefCon);

        // -(IOBluetoothRFCOMMChannel *)getRFCOMMChannel;
        [Export("getRFCOMMChannel")]
        RFCOMMChannel RFCOMMChannel { get; }

        // -(IOBluetoothDevice *)getDevice;
        [Export("getDevice")]
        BluetoothDevice Device { get; }

        // -(IOReturn)sendBufferTroughChannel;
        [Export("sendBufferTroughChannel")]
        int SendBufferTroughChannel();

        // -(void)restartTransmission;
        [Export("restartTransmission")]
        void RestartTransmission();

        // -(BOOL)isSessionTargetAMac;
        [Export("isSessionTargetAMac")]
        bool IsSessionTargetAMac { get; }

        // -(OBEXError)openTransportConnection:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("openTransportConnection:selectorTarget:refCon:")]
        int OpenTransportConnection(Selector inSelector, NSObject inTarget, IntPtr inUserRefCon);

        // -(BOOL)hasOpenTransportConnection;
        [Export("hasOpenTransportConnection")]
        [New]
        bool HasOpenTransportConnection { get; }

        // -(OBEXError)closeTransportConnection;
        [Export("closeTransportConnection")]
        [New]
        int CloseTransportConnection();

    //    // -(OBEXError)sendDataToTransport:(void *)inDataToSend dataLength:(size_t)inDataLength;
    //    [Export("sendDataToTransport:dataLength:")]
    //    int SendDataToTransport(IntPtr inDataToSend, nuint inDataLength);

        // -(void)setOpenTransportConnectionAsyncSelector:(SEL)inSelector target:(id)inSelectorTarget refCon:(id)inUserRefCon;
        [Export("setOpenTransportConnectionAsyncSelector:target:refCon:")]
        void SetOpenTransportConnectionAsyncSelector(Selector inSelector, NSObject inSelectorTarget, NSObject inUserRefCon);

    //    // -(void)setOBEXSessionOpenConnectionCallback:(IOBluetoothOBEXSessionOpenConnectionCallback)inCallback refCon:(void *)inUserRefCon;
    //    //[Export ("setOBEXSessionOpenConnectionCallback:refCon:")]
    //    //unsafe void SetOBEXSessionOpenConnectionCallback (IOBluetoothOBEXSessionOpenConnectionCallback* inCallback, void* inUserRefCon);
    }

    // @interface OBEXFileTransferServices : NSObject
    [BaseType(typeof(NSObject))]
    interface OBEXFileTransferServices
    {
        [Wrap("WeakDelegate")]
        OBEXFileTransferServicesDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)withOBEXSession:(IOBluetoothOBEXSession *)inOBEXSession;
        [Static]
        [Export("withOBEXSession:")]
        OBEXFileTransferServices WithOBEXSession(BluetoothOBEXSession inOBEXSession);

        // -(instancetype)initWithOBEXSession:(IOBluetoothOBEXSession *)inOBEXSession;
        [Export("initWithOBEXSession:")]
        IntPtr Constructor(BluetoothOBEXSession inOBEXSession);

        // -(NSString *)currentPath;
        [Export("currentPath")]
        string CurrentPath { get; }

        // -(BOOL)isBusy;
        [Export("isBusy")]
        bool IsBusy { get; }

        // -(BOOL)isConnected;
        [Export("isConnected")]
        bool IsConnected { get; }

        // -(OBEXError)connectToFTPService;
        [Export("connectToFTPService")]
        int ConnectToFTPService();

        // -(OBEXError)connectToObjectPushService;
        [Export("connectToObjectPushService")]
        int ConnectToObjectPushService();

        // -(OBEXError)disconnect;
        [Export("disconnect")]
        int Disconnect();

        // -(OBEXError)changeCurrentFolderToRoot;
        [Export("changeCurrentFolderToRoot")]
        int ChangeCurrentFolderToRoot();

        // -(OBEXError)changeCurrentFolderBackward;
        [Export("changeCurrentFolderBackward")]
        int ChangeCurrentFolderBackward();

        // -(OBEXError)changeCurrentFolderForwardToPath:(NSString *)inDirName;
        [Export("changeCurrentFolderForwardToPath:")]
        int ChangeCurrentFolderForwardToPath(string inDirName);

        // -(OBEXError)createFolder:(NSString *)inDirName;
        [Export("createFolder:")]
        int CreateFolder(string inDirName);

        // -(OBEXError)removeItem:(NSString *)inItemName;
        [Export("removeItem:")]
        int RemoveItem(string inItemName);

        // -(OBEXError)retrieveFolderListing;
        [Export("retrieveFolderListing")]
        int RetrieveFolderListing();

        // -(OBEXError)sendFile:(NSString *)inLocalPathAndName;
        [Export("sendFile:")]
        int SendFile(string inLocalPathAndName);

        // -(OBEXError)copyRemoteFile:(NSString *)inRemoteFileName toLocalPath:(NSString *)inLocalPathAndName;
        [Export("copyRemoteFile:toLocalPath:")]
        int CopyRemoteFile(string inRemoteFileName, string inLocalPathAndName);

        // -(OBEXError)sendData:(NSData *)inData type:(NSString *)inType name:(NSString *)inName;
        [Export("sendData:type:name:")]
        int SendData(NSData inData, string inType, string inName);

        // -(OBEXError)getDefaultVCard:(NSString *)inLocalPathAndName;
        [Export("getDefaultVCard:")]
        int GetDefaultVCard(string inLocalPathAndName);

        // -(OBEXError)abort;
        [Export("abort")]
        int Abort();
    }

    // @interface OBEXFileTransferServicesDelegate (NSObject)
    [BaseType (typeof(NSObject))]
    [Protocol]
    interface OBEXFileTransferServicesDelegate
    {
    	// -(void)fileTransferServicesConnectionComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesConnectionComplete:error:")]
    	void FileTransferServicesConnectionComplete (OBEXFileTransferServices inServices, int inError);

    	// -(void)fileTransferServicesDisconnectionComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesDisconnectionComplete:error:")]
    	void FileTransferServicesDisconnectionComplete (OBEXFileTransferServices inServices, int inError);

    	// -(void)fileTransferServicesAbortComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesAbortComplete:error:")]
    	void FileTransferServicesAbortComplete (OBEXFileTransferServices inServices, int inError);

    	// -(void)fileTransferServicesRemoveItemComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError removedItem:(NSString *)inItemName;
    	[Export ("fileTransferServicesRemoveItemComplete:error:removedItem:")]
    	void FileTransferServicesRemoveItemComplete (OBEXFileTransferServices inServices, int inError, string inItemName);

    	// -(void)fileTransferServicesCreateFolderComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError folder:(NSString *)inFolderName;
    	[Export ("fileTransferServicesCreateFolderComplete:error:folder:")]
    	void FileTransferServicesCreateFolderComplete (OBEXFileTransferServices inServices, int inError, string inFolderName);

    	// -(void)fileTransferServicesPathChangeComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError finalPath:(NSString *)inPath;
    	[Export ("fileTransferServicesPathChangeComplete:error:finalPath:")]
    	void FileTransferServicesPathChangeComplete (OBEXFileTransferServices inServices, int inError, string inPath);

    	// -(void)fileTransferServicesRetrieveFolderListingComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError listing:(NSArray *)inListing;
    	[Export ("fileTransferServicesRetrieveFolderListingComplete:error:listing:")]
    	void FileTransferServicesRetrieveFolderListingComplete (OBEXFileTransferServices inServices, int inError, NSString[] inListing);

    	// -(void)fileTransferServicesFilePreparationComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesFilePreparationComplete:error:")]
    	void FileTransferServicesFilePreparationComplete (OBEXFileTransferServices inServices, int inError);

    	// -(void)fileTransferServicesSendFileProgress:(OBEXFileTransferServices *)inServices transferProgress:(NSDictionary *)inProgressDescription;
    	[Export ("fileTransferServicesSendFileProgress:transferProgress:")]
    	void FileTransferServicesSendFileProgress (OBEXFileTransferServices inServices, NSDictionary inProgressDescription);

    	// -(void)fileTransferServicesSendFileComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesSendFileComplete:error:")]
    	void FileTransferServicesSendFileComplete (OBEXFileTransferServices inServices, int inError);

    	// -(void)fileTransferServicesCopyRemoteFileProgress:(OBEXFileTransferServices *)inServices transferProgress:(NSDictionary *)inProgressDescription;
    	[Export ("fileTransferServicesCopyRemoteFileProgress:transferProgress:")]
    	void FileTransferServicesCopyRemoteFileProgress (OBEXFileTransferServices inServices, NSDictionary inProgressDescription);

    	// -(void)fileTransferServicesCopyRemoteFileComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesCopyRemoteFileComplete:error:")]
    	void FileTransferServicesCopyRemoteFileComplete (OBEXFileTransferServices inServices, int inError);
    }

    /*[Static]
    [Verify (ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern CFStringRef kFTSProgressBytesTransferredKey;
        [Field ("kFTSProgressBytesTransferredKey")]
        unsafe CFStringRef* kFTSProgressBytesTransferredKey { get; }

        // extern CFStringRef kFTSProgressBytesTotalKey;
        [Field ("kFTSProgressBytesTotalKey")]
        unsafe CFStringRef* kFTSProgressBytesTotalKey { get; }

        // extern CFStringRef kFTSProgressPercentageKey;
        [Field ("kFTSProgressPercentageKey")]
        unsafe CFStringRef* kFTSProgressPercentageKey { get; }

        // extern CFStringRef kFTSProgressPrecentageKey;
        [Field ("kFTSProgressPrecentageKey")]
        unsafe CFStringRef* kFTSProgressPrecentageKey { get; }

        // extern CFStringRef kFTSProgressEstimatedTimeKey;
        [Field ("kFTSProgressEstimatedTimeKey")]
        unsafe CFStringRef* kFTSProgressEstimatedTimeKey { get; }

        // extern CFStringRef kFTSProgressTimeElapsedKey;
        [Field ("kFTSProgressTimeElapsedKey")]
        unsafe CFStringRef* kFTSProgressTimeElapsedKey { get; }

        // extern CFStringRef kFTSProgressTransferRateKey;
        [Field ("kFTSProgressTransferRateKey")]
        unsafe CFStringRef* kFTSProgressTransferRateKey { get; }

        // extern CFStringRef kFTSListingNameKey;
        [Field ("kFTSListingNameKey")]
        unsafe CFStringRef* kFTSListingNameKey { get; }

        // extern CFStringRef kFTSListingTypeKey;
        [Field ("kFTSListingTypeKey")]
        unsafe CFStringRef* kFTSListingTypeKey { get; }

        // extern CFStringRef kFTSListingSizeKey;
        [Field ("kFTSListingSizeKey")]
        unsafe CFStringRef* kFTSListingSizeKey { get; }
    }*/

    //// @interface NSDictionaryOBEXExtensions (NSMutableDictionary)
    //[Category]
    //[BaseType (typeof(NSMutableDictionary))]
    //interface NSDictionaryOBEXExtensions
    //{
    //	//// +(instancetype)dictionaryWithOBEXHeadersData:(const void *)inHeadersData headersDataSize:(size_t)inDataSize;
    //	[Static]
    //	[Export ("dictionaryWithOBEXHeadersData:headersDataSize:")]
    //	unsafe NSMutableDictionary DictionaryWithOBEXHeadersData (IntPtr inHeadersData, nuint inDataSize);

    //	// +(instancetype)dictionaryWithOBEXHeadersData:(NSData *)inHeadersData;
    //	[Static]
    //	[Export ("dictionaryWithOBEXHeadersData:")]
    //	NSMutableDictionary DictionaryWithOBEXHeadersData (NSData inHeadersData);

    //	// -(NSMutableData *)getHeaderBytes;
    //	[Export ("getHeaderBytes")]
    //	NSMutableData HeaderBytes { get; }

    //	// -(OBEXError)addTargetHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addTargetHeader:length:")]
    //	int AddTargetHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addHTTPHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addHTTPHeader:length:")]
    //	int AddHTTPHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addBodyHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength endOfBody:(BOOL)isEndOfBody;
    //	[Export ("addBodyHeader:length:endOfBody:")]
    //	int AddBodyHeader (IntPtr inHeaderData, uint inHeaderDataLength, bool isEndOfBody);

    //	// -(OBEXError)addWhoHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addWhoHeader:length:")]
    //	int AddWhoHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addConnectionIDHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addConnectionIDHeader:length:")]
    //	int AddConnectionIDHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addApplicationParameterHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addApplicationParameterHeader:length:")]
    //	int AddApplicationParameterHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addByteSequenceHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addByteSequenceHeader:length:")]
    //	int AddByteSequenceHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addObjectClassHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addObjectClassHeader:length:")]
    //	int AddObjectClassHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addAuthorizationChallengeHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addAuthorizationChallengeHeader:length:")]
    //	int AddAuthorizationChallengeHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addAuthorizationResponseHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addAuthorizationResponseHeader:length:")]
    //	int AddAuthorizationResponseHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addTimeISOHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addTimeISOHeader:length:")]
    //	int AddTimeISOHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addTypeHeader:(NSString *)type;
    //	[Export ("addTypeHeader:")]
    //	int AddTypeHeader (string type);

    //	// -(OBEXError)addLengthHeader:(uint32_t)length;
    //	[Export ("addLengthHeader:")]
    //	int AddLengthHeader (uint length);

    //	// -(OBEXError)addTime4ByteHeader:(uint32_t)time4Byte;
    //	[Export ("addTime4ByteHeader:")]
    //	int AddTime4ByteHeader (uint time4Byte);

    //	// -(OBEXError)addCountHeader:(uint32_t)inCount;
    //	[Export ("addCountHeader:")]
    //	int AddCountHeader (uint inCount);

    //	// -(OBEXError)addDescriptionHeader:(NSString *)inDescriptionString;
    //	[Export ("addDescriptionHeader:")]
    //	int AddDescriptionHeader (string inDescriptionString);

    //	// -(OBEXError)addNameHeader:(NSString *)inNameString;
    //	[Export ("addNameHeader:")]
    //	int AddNameHeader (string inNameString);

    //	// -(OBEXError)addUserDefinedHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addUserDefinedHeader:length:")]
    //	unsafe int AddUserDefinedHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addImageHandleHeader:(NSString *)type;
    //	[Export ("addImageHandleHeader:")]
    //	int AddImageHandleHeader (string type);

    //	// -(OBEXError)addImageDescriptorHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addImageDescriptorHeader:length:")]
    //	int AddImageDescriptorHeader (IntPtr inHeaderData, uint inHeaderDataLength);
    //}

    /*[Static]
    [Verify (ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern NSString *const IOBluetoothHandsFreeIndicatorService;
        [Field ("IOBluetoothHandsFreeIndicatorService")]
        NSString IOBluetoothHandsFreeIndicatorService { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorCall;
        [Field ("IOBluetoothHandsFreeIndicatorCall")]
        NSString IOBluetoothHandsFreeIndicatorCall { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorCallSetup;
        [Field ("IOBluetoothHandsFreeIndicatorCallSetup")]
        NSString IOBluetoothHandsFreeIndicatorCallSetup { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorCallHeld;
        [Field ("IOBluetoothHandsFreeIndicatorCallHeld")]
        NSString IOBluetoothHandsFreeIndicatorCallHeld { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorSignal;
        [Field ("IOBluetoothHandsFreeIndicatorSignal")]
        NSString IOBluetoothHandsFreeIndicatorSignal { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorRoam;
        [Field ("IOBluetoothHandsFreeIndicatorRoam")]
        NSString IOBluetoothHandsFreeIndicatorRoam { get; }

        // extern NSString *const IOBluetoothHandsFreeIndicatorBattChg;
        [Field ("IOBluetoothHandsFreeIndicatorBattChg")]
        NSString IOBluetoothHandsFreeIndicatorBattChg { get; }

        // extern NSString *const IOBluetoothHandsFreeCallIndex;
        [Field ("IOBluetoothHandsFreeCallIndex")]
        NSString IOBluetoothHandsFreeCallIndex { get; }

        // extern NSString *const IOBluetoothHandsFreeCallDirection;
        [Field ("IOBluetoothHandsFreeCallDirection")]
        NSString IOBluetoothHandsFreeCallDirection { get; }

        // extern NSString *const IOBluetoothHandsFreeCallStatus;
        [Field ("IOBluetoothHandsFreeCallStatus")]
        NSString IOBluetoothHandsFreeCallStatus { get; }

        // extern NSString *const IOBluetoothHandsFreeCallMode;
        [Field ("IOBluetoothHandsFreeCallMode")]
        NSString IOBluetoothHandsFreeCallMode { get; }

        // extern NSString *const IOBluetoothHandsFreeCallMultiparty;
        [Field ("IOBluetoothHandsFreeCallMultiparty")]
        NSString IOBluetoothHandsFreeCallMultiparty { get; }

        // extern NSString *const IOBluetoothHandsFreeCallNumber;
        [Field ("IOBluetoothHandsFreeCallNumber")]
        NSString IOBluetoothHandsFreeCallNumber { get; }

        // extern NSString *const IOBluetoothHandsFreeCallType;
        [Field ("IOBluetoothHandsFreeCallType")]
        NSString IOBluetoothHandsFreeCallType { get; }

        // extern NSString *const IOBluetoothHandsFreeCallName;
        [Field ("IOBluetoothHandsFreeCallName")]
        NSString IOBluetoothHandsFreeCallName { get; }
    }

    [Static]
    [Verify (ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern NSString *const IOBluetoothPDUServicCenterAddress;
        [Field ("IOBluetoothPDUServicCenterAddress")]
        NSString IOBluetoothPDUServicCenterAddress { get; }

        // extern NSString *const IOBluetoothPDUServiceCenterAddressType;
        [Field ("IOBluetoothPDUServiceCenterAddressType")]
        NSString IOBluetoothPDUServiceCenterAddressType { get; }

        // extern NSString *const IOBluetoothPDUType;
        [Field ("IOBluetoothPDUType")]
        NSString IOBluetoothPDUType { get; }

        // extern NSString *const IOBluetoothPDUOriginatingAddress;
        [Field ("IOBluetoothPDUOriginatingAddress")]
        NSString IOBluetoothPDUOriginatingAddress { get; }

        // extern NSString *const IOBluetoothPDUOriginatingAddressType;
        [Field ("IOBluetoothPDUOriginatingAddressType")]
        NSString IOBluetoothPDUOriginatingAddressType { get; }

        // extern NSString *const IOBluetoothPDUProtocolID;
        [Field ("IOBluetoothPDUProtocolID")]
        NSString IOBluetoothPDUProtocolID { get; }

        // extern NSString *const IOBluetoothPDUTimestamp;
        [Field ("IOBluetoothPDUTimestamp")]
        NSString IOBluetoothPDUTimestamp { get; }

        // extern NSString *const IOBluetoothPDUEncoding;
        [Field ("IOBluetoothPDUEncoding")]
        NSString IOBluetoothPDUEncoding { get; }

        // extern NSString *const IOBluetoothPDUUserData;
        [Field ("IOBluetoothPDUUserData")]
        NSString IOBluetoothPDUUserData { get; }
    }*/

    // @interface IOBluetoothHandsFree : NSObject
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [BaseType(typeof(NSObject))]
    interface IOBluetoothHandsFree
    {
        // @property (assign) uint32_t supportedFeatures __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("supportedFeatures")]
        uint SupportedFeatures { get; set; }

        // @property (assign) float inputVolume __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("inputVolume")]
        float InputVolume { get; set; }

        // @property (getter = isInputMuted, assign) BOOL inputMuted __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("inputMuted")]
        bool InputMuted { [Bind("isInputMuted")] get; set; }

        // @property (assign) float outputVolume __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("outputVolume")]
        float OutputVolume { get; set; }

        // @property (getter = isOutputMuted, assign) BOOL outputMuted __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("outputMuted")]
        bool OutputMuted { [Bind("isOutputMuted")] get; set; }

        // @property (readonly, retain) IOBluetoothDevice * device __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("device", ArgumentSemantic.Retain)]
        BluetoothDevice Device { get; }

        // @property (readonly) uint32_t deviceSupportedFeatures __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("deviceSupportedFeatures")]
        uint DeviceSupportedFeatures { get; }

        // @property (readonly) uint32_t deviceSupportedSMSServices __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("deviceSupportedSMSServices")]
        uint DeviceSupportedSMSServices { get; }

        // @property (readonly) uint32_t deviceCallHoldModes __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("deviceCallHoldModes")]
        HandsFreeCallHoldModes DeviceCallHoldModes { get; }

        // @property (readonly) IOBluetoothSMSMode SMSMode __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("SMSMode")]
        SMSMode SMSMode { get; }

        // @property (readonly, getter = isSMSEnabled) BOOL SMSEnabled __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("SMSEnabled")]
        bool SMSEnabled { [Bind("isSMSEnabled")] get; }

        [Wrap("WeakDelegate")]
        IOBluetoothHandsFreeDelegate Delegate { get; set; }

        // @property (assign) id<IOBluetoothHandsFreeDelegate> delegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // -(int)indicator:(NSString *)indicatorName __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("indicator:")]
        int Indicator(string indicatorName);

        // -(void)setIndicator:(NSString *)indicatorName value:(int)indicatorValue __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("setIndicator:value:")]
        void SetIndicator(string indicatorName, int indicatorValue);

        // -(instancetype)initWithDevice:(IOBluetoothDevice *)device delegate:(id<IOBluetoothHandsFreeDelegate>)inDelegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("initWithDevice:delegate:")]
        IntPtr Constructor(BluetoothDevice device, IOBluetoothHandsFreeDelegate inDelegate);

        // -(void)connect __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("connect")]
        void Connect();

        // -(void)disconnect __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("disconnect")]
        void Disconnect();

        // @property (readonly, getter = isConnected) BOOL connected __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("connected")]
        bool Connected { [Bind("isConnected")] get; }

        // -(void)connectSCO __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("connectSCO")]
        void ConnectSCO();

        // -(void)disconnectSCO __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("disconnectSCO")]
        void DisconnectSCO();

        // -(BOOL)isSCOConnected __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("isSCOConnected")]
        bool IsSCOConnected { get; }
    }

    // @protocol IOBluetoothHandsFreeDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface IOBluetoothHandsFreeDelegate
    {
        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device connected:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:connected:")]
        void Connected(IOBluetoothHandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device disconnected:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:disconnected:")]
        void Disconnected(IOBluetoothHandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device scoConnectionOpened:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:scoConnectionOpened:")]
        void ScoConnectionOpened(IOBluetoothHandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device scoConnectionClosed:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:scoConnectionClosed:")]
        void ScoConnectionClosed(IOBluetoothHandsFree device, NSNumber status);
    }

    //// @interface HandsFreeDeviceAdditions (IOBluetoothDevice)
    //[Category]
    //[BaseType(typeof(IOBluetoothDevice))]
    //interface IOBluetoothDevice_HandsFreeDeviceAdditions
    //{
    //    // -(IOBluetoothSDPServiceRecord *)handsFreeAudioGatewayServiceRecord __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeAudioGatewayServiceRecord")]
    //    IOBluetoothSDPServiceRecord HandsFreeAudioGatewayServiceRecord { get; }

    //    // @property (readonly, getter = isHandsFreeAudioGateway) BOOL handsFreeAudioGateway __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeAudioGateway")]
    //    bool IsHandsFreeAudioGateway { [Bind("isHandsFreeAudioGateway")] get; }

    //    // -(IOBluetoothSDPServiceRecord *)handsFreeDeviceServiceRecord __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeDeviceServiceRecord")]
    //    IOBluetoothSDPServiceRecord HandsFreeDeviceServiceRecord { get; }

    //    // @property (readonly, getter = isHandsFreeDevice) BOOL handsFreeDevice __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeDevice")]
    //    bool IsHandsFreeDevice { [Bind("isHandsFreeDevice")] get; }
    //}

    //// @interface HandsFreeSDPServiceRecordAdditions (IOBluetoothSDPServiceRecord)
    //[Category]
    //[BaseType(typeof(IOBluetoothSDPServiceRecord))]
    //interface IOBluetoothSDPServiceRecord_HandsFreeSDPServiceRecordAdditions
    //{
    //    // -(uint16_t)handsFreeSupportedFeatures __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeSupportedFeatures")]
    //    ushort HandsFreeSupportedFeatures { get; }
    //}

    // @interface IOBluetoothHandsFreeAudioGateway : IOBluetoothHandsFree
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [BaseType(typeof(IOBluetoothHandsFree))]
    interface HandsFreeAudioGateway
    {
        // -(instancetype)initWithDevice:(IOBluetoothDevice *)device delegate:(id)inDelegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("initWithDevice:delegate:")]
        IntPtr Constructor(BluetoothDevice device, NSObject inDelegate);

        // -(void)createIndicator:(NSString *)indicatorName min:(int)minValue max:(int)maxValue currentValue:(int)currentValue __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("createIndicator:min:max:currentValue:")]
        void CreateIndicator(string indicatorName, int minValue, int maxValue, int currentValue);

        // -(void)processATCommand:(NSString *)atCommand __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("processATCommand:")]
        void ProcessATCommand(string atCommand);

        // -(void)sendOKResponse __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendOKResponse")]
        void SendOKResponse();

        // -(void)sendResponse:(NSString *)response __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendResponse:")]
        void SendResponse(string response);

        // -(void)sendResponse:(NSString *)response withOK:(BOOL)withOK __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendResponse:withOK:")]
        void SendResponse(string response, bool withOK);
    }

    // @protocol IOBluetoothHandsFreeAudioGatewayDelegate
    [Protocol]
    interface HandsFreeAudioGatewayDelegate
    {
        // @optional -(void)handsFree:(IOBluetoothHandsFreeAudioGateway *)device hangup:(NSNumber *)hangup __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:hangup:")]
        void Hangup(HandsFreeAudioGateway device, NSNumber hangup);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeAudioGateway *)device redial:(NSNumber *)redial __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:redial:")]
        void Redial(HandsFreeAudioGateway device, NSNumber redial);
    }

    // @interface IOBluetoothHandsFreeDevice : IOBluetoothHandsFree
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [BaseType(typeof(IOBluetoothHandsFree))]
    interface HandsFreeDevice
    {
        // -(instancetype)initWithDevice:(IOBluetoothDevice *)device delegate:(id)delegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("initWithDevice:delegate:")]
        IntPtr Constructor(BluetoothDevice device, NSObject @delegate);

        // -(void)dialNumber:(NSString *)aNumber __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("dialNumber:")]
        void DialNumber(string aNumber);

        // -(void)memoryDial:(int)memoryLocation __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("memoryDial:")]
        void MemoryDial(int memoryLocation);

        // -(void)redial __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("redial")]
        void Redial();

        // -(void)endCall __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("endCall")]
        void EndCall();

        // -(void)acceptCall __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("acceptCall")]
        void AcceptCall();

        // -(void)acceptCallOnPhone __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("acceptCallOnPhone")]
        void AcceptCallOnPhone();

        // -(void)sendDTMF:(NSString *)character __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendDTMF:")]
        void SendDTMF(string character);

        // -(void)subscriberNumber __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("subscriberNumber")]
        void SubscriberNumber();

        // -(void)currentCallList __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("currentCallList")]
        void CurrentCallList();

        // -(void)releaseHeldCalls __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("releaseHeldCalls")]
        void ReleaseHeldCalls();

        // -(void)releaseActiveCalls __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("releaseActiveCalls")]
        void ReleaseActiveCalls();

        // -(void)releaseCall:(int)index __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("releaseCall:")]
        void ReleaseCall(int index);

        // -(void)holdCall __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("holdCall")]
        void HoldCall();

        // -(void)placeAllOthersOnHold:(int)index __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("placeAllOthersOnHold:")]
        void PlaceAllOthersOnHold(int index);

        // -(void)addHeldCall __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("addHeldCall")]
        void AddHeldCall();

        // -(void)callTransfer __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("callTransfer")]
        void CallTransfer();

        // -(void)transferAudioToComputer __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("transferAudioToComputer")]
        void TransferAudioToComputer();

        // -(void)transferAudioToPhone __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("transferAudioToPhone")]
        void TransferAudioToPhone();

        // -(void)sendSMS:(NSString *)aNumber message:(NSString *)aMessage __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendSMS:message:")]
        void SendSMS(string aNumber, string aMessage);

        // -(void)sendATCommand:(NSString *)atCommand __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendATCommand:")]
        void SendATCommand(string atCommand);

        // -(void)sendATCommand:(NSString *)atCommand timeout:(float)timeout selector:(SEL)selector target:(id)target __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("sendATCommand:timeout:selector:target:")]
        void SendATCommand(string atCommand, float timeout, Selector selector, NSObject target);
    }

    // @protocol IOBluetoothHandsFreeDeviceDelegate <IOBluetoothHandsFreeDelegate>
    [Protocol]
    interface HandsFreeDeviceDelegate : IOBluetoothHandsFreeDelegate
    {
        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device isServiceAvailable:(NSNumber *)isServiceAvailable __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:isServiceAvailable:")]
        void IsServiceAvailable(HandsFreeDevice device, NSNumber isServiceAvailable);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device isCallActive:(NSNumber *)isCallActive __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:isCallActive:")]
        void IsCallActive(HandsFreeDevice device, NSNumber isCallActive);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device callSetupMode:(NSNumber *)callSetupMode __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:callSetupMode:")]
        void CallSetupMode(HandsFreeDevice device, NSNumber callSetupMode);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device callHoldState:(NSNumber *)callHoldState __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:callHoldState:")]
        void CallHoldState(HandsFreeDevice device, NSNumber callHoldState);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device signalStrength:(NSNumber *)signalStrength __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:signalStrength:")]
        void SignalStrength(HandsFreeDevice device, NSNumber signalStrength);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device isRoaming:(NSNumber *)isRoaming __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:isRoaming:")]
        void IsRoaming(HandsFreeDevice device, NSNumber isRoaming);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device batteryCharge:(NSNumber *)batteryCharge __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:batteryCharge:")]
        void BatteryCharge(HandsFreeDevice device, NSNumber batteryCharge);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device incomingCallFrom:(NSString *)number __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:incomingCallFrom:")]
        void IncomingCallFrom(HandsFreeDevice device, string number);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device ringAttempt:(NSNumber *)ringAttempt __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:ringAttempt:")]
        void RingAttempt(HandsFreeDevice device, NSNumber ringAttempt);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device currentCall:(NSDictionary *)currentCall __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:currentCall:")]
        void CurrentCall(HandsFreeDevice device, NSDictionary currentCall);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device subscriberNumber:(NSString *)subscriberNumber __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:subscriberNumber:")]
        void SubscriberNumber(HandsFreeDevice device, string subscriberNumber);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device incomingSMS:(NSDictionary *)sms __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:incomingSMS:")]
        void IncomingSMS(HandsFreeDevice device, NSDictionary sms);

        // @optional -(void)handsFree:(IOBluetoothHandsFreeDevice *)device unhandledResultCode:(NSString *)resultCode __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:unhandledResultCode:")]
        void UnhandledResultCode(HandsFreeDevice device, string resultCode);
    }
}