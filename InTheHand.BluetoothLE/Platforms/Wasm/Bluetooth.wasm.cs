//-----------------------------------------------------------------------
// <copyright file="Bluetooth.wasm.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        //[Inject]
        //public static IJSRuntime javaScriptRuntime { get; set; }

        private static async Task<bool> PlatformGetAvailability()
        {
            //bool available = await javaScriptRuntime.InvokeAsync<bool>("navigator.bluetooth.getAvailability()");
            return false;
        }

        private static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromException<BluetoothDevice>(new PlatformNotSupportedException());
        }

        private static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options, CancellationToken cancellationToken = default)
        {
            return Task.FromException<IReadOnlyCollection<BluetoothDevice>>(new PlatformNotSupportedException());
        }

        private static Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            return Task.FromException<IReadOnlyCollection<BluetoothDevice>>(new PlatformNotSupportedException());
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
