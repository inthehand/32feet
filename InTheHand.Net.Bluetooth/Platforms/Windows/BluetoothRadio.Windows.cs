// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (WinRT)
// 
// Copyright (c) 2019-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.Radios;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        static BluetoothRadio _default;

        private string _name;
        private BluetoothAdapter _adapter;
        private Radio _radio;

        private static BluetoothRadio GetDefault()
        {
            if (_default == null)
            {
                var t = Task<BluetoothRadio>.Run(async () =>
                {
                    var adapter = await BluetoothAdapter.GetDefaultAsync();
                    if(adapter != null)
                    {
                        var info = await DeviceInformation.CreateFromIdAsync(adapter.DeviceId);

                        var radio = await adapter.GetRadioAsync();
                        _default = new BluetoothRadio(info.Name, adapter, radio);
                    }
                });
                t.Wait();
            }

            return _default;
        }

        private BluetoothRadio(string name, BluetoothAdapter adapter, Radio radio)
        {
            _name = name;
            _adapter = adapter;
            _radio = radio;
        }
        
        private string GetName()
        {
            return _name;
        }

        private BluetoothAddress GetLocalAddress()
        {
            return new BluetoothAddress(_adapter.BluetoothAddress);
        }

        private RadioMode GetMode()
        {
            return _radio.State == RadioState.On ? RadioMode.Connectable : RadioMode.PowerOff;
        }

        private void SetMode(RadioMode value)
        {
            Windows.UI.Core.CoreWindow.GetForCurrentThread().DispatcherQueue.TryEnqueue(async () =>
                {
                    if (await Radio.RequestAccessAsync() == RadioAccessStatus.Allowed)
                    {
                        await _radio.SetStateAsync(value == RadioMode.PowerOff ? RadioState.Off : RadioState.On);
                    }
                });
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
