//-----------------------------------------------------------------------
// <copyright file="Bluetooth.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    public sealed partial class Bluetooth
    {
        /// <summary>
        /// Returns true if Bluetooth is physically available and user has given the app permission.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetAvailability()
        {
            return DoGetAvailability();
        }

        /// <summary>
        /// Performs a device lookup and prompts the user for permission if required.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>A BluetoothDevice or null if unsuccessful.</returns>
        public Task<BluetoothDevice> RequestDevice(RequestDeviceOptions options)
        {
            return PlatformRequestDevice(options);
        }

        private event EventHandler _availabilityChanged;
        /// <summary>
        /// Bluetooth availability has changed, for example by disabling the radio or revoking user permission.
        /// </summary>
        public event EventHandler AvailabilityChanged
        {
            add
            {
                if (_availabilityChanged == null)
                {
                    AddAvailabilityChanged();
                }

                _availabilityChanged += value;
            }
            remove
            {
                _availabilityChanged -= value;

                if (_availabilityChanged == null)
                {
                    RemoveAvailabilityChanged();
                }
            }
        }

        private void OnAvailabilityChanged()
        {
            _availabilityChanged?.Invoke(this, EventArgs.Empty);
        }

        public Task<IReadOnlyCollection<BluetoothDevice>> GetPairedDevices()
        {
            return PlatformGetPairedDevices();
        }

#if DEBUG
        public Task<IReadOnlyCollection<BluetoothDevice>> ScanForDevices(RequestDeviceOptions options)
        {
            return PlatformScanForDevices(options);
        }

        
        public Task<BluetoothLEScan> RequestLEScan(BluetoothLEScanFilter filter)
        {
            return DoRequestLEScan(filter);
        }

        public event EventHandler<BluetoothAdvertisingEvent> AdvertisementReceived;
#endif
    }
}
