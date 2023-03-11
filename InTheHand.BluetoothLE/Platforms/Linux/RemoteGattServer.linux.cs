//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Threading.Tasks;
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
        }

        bool GetConnected()
        {
            return AsyncHelpers.RunSync(() => { return Device._device.GetConnectedAsync(); });
        }

        async Task PlatformConnect()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            await Device._device.ConnectAsync();
            await Device._device.WaitForPropertyValueAsync("Connected", value: true, timeout: timeout);
            await Device._device.WaitForPropertyValueAsync("ServicesResolved", value: true, timeout: timeout);
        }

        async void PlatformDisconnect()
        {
            await Device._device.DisconnectAsync();
        }

        void PlatformCleanup()
        {
        }

        async Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            var gattService = await Device._device.GetServiceAsync(service.Value.ToString());
            return gattService == null ? null : new GattService(Device, gattService);
        }

        async Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            List<GattService> services = new List<GattService>();
            var servs = await Device._device.GetServicesAsync();
            if (servs != null && servs.Count > 0)
            {
                foreach (var linuxService in servs)
                {
                    if(service == null || service.Value.Value.ToString() == await linuxService.GetUUIDAsync())
                    services.Add(new GattService(Device, linuxService));
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
