// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.UsbInTransferResult
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Usb
{
    public class UsbInTransferResult
    {
        public byte[] Data { get; internal set; }

        public TransferStatus Status { get; internal set; }
    }
}