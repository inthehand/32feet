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
                case CBPeripheralState.Connecting:
                    return true;

                default:
                    return false;
            }
        }

        Task PlatformConnect()
        {
            return Task.Run(() =>
            {
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);
                Bluetooth.ConnectedPeripheral += (sender, peripheral) =>
                 {
                     if (peripheral.Identifier.IsEqual(((CBPeripheral)Device).Identifier))
                         handle.Set();
                 };
                Bluetooth.FailedToConnectPeripheral += (sender, peripheral) =>
                {
                    if (peripheral.Identifier.IsEqual(((CBPeripheral)Device).Identifier))
                        handle.Set();
                };

                Bluetooth._manager.ConnectPeripheral(Device
#if __IOS__
                    , new CBConnectPeripheralOptions { RequiresAncs = true }
#endif
                    );

                handle.WaitOne(5000);

                if (!Connected)
                    Bluetooth._manager.CancelPeripheralConnection(Device);
            });

        }

        void PlatformDisconnect()
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
                if (!tcs.Task.IsCompleted)
                {
                    tcs.SetResult((short)e.Rssi);
                }

                peripheral.RssiRead += handler;
            };

            peripheral.RssiRead += handler;
            peripheral.ReadRSSI();

            return tcs.Task;
        }
    }
}
