// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.BluetoothEndPoint (Win32)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Net;
using System.Net.Sockets;

namespace InTheHand.Net
{
    /// <summary>
    /// Represents a network endpoint as a Bluetooth address and a Service Class Id and/or a port number.
    /// </summary>
    public sealed class BluetoothEndPoint : EndPoint
    {
        private ulong _bluetoothAddress;
        private Guid _serviceId;

        /// <summary>
        /// Initializes a new instance of the BluetoothEndPoint class with the specified address and service.
        /// </summary>
        /// <param name="address">The Bluetooth address of the device.</param>
        /// <param name="service">The Bluetooth service to use.</param>
        public BluetoothEndPoint(BluetoothAddress address, Guid service) : this(address.ToUInt64(), service) { }

        internal BluetoothEndPoint(ulong bluetoothAddress, Guid serviceId)
        {
            _bluetoothAddress = bluetoothAddress;
            _serviceId = serviceId;
        }

        internal BluetoothEndPoint(byte[] sockaddr_bt)
        {
            if (sockaddr_bt[0] != 32)
                throw new ArgumentException(nameof(sockaddr_bt));

            byte[] addrbytes = new byte[8];

            for (int ibyte = 0; ibyte < 8; ibyte++)
            {
                addrbytes[ibyte] = sockaddr_bt[2 + ibyte];
            }
            _bluetoothAddress = BitConverter.ToUInt64(addrbytes, 0);

            byte[] servicebytes = new byte[16];

            for (int ibyte = 0; ibyte < 16; ibyte++)
            {
                servicebytes[ibyte] = sockaddr_bt[10 + ibyte];
            }

            _serviceId = new Guid(servicebytes);
        }

        /// <summary>
        /// Gets the address family of the Bluetooth address.
        /// </summary>
        public override AddressFamily AddressFamily
        {
            get
            {
                return (AddressFamily)32;
            }
        }

        /// <summary>
        /// Gets the Bluetooth address of the endpoint.
        /// </summary>
        public BluetoothAddress Address
        {
            get
            {
                return _bluetoothAddress;
            }
        }

        /// <summary>
        /// Gets the Bluetooth service to use for the connection.
        /// </summary>
        public Guid Service
        {
            get
            {
                return _serviceId;
            }
        }

        /// <summary>
        /// Creates an endpoint from a socket address.
        /// </summary>
        /// <param name="socketAddress"></param>
        /// <returns></returns>
        public override EndPoint Create(SocketAddress socketAddress)
        {
            if (socketAddress == null)
            {
                throw new ArgumentNullException("socketAddress");
            }

            if (socketAddress.Family == AddressFamily)
            {
                int ibyte;



                byte[] addrbytes = new byte[8];

                for (ibyte = 0; ibyte < 8; ibyte++)
                {
                    addrbytes[ibyte] = socketAddress[2 + ibyte];
                }
                ulong address = BitConverter.ToUInt64(addrbytes, 0);

                byte[] servicebytes = new byte[16];

                for (ibyte = 0; ibyte < 16; ibyte++)
                {
                    servicebytes[ibyte] = socketAddress[10 + ibyte];
                }

                return new BluetoothEndPoint(address, new Guid(servicebytes));
            }

            return base.Create(socketAddress);
        }

        /// <summary>
        /// Serializes endpoint information into a SocketAddress instance.
        /// </summary>
        /// <returns></returns>
        public override SocketAddress Serialize()
        {
            SocketAddress btsa = new SocketAddress(AddressFamily, 30);

            // copy address type
            btsa[0] = checked((byte)AddressFamily);

            // copy device id
            byte[] deviceidbytes = BitConverter.GetBytes(_bluetoothAddress);

            for (int idbyte = 0; idbyte < 6; idbyte++)
            {
                btsa[idbyte + 2] = deviceidbytes[idbyte];
            }

            // copy service clsid
            if (_serviceId != Guid.Empty)
            {
                byte[] servicebytes = _serviceId.ToByteArray();

                for (int servicebyte = 0; servicebyte < 16; servicebyte++)
                {
                    btsa[servicebyte + 10] = servicebytes[servicebyte];
                }
            }

            return btsa;
        }

        /// <summary>
        /// Returns the string representation of the BluetoothEndPoint.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _bluetoothAddress.ToString("X6") + ":" + _serviceId.ToString("D");
        }
    }
}
