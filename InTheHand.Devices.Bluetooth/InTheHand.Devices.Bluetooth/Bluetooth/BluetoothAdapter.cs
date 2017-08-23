//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Represents a local Bluetooth adapter.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>
    public sealed partial class BluetoothAdapter
    {
        private static BluetoothAdapter s_default;

        /// <summary>
        /// Gets the default BluetoothAdapter.
        /// </summary>
        /// <returns>An asynchronous operation that completes with a BluetoothAdapter.</returns>
        public static Task<BluetoothAdapter> GetDefaultAsync()
        {
            return GetDefaultAsyncImpl();
        }

        internal static BluetoothAdapter Default
        {
            get
            {
                if(s_default == null)
                {
                    var t = GetDefaultAsync();
                    t.Wait();
                    s_default = t.Result;
                }

                return s_default;
            }
        }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress
        {
            get
            {
                return GetBluetoothAddress();
            }
        }

        public BluetoothClassOfDevice ClassOfDevice
        {
            get
            {
                return GetClassOfDevice();
            }
        }
        
        /// <summary>
        /// Gets a boolean indicating if the adapter supports the Bluetooth Classic transport type.
        /// </summary>
        /// <remarks>Always returns false for iOS, macOS and tvOS.</remarks>
        public bool IsClassicSupported
        {
            get
            {
                return GetIsClassicSupported();
            }
        }

        /// <summary>
        /// Gets a boolean indicating if the adapater supports Low Energy Bluetooth Transport type.
        /// </summary>
        /// <remarks>Always returns false for Windows Desktop.</remarks>
        public bool IsLowEnergySupported
        {
            get
            {
                return GetIsLowEnergySupported();
            }
        }

        /// <summary>
        /// Gets the Name of the adapter.
        /// </summary>
        /// <value>The name of the adapter.</value>
        /// <remarks>On most platforms this is not separately editable from the machine name.</remarks>
        public string Name
        {
            get
            {
                return GetName();
            }
        }
    }
}