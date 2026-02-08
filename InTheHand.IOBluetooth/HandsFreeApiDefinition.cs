using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace IOBluetooth
{
    // @interface IOBluetoothHandsFree : NSObject
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [BaseType(typeof(NSObject), Name = "IOBluetoothHandsFree")]
    public interface HandsFree
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
        uint DeviceSupportedSmsServices { get; }

        // @property (readonly) uint32_t deviceCallHoldModes __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("deviceCallHoldModes")]
        HandsFreeCallHoldMode DeviceCallHoldModes { get; }

        // @property (readonly) IOBluetoothSMSMode SMSMode __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("SMSMode")]
        SmsMode SmsMode { get; }

        // @property (readonly, getter = isSMSEnabled) BOOL SMSEnabled __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("SMSEnabled")]
        bool SmsEnabled { [Bind("isSMSEnabled")] get; }

        [Wrap("WeakDelegate")]
        HandsFreeDelegate Delegate { get; set; }

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
        NativeHandle Constructor(BluetoothDevice device, HandsFreeDelegate hfDelegate);

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
    public interface HandsFreeDelegate
    {
        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device connected:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:connected:"), EventArgs("HandsFreeConnected")]
        void Connected(HandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device disconnected:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:disconnected:")]
        void Disconnected(HandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device scoConnectionOpened:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:scoConnectionOpened:")]
        void ScoConnectionOpened(HandsFree device, NSNumber status);

        // @optional -(void)handsFree:(IOBluetoothHandsFree *)device scoConnectionClosed:(NSNumber *)status __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("handsFree:scoConnectionClosed:")]
        void ScoConnectionClosed(HandsFree device, NSNumber status);
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

    // @interface HandsFreeSDPServiceRecordAdditions (IOBluetoothSDPServiceRecord)
    //[Category]
    //[BaseType(typeof(SdpServiceRecord))]
    //interface HandsFreeSDPServiceRecordAdditions
    //{
    //    // -(uint16_t)handsFreeSupportedFeatures __attribute__((availability(macos, introduced=10.7)));
    //    [Introduced(PlatformName.MacOSX, 10, 7)]
    //    [Export("handsFreeSupportedFeatures")]
    //    ushort HandsFreeSupportedFeatures { get; }
    //}

    // @interface IOBluetoothHandsFreeAudioGateway : IOBluetoothHandsFree
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [BaseType(typeof(HandsFree), Name = "IOBluetoothHandsFreeAudioGateway")]
    public interface HandsFreeAudioGateway
    {
        // -(instancetype)initWithDevice:(IOBluetoothDevice *)device delegate:(id)inDelegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("initWithDevice:delegate:")]
        NativeHandle Constructor(BluetoothDevice device, NSObject @delegate);

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
    [Protocol(Name = "IOBluetoothHandsFreeAudioGatewayDelegate")]
    public interface HandsFreeAudioGatewayDelegate
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
    [BaseType(typeof(HandsFree), Name = "IOBluetoothHandsFreeDevice")]
    public interface HandsFreeDevice
    {
        // -(instancetype)initWithDevice:(IOBluetoothDevice *)device delegate:(id)delegate __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("initWithDevice:delegate:")]
        NativeHandle Constructor(BluetoothDevice device, NSObject @delegate);

        // -(void)dialNumber:(NSString *)aNumber __attribute__((availability(macos, introduced=10.7)));
        [Introduced(PlatformName.MacOSX, 10, 7)]
        [Export("dialNumber:")]
        void DialNumber(string number);

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
    [Protocol(Name = "IOBluetoothHandsFreeDeviceDelegate")]
    public interface HandsFreeDeviceDelegate : HandsFreeDelegate
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