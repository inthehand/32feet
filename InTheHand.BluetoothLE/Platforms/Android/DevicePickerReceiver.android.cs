//-----------------------------------------------------------------------
// <copyright file="DevicePickerReceiver.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android.Content;
using System;

namespace InTheHand.Bluetooth
{
    [BroadcastReceiver(Enabled = true)]
    internal class DevicePickerReceiver : BroadcastReceiver
    {
        // receive broadcast if a device is selected and store the device.
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                Bluetooth.s_device = (Android.Bluetooth.BluetoothDevice?)intent?.GetParcelableExtra("android.bluetooth.device.extra.DEVICE", Java.Lang.Class.ForName("android.Bluetooth.BluetoothDevice"));
            }
            else
            {
                Bluetooth.s_device = (Android.Bluetooth.BluetoothDevice?)intent?.GetParcelableExtra("android.bluetooth.device.extra.DEVICE");
            }
        }
    }
}
