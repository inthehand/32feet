using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace RfcommServiceConsole
{
    internal class Program
    {
        static BluetoothListener listener = new BluetoothListener(BluetoothService.SerialPort);

        static void Main(string[] args)
        {
            listener.Start();
            while(true)
            {
                var client = listener.AcceptBluetoothClient();
                Console.WriteLine(client.RemoteMachineName);
                var stream = client.GetStream();
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                while(client.Connected)
                {
                    Console.WriteLine(sr.ReadLine());
                }

                Console.WriteLine("Disconnected");
            }
        }
    }
}