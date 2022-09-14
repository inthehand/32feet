// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Droid.BluetoothDevicePickerReceiver (Android)
// 
// Copyright (c) 2018-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Content;

namespace InTheHand.Net.Bluetooth.Droid
{
    [BroadcastReceiver(Enabled = true)]
    internal class BluetoothDevicePickerReceiver : BroadcastReceiver
    {
        // receive broadcast if a device is selected and store the device.
        public override void OnReceive(Context context, Intent intent)
        {
            var dev = (Android.Bluetooth.BluetoothDevice)intent.Extras?.Get("android.bluetooth.device.extra.DEVICE");
            BluetoothDevicePicker.s_current._device = dev;
            BluetoothDevicePicker.s_handle.Set();
        }
    }
}
