// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (Multiplatform)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        void PlatformStart()
        {
            throw Exceptions.GetNotImplementedException();
        }

        void PlatformStop()
        {

        }

        bool PlatformPending()
        {
            return false;
        }

        BluetoothClient PlatformAcceptBluetoothClient()
        {
            return null;
        }
    }
}
