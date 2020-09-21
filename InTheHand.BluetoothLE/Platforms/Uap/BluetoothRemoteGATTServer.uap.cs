using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothRemoteGATTServer
    {
        private void PlatformInit()
        {
            Device.NativeDevice.ConnectionStatusChanged += NativeDevice_ConnectionStatusChanged;
        }

        private void NativeDevice_ConnectionStatusChanged(Windows.Devices.Bluetooth.BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Disconnected)
                Device.OnGattServerDisconnected();
        }

        bool GetConnected()
        {
            return Device.NativeDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
        }

        async Task PlatformConnect()
        {
            var status = await Device.NativeDevice.RequestAccessAsync();
            if(status == Windows.Devices.Enumeration.DeviceAccessStatus.Allowed)
            {
                // need to request something to force a connection
                await Device.NativeDevice.GetGattServicesAsync(cacheMode: Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
            }
        }

        void PlatformDisconnect()
        {

        }

        async Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result = null;

            result = await Device.NativeDevice.GetGattServicesForUuidAsync(service, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

            if (result == null || result.Services.Count == 0)
                return null;

            return new GattService(Device, result.Services[0]);
        }

        async Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
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
