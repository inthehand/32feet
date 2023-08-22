// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Linux)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        private BluetoothEndPoint endPoint;
        private LinuxSocket socket;

        void PlatformStart()
        {
            endPoint = new BluetoothEndPoint(BluetoothAddress.None, serviceUuid, 0);

            socket = new LinuxSocket();
            socket.Bind(endPoint);
            Debug.WriteLine(socket.IsBound);
            socket.Listen(1);
            // get endpoint with channel
            endPoint = socket.LocalEndPoint as BluetoothEndPoint;

            Debug.WriteLine($"Listening on Port {endPoint.Port}");
        }

        void PlatformStop()
        {
            socket.Close();
            socket = null;
        }

        bool PlatformPending()
        {
            return socket.Poll(0, SelectMode.SelectRead);
        }

        BluetoothClient PlatformAcceptBluetoothClient()
        {
            LinuxSocket s = socket.Accept();

            return new BluetoothClient(s);
        }
    }
}
