//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTServer.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-21 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class RemoteGattServer
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
            if (Device.IsDisposedItem(Device)) return false;
            return Device.NativeDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
        }

        async Task PlatformConnect()
        {
            // Ensure that our native objects have not been disposed.
            // If they have, re-create the native device object.
            if (await Device.CreateNativeInstance()) PlatformInit();

            var status = await Device.NativeDevice.RequestAccessAsync();
            if(status == Windows.Devices.Enumeration.DeviceAccessStatus.Allowed)
            {
                Device.LastKnownAddress = Device.NativeDevice.BluetoothAddress;
                var session = await Windows.Devices.Bluetooth.GenericAttributeProfile.GattSession.FromDeviceIdAsync(Device.NativeDevice.BluetoothDeviceId);
                if(session != null)
                {
                    // Even though this is a local variable, we still want to add it to our dispose list so
                    // we don't have to rely on the GC to clean it up.
                    Device.AddDisposableObject(this, session);
                    session.MaintainConnection = true;
                }

                // need to request something to force a connection
                var services = await Device.NativeDevice.GetGattServicesAsync(cacheMode: Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
                foreach (var service in services.Services)
                {
                    service.Dispose();
                }
            }
            else
            {
                throw new SecurityException();
            }
        }

        void PlatformDisconnect()
        {
            // Windows has no explicit disconnect 🤪
        }

        void PlatformCleanup()
        {
            // The user has explicitly called the Disconnect method so unhook ConnectionStatusChanged
            // and dispose all of the native windows bluetooth objects.  This will release the device
            // so that it can be used by another application or re-connected by the current
            // application.
            if (Device.NativeDisposeList.TryGetValue(Device.GetHashCode(), out IDisposable existingDevice))
            {
                if (existingDevice != null)
                {
                    Device.NativeDevice.ConnectionStatusChanged -= NativeDevice_ConnectionStatusChanged;
                    Device.DisposeAllNativeObjects();
                }
            }
        }

        async Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            if (await Device.CreateNativeInstance()) PlatformInit();
            var result = await Device.NativeDevice.GetGattServicesForUuidAsync(service, Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);

            if (result == null || result.Services.Count == 0)
                return null;

            return new GattService(Device, result.Services[0], true);
        }

        async Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            if (await Device.CreateNativeInstance()) PlatformInit();
            var services = new List<GattService>();
            var nativeDevice = Device.NativeDevice;
            Windows.Devices.Bluetooth.BluetoothCacheMode cacheMode = nativeDevice.DeviceInformation.Pairing.IsPaired ? Windows.Devices.Bluetooth.BluetoothCacheMode.Cached : Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached;
            Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceServicesResult result;
            if (service == null)
            {
                result = await nativeDevice.GetGattServicesAsync(cacheMode);
            }
            else
            {
                result = await nativeDevice.GetGattServicesForUuidAsync(service.Value, cacheMode);
            }

            if (result != null && result.Services.Count > 0)
            {
                foreach(var serv in result.Services)
                {
                    services.Add(new GattService(Device, serv, true));
                }
            }
            
            return services;
        }

        Task<short> PlatformReadRssi()
        {
            return Task.FromResult((short)0);
        }

        void PlatformSetPreferredPhy(BluetoothPhy phy)
        {
        }
    }
}
