//-----------------------------------------------------------------------
// <copyright file="RequestDeviceOptions.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-22 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Specifies options to use when requesting access to a device.
    /// </summary>
    public sealed class RequestDeviceOptions
    {
        /// <summary>
        /// Filters which define the services or attributes the remote device must support.
        /// </summary>
        public IList<BluetoothLEScanFilter> Filters { get; } = new List<BluetoothLEScanFilter>();

        /// <summary>
        /// Additional services which are not required but the client will have access to.
        /// </summary>
        /// <remarks>Not used as unlike the web we do not restrict which services are accessible.</remarks>
        public IList<Guid> OptionalServices { get; } = new List<Guid>();

        /// <summary>
        /// If set the request returns all available devices with no filters applied.
        /// </summary>
        public bool AcceptAllDevices { get; set; }
    }
}
