// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (.NET Standard)
// 
// Copyright (c) 2018-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        void DoStart()
        {
            throw Exceptions.GetNotImplementedException();
        }

        void DoStop()
        {

        }

        bool DoPending()
        {
            return false;
        }

        BluetoothClient DoAcceptBluetoothClient()
        {
            return null;
        }
    }
}
