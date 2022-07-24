// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Usb.ControlTransferSetup
// 
// Copyright (c) 2020-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace InTheHand.Usb
{
    /// <summary>
    /// An object that sets options for a control transfer.
    /// </summary>
    public sealed class ControlTransferSetup
    {
        /// <summary>
        /// Must be one of three values specifying whether the tranfer is "standard" (common to all USB devices) "class" (common to an industry-standard class of devices) or "vendor".
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// Specifices the target of the transfer on the device
        /// </summary>
        public Recipient Recipient { get; set; }

        /// <summary>
        /// A vendor-specific command.
        /// </summary>
        public int Request { get; set; }

        /// <summary>
        /// Vender-specific request parameters.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The interface number of the recipient.
        /// </summary>
        public int Index { get; set; }
    }
}
