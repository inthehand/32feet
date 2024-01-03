// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio
// 
// Copyright (c) 2003-2024 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Sockets;
using System;

namespace InTheHand.Net.Bluetooth
{
    /// <summary>
    /// Represents a local Bluetooth Radio device.
    /// </summary>
    public sealed partial class BluetoothRadio : IDisposable
    {
        private static  BluetoothRadio s_default;

        /// <summary>
        /// Returns the default Bluetooth radio (if present).
        /// </summary>
        public static BluetoothRadio Default
        {
            get
            {
                if(s_default == null)
                {
                    IBluetoothRadio radio = null;
#if ANDROID || MONOANDROID
                    radio = AndroidBluetoothRadio.GetDefault();
#elif IOS || __IOS__
                    radio = new ExternalAccessoryBluetoothRadio();
#elif WINDOWS_UWP || WINDOWS10_0_17763_0_OR_GREATER
                    radio = WindowsBluetoothRadio.GetDefault();
#elif NET462 || WINDOWS7_0_OR_GREATER
                    radio = Win32BluetoothRadio.GetDefault();
#elif NETSTANDARD
#else
                    switch (Environment.OSVersion.Platform)
                    {
                        case PlatformID.Unix:
                            radio = LinuxBluetoothRadio.GetDefault();
                            break;
                        case PlatformID.Win32NT:
                            radio = Win32BluetoothRadio.GetDefault();
                            break;
                    }
#endif
                    if (radio == null)
                        throw new PlatformNotSupportedException();
                    s_default = new BluetoothRadio(radio);
                }

                return s_default;
            }
        }

        private IBluetoothRadio _radio;

        private BluetoothRadio(IBluetoothRadio radio)
        {
            _radio = radio;
        }

        internal IBluetoothRadio Radio { get { return _radio; } }

        /// <summary>
        /// Returns the friendly name of the local Bluetooth radio.
        /// </summary>
        public string Name { get => _radio.Name; }

        /// <summary>
        /// Get the address of the local Bluetooth radio device.
        /// </summary>
        public BluetoothAddress LocalAddress { get => _radio.LocalAddress; }

        /// <summary>
        /// Gets or sets the Scan Mode of the radio.
        /// </summary>
        public RadioMode Mode { get => _radio.Mode; set => _radio.Mode = value; }

        /// <summary>
        /// Gets the company identifier for the radio (if available).
        /// </summary>
        /// <remarks>
        /// On platforms where there is no API to retrieve the manufacturer this property will return <see cref="CompanyIdentifier.Unknown"/>. 
        /// On iOS and macOS this property always returns <see cref="CompanyIdentifier.Apple"/>.
        /// </remarks>
        public CompanyIdentifier Manufacturer { get => _radio.Manufacturer; }

        /// <summary>
        /// Returns the Bluetooth specification version supported (if available).
        /// </summary>
        /// <remarks>
        /// It's not possible to determine this version on all platforms. Instead we make a best guess based on supported features.
        /// <para>Win32 - Can only determine version up to Bluetooth 2.1.</para>
        /// <para>WinRT - Can determine 5.0, 4.0 or 1.0 based on available features.</para>
        /// <para>Android - Returns 5.2 if Bluetooth Audio is supported, 5.0 if Le2M or Coded PHY supported, 4.0 if Low Energy is available, else 1.0.</para>
        /// <para>iOS - Always returns 4.0 as all supported devices support Bluetooth Low Energy.</para>
        /// <para>Linux - Returns 4.0 if local adapter is running a Gatt service, otherwise returns 1.0 as there isn't currently an API to query the exact version.</para>
        /// </remarks>
        public BluetoothVersion LmpVersion { get => _radio.LmpVersion; }

        /// <summary>
        /// LMP Subversion of the radio (if available).
        /// </summary>
        /// <remarks>Currently only supported on Win32.</remarks>
        public ushort LmpSubversion { get => _radio.LmpSubversion; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _radio.Dispose();
                _radio = null;

                if (disposing)
                {
                }

                disposedValue = true;
            }
        }


        #endregion

        ~BluetoothRadio()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
