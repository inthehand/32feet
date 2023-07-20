// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Android)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using Android.OS;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using Java.Lang.Reflect;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private readonly BluetoothDevice _device;

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

        // WIP
        private Guid UUIDToGuid(UUID uuid)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(uuid.MostSignificantBits).CopyTo(bytes, 0); 
            BitConverter.GetBytes(uuid.LeastSignificantBits).CopyTo(bytes, 8);

            return new Guid(bytes);
        }

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            if (cached)
            {
                List<Guid> services = new List<Guid>();
                var uuids = _device.GetUuids();
                foreach (var uuid in uuids)
                {
                    var u = Guid.Parse(uuid.Uuid.ToString());

                    if (u != BluetoothService.BluetoothBase)
                    {
                        services.Add(u);
                    }
                }

                return services.AsReadOnly();
            }
            else
            {
                TaskCompletionSource<IEnumerable<Guid>> servicesReceiver = new TaskCompletionSource<IEnumerable<Guid>>();
                var receiver = new BluetoothUuidReceiver(servicesReceiver);

                InTheHand.AndroidActivity.CurrentActivity.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionUuid));

                bool success = _device.FetchUuidsWithSdp();

                return await servicesReceiver.Task;
            }
        }

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            return new List<Guid>().AsReadOnly();
        }

        void PlatformSetServiceState(Guid service, bool state)
        {
            throw new PlatformNotSupportedException();
        }

        bool GetConnected()
        {
            Method m = _device.Class.GetMethod("isConnected", null);
            bool connected = (bool)m.Invoke(_device, null);
            return connected;
        }

        bool GetAuthenticated()
        {
            return _device.BondState == Bond.Bonded;
        }

        void PlatformRefresh()
        {
        }
    }
}
