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
            return DoGetAvailability();
        }

        /// <summary>
        /// Performs a device lookup and prompts the user for permission if required.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>A BluetoothDevice or null if unsuccessful.</returns>
        public static Task<BluetoothDevice> RequestDeviceAsync(RequestDeviceOptions options)
        {
            return PlatformRequestDevice(options);
        }

        private static event EventHandler _availabilityChanged;
        /// <summary>
        /// Bluetooth availability has changed, for example by disabling the radio or revoking user permission.
        /// </summary>
        public static event EventHandler AvailabilityChanged
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

        private static void OnAvailabilityChanged()
        {
            _availabilityChanged?.Invoke(null, EventArgs.Empty); ;
        }

        public static Task<IReadOnlyCollection<BluetoothDevice>> GetPairedDevicesAsync()
        {
            return PlatformGetPairedDevices();
        }

#if DEBUG
        public static Task<IReadOnlyCollection<BluetoothDevice>> ScanForDevicesAsync(RequestDeviceOptions options)
        {
            return PlatformScanForDevices(options);
        }

        
        public static Task<BluetoothLEScan> RequestLEScanAsync(BluetoothLEScanFilter filter)
        {
            return DoRequestLEScan(filter);
        }

        public static event EventHandler<BluetoothAdvertisingEvent> AdvertisementReceived;
#endif
    }
}
