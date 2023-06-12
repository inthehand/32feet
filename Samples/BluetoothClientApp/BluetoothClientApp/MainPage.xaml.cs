using InTheHand.Bluetooth;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BluetoothClientApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Appearing += MainPage_Appearing;
        }

        BluetoothLEScan scan;

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            Debug.WriteLine("MainPage");

            bool availability = false;

            /*while (!availability)
            {
                availability = await Bluetooth.GetAvailabilityAsync();
                await Task.Delay(500);
            }*/

            foreach (var d in await Bluetooth.GetPairedDevicesAsync())
            {
                Debug.WriteLine($"{d.Id} {d.Name}");
            }

            Bluetooth.AdvertisementReceived += Bluetooth_AdvertisementReceived;
            scan = await Bluetooth.RequestLEScanAsync();

            RequestDeviceOptions options = new RequestDeviceOptions();
            options.AcceptAllDevices = true;
            BluetoothDevice device = await Bluetooth.RequestDeviceAsync(options);
            if (device != null)
            {
                device.GattServerDisconnected += Device_GattServerDisconnected;
                await device.Gatt.ConnectAsync();

                var servs = await device.Gatt.GetPrimaryServicesAsync();

                foreach (var serv in servs)
                {
                    var rssi = await device.Gatt.ReadRssi();
                    Debug.WriteLine($"{rssi} {serv.Uuid} Primary:{serv.IsPrimary}");

                    Debug.Indent();

                    foreach (var chars in await serv.GetCharacteristicsAsync())
                    {
                        Debug.WriteLine($"{chars.Uuid} Properties:{chars.Properties}");

                        Debug.Indent();

                        foreach (var descriptors in await chars.GetDescriptorsAsync())
                        {
                            Debug.WriteLine($"Descriptor:{descriptors.Uuid}");

                            var val2 = await descriptors.ReadValueAsync();

                            if (descriptors.Uuid == GattDescriptorUuids.ClientCharacteristicConfiguration)
                            {
                                Debug.WriteLine($"Notifying:{val2[0] > 0}");
                            }
                            else if (descriptors.Uuid == GattDescriptorUuids.CharacteristicUserDescription)
                            {
                                Debug.WriteLine($"UserDescription:{ByteArrayToString(val2)}");
                            }
                            else
                            {
                                Debug.WriteLine(ByteArrayToString(val2));
                            }

                        }

                        Debug.Unindent();
                    }

                    Debug.Unindent();
                }
            }
        }

        private void Bluetooth_AdvertisementReceived(object sender, BluetoothAdvertisingEvent e)
        {
            Debug.WriteLine($"Name:{e.Name} Rssi:{e.Rssi}");
        }

        private async void Device_GattServerDisconnected(object sender, EventArgs e)
        {
            var device = sender as BluetoothDevice;
            await device.Gatt.ConnectAsync();
        }

        private static string ByteArrayToString(byte[] data)
        {
            if (data == null)
                return "<NULL>";

            StringBuilder sb = new StringBuilder();

            foreach(byte b in data)
            {
                sb.Append(b.ToString("X"));
            }

            return sb.ToString();
        }

    }
}
