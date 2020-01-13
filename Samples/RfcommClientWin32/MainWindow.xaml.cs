using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RfcommClientWin32
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Activated += MainWindow_Activated;
            
        }

        private async void MainWindow_Activated(object sender, EventArgs e)
        {
            BluetoothDevicePicker picker = new BluetoothDevicePicker();
            //picker.ClassOfDevices.Add(new ClassOfDevice(DeviceClass.SmartPhone, 0));

            var device = await picker.PickSingleDeviceAsync();

            InTheHand.Net.Sockets.BluetoothClient client = new InTheHand.Net.Sockets.BluetoothClient();
            System.Diagnostics.Debug.WriteLine("Unknown");

            foreach (BluetoothDeviceInfo bdi in client.DiscoverDevices())
            {
                System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress);
            }
            
            System.Diagnostics.Debug.WriteLine("Paired");
            foreach(BluetoothDeviceInfo bdi in client.PairedDevices)
            {
                System.Diagnostics.Debug.WriteLine(bdi.DeviceName + " " + bdi.DeviceAddress);
            }
            

            //System.Diagnostics.Debug.WriteLine(client.RemoteMachineName);
            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
            var stream = client.GetStream();
            stream.Write(System.Text.Encoding.ASCII.GetBytes("Hello World\r\n\r\n"),0,15);
            stream.Close();
        }
    }
}
