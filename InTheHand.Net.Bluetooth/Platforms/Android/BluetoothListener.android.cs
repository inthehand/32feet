// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Android)
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        private BluetoothServerSocket socket;

        void DoStart()
        {
            socket = BluetoothRadio.Default.Adapter.ListenUsingRfcommWithServiceRecord(this.serviceUuid.ToString(), Java.Util.UUID.FromString(this.serviceUuid.ToString()));
            if (socket != null)
                Active = true;
        }

        void DoStop()
        {
            if (Active)
            {
                socket?.Close();
                socket = null;
                Active = false;
            }
        }

        bool DoPending()
        {
            return false;
        }

        BluetoothClient DoAcceptBluetoothClient()
        {
            var newSocket = socket.Accept();
            if (newSocket != null)
            {
                return new BluetoothClient(newSocket);
            }

            return null;
        }
    }
}
