namespace IOBluetoothUI;

/// <summary>
/// Option values to customize the behavior of a <see cref="ServiceBrowserController"/> object.
/// </summary>
[Flags]
public enum ServiceBrowserControllerOptions : ulong
{
    /// <summary>
    /// No options set.
    /// </summary>
    None = 0,
    //automatically start an inquiry when the panel is displayed. This has been deprecated in 10.5
    //AutoStartInquiry = (1 << 0),
    /// <summary>
    /// Disconnect from the device when UI operations are finished.
    /// </summary>
    DisconnectWhenDone = (1 << 1)
}


public enum IOBluetoothUI
{
    Success = (-1000),
    UserCanceledErr = (-1001)
}

/*static class CFunctions
{

    // extern IOReturn IOBluetoothValidateHardwareWithDescription (CFStringRef cancelButtonTitle, CFStringRef descriptionText) __attribute__((availability(macos, introduced=10.7)));
    [Introduced(PlatformName.MacOSX, 10, 7)]
    [DllImport("__Internal")]
    static extern int IOBluetoothValidateHardwareWithDescription(NSString cancelButtonTitle, NSString descriptionText);


    // extern IOBluetoothPairingControllerRef IOBluetoothGetPairingController ();
    [DllImport("__Internal")]
    static extern BluetoothPairingController IOBluetoothGetPairingController();

    // extern IOBluetoothDeviceSelectorControllerRef IOBluetoothGetDeviceSelectorController ();
    [DllImport("__Internal")]
    static extern DeviceSelectorController IOBluetoothGetDeviceSelectorController();

}*/

/*public enum BluetoothKeyboardReturnType : uint
{
    AnsiReturn,
    IsoReturn,
    JisReturn,
    NoReturn
}*/