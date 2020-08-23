//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanFilter.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    public sealed partial class BluetoothLEScanFilter
    {
        public string Name { get; set; }

        public string NamePrefix { get; set; }
        
        public IList<BluetoothUuid> Services { get; } = new List<BluetoothUuid>();

    }
}