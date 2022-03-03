using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RfcommXamarinForms
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var r = BluetoothRadio.Default;
            r.Mode = RadioMode.PowerOff;

            BluetoothDeviceInfo device = null;

            BluetoothClient client = new BluetoothClient();

            var picker = new BluetoothDevicePicker();
            
            device = await picker.PickSingleDeviceAsync();

            await Task.Delay(2000);

            if (device != null)
            {
                bool paired = BluetoothSecurity.PairRequest(device.DeviceAddress, "0000");
                await Task.Delay(2000);
                //bool unpaired = BluetoothSecurity.RemoveDevice(device.DeviceAddress);
                client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
                if (client.Connected)
                {
                    stream = client.GetStream();
                    StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.ASCII);
                    sw.WriteLine("Hello world!\r\n\r\n");
                    sw.Close();
                }

                /*byte[] data = System.Text.Encoding.ASCII.GetBytes("Hello world!\r\n\r\n"); //! 0 200 200 210 1\r\nTEXT 4 0 30 40 Hello World\r\nFORM\r\nPRINT\r\n");
                stream.Write(data, 0, data.Length);
                stream.Close();*/
            }
        }
    }
}
