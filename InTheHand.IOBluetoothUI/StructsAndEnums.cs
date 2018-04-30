using System.Runtime.InteropServices;
using CoreFoundation;
using IOBluetooth;
using ObjCRuntime;
using Foundation;

namespace IOBluetoothUI
{
    
[Native]
public enum IOBluetoothServiceBrowserControllerOptions : ulong
{
	None = 0,
        //automatically start an inquiry when the panel is displayed. This has been deprecated in 10.5

	AutoStartInquiry = (1 << 0),
	DisconnectWhenDone = (1 << 1)
}


    public enum IOBluetoothUI
{
	Success = (-1000),
	UserCanceledErr = (-1001)
}

static class CFunctions
{
	
	// extern IOReturn IOBluetoothValidateHardwareWithDescription (CFStringRef cancelButtonTitle, CFStringRef descriptionText) __attribute__((availability(macos, introduced=10.7)));
    [Introduced (PlatformName.MacOSX, 10, 7)]
	[DllImport ("__Internal")]
	static extern int IOBluetoothValidateHardwareWithDescription (NSString cancelButtonTitle, NSString descriptionText);

	
	// extern IOBluetoothPairingControllerRef IOBluetoothGetPairingController ();
	[DllImport ("__Internal")]
	static extern IOBluetoothPairingController IOBluetoothGetPairingController ();

	// extern IOBluetoothDeviceSelectorControllerRef IOBluetoothGetDeviceSelectorController ();
	[DllImport ("__Internal")]
	static extern IOBluetoothDeviceSelectorController IOBluetoothGetDeviceSelectorController ();

	}

public enum BluetoothKeyboardReturnType : uint
{
	ANSIReturn,
	ISOReturn,
	JISReturn,
	NoReturn
}

}