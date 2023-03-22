//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class RemoteGattServer
    {
        private void PlatformInit()
        {
            Task.Run(async () =>
            {
                _connected = await Device._device.GetConnectedAsync();
            });
        }

        bool _connected;

        bool GetConnected()
        {
            return _connected;
        }

        async Task PlatformConnect()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            await Device._device.ConnectAsync();
            await Device._device.WaitForPropertyValueAsync("Connected", value: true, timeout: timeout);
            await Device._device.WaitForPropertyValueAsync("ServicesResolved", value: true, timeout: timeout);
            _connected = await Device._device.GetConnectedAsync();
        }

        async void PlatformDisconnect()
        {
            await Device._device.DisconnectAsync();
            _connected = await Device._device.GetConnectedAsync();
        }

        void PlatformCleanup()
        {
        }

        async Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            string uuid = service.Value.ToString();
            var gattService = await Device._device.GetServiceAsync(uuid);
            if(gattService != null)
            {
                GattService returnedService = new GattService(Device, gattService, service);
                await returnedService.Init();
                return returnedService;
            }
            
            return null;
        }

        async Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            List<GattService> services = new List<GattService>();
            var servs = await Device._device.GetServicesAsync();
            if (servs != null && servs.Count > 0)
            {
                foreach (var linuxService in servs)
                {
                    string uuid = await linuxService.GetUUIDAsync();
                    if(service == null || service.Value.Value.ToString() == uuid)
                    {
                        GattService returnedService = new GattService(Device, linuxService, Guid.Parse(uuid));
                        await returnedService.Init();
                        services.Add(returnedService);
                    }
                }
            }

            return services;
        }

        async Task<short> PlatformReadRssi()
        {
            return await Device._device.GetRSSIAsync();
        }

        void PlatformSetPreferredPhy(BluetoothPhy phy)
        {
        }
        
        bool PlatformRequestMtu(int mtu)
        {
            return false;
        }

    }
}
