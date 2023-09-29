// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Win32)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class Win32BluetoothRadio : IBluetoothRadio
    {
        internal static IBluetoothRadio GetDefault()
        {
            BLUETOOTH_FIND_RADIO_PARAMS p = BLUETOOTH_FIND_RADIO_PARAMS.Create();
            BLUETOOTH_RADIO_INFO info = BLUETOOTH_RADIO_INFO.Create();
            IntPtr findHandle = NativeMethods.BluetoothFindFirstRadio(ref p, out IntPtr hRadio);

            if (hRadio != IntPtr.Zero)
            {
                int result = NativeMethods.BluetoothGetRadioInfo(hRadio, ref info);
            }

            if (findHandle != IntPtr.Zero)
            {
                NativeMethods.BluetoothFindRadioClose(findHandle);
            }

            if (hRadio != IntPtr.Zero)
            {
                return new Win32BluetoothRadio(info, hRadio);
            }

            return null;
        }

        private BLUETOOTH_RADIO_INFO _radio;
        private IntPtr _handle;

        private Win32BluetoothRadio(BLUETOOTH_RADIO_INFO info, IntPtr handle)
        {
            _radio = info;
            _handle = handle;
        }

        public string Name { get => _radio.szName; }

        public BluetoothAddress LocalAddress { get => _radio.address; }

        /*private void ReadLocalRadioInfo()
        {
            NativeMethods.BTH_LOCAL_RADIO_INFO buffer = new NativeMethods.BTH_LOCAL_RADIO_INFO();
            int returned;
            bool result = NativeMethods.DeviceIoControl(_handle, IOCTL_BTH.GET_LOCAL_INFO, IntPtr.Zero, 0, ref buffer, Marshal.SizeOf(buffer), out returned, IntPtr.Zero);
        }*/

        public RadioMode Mode
        {
            get
            {
                // if radio has been turned off enumeration will no longer return a radio handle
                if (GetDefault() == null)
                    return RadioMode.PowerOff;

                if (NativeMethods.BluetoothIsDiscoverable(_handle))
                {
                    return RadioMode.Discoverable;
                }

                if (NativeMethods.BluetoothIsConnectable(_handle))
                {
                    return RadioMode.Connectable;
                }

                return RadioMode.PowerOff;
            }
            set
            {
                switch (value)
                {
                    case RadioMode.Discoverable:
                        if (Mode == RadioMode.PowerOff)
                        {
                            NativeMethods.BluetoothEnableIncomingConnections(_handle, true);
                        }

                        NativeMethods.BluetoothEnableDiscovery(_handle, true);
                        break;

                    case RadioMode.Connectable:
                        if (Mode == RadioMode.Discoverable)
                        {
                            NativeMethods.BluetoothEnableDiscovery(_handle, false);
                        }
                        else
                        {
                            NativeMethods.BluetoothEnableIncomingConnections(_handle, true);
                        }
                        break;

                    case RadioMode.PowerOff:
                        if (Mode == RadioMode.Discoverable)
                        {
                            NativeMethods.BluetoothEnableDiscovery(_handle, false);
                        }

                        NativeMethods.BluetoothEnableIncomingConnections(_handle, false);
                        break;
                }
            }
        }

        /// <summary>
        /// Manufacturer's revision number of the LMP implementation.
        /// </summary>
        public int LmpSubversion
        {
            get
            {
                return _radio.lmpSubversion;
            }
        }

        /// <summary>
        /// Returns the manufacturer of the BluetoothRadio device.
        /// </summary>
        public ushort Manufacturer
        {
            get
            {
                return _radio.manufacturer;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if(_handle != IntPtr.Zero)
                {
                    NativeMethods.CloseHandle(_handle);
                    _handle = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
