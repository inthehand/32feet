// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Win32)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth.Win32;
using System;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        private static BluetoothRadio GetDefault()
        {
            BLUETOOTH_FIND_RADIO_PARAMS p = BLUETOOTH_FIND_RADIO_PARAMS.Create();
            BLUETOOTH_RADIO_INFO info = BLUETOOTH_RADIO_INFO.Create();
            IntPtr findHandle = NativeMethods.BluetoothFindFirstRadio(ref p, out IntPtr hRadio);

            if (hRadio != IntPtr.Zero)
            {              
                int result = NativeMethods.BluetoothGetRadioInfo(hRadio, ref info);
            }

            if(findHandle != IntPtr.Zero)
            {
                NativeMethods.BluetoothFindRadioClose(findHandle);
            }

            if(hRadio != IntPtr.Zero)
            {
                return new BluetoothRadio(info, hRadio);
            }

            return null;
        }

        private BLUETOOTH_RADIO_INFO _radio;
        private IntPtr _handle;

        private BluetoothRadio(BLUETOOTH_RADIO_INFO info, IntPtr handle)
        {
            _radio = info;
            _handle = handle;
        }

        private string GetName()
        {
            return _radio.szName;
        }

        private BluetoothAddress GetLocalAddress()
        {
            return _radio.address;
        }

        private RadioMode GetMode()
        {
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

        private void SetMode(RadioMode value)
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

        #endregion
    }
}