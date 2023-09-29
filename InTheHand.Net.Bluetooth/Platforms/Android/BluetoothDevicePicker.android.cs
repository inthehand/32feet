// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothDevicePicker (Android)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.App;
using Android.Content;
using InTheHand.Net.Bluetooth.Droid;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Net.Bluetooth
{
    internal sealed class AndroidBluetoothDevicePicker : IBluetoothDevicePicker
    {
        internal static EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static AndroidBluetoothDevicePicker s_current;

        internal Android.Bluetooth.BluetoothDevice _device;

        public Task<BluetoothDeviceInfo> PickSingleDeviceAsync(List<ClassOfDevice> classOfDevices, bool requiresAuthentication)
        {
            if (InTheHand.AndroidActivity.CurrentActivity == null)
                throw new NotSupportedException("CurrentActivity was not detected or specified");

            s_current = this;

            int filterType = 0;

            if (classOfDevices != null && classOfDevices.Count == 1)
            {
                var bcod = classOfDevices[0];

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
            i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", requiresAuthentication);
            i.PutExtra("android.bluetooth.devicepicker.extra.FILTER_TYPE", filterType);

            InTheHand.AndroidActivity.CurrentActivity.StartActivityForResult(i, 1);

            return Task.Run(() =>
            {
                s_handle.WaitOne();

                if (_device != null)
                {
                    return Task.FromResult<BluetoothDeviceInfo>(new AndroidBluetoothDeviceInfo(_device));
                }
                else
                {
                    return Task.FromResult<BluetoothDeviceInfo>(null);
                }
            });
        }
    }
}
