//-----------------------------------------------------------------------
// <copyright file="GattService.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    public sealed partial class GattService
    {
        internal GattService(BluetoothDevice device)
        {
            Device = device;
        }

        public BluetoothDevice Device { get; private set; }

        public BluetoothUuid Uuid { get { return GetUuid(); } }

        public bool IsPrimary { get { return GetIsPrimary(); } }

        public Task<GattCharacteristic> GetCharacteristicAsync(BluetoothUuid characteristic)
        {
            return DoGetCharacteristic(characteristic);
        }

        public Task<IReadOnlyList<GattCharacteristic>> GetCharacteristicsAsync()
        {
            return DoGetCharacteristics();
        }

        public Task<GattService> GetIncludedServiceAsync(BluetoothUuid service)
        {
            return DoGetIncludedServiceAsync(service);
        }

        public Task<IReadOnlyList<GattService>> GetIncludedServicesAsync()
        {
            return DoGetIncludedServicesAsync();
        }

#if DEBUG
        public event EventHandler ServiceAdded;
        public event EventHandler ServiceChanged;
        public event EventHandler ServiceRemoved;
#endif
    }
}
