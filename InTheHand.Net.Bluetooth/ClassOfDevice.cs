// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.ClassOfDevice
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Diagnostics;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Describes the device and service capabilities of a device.
    /// </summary>
    /// -
    /// <remarks>
    /// <para>Is returned by the properties
    /// <see cref="P:InTheHand.Net.Sockets.BluetoothDeviceInfo.ClassOfDevice">BluetoothDeviceInfo.ClassOfDevice</see>
    /// and
    /// <see cref="P:InTheHand.Net.Bluetooth.BluetoothRadio.ClassOfDevice">BluetoothRadio.ClassOfDevice</see>.
    /// </para>
    /// </remarks>
#if !UAP
    [Serializable]
#endif
    public sealed class ClassOfDevice : IEquatable<ClassOfDevice>
    {
        private readonly uint _cod;

        /// <summary>
        /// Initialize a new instance of class <see cref="T:InTheHand.Net.Bluetooth.ClassOfDevice"/>.
        /// </summary>
        /// -
        /// <remarks>
        /// <para>An example raw value is 0x00020104, which stands for
        /// device: DesktopComputer, service: Network.
        /// </para>
        /// </remarks>
        /// -
        /// <param name="cod">A <see cref="T:System.UInt32"/> containing the
        /// raw Class of Device value.
        /// </param>
        internal ClassOfDevice(uint cod)
        {
            _cod = cod;
        }
        
        [CLSCompliant(false)]
        public static implicit operator uint(ClassOfDevice cod)
        {
            if (cod == null)
                return 0;

            return cod._cod;
        }

        [CLSCompliant(false)]
        public static explicit operator ClassOfDevice(uint cod)
        {
            return new ClassOfDevice(cod);
        }

        /// <summary>
        /// Initialize a new instance of class <see cref="T:InTheHand.Net.Bluetooth.ClassOfDevice"/>.
        /// </summary>
        /// -
        /// <param name="device">A <see cref="T:InTheHand.Net.Bluetooth.DeviceClass"/>
        /// value.
        /// </param>
        /// <param name="service">A <see cref="T:InTheHand.Net.Bluetooth.ServiceClass"/>
        /// value.
        /// </param>
        public ClassOfDevice(DeviceClass device, ServiceClass service)
        {
            var scU = ((uint)service) << 13;
            _cod = (uint)device | scU;
        }

        /// <summary>
        /// Returns the device type.
        /// </summary>
        public DeviceClass Device
        {
            get
            {
                return (DeviceClass)(_cod & 0x001ffc);
            }
        }

        /// <summary>
        /// Returns the major device type.
        /// </summary>
        public DeviceClass MajorDevice
        {
            get
            {
                return (DeviceClass)(_cod & 0x001f00);
            }
        }

        /// <summary>
        /// Returns supported service types.
        /// </summary>
        public ServiceClass Service
        {
            get
            {
                return (ServiceClass)(_cod >> 13);
            }
        }

        /// <summary>
        /// Gets the numerical value.
        /// </summary>
        /// <seealso cref="P:InTheHand.Net.Bluetooth.ClassOfDevice.ValueAsInt32"/>
        [CLSCompliant(false)]//use ValueAsInt32
        public uint Value
        {
            get { return _cod; }
        }

        /// <summary>
        /// Gets the numerical value, suitable for CLS Compliance.
        /// </summary>
        /// <seealso cref="P:InTheHand.Net.Bluetooth.ClassOfDevice.Value"/>
        [Obsolete("Use the UInt32 value",true)]
        [DebuggerHidden]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int ValueAsInt32
        {
            get { return unchecked((int)_cod); }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        public byte FormatType
        {
            get
            {
                return (byte)(cod & 0x03);
            }
        }*/

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _cod.GetHashCode();
        }

        /// <summary>
        /// Returns the numerical value represented in a hexadecimal.
        /// </summary>
        /// -
        /// <returns>A <see cref="T:System.String"/> containing
        /// the numerical value represented in a hexadecimal
        /// e.g. "720104", "5A020C".
        /// </returns>
        public override string ToString()
        {
            return _cod.ToString("X");
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified 
        /// object.
        /// </summary>
        /// <param name="obj">An object
        /// value to compare with the current instance.
        /// </param>
        /// <returns>true if <paramref name="obj"/> is an instance of <see cref="T:InTheHand.Net.Bluetooth.ClassOfDevice"/>
        /// and equals the value of this instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ClassOfDevice);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified 
        /// <see cref="T:InTheHand.Net.Bluetooth.ClassOfDevice"/> value.
        /// </summary>
        /// <param name="other">An <see cref="T:InTheHand.Net.Bluetooth.ClassOfDevice"/>
        /// value to compare with the current instance.
        /// </param>
        /// <returns>true if <paramref name="other"/>
        /// has the same value as this instance; otherwise, false.
        /// </returns>
        public bool Equals(ClassOfDevice other)
        {
            if (other == null)
                return false;
            return _cod == other._cod;
        }

    }
}
