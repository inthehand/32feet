//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            return new GattReadResult(GattCommunicationStatus.Unreachable, null);
        }

        private Guid GetUuid()
        {
            return Guid.Empty;
        }
    }
}