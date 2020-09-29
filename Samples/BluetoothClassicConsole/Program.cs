using System;
using System.Collections.Generic;
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
                if(dev.DeviceName.Contains("RequiredName"))
                {
                    device = dev;
                    break;
                }
            }

            client.Connect(device.DeviceAddress, BluetoothService.SerialPort);
            client.Close();
        }
    }
}
