using System;
using AppKit;
using Foundation;
using IOBluetooth;
using IOBluetoothUI;
using ObjCRuntime;

namespace IOBluetoothUI
{
    // @interface IOBluetoothDeviceSelectorController : NSWindowController
    [BaseType(typeof(NSWindowController))]
    interface IOBluetoothDeviceSelectorController
    {
        // +(IOBluetoothDeviceSelectorController *)deviceSelector;
        [Static]
        [Export("deviceSelector")]
        IOBluetoothDeviceSelectorController DeviceSelector { get; }

        // -(int)runModal;
        [Export("runModal")]
        int RunModal();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, void* contextInfo);

        // -(NSArray *)getResults;
        [Export("getResults")]
        IOBluetoothDevice[] Results { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)options;
        [Export("setOptions:")]
        void SetOptions(IOBluetoothServiceBrowserControllerOptions options);

        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        [Export("getOptions")]
        IOBluetoothServiceBrowserControllerOptions Options { get; }

        // -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        //[Export("setSearchAttributes:")]
        //unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        // -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        //[Export("getSearchAttributes")]
        //[Verify(MethodToProperty)]
        //unsafe IOBluetoothDeviceSearchAttributes* SearchAttributes { get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(IOBluetoothSDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        [Export("addAllowedUUIDArray:")]
        void AddAllowedUUIDArray(IOBluetoothSDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        // -(void)setTitle:(NSString *)windowTitle;
        [Export("setTitle:")]
        void SetTitle(string windowTitle);

        // -(NSString *)getTitle;
        [Export("getTitle")]
        string Title { get; }

        // -(void)setHeader:(NSString *)headerText;
        [Export("setHeader:")]
        void SetHeader(string headerText);

        // -(NSString *)getHeader;
        [Export("getHeader")]
        string Header { get; }

        // -(void)setDescriptionText:(NSString *)descriptionText;
        [Export("setDescriptionText:")]
        void SetDescriptionText(string descriptionText);

        // -(NSString *)getDescriptionText;
        [Export("getDescriptionText")]
        string DescriptionText { get; }

        // -(void)setPrompt:(NSString *)prompt;
        [Export("setPrompt:")]
        void SetPrompt(string prompt);

        // -(NSString *)getPrompt;
        [Export("getPrompt")]
        string Prompt { get; }

        // -(void)setCancel:(NSString *)prompt;
        [Export("setCancel:")]
        void SetCancel(string prompt);

        // -(NSString *)getCancel;
        [Export("getCancel")]
        string Cancel { get; }
    }

    // @interface IOBluetoothPairingController : NSWindowController
    [BaseType(typeof(NSWindowController))]
    interface IOBluetoothPairingController
    {
        // +(IOBluetoothPairingController *)pairingController;
        [Static]
        [Export("pairingController")]
        IOBluetoothPairingController PairingController { get; }

        // -(int)runModal;
        [Export("runModal")]
        int RunModal();

        // -(NSArray *)getResults;
        [Export("getResults")]
        IOBluetoothDevice[] Results { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)options;
        [Export("setOptions:")]
        void SetOptions(IOBluetoothServiceBrowserControllerOptions options);

        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        [Export("getOptions")]
        IOBluetoothServiceBrowserControllerOptions Options { get; }

        // -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        //[Export("setSearchAttributes:")]
        //unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        // -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        //[Export("getSearchAttributes")]
        //unsafe IOBluetoothDeviceSearchAttributes* SearchAttributes { get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(IOBluetoothSDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        [Export("addAllowedUUIDArray:")]
        [Verify(StronglyTypedNSArray)]
        void AddAllowedUUIDArray(IOBluetoothSDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        // -(void)setTitle:(NSString *)windowTitle;
        [Export("setTitle:")]
        void SetTitle(string windowTitle);

        // -(NSString *)getTitle;
        [Export("getTitle")]
        string Title { get; }

        // -(void)setDescriptionText:(NSString *)descriptionText;
        [Export("setDescriptionText:")]
        void SetDescriptionText(string descriptionText);

        // -(NSString *)getDescriptionText;
        [Export("getDescriptionText")]
        string DescriptionText { get; }

        // -(void)setPrompt:(NSString *)prompt;
        [Export("setPrompt:")]
        void SetPrompt(string prompt);

        // -(NSString *)getPrompt;
        [Export("getPrompt")]
        string Prompt { get; }
    }

    // @interface IOBluetoothServiceBrowserController : NSWindowController
    [BaseType(typeof(NSWindowController))]
    interface IOBluetoothServiceBrowserController
    {
        // +(IOBluetoothServiceBrowserController *)serviceBrowserController:(IOBluetoothServiceBrowserControllerOptions)inOptions;
        [Static]
        [Export("serviceBrowserController:")]
        IOBluetoothServiceBrowserController ServiceBrowserController(uint inOptions);

        // +(IOBluetoothServiceBrowserController *)withServiceBrowserControllerRef:(IOBluetoothServiceBrowserControllerRef)serviceBrowserControllerRef;
        //[Static]
        //[Export("withServiceBrowserControllerRef:")]
        //unsafe IOBluetoothServiceBrowserController WithServiceBrowserControllerRef(IOBluetoothServiceBrowserControllerRef* serviceBrowserControllerRef);

        // -(IOBluetoothServiceBrowserControllerRef)getServiceBrowserControllerRef;
        //[Export("getServiceBrowserControllerRef")]
        //[Verify(MethodToProperty)]
        //unsafe IOBluetoothServiceBrowserControllerRef* ServiceBrowserControllerRef { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)inOptions;
        [Export("setOptions:")]
        void SetOptions(IOBluetoothServiceBrowserControllerOptions inOptions);

        // -(int)runModal;
        [Export("runModal")]
        int RunModal();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, void* contextInfo);

        // -(NSArray *)getResults;
        [Export("getResults")]
        IOBluetoothSDPServiceRecord[] Results { get; }

        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        [Export("getOptions")]
        IOBluetoothServiceBrowserControllerOptions Options { get; }

        // -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        [Export("setSearchAttributes:")]
        unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        // -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        [Export("getSearchAttributes")]
        unsafe IOBluetoothDeviceSearchAttributes* SearchAttributes { get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(IOBluetoothSDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        [Export("addAllowedUUIDArray:")]
        void AddAllowedUUIDArray(IOBluetoothSDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        // -(void)setTitle:(NSString *)windowTitle;
        [Export("setTitle:")]
        void SetTitle(string windowTitle);

        // -(NSString *)getTitle;
        [Export("getTitle")]
        string Title { get; }

        // -(void)setDescriptionText:(NSString *)descriptionText;
        [Export("setDescriptionText:")]
        void SetDescriptionText(string descriptionText);

        // -(NSString *)getDescriptionText;
        [Export("getDescriptionText")]
        string DescriptionText { get; }

        // -(void)setPrompt:(NSString *)prompt;
        [Export("setPrompt:")]
        void SetPrompt(string prompt);

        // -(NSString *)getPrompt;
        [Export("getPrompt")]
        string Prompt { get; }
    }

    // @interface IOBluetoothObjectPushUIController : NSWindowController
    [BaseType(typeof(NSWindowController))]
    interface IOBluetoothObjectPushUIController
    {
        // -(IOBluetoothObjectPushUIController *)initObjectPushWithBluetoothDevice:(IOBluetoothDevice *)inDevice withFiles:(NSArray *)inFiles delegate:(id)inDelegate;
        [Export("initObjectPushWithBluetoothDevice:withFiles:delegate:")]
        IntPtr Constructor(IOBluetoothDevice inDevice, NSString[] inFiles, NSObject inDelegate);

        // -(void)runModal;
        [Export("runModal")]
        void RunModal();

        // -(void)runPanel;
        [Export("runPanel")]
        void RunPanel();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, void* contextInfo);

        // -(void)stop;
        [Export("stop")]
        void Stop();

        // -(void)setTitle:(NSString *)windowTitle;
        [Export("setTitle:")]
        void SetTitle(string windowTitle);

        // -(NSString *)getTitle;
        [Export("getTitle")]
        string Title { get; }

        // -(void)setIconImage:(NSImage *)image;
        [Export("setIconImage:")]
        void SetIconImage(NSImage image);

        // -(IOBluetoothDevice *)getDevice;
        [Export("getDevice")]
        IOBluetoothDevice Device { get; }

        // -(BOOL)isTransferInProgress;
        [Export("isTransferInProgress")]
        bool IsTransferInProgress { get; }
    }

    // @interface IOBluetoothPasskeyDisplay : NSView
    [BaseType(typeof(NSView))]
    interface IOBluetoothPasskeyDisplay
    {
        // @property (assign) BOOL usePasskeyNotificaitons;
        [Export("usePasskeyNotificaitons")]
        bool UsePasskeyNotificaitons { get; set; }

        // @property (assign) BOOL isIncomingRequest;
        [Export("isIncomingRequest")]
        bool IsIncomingRequest { get; set; }

        // @property (copy) NSString * passkey;
        [Export("passkey")]
        string Passkey { get; set; }

        // @property (retain) NSImage * returnImage;
        [Export("returnImage", ArgumentSemantic.Retain)]
        NSImage ReturnImage { get; set; }

        // @property (retain) NSImage * returnHighlightImage;
        [Export("returnHighlightImage", ArgumentSemantic.Retain)]
        NSImage ReturnHighlightImage { get; set; }

        // @property (assign) NSView * centeredView __attribute__((iboutlet));
        [Export("centeredView", ArgumentSemantic.Assign)]
        NSView CenteredView { get; set; }

        // @property (assign) NSLayoutConstraint * backgroundImageConstraint __attribute__((iboutlet));
        [Export("backgroundImageConstraint", ArgumentSemantic.Assign)]
        NSLayoutConstraint BackgroundImageConstraint { get; set; }

        // +(IOBluetoothPasskeyDisplay *)sharedDisplayView;
        [Static]
        [Export("sharedDisplayView")]
        IOBluetoothPasskeyDisplay SharedDisplayView { get; }

        // -(void)setPasskey:(NSString *)inString forDevice:(IOBluetoothDevice *)device usingSSP:(BOOL)isSSP;
        [Export("setPasskey:forDevice:usingSSP:")]
        void SetPasskey(string inString, IOBluetoothDevice device, bool isSSP);

        // -(void)advancePasskeyIndicator;
        [Export("advancePasskeyIndicator")]
        void AdvancePasskeyIndicator();

        // -(void)retreatPasskeyIndicator;
        [Export("retreatPasskeyIndicator")]
        void RetreatPasskeyIndicator();

        // -(void)resetPasskeyIndicator;
        [Export("resetPasskeyIndicator")]
        void ResetPasskeyIndicator();
    }

    // @interface IOBluetoothAccessibilityIgnoredTextFieldCell : NSTextFieldCell
    [BaseType(typeof(NSTextFieldCell))]
    interface IOBluetoothAccessibilityIgnoredTextFieldCell
    {
    }

    // @interface IOBluetoothAccessibilityIgnoredImageCell : NSImageCell
    [BaseType(typeof(NSImageCell))]
    interface IOBluetoothAccessibilityIgnoredImageCell
    {
    }
}