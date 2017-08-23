//-----------------------------------------------------------------------
// <copyright file="DevicePairingResult.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Contains information about the result of attempting to pair a device.
    /// </summary>
    public sealed class DevicePairingResult
    {
#if __ANDROID__
        private DevicePairingResultStatus _status;

        internal DevicePairingResult(bool success)
        {
            _status = success ? DevicePairingResultStatus.Paired : DevicePairingResultStatus.Failed;
        }

#elif WINDOWS_UWP
        private Windows.Devices.Enumeration.DevicePairingResult _result;

        private DevicePairingResult(Windows.Devices.Enumeration.DevicePairingResult result)
        {
            _result = result;
        }

        public static implicit operator Windows.Devices.Enumeration.DevicePairingResult(DevicePairingResult result)
        {
            return result._result;
        }

        public static implicit operator DevicePairingResult(Windows.Devices.Enumeration.DevicePairingResult result)
        {
            return new DevicePairingResult(result);
        }

#elif WIN32
        private DevicePairingResultStatus _status;

        internal DevicePairingResult(int error)
        {
            switch(error)
            {
                case 0:
                    _status = DevicePairingResultStatus.Paired;
                    break;

                case 258: /* WAIT_TIMEOUT */
                    _status = DevicePairingResultStatus.AuthenticationTimeout;
                    break;

                case 259: /*ERROR_NO_MORE_ITEMS*/
                    _status = DevicePairingResultStatus.AlreadyPaired;
                    break;

                case 1223: /*ERROR_CANCELLED*/
                    _status = DevicePairingResultStatus.PairingCanceled;
                    break;

                case 1244: /*ERROR_NOT_AUTHENTICATED*/
                    _status = DevicePairingResultStatus.AuthenticationFailure;
                    break;

                default:
                    _status = DevicePairingResultStatus.Failed;
                    break;
            }
        }
#endif

        /// <summary>
        /// Gets the paired status of the device after the pairing action completed.
        /// </summary>
        /// <value>The paired status of the device.</value>
        public DevicePairingResultStatus Status
        {
            get
            {
#if WINDOWS_UWP
                return (DevicePairingResultStatus)((int)_result.Status);
#elif WIN32
                return _status;
#else
                return DevicePairingResultStatus.Failed;
#endif
            }
        }


    }

    public enum DevicePairingResultStatus
    {
        /// <summary>
        /// The device object is now paired.
        /// </summary>
        Paired = 0,

        /// <summary>
        /// The device object is not in a state where it can be paired.
        /// </summary>
        NotReadyToPair = 1,

        /// <summary>
        /// The device object is not currently paired.
        /// </summary>
        NotPaired = 2,

        /// <summary>
        /// The device object has already been paired.
        /// </summary>
        AlreadyPaired = 3,

        /// <summary>
        /// The device object rejected the connection.
        /// </summary>
        ConnectionRejected = 4,

        /// <summary>
        /// The device object indicated it cannot accept any more incoming connections.
        /// </summary>
        TooManyConnections = 5,

        /// <summary>
        /// The device object indicated there was a hardware failure.
        /// </summary>
        HardwareFailure = 6,

        /// <summary>
        /// The authentication process timed out before it could complete.
        /// </summary>
        AuthenticationTimeout = 7,

        /// <summary>
        /// The authentication protocol is not supported, so the device is not paired.
        /// </summary>
        AuthenticationNotAllowed = 8,

        /// <summary>
        /// Authentication failed, so the device is not paired.
        /// Either the device object or the application rejected the authentication.
        /// </summary>
        AuthenticationFailure = 9,

        /// <summary>
        /// There are no network profiles for this device object to use.
        /// </summary>
        NoSupportedProfiles = 10,

        /// <summary>
        /// The minimum level of protection is not supported by the device object or the application.
        /// </summary>
        ProtectionLevelCouldNotBeMet = 11,

        /// <summary>
        /// Your application does not have the appropriate permissions level to pair the device object.
        /// </summary>
        AccessDenied = 12,

        /// <summary>
        /// The ceremony data was incorrect.
        /// </summary>
        InvalidCeremonyData = 13,

        /// <summary>
        /// The pairing action was canceled before completion.
        /// </summary>
        PairingCanceled = 14,

        /// <summary>
        /// The device object is already attempting to pair or unpair.
        /// </summary>
        OperationAlreadyInProgress = 15,

        /// <summary>
        /// Either the event handler wasn't registered or a required DevicePairingKinds was not supported.
        /// </summary>
        RequiredHandlerNotRegistered = 16,

        /// <summary>
        /// The application handler rejected the pairing.
        /// </summary>
        RejectedByHandler = 17,

        /// <summary>
        /// The remove device already has an association.
        /// </summary>
        RemoteDeviceHasAssociation = 18,

        /// <summary>
        /// An unknown failure occurred.
        /// </summary>
        Failed = 19,
    }
}