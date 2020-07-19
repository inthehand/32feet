using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InTheHand.Bluetooth;
using InTheHand.Bluetooth.GenericAttributeProfile;
using System.Diagnostics;

namespace BluetoothClientApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Appearing += MainPage_Appearing;
        }

        
        private async void MainPage_Appearing(object sender, EventArgs e)
        {

            foreach(var prop in typeof(GattServiceUuids).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                BluetoothUuid val = (BluetoothUuid)prop.GetValue(null);
                ushort shortVal = (ushort)val;
                BluetoothUuid convertedVal = shortVal;
            }


            Bluetooth b = new Bluetooth();
            b.AdvertisementReceived += B_AdvertisementReceived;
            //b.RequestLEScan(new BluetoothLEScan() { AcceptAllAdvertisements = true });

            var device = await b.RequestDevice(new RequestDeviceOptions() { AcceptAllDevices = true });
            if (device != null)
            {
                var servs = await device.Gatt.GetPrimaryServices();
                foreach (var serv in servs)
                {
                    System.Diagnostics.Debug.WriteLine($"{serv.Uuid} Primary:{serv.IsPrimary}");


                    if(serv.Uuid == GattServiceUuids.Battery)
                    {
                        var batchar = await serv.GetCharacteristicAsync(GattCharacteristicUuids.BatteryLevel);
                        var batval = await batchar.ReadValueAsync();
                        //battery percent stored as a single byte
                        System.Diagnostics.Debug.WriteLine($"Battery: {batval[0]}");
                    }

                    System.Diagnostics.Debug.Indent();

                    foreach(var chars in await serv.GetCharacteristicsAsync())
                    {
                        System.Diagnostics.Debug.WriteLine($"{chars.Uuid} UserDescription:{chars.UserDescription} Properties:{chars.Properties}");

                         var val = await chars.ReadValueAsync();
                        System.Diagnostics.Debug.WriteLine(ByteArrayToString(val));
                        if (chars.Uuid == GattCharacteristicUuids.DeviceName)
                        {
                            Debug.WriteLine($"DeviceName:{System.Text.Encoding.UTF8.GetString(val)}");
                        }

                        System.Diagnostics.Debug.Indent();

                        foreach (var descriptors in await chars.GetDescriptorsAsync())
                        {
                            System.Diagnostics.Debug.WriteLine($"Descriptor:{descriptors.Uuid}");

                            var val2 = await descriptors.ReadValueAsync();
                                
                            if (descriptors.Uuid == GattDescriptorUuids.ClientCharacteristicConfiguration)
                            {
                                System.Diagnostics.Debug.WriteLine($"Notifying:{val2[0] > 0}");
                            }
                            else if(descriptors.Uuid == GattDescriptorUuids.CharacteristicUserDescription)
                            {
                                System.Diagnostics.Debug.WriteLine($"UserDescription:{ByteArrayToString(val2)}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine(ByteArrayToString(val2));
                            }

                        }

                        System.Diagnostics.Debug.Unindent();
                    }

                    System.Diagnostics.Debug.Unindent();
                }
            }
        }

        private void B_AdvertisementReceived(object sender, BluetoothAdvertisingEvent e)
        {
            System.Diagnostics.Debug.WriteLine($"Device:{e.Device} Name:{e.Name} Rssi:{e.Rssi} TxPower:{e.TxPower} Appearance:{e.Appearance}");
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
