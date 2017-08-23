//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Descriptor of a GATT Characteristic.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.1 or later</description></item></list>
    /// </remarks>
    public sealed partial class GattDescriptor
    {
        /// <summary>
        /// Performs a Descriptor Value read from a value cache maintained by the system.
        /// </summary>
        /// <returns></returns>
        public Task<GattReadResult> ReadValueAsync()
        {
            return ReadValueAsync(BluetoothCacheMode.Cached);
        }

        /// <summary>
        /// Performs a Descriptor Value read either from the value cache maintained by the system, or directly from the device.
        /// </summary>
        /// <param name="cacheMode">Specifies whether to read the value directly from the device or from a value cache maintained by the system.</param>
        /// <returns></returns>
        public async Task<GattReadResult> ReadValueAsync(BluetoothCacheMode cacheMode)
        {
            return await DoReadValueAsync(cacheMode);
        }

        /// <summary>
        /// Gets the GATT Descriptor UUID for this GattDescriptor.
        /// </summary>
        public Guid Uuid
        {
            get
            {
                return GetUuid();
            }
        }
    }
}