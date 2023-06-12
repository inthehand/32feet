// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Multiplatform)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    internal interface IBluetoothRadio
    {
        BluetoothRadio GetDefault();

        string GetName();

        BluetoothAddress GetLocalAddress();

        RadioMode GetMode();

        void SetMode(RadioMode value);
    }

    partial class BluetoothRadio
    {
        static IBluetoothRadio _implementation;
        private static BluetoothRadio GetDefault()
        {
            if (_implementation == null)
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Unix:
                        _implementation = LinuxBluetoothRadio.GetDefault();
                        break;
                    case PlatformID.Win32NT:
                        _implementation = WindowsBluetoothRadio.GetDefault();
                        break;
                    default:
                        return null;
                }
            }

            return _implementation.
        }
        
        private string GetName()
        {
            return string.Empty;
        }

        private BluetoothAddress GetLocalAddress()
        {
            return BluetoothAddress.None;
        }

        private RadioMode GetMode()
        {
            return RadioMode.PowerOff;
        }

        private void SetMode(RadioMode value)
        {
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
                
                disposedValue = true;
            }
        }


        #endregion
    }
}
