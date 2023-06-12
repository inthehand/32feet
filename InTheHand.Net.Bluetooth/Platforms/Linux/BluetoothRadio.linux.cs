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

        private static BluetoothRadio GetDefault()
        {
            BluetoothRadio radio = AsyncHelpers.RunSync(async () =>
            {
                Adapter adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
                if (adapter != null)
                {
                    var radio = (BluetoothRadio)adapter;
                    await radio.Init();
                    return radio;
                }

                return null;
            });
            
            return radio;
        }
 
        public static implicit operator BluetoothRadio(Adapter adapter)
        {
            return new BluetoothRadio(adapter);
        }

        public static implicit operator Adapter(BluetoothRadio radio)
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

        private BluetoothRadio(Adapter adapter)
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
        string GetName()
        {
            return _name;
        }

        private BluetoothAddress _address;
        BluetoothAddress GetLocalAddress()
        {
            return _address;
        }

        RadioMode GetMode()
        {
            if(AsyncHelpers.RunSync(() => { return _adapter.GetPoweredAsync(); }))
            {
                if (AsyncHelpers.RunSync(() => { return _adapter.GetDiscoverableAsync(); }))
                {
                    return RadioMode.Discoverable;
                }

                return RadioMode.Connectable;
            }
            return RadioMode.PowerOff;
        }

        void SetMode(RadioMode value)
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
