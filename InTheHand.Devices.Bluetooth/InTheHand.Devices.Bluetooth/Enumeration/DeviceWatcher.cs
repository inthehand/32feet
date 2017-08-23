//-----------------------------------------------------------------------
// <copyright file="DeviceWatcher.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Enumerates devices dynamically, so that the app receives notifications if devices are added, removed, or changed after the initial enumeration is complete.
    /// </summary>
    public sealed partial class DeviceWatcher
    {
        /// <summary>
        /// The status of the <see cref="DeviceWatcher"/>.
        /// </summary>
        public DeviceWatcherStatus Status
        {
            get
            {
                return GetStatus();
            }
        }

        /// <summary>
        /// Starts a search for devices, and subscribes to device enumeration events.
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Stop raising the events that add, update and remove enumeration results.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Event that is raised when a device is added to the collection enumerated by the Windows.Devices.Enumeration.DeviceWatcher.
        /// </summary>
        public event EventHandler<DeviceInformation> Added;

        private void RaiseAdded(DeviceInformation deviceInformation)
        {
            Added?.Invoke(this, deviceInformation);
        }

        /// <summary>
        /// Event that is raised when a device is updated in the collection of enumerated devices.
        /// </summary>
        public event EventHandler<DeviceInformation> Updated;

        private void RaiseUpdated(DeviceInformation deviceInformation)
        {
            Updated?.Invoke(this, deviceInformation);
        }

        /// <summary>
        /// Event that is raised when a device is removed from the collection of enumerated devices.
        /// </summary>
        public event EventHandler<DeviceInformation> Removed;

        private void RaiseRemoved(DeviceInformation deviceInformation)
        {
            Removed?.Invoke(this, deviceInformation);
        }

        /// <summary>
        /// Event that is raised when the enumeration of devices completes.
        /// </summary>
        public event EventHandler<object> EnumerationCompleted;

        private void RaiseEnumerationCompleted()
        {
            EnumerationCompleted?.Invoke(this, null);
        }

        /// <summary>
        /// Event that is raised when the enumeration operation has been stopped.
        /// </summary>
        public event EventHandler<object> Stopped;

        private void RaiseStopped()
        {
            Stopped?.Invoke(this, null);
        }
    }
}