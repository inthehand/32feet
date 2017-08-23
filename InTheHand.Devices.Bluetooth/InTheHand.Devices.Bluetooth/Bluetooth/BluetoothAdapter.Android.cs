//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Globalization;
using Android.Content;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(() =>
            {
                if (s_default == null)
                {
                    s_default = new BluetoothAdapter(Android.Bluetooth.BluetoothAdapter.DefaultAdapter);
                }

                return s_default;
            });
        }

        private Android.Bluetooth.BluetoothAdapter _adapter;

        internal BluetoothAdapter(Android.Bluetooth.BluetoothAdapter adapter)
        {
            _adapter = adapter;
            IntentFilter filter = new IntentFilter();
            filter.AddAction(Android.Bluetooth.BluetoothDevice.ActionNameChanged);
            _deviceReceiver = new BluetoothDeviceReceiver(this);
            Android.App.Application.Context.RegisterReceiver(_deviceReceiver, filter);
        }

        private ulong GetBluetoothAddress()
        {
            return ulong.Parse(_adapter.Address.Replace(":", ""), NumberStyles.HexNumber);
        }

        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(0);
        }

        private bool GetIsClassicSupported()
        {
            return true;
        }

        private bool GetIsLowEnergySupported()
        {
            return true;
        }

        private string GetName()
        {
            return _adapter.Name;
        }


        internal event EventHandler<ulong> DeviceConnected;

        internal event EventHandler<ulong> DeviceDisconnected;

        internal event EventHandler<ulong> NameChanged;


        private BluetoothDeviceReceiver _deviceReceiver;


        private sealed class BluetoothDeviceReceiver : BroadcastReceiver
        {
            private BluetoothAdapter _parent;

            internal BluetoothDeviceReceiver(BluetoothAdapter parent)
            {
                _parent = parent;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                ulong address = 0;
                var device = intent.Extras.GetParcelable(Android.Bluetooth.BluetoothDevice.ExtraDevice) as Android.Bluetooth.BluetoothDevice;
                if (device != null)
                {
                    address = ulong.Parse(device.Address.Replace(":", ""), NumberStyles.HexNumber);
                }

                switch (intent.Action)
                {
                    case Android.Bluetooth.BluetoothDevice.ActionAclConnected:
                        _parent.DeviceConnected?.Invoke(_parent,address);
                        break;

                    case Android.Bluetooth.BluetoothDevice.ActionAclDisconnected:
                        _parent.DeviceDisconnected?.Invoke(_parent, address);
                        break;

                    case Android.Bluetooth.BluetoothDevice.ActionNameChanged:
                        _parent.NameChanged?.Invoke(_parent, address);
                        break;
                }
            }
        }
    }
}