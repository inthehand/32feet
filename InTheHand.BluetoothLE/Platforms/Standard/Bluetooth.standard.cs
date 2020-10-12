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
        private static Task<bool> PlatformGetAvailability()
        {
            return Task.FromResult(false);
        }

        private static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromException<BluetoothDevice>(new PlatformNotSupportedException());
        }

        private static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
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
