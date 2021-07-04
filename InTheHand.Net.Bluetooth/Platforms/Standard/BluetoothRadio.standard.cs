// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (.NET Standard)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        private static BluetoothRadio GetDefault()
        {
            return null;
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
