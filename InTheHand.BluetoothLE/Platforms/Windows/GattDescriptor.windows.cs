//-----------------------------------------------------------------------
// <copyright file="GattDescriptor.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Uap = Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Runtime.InteropServices.WindowsRuntime;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        private readonly Uap.GattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, Uap.GattDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptor"></param>
        public static implicit operator Uap.GattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        private Guid GetUuid()
        {
            return _descriptor.Uuid;
        }

        private byte[] PlatformGetValue()
        {
            var result = _descriptor.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Cached).GetResults();

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.Length == 0 ? [] : result.Value.ToArray();
            }

            return null;
        }

        private async Task<byte[]> PlatformReadValue()
        {
            var result = await _descriptor.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().ConfigureAwait(false);

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.Length == 0 ? [] : result.Value.ToArray();
            }

            return null;
        }

        private async Task PlatformWriteValue(byte[] value)
        {
            await _descriptor.WriteValueAsync(value.AsBuffer());
        }
    }
}