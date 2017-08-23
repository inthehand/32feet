//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    partial class GattDeviceService
    {
        private static async Task<GattDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return null;
        }

        private static string GetDeviceSelectorFromShortIdImpl(ushort serviceShortId)
        {
            return string.Empty;
        }

        private static string GetDeviceSelectorFromUuidImpl(Guid serviceUuid)
        {
            return string.Empty;
        }


        private void GetAllCharacteristics(List<GattCharacteristic> characteristics) { }

        private void GetCharacteristics(Guid characteristicUuid, List<GattCharacteristic> characteristics) { }

        private BluetoothLEDevice GetDevice()
        {
            return null;
        }

        private Guid GetUuid()
        {
            return Guid.Empty;
        }

        private void DoDispose() { }
    }
}