// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Android)
// 
// Copyright (c) 2018-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    internal sealed class AndroidBluetoothListener : IBluetoothListener
    {
        private BluetoothServerSocket socket;
        public bool Active { get; private set; }

        public InTheHand.Net.Bluetooth.ServiceClass ServiceClass { get; set; }

        public string ServiceName { get; set; }

        public InTheHand.Net.Bluetooth.Sdp.ServiceRecord ServiceRecord { get; set; }
        public Guid ServiceUuid { get; set; }

        public void Start()
        {
            string serviceName = string.IsNullOrEmpty(ServiceName) ? ServiceUuid.ToString() : ServiceName;
            
            socket = ((AndroidBluetoothRadio)BluetoothRadio.Default.Radio).Adapter.ListenUsingRfcommWithServiceRecord(serviceName, Java.Util.UUID.FromString(ServiceUuid.ToString()));
            if (socket != null)
                Active = true;
        }

        public void Stop()
        {
            socket?.Close();
            socket = null;
            Active = false;
        }

        bool IBluetoothListener.Pending()
        {
            return false;
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            var newSocket = socket.Accept();
            if (newSocket != null)
            {
                return new BluetoothClient(new AndroidBluetoothClient(newSocket));
            }

            return null;
        }
    }
}