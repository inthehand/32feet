// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.IrDADeviceInfo
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
	/// <summary>
	/// Provides information about remote devices connected by infrared communications.
	/// </summary>
    /// <seealso cref="T:System.Net.Sockets.IrDADeviceInfo"/>
	public class IrDADeviceInfo
	{
		internal IrDADeviceInfo(IrDAAddress id, string name, IrDAHints hints, IrDACharacterSet charset)
		{
			DeviceAddress = id;
			DeviceName = name;
            Hints = hints;
            CharacterSet = charset;
		}

		/// <summary>
		/// Returns the address of the remote device.
		/// </summary>
		public IrDAAddress DeviceAddress { get; private set; }

		/// <summary>
		/// Provided solely for compatibility with System.Net.IrDA - consider using <see cref="DeviceAddress"/> instead.
		/// </summary>
        [Obsolete("Use the DeviceAddress property to access the device Address.", false)]
        public byte[] DeviceID
		{
			get
			{
				return DeviceAddress.ToByteArray();
			}
		}

		/// <summary>
		/// Gets the name of the device.
		/// </summary>
		public string DeviceName { get; private set; }

		/// <summary>
		/// Gets the character set used by the server, such as ASCII.
		/// </summary>
		public IrDACharacterSet CharacterSet { get; private set; }

		/// <summary>
		/// Gets the type of the device, such as a computer.
		/// </summary>
		public IrDAHints Hints { get; private set; }

		/// <summary>
		/// Compares two <see cref="IrDADeviceInfo"/> instances for equality.
		/// </summary>
        /// -
        /// <param name="obj">The <see cref="IrDADeviceInfo"/>
        /// to compare with the current instance.
        /// </param>
        /// -
        /// <returns><c>true</c> if <paramref name="obj"/>
        /// is a <see cref="IrDADeviceInfo"/> and equal to the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
		public override bool Equals(object obj)
		{
            // objects are equal if device address matches
            if (obj is IrDADeviceInfo irdi)
            {
                return DeviceAddress.Equals(irdi.DeviceAddress);
            }

            return base.Equals(obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
        /// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return DeviceAddress.GetHashCode();
		}
	}
}
