using System;
using System.Runtime.InteropServices;
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
            ObjCRuntime.Dlfcn.dlopen("/System.Library/Frameworks/CoreFoundation.framework/CoreFoundation", 0);
            ObjCRuntime.Dlfcn.dlopen("/System.Library/Frameworks/IOBluetooth.framework/IOBluetooth", 0);
            ObjCRuntime.Dlfcn.dlopen("/System.Library/Frameworks/IOBluetoothUI.framework/IOBluetoothUI", 0);
            var controller = new IOBluetooth.IOBluetoothHostController();
            var cod = controller.ClassOfDevice.ToString("x8");
            var addr = controller.AddressAsString;

            foreach(IOBluetooth.IOBluetoothDevice dev in IOBluetooth.IOBluetoothDevice.PairedDevices)
            {
                IntPtr paddr = dev.GetAddress();
                byte[] daddr = new byte[16];
                Marshal.Copy(paddr, daddr, 0, 16);
                //System.Diagnostics.Debug.WriteLine(dev.Address.ToString("x6"));
                System.Diagnostics.Debug.WriteLine(dev.AddressString);
                System.Diagnostics.Debug.WriteLine(dev.NameOrAddress);
            }

            var c = IOBluetoothUI.IOBluetoothDeviceSelectorController.DeviceSelector;
            c.Cancel = "Give it a rest";
            c.DescriptionText = "This is a customised picker";
            c.Title = "Custom Title";
            int result = c.RunModal();
            var devs = c.Results;
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
