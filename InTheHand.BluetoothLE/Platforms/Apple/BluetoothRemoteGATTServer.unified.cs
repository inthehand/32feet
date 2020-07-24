//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTServer.unified.cs" company="In The Hand Ltd">
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
    partial class BluetoothRemoteGATTServer
    {
        bool GetConnected()
        {
#if __TVOS__ || __WATCHOS__
            throw new PlatformNotSupportedException();
#else
            return ((CBPeripheral)Device).IsConnected;
#endif
        }

        Task DoConnect()
        {
            return Task.Run(() =>
            {
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);
                Bluetooth._manager.ConnectedPeripheral += (sender, args) =>
                 {
                     if (args.Peripheral == (CBPeripheral)Device)
                         handle.Set();
                 };
                Bluetooth._manager.ConnectPeripheral(Device);

                handle.WaitOne();
            });

        }

        void DoDisconnect()
        {
        }

        Task<GattService> DoGetPrimaryService(BluetoothUuid service)
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

        Task<List<GattService>> DoGetPrimaryServices(BluetoothUuid? service)
        {
            return Task.Run(() =>
            {
                var services = new List<GattService>();
            
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                GattService matchingService = null;

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
    }
}
