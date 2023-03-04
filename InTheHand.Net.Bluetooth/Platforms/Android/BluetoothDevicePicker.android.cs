// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Android)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothDevicePicker
    {
        internal static EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static BluetoothDevicePicker s_current;

        internal Android.Bluetooth.BluetoothDevice _device;

        private Task<BluetoothDeviceInfo> DoPickSingleDeviceAsync()
        {
            if (InTheHand.AndroidActivity.CurrentActivity == null)
                throw new NotSupportedException("CurrentActivity was not detected or specified");

            s_current = this;

            Intent i = new Intent(InTheHand.AndroidActivity.CurrentActivity, typeof(DevicePickerActivity));
            InTheHand.AndroidActivity.CurrentActivity.StartActivity(i);

            return Task.Run(() =>
            {
                s_handle.WaitOne();

                if (_device != null)
                {
                    return Task.FromResult<BluetoothDeviceInfo>(_device);
                }
                else
                {
                    return Task.FromResult<BluetoothDeviceInfo>(null);
                }
            });
        }

        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class DevicePickerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                bool paired = false;
                int filterType = 0;

                if(s_current.ClassOfDevices.Count == 1)
                {
                    var bcod = s_current.ClassOfDevices[0];

                    switch (bcod.Service)
                    {
                        case ServiceClass.Audio:
                            filterType = 1;
                            break;

                        case ServiceClass.ObjectTransfer:
                            filterType = 2;
                            break;

                        case ServiceClass.Network:
                            filterType = 3;
                            break;
                    }

                    switch (bcod.MajorDevice)
                    {
                        case DeviceClass.AccessPointAvailable:
                            filterType = 4;
                            break;
                    }
                }

                Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
                i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
                i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", Java.Lang.Class.FromType(typeof(BluetoothDevicePickerReceiver)).Name);
                i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", paired);
                i.PutExtra("android.bluetooth.devicepicker.extra.FILTER_TYPE", filterType);

                InTheHand.AndroidActivity.CurrentActivity.StartActivityForResult(i, 1);
            }

            // set the handle when the picker has completed and return control straight back to the calling activity
            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                base.OnActivityResult(requestCode, resultCode, data);

                Finish();
            }
        }
    }
}
