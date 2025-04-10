﻿//-----------------------------------------------------------------------
// <copyright file="Bluetooth.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static Adapter adapter;

        private static async Task Initialize()
        {
            if(adapter == null)
            {
                adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
                adapter.DeviceFound += Adapter_DeviceFound;
            }
        }

        private static async Task Adapter_DeviceFound(Adapter sender, DeviceFoundEventArgs eventArgs)
        {
            BluetoothDevice device = eventArgs.Device;
            await device.Init();
            var appearance = await eventArgs.Device.GetAppearanceAsync();
            BluetoothAdvertisingEvent eventInfo = new BluetoothAdvertisingEvent(device, appearance);
            OnAdvertisementReceived(eventInfo);
        }

        private static async Task<bool> PlatformGetAvailability()
        {
            await Initialize();
            return adapter != null && await adapter.GetPoweredAsync();
        }

        private static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            await Initialize();
            throw new PlatformNotSupportedException();
        }

        private static bool ContainsAddress(List<BluetoothDevice> devices, string address)
        {
            for (int i = devices.Count - 1; i >= 0; i--)
            {
                if (devices[i].Id == address)
                {
                    return true;
                }
            }

            return false;
        }

        private static void RemoveWithAddress(List<BluetoothDevice> devices, string address)
        {
            for(int i = devices.Count-1; i >=0; i--)
            {
                if (devices[i].Id == address)
                {
                    devices.RemoveAt(i);
                    return;
                }
            }
        }

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options, CancellationToken cancellationToken = default)
        {
            await Initialize();
            List<BluetoothDevice> devices = new List<BluetoothDevice>();
            TaskCompletionSource<IReadOnlyCollection<BluetoothDevice>> result = new TaskCompletionSource<IReadOnlyCollection<BluetoothDevice>>();
            async Task handler(Adapter sender, DeviceFoundEventArgs eventArgs)
            {
                string address = await eventArgs.Device.GetAddressAsync();

                if (eventArgs.IsStateChange)
                {
                    RemoveWithAddress(devices, address);
                }

                BluetoothDevice device = (BluetoothDevice)eventArgs.Device;
                if (!ContainsAddress(devices, address))
                {
                    await device.Init();
                    devices.Add(device);
                }
            }

            adapter.DeviceFound += handler;

            TimeSpan timeout = options.Timeout ?? TimeSpan.FromMilliseconds(60000);
            
            Task.Run(async () =>
            {
                try
                {
                    await adapter.StartDiscoveryAsync();
                    await Task.Delay(timeout, cancellationToken);
                    await adapter.StopDiscoveryAsync();
                    result.TrySetResult(devices);
                }
                catch (TaskCanceledException) 
                {
                    await adapter.StopDiscoveryAsync();
                    result.TrySetResult(devices); // if cancelled, return the devices found so far
                }
                finally
                {
                    adapter.DeviceFound -= handler;
                }
            }, CancellationToken.None);

            return await result.Task;
        }

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            await Initialize();
            List<BluetoothDevice> bluetoothDevices = new List<BluetoothDevice>();
                
            var devices = await adapter.GetDevicesAsync();

            if(devices != null && devices.Count > 0)
            {
                foreach(var device in devices)
                {
                    BluetoothDevice bluetoothDevice = (BluetoothDevice)device;
                    await bluetoothDevice.Init();
                    bluetoothDevices.Add(bluetoothDevice);
                }
            }

            return bluetoothDevices.AsReadOnly();
        }

        private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            await Initialize();
            throw new PlatformNotSupportedException();
        }

        private static void AddAvailabilityChanged()
        {
        }

        private static void RemoveAvailabilityChanged()
        {
        }
    }
}
