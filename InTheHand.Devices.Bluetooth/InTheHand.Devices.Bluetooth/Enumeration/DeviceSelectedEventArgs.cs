//-----------------------------------------------------------------------
// <copyright file="DeviceSelectedEventArgs.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Provides data for the <see cref="DeviceSelected"/> event on the <see cref="DevicePicker"/> object.
    /// </summary>
    public sealed class DeviceSelectedEventArgs
    {
        /// <summary>
        /// The device selected by the user in the picker.
        /// </summary>
        /// <value>The selected device.</value>
        public DeviceInformation SelectedDevice
        {
            get;
            internal set;
        }
    }
}