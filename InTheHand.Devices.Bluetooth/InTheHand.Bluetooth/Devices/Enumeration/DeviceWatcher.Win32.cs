//-----------------------------------------------------------------------
// <copyright file="DeviceWatcher.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceWatcher
    {
        private DeviceWatcherStatus GetStatus()
        {
            return DeviceWatcherStatus.Created;
        }
    }
}