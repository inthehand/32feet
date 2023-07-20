// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Android)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using System;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        private static BluetoothManager manager;

        static BluetoothRadio()
        {
            manager = InTheHand.AndroidActivity.CurrentActivity.GetSystemService(Context.BluetoothService) as BluetoothManager;
        }

        internal static BluetoothManager Manager { get => manager; }

        private static BluetoothRadio GetDefault()
        {
            if (manager == null)
                throw new PlatformNotSupportedException();

            if (manager.Adapter == null)
                return null;

            return new BluetoothRadio(manager.Adapter);
        }

        public static implicit operator BluetoothAdapter(BluetoothRadio radio)
        {
            return radio._adapter;
        }

        public static implicit operator BluetoothRadio(BluetoothAdapter adapter)
        {
            return new BluetoothRadio(adapter);
        }

        private readonly BluetoothAdapter _adapter;

        internal BluetoothAdapter Adapter { get => _adapter; }

        private BluetoothRadio(BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        private string GetName()
        {
            return _adapter.Name;
        }

        private BluetoothAddress GetLocalAddress()
        {
            return BluetoothAddress.Parse(_adapter.Address);
        }

        private RadioMode GetMode()
        {
            State state = _adapter.State;

            switch (state)
            {
                case State.TurningOff:
                case State.Off:
                    return RadioMode.PowerOff;

                default:
                    switch (_adapter.ScanMode)
                    {
                        case ScanMode.ConnectableDiscoverable:
                            return RadioMode.Discoverable;

                        case ScanMode.Connectable:
                            return RadioMode.Connectable;

                        case ScanMode.None:
                        default:
                            return RadioMode.PowerOff;
                    }
            }
        }

        private void SetMode(RadioMode value)
        {
            switch (value)
            {
                case RadioMode.PowerOff:
                    _adapter.Disable();
                    break;

                default:
                    // TODO: Determine if setting ScanMode is possible
                    if (!_adapter.IsEnabled)
                        _adapter.Enable();

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

                disposedValue = true;
            }
        }


        #endregion
    }
}
