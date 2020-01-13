using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTServer
    {
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

        Task<BluetoothRemoteGATTService> DoGetPrimaryService(Guid? service)
        {
            return Task.FromResult((BluetoothRemoteGATTService)null);
        }

        Task<List<BluetoothRemoteGATTService>> DoGetPrimaryServices(Guid? service)
        {
            return Task.FromResult(new List<BluetoothRemoteGATTService>());
        }
    }
}
