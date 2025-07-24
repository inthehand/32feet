//-----------------------------------------------------------------------
// <copyright file="BluetoothDevice.standard.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class BluetoothDevice
    {
        private bool _watchingAdvertisements = false;

        private static async Task<BluetoothDevice?> PlatformFromId(string id)
        {
            return null;
        }

        private string GetId()
        {
            return string.Empty;
        }

        private string GetName()
        {
            return string.Empty;
        }

        private RemoteGattServer GetGatt()
        {
            return new RemoteGattServer(this);
        }

        private bool GetIsPaired()
        {
            return false;
        }

        private Task PlatformPairAsync()
        {
            throw new PlatformNotSupportedException();
        }

        private Task PlatformPairAsync(string pairingCode)
        {
            throw new PlatformNotSupportedException();
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

        public void Dispose() {}
    }
}
