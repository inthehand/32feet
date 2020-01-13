using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InTheHand.Bluetooth;


namespace BluetoothClientApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Appearing += MainPage_Appearing;
        }

        
        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            Bluetooth b = new Bluetooth();
            var device = await b.RequestDevice(new RequestDeviceOptions() { AcceptAllDevices = true });
            if (device != null)
            {
                var servs = await device.Gatt.GetPrimaryServices();
                foreach (var serv in servs)
                {
                    System.Diagnostics.Debug.WriteLine(serv.Uuid + " " + serv.IsPrimary);
                }
            }
        }

    }
}
