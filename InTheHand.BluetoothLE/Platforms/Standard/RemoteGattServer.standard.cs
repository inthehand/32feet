//-----------------------------------------------------------------------
// <copyright file="RemoteGattServer.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class RemoteGattServer
    {
        private void PlatformInit()
        {
        }

        bool GetConnected()
        {
            return false;
        }

        async Task PlatformConnect()
        {
        }

        void PlatformDisconnect()
        {
        }

        void PlatformCleanup()
        {
        }

        Task<GattService> PlatformGetPrimaryService(BluetoothUuid service)
        {
            return Task.FromResult((GattService)null);
        }

        Task<List<GattService>> PlatformGetPrimaryServices(BluetoothUuid? service)
        {
            return Task.FromResult(new List<GattService>());
        }

        Task<short> PlatformReadRssi()
        {
            return Task.FromResult((short)0);
        }

        void PlatformSetPreferredPhy(BluetoothPhy phy)
        {
        }
    }
}
