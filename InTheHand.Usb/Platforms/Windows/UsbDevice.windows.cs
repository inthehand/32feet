// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.UsbDevice (Windows)
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace InTheHand.Usb
{
    public partial class UsbDevice
    {
        internal static async Task<IReadOnlyCollection<UsbDevice>> PlatformGetDevices()
        {
            List<UsbDevice> devices = new List<UsbDevice>();

            return devices.AsReadOnly();
        }

        private Task PlatformOpen()
        {
            return Task.CompletedTask;
        }

        private Task PlatformClose()
        {
            return Task.CompletedTask;
        }

        private Task PlatformClaimInterface(int interfaceNumber)
        {
            return Task.CompletedTask;
        }

        private Task PlatformReleaseInterface(int interfaceNumber)
        {
            return Task.CompletedTask;
        }

        private Task<UsbInTransferResult> PlatformControlTransferIn(ControlTransferSetup setup, int length)
        {
            return (Task<UsbInTransferResult>)Task.CompletedTask;
        }

        private Task PlatformControlTransferOut(ControlTransferSetup setup, byte[] data)
        {
            return Task.CompletedTask;
        }

        private Task<UsbInTransferResult> PlatformTransferIn(int endpointNumber, int length)
        {
            return (Task<UsbInTransferResult>)Task.CompletedTask;
        }

        private Task PlatformTransferOut(int endpointNumber, byte[] data)
        {
            return Task.CompletedTask;
        }
    }
}