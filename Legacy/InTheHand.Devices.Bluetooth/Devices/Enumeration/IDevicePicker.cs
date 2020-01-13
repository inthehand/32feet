//-----------------------------------------------------------------------
// <copyright file="IDevicePicker.cs" company="In The Hand Ltd">
//   Copyright (c) 2018 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
#if !UNITY
using System.Threading.Tasks;
#endif

namespace InTheHand.Devices.Enumeration
{
    internal interface IDevicePicker
    {
        //DevicePickerAppearance Appearance { get; }

        DevicePickerFilter Filter { get; }

#if WIN32
        DeviceInformation PickSingleDevice();
#endif
#if !UNITY
        Task<DeviceInformation> PickSingleDeviceAsync();
#endif
    }
}