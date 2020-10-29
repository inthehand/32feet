// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth
{
    public sealed partial class BluetoothRadio : IDisposable
    {
        private static  BluetoothRadio s_default;

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

        public string Name
        {
            get
            {
                return GetName();
            }
        }

        public BluetoothAddress LocalAddress
        {
            get
            {
                return GetLocalAddress();
            }
        }

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
