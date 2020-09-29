using InTheHand.Net.Sockets;
using System.ComponentModel;
using System.IO;
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
            BluetoothDeviceInfo device = null;

            BluetoothClient client = new BluetoothClient();

            var devices = client.PairedDevices;

            foreach (BluetoothDeviceInfo bdi in devices)
            {
                System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress.ToString("C") + " " + bdi.Authenticated);
            }

            foreach (BluetoothDeviceInfo bdi in client.DiscoverDevices(24))
            {
                System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress.ToString("C") + " " + bdi.Authenticated);
                if (device == null)
                    device = bdi;
            }

            //client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
            //stream = client.GetStream();
            //byte[] data = System.Text.Encoding.ASCII.GetBytes("Hello world!"); //! 0 200 200 210 1\r\nTEXT 4 0 30 40 Hello World\r\nFORM\r\nPRINT\r\n");
            //stream.Write(data, 0, data.Length);
            //stream.Close();

            base.OnAppearing();
        }

        Stream stream = null;

        protected override void OnDisappearing()
        {
            if (stream is object)
            {
                stream.Dispose();
                stream = null;
            }
            base.OnDisappearing();
        }
    }
}
