//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.uwp.cs" company="In The Hand Ltd">
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
        private Windows.Devices.Enumeration.DeviceInformation _deviceInformation;

        private DeviceInformation(Windows.Devices.Enumeration.DeviceInformation deviceInformation)
        {
            _deviceInformation = deviceInformation;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceInformation(DeviceInformation deviceInformation)
        {
            return deviceInformation == null ? null : deviceInformation._deviceInformation;
        }

        public static implicit operator DeviceInformation(Windows.Devices.Enumeration.DeviceInformation deviceInformation)
        {
            return deviceInformation == null ? null : new DeviceInformation(deviceInformation);
        }

        private static async Task FindAllAsyncImpl(string aqsFilter, List<DeviceInformation> list)
        {
            var devs = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqsFilter);

            foreach (Windows.Devices.Enumeration.DeviceInformation di in devs)
            {
                list.Add(new DeviceInformation(di));
            }
        }

        private string GetId()
        {
            return _deviceInformation.Id;
        }

        private string GetName()
        {
            return _deviceInformation.Name;
        }

#if WINDOWS_UWP
        private DeviceInformationPairing GetPairing()
        {
            return _deviceInformation.Pairing;
        }
#endif
    }
}