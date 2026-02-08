using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace IOBluetooth
{
    // @interface IOBluetoothObject : NSObject <NSCopying>
    [BaseType(typeof(NSObject), Name ="IOBluetoothObject")]
    interface BluetoothObject : INSCopying
    {
    }

    // @interface IOBluetoothUserNotification : NSObject
    /// <summary>
    /// Represents a registered notification.
    /// </summary>
    [BaseType(typeof(NSObject), Name = "IOBluetoothUserNotification")]
    public interface UserNotification
    {
        // -(void)unregister;
        /// <summary>
        /// Called to unregister the target notification.
        /// </summary>
        [Export("unregister")]
        void Unregister();
    }

    // @protocol IOBluetoothDeviceAsyncCallbacks
    [Protocol(Name = "IOBluetoothDeviceAsyncCallbacks"), Model]
    public interface BluetoothDeviceAsyncCallbacks
    {
        // @required -(void)remoteNameRequestComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
        [Abstract]
        [Export("remoteNameRequestComplete:status:")]
        void RemoteNameRequestComplete(BluetoothDevice device, IOReturn status);

        // @required -(void)connectionComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
        [Abstract]
        [Export("connectionComplete:status:")]
        void ConnectionComplete(BluetoothDevice device, IOReturn status);

        // @required -(void)sdpQueryComplete:(IOBluetoothDevice *)device status:(IOReturn)status;
        [Abstract]
        [Export("sdpQueryComplete:status:")]
        void SdpQueryComplete(BluetoothDevice device, int status);
    }

    // @interface IOBluetoothDevice : IOBluetoothObject <NSCoding, NSSecureCoding>
    /// <summary>
    /// Represents a single remote Bluetooth device.
    /// </summary>
    [BaseType(typeof(BluetoothObject), Name = "IOBluetoothDevice")]
    public partial interface BluetoothDevice : INSCoding, INSSecureCoding
    {
        // +(IOBluetoothUserNotification *)registerForConnectNotifications:(id)observer selector:(SEL)inSelector;
        [Static]
        [Export("registerForConnectNotifications:selector:")]
        UserNotification RegisterForConnectNotifications(NSObject observer, Selector inSelector);

        // -(IOBluetoothUserNotification *)registerForDisconnectNotification:(id)observer selector:(SEL)inSelector;
        [Export("registerForDisconnectNotification:selector:")]
        UserNotification RegisterForDisconnectNotification(NSObject observer, Selector inSelector);

        // +(instancetype)deviceWithAddress:(const BluetoothDeviceAddress *)address;
        /// <summary>
        /// Returns the BluetoothDevice object for the given BluetoothDeviceAddress
        /// </summary>
        /// <param name="address">BluetoothDeviceAddress for which a BluetoothDevice instance is desired.</param>
        /// <returns>Returns the BluetoothDevice object for the given BluetoothDeviceAddress.</returns>
        [Static]
        [Export("deviceWithAddress:")]
        BluetoothDevice DeviceWithAddress(ref BluetoothDeviceAddress address);

        // +(instancetype)deviceWithAddressString:(NSString *)address;
        /// <summary>
        /// Returns the BluetoothDevice object for the given address string.
        /// </summary>
        /// <returns>Returns the BluetoothDevice object for the given.</returns>
        /// <param name="address">A string containing the BD_ADDR for which a BluetoothDevice instance is desired.
        /// The string should be of the form xx:xx:xx:xx:xx:xx</param>
        [Static]
        [Export("deviceWithAddressString:")]
        BluetoothDevice DeviceWithAddressString(string address);

        // -(IOReturn)openL2CAPChannelSync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm delegate:(id)channelDelegate;
        /// <summary>
        /// Opens a new L2CAP channel to the target device.
        /// Returns only after the channel is opened.
        /// </summary>
        /// <param name="newChannel">A pointer to an IOBluetoothL2CAPChannel object to receive the L2CAP channel requested to be opened.
        /// The newChannel pointer will only be set if kIOReturnSuccess is returned.</param>
        /// <param name="psm">The L2CAP PSM value for the new channel.</param>
        /// <param name="channelDelegate">The object that will play the role of delegate for the channel.
        /// A channel delegate is the object the l2cap uses as target for data and events.
        /// The developer will implement only the the methods he/she is interested in.</param>
        /// <returns>Returns IOReturn.Success if the open process was successfully started (or if an existing L2CAP channel was found).</returns>
        [Export("openL2CAPChannelSync:withPSM:delegate:")]
        IOReturn OpenL2CAPChannelSync(out L2CapChannel newChannel, L2CapPsm psm, L2CapChannelDelegate channelDelegate);

        // -(IOReturn)openL2CAPChannelAsync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm delegate:(id)channelDelegate;
        /// <summary>
        /// Opens a new L2CAP channel to the target device.
        /// Returns immediately after starting the opening process.
        /// </summary>
        /// <param name="newChannel">An L2CAPChannel object to receive the L2CAP channel requested to be opened.
        /// The newChannel will only be set if IOReturn.Success is returned.</param>
        /// <param name="psm">The L2CAP PSM value for the new channel.</param>
        /// <param name="channelDelegate">The object that will play the role of delegate for the channel.
        /// A channel delegate is the object the l2cap uses as target for data and events.
        /// The developer will implement only the the methods he/she is interested in.</param>
        /// <returns>Returns IOReturn.Success if the open process was successfully started (or if an existing L2CAP channel was found).</returns>
        [Export("openL2CAPChannelAsync:withPSM:delegate:")]
        IOReturn OpenL2CAPChannelAsync(out L2CapChannel newChannel, L2CapPsm psm, L2CapChannelDelegate channelDelegate);

        // -(IOReturn)sendL2CAPEchoRequest:(void *)data length:(UInt16)length;
        /// <summary>
        /// Send an echo request over the L2CAP connection to a remote device.
        /// </summary>
        /// <param name="data">Pointer to buffer to send.</param>
        /// <param name="length">Length of the buffer to send.</param>
        /// <returns>Returns IOReturn.Success if the echo request was able to be sent.</returns>
        [Export("sendL2CAPEchoRequest:length:")]
        IOReturn SendL2CAPEchoRequest(NSArray data, ushort length);

        // -(IOReturn)openRFCOMMChannelSync:(IOBluetoothRFCOMMChannel **)rfcommChannel withChannelID:(BluetoothRFCOMMChannelID)channelID delegate:(id)channelDelegate;
        /// <summary>
        /// Opens a new RFCOMM channel to the target device.
        /// Returns only once the channel is open or failed to open.
        /// </summary>
        /// <returns>Returns IOReturnSuccess if the open process was successfully started (or if an existing RFCOMM channel was found). 
        /// The channel must be released when the caller is done with it.</returns>
        /// <param name="rfcommChannel">A pointer to an IOBluetoothRFCOMMChannel object to receive the RFCOMM channel requested to be opened. 
        /// The rfcommChannel pointer will only be set if IOReturn.Success is returned.</param>
        /// <param name="channelID">The RFCOMM channel ID for the new channel.</param>
        /// <param name="channelDelegate">the object that will play the role of delegate for the channel. 
        /// A channel delegate is the object the rfcomm uses as target for data and events. 
        /// The developer will implement only the the methods he/she is interested in.</param>
        [Export("openRFCOMMChannelSync:withChannelID:delegate:")]
        IOReturn OpenRfcommChannelSync(out RfcommChannel rfcommChannel, byte channelID, RfcommChannelDelegate channelDelegate);

        // -(IOReturn)openRFCOMMChannelAsync:(IOBluetoothRFCOMMChannel **)rfcommChannel withChannelID:(BluetoothRFCOMMChannelID)channelID delegate:(id)channelDelegate;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rfcommChannel">An RFCommChannel to receive the RFCOMM channel requested to be opened.
        /// The rfcommChannel will only be set if IOReturnSuccess is returned.</param>
        /// <param name="channelID">The RFCOMM channel ID for the new channel.</param>
        /// <param name="channelDelegate">The object that will play the role of delegate for the channel.
        /// A channel delegate is the object the rfcomm uses as target for data and events.
        /// The developer will implement only the the methods he/she is interested in.</param>
        /// <returns>Returns IOReturn.Success if the open process was successfully started (or if an existing RFCOMM channel was found).
        /// The channel must be released when the caller is done with it.</returns>
        [Export("openRFCOMMChannelAsync:withChannelID:delegate:")]
        IOReturn OpenRfcommChannelAsync(out RfcommChannel rfcommChannel, byte channelID, RfcommChannelDelegate channelDelegate);

        // @property (readonly) BluetoothClassOfDevice classOfDevice;
        /// <summary>
        /// Gets the full class of device value for the remote device.
        /// </summary>
        /// <value>The class of device.</value>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("classOfDevice")]
        uint ClassOfDevice { get; }

        // @property (readonly) BluetoothServiceClassMajor serviceClassMajor;
        /// <summary>
        /// Get the major service class of the device.
        /// </summary>
        /// <value>The service class major.</value>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("serviceClassMajor")]
        ServiceClassMajor ServiceClassMajor { get; }

        // @property (readonly) BluetoothDeviceClassMajor deviceClassMajor;
        /// <summary>
        /// Get the major device class of the device.
        /// </summary>
        /// <value>The device class major.</value>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("deviceClassMajor")]
        DeviceClassMajor DeviceClassMajor { get; }

        // @property (readonly) BluetoothDeviceClassMinor deviceClassMinor;
        /// <summary>
        /// Get the minor service class of the device.
        /// </summary>
        /// <value>The device class minor.</value>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("deviceClassMinor")]
        DeviceClassMinor DeviceClassMinor { get; }

        // @property (readonly, copy) NSString * name;
        /// <summary>
        /// Get the human readable name of the remote device.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("name")]
        string Name { get; }

        // @property (readonly) NSString * nameOrAddress;
        /// <summary>
        /// Get the human readable name of the remote device. 
        /// If the name is not present, it will return a string containing the device's address.
        /// </summary>
        /// <value>The name or address.</value>
        /// <remarks>If a remote name request has been successfully completed, the device name will be returned.
        /// If not, a string containg the device address in the format of “XX-XX-XX-XX-XX-XX” will be returned.</remarks>
        [Export("nameOrAddress")]
        string NameOrAddress { get; }

        // @property (readonly, retain) NSDate * lastNameUpdate;
        [Internal]
        [Export("lastNameUpdate", ArgumentSemantic.Retain)]
        NSDate? GetLastNameUpdate();

        // -(const BluetoothDeviceAddress *)getAddress;
        [Internal]
        [Export("getAddress")]
        unsafe IntPtr GetAddress();

        // @property (readonly) NSString * addressString;
        /// <summary>
        /// Get a string representation of the Bluetooth device address for the target device. 
        /// The format of the string is the same as returned by IOBluetoothNSStringFromDeviceAddress().
        /// </summary>
        /// <value>The address string.</value>
        [Export("addressString")]
        string AddressString { get; }

        // -(BluetoothPageScanRepetitionMode)getPageScanRepetitionMode;
        /// <summary>
        /// Get the value of the page scan repetition mode for the device.
        /// </summary>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("getPageScanRepetitionMode")]
        PageScanRepetitionMode PageScanRepetitionMode { get; }

        // -(BluetoothPageScanPeriodMode)getPageScanPeriodMode;
        /// <summary>
        /// Get the value of the page scan period mode for the device.
        /// </summary>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("getPageScanPeriodMode")]
        PageScanPeriodMode PageScanPeriodMode { get; }

        // -(BluetoothPageScanMode)getPageScanMode;
        /// <summary>
        /// Get the page scan mode for the device.
        /// </summary>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("getPageScanMode")]
        PageScanMode PageScanMode { get; }

        // -(BluetoothClockOffset)getClockOffset;
        /// <summary>
        /// Get the clock offset value of the device.
        /// </summary>
        /// <remarks>This value is only meaningful if the target device has been seen during an inquiry.
        /// This can be by checking the result of <see cref="LastInquiryUpdate"/>.
        /// If null is returned, then the device hasn’t been seen.</remarks>
        [Export("getClockOffset")]
        ushort ClockOffset { get; }

        // -(NSDate *)getLastInquiryUpdate;
        /// <summary>
        /// Get the date/time of the last time the device was returned during an inquiry.
        /// </summary>
        /// <value>Returns the date/time of the last time the device was seen during an inquiry.
        /// If the device has never been seen during an inquiry, null is returned.</value>
        [Internal]
        [Export("getLastInquiryUpdate")]
        NSDate? GetLastInquiryUpdate();

        // -(BluetoothHCIRSSIValue)RSSI __attribute__((availability(macos, introduced=10.7)));
        /// <summary>
        /// Get the RSSI device (if connected), above or below the golden range.
        /// </summary>
        /// <value>Returns the RSSI of the device.
        /// If the value cannot be read (e.g. the device is disconnected), a value of +127 will be returned.</value>
        /// <remarks>If the RSSI is within the golden range, a value of 0 is returned.
        /// For the actual RSSI value, use <see cref="RawRssi"/>.
        /// For more information, see the Bluetooth 4.0 Core Specification.</remarks>
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("RSSI")]
        sbyte Rssi { get; }

        // -(BluetoothHCIRSSIValue)rawRSSI __attribute__((availability(macos, introduced=10.7)));
        /// <summary>
        /// Get the raw RSSI device (if connected).
        /// </summary>
        /// <remarks>This value is the perceived RSSI value, not relative the the golden range (see <see cref="Rssi"/> for that value).
        /// This value will not available on all Bluetooth modules.
        /// If the value cannot be read (e.g. the device is disconnected) or is not available on a module, a value of +127 will be returned.</remarks>
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("rawRSSI")]
        sbyte RawRssi { get; }

        // -(BOOL)isConnected;
        /// <summary>
        /// Indicates whether a baseband connection to the device exists.
        /// </summary>
        /// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
        [Export("isConnected")]
        bool IsConnected { get; }

        // -(IOReturn)openConnection;
        /// <summary>
        /// Create a baseband connection to the device.
        /// </summary>
        /// <returns>Returns IOReturn.Success if the connection was successfully created.</returns>
        /// <remarks>his method is synchronous and will not return until either a connection has been established or the create connection has failed (perhaps timed out).
        /// This method does the same thing as calling OpenConnection with a null target.
        /// This call with proceed without authentication required, and using the default page timeout value.
        /// If authentication or a non-default page timeout is required the method OpenConnection:withPageTimeout:authenticationRequired: should be used instead.
        /// As of OS X 10.7, this method will no longer mask out “Connection Exists” ‘errors’ with a success result code; your code must account for the cases where the baseband connection is already open.</remarks>
        [Export("openConnection")]
        IOReturn OpenConnection();

        // -(IOReturn)openConnection:(id)target;
        /// <summary>
        /// Create a baseband connection to the device.
        /// </summary>
        /// <param name="target">The target to message when the create connection call is complete.</param>
        /// <returns>Returns IOReturn.Success if the connection was successfully created (or if asynchronous, if the CREATE_CONNECTION command was successfully issued).</returns>
        /// <remarks>If a target is specified, the open connection call is asynchronous and on completion of the CREATE_CONNECTION command, the method <see cref="BluetoothDeviceAsyncCallbacks.ConnectionComplete"/> will be called on the specified target.
        /// If no target is specified, the call is synchronous and will not return until the connection is open or the CREATE_CONNECTION call has failed.
        /// This call with proceed without authentication required, and using the default page timeout value.
        /// If authentication or a non-default page timeout is required the method OpenConnection:withPageTimeout:authenticationRequired: should be used instead.
        /// As of OS X 10.7, this method will no longer mask out “Connection Exists” ‘errors’ with a success result code; your code must account for the cases where the baseband connection is already open.</remarks>
        [Export("openConnection:")]
        IOReturn OpenConnection(NSObject target);

        // -(IOReturn)openConnection:(id)target withPageTimeout:(BluetoothHCIPageTimeout)pageTimeoutValue authenticationRequired:(BOOL)authenticationRequired;
        /// <summary>
        /// Create a baseband connection to the device.
        /// </summary>
        /// <param name="target">The target to message when the create connection call is complete.</param>
        /// <param name="pageTimeoutValue">The page timeout value to use for this call.</param>
        /// <param name="authenticationRequired">Bool value to indicate whether authentication should be required for the connection.</param>
        /// <returns>Returns IOReturn.Success if the connection was successfully created (or if asynchronous, if the CREATE_CONNECTION command was successfully issued).</returns>
        /// <remarks>If a target is specified, the open connection call is asynchronous and on completion of the CREATE_CONNECTION command, the method <see cref="BluetoothDeviceAsyncCallbacks.ConnectionComplete"/> will be called on the specified target.
        /// If no target is specified, the call is synchronous and will not return until the connection is open or the CREATE_CONNECTION call has failed.
        /// NOTE: This method is only available in macOS 10.2.7 (Bluetooth v1.3) or later.
        /// As of OS X 10.7, this method will no longer mask out “Connection Exists” ‘errors’ with a success result code; your code must account for the cases where the baseband connection is already open.</remarks>
        [Export("openConnection:withPageTimeout:authenticationRequired:")]
        IOReturn OpenConnection(NSObject target, ushort pageTimeoutValue, bool authenticationRequired);

        // -(IOReturn)closeConnection;
        /// <summary>
        /// Close down the baseband connection to the device.
        /// </summary>
        /// <returns>Returns IOReturn.Success if the connection has successfully been closed.</returns>
        /// <remarks>This method is synchronous and will not return until the connection has been closed (or the command failed).
        /// In the future this API will be changed to allow asynchronous operation.</remarks>
        [Export("closeConnection")]
        IOReturn CloseConnection();

        // -(IOReturn)remoteNameRequest:(id)target;
        /// <summary>
        /// Issues a remote name request to the target device.
        /// </summary>
        /// <param name="target">The target to message when the remote name request is complete.</param>
        /// <returns>Returns IOReturn.Success if the remote name request was successfully issued (and if synchronous, if the request completed successfully).</returns>
        /// <remarks>If a target is specified, the request is asynchronous and on completion of the request, the method <see cref="BluetoothDeviceAsyncCallbacks.RemoteNameRequestComplete"/> will be called on the specified target.
        /// If no target is specified, the request is made synchronously and won’t return until the request is complete.
        /// This call with operate with the default page timeout value.
        /// If a different page timeout value is desired, the method -remoteNameRequest:withPageTimeout: should be used instead.</remarks>
        [Export("remoteNameRequest:")]
        IOReturn RemoteNameRequest(NSObject target);

        // -(IOReturn)remoteNameRequest:(id)target withPageTimeout:(BluetoothHCIPageTimeout)pageTimeoutValue;
        /// <summary>
        /// Issues a remote name request to the target device.
        /// </summary>
        /// <param name="target">The target to message when the remote name request is complete.</param>
        /// <param name="pageTimeoutValue">The page timeout value to use for this call.</param>
        /// <returns></returns>
        /// <remarks>If a target is specified, the request is asynchronous and on completion of the REMOTE_NAME_REQUEST command, the method <see cref="BluetoothDeviceAsyncCallbacks.RemoteNameRequestComplete"/> will be called on the specified target.
        /// If no target is specified, the request is made synchronously and won’t return until the request is complete.
        /// NOTE: This method is only available in macOS 10.2.7 (Bluetooth v1.3) or later.</remarks>
        [Export("remoteNameRequest:withPageTimeout:")]
        IOReturn RemoteNameRequest(NSObject target, ushort pageTimeoutValue);

        // -(IOReturn)requestAuthentication;
        /// <summary>
        /// Requests that the existing baseband connection be authenticated.
        /// </summary>
        /// <returns>Returns IOReturn.Success if the connection has been successfully been authenticated.
        /// Returns an error if authentication fails or no baseband connection exists.</returns>
        /// <remarks>In order to authenticate a baseband connection, a link key needs to be generated as a result of the pairing process.
        /// This call will synchronously initiate the pairing process with the target device and not return until the authentication process is complete.</remarks>
        [Export("requestAuthentication")]
        IOReturn RequestAuthentication();

        // @property (readonly, assign) BluetoothConnectionHandle connectionHandle;
        /// <summary>
        /// Get the connection handle for the baseband connection.
        /// </summary>
        /// <remarks>This method only returns a valid result if a baseband connection is present (IsConnected returns True).</remarks>
        [Export("connectionHandle")]
        ushort ConnectionHandle { get; }

        // -(BOOL)isIncoming;
        /// <summary>
        /// Returns True if the device connection was generated by the remote host.
        /// </summary>
        /// <value><c>true</c> if is incoming; otherwise, <c>false</c>.</value>
        [Export("isIncoming")]
        bool IsIncoming { get; }

        // -(BluetoothLinkType)getLinkType;
        /// <summary>
        /// Get the link type for the baseband connection.
        /// </summary>
        /// <value>Returns the link type for the baseband connection.
        /// If no baseband connection is present, BluetoothLinkType.None is returned.</value>
        [Export("getLinkType")]
        BluetoothLinkType LinkType { get; }

        // -(BluetoothHCIEncryptionMode)getEncryptionMode;
        /// <summary>
        /// Get the encryption mode for the baseband connection.
        /// </summary>
        /// <value>Returns the encryption mode for the baseband connection.
        /// If no baseband connection is present, HciEncryptionMode.Disabled is returned.</value>
        /// <remarks>This method only returns a valid result if a baseband connection is present (IsConnected returns True).</remarks>
        [Export("getEncryptionMode")]
        HciEncryptionMode EncryptionMode { get; }

        // -(IOReturn)performSDPQuery:(id)target;
        [Export("performSDPQuery:")]
        IOReturn PerformSdpQuery(NSObject target);

        // -(IOReturn)performSDPQuery:(id)target uuids:(NSArray *)uuidArray __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("performSDPQuery:uuids:")]
        IOReturn PerformSdpQuery(NSObject target, SdpUuid[] uuidArray);

        // @property (readonly, retain) NSArray * services;
        [Export("services", ArgumentSemantic.Retain)]
        SdpServiceRecord[] Services { get; }

        // -(NSDate *)getLastServicesUpdate;
        [Internal]
        [Export("getLastServicesUpdate")]
        NSDate? GetLastServicesUpdate();

        // -(IOBluetoothSDPServiceRecord *)getServiceRecordForUUID:(IOBluetoothSDPUUID *)sdpUUID;
        [Export("getServiceRecordForUUID:")]
        SdpServiceRecord GetServiceRecordForUuid(SdpUuid sdpUuid);

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
        /// <returns>Returns IOReturn.Success if the device was successfully added to the user’s list of favorite devices.</returns>
        [Export("addToFavorites")]
        IOReturn AddToFavorites();

        // -(IOReturn)removeFromFavorites;
        /// <summary>
        /// Removes the target device from the user's favorite devices list.
        /// </summary>
        /// <returns>Returns IOReturn.Success if the device was successfully removed from the user’s list of favorite devices.</returns>
        [Export("removeFromFavorites")]
        IOReturn RemoveFromFavorites();

        // +(NSArray *)recentDevices:(unsigned long)numDevices;
        /// <summary>
        /// Gets an array of recently used Bluetooth devices.
        /// </summary>
        /// <returns>Returns an array of device objects recently used by the system. 
        /// If no devices have been accessed, null is returned.</returns>
        /// <param name="numDevices">The number of devices to return.</param>
        [Static]
        [Export("recentDevices:")]  
        BluetoothDevice[]? GetRecentDevices(nuint numDevices);

        // -(NSDate *)recentAccessDate;
        [Internal]
        [Export("recentAccessDate")]
        NSDate? GetRecentAccessDate();

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
        /// <returns>Returns IOReturn.Success if it was possible to set the connection supervision timeout.</returns>
        /// <param name="timeout">A client-supplied link supervision timeout value to use to monitor the connection. 
        /// The timeout value should be specified in slots, so you can use the BluetoothGetSlotsFromSeconds macro to get the proper value. 
        /// e.g. BluetoothGetSlotsFromSeconds( 5.0 ) will give yield the proper number of slots (8000) for 5 seconds.</param>
        [Export("setSupervisionTimeout:")]
        IOReturn SetSupervisionTimeout(ushort timeout);

        // -(IOReturn)openL2CAPChannelSync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm withConfiguration:(NSDictionary *)channelConfiguration delegate:(id)channelDelegate;
        [Export("openL2CAPChannelSync:withPSM:withConfiguration:delegate:")]
        IOReturn OpenL2CAPChannelSync(out L2CapChannel newChannel, L2CapPsm psm, NSDictionary channelConfiguration, NSObject channelDelegate);

        // -(IOReturn)openL2CAPChannelAsync:(IOBluetoothL2CAPChannel **)newChannel withPSM:(BluetoothL2CAPPSM)psm withConfiguration:(NSDictionary *)channelConfiguration delegate:(id)channelDelegate;
        [Export("openL2CAPChannelAsync:withPSM:withConfiguration:delegate:")]
        IOReturn OpenL2CAPChannelAsync(out L2CapChannel newChannel, L2CapPsm psm, NSDictionary channelConfiguration, NSObject channelDelegate);

        // -(id)awakeAfterUsingCoder:(NSCoder *)coder __attribute__((ns_returns_retained)) __attribute__((ns_consumes_self));
        [Export("awakeAfterUsingCoder:")]
        NSObject AwakeAfterUsingCoder(NSCoder coder);
    }

    // @interface IOBluetoothDeviceInquiry : NSObject
    [BaseType(typeof(NSObject),
        Name = "IOBluetoothDeviceInquiry",
        Delegates = new string[] { "WeakDelegate" },
        Events = new Type[] { typeof(DeviceInquiryDelegate) })]
    public interface DeviceInquiry
    {
        [Wrap("WeakDelegate")]
        DeviceInquiryDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)inquiryWithDelegate:(id)delegate;
        [Static]
        [Export("inquiryWithDelegate:")]
        DeviceInquiry InquiryWithDelegate(DeviceInquiryDelegate @delegate);

        // -(instancetype)initWithDelegate:(id)delegate;
        [Export("initWithDelegate:")]
        NativeHandle Constructor(DeviceInquiryDelegate @delegate);

        // -(IOReturn)start;
        /// <summary>
        /// Tells inquiry object to begin the inquiry and name updating process, if specified.
        /// </summary>
        /// <returns>Returns IOReturn.Success if start was successful.
        /// Returns IOReturn.Busy if the object is already in process.
        /// May return other IOReturn values, as appropriate.</returns>
        [Export("start")]
        IOReturn Start();

        // -(IOReturn)stop;
        /// <summary>
        /// Halts the inquiry object.
        /// Could either stop the search for new devices, or the updating of found device names.
        /// </summary>
        /// <returns></returns>
        [Export("stop")]
        IOReturn Stop();

        // @property (assign) uint8_t inquiryLength;
        /// <summary>
        /// Set the length of the inquiry that is performed each time <see cref="Start"/> is used on an inquiry object.
        /// </summary>
        [Export("inquiryLength")]
        byte InquiryLength { get; set; }

        // @property (assign) IOBluetoothDeviceSearchTypes searchType;
        /// <summary>
        /// Set the devices that are found.
        /// </summary>
        [Export("searchType")]
        DeviceSearchType SearchType { get; set; }

        // @property (assign) BOOL updateNewDeviceNames;
        /// <summary>
        /// Sets whether or not the inquiry object will retrieve the names of devices found during the search.
        /// </summary>
        [Export("updateNewDeviceNames")]
        bool UpdateNewDeviceNames { get; set; }

        // -(NSArray *)foundDevices;
        /// <summary>
        /// Returns found BluetoothDevice objects as an array.
        /// </summary>
        [Export("foundDevices")]
        BluetoothDevice[]? FoundDevices { get; }

        // -(void)clearFoundDevices;
        /// <summary>
        /// Removes all found devices from the inquiry object.
        /// </summary>
        [Export("clearFoundDevices")]
        void ClearFoundDevices();

        // -(void)setSearchCriteria:(BluetoothServiceClassMajor)inServiceClassMajor majorDeviceClass:(BluetoothDeviceClassMajor)inMajorDeviceClass minorDeviceClass:(BluetoothDeviceClassMinor)inMinorDeviceClass;
        /// <summary>
        /// Use this method to set the criteria for the device search.
        /// </summary>
        /// <param name="serviceClassMajor">Set the major service class for found devices. Set to <see cref="ServiceClassMajor.Any"/> for all devices.</param>
        /// <param name="majorDeviceClass">Set the major device class for found devices. Set to <see cref="DeviceClassMajor.Any"/> for all devices.</param>
        /// <param name="minorDeviceClass">Set the minor device class for found devices. Set to <see cref="DeviceClassMinor.Any"/> for all devices.</param>
        [Export("setSearchCriteria:majorDeviceClass:minorDeviceClass:")]
        void SetSearchCriteria(ServiceClassMajor serviceClassMajor, DeviceClassMajor majorDeviceClass, DeviceClassMinor minorDeviceClass);
    }

    // @protocol IOBluetoothDeviceInquiryDelegate
    [Protocol]
    [BaseType(typeof(NSObject), Name = "IOBluetoothDeviceInquiryDelegate")]
    public interface DeviceInquiryDelegate
    {
        // @optional -(void)deviceInquiryStarted:(IOBluetoothDeviceInquiry *)sender;
        [Export("deviceInquiryStarted:")]
        void DeviceInquiryStarted(DeviceInquiry sender);

        // @optional -(void)deviceInquiryDeviceFound:(IOBluetoothDeviceInquiry *)sender device:(IOBluetoothDevice *)device;
        [Export("deviceInquiryDeviceFound:device:"), EventArgs("DeviceFound")]
        void DeviceFound(DeviceInquiry sender, BluetoothDevice device);

        // @optional -(void)deviceInquiryUpdatingDeviceNamesStarted:(IOBluetoothDeviceInquiry *)sender devicesRemaining:(uint32_t)devicesRemaining;
        [Export("deviceInquiryUpdatingDeviceNamesStarted:devicesRemaining:"), EventArgs("UpdatingDeviceNamesStarted")]
        void UpdatingDeviceNamesStarted(DeviceInquiry sender, uint devicesRemaining);

        // @optional -(void)deviceInquiryDeviceNameUpdated:(IOBluetoothDeviceInquiry *)sender device:(IOBluetoothDevice *)device devicesRemaining:(uint32_t)devicesRemaining;
        [Export("deviceInquiryDeviceNameUpdated:device:devicesRemaining:"), EventArgs("DeviceNameUpdated")]
        void DeviceNameUpdated(DeviceInquiry sender, BluetoothDevice device, uint devicesRemaining);

        // @optional -(void)deviceInquiryComplete:(IOBluetoothDeviceInquiry *)sender error:(IOReturn)error aborted:(BOOL)aborted;
        [Export("deviceInquiryComplete:error:aborted:"), EventName("Completed"), EventArgs("DeviceInquiryCompleted")]
        void Complete(DeviceInquiry sender, int error, bool aborted);
    }

    // @interface IOBluetoothDevicePair : NSObject
    [BaseType(typeof(NSObject),
        Name = "IOBluetoothDevicePair",
        Delegates = new string[] { "WeakDelegate" },
        Events = new Type[] { typeof(DevicePairDelegate) })]
    public interface DevicePair
    {
        [Wrap("WeakDelegate")]
        DevicePairDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)pairWithDevice:(IOBluetoothDevice *)device;
        [Static]
        [Export("pairWithDevice:")]
        DevicePair PairWithDevice(BluetoothDevice device);

        // -(IOReturn)start;
        [Export("start")]
        IOReturn Start();

        // -(void)stop;
        [Export("stop")]
        void Stop();

        // -(IOBluetoothDevice *)device;
        // -(void)setDevice:(IOBluetoothDevice *)inDevice;
        [Export("device")]
        BluetoothDevice Device { get; set; }

        // -(void)replyPINCode:(ByteCount)PINCodeSize PINCode:(BluetoothPINCode *)PINCode;
        [Export("replyPINCode:PINCode:")]
        void ReplyPinCode(nuint pinCodeSize, NSArray pinCode);

        // -(void)replyUserConfirmation:(BOOL)reply;
        [Export("replyUserConfirmation:")]
        void ReplyUserConfirmation(bool reply);
    }

    // @protocol IOBluetoothDevicePairDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject), Name = "IOBluetoothDevicePairDelegate")]
    public interface DevicePairDelegate
    {
        // @optional -(void)devicePairingStarted:(id)sender;
        [Export("devicePairingStarted:")]
        void PairingStarted(NSObject sender);

        // @optional -(void)devicePairingConnecting:(id)sender;
        [Export("devicePairingConnecting:")]
        void PairingConnecting(NSObject sender);

        // @optional -(void)devicePairingPINCodeRequest:(id)sender;
        [Export("devicePairingPINCodeRequest:")]
        void PairingPinCodeRequest(NSObject sender);

        // @optional -(void)devicePairingUserConfirmationRequest:(id)sender numericValue:(BluetoothNumericValue)numericValue;
        [Export("devicePairingUserConfirmationRequest:numericValue:"), EventArgs("PairingUserConfirmationRequest")]
        void PairingUserConfirmationRequest(NSObject sender, uint numericValue);

        // @optional -(void)devicePairingUserPasskeyNotification:(id)sender passkey:(BluetoothPasskey)passkey;
        [Export("devicePairingUserPasskeyNotification:passkey:"), EventArgs("PairingUserPasskeyNotification")]
        void PairingUserPasskeyNotification(NSObject sender, uint passkey);

        // @optional -(void)devicePairingFinished:(id)sender error:(IOReturn)error;
        [Export("devicePairingFinished:error:"), EventArgs("PairingFinished")]
        void PairingFinished(NSObject sender, int error);

        // @optional -(void)deviceSimplePairingComplete:(id)sender status:(BluetoothHCIEventStatus)status;
        [Export("deviceSimplePairingComplete:status:"), EventName("Completed"), EventArgs("SimplePairingCompleted")]
        void SimplePairingComplete(NSObject sender, byte status);
    }

    // @interface IOBluetoothHostController : NSObject
    [BaseType(typeof(NSObject), Name = "IOBluetoothHostController",
        Delegates = new string[] { "WeakDelegate" },
        Events = new Type[] { typeof(HostControllerDelegate) })]
    public interface HostController
    {
        [Wrap("WeakDelegate")]
        HostControllerDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)defaultController;
        [Static]
        [Export("defaultController")]
        HostController DefaultController { get; }

        // @property (readonly) BluetoothHCIPowerState powerState;
        [Export("powerState")]
        HciPowerState PowerState { get; }

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

        // extern NSString *const IOBluetoothHostControllerPoweredOnNotification;
        [Field("IOBluetoothHostControllerPoweredOnNotification", LibraryName = "__Internal")]
        NSString PoweredOnNotification { get; }

        // extern NSString *const IOBluetoothHostControllerPoweredOffNotification;
        [Field("IOBluetoothHostControllerPoweredOffNotification", LibraryName = "__Internal")]
        NSString PoweredOffNotification { get; }

    }

    // @interface IOBluetoothHostControllerDelegate (NSObject)
    [Protocol]
    [BaseType(typeof(NSObject), Name = "IOBluetoothHostControllerDelegate")]
    public interface HostControllerDelegate
    {
        // -(void)readRSSIForDeviceComplete:(id)controller device:(IOBluetoothDevice *)device info:(BluetoothHCIRSSIInfo *)info error:(IOReturn)error;
        [Export("readRSSIForDeviceComplete:device:info:error:"), EventName("ReadRssiForDeviceCompleted"), EventArgs("RssiForDevice")]
        unsafe void ReadRssiForDeviceComplete(NSObject controller, BluetoothDevice device, HciRssiInfo info, int error);

        // -(void)readLinkQualityForDeviceComplete:(id)controller device:(IOBluetoothDevice *)device info:(BluetoothHCILinkQualityInfo *)info error:(IOReturn)error;
        [Export("readLinkQualityForDeviceComplete:device:info:error:"), EventArgs("LinkQualityForDevice")]
        unsafe void ReadLinkQualityForDeviceComplete(NSObject controller, BluetoothDevice device, HciLinkQualityInfo info, int error);
    }


    // @interface IOBluetoothL2CAPChannel : IOBluetoothObject <NSPortDelegate>
    [BaseType(typeof(BluetoothObject), Name = "IOBluetoothL2CAPChannel",
        Delegates = new string[] { "WeakDelegate" },
        Events = new Type[] { typeof(L2CapChannelDelegate) })]
    public interface L2CapChannel : INSPortDelegate
    {
        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector);

        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector withPSM:(BluetoothL2CAPPSM)psm direction:(IOBluetoothUserNotificationChannelDirection)inDirection;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:withPSM:direction:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector, ushort psm, uint direction);

        // +(instancetype)withObjectID:(IOBluetoothObjectID)objectID;
        [Static]
        [Export("withObjectID:")]
        L2CapChannel WithObjectID(nuint objectID);

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
        int RequestRemoteMtu(ushort remoteMtu);

        // -(IOReturn)writeAsync:(void *)data length:(UInt16)length refcon:(void *)refcon;
        [Export("writeAsync:length:refcon:")]
        int WriteAsync(IntPtr data, ushort length, IntPtr refcon);

        // -(IOReturn)writeSync:(void *)data length:(UInt16)length;
        [Export("writeSync:length:")]
        int WriteSync(IntPtr data, ushort length);

        // -(IOReturn)setDelegate:(id)channelDelegate;
        [Export("setDelegate:")]
        int SetDelegate(L2CapChannelDelegate channelDelegate);

        // -(IOReturn)setDelegate:(id)channelDelegate withConfiguration:(NSDictionary *)channelConfiguration;
        [Export("setDelegate:withConfiguration:")]
        int SetDelegate(L2CapChannelDelegate channelDelegate, NSDictionary channelConfiguration);

        [Wrap("WeakDelegate")]
        L2CapChannelDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

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
        UserNotification RegisterForChannelCloseNotification(NSObject observer, Selector selector);

        // extern NSString *const IOBluetoothL2CAPChannelPublishedNotification;
        [Field("IOBluetoothL2CAPChannelPublishedNotification", LibraryName = "__Internal")]
        NSString PublishedNotification { get; }

        // extern NSString *const IOBluetoothL2CAPChannelTerminatedNotification;
        [Field("IOBluetoothL2CAPChannelTerminatedNotification", LibraryName = "__Internal")]
        NSString TerminatedNotification { get; }
    }

    // @protocol IOBluetoothL2CAPChannelDelegate
    [Protocol,Model]
    [BaseType(typeof(NSObject), Name = "IOBluetoothL2CAPChannelDelegate")]
    public interface L2CapChannelDelegate
    {
        // @optional -(void)l2capChannelData:(IOBluetoothL2CAPChannel *)l2capChannel data:(void *)dataPointer length:(size_t)dataLength;
        [Export("l2capChannelData:data:length:"), EventArgs("L2CapChannelData")]
        void L2CapChannelData(L2CapChannel l2capChannel, IntPtr dataPointer, nuint dataLength);

        // @optional -(void)l2capChannelOpenComplete:(IOBluetoothL2CAPChannel *)l2capChannel status:(IOReturn)error;
        [Export("l2capChannelOpenComplete:status:"), EventName("OpenCompleted"), EventArgs("L2CapChannelOpenCompleted")]
        void L2CapChannelOpenComplete(L2CapChannel l2capChannel, int error);

        // @optional -(void)l2capChannelClosed:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelClosed:"), EventName("Closed")]
        void L2CapChannelClosed(L2CapChannel l2capChannel);

        // @optional -(void)l2capChannelReconfigured:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelReconfigured:"), EventName("Reconfigured")]
        void L2CapChannelReconfigured(L2CapChannel l2capChannel);

        // @optional -(void)l2capChannelWriteComplete:(IOBluetoothL2CAPChannel *)l2capChannel refcon:(void *)refcon status:(IOReturn)error;
        [Export("l2capChannelWriteComplete:refcon:status:"), EventName("WriteCompleted"), EventArgs("L2CapChannelWriteCompleted")]
        void L2CapChannelWriteComplete(L2CapChannel l2capChannel, IntPtr refcon, int error);

        // @optional -(void)l2capChannelQueueSpaceAvailable:(IOBluetoothL2CAPChannel *)l2capChannel;
        [Export("l2capChannelQueueSpaceAvailable:"), EventName("QueueSpaceAvailable")]
        void L2CapChannelQueueSpaceAvailable(L2CapChannel l2capChannel);
    }

    // @interface IOBluetoothRFCOMMChannel : IOBluetoothObject <NSPortDelegate>
    [BaseType(typeof(BluetoothObject), Name = "IOBluetoothRFCOMMChannel",
        Delegates = new string[] { "Delegate" },
        Events = new Type[] { typeof(RfcommChannelDelegate) })]
    public interface RfcommChannel : INSPortDelegate
    {
        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector);

        // +(IOBluetoothUserNotification *)registerForChannelOpenNotifications:(id)object selector:(SEL)selector withChannelID:(BluetoothRFCOMMChannelID)channelID direction:(IOBluetoothUserNotificationChannelDirection)inDirection;
        [Static]
        [Export("registerForChannelOpenNotifications:selector:withChannelID:direction:")]
        UserNotification RegisterForChannelOpenNotifications(NSObject @object, Selector selector, byte channelID, uint direction);

        // +(instancetype)withRFCOMMChannelRef:(IOBluetoothRFCOMMChannelRef)rfcommChannelRef;
        //[Static]
        //[Export ("withRFCOMMChannelRef:")]
        //unsafe IOBluetoothRFCOMMChannel WithRFCOMMChannelRef (IOBluetoothRFCOMMChannelRef* rfcommChannelRef);

        // +(instancetype)withObjectID:(IOBluetoothObjectID)objectID;
        [Static]
        [Export("withObjectID:")]
        RfcommChannel WithObjectID(nuint objectID);

        // -(IOBluetoothRFCOMMChannelRef)getRFCOMMChannelRef;
        //[Export ("getRFCOMMChannelRef")]
        //[Verify (MethodToProperty)]
        //unsafe IOBluetoothRFCOMMChannelRef* RFCOMMChannelRef { get; }

        // -(IOReturn)closeChannel;
        [Export("closeChannel")]
        IOReturn CloseChannel();

        // -(BOOL)isOpen;
        [Export("isOpen")]
        bool IsOpen { get; }

        // -(BluetoothRFCOMMMTU)getMTU;
        [Export("getMTU")]
        ushort Mtu { get; }

        // -(BOOL)isTransmissionPaused;
        [Export("isTransmissionPaused")]
        bool IsTransmissionPaused { get; }

        // -(IOReturn)writeAsync:(void *)data length:(UInt16)length refcon:(void *)refcon;
        [Export("writeAsync:length:refcon:")]
        IOReturn WriteAsync(IntPtr data, ushort length, IntPtr refcon);

        // -(IOReturn)writeSync:(void *)data length:(UInt16)length;
        [Export("writeSync:length:")]
        IOReturn WriteSync(IntPtr data, ushort length);

        // -(IOReturn)setSerialParameters:(UInt32)speed dataBits:(UInt8)nBits parity:(BluetoothRFCOMMParityType)parity stopBits:(UInt8)bitStop;
        [Export("setSerialParameters:dataBits:parity:stopBits:")]
        IOReturn SetSerialParameters(uint speed, byte bits, RfcommParityType parity, byte bitStop);

        // -(IOReturn)sendRemoteLineStatus:(BluetoothRFCOMMLineStatus)lineStatus;
        [Export("sendRemoteLineStatus:")]
        IOReturn SendRemoteLineStatus(RfcommLineStatus lineStatus);


        [Wrap("WeakDelegate")]
        RfcommChannelDelegate Delegate { get; set; }

        // -(IOReturn)setDelegate:(id)delegate;
        [Internal]
        [Export("setDelegate:")]
        IOReturn SetDelegate(NSObject @delegate);

        // -(id)delegate;
        [Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; [Bind("setDelegate:")] set; }

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
        UserNotification RegisterForChannelCloseNotification(NSObject observer, Selector selector);
    }

    // @protocol IOBluetoothRFCOMMChannelDelegate
    [Protocol,Model]
    [BaseType(typeof(NSObject), Name = "IOBluetoothRFCOMMChannelDelegate")]
    public interface RfcommChannelDelegate
    {
        // @optional -(void)rfcommChannelData:(IOBluetoothRFCOMMChannel *)rfcommChannel data:(void *)dataPointer length:(size_t)dataLength;
        [Export("rfcommChannelData:data:length:"), EventName("Data"), EventArgs("RfcommChannelData")]
        void RfcommChannelData(RfcommChannel rfcommChannel, IntPtr dataPointer, nuint dataLength);

        // @optional -(void)rfcommChannelOpenComplete:(IOBluetoothRFCOMMChannel *)rfcommChannel status:(IOReturn)error;
        [Export("rfcommChannelOpenComplete:status:"), EventName("OpenCompleted"), EventArgs("RfcommChannelOpenCompleted")]
        void RfcommChannelOpenComplete(RfcommChannel rfcommChannel, IOReturn error);

        // @optional -(void)rfcommChannelClosed:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelClosed:"), EventName("Closed")]
        void RfcommChannelClosed(RfcommChannel rfcommChannel);

        // @optional -(void)rfcommChannelControlSignalsChanged:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelControlSignalsChanged:"), EventName("SignalsChanged")]
        void RfcommChannelControlSignalsChanged(RfcommChannel rfcommChannel);

        // @optional -(void)rfcommChannelFlowControlChanged:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelFlowControlChanged:"), EventName("FlowControlChanged")]
        void RfcommChannelFlowControlChanged(RfcommChannel rfcommChannel);

        // @optional -(void)rfcommChannelWriteComplete:(IOBluetoothRFCOMMChannel *)rfcommChannel refcon:(void *)refcon status:(IOReturn)error;
        [Export("rfcommChannelWriteComplete:refcon:status:"), EventName("WriteCompleted"), EventArgs("RfcommChannelWriteCompleted")]
        void RfcommChannelWriteComplete(RfcommChannel rfcommChannel, IntPtr refcon, IOReturn error);

        // @optional -(void)rfcommChannelQueueSpaceAvailable:(IOBluetoothRFCOMMChannel *)rfcommChannel;
        [Export("rfcommChannelQueueSpaceAvailable:"), EventName("QueueSpaceAvailable")]
        void RfcommChannelQueueSpaceAvailable(RfcommChannel rfcommChannel);
    }

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
}