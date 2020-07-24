// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.IrDAEndPoint
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net
{
	/// <summary>
	/// Represents an end point for an infrared connection.
	/// </summary>
    /// <seealso cref="T:System.Net.IrDAEndPoint"/>
#if CODE_ANALYSIS
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId="EndPoint")]
#endif
    public class IrDAEndPoint : EndPoint
	{
		private const int MaxServiceNameBytes = 24;

		private IrDAAddress id;
		private string service;

        /// <summary>
		/// Initializes a new instance of the <see cref="IrDAEndPoint"/> class.
		/// </summary>
		/// <param name="irdaDeviceID">The device identifier.</param>
        /// <param name="serviceName">The Service Name to connect to/listen on eg "<c>OBEX</c>".
        /// In the very uncommon case where a connection is to be made to
        /// / a server is to listen on 
        /// a specific LSAP-SEL (port number), then use 
        /// the form "<c>LSAP-SELn</c>", where n is an integer.
        /// </param>
        [Obsolete("Use the constructor which accepts an IrDAAddress.", false)]
		public IrDAEndPoint(byte[] irdaDeviceID, string serviceName)
		{
            if (irdaDeviceID == null) 
			{
                throw new ArgumentNullException("irdaDeviceID");
            }

            this.id = new IrDAAddress(irdaDeviceID);
			this.service = serviceName ?? throw new ArgumentNullException("serviceName");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IrDAEndPoint"/> class.
		/// </summary>
		/// <param name="irdaDeviceAddress">The device address.</param>
        /// <param name="serviceName">The Service Name to connect to/listen on eg "<c>OBEX</c>".
        /// In the very uncommon case where a connection is to be made to
        /// / a server is to listen on 
        /// a specific LSAP-SEL (port number), then use 
        /// the form "<c>LSAP-SELn</c>", where n is an integer.
        /// </param>
        public IrDAEndPoint(IrDAAddress irdaDeviceAddress, string serviceName)
		{
            this.id = irdaDeviceAddress ?? throw new ArgumentNullException("irdaDeviceAddress");
			this.service = serviceName ?? throw new ArgumentNullException("serviceName");
        }

        /// <summary>
		/// Gets or sets an address for the device.
		/// </summary>
		public IrDAAddress Address
		{
			get
			{
				return id;
			}
			set
			{
                id = value ?? throw new ArgumentNullException("value");
			}
		}

		/// <summary>
		/// Gets or sets an identifier for the device.
		/// </summary>
        /// <exception cref="T:System.ArgumentNullException">
        /// The specified byte array is null (<c>Nothing</c> in Visual Basic).
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The specified byte array is not four bytes long.
        /// </exception>
        [Obsolete("Use the Address property to access the device Address.", false)]
		public byte[] DeviceID
		{
			get
			{
				return id.ToByteArray();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				if (value.Length != 4)
				{
                    throw new ArgumentOutOfRangeException("value", "DeviceID must be 4 bytes");
                }

				id = new IrDAAddress(value);
			}
        }

        /// <summary>
		/// Gets or sets the name of the service.
		/// </summary>
		public string ServiceName
		{
			get
			{
				return service;
			}
			set
			{
                service = value ?? throw new ArgumentNullException("value");
			}
        }

        /// <summary>
		/// Gets the address family to which the endpoint belongs.
		/// </summary>
		public override AddressFamily AddressFamily
		{
			get
			{
				return AddressFamily.Irda;
			}
        }

        /// <inheritdoc/>
		public override SocketAddress Serialize()
		{
			SocketAddress sa = new SocketAddress(AddressFamily.Irda, 32);
			
			byte[] b = id.ToByteArray();
			for(int ibyte = 0; ibyte < 4; ibyte++)
			{
				sa[ibyte+2] = b[ibyte];
			}
			
			byte[] buffer = System.Text.Encoding.ASCII.GetBytes(ServiceName);
            
            if (buffer.Length > MaxServiceNameBytes) {
                throw new InvalidOperationException("ServiceName has a maximum length of 24 bytes.");
            }
			for(int iservice = 0; iservice < buffer.Length; iservice++)
			{
				sa[iservice + 6] = buffer[iservice];
			}

			// Ensure null-terminated
            if(sa[30] != 0 || sa[31] != 0){
                throw new InvalidOperationException("ServiceName too long for SocketAddress.");
            }

			return sa;
        }

        /// <inheritdoc/>
		public override EndPoint Create(SocketAddress socketAddress)
		{
            if (socketAddress == null) {
                throw new ArgumentNullException("socketAddress");
            }
            //
			byte[] id = new byte[4];
			for(int ibyte = 0; ibyte < 4; ibyte++)
			{
				id[ibyte] = socketAddress[ibyte+2];
			}
			
			byte[] buffer = new byte[MaxServiceNameBytes];
			for(int iservice = 0; iservice < buffer.Length; iservice++)
			{
				buffer[iservice] = socketAddress[iservice + 6];
			}
			string name = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
			if(name.IndexOf('\0') > -1)
			{
				name = name.Substring(0, name.IndexOf('\0'));
			}

            return new IrDAEndPoint(new IrDAAddress(id), name);
        }

        /// <summary>
		/// Compares two <see cref="IrDAEndPoint"/> instances for equality.
		/// </summary>
        /// -
        /// <param name="obj">The <see cref="BluetoothEndPoint"/>
        /// to compare with the current instance.
        /// </param>
        /// -
        /// <returns><c>true</c> if <paramref name="obj"/>
        /// is a <see cref="IrDAEndPoint"/> and equal to the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
		public override bool Equals(object obj)
		{
            if (obj is IrDAEndPoint irep)
            {
                return (Address.Equals(irep.Address) && ServiceName.Equals(irep.ServiceName));
            }

            return base.Equals(obj);
        }

        /// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
		{
			unchecked
			{
				// Choose large primes to avoid hashing collisions
				const int HashingBase = (int)2166136261;
				const int HashingMultiplier = 16777619;

				int hash = HashingBase;
				hash = (hash * HashingMultiplier) ^ (Address is object ? Address.GetHashCode() : 0);
				hash = (hash * HashingMultiplier) ^ (ServiceName is object ? ServiceName.GetHashCode() : 0);
				return hash;
			}
        }

        /// <summary>
        /// Returns the string representation of the IrDAEndPoint.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The string is in format <c>&lt;DeviceAddress&gt;:&lt;ServiceName&gt;</c>
        /// </para>
        /// An example is:
        /// <code lang="none">"04E20304:OBEX"</code>
        /// </remarks>
        /// <returns>The string representation of the IrDAEndPoint.</returns>
        public override string ToString()
        {
            return Address.ToString() + ":" + ServiceName;
        }
	}
}
