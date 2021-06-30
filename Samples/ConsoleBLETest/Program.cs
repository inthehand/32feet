using System;
using System.Threading.Tasks;
using InTheHand.Bluetooth;

namespace InTheHandBLETest
{
    class Program
    {
        static void Main(string[] args)
        {
            var discoveryTask = TestDeviceDiscovery();
            discoveryTask.Wait();
        }

        private static async Task<bool> TestDeviceDiscovery()
        {
            var discoveredDevices = await Bluetooth.ScanForDevicesAsync(); 
            Console.WriteLine($"found {discoveredDevices?.Count} devices");
            return discoveredDevices?.Count > 0;
        }
    }
}
