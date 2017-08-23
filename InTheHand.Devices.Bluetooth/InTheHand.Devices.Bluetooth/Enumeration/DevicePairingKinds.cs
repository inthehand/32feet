//-----------------------------------------------------------------------
// <copyright file="DevicePairingKinds.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Indicates the kinds of pairing supported by your application or requested by the system.
    /// As an input value, use this value to indicate what kinds of pairing your application supports.
    /// When this datatype is received as an output value, it indicates the kind of pairing requested by the system.
    /// In this case, your code will need to respond accordingly.
    /// </summary>
    [Flags]
    public enum DevicePairingKinds
    {
        /// <summary>
        /// No pairing is supported.
        /// </summary>
        None = 0,

        /// <summary>
        /// The application must confirm they wish to perform the pairing action. 
        /// You can present an optional confirmation dialog to the user. 
        /// With a value of ConfirmOnly, Accept the pairing if you want it to complete.
        /// </summary>
        ConfirmOnly = 1,

        /// <summary>
        /// The application must display the given PIN to the user. 
        /// The user will then need to enter or confirm that PIN on the device that is being paired. 
        /// With a value of DisplayPin, Accept the pairing if you want it to complete. 
        /// If your application cancels the pairing at this point, the device might still be paired. 
        /// This is because the system and the target device don't need any confirmation for this DevicePairingKinds value.
        /// </summary>
        DisplayPin = 2,

        /// <summary>
        /// The application must request a PIN from the user. 
        /// The PIN will typically be displayed on the target device. 
        /// With a value of ProvidePin, Accept the pairing and pass in the PIN as a parameter.
        /// </summary>
        ProvidePin = 4,

        /// <summary>
        /// The application must display the given PIN to the user and ask the user to confirm that the PIN matches the one show on the target device. 
        /// With a value of ConfirmPinMatch, Accept the pairing if you want it to complete.
        /// </summary>
        ConfirmPinMatch = 8,
    }
}