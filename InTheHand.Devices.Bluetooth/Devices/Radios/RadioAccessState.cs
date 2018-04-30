//-----------------------------------------------------------------------
// <copyright file="RadioAccessStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Radios
{
    /// <summary>
    /// Enumeration that describes possible access states that a user can have to a given radio.
    /// </summary>
    public enum RadioAccessStatus
    {
        /// <summary>
        /// Access state is unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Access is allowed.
        /// </summary>
        Allowed,

        /// <summary>
        /// Access was denied because of user action, usually through denying an operation through the radio privacy settings page.
        /// </summary>
        DeniedByUser,

        /// <summary>
        /// Access was denied by the system. One common reason for this result is that the user does not have suitable permission to manipulate the radio in question.
        /// </summary>
        DeniedBySystem,
    }
}