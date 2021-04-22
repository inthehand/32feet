using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace BluetoothClassicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BluetoothClient client = new BluetoothClient();

            BluetoothDeviceInfo device = null;
            foreach(var dev in client.DiscoverDevices())
            {
                if(dev.DeviceName.Contains("PREFIX"))
                {
                    device = dev;
                    break;
                }
            }

            if (!device.Authenticated)
            {
                BluetoothSecurity.PairRequest(device.DeviceAddress, "1234");
            }

            device.Refresh();
            System.Diagnostics.Debug.WriteLine(device.Authenticated);

            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);

            var stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.ASCII);
            sw.WriteLine("Hello world!\r\n\r\n");
            sw.Close();

            client.Close();
        }
    }
}
