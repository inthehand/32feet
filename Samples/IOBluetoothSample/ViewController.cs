using System;

using AppKit;
using Foundation;

using IOBluetooth;
using IOBluetoothUI;

namespace IOBluetoothSample
{
    public partial class ViewController : NSViewController, IRfcommChannelDelegate
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private class MyRfcommDelegate : RfcommChannelDelegate
        {
            public override void RfcommChannelData(RfcommChannel rfcommChannel, IntPtr dataPointer, nuint dataLength)
            {
                System.Diagnostics.Debug.WriteLine(dataLength);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            var selector = DeviceSelectorController.DeviceSelector;
            var result = selector.RunModal();

            if(result == -1000)
            {
                var devices = selector.Results;

                if(devices != null)
                {
                    var device = devices[0];

                    foreach (var sr in device.Services)
                    {
                        sr.GetRfcommChannelID(out var chan);
                        System.Diagnostics.Debug.WriteLine(sr.ServiceName + " " + chan);
                    }

                    // making some assumptions here and outputting text to channel 1 as we know it's for an ESC/POS printer initially
                    System.Diagnostics.Debug.WriteLine(device.NameOrAddress);
                    var openResult = device.OpenRfcommChannelSync(out var channel, 1, new MyRfcommDelegate());
                    System.Diagnostics.Debug.Write(channel.IsOpen);

                    string message = "Printed over Bluetooth from\r\nmacOS using IOBluetooth C#\r\nbindings for Xamarin.\r\nhttps://32feet.net\r\n\r\n\r\n\r\n";
                    
                    IntPtr data = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(message);

                    var writeResult = channel.WriteSync(data, (ushort)message.Length);
                    channel.CloseChannel();
                }
            }
            selector.Dispose();

        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
