using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTServer
    {
        bool GetConnected()
        {
            return Device.NativeDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
        }

        async Task DoConnect()
        {
            await Device.NativeDevice.RequestAccessAsync();
        }

        void DoDisconnect()
        {

        }

        async Task<BluetoothRemoteGATTService> DoGetPrimaryService(Guid? service)
        {
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result = null;

            if (service.HasValue)
            {
                result = await Device.NativeDevice.GetGattServicesForUuidAsync(service.Value);
            }
            else
            {
                result = await Device.NativeDevice.GetGattServicesAsync();
            }

            if (result == null || result.Services.Count == 0)
                return null;

            return new BluetoothRemoteGATTService(Device, result.Services[0]);
        }

        async Task<List<BluetoothRemoteGATTService>> DoGetPrimaryServices(Guid? service)
        {
            var services = new List<BluetoothRemoteGATTService>();

            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result = null;

            if (service == null)
            {
                result = await Device.NativeDevice.GetGattServicesAsync();
            }
            else
            {
                result = await Device.NativeDevice.GetGattServicesForUuidAsync(service.Value);
            }

            if (result != null || result.Services.Count > 0)
            {
                foreach(var serv in result.Services)
                {
                    services.Add(new BluetoothRemoteGATTService(Device, serv));
                }
            }
            
            return services;
        }
    }
}
