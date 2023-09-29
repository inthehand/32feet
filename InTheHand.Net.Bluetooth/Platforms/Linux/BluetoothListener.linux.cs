// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Linux)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    internal sealed class LinuxBluetoothListener : IBluetoothListener
    {
        private BluetoothEndPoint endPoint;
        private LinuxSocket socket;

        public bool Active { get; private set; }

        public ServiceClass ServiceClass { get; set; }
        public string ServiceName { get; set; }
        public ServiceRecord ServiceRecord { get; set; }
        public Guid ServiceUuid { get; set; }

        public void Start()
        {
            //TODO: Implement SDP publishing

            endPoint = new BluetoothEndPoint(BluetoothAddress.None, ServiceUuid, 0);

            socket = new LinuxSocket();
            socket.Bind(endPoint);
            Debug.WriteLine(socket.IsBound);
            socket.Listen(1);
            // get endpoint with channel
            endPoint = socket.LocalEndPoint as BluetoothEndPoint;

            Debug.WriteLine($"Listening on Port {endPoint.Port}");
        }

        public void Stop()
        {
            socket.Close();
            socket = null;
        }

        public bool Pending()
        {
            return socket.Poll(0, SelectMode.SelectRead);
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            LinuxSocket s = socket.Accept();

            return new BluetoothClient(new LinuxBluetoothClient(s));
        }
    }
}
