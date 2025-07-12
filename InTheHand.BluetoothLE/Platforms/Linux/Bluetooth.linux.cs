//-----------------------------------------------------------------------
// <copyright file="Bluetooth.linux.cs" company="In The Hand Ltd">
//   Copyright (c) 2023-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;

namespace InTheHand.Bluetooth;

partial class Bluetooth
{
    internal static Adapter? Adapter;

    private static async Task Initialize()
    {
        if(Adapter == null)
        {
            Adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();

            if (Adapter == null)
                throw new PlatformNotSupportedException("No Bluetooth adapter present.");
                
            Adapter.DeviceFound += Adapter_DeviceFound;
        }
    }

    private static async Task Adapter_DeviceFound(Adapter sender, DeviceFoundEventArgs eventArgs)
    {
        BluetoothDevice device = eventArgs.Device;
        await device.Init();
        var appearance = await eventArgs.Device.GetAppearanceAsync();
        var eventInfo = new BluetoothAdvertisingEvent(device, appearance);
        OnAdvertisementReceived(eventInfo);
    }

    private static async Task<bool> PlatformGetAvailability()
    {
        await Initialize();
        return Adapter != null && await Adapter.GetPoweredAsync();
    }

    private static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
    {
        await Initialize();
        throw new PlatformNotSupportedException();
    }

    private static bool ContainsAddress(List<BluetoothDevice> devices, string address)
    {
        for (var i = devices.Count - 1; i >= 0; i--)
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
        for(var i = devices.Count-1; i >=0; i--)
        {
            if (devices[i].Id == address)
            {
                devices.RemoveAt(i);
                return;
            }
        }
    }

    private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions? options, CancellationToken cancellationToken = default)
    {
        await Initialize();
        var devices = new List<BluetoothDevice>();
        var result = new TaskCompletionSource<IReadOnlyCollection<BluetoothDevice>>();
        async Task handler(Adapter sender, DeviceFoundEventArgs eventArgs)
        {
            string address = await eventArgs.Device.GetAddressAsync();

            if (eventArgs.IsStateChange)
            {
                RemoveWithAddress(devices, address);
            }

            var device = (BluetoothDevice)eventArgs.Device;
            if (!ContainsAddress(devices, address))
            {
                await device.Init();
                devices.Add(device);
            }
        }

        Adapter.DeviceFound += handler;

        var timeout = options?.Timeout ?? TimeSpan.FromMilliseconds(30000);
            
        Task.Run(async () =>
        {
            try
            {
                await Adapter.StartDiscoveryAsync();
                await Task.Delay(timeout, cancellationToken);
                await Adapter.StopDiscoveryAsync();
                result.TrySetResult(devices);
            }
            catch (TaskCanceledException) 
            {
                await Adapter.StopDiscoveryAsync();
                result.TrySetResult(devices); // if cancelled, return the devices found so far
            }
            finally
            {
                Adapter.DeviceFound -= handler;
            }
        }, CancellationToken.None);

        return await result.Task;
    }

    private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
    {
        await Initialize();
        var bluetoothDevices = new List<BluetoothDevice>();
                
        var devices = await Adapter.GetDevicesAsync();

        if(devices is { Count: > 0 })
        {
            foreach(var device in devices)
            {
                var bluetoothDevice = (BluetoothDevice)device;
                await bluetoothDevice.Init();
                bluetoothDevices.Add(bluetoothDevice);
            }
        }

        return bluetoothDevices.AsReadOnly();
    }

    private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions? options)
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