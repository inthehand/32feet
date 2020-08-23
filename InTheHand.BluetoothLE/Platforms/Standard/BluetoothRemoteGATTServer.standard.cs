//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTServer.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothRemoteGATTServer
    {
        private void PlatformInit()
        {
        }

        bool GetConnected()
        {
            return false;
        }

        async Task DoConnect()
        {
        }

        void DoDisconnect()
        {
        }

        Task<GattService> DoGetPrimaryService(BluetoothUuid service)
        {
            return Task.FromResult((GattService)null);
        }

        Task<List<GattService>> DoGetPrimaryServices(BluetoothUuid? service)
        {
            return Task.FromResult(new List<GattService>());
        }
    }
}
