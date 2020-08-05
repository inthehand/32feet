using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothRemoteGATTServer
    {
        bool GetConnected()
        {
            return Device.NativeDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
        }

        async Task DoConnect()
        {
            var status = await Device.NativeDevice.RequestAccessAsync();
        }

        void DoDisconnect()
        {

        }

        async Task<GattService> DoGetPrimaryService(BluetoothUuid service)
        {
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result = null;

            result = await Device.NativeDevice.GetGattServicesForUuidAsync(service);

            if (result == null || result.Services.Count == 0)
                return null;

            return new GattService(Device, result.Services[0]);
        }

        async Task<List<GattService>> DoGetPrimaryServices(BluetoothUuid? service)
        {
            var services = new List<GattService>();

            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result = null;

            if (service == null)
            {
                result = await Device.NativeDevice.GetGattServicesAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
            }
            else
            {
                result = await Device.NativeDevice.GetGattServicesForUuidAsync(service.Value, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
            }

            if (result != null && result.Services.Count > 0)
            {
                foreach(var serv in result.Services)
                {
                    services.Add(new GattService(Device, serv));
                }
            }
            
            return services;
        }
    }
}
