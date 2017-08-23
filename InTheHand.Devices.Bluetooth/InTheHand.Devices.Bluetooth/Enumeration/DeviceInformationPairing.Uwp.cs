//-----------------------------------------------------------------------
// <copyright file="DeviceInformationPairing.Uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceInformationPairing
    {
        private Windows.Devices.Enumeration.DeviceInformationPairing _pairing;

        private DeviceInformationPairing(Windows.Devices.Enumeration.DeviceInformationPairing pairing)
        {
            _pairing = pairing;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceInformationPairing(DeviceInformationPairing pairing)
        {
            return pairing._pairing;
        }

        public static implicit operator DeviceInformationPairing(Windows.Devices.Enumeration.DeviceInformationPairing pairing)
        {
            return new DeviceInformationPairing(pairing);
        }

        private bool GetCanPair()
        {
            return _pairing.CanPair;
        }

        /// <summary>
        /// Gets the <see cref="DeviceInformationCustomPairing"/> object necessary for custom pairing.
        /// </summary>
        /// <remarks>
        /// <para/><list type="table">
        /// <listheader><term>Platform</term><description>Version supported</description></listheader>
        /// <item><term>Windows UWP</term><description>Windows 10</description></item>
        /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
        /// </remarks>
        private DeviceInformationCustomPairing GetCustom()
        {
            return _pairing.Custom;
        }

        private bool GetIsPaired()
        {
            return _pairing.IsPaired;
        }

        private async Task<DevicePairingResult> DoPairAsync()
        {
            return await _pairing.PairAsync();
        }

        private async Task<DeviceUnpairingResult> DoUnpairAsync()
        {
            return await _pairing.UnpairAsync();
        }
    }
}