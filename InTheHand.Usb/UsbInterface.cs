// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.UsbInterface
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Usb
{
    public partial class UsbInterface
    {
        public int InterfaceNumber { get; private set; }

        public bool IsClaimed { get; private set; }
    }
}
