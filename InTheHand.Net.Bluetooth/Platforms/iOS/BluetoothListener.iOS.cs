// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (iOS)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        void PlatformStart()
        {
            throw new PlatformNotSupportedException();
        }

        void PlatformStop()
        {
            throw new PlatformNotSupportedException();
        }

        bool PlatformPending()
        {
            return false;
        }

        BluetoothClient PlatformAcceptBluetoothClient()
        {
            throw new PlatformNotSupportedException();
        }
    }
}
