// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.Win32BluetoothAuthentication
// 
// Copyright (c) 2003-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal sealed class Win32BluetoothAuthentication
    {
        readonly string _pin;
        IntPtr _handle = IntPtr.Zero;
        readonly NativeMethods.BluetoothAuthenticationCallbackEx _callback;
        readonly EventWaitHandle _waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        public ulong Address { get; set; }

        public Win32BluetoothAuthentication(BluetoothAddress address, string pin)
        {
            Address = address;
            _pin = pin;
            BLUETOOTH_DEVICE_INFO device = BLUETOOTH_DEVICE_INFO.Create();
            device.Address = address;
            NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref device);
            _callback = new NativeMethods.BluetoothAuthenticationCallbackEx(Callback);

            int result = NativeMethods.BluetoothRegisterForAuthenticationEx(ref device, out _handle, _callback, IntPtr.Zero);

            if (result != 0)
                _waitHandle.Set();
        }

        private bool Callback(IntPtr pvParam, ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams)
        {
            try
            {
                switch (pAuthCallbackParams.authenticationMethod)
                {
                    case BluetoothAuthenticationMethod.Passkey:
                    case BluetoothAuthenticationMethod.NumericComparison:
                        BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO nresponse = new BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO
                        {
                            authMethod = pAuthCallbackParams.authenticationMethod,
                            bthAddressRemote = pAuthCallbackParams.deviceInfo.Address,
                            numericComp_passkey = pAuthCallbackParams.Numeric_Value_Passkey
                        };

                        int result = NativeMethods.BluetoothSendAuthenticationResponseEx(IntPtr.Zero, ref nresponse);
                        System.Diagnostics.Debug.WriteLine($"{result} {nresponse.negativeResponse}");
                        return result == 0;

                    case BluetoothAuthenticationMethod.Legacy:
                        BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO response = new BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO
                        {
                            authMethod = pAuthCallbackParams.authenticationMethod,
                            bthAddressRemote = pAuthCallbackParams.deviceInfo.Address
                        };
                        response.pinInfo.pin = new byte[16];
                        System.Text.Encoding.ASCII.GetBytes(_pin).CopyTo(response.pinInfo.pin, 0);
                        response.pinInfo.pinLength = _pin.Length;

                        int authResult = NativeMethods.BluetoothSendAuthenticationResponseEx(IntPtr.Zero, ref response);
                        System.Diagnostics.Debug.WriteLine($"BluetoothSendAuthenticationResponseEx: {authResult}");
                        return authResult == 0;
                }

                return false;
            }
            finally
            {
                _waitHandle.Set();
                // after one call remove the registration
                Win32BluetoothSecurity.RemoveRedundantAuthHandler(Address);
            }
        }

        public void WaitOne()
        {
            _waitHandle.WaitOne();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if(_handle != IntPtr.Zero)
                    NativeMethods.BluetoothUnregisterAuthentication(_handle);
                _handle = IntPtr.Zero;
            }
        }
        
        ~Win32BluetoothAuthentication()
        {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential, Size = 52)]
    internal struct BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO // see above
    {
        internal ulong bthAddressRemote;
        internal BluetoothAuthenticationMethod authMethod;
        internal BLUETOOTH_PIN_INFO pinInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        private readonly byte[] _padding;
        internal byte negativeResponse;
    }

    [StructLayout(LayoutKind.Sequential, Size = 52)]
    internal struct BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO // see above
    {
        internal ulong bthAddressRemote;
        internal BluetoothAuthenticationMethod authMethod;
        internal BLUETOOTH_OOB_DATA_INFO oobInfo;
        internal byte negativeResponse;
    }
    
    [StructLayout(LayoutKind.Sequential, Size = 52)]
    internal struct BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO // see above
    {
        internal ulong bthAddressRemote;
        internal BluetoothAuthenticationMethod authMethod;
        internal uint numericComp_passkey;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        private readonly byte[] _padding;

        internal byte negativeResponse;
    }

    /// <summary>
    /// The BLUETOOTH_PIN_INFO structure contains information used for authentication via PIN.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 20)]
    internal struct BLUETOOTH_PIN_INFO
    {
        public const int BTH_MAX_PIN_SIZE = 16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = BTH_MAX_PIN_SIZE)]
        internal byte[] pin;
        internal int pinLength;
    }

    /// <summary>
    /// The BLUETOOTH_OOB_DATA_INFO structure contains data used to authenticate prior to establishing an Out-of-Band device pairing.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    internal struct BLUETOOTH_OOB_DATA_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        internal byte[] C;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        internal byte[] R;
    }
}
