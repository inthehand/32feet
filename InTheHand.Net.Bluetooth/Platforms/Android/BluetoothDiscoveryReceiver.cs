// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Droid.BluetoothDiscoveryReceiver (Android)
// 
// Copyright (c) 2018-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using System;

namespace InTheHand.Net.Bluetooth.Droid
{
    internal sealed class BluetoothDiscoveryReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            switch(intent.Action)
            {
                case BluetoothAdapter.ActionDiscoveryStarted:
                    System.Diagnostics.Debug.WriteLine("DiscoveryStarted");
                    break;

                case BluetoothAdapter.ActionDiscoveryFinished:
                    System.Diagnostics.Debug.WriteLine("DiscoveryFinished");
                    DiscoveryComplete?.Invoke(this, EventArgs.Empty);
                    break;

                case BluetoothDevice.ActionFound:
                    var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    if (device.Type != BluetoothDeviceType.Le)
                    {
                        System.Diagnostics.Debug.WriteLine($"Found {device.Name} {device.Address} {device.BondState}");
                        DeviceFound?.Invoke(this, device);
                    }
                    break;

                case BluetoothDevice.ActionNameChanged:
                    var device2 = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    System.Diagnostics.Debug.WriteLine($"Name Changed {device2.Name} {device2.Address}");
                    break;

                case BluetoothDevice.ActionAclConnected:
                    break;

                case BluetoothDevice.ActionAclDisconnected:
                    break;
            }
        }

        public event EventHandler<BluetoothDevice> DeviceFound;

        public event EventHandler DiscoveryComplete;
    }
}