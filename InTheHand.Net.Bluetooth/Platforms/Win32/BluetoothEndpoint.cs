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
    public sealed class BluetoothEndPoint : EndPoint
    {
        private ulong _bluetoothAddress;
        private Guid _serviceId;
        
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

        public override AddressFamily AddressFamily
        {
            get
            {
                return (AddressFamily)32;
            }
        }

        public BluetoothAddress Address
        {
            get
            {
                return _bluetoothAddress;
            }
        }

        public Guid Service
        {
            get
            {
                return _serviceId;
            }
        }

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

        public override string ToString()
        {
            return _bluetoothAddress.ToString("X6") + ":" + _serviceId.ToString("D");
        }
    }
}