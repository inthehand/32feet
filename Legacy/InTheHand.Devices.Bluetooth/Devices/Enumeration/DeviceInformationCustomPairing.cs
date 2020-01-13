//-----------------------------------------------------------------------
// <copyright file="DeviceInformationCustomPairing.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System.Threading.Tasks;
using System;
#if WINDOWS_UWP
using System.Runtime.InteropServices.WindowsRuntime;
#elif WIN32
using System.Runtime.InteropServices;
using InTheHand.Devices.Bluetooth;
#endif

namespace InTheHand.Devices.Enumeration
{
    /// <summary>
    /// Represents a custom pairing for a DeviceInformation object.
    /// </summary>
    public sealed class DeviceInformationCustomPairing
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Enumeration.DeviceInformationCustomPairing _pairing;

        private DeviceInformationCustomPairing(Windows.Devices.Enumeration.DeviceInformationCustomPairing pairing)
        {
            _pairing = pairing;
        }

        public static implicit operator Windows.Devices.Enumeration.DeviceInformationCustomPairing(DeviceInformationCustomPairing pairing)
        {
            return pairing._pairing;
        }

        public static implicit operator DeviceInformationCustomPairing(Windows.Devices.Enumeration.DeviceInformationCustomPairing pairing)
        {
            return new DeviceInformationCustomPairing(pairing);
        }

        private void _pairing_PairingRequested(Windows.Devices.Enumeration.DeviceInformationCustomPairing sender, Windows.Devices.Enumeration.DevicePairingRequestedEventArgs args)
        {
            _pairingRequested?.Invoke(this, args);
        }

#elif WIN32
        private BLUETOOTH_DEVICE_INFO _deviceInfo;
        private IntPtr _callbackHandle;
        private PFN_AUTHENTICATION_CALLBACK_EX _callback;

        internal DeviceInformationCustomPairing(BLUETOOTH_DEVICE_INFO info)
        {
            _deviceInfo = info;
            _callback = new PFN_AUTHENTICATION_CALLBACK_EX(AUTHENTICATION_CALLBACK);
        }

        private bool AUTHENTICATION_CALLBACK(IntPtr pvParam, ref NativeMethods.BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams)
        {
            DevicePairingRequestedEventArgs args = new Enumeration.DevicePairingRequestedEventArgs(new Enumeration.DeviceInformation(pAuthCallbackParams.deviceInfo), NativeMethods.BluetoothAuthenticationMethodToDevicePairingKinds(pAuthCallbackParams.authenticationMethod), pAuthCallbackParams.Numeric_Value_Passkey.ToString());
            _pairingRequested?.Invoke(this, args);
            return false;
        }
#endif


        /// <summary>
        /// Attempts to pair the device.
        /// </summary>
        /// <param name="pairingKindsSupported">The different pairing kinds supported by this DeviceInformation object.</param>
        /// <returns>The result of the pairing action.</returns>
        public async Task<DevicePairingResult> PairAsync(DevicePairingKinds pairingKindsSupported)
        {
#if WINDOWS_UWP
            return await _pairing.PairAsync((Windows.Devices.Enumeration.DevicePairingKinds)((int)pairingKindsSupported));
#elif WIN32
            int result = NativeMethods.BluetoothAuthenticateDeviceEx(IntPtr.Zero, IntPtr.Zero, ref _deviceInfo, IntPtr.Zero, 0);
            return new DevicePairingResult(result);
#else
            return null;
#endif
        }


        private event EventHandler<DevicePairingRequestedEventArgs> _pairingRequested;
        /// <summary>
        /// Raised when a pairing action is requested.
        /// </summary>
        public event EventHandler<DevicePairingRequestedEventArgs> PairingRequested
        {
            add
            {
                if (_pairingRequested == null)
                {
                    // add callback
#if WINDOWS_UWP
                    _pairing.PairingRequested += _pairing_PairingRequested;
#elif WIN32
                    int hresult = NativeMethods.BluetoothRegisterForAuthenticationEx(ref _deviceInfo, out _callbackHandle, _callback, IntPtr.Zero);
#endif
                }

                _pairingRequested += value;
            }
            remove
            {
                _pairingRequested -= value;

                if (_pairingRequested == null)
                {
                    // remove callback
#if WINDOWS_UWP
                    _pairing.PairingRequested -= _pairing_PairingRequested;
#elif WIN32
                    if (_callbackHandle != IntPtr.Zero)
                    {
                        if (NativeMethods.BluetoothUnregisterAuthentication(_callbackHandle))
                        {
                            _callbackHandle = IntPtr.Zero;
                        }
                    }
#endif

                }
            }
        }
    }
}