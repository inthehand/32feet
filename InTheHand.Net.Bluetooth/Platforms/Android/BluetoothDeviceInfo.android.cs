// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Android)
// 
// Copyright (c) 2003-2025 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Droid;
using Java.Lang.Reflect;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Net.Sockets
{
    internal sealed class AndroidBluetoothDeviceInfo : IBluetoothDeviceInfo
    {
        private readonly BluetoothDevice _device;

        internal AndroidBluetoothDeviceInfo(BluetoothAddress address)
        {
            var device = ((BluetoothAdapter)BluetoothRadio.Default).GetRemoteDevice(address.ToString());

            if (device is null)
                throw new ArgumentException(nameof(address));

            _device = device;
        }

        internal AndroidBluetoothDeviceInfo(BluetoothDevice device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device));

            _device = device;
        }

        public static implicit operator BluetoothDevice(AndroidBluetoothDeviceInfo deviceInfo)
        {
            return deviceInfo._device;
        }

        public static implicit operator AndroidBluetoothDeviceInfo(BluetoothDevice device)
        {
            return new AndroidBluetoothDeviceInfo(device);
        }

        public BluetoothAddress DeviceAddress { get => BluetoothAddress.Parse(_device.Address); }

        public string DeviceName { get => _device.Name; }

        private ClassOfDevice _cod;
        public ClassOfDevice ClassOfDevice
        {
            get
            {
                if (_cod == 0)
                {
                    _cod = ClassOfDeviceHelper.ToClassOfDevice(_device.BluetoothClass);
                }

                return _cod;
            }
        }

        // WIP
        private Guid UUIDToGuid(UUID uuid)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(uuid.MostSignificantBits).CopyTo(bytes, 0);
            BitConverter.GetBytes(uuid.LeastSignificantBits).CopyTo(bytes, 8);

            return new Guid(bytes);
        }

        public async Task<IEnumerable<Guid>> GetRfcommServicesAsync(bool cached)
        {
            if (cached)
            {
                List<Guid> services = new List<Guid>();
                var uuids = _device.GetUuids();
                if (uuids != null)
                {
                    foreach (var uuid in uuids)
                    {
                        var u = Guid.Parse(uuid.Uuid.ToString());

                        if (u != BluetoothService.BluetoothBase)
                        {
                            services.Add(u);
                        }
                    }
                }

                return services.AsReadOnly();
            }

            var servicesReceiver = new TaskCompletionSource<IEnumerable<Guid>>();
            var receiver = new BluetoothUuidReceiver(servicesReceiver);

            AndroidActivity.CurrentActivity.RegisterReceiver(receiver, new IntentFilter(BluetoothDevice.ActionUuid));

            bool success = _device.FetchUuidsWithSdp();

            if (!success)
            {
                AndroidActivity.CurrentActivity.UnregisterReceiver(receiver);
                return System.Array.AsReadOnly(System.Array.Empty<Guid>());
            }

            return await servicesReceiver.Task;
        }

        void IBluetoothDeviceInfo.Refresh() { }

        public bool Connected
        {
            get
            {
                Method m = _device.Class.GetMethod("isConnected", null);
                bool connected = (bool)m.Invoke(_device, null);
                return connected;
            }
        }

        public bool Authenticated {  get => _device.BondState == Bond.Bonded; }
    }
}