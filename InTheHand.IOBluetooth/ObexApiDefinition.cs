using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace IOBluetooth
{
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
    [BaseType(typeof(NSObject), Name = "OBEXSession")]
    interface ObexSession
    {
        // -(OBEXError)OBEXConnect:(OBEXFlags)inFlags maxPacketLength:(OBEXMaxPacketLength)inMaxPacketLength optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXConnect:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexConnect(byte flags, ushort maxPacketLength, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXDisconnect:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXDisconnect:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexDisconnect(IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXPut:(Boolean)isFinalChunk headersData:(void *)inHeadersData headersDataLength:(size_t)inHeadersDataLength bodyData:(void *)inBodyData bodyDataLength:(size_t)inBodyDataLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXPut:headersData:headersDataLength:bodyData:bodyDataLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexPut(byte isFinalChunk, IntPtr headersData, nuint headersDataLength, IntPtr bodyData, nuint bodyDataLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXGet:(Boolean)isFinalChunk headers:(void *)inHeaders headersLength:(size_t)inHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXGet:headers:headersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexGet(byte isFinalChunk, IntPtr headers, nuint headersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXAbort:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXAbort:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexAbort(IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXSetPath:(OBEXFlags)inFlags constants:(OBEXConstants)inConstants optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXSetPath:constants:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexSetPath(byte flags, byte constants, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXConnectResponse:(OBEXOpCode)inResponseOpCode flags:(OBEXFlags)inFlags maxPacketLength:(OBEXMaxPacketLength)inMaxPacketLength optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXConnectResponse:flags:maxPacketLength:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexConnectResponse(byte responseOpCode, byte flags, ushort maxPacketLength, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXDisconnectResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXDisconnectResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexDisconnectResponse(byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXPutResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXPutResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexPutResponse(byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXGetResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXGetResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexGetResponse(byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXAbortResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXAbortResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexAbortResponse(byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXError)OBEXSetPathResponse:(OBEXOpCode)inResponseOpCode optionalHeaders:(void *)inOptionalHeaders optionalHeadersLength:(size_t)inOptionalHeadersLength eventSelector:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("OBEXSetPathResponse:optionalHeaders:optionalHeadersLength:eventSelector:selectorTarget:refCon:")]
        ObexError ObexSetPathResponse(byte responseOpCode, IntPtr optionalHeaders, nuint optionalHeadersLength, Selector selector, NSObject target, IntPtr userRefCon);

        // -(OBEXMaxPacketLength)getAvailableCommandPayloadLength:(OBEXOpCode)inOpCode;
        [Export("getAvailableCommandPayloadLength:")]
        ushort GetAvailableCommandPayloadLength(byte opCode);

        // -(OBEXMaxPacketLength)getAvailableCommandResponsePayloadLength:(OBEXOpCode)inOpCode;
        [Export("getAvailableCommandResponsePayloadLength:")]
        ushort GetAvailableCommandResponsePayloadLength(byte opCode);

        // -(OBEXMaxPacketLength)getMaxPacketLength;
        [Export("getMaxPacketLength")]
        ushort MaxPacketLength { get; }

        // -(BOOL)hasOpenOBEXConnection;
        [Export("hasOpenOBEXConnection")]
        bool HasOpenObexConnection { get; }

    //    // -(void)setEventCallback:(OBEXSessionEventCallback)inEventCallback;
    //    //[Export ("setEventCallback:")]
    //    //unsafe void SetEventCallback (OBEXSessionEventCallback* inEventCallback);

    //    // -(void)setEventRefCon:(void *)inRefCon;
    //    [Export("setEventRefCon:")]
    //    void SetEventRefCon(IntPtr inRefCon);

        // -(void)setEventSelector:(SEL)inEventSelector target:(id)inEventSelectorTarget refCon:(id)inUserRefCon;
        [Export("setEventSelector:target:refCon:")]
        void SetEventSelector(Selector eventSelector, NSObject eventSelectorTarget, NSObject userRefCon);

    //    //// -(void)serverHandleIncomingData:(OBEXTransportEvent *)event;
    //    //[Export ("serverHandleIncomingData:")]
    //    //unsafe void ServerHandleIncomingData (OBEXTransportEvent* @event);

    //    //// -(void)clientHandleIncomingData:(OBEXTransportEvent *)event;
    //    //[Export ("clientHandleIncomingData:")]
    //    //unsafe void ClientHandleIncomingData (OBEXTransportEvent* @event);

        // -(OBEXError)sendDataToTransport:(void *)inDataToSend dataLength:(size_t)inDataLength;
        [Export("sendDataToTransport:dataLength:")]
        ObexError SendDataToTransport(IntPtr dataToSend, nuint dataLength);

        // -(OBEXError)openTransportConnection:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("openTransportConnection:selectorTarget:refCon:")]
        ObexError OpenTransportConnection(Selector selector, NSObject target, IntPtr userRefCon);

        // -(Boolean)hasOpenTransportConnection;
        [Export("hasOpenTransportConnection")]
        bool HasOpenTransportConnection { get; }

        // -(OBEXError)closeTransportConnection;
        [Export("closeTransportConnection")]
        ObexError CloseTransportConnection();
    }

    // @interface IOBluetoothOBEXSession : OBEXSession <IOBluetoothRFCOMMChannelDelegate>
    [BaseType(typeof(ObexSession), Name = "IOBluetoothOBEXSession")]
    public interface BluetoothObexSession : RfcommChannelDelegate
    {
        // +(instancetype)withSDPServiceRecord:(IOBluetoothSDPServiceRecord *)inSDPServiceRecord;
        [Static]
        [Export("withSDPServiceRecord:")]
        BluetoothObexSession WithSDPServiceRecord(SdpServiceRecord sdpServiceRecord);

        // +(instancetype)withDevice:(IOBluetoothDevice *)inDevice channelID:(BluetoothRFCOMMChannelID)inRFCOMMChannelID;
        [Static]
        [Export("withDevice:channelID:")]
        BluetoothObexSession WithDevice(BluetoothDevice device, byte rfcommChannelID);

        // +(instancetype)withIncomingRFCOMMChannel:(IOBluetoothRFCOMMChannel *)inChannel eventSelector:(SEL)inEventSelector selectorTarget:(id)inEventSelectorTarget refCon:(void *)inUserRefCon;
        [Static]
        [Export("withIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
        BluetoothObexSession WithIncomingRFCOMMChannel(RfcommChannel channel, Selector eventSelector, NSObject eventSelectorTarget, IntPtr userRefCon);

        // -(instancetype)initWithSDPServiceRecord:(IOBluetoothSDPServiceRecord *)inSDPServiceRecord;
        [Export("initWithSDPServiceRecord:")]
        NativeHandle Constructor(SdpServiceRecord sdpServiceRecord);

        // -(instancetype)initWithDevice:(IOBluetoothDevice *)inDevice channelID:(BluetoothRFCOMMChannelID)inChannelID;
        [Export("initWithDevice:channelID:")]
        NativeHandle Constructor(BluetoothDevice device, byte channelID);

        // -(instancetype)initWithIncomingRFCOMMChannel:(IOBluetoothRFCOMMChannel *)inChannel eventSelector:(SEL)inEventSelector selectorTarget:(id)inEventSelectorTarget refCon:(void *)inUserRefCon;
        [Export("initWithIncomingRFCOMMChannel:eventSelector:selectorTarget:refCon:")]
        NativeHandle Constructor(RfcommChannel channel, Selector eventSelector, NSObject eventSelectorTarget, IntPtr userRefCon);

        // -(IOBluetoothRFCOMMChannel *)getRFCOMMChannel;
        [Export("getRFCOMMChannel")]
        RfcommChannel RFCommChannel { get; }

        // -(IOBluetoothDevice *)getDevice;
        [Export("getDevice")]
        BluetoothDevice Device { get; }

        // -(IOReturn)sendBufferTroughChannel;
        [Export("sendBufferTroughChannel")]
        IOReturn SendBufferTroughChannel();

        // -(void)restartTransmission;
        [Export("restartTransmission")]
        void RestartTransmission();

        // -(BOOL)isSessionTargetAMac;
        [Export("isSessionTargetAMac")]
        bool IsSessionTargetAMac { get; }

        // -(OBEXError)openTransportConnection:(SEL)inSelector selectorTarget:(id)inTarget refCon:(void *)inUserRefCon;
        [Export("openTransportConnection:selectorTarget:refCon:")]
        [Override]
        ObexError OpenTransportConnection(Selector selector, NSObject target, IntPtr userRefCon);

        // -(BOOL)hasOpenTransportConnection;
        [Export("hasOpenTransportConnection")]
        [New]
        bool HasOpenTransportConnection { get; }

        // -(OBEXError)closeTransportConnection;
        [Export("closeTransportConnection")]
        [New]
        ObexError CloseTransportConnection();

        // -(OBEXError)sendDataToTransport:(void *)inDataToSend dataLength:(size_t)inDataLength;
        [Export("sendDataToTransport:dataLength:")]
        [Override]
        ObexError SendDataToTransport(IntPtr dataToSend, nuint dataLength);

        // -(void)setOpenTransportConnectionAsyncSelector:(SEL)inSelector target:(id)inSelectorTarget refCon:(id)inUserRefCon;
        [Export("setOpenTransportConnectionAsyncSelector:target:refCon:")]
        void SetOpenTransportConnectionAsyncSelector(Selector selector, NSObject selectorTarget, NSObject userRefCon);

    //    // -(void)setOBEXSessionOpenConnectionCallback:(IOBluetoothOBEXSessionOpenConnectionCallback)inCallback refCon:(void *)inUserRefCon;
    //    //[Export ("setOBEXSessionOpenConnectionCallback:refCon:")]
    //    //unsafe void SetOBEXSessionOpenConnectionCallback (IOBluetoothOBEXSessionOpenConnectionCallback* inCallback, void* inUserRefCon);
    }

    // @interface OBEXFileTransferServices : NSObject
    [BaseType(typeof(NSObject), Name = "OBEXFileTransferServices",
        Delegates = new string[] { "WeakDelegate" },
        Events = new Type[] { typeof(ObexFileTransferServicesDelegate) })]
    interface ObexFileTransferServices
    {
        [Wrap("WeakDelegate")]
        ObexFileTransferServicesDelegate Delegate { get; set; }

        // @property (assign) id delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Assign)]
        NSObject WeakDelegate { get; set; }

        // +(instancetype)withOBEXSession:(IOBluetoothOBEXSession *)inOBEXSession;
        [Static]
        [Export("withOBEXSession:")]
        ObexFileTransferServices WithOBEXSession(BluetoothObexSession obexSession);

        // -(instancetype)initWithOBEXSession:(IOBluetoothOBEXSession *)inOBEXSession;
        [Export("initWithOBEXSession:")]
        NativeHandle Constructor(BluetoothObexSession obexSession);

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
        ObexError ConnectToFTPService();

        // -(OBEXError)connectToObjectPushService;
        [Export("connectToObjectPushService")]
        ObexError ConnectToObjectPushService();

        // -(OBEXError)disconnect;
        [Export("disconnect")]
        ObexError Disconnect();

        // -(OBEXError)changeCurrentFolderToRoot;
        [Export("changeCurrentFolderToRoot")]
        ObexError ChangeCurrentFolderToRoot();

        // -(OBEXError)changeCurrentFolderBackward;
        [Export("changeCurrentFolderBackward")]
        ObexError ChangeCurrentFolderBackward();

        // -(OBEXError)changeCurrentFolderForwardToPath:(NSString *)inDirName;
        [Export("changeCurrentFolderForwardToPath:")]
        ObexError ChangeCurrentFolderForwardToPath(string directoryName);

        // -(OBEXError)createFolder:(NSString *)inDirName;
        [Export("createFolder:")]
        ObexError CreateFolder(string directoryName);

        // -(OBEXError)removeItem:(NSString *)inItemName;
        [Export("removeItem:")]
        ObexError RemoveItem(string itemName);

        // -(OBEXError)retrieveFolderListing;
        [Export("retrieveFolderListing")]
        ObexError RetrieveFolderListing();

        // -(OBEXError)sendFile:(NSString *)inLocalPathAndName;
        [Export("sendFile:")]
        ObexError SendFile(string localPathAndName);

        // -(OBEXError)copyRemoteFile:(NSString *)inRemoteFileName toLocalPath:(NSString *)inLocalPathAndName;
        [Export("copyRemoteFile:toLocalPath:")]
        ObexError CopyRemoteFile(string remoteFileName, string localPathAndName);

        // -(OBEXError)sendData:(NSData *)inData type:(NSString *)inType name:(NSString *)inName;
        [Export("sendData:type:name:")]
        ObexError SendData(NSData data, string type, string name);

        // -(OBEXError)getDefaultVCard:(NSString *)inLocalPathAndName;
        [Export("getDefaultVCard:")]
        ObexError GetDefaultVCard(string localPathAndName);

        // -(OBEXError)abort;
        [Export("abort")]
        ObexError Abort();
    }

    // @interface OBEXFileTransferServicesDelegate (NSObject)
    [BaseType (typeof(NSObject), Name = "OBEXFileTransferServicesDelegate")]
    [Protocol]
    interface ObexFileTransferServicesDelegate
    {
    	// -(void)fileTransferServicesConnectionComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesConnectionComplete:error:"), EventName("ConnectionCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesConnectionComplete (ObexFileTransferServices services, ObexError error);

    	// -(void)fileTransferServicesDisconnectionComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesDisconnectionComplete:error:"), EventName("DisconnectionCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesDisconnectionComplete (ObexFileTransferServices services, ObexError error);

    	// -(void)fileTransferServicesAbortComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesAbortComplete:error:"), EventName("AbortCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesAbortComplete (ObexFileTransferServices services, ObexError error);

    	// -(void)fileTransferServicesRemoveItemComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError removedItem:(NSString *)inItemName;
    	[Export ("fileTransferServicesRemoveItemComplete:error:removedItem:"), EventName("RemoveItemCompleted"), EventArgs("FileTransferServicesItem")]
    	void FileTransferServicesRemoveItemComplete (ObexFileTransferServices services, ObexError error, string itemName);

    	// -(void)fileTransferServicesCreateFolderComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError folder:(NSString *)inFolderName;
    	[Export ("fileTransferServicesCreateFolderComplete:error:folder:"), EventName("CreateFolderCompleted"), EventArgs("FileTransferServicesFolder")]
    	void FileTransferServicesCreateFolderComplete (ObexFileTransferServices services, ObexError error, string folderName);

    	// -(void)fileTransferServicesPathChangeComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError finalPath:(NSString *)inPath;
    	[Export ("fileTransferServicesPathChangeComplete:error:finalPath:"), EventName("PathChangeCompleted"), EventArgs("FileTransferServicesPath")]
    	void FileTransferServicesPathChangeComplete (ObexFileTransferServices services, ObexError error, string path);

    	// -(void)fileTransferServicesRetrieveFolderListingComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError listing:(NSArray *)inListing;
    	[Export ("fileTransferServicesRetrieveFolderListingComplete:error:listing:"), EventName("RetrieveFolderListingCompleted"), EventArgs("FolderListing")]
    	void FileTransferServicesRetrieveFolderListingComplete (ObexFileTransferServices services, ObexError error, NSString[] listing);

    	// -(void)fileTransferServicesFilePreparationComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesFilePreparationComplete:error:"), EventName("FilePreparationCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesFilePreparationComplete (ObexFileTransferServices services, ObexError error);

    	// -(void)fileTransferServicesSendFileProgress:(OBEXFileTransferServices *)inServices transferProgress:(NSDictionary *)inProgressDescription;
    	[Export ("fileTransferServicesSendFileProgress:transferProgress:"), EventName("SendFileProgress"), EventArgs("FileTransferServicesProgress")]
    	void FileTransferServicesSendFileProgress (ObexFileTransferServices services, NSDictionary progressDescription);

    	// -(void)fileTransferServicesSendFileComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesSendFileComplete:error:"), EventName("SendFileCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesSendFileComplete (ObexFileTransferServices services, ObexError error);

    	// -(void)fileTransferServicesCopyRemoteFileProgress:(OBEXFileTransferServices *)inServices transferProgress:(NSDictionary *)inProgressDescription;
    	[Export ("fileTransferServicesCopyRemoteFileProgress:transferProgress:"), EventName("CopyRemoteFileProgress"), EventArgs("FileTransferServicesProgress")]
    	void FileTransferServicesCopyRemoteFileProgress (ObexFileTransferServices services, NSDictionary progressDescription);

    	// -(void)fileTransferServicesCopyRemoteFileComplete:(OBEXFileTransferServices *)inServices error:(OBEXError)inError;
    	[Export ("fileTransferServicesCopyRemoteFileComplete:error:"), EventName("CopyRemoteFileCompleted"), EventArgs("FileTransferServices")]
    	void FileTransferServicesCopyRemoteFileComplete (ObexFileTransferServices services, ObexError error);
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
    [Category]
    [BaseType (typeof(NSMutableDictionary), Name = "NSDictionaryOBEXExtensions")]
    interface NSDictionaryObexExtensions
    {
        //	//// +(instancetype)dictionaryWithOBEXHeadersData:(const void *)inHeadersData headersDataSize:(size_t)inDataSize;
        //	[Static]
        //	[Export ("dictionaryWithOBEXHeadersData:headersDataSize:")]
        //	unsafe NSMutableDictionary DictionaryWithOBEXHeadersData (IntPtr inHeadersData, nuint inDataSize);

        //	// +(instancetype)dictionaryWithOBEXHeadersData:(NSData *)inHeadersData;
        //	[Static]
        //	[Export ("dictionaryWithOBEXHeadersData:")]
        //	NSMutableDictionary DictionaryWithOBEXHeadersData (NSData inHeadersData);

        //	// -(NSMutableData *)getHeaderBytes;
        [Export("getHeaderBytes")]
        NSMutableData GetHeaderBytes();

    	// -(OBEXError)addTargetHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    	[Export ("addTargetHeader:length:")]
    	ObexError AddTargetHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addHTTPHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addHTTPHeader:length:")]
    //	ObexError AddHTTPHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addBodyHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength endOfBody:(BOOL)isEndOfBody;
    //	[Export ("addBodyHeader:length:endOfBody:")]
    //	ObexError AddBodyHeader (IntPtr inHeaderData, uint inHeaderDataLength, bool isEndOfBody);

    //	// -(OBEXError)addWhoHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addWhoHeader:length:")]
    //	ObexError AddWhoHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addConnectionIDHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addConnectionIDHeader:length:")]
    //	ObexError AddConnectionIDHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addApplicationParameterHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addApplicationParameterHeader:length:")]
    //	ObexError AddApplicationParameterHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addByteSequenceHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addByteSequenceHeader:length:")]
    //	ObexError AddByteSequenceHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addObjectClassHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addObjectClassHeader:length:")]
    //	ObexError AddObjectClassHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addAuthorizationChallengeHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addAuthorizationChallengeHeader:length:")]
    //	ObexError AddAuthorizationChallengeHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addAuthorizationResponseHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addAuthorizationResponseHeader:length:")]
    //	ObexError AddAuthorizationResponseHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addTimeISOHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addTimeISOHeader:length:")]
    //	ObexError AddTimeISOHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addTypeHeader:(NSString *)type;
    	[Export ("addTypeHeader:")]
    	ObexError AddTypeHeader (string type);

    //	// -(OBEXError)addLengthHeader:(uint32_t)length;
    	[Export ("addLengthHeader:")]
	    ObexError AddLengthHeader (uint length);

    //	// -(OBEXError)addTime4ByteHeader:(uint32_t)time4Byte;
    	[Export ("addTime4ByteHeader:")]
	    ObexError AddTime4ByteHeader (uint time4Byte);

    //	// -(OBEXError)addCountHeader:(uint32_t)inCount;
    	[Export ("addCountHeader:")]
	    ObexError AddCountHeader (uint inCount);

    //	// -(OBEXError)addDescriptionHeader:(NSString *)inDescriptionString;
    	[Export ("addDescriptionHeader:")]
	    ObexError AddDescriptionHeader (string descriptionString);

    //	// -(OBEXError)addNameHeader:(NSString *)inNameString;
    	[Export ("addNameHeader:")]
	    ObexError AddNameHeader (string nameString);

    //	// -(OBEXError)addUserDefinedHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addUserDefinedHeader:length:")]
    //	unsafe ObexError AddUserDefinedHeader (IntPtr inHeaderData, uint inHeaderDataLength);

    //	// -(OBEXError)addImageHandleHeader:(NSString *)type;
    	[Export ("addImageHandleHeader:")]
	    ObexError AddImageHandleHeader (string type);

    //	// -(OBEXError)addImageDescriptorHeader:(const void *)inHeaderData length:(uint32_t)inHeaderDataLength;
    //	[Export ("addImageDescriptorHeader:length:")]
    //	ObexError AddImageDescriptorHeader (IntPtr inHeaderData, uint inHeaderDataLength);
    }
}