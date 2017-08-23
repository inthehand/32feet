//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceServicesResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// The result of an Rfcomm device service request.
    /// </summary>
    public sealed partial class RfcommDeviceServicesResult
    {
        private BluetoothError _error;
        private IReadOnlyList<RfcommDeviceService> _services;

        internal RfcommDeviceServicesResult(BluetoothError error, IReadOnlyList<RfcommDeviceService> services)
        {
            _error = error;
            _services = services;
        }

        /// <summary>
        /// Indicates that an error occurred.
        /// </summary>
        public BluetoothError Error
        {
            get
            {
                return _error;
            }
        }

        /// <summary>
        /// The collection of returned services.
        /// </summary>
        public IReadOnlyList<RfcommDeviceService> Services
        {
            get
            {
                return _services;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", nameof(RfcommDeviceServicesResult), Error);
        }
    }
}