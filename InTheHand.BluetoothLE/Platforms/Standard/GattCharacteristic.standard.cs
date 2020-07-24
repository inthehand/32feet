//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.standard.cs" company="In The Hand Ltd">
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
    partial class GattCharacteristic
    {
        BluetoothUuid GetUuid()
        {
            return default;
        }

        GattCharacteristicProperties GetProperties()
        {
            return 0;
        }

        string GetUserDescription()
        {
            return string.Empty;
        }

        Task<GattDescriptor> DoGetDescriptor(BluetoothUuid descriptor)
        {
            return Task.FromResult((GattDescriptor)null);
        }

        Task<IReadOnlyList<GattDescriptor>> DoGetDescriptors()
        {
            return Task.FromResult((IReadOnlyList<GattDescriptor>)null);
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task<byte[]> DoReadValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task DoWriteValue(byte[] value)
        {
            return Task.CompletedTask;
        }

        void AddCharacteristicValueChanged()
        {
        }

        void RemoveCharacteristicValueChanged()
        {
        }

        private Task DoStartNotifications()
        {
            return Task.CompletedTask;
        }

        private Task DoStopNotifications()
        {
            return Task.CompletedTask;
        }
    }
}
