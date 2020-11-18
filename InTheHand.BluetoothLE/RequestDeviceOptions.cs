//-----------------------------------------------------------------------
// <copyright file="RequestDeviceOptions.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace InTheHand.Bluetooth
{
    public sealed class RequestDeviceOptions
    {
        public IList<BluetoothLEScanFilter> Filters { get; } = new List<BluetoothLEScanFilter>();

        public IList<Guid> OptionalServices { get; } = new List<Guid>();

        public bool AcceptAllDevices { get; set; }
    }
}
