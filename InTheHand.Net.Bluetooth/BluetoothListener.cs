// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    public sealed partial class BluetoothListener
    {
        Guid serviceUuid;

        public BluetoothListener(Guid service)
        {
            serviceUuid = service;
        }

        public bool Active
        {
            get;
            private set;
        }

        public ServiceClass ServiceClass
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public bool Pending()
        {
            return DoPending();
        }

        public void Start()
        {
            DoStart();
            Active = true;
        }

        public void Stop()
        {
            DoStop();
            Active = false;
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            return DoAcceptBluetoothClient();
        }
    }
}
