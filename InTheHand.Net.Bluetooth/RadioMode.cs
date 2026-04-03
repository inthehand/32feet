// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.RadioMode
// 
// Copyright (c) 2003-2026 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Defines the possible states of the local radio device.
    /// </summary>
    public enum RadioMode
    {
        /// <summary>
        /// Radio is powered off.
        /// </summary>
        PowerOff,
        /// <summary>
        /// Radio is powered on and supports incoming and outgoing connections.
        /// </summary>
        Connectable,
        /// <summary>
        /// Radio is discoverable from other devices.
        /// </summary>
        Discoverable,
    }
}
