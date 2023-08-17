using System;
using InTheHand.Net.Bluetooth;
using InTheHand.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var inputString = Console.ReadLine();

            var radio = BluetoothRadio.Default;
            Console.WriteLine($"{radio.Name}\t{radio.LocalAddress}\t{radio.Mode}");

            Task t = Task.Run(async () =>
            {


                BluetoothClient client = new BluetoothClient();
                foreach (var device in client.PairedDevices)
                {
                    Console.WriteLine($"{device.DeviceAddress}\t{device.DeviceName}\t{device.Authenticated}");

                    var services = await device.GetRfcommServicesAsync();
                    foreach (var service in services)
                    {
                        Console.WriteLine(service.ToString());
                    }

                    if (device.DeviceName.Contains("EM220"))
                    {
                        client.Connect(new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort, 1));
                        if (client.Connected)
                        {
                            var stream = client.GetStream();
                            StreamWriter writer = new StreamWriter(stream);
                            writer.WriteLine("Hello printer");
                            writer.Flush();
                            writer.Close();
                            client.Close();
                        }
                    }
                }
            
                await foreach(var device2 in client.DiscoverDevicesAsync())
            {
                Console.WriteLine($"{device2.DeviceAddress}\t{device2.DeviceName}\t{device2.Authenticated}\t{device2.Connected}");
            }
                /*bool available = await Bluetooth.GetAvailabilityAsync();
                Console.WriteLine($"Bluetooth.GetAvailabilityAsync:{available}");

                if(available)
                {
                    /*var dev2 = await BluetoothDevice.FromIdAsync("D5:C3:DD:2D:0B:88");
                    await dev2.Gatt.ConnectAsync();
                    var servs2 = await  dev2.Gatt.GetPrimaryServicesAsync();/


                    var devs = await Bluetooth.ScanForDevicesAsync();
                    //var devs = await Bluetooth.GetPairedDevicesAsync();
                    foreach(var dev in devs) 
                    {
                        Console.WriteLine($"Device {dev.Id} {dev.Name}");

                        if (dev.Name.Contains("nut"))
                        {
                            await dev.Gatt.ConnectAsync();

                            Console.WriteLine($"Gatt.IsConnected:{dev.Gatt.IsConnected}");

                            if (dev.Gatt.IsConnected)
                            {
                                var servs = await dev.Gatt.GetPrimaryServicesAsync();

                                foreach (var serv in servs)
                                {
                                    Console.WriteLine($"Service:{serv.Uuid}");

                                    var chars = await serv.GetCharacteristicsAsync();
                                    foreach (var gattchar in chars)
                                    {
                                        Console.WriteLine($"Characteristic:{gattchar.Uuid}");
                                    }
                                }

                                dev.Gatt.Disconnect();
                            }
                        }
                    }

                }*/
            });

            t.Wait();
        }
    }
}