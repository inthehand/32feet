// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.UsbDevice
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;

namespace InTheHand.Usb
{
    public partial class UsbDevice
    {
        public int DeviceClass { get; private set; }

        public int DeviceProtocol { get; private set; }

        public int DeviceSubclass { get; private set; }

        public Version DeviceVersion { get; private set; }

        public string ManufacturerName { get; private set; }

        public bool IsOpened { get; private set; }

        public int ProductId { get; private set; }

        public string ProductName {  get; private set; }

        public string SerialNumber { get; private set; }

        public Version UsbVersion { get; private set; }

        public int VendorId { get; private set; }

        public Task Open()
        {
            return PlatformOpen();
        }

        public Task Close()
        {
            return PlatformClose();
        }

        public Task ClaimInterface(int interfaceNumber)
        {
            return PlatformClaimInterface(interfaceNumber);
        }

        public Task ReleaseInterface(int interfaceNumber)
        {
            return PlatformReleaseInterface(interfaceNumber);
        }

        public Task<UsbInTransferResult> ControlTransferIn(ControlTransferSetup setup, int length)
        {
            return PlatformControlTransferIn(setup, length);
        }
        public Task ControlTransferOut(ControlTransferSetup setup, byte[] data)
        {
            return PlatformControlTransferOut(setup, data);
        }

        public Task<UsbInTransferResult> TransferIn(int endpointNumber, int length)
        {
            return PlatformTransferIn(endpointNumber, length);
        }

        public Task TransferOut(int endpointNumber, byte[] data)
        {
            return PlatformTransferOut(endpointNumber, data);
        }
    }
}