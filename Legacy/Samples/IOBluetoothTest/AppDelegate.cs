using System;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using ObjCRuntime;
using IOBluetooth;
using IOBluetoothUI;

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
            var controller = new HostController();
            var cod = controller.ClassOfDevice.ToString("x8");
            var addr = controller.AddressAsString;

            RFCOMMChannel channel = null;

            foreach(BluetoothDevice dev in BluetoothDevice.PairedDevices)
            {
                var addrstruct = dev.Address;

                
                //System.Diagnostics.Debug.WriteLine(dev.Address.ToString("x6"));
                System.Diagnostics.Debug.WriteLine(dev.AddressString);
                System.Diagnostics.Debug.WriteLine(dev.NameOrAddress);

                if(dev.ServiceClassMajor == IOBluetooth.ServiceClassMajor.Rendering)
                {
                    int res = dev.OpenRFCOMMChannelSync(out channel, 1, null);
                    if(channel != null)
                    {
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes("hello world!\r\n");

                        int len = buffer.Length;
                        IntPtr buff = Marshal.AllocHGlobal(len);
                        Marshal.StructureToPtr(buffer, buff, true);

                        res = channel.WriteSync(buff, (ushort)len);
                        channel.CloseChannel();
                    }
                }
            }

            var c = DeviceSelectorController.DeviceSelector;
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

    public class Deleg : RFCOMMChannelDelegate
    {
    }
}
