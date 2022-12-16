// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private BluetoothDevice _device;
        private BluetoothReceiver _receiver;

        internal BluetoothDeviceInfo(BluetoothDevice device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            _device = device;
        }

        public static implicit operator BluetoothDevice(BluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._device;
        }

        public static implicit operator BluetoothDeviceInfo(BluetoothDevice device)
        {
            return new BluetoothDeviceInfo(device);
        }

        BluetoothAddress GetDeviceAddress()
        {
            return BluetoothAddress.Parse(_device.Address);
        }

        string GetDeviceName()
        {
            return _device.Name;
        }

        private ClassOfDevice _cod;
        ClassOfDevice GetClassOfDevice()
        {
            if (_cod == 0)
            {
                _cod = ClassOfDeviceHelper.ToClassOfDevice(_device.BluetoothClass);
            }

            return _cod;
        }

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            List<Guid> services = new List<Guid>();

            if (cached)
            {
                var uuids = _device.GetUuids();
                foreach (var uuid in uuids)
                {
                    services.Add(new Guid(uuid.Uuid.ToString()));
                }
            }
            else
            {
                if (_receiver == null)
                {
                    _receiver = new BluetoothReceiver(this);
#if NET6_0_OR_GREATER
                    Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.RegisterReceiver(_receiver, new IntentFilter(BluetoothDevice.ActionUuid));
#else
                    Xamarin.Essentials.Platform.CurrentActivity.RegisterReceiver(_receiver, new IntentFilter(BluetoothDevice.ActionUuid));
#endif
                }

                bool success = _device.FetchUuidsWithSdp();
                await Task.Run(() =>
                {
                    servicesHandle.WaitOne();

                    if (serviceUuids != null)
                        services.AddRange(serviceUuids);
                });

            }

            return services;
        }

        private EventWaitHandle servicesHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private IEnumerable<Guid> serviceUuids = null;

        private void FetchedServiceUuids(IEnumerable<Guid> uuids)
        { 
            serviceUuids = uuids;
            servicesHandle.Set();
        }

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void DoSetServiceState(Guid service, bool state)
        {
        }

        bool GetConnected()
        {
            return false;
        }

        bool GetAuthenticated()
        {
            return _device.BondState == Bond.Bonded;
        }

        void DoRefresh()
        {
        }

        [BroadcastReceiver(Enabled = true, Exported = false)]
        private class BluetoothReceiver : BroadcastReceiver
        {
            private BluetoothDeviceInfo _owner;

            public BluetoothReceiver() { }

            public BluetoothReceiver(BluetoothDeviceInfo owner)
            {
                _owner = owner;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                if(intent.Action == BluetoothDevice.ActionUuid)
                {
                    // process uuids;
                    if(intent.Extras.ContainsKey(BluetoothDevice.ExtraUuid))
                    {
                        var list = intent.GetParcelableArrayExtra(BluetoothDevice.ExtraUuid);
                        if(list == null)
                            return;

                        List<Guid> uuids = new List<Guid>();
                        foreach(var item in list)
                        {
                            var uuid = item as ParcelUuid;
                            System.Diagnostics.Debug.WriteLine(uuid.ToString());
                            uuids.Add(Guid.Parse(uuid.ToString()));
                        }

                        _owner.FetchedServiceUuids(uuids);

                    }
                }
            }
        }
    }
}
