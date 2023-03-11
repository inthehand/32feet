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
using InTheHand.Threading.Tasks;
using Linux.Bluetooth;
using Linux.Bluetooth.Extensions;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static Adapter adapter;

        static Bluetooth()
        {
            adapter = AsyncHelpers.RunSync(async () =>
            {
                Adapter adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
                return adapter;
            });
        }

        private static async Task<bool> PlatformGetAvailability()
        {
            return adapter != null && await adapter.GetPoweredAsync();
        }

        private static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromException<BluetoothDevice>(new PlatformNotSupportedException());
        }

        private static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options, CancellationToken cancellationToken = default)
        {
            return Task.FromException<IReadOnlyCollection<BluetoothDevice>>(new PlatformNotSupportedException());
        }

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            List<BluetoothDevice> bluetoothDevices = new List<BluetoothDevice>();
                
            var devices = await adapter.GetDevicesAsync();

            if(devices != null && devices.Count > 0)
            {
                foreach(var device in devices)
                {
                    bluetoothDevices.Add(device);
                }
            }

            return bluetoothDevices.AsReadOnly();
        }

        private static Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            return Task.FromException<BluetoothLEScan>(new PlatformNotSupportedException());
        }

        private static void AddAvailabilityChanged()
        {
        }

        private static void RemoveAvailabilityChanged()
        {
        }
    }
}
