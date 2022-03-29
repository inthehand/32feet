// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

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
                    s_default = GetDefault();
                }

                return s_default;
            }
        }
        
        private BluetoothRadio()
        {

        }

        /// <summary>
        /// Returns the friendly name of the local Bluetooth radio.
        /// </summary>
        public string Name
        {
            get
            {
                return GetName();
            }
        }

        /// <summary>
        /// Get the address of the local Bluetooth radio device.
        /// </summary>
        public BluetoothAddress LocalAddress
        {
            get
            {
                return GetLocalAddress();
            }
        }

        /// <summary>
        /// Gets or sets the Scan Mode of the radio.
        /// </summary>
        public RadioMode Mode
        {
            get
            {
                return GetMode();
            }
            set
            {
                SetMode(value);
            }
        }

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
