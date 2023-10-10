// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio (Linux)
// 
// Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Threading.Tasks;
using Linux.Bluetooth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        public static implicit operator Adapter(BluetoothRadio radio)
        {
            return ((LinuxBluetoothRadio)radio.Radio).Adapter;
        }

        public static implicit operator BluetoothRadio(Adapter adapter)
        {
            return new BluetoothRadio((LinuxBluetoothRadio)adapter);
        }
    }

    internal sealed class LinuxBluetoothRadio : IBluetoothRadio
    {

        internal static IBluetoothRadio GetDefault()
        {
            IBluetoothRadio radio = AsyncHelpers.RunSync(async () =>
            {
                Adapter adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
                if (adapter != null)
                {
                    var radio = (LinuxBluetoothRadio)adapter;
                    await radio.Init();
                    return radio;
                }

                return null;
            });
            
            return radio;
        }
 
        public static implicit operator LinuxBluetoothRadio(Adapter adapter)
        {
            return new LinuxBluetoothRadio(adapter);
        }

        public static implicit operator Adapter(LinuxBluetoothRadio radio)
        {
            return radio._adapter;
        }

        private Adapter _adapter;
        internal Adapter Adapter
        {
            get
            {
                return _adapter;
            }
        }

        private LinuxBluetoothRadio(Adapter adapter)
        {
            ArgumentNullException.ThrowIfNull(adapter, nameof(adapter));

            _adapter = adapter;
        }

        private async Task Init()
        {
            var props = await _adapter.GetAllAsync();
            _name = props.Name;
            _address = BluetoothAddress.Parse(props.Address);
        }

        private string _name;
        public string Name {  get =>  _name; }

        private BluetoothAddress _address;
        public BluetoothAddress LocalAddress {  get =>  _address; }

        public RadioMode Mode
        {
            get
            {
                if (AsyncHelpers.RunSync(() => { return _adapter.GetPoweredAsync(); }))
                {
                    if (AsyncHelpers.RunSync(() => { return _adapter.GetDiscoverableAsync(); }))
                    {
                        return RadioMode.Discoverable;
                    }

                    return RadioMode.Connectable;
                }

                return RadioMode.PowerOff;
            }
            set
            {
                switch (value)
                {
                    case RadioMode.Discoverable:
                        AsyncHelpers.RunSync(() => { return _adapter.SetPoweredAsync(true); });
                        AsyncHelpers.RunSync(() => { return _adapter.SetDiscoverableAsync(true); });
                        break;
                    case RadioMode.Connectable:
                        AsyncHelpers.RunSync(() => { return _adapter.SetPoweredAsync(true); });
                        AsyncHelpers.RunSync(() => { return _adapter.SetDiscoverableAsync(false); });
                        break;
                    case RadioMode.PowerOff:
                        AsyncHelpers.RunSync(() => { return _adapter.SetPoweredAsync(false); });
                        break;
                }
            }
        }

        public CompanyIdentifier Manufacturer { get => CompanyIdentifier.Unknown; }

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
