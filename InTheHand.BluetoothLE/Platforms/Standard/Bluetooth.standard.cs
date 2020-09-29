//-----------------------------------------------------------------------
// <copyright file="Bluetooth.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        static Task<bool> DoGetAvailability()
        {
            return Task.FromResult(false);
        }

        static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromResult((BluetoothDevice)null);
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

        private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            return null;
        }

        private static void AddAvailabilityChanged()
        {
        }

        private static void RemoveAvailabilityChanged()
        {
        }
    }
}
