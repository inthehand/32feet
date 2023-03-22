//-----------------------------------------------------------------------
// <copyright file="Bluetooth.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
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
            }
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

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options, CancellationToken cancellationToken = default)
        {
            await Initialize();
            List<BluetoothDevice> devices = new List<BluetoothDevice>();
            TaskCompletionSource<IReadOnlyCollection<BluetoothDevice>> result = new TaskCompletionSource<IReadOnlyCollection<BluetoothDevice>>();
            async Task handler(Adapter sender, DeviceFoundEventArgs eventArgs)
            {
                if(eventArgs.IsStateChange)
                {
                    devices.Remove(eventArgs.Device);
                }

                BluetoothDevice device = (BluetoothDevice)eventArgs.Device;
                await device.Init();
                devices.Add(eventArgs.Device);
            }

            adapter.DeviceFound += handler;
            
            Task.Run(async () =>
            {
                await adapter.StartDiscoveryAsync();
                await Task.Delay(5000);
                await adapter.StopDiscoveryAsync();
                result.TrySetResult(devices);
            }, cancellationToken);

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
