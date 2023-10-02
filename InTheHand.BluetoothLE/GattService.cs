//-----------------------------------------------------------------------
// <copyright file="GattService.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents a GATT Service, a collection of characteristics and relationships to other services that encapsulate the behavior of part of a device.
    /// </summary>
    [DebuggerDisplay("{Uuid} (Service)")]
    public sealed partial class GattService
    {
        internal GattService(BluetoothDevice device)
        {
            Device = device;
        }

        /// <summary>
        /// The <see cref="BluetoothDevice"/> representing the remote peripheral that the GATT service belongs to.
        /// </summary>
        public BluetoothDevice Device { get; private set; }

        /// <summary>
        /// The UUID of the service, e.g. '0000180d-0000-1000-8000-00805f9b34fb' for the Heart Rate service.
        /// </summary>
        public BluetoothUuid Uuid { get { return GetUuid(); } }

        /// <summary>
        /// Indicates whether the type of this service is primary or secondary.
        /// </summary>
        public bool IsPrimary { get { return GetIsPrimary(); } }

        /// <summary>
        /// Retrieves a Characteristic inside this Service.
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public Task<GattCharacteristic> GetCharacteristicAsync(BluetoothUuid characteristic)
        {
            return PlatformGetCharacteristic(characteristic);
        }

        /// <summary>
        /// Retrieves a list of Characteristics inside this Service.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<GattCharacteristic>> GetCharacteristicsAsync()
        {
            return PlatformGetCharacteristics();
        }

        /// <summary>
        /// Retrieves an Included Service inside this Service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Task<GattService> GetIncludedServiceAsync(BluetoothUuid service)
        {
            return PlatformGetIncludedServiceAsync(service);
        }

        /// <summary>
        /// Retrieves a list of Included Services inside this Service.
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyList<GattService>> GetIncludedServicesAsync()
        {
            return PlatformGetIncludedServicesAsync();
        }

#if DEBUG
        public event EventHandler ServiceAdded;
        public event EventHandler ServiceChanged;
        public event EventHandler ServiceRemoved;
#endif
    }
}
