using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RfcommXamarinForms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();         
        }

        protected override async void OnAppearing()
        {
                BluetoothDevicePicker picker = new BluetoothDevicePicker();
                picker.RequireAuthentication = false;
                var device = await picker.PickSingleDeviceAsync();

                if (!device.Authenticated)
                {
                    BluetoothSecurity.PairRequest(device.DeviceAddress, "0000");
                }

                BluetoothClient client = new BluetoothClient();

                //await Task.Run(async () =>
                //{
                foreach (BluetoothDeviceInfo bdi in client.DiscoverDevices(24))
                {
                    System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress.ToString("C") + " " + bdi.Authenticated);
                }

                foreach (BluetoothDeviceInfo bdi in client.PairedDevices)
                {
                    System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress.ToString("C") + " " + bdi.Authenticated);
                }
            //});

            //var device = client.PairedDevices.First();

                client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
                stream = client.GetStream();
            byte[] data = System.Text.Encoding.ASCII.GetBytes("Hello world!"); //! 0 200 200 210 1\r\nTEXT 4 0 30 40 Hello World\r\nFORM\r\nPRINT\r\n");
                stream.Write(data, 0, data.Length);
                //stream.Close();
         
           

            

            base.OnAppearing();
        }
    Stream stream = null;

        protected override void OnDisappearing()
        {
            if(stream is object)
            {
                stream.Dispose();
                stream = null;
            }
            base.OnDisappearing();
        }
    }

}
