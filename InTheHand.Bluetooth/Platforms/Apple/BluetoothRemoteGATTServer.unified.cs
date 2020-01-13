using CoreBluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
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

        Task<BluetoothRemoteGATTService> DoGetPrimaryService(Guid? service)
        {
            return Task.Run(() =>
            {
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                BluetoothRemoteGATTService matchingService = null;

                ((CBPeripheral)Device).DiscoveredService += (sender, args) =>
                 {
                     handle.Set();
                 };

                if (service.HasValue)
                {
                    ((CBPeripheral)Device).DiscoverServices(new CBUUID[] { service.Value.ToCBUUID() });
                }
                else
                {
                    ((CBPeripheral)Device).DiscoverServices();
                }

                handle.WaitOne();

                foreach (CBService cbservice in ((CBPeripheral)Device).Services)
                {
                    if (cbservice.UUID.ToGuid() == service)
                    {
                        matchingService = new BluetoothRemoteGATTService(Device, cbservice);
                    }
                }

                return matchingService;
            });
        }

        Task<List<BluetoothRemoteGATTService>> DoGetPrimaryServices(Guid? service)
        {
            return Task.Run(() =>
            {
                var services = new List<BluetoothRemoteGATTService>();
            
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

                BluetoothRemoteGATTService matchingService = null;

                ((CBPeripheral)Device).DiscoveredService += (sender, args) =>
                {
                    handle.Set();
                };

                if (service.HasValue)
                {
                    ((CBPeripheral)Device).DiscoverServices(new CBUUID[] { service.Value.ToCBUUID() });
                }
                else
                {
                    ((CBPeripheral)Device).DiscoverServices();
                }

                handle.WaitOne();

                foreach (CBService cbservice in ((CBPeripheral)Device).Services)
                {
                    services.Add(new BluetoothRemoteGATTService(Device, cbservice));
                }

                return services;
            });
        }
    }
}
