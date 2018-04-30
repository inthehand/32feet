//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.Portable.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;
using System.Threading;
using System.Diagnostics;

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceInformation
    {
        private static async Task FindAllAsyncImpl(string aqsFilter, List<DeviceInformation> list)
        {
        }

        private string GetId()
        {
            return string.Empty;
        }

        private string GetName()
        {
            return string.Empty;
        }
    }
}