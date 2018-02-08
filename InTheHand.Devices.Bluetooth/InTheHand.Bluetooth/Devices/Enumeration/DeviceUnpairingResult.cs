//-----------------------------------------------------------------------
// <copyright file="DeviceUnpairingResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Contains information about the result of attempting to unpair a device.
    /// </summary>
    public sealed class DeviceUnpairingResult
    {
#if WINDOWS_UWP
        private Windows.Devices.Enumeration.DeviceUnpairingResult _result;

        private DeviceUnpairingResult(Windows.Devices.Enumeration.DeviceUnpairingResult result)
        {
            _result = result;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceUnpairingResult(DeviceUnpairingResult result)
        {
            return result._result;
        }

        public static implicit operator DeviceUnpairingResult(Windows.Devices.Enumeration.DeviceUnpairingResult result)
        {
            return new DeviceUnpairingResult(result);
        }
#else
        private DeviceUnpairingResultStatus _status;

        internal DeviceUnpairingResult()
        {
            _status = DeviceUnpairingResultStatus.Failed;
        }

#if WIN32

        internal DeviceUnpairingResult(int error)
        {
            switch(error)
            {
                case 0:
                    _status = DeviceUnpairingResultStatus.Unpaired;
                    break;

                default:
                    _status = DeviceUnpairingResultStatus.Failed;
                    break;
            }
        }
#endif
#endif

        /// <summary>
        /// Gets the paired status of the device after the unpairing action completed.
        /// </summary>
        /// <value>The paired status of the device.</value>
        public DeviceUnpairingResultStatus Status
        {
            get
            {
#if WINDOWS_UWP
                return (DeviceUnpairingResultStatus)((int)_result.Status);

#else
                return _status;
#endif
            }
        }

    }
}