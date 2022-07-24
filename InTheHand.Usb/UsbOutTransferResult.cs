// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.UsbOutTransferResult
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Usb
{
    public class UsbOutTransferResult
    {
        public int BytesWritten { get; internal set; }

        public TransferStatus Status { get; internal set; }
    }
}