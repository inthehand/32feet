// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Android)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using System;
using System.Runtime.CompilerServices;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        public static implicit operator BluetoothAdapter(BluetoothRadio radio)
        {
            return ((AndroidBluetoothRadio)radio.Radio).Adapter;
        }

        public static implicit operator BluetoothRadio(BluetoothAdapter adapter)
        {
            return new BluetoothRadio((AndroidBluetoothRadio)adapter);
        }
    }
    
    internal sealed class AndroidBluetoothRadio : IBluetoothRadio
    {
        private static readonly BluetoothManager manager;

        static AndroidBluetoothRadio()
        {
            manager = InTheHand.AndroidActivity.CurrentActivity.GetSystemService(Context.BluetoothService) as BluetoothManager;
        }

        internal static BluetoothManager Manager { get => manager; }

        internal static IBluetoothRadio GetDefault()
        {
            if (manager == null)
                throw new PlatformNotSupportedException();

            if (manager.Adapter == null)
                return null;

            return new AndroidBluetoothRadio(manager.Adapter);
        }

        public static implicit operator BluetoothAdapter(AndroidBluetoothRadio radio)
        {
            return radio._adapter;
        }

        public static implicit operator AndroidBluetoothRadio(BluetoothAdapter adapter)
        {
            return new AndroidBluetoothRadio(adapter);
        }

        private readonly BluetoothAdapter _adapter;

        internal BluetoothAdapter Adapter { get => _adapter; }

        private AndroidBluetoothRadio(BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        public string Name { get => _adapter.Name; }

        public BluetoothAddress LocalAddress { get => BluetoothAddress.Parse(_adapter.Address); }

        public RadioMode Mode
        {
            get
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
            set
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
        }

        public CompanyIdentifier Manufacturer { get => CompanyIdentifier.Unknown; }

        public BluetoothVersion LmpVersion
        {
            get
            {
                // make best guess at supported version based on the features which Android exposes.

                if (OperatingSystem.IsAndroidVersionAtLeast(26))
                {
#if NET7_0_OR_GREATER
                    if (OperatingSystem.IsAndroidVersionAtLeast(33))
                    {
                        if(_adapter.IsLeAudioSupported() == (int)CurrentBluetoothStatusCodes.FeatureSupported) 
                            return BluetoothVersion.Version52;
                    }
#endif

                    if (_adapter.IsLe2MPhySupported || _adapter.IsLeCodedPhySupported || _adapter.IsLeExtendedAdvertisingSupported || _adapter.IsLePeriodicAdvertisingSupported)
                        return BluetoothVersion.Version50;
                }

                if (Android.App.Application.Context.PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe))
                    return BluetoothVersion.Version40;

                return BluetoothVersion.Version10;
            }
        }

        public ushort LmpSubversion { get => 0; }

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

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}