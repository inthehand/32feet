// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using System.Collections.Generic;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {

        private static BluetoothRadio GetDefault()
        {
            return new BluetoothRadio(BluetoothAdapter.DefaultAdapter);
        }
 
        private BluetoothAdapter _adapter;

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
