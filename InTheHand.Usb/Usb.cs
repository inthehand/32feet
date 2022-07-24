// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.Usb
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace InTheHand.Usb
{
    public static class Usb
    {
        public static Task<IReadOnlyCollection<UsbDevice>> GetDevices()
        {
            return UsbDevice.PlatformGetDevices();
        }
    }
}
