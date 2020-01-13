//-----------------------------------------------------------------------
// <copyright file="BluetoothLEScanFilterInit.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    public sealed class BluetoothLEScanFilterInit
    {
        public IList<Guid> Services { get; } = new List<Guid>();

        public string Name { get; set; }

        public string NamePrefix { get; set; }
    }
}