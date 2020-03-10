// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (iOS)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        void DoStart()
        {
            throw new PlatformNotSupportedException();
        }

        void DoStop()
        {
            throw new PlatformNotSupportedException();
        }

        bool DoPending()
        {
            return false;
        }

        BluetoothClient DoAcceptBluetoothClient()
        {
            throw new PlatformNotSupportedException();
        }
    }
}
