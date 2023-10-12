// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (WinRT)
// 
// Copyright (c) 2019-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.Radios;
using Windows.Foundation.Metadata;

namespace InTheHand.Net.Bluetooth
{
    internal class WindowsBluetoothRadio : IBluetoothRadio
    {
        private readonly DeviceInformation _info;
        private readonly BluetoothAdapter _adapter;
        private readonly Radio _radio;

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
                    defaultRadio = new WindowsBluetoothRadio(info, adapter, radio);
                }
            });
            t.Wait();

            return defaultRadio;
        }

        public void Dispose()
        {
        }

        private WindowsBluetoothRadio(DeviceInformation info, BluetoothAdapter adapter, Radio radio)
        {
            _info = info;
            _adapter = adapter;
            _radio = radio;
        }
        
        public string Name { get => _info.Name; }

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

        public CompanyIdentifier Manufacturer { get => CompanyIdentifier.Unknown; }

        public BluetoothVersion LmpVersion
        {
            get
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 10))
                {
                    if (_adapter.IsExtendedAdvertisingSupported)
                    {
                        return BluetoothVersion.Version50;
                    }
                }

                if (_adapter.IsLowEnergySupported)
                {
                    return BluetoothVersion.Version40;
                }
                
                return BluetoothVersion.Version21;
            }
        }

        public ushort LmpSubversion { get => 0; }
    }
}
