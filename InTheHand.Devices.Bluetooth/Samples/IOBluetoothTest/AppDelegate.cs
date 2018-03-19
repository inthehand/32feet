using AppKit;
using Foundation;
using ObjCRuntime;


namespace IOBluetoothTest
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            ObjCRuntime.Dlfcn.dlopen("/System.Library/Frameworks/IOBluetooth.framework/IOBluetooth", 0);
            var cod = IOBluetooth.IOBluetoothHostController.DefaultController.ClassOfDevice;
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
