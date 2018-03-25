using System.Runtime.InteropServices;
using CoreFoundation;
using IOBluetooth;
using ObjCRuntime;

namespace IOBluetoothUI
{

[Native]
public enum IOBluetoothServiceBrowserControllerOptions : ulong
{
	None = 0,
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
	[Verify (PlatformInvoke)]
	static extern unsafe int IOBluetoothValidateHardwareWithDescription (CFStringRef* cancelButtonTitle, CFStringRef* descriptionText);

	
	// extern IOBluetoothPairingControllerRef IOBluetoothGetPairingController ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe IOBluetoothPairingControllerRef* IOBluetoothGetPairingController ();

	// extern IOBluetoothDeviceSelectorControllerRef IOBluetoothGetDeviceSelectorController ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe IOBluetoothDeviceSelectorControllerRef* IOBluetoothGetDeviceSelectorController ();

	}

public enum BluetoothKeyboardReturnType : uint
{
	ANSIReturn,
	ISOReturn,
	JISReturn,
	NoReturn
}

}