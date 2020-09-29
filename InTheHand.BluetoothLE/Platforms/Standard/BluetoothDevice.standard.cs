//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private bool _watchingAdvertisements = false;

        private static async Task<BluetoothDevice> PlatformFromId(string id)
        {
            return null;
        }

        string GetId()
        {
            return string.Empty;
        }

        string GetName()
        {
            return string.Empty;
        }

        RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        /*
        bool GetWatchingAdvertisements()
        {
            return _watchingAdvertisements;
        }

        async Task DoWatchAdvertisements()
        {
            _watchingAdvertisements = true;
        }

        void DoUnwatchAdvertisements()
        {
            _watchingAdvertisements = false;
        }*/
    }
}
