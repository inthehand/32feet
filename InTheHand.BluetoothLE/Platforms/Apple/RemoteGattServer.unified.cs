//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if !__MACOS__
using UIKit;
#endif

namespace InTheHand.Bluetooth
{
    partial class RemoteGattServer
    {
        private void PlatformInit()
        {
        }

        bool GetConnected()
        {
            switch (((CBPeripheral)Device).State)
            {
                case CBPeripheralState.Connected:
                    return true;

                default:
                    return false;
            }
        }

        Task PlatformConnect()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void connectedHandler(object sender, CBPeripheralEventArgs e)
             {
                 if (e.Peripheral.Identifier.IsEqual(((CBPeripheral)Device).Identifier))
                 {
                    Bluetooth.ConnectedPeripheral -= connectedHandler;
                    Bluetooth.FailedToConnectPeripheral -= failedConnectHandler;
                    tcs.SetResult(true);
                 }
             };

            void failedConnectHandler(object sender, CBPeripheralErrorEventArgs e)
            {
                if (e.Peripheral.Identifier.IsEqual(((CBPeripheral)Device).Identifier))
                {
                    Bluetooth.ConnectedPeripheral -= connectedHandler;
                    Bluetooth.FailedToConnectPeripheral -= failedConnectHandler;
                    tcs.SetResult(false);
                }
            };

            Bluetooth.ConnectedPeripheral += connectedHandler;
            Bluetooth.FailedToConnectPeripheral += failedConnectHandler;
#if __IOS__
            if (Device.RequiresAncs)
            {
                Bluetooth._manager.ConnectPeripheral(Device, new CBConnectPeripheralOptions { RequiresAncs = Device.RequiresAncs });
            }
            else
            {
#endif
                Bluetooth._manager.ConnectPeripheral(Device);
#if __IOS__
            }
#endif

            switch(((CBPeripheral)Device).State)
            {
                case CBPeripheralState.Connected:
                    Bluetooth.ConnectedPeripheral -= connectedHandler;
                    Bluetooth.FailedToConnectPeripheral -= failedConnectHandler;
                    return Task.CompletedTask;

                case CBPeripheralState.Connecting:
                    Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        if (!tcs.Task.IsCompletedSuccessfully)
                        {
                            tcs.SetResult(false);
                        }
                    });
                    return tcs.Task;

                default:
                    Bluetooth._manager.CancelPeripheralConnection(Device);
                    return Task.CompletedTask;
            }
        }

        void PlatformDisconnect()
        {
        }

        void PlatformCleanup()
        {
        }

        Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            return Task.Run(() =>
            {
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                GattService matchingService = null;

                ((CBPeripheral)Device).DiscoveredService += (sender, args) =>
                 {
                     handle.Set();
                 };

                ((CBPeripheral)Device).DiscoverServices(new CBUUID[] { service });
                
                handle.WaitOne();

                foreach (CBService cbservice in ((CBPeripheral)Device).Services)
                {
                    if ((BluetoothUuid)cbservice.UUID == service)
                    {
                        matchingService = new GattService(Device, cbservice);
                    }
                }

                return matchingService;
            });
        }

        Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            return Task.Run(() =>
            {
                var services = new List<GattService>();
            
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                ((CBPeripheral)Device).DiscoveredService += (sender, args) =>
                {
                    handle.Set();
                };

                if (service.HasValue)
                {
                    ((CBPeripheral)Device).DiscoverServices(new CBUUID[] { service });
                }
                else
                {
                    ((CBPeripheral)Device).DiscoverServices();
                }

                handle.WaitOne();

                foreach (CBService cbservice in ((CBPeripheral)Device).Services)
                {
                    services.Add(new GattService(Device, cbservice));
                }

                return services;
            });
        }

        Task<short> PlatformReadRssi()
        {
            TaskCompletionSource<short> tcs = new TaskCompletionSource<short>();
            var peripheral = (CBPeripheral)Device;

            void handler(object s, CBRssiEventArgs e)
            {
                peripheral.RssiRead -= handler;

                if (!tcs.Task.IsCompleted)
                {
                    tcs.SetResult(e.Rssi.Int16Value);
                }
            };

            peripheral.RssiRead += handler;
            peripheral.ReadRSSI();

            return tcs.Task;
        }

        void PlatformSetPreferredPhy(BluetoothPhy phy)
        {
        }
    }
}
