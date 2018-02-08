//-----------------------------------------------------------------------
// <copyright file="DeviceInformationPairing.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Contains information and enables pairing for a device.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class DeviceInformationPairing
    {
        /// <summary>
        /// Gets a value that indicates whether the device can be paired.
        /// </summary>
        /// <value>True if the device can be paired, otherwise false.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public bool CanPair
        {
            get
            {
#if WINDOWS_UWP
                return GetCanPair();
#elif __ANDROID__ || WIN32
                return !IsPaired;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets the <see cref="DeviceInformationCustomPairing"/> object necessary for custom pairing.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public DeviceInformationCustomPairing Custom
        {
            get
            {
#if WINDOWS_UWP || WIN32
                return GetCustom();

#else
                return null;
#endif              
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the device is currently paired.
        /// </summary>
        /// <value>True if the device is currently paired, otherwise false.</value>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public bool IsPaired
        {
            get
            {
#if __ANDROID__ || WINDOWS_UWP || WIN32
                return GetIsPaired();

#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Attempts to pair the device.
        /// </summary>
        /// <returns>The result of the pairing action.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Android</term><description>Android 4.4 and later (Requires BLUETOOTH_ADMIN permission)</description></item>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public Task<DevicePairingResult> PairAsync()
        {
#if __ANDROID__ ||  WIN32
            return Task.FromResult<DevicePairingResult>(DoPair());

#elif WINDOWS_UWP
            return DoPairAsync();

#else
            return Task.FromResult<DevicePairingResult>(null);
#endif
        }

        /// <summary>
        /// Attempts to unpair the device.
        /// </summary>
        /// <returns>The result of the unpairing action.</returns>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        public Task<DeviceUnpairingResult> UnpairAsync()
        {
#if __ANDROID__ || WIN32
            return Task.FromResult<DeviceUnpairingResult>(DoUnpair());

#elif WINDOWS_UWP
            return DoUnpairAsync();

#else
            return Task.FromResult<DeviceUnpairingResult>(null);
#endif
        }
    }
}