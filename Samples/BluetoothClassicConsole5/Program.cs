using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net.Obex;
using System;
using System.IO;
using InTheHand.Net;
using System.Threading.Tasks;

namespace BluetoothClassicConsole5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BluetoothClient client = new BluetoothClient();

            System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();


            var t = Task.Run(async () =>
            {
                BluetoothDevicePicker picker = new BluetoothDevicePicker();
                var device = await picker.PickSingleDeviceAsync();
                device.Refresh();
                System.Diagnostics.Debug.WriteLine(device.Authenticated);

                ObexWebRequest r = new ObexWebRequest(new Uri($"obex-pbap://{device.DeviceAddress}/*.vcf"));
                var response = r.GetResponse();
                var stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var text = sr.ReadToEnd();
            }, cancellationTokenSource.Token);

            Task.WaitAll(t);
             /*BluetoothDeviceInfo device = null;
            foreach (var dev in client.DiscoverDevices())
            {
                if (dev.DeviceName.Contains("PREFIX"))
                {
                    device = dev;
                    break;
                }
            }

            if (!device.Authenticated)
            {
                BluetoothSecurity.PairRequest(device.DeviceAddress, "1234");
            }*/

            


            /*ObexClient oc = new ObexClient();
            oc.BaseAddress = new Uri($"obex-pbap://{device.DeviceAddress}/");
            bool connected = await oc.ConnectAsync();
            oc.GetAsync()
            client.Connect(device.DeviceAddress, BluetoothService.PhonebookAccessPce);

            var stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.ASCII);
            sw.WriteLine("Hello world!\r\n\r\n");
            sw.Close();

            client.Close();*/
        }
    }
}
