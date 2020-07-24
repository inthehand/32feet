//-----------------------------------------------------------------------
// <copyright file="BluetoothRemoteGATTServer.cs" company="In The Hand Ltd">
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
    public sealed partial class BluetoothRemoteGATTServer
    {
        internal BluetoothRemoteGATTServer(BluetoothDevice device)
        {
            Device = device;
        }

        public BluetoothDevice Device { get; private set; }

        public bool Connected { get { return GetConnected(); } }

        public Task Connect()
        {
            return DoConnect();
        }

        public void Disconnect()
        {
            DoDisconnect();
            Device.OnGattServerDisconnected();
        }

        public Task<GattService> GetPrimaryService(BluetoothUuid service)
        {
            return DoGetPrimaryService(service);
        }

        public Task<List<GattService>> GetPrimaryServices(BluetoothUuid? service = null)
        {
            return DoGetPrimaryServices(service);
        }
    }
}
