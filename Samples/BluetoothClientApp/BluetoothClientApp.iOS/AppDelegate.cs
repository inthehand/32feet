using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using InTheHand.Bluetooth;
using UIKit;

namespace BluetoothClientApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            Task.Run(() =>
            {
                UIDevice.CurrentDevice.BeginInvokeOnMainThread(() =>
                {
                    ExternalAccessory.EAAccessoryManager.SharedAccessoryManager.ShowBluetoothAccessoryPicker(null, (e) =>
                    {
                        // error 2 if cancel is selected
                        System.Diagnostics.Debug.WriteLine(e);
                    });
                });

            });
            
            return base.FinishedLaunching(app, options);
        }

        BluetoothDevice device;

        public override async void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);

            Bluetooth b = new Bluetooth();

            while(!await b.GetAvailability())
            {
                await Task.Delay(100);
            }

            /*var opts = new RequestDeviceOptions();
            opts.AcceptAllDevices = true;
            device = await b.RequestDevice(opts);
            if (device != null)
            {
                await device.Gatt.Connect();
                //device.AdvertisementReceived += Device_AdvertisementReceived;
                //await device.WatchAdvertisementsAsync();
                var service = await device.Gatt.GetPrimaryServiceAsync(new Guid(0x0000180F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB));
                var chars = await service.GetCharacteristicsAsync();
                foreach (var ch in chars)
                {
                    System.Diagnostics.Debug.WriteLine(ch.Properties);
                    var desc = await ch.GetDescriptorAsync(new Guid(0x00002902, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB));
                    if (desc != null)
                    {
                        var descval = await desc.ReadValueAsync();
                        System.Diagnostics.Debug.WriteLine(desc.Uuid);
                    }

                    System.Diagnostics.Debug.WriteLine(ch.Uuid);
                    byte[] val = await ch.ReadValueAsync();
                    int ival = val[0];
                }
            }*/
        }
    }
}
