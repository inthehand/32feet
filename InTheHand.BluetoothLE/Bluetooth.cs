//-----------------------------------------------------------------------
// <copyright file="Bluetooth.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Entry point to all Bluetooth LE functionality.
    /// </summary>
    /// <remarks>
    /// Several platforms require permissions for this functionality to work. See <see href="https://github.com/inthehand/32feet/wiki/Permissions">Permissions</see> for more details
    /// </remarks>
    public static partial class Bluetooth
    {
        private static bool _oldAvailability;

        /// <summary>
        /// Returns true if Bluetooth is physically available and user has given the app permission.
        /// </summary>
        /// <returns></returns>
        public static Task<bool> GetAvailabilityAsync()
        {
            return PlatformGetAvailability();
        }

        /// <summary>
        /// Performs a device lookup and prompts the user for permission if required.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>A BluetoothDevice or null if unsuccessful.</returns>
        public static Task<BluetoothDevice> RequestDeviceAsync(RequestDeviceOptions options = null)
        {
            ThrowOnInvalidOptions(options);
            return PlatformRequestDevice(options);
        }

        private static void ThrowOnInvalidOptions(RequestDeviceOptions options)
        {
            if (options != null)
            {
                if ((options.Filters != null && options.Filters.Count > 0) == options.AcceptAllDevices)
                    throw new ArgumentException("Cannot set both Filters and AcceptAllDevices");
            }
        }

        private static event EventHandler availabilityChanged;
        /// <summary>
        /// Bluetooth availability has changed, for example by disabling the radio or revoking user permission.
        /// </summary>
        public static event EventHandler AvailabilityChanged
        {
            add
            {
                if (availabilityChanged == null)
                {
                    AddAvailabilityChanged();
                }

                availabilityChanged += value;
            }
            remove
            {
                availabilityChanged -= value;

                if (availabilityChanged == null)
                {
                    RemoveAvailabilityChanged();
                }
            }
        }

        internal static async Task OnAvailabilityChanged()
        {
            bool newAvailability = await PlatformGetAvailability();
            if (newAvailability != _oldAvailability)
            {
                _oldAvailability = newAvailability;
                System.Diagnostics.Debug.WriteLine("Bluetooth.AvailiabilityChanged");
                availabilityChanged?.Invoke(null, EventArgs.Empty); ;
            }
        }

        public static Task<IReadOnlyCollection<BluetoothDevice>> GetPairedDevicesAsync()
        {
            return PlatformGetPairedDevices();
        }

        public static Task<IReadOnlyCollection<BluetoothDevice>> ScanForDevicesAsync(RequestDeviceOptions options = null, CancellationToken cancellationToken = default)
        {
            ThrowOnInvalidOptions(options);
            return PlatformScanForDevices(options, cancellationToken);
        }

        public static Task<BluetoothLEScan> RequestLEScanAsync(BluetoothLEScanOptions options = null)
        {
            return PlatformRequestLEScan(options);
        }

        public static event EventHandler<BluetoothAdvertisingEvent> AdvertisementReceived;

        internal static void OnAdvertisementReceived(BluetoothAdvertisingEvent advertisingEvent)
        {
            AdvertisementReceived?.Invoke(null, advertisingEvent);
        }

        // Used by Characteristic and Descriptor write operations
        internal static void ThrowOnInvalidAttributeValue(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length > 512)
                throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be larger than 512 bytes.");
        }
    }
}
