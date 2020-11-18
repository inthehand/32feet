//-----------------------------------------------------------------------
// <copyright file="Bluetooth.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Entry point to all Bluetooth LE functionality.
    /// </summary>
    /// <remarks>
    /// Several platforms require permissions for this functionality to work:-
    /// iOS 13 (and above) - Set NSBluetoothAlwaysUsageDescription in your Info.plist to a descriptive message to be displayed to the user
    /// iOS (older versions) - Set NSBluetoothPeripheralUsageDescription in your Info.plist to a descriptive message to be displayed to the user
    /// Android - Add <uses-permission android:name="android.permission.BLUETOOTH" /> to your android manifest. To enable scanning also add <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" /> and <uses-permission android:name="android.permission.COARSE_LOCATION" />
    /// Windows - Add the Bluetooth permission
    /// </remarks>
    public static partial class Bluetooth
    {
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

        private static void OnAvailabilityChanged()
        {
            availabilityChanged?.Invoke(null, EventArgs.Empty); ;
        }

        public static Task<IReadOnlyCollection<BluetoothDevice>> GetPairedDevicesAsync()
        {
            return PlatformGetPairedDevices();
        }

        public static Task<IReadOnlyCollection<BluetoothDevice>> ScanForDevicesAsync(RequestDeviceOptions options = null)
        {
            ThrowOnInvalidOptions(options);
            return PlatformScanForDevices(options);
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

    }
}
