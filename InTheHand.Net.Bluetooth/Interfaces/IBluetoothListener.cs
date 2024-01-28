// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.IBluetoothListener
// 
// Copyright (c) 2023-24 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using System;

namespace InTheHand.Net.Sockets
{
    internal interface IBluetoothListener
    {
        void Start();

        void Stop();

        bool Pending();
        
        bool Active { get; }

        Guid ServiceUuid { get; set; }
        ServiceClass ServiceClass { get; set; }
        string ServiceName { get; set; }
        ServiceRecord ServiceRecord { get; set; }

        BluetoothClient AcceptBluetoothClient();
    }
}