// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Provides information about an available device obtained by the client during device discovery.
    /// </summary>
    [DebuggerDisplay("({DeviceAddress.ToString(\"C\")}) {DeviceName}")]
    public abstract class BluetoothDeviceInfo : IEquatable<BluetoothDeviceInfo>
    {
        internal BluetoothDeviceInfo() { }

        /// <summary>
        /// Forces the system to refresh the device information.
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        public virtual BluetoothAddress DeviceAddress { get => BluetoothAddress.None; }

        /// <summary>
        /// Gets the name of a device.
        /// </summary>
        public virtual string DeviceName { get => string.Empty; }

        /// <summary>
        /// Returns the Class of Device of the remote device.
        /// </summary>
        public virtual ClassOfDevice ClassOfDevice { get => (ClassOfDevice)0; }

        /// <premliminary/>
        /// <summary>
        /// Gets a list of all available Rfcomm service UUIDs on the remote device.
        /// </summary>
        /// <param name="cached">If true and supported on the runtime platform return locally cached devices without doing an SDP request.</param>
        /// <returns>All available Rfcomm service UUIDs on the remote device.</returns>
        public virtual Task<IEnumerable<Guid>> GetRfcommServicesAsync(bool cached = true) => Task.FromException<IEnumerable<Guid>>(new PlatformNotSupportedException());

        /// <summary>
        /// Returns a list of services which are already installed for use on the calling machine.
        /// </summary>
        public virtual IReadOnlyCollection<Guid> InstalledServices { get => throw new PlatformNotSupportedException(); }

        /// <summary>
        /// Specifies whether the device is connected.
        /// </summary>
        public virtual bool Connected { get => false; }

        /// <summary>
        /// Specifies whether the device is authenticated, paired, or bonded.
        /// All authenticated devices are remembered.
        /// </summary>
        public virtual bool Authenticated { get => false; }

        /// <summary>
        /// Enables or disables services for a Bluetooth device.
        /// </summary>
        /// <remarks>Only applies to Windows platform.</remarks>
        /// <param name="service"></param>
        /// <param name="state"></param>
        public virtual void SetServiceState(Guid service, bool state) => throw new PlatformNotSupportedException();

        /// <summary>
        /// Compares two BluetoothDeviceInfo instances for equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BluetoothDeviceInfo other)
        {
            if (other is null)
                return false;

            return DeviceAddress == other.DeviceAddress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is BluetoothDeviceInfo info)
            {
                return Equals(info);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DeviceAddress.GetHashCode();
        }
    }
}
