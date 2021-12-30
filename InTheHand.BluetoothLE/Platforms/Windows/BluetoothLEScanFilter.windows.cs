//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanFilter.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if DEBUG
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Windows.Foundation;

namespace InTheHand.Bluetooth
{
    partial class BluetoothLEScanFilter
    {
        private BluetoothLEAdvertisementFilter _platformFilter;

        public BluetoothLEScanFilter()
        {
            _platformFilter = new BluetoothLEAdvertisementFilter();
        }

        internal BluetoothLEScanFilter(BluetoothLEAdvertisementFilter filter)
        {
            _platformFilter = filter;
        }

        public static implicit operator BluetoothLEAdvertisementFilter(BluetoothLEScanFilter filter)
        {
            return filter._platformFilter;
        }

        public static implicit operator BluetoothLEScanFilter(BluetoothLEAdvertisementFilter filter)
        {
            return new BluetoothLEScanFilter(filter);
        }

        private string PlatformName
        {
            get
            {
                return _platformFilter.Advertisement.LocalName;
            }
            set
            {
                _platformFilter.Advertisement.LocalName = value;
            }
        }
    }
}
#endif