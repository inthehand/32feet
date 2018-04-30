//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceServicesResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// The result of an Rfcomm device service request.
    /// </summary>
    public sealed partial class RfcommDeviceServicesResult
    {
        private BluetoothError _error;
#if UNITY
        private ReadOnlyCollection<RfcommDeviceService> _services;

        internal RfcommDeviceServicesResult(BluetoothError error, ReadOnlyCollection<RfcommDeviceService> services)
#else
        private IReadOnlyList<RfcommDeviceService> _services;

        internal RfcommDeviceServicesResult(BluetoothError error, IReadOnlyList<RfcommDeviceService> services)
#endif
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
#if UNITY
        public ReadOnlyCollection<RfcommDeviceService> Services
#else
        public IReadOnlyList<RfcommDeviceService> Services
#endif
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