//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;


namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    /// <summary>
    /// Represents an instance of a service on a remote Bluetooth device.
    /// </summary>
    public sealed partial class RfcommDeviceService
    {
        public static Task<RfcommDeviceService> FromIdAsync(string deviceId)
        {
            return FromIdAsyncImpl(deviceId);
        }

        /// <summary>
        /// Gets an Advanced Query Syntax (AQS) string for identifying instances of an RFCOMM service.
        /// </summary>
        /// <param name="serviceId">The service id for which to query.</param>
        /// <returns>An AQS string for identifying RFCOMM service instances.</returns>
        public static string GetDeviceSelector(RfcommServiceId serviceId)
        {
            return GetDeviceSelectorImpl(serviceId);
        }



#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE_81
        /// <summary>
        /// Gets the <see cref="BluetoothDevice"/> object describing the device associated with the current <see cref="RfcommDeviceService"/> object.
        /// </summary>
        public BluetoothDevice Device
        {
            get
            {
                return GetDevice();
            }
        }
#endif

        /// <summary>
        /// Gets the RfcommServiceId of this RFCOMM service instance.
        /// </summary>
        /// <value>The RfcommServiceId of the RFCOMM service instance.</value>
        public RfcommServiceId ServiceId
        {
            get
            {
                return GetServiceId();
            }
        }

        /// <summary>
        /// Connects to the remote service and returns a read/write Stream to communicate over.
        /// </summary>
        /// <returns>A <see cref="Stream"/> for reading and writing from the remote service. 
        /// Remember to Dispose of this Stream when you've finished working.</returns>
        public Task<Stream> OpenStreamAsync()
        {
            return OpenStreamAsyncImpl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return _service.ConnectionServiceName;
#else
            return _device.ToString() + "#" + _service.ToString();
#endif
        }
    }
}