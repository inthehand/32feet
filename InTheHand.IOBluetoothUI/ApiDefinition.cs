using System;
using AppKit;
using Foundation;
using IOBluetooth;
using IOBluetoothUI;
using ObjCRuntime;


namespace IOBluetoothUI
{
    /// <summary>
    /// An NSWindowController subclass to display a window to initiate pairing to other bluetooth devices.
    /// </summary>
    // @interface IOBluetoothDeviceSelectorController : NSWindowController
    [BaseType(typeof(NSWindowController), Name = "IOBluetoothDeviceSelectorController")]
    public interface DeviceSelectorController
    {
        // +(IOBluetoothDeviceSelectorController *)deviceSelector;
        /// <summary>
        /// Method call to instantiate a new IOBluetoothDeviceSelectorController object.
        /// </summary>
        /// <value>Success - a new instance of the device selector Controller Failure - nil.</value>
        [Static]
        [Export("deviceSelector")]
        DeviceSelectorController DeviceSelector { get; }

        // -(int)runModal;
        /// <summary>
        /// Runs the device selector panel in a modal session to allow the user to select a Bluetooth device.
        /// </summary>
        /// <returns>The modal.</returns>
        [Export("runModal")]
        int RunModal();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        /// <summary>
        /// Runs the device selector panel as a sheet on the target window.
        /// </summary>
        /// <returns>The sheet modal for window.</returns>
        /// <param name="sheetWindow">Sheet window.</param>
        /// <param name="modalDelegate">Modal delegate.</param>
        /// <param name="didEndSelector">Did end selector.</param>
        /// <param name="contextInfo">Context info.</param>
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

        // -(NSArray *)getResults;
        /// <summary>
        /// Returns the result of the user's selection.
        /// </summary>
        /// <value>The results.</value>
        [Export("getResults")]
        BluetoothDevice[] Results { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)options;
        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        /// <summary>
        /// Gets or sets the option bits that control the panel's behavior.
        /// </summary>
        /// <value>The options.</value>
        [Export("options")]
        ServiceBrowserControllerOptions Options { [Bind("getOptions")] get; set; }

        // -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        //[Export("setSearchAttributes:")]
        //unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        // -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        //[Export("getSearchAttributes")]
        //[Verify(MethodToProperty)]
        //unsafe IOBluetoothDeviceSearchAttributes* SearchAttributes { get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        /// <summary>
        /// Adds a UUID to the list of UUIDs that are used to validate the user's selection.
        /// </summary>
        /// <param name="allowedUUID">Allowed UUID.</param>
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(SDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        /// <summary>
        /// Adds an array of UUIDs to the list of UUIDs that are used to validate the user's selection.
        /// </summary>
        /// <param name="allowedUUIDArray">Allowed UUIDA rray.</param>
        [Export("addAllowedUUIDArray:")]
        void AddAllowedUUIDArray(SDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        /// <summary>
        /// Resets the controller back to the default state where it will accept any device the user selects.
        /// </summary>
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        // -(void)setTitle:(NSString *)windowTitle;
        // -(NSString *)getTitle;
        /// <summary>
        /// Gets or sets the title of the panel when not run as a sheet.
        /// </summary>
        /// <value>The title.</value>
        [Export("title")]
        string Title { [Bind("getTitle")] get; set; }

        // -(void)setHeader:(NSString *)headerText;
        // -(NSString *)getHeader;

        /// <summary>
        /// Gets or sets the header text that appears in the device selector panel.
        /// </summary>
        /// <value>The header.</value>
        [Export("header")]
        string Header { [Bind("getHeader")] get; set; }

        // -(void)setDescriptionText:(NSString *)descriptionText;
        // -(NSString *)getDescriptionText;
        /// <summary>
        /// Gets or sets the description text that appears in the device selector panel.
        /// </summary>
        /// <value>The description text.</value>
        [Export("descriptionText")]
        string DescriptionText { [Bind("getDescriptionText")] get; set; }

        // -(void)setPrompt:(NSString *)prompt;
        // -(NSString *)getPrompt;
        /// <summary>
        /// Gets or sets the title of the default/select button in the device selector panel.
        /// </summary>
        /// <value>The prompt.</value>
        [Export("prompt")]
        string Prompt { [Bind("getPrompt")] get; set; }

        // -(void)setCancel:(NSString *)prompt;
        // -(NSString *)getCancel;
        /// <summary>
        /// Gets or sets the title of the default/cancel button in the device selector panel.
        /// </summary>
        /// <value>String that appears in the default/cancel button in the device selector panel.</value>
        [Export("cancel")]
        string Cancel { [Bind("getCancel")] get; set; }
    }

    /// <summary>
    /// An NSWindowController subclass to display a window to initiate pairing to other bluetooth devices.
    /// </summary>
    // @interface IOBluetoothPairingController : NSWindowController
    [BaseType(typeof(NSWindowController), Name = "IOBluetoothPairingController")]
    public interface BluetoothPairingController
    {
        // +(IOBluetoothPairingController *)pairingController;
        [Static]
        [Export("pairingController")]
        BluetoothPairingController PairingController { get; }

        /// <summary>
        /// Runs the pairing panel in a modal session to allow the user to select a Bluetooth device.
        /// </summary>
        /// <returns>Returns kIOBluetoothUISuccess if a successful, validated device selection was made by the user and that device successfully paired. 
        /// Returns kIOBluetoothUIUserCanceledErr if the user cancelled the panel. 
        /// These return values are the same as NSRunStoppedResponse and NSRunAbortedResponse respectively. 
        /// They are the standard values used in a modal session.</returns>
        // -(int)runModal;
        [Export("runModal")]
        int RunModal();

        /// <summary>
        /// Returns an array of the devices that were paired.
        /// </summary>
        /// <value>The results.</value>
        // -(NSArray *)getResults;
        [Export("getResults")]
        BluetoothDevice[] Results { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)options;
        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        [Export("options")]
        ServiceBrowserControllerOptions Options { [Bind("getOptions")] get; set; }

        // -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        //[Export("setSearchAttributes:")]
        //unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        // -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        //[Export("getSearchAttributes")]
        //unsafe IOBluetoothDeviceSearchAttributes* SearchAttributes { get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(SDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        [Export("addAllowedUUIDArray:")]
        void AddAllowedUUIDArray(SDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        /// <summary>
        /// Gets or sets the title of the device selector panel.
        /// </summary>
        /// <value>The title.</value>
        // -(NSString *)getTitle;
        // -(void)setTitle:(NSString *)windowTitle;
        [Export("title")]
        string Title { [Bind("getTitle")]get; set; }

        /// <summary>
        /// Gets or sets the description text that appears in the device selector panel.
        /// </summary>
        /// <value>The description text.</value>
        // -(NSString *)getDescriptionText;
        // -(void)setDescriptionText:(NSString *)descriptionText;
        [Export("descriptionText")]
        string DescriptionText { [Bind("getDescriptionText")] get; set; }

        /// <summary>
        /// Gets or sets the title of the default/select button in the device selector panel.
        /// </summary>
        /// <value>The prompt.</value>
        // -(NSString *)getPrompt;
        // -(void)setPrompt:(NSString *)prompt;
        [Export("prompt")]
        string Prompt { [Bind("getPrompt")] get; set; }
    }

    // @interface IOBluetoothServiceBrowserController : NSWindowController
    /// <summary>
    /// An NSWindowController subclass to display a window to search for and perform SDP queries on bluetooth devices within range.
    /// </summary>
    [BaseType(typeof(NSWindowController), Name = "IOBluetoothServiceBrowserController")]
    public interface ServiceBrowserController
    {
        // +(IOBluetoothServiceBrowserController *)serviceBrowserController:(IOBluetoothServiceBrowserControllerOptions)inOptions;
        [Static]
        [Export("serviceBrowserController:")]
        ServiceBrowserController CreateServiceBrowserController(ServiceBrowserControllerOptions inOptions);

        // +(IOBluetoothServiceBrowserController *)withServiceBrowserControllerRef:(IOBluetoothServiceBrowserControllerRef)serviceBrowserControllerRef;
        //[Static]
        //[Export("withServiceBrowserControllerRef:")]
        //unsafe IOBluetoothServiceBrowserController WithServiceBrowserControllerRef(IOBluetoothServiceBrowserControllerRef* serviceBrowserControllerRef);

        // -(IOBluetoothServiceBrowserControllerRef)getServiceBrowserControllerRef;
        //[Export("getServiceBrowserControllerRef")]
        //[Verify(MethodToProperty)]
        //unsafe IOBluetoothServiceBrowserControllerRef* ServiceBrowserControllerRef { get; }

        // -(int)runModal;
        /// <summary>
        /// Runs the service browser panel in a modal session to allow the user to select a service on a Bluetooth device.
        /// </summary>
        /// <returns>Returns kIOBluetoothUISuccess if a successful, validated service selection was made by the user. 
        /// Returns kIOBluetoothUIUserCanceledErr if the user cancelled the panel. 
        /// These return values are the same as NSRunStoppedResponse and NSRunAbortedResponse respectively. 
        /// They are the standard values used in a modal session.</returns>
        [Export("runModal")]
        nint RunModal();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        /// <summary>
        /// Runs the service browser panel as a sheet on the target window.
        /// </summary>
        /// <returns>The sheet modal for window.</returns>
        /// <param name="sheetWindow">NSWindow to attach the service browser panel to as a sheet.</param>
        /// <param name="modalDelegate">Delegate object that gets sent the didEndSelector when the sheet modal session is finished.</param>
        /// <param name="didEndSelector">Selector sent to the modalDelegate when the sheet modal session is finished.</param>
        /// <param name="contextInfo">User-definied value passed to the modalDelegate in the didEndSelector.</param>
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

        // -(NSArray *)getResults;
        /// <summary>
        /// Returns the result of the user's selection.
        /// </summary>
        /// <value>The results.</value>
        [Export("getResults")]
        SdpServiceRecord[] Results { get; }

        // -(void)setOptions:(IOBluetoothServiceBrowserControllerOptions)inOptions;
        // -(IOBluetoothServiceBrowserControllerOptions)getOptions;
        /// <summary>
        /// Gets or sets the option bits that control the panel's behavior.
        /// </summary>
        /// <value>The options.</value>
        [Export("options")]
        ServiceBrowserControllerOptions Options { [Bind("getOptions")] get; set; }

        //// -(void)setSearchAttributes:(const IOBluetoothDeviceSearchAttributes *)searchAttributes;
        //[Export("setSearchAttributes:")]
        //unsafe void SetSearchAttributes(IOBluetoothDeviceSearchAttributes* searchAttributes);

        //// -(const IOBluetoothDeviceSearchAttributes *)getSearchAttributes;
        //[Export("getSearchAttributes")]
        //IOBluetoothDeviceSearchAttributes SearchAttributes { [Bind("getSearchAttributes")]get; }

        // -(void)addAllowedUUID:(IOBluetoothSDPUUID *)allowedUUID;
        /// <summary>
        /// Adds a UUID to the list of UUIDs that are used to validate the user's selection.
        /// </summary>
        /// <param name="allowedUUID">Allowed UUID.</param>
        [Export("addAllowedUUID:")]
        void AddAllowedUUID(SDPUUID allowedUUID);

        // -(void)addAllowedUUIDArray:(NSArray *)allowedUUIDArray;
        /// <summary>
        /// Adds an array of UUIDs to the list of UUIDs that are used to validate the user's selection.
        /// </summary>
        /// <param name="allowedUUIDArray">Allowed UUIDA rray.</param>
        [Export("addAllowedUUIDArray:")]
        void AddAllowedUUIDArray(SDPUUID[] allowedUUIDArray);

        // -(void)clearAllowedUUIDs;
        /// <summary>
        /// Resets the controller back to the default state where it will accept any device the user selects.
        /// </summary>
        [Export("clearAllowedUUIDs")]
        void ClearAllowedUUIDs();

        // -(void)setTitle:(NSString *)windowTitle;
        // -(NSString *)getTitle;
        /// <summary>
        /// Gets or sets the title of the panel when not run as a sheet.
        /// </summary>
        /// <value>The title.</value>
        [Export("title")]
        string Title { [Bind("getTitle")] get; set; }

        // -(void)setDescriptionText:(NSString *)descriptionText;
        // -(NSString *)getDescriptionText;
        /// <summary>
        /// Gets or sets the description text that appears in the device selector panel.
        /// </summary>
        /// <value>The description text.</value>
        [Export("descriptionText")]
        string DescriptionText { [Bind("getDescriptionText")] get; set; }

        // -(void)setPrompt:(NSString *)prompt;
        // -(NSString *)getPrompt;
        /// <summary>
        /// Gets or sets the title of the default/select button in the device selector panel.
        /// </summary>
        /// <value>The prompt.</value>
        [Export("prompt")]
        string Prompt { [Bind("getPrompt")] get; set; }
    }

    // @interface IOBluetoothObjectPushUIController : NSWindowController
    [BaseType(typeof(NSWindowController), Name = "IOBluetoothObjectPushUIController")]
    public interface ObjectPushUIController
    {
        // -(IOBluetoothObjectPushUIController *)initObjectPushWithBluetoothDevice:(IOBluetoothDevice *)inDevice withFiles:(NSArray *)inFiles delegate:(id)inDelegate;
        [Export("initObjectPushWithBluetoothDevice:withFiles:delegate:")]
        IntPtr Constructor(BluetoothDevice device, NSString[] files, NSObject inDelegate);

        // -(void)runModal;
        [Export("runModal")]
        void RunModal();

        // -(void)runPanel;
        [Export("runPanel")]
        void RunPanel();

        // -(IOReturn)beginSheetModalForWindow:(NSWindow *)sheetWindow modalDelegate:(id)modalDelegate didEndSelector:(SEL)didEndSelector contextInfo:(void *)contextInfo;
        [Export("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
        unsafe int BeginSheetModalForWindow(NSWindow sheetWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

        // -(void)stop;
        [Export("stop")]
        void Stop();

        // -(void)setTitle:(NSString *)windowTitle;
        // -(NSString *)getTitle;
        [Export("title")]
        string Title { [Bind("getTitle")] get; set; }

        // -(void)setIconImage:(NSImage *)image;
        [Export("setIconImage:")]
        void SetIconImage(NSImage image);

        // -(IOBluetoothDevice *)getDevice;
        [Export("getDevice")]
        BluetoothDevice Device { get; }

        // -(BOOL)isTransferInProgress;
        [Export("isTransferInProgress")]
        bool IsTransferInProgress { get; }
    }

    // @interface IOBluetoothPasskeyDisplay : NSView
    [BaseType(typeof(NSView), Name = "IOBluetoothPasskeyDisplay")]
    interface PasskeyDisplay
    {
        // @property (assign) BOOL usePasskeyNotificaitons;
        [Export("usePasskeyNotificaitons")]
        bool UsePasskeyNotifications { get; set; }

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
        PasskeyDisplay SharedDisplayView { get; }

        // -(void)setPasskey:(NSString *)inString forDevice:(IOBluetoothDevice *)device usingSSP:(BOOL)isSSP;
        [Export("setPasskey:forDevice:usingSSP:")]
        void SetPasskey(string inString, BluetoothDevice device, bool isSSP);

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
    [Internal]
    [BaseType(typeof(NSTextFieldCell))]
    interface IOBluetoothAccessibilityIgnoredTextFieldCell
    {
    }

    // @interface IOBluetoothAccessibilityIgnoredImageCell : NSImageCell
    [Internal]
    [BaseType(typeof(NSImageCell))]
    interface IOBluetoothAccessibilityIgnoredImageCell
    {
    }
}
