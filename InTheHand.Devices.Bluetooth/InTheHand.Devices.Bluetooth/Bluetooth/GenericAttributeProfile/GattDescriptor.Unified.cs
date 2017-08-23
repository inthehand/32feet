//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using CoreBluetooth;
using Foundation;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        internal CBDescriptor _descriptor;

        private GattDescriptor(CBDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public static implicit operator CBDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        public static implicit operator GattDescriptor(CBDescriptor descriptor)
        {
            return new GattDescriptor(descriptor);
        }

        private async Task<GattReadResult> DoReadValueAsync(BluetoothCacheMode cacheMode)
        {
            try
            {
                return new GattReadResult(GattCommunicationStatus.Success, ((NSData)_descriptor.Value).ToArray());
            }
            catch
            {
                return new GattReadResult(GattCommunicationStatus.Unreachable, null);
            }
        }
        
        private Guid GetUuid()
        {
            return _descriptor.UUID.ToGuid();
        }
    }
}