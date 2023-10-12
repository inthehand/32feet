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

        private readonly Adapter _adapter;
        internal Adapter Adapter { get => _adapter; }

        private LinuxBluetoothRadio(Adapter adapter)
        {
            ArgumentNullException.ThrowIfNull(adapter, nameof(adapter));
            _adapter = adapter;
        }

        private async Task Init()
        {
            var props = await _adapter.GetAllAsync();
            _name = props.Name;
            Guid gattService = new(0x00001800, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

            foreach (string uuid in props.UUIDs)
            {
                Console.WriteLine(uuid);

                if(Guid.TryParse(uuid, out Guid guid))
                {
                    if(guid == gattService)
                    {
                        _isLowEnergy = true;
                        break;
                    }
                }
            }

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

        private bool _isLowEnergy;
        public BluetoothVersion LmpVersion { get => _isLowEnergy ? BluetoothVersion.Version40: BluetoothVersion.Version10; }
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
