// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.BluetoothAddress (iOS)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalAccessory;

namespace InTheHand.Net
{
    partial class BluetoothAddress
    {
        private EAAccessory _accessory;

        internal BluetoothAddress(EAAccessory accessory)
        {
            _accessory = accessory;
            _address = (ulong)accessory.GetHashCode();
        }

        internal EAAccessory Accessory
        {
            get
            {
                return _accessory;
            }
        }
    }
}
