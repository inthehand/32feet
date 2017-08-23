//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;
using InTheHand.Devices.Enumeration;
using System.Collections.Generic;
using System.Diagnostics;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(() =>
            {
                if (s_default != null)
                {
                    s_default = new BluetoothAdapter();
                }

                return s_default;
            });
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return BluetoothClassOfDevice.FromRawValue(0);
        }

        private bool GetIsClassicSupported()
        {
            return false;
        }

        private bool GetIsLowEnergySupported()
        {
            return true;
        }

        private string GetName()
        {
            return string.Empty;
        }
    }
}