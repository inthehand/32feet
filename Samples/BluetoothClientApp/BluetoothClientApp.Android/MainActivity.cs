using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using InTheHand.Bluetooth;

namespace BluetoothClientApp.Droid
{
    [Activity(Label = "BluetoothClientApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        BluetoothDevice device = null;


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            base.OnCreate(savedInstanceState);

            /*Bluetooth b = new Bluetooth();
            var opts = new RequestDeviceOptions();
            opts.AcceptAllDevices = true;
            device = await b.RequestDevice(opts);
            if (device != null)
            {
                //await device.Gatt.Connect();
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

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }


        private void Device_AdvertisementReceived(object sender, BluetoothAdvertisingEvent e)
        {
            System.Diagnostics.Debug.WriteLine(e.Name);
            System.Diagnostics.Debug.WriteLine(e.Appearance);
            foreach (var data in e.ManufacturerData)
            {
                System.Diagnostics.Debug.WriteLine(data.Key);
                System.Diagnostics.Debug.WriteLine(data.Value.Length);
            }
            foreach (var data in e.ServiceData)
            {
                System.Diagnostics.Debug.WriteLine(data.Key);
                System.Diagnostics.Debug.WriteLine(data.Value.Length);
            }
        }
    }
}