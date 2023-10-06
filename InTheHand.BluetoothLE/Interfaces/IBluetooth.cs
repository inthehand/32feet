//-----------------------------------------------------------------------
// <copyright file="IBluetooth.cs" company="In The Hand Ltd">
//   Copyright (c) 2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    internal interface IBluetooth
    {
        Task<bool> GetAvailabilityAsync();

        event EventHandler AvailabilityChanged;

        Task<BluetoothDevice> RequestDeviceAsync(RequestDeviceOptions options);

        Task<IReadOnlyCollection<BluetoothDevice>> ScanForDevicesAsync(RequestDeviceOptions options, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<BluetoothDevice>> GetPairedDevicesAsync();

        Task<BluetoothLEScan> RequestLEScanAsync(BluetoothLEScanOptions options);

        event EventHandler<BluetoothAdvertisingEvent> AdvertisementReceived;
    }
}
