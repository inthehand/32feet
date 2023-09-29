// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (WinRT)
// 
// Copyright (c) 2019-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.Radios;

namespace InTheHand.Net.Bluetooth
{
    internal class WindowsBluetoothRadio : IBluetoothRadio
    {
        private string _name;
        private BluetoothAdapter _adapter;
        private Radio _radio;

        internal static IBluetoothRadio GetDefault()
        {
            IBluetoothRadio defaultRadio = null;
            var t = Task<BluetoothRadio>.Run(async () =>
            {
                var adapter = await BluetoothAdapter.GetDefaultAsync();
                if (adapter != null)
                {
                    var info = await DeviceInformation.CreateFromIdAsync(adapter.DeviceId);

                    var radio = await adapter.GetRadioAsync();
                    defaultRadio = new WindowsBluetoothRadio(info.Name, adapter, radio);
                }
            });
            t.Wait();

            return defaultRadio;
        }

        public void Dispose()
        {
            _adapter = null;
            _radio = null;
        }

        private WindowsBluetoothRadio(string name, BluetoothAdapter adapter, Radio radio)
        {
            _name = name;
            _adapter = adapter;
            _radio = radio;
        }
        
        public string Name { get => _name; }

        public BluetoothAddress LocalAddress { get => new BluetoothAddress(_adapter.BluetoothAddress); }

        public RadioMode Mode
        {
            get
            {
                return _radio.State == RadioState.On ? RadioMode.Connectable : RadioMode.PowerOff;
            }
            set
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().DispatcherQueue.TryEnqueue(async () =>
                                {
                                    if (await Radio.RequestAccessAsync() == RadioAccessStatus.Allowed)
                                    {
                                        await _radio.SetStateAsync(value == RadioMode.PowerOff ? RadioState.Off : RadioState.On);
                                    }
                                });
            }
        }
    }
}
