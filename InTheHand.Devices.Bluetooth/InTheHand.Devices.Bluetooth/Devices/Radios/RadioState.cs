//-----------------------------------------------------------------------
// <copyright file="RadioState.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Radios
{
    /// <summary>
    /// Enumeration that describes possible radio states.
    /// </summary>
    public enum RadioState
    {
        /// <summary>
        /// The radio state is unknown, or the radio is in a bad or uncontrollable state.
        /// </summary>
        Unknown,

        /// <summary>
        /// The radio is powered on.
        /// </summary>
        On,

        /// <summary>
        /// The radio is powered off.
        /// </summary>
        Off,

        /// <summary>
        /// The radio is powered off and disabled by the device firmware or a hardware switch on the device. 
        /// </summary>
        Disabled,
    }
}