// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothProtocolType
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System.Net.Sockets;

namespace InTheHand.Net.Sockets
{
    /// <summary>
    /// Specifies additional protocols that the <see cref="Socket"/> class supports.
    /// </summary>
    /// <remarks>
    /// <para>These constants are defined by the Bluetooth SIG - <see href="https://www.bluetooth.com/specifications/assigned-numbers/service-discovery/"/>
    /// </para>
    /// </remarks>
    public static class BluetoothProtocolType
    {
        /// <summary>
        /// Service Discovery Protocol (bt-sdp)
        /// </summary>
        public const ProtocolType Sdp = (ProtocolType)0x0001;

        /// <summary>
        /// Bluetooth RFComm protocol (bt-rfcomm)
        /// </summary>
        public const ProtocolType RFComm = (ProtocolType)0x0003;

        /// <summary>
        /// Logical Link Control and Adaptation Protocol (bt-l2cap)
        /// </summary>
        public const ProtocolType L2Cap = (ProtocolType)0x0100;
    }
}
