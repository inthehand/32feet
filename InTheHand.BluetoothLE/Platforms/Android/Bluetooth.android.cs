//-----------------------------------------------------------------------
// <copyright file="Bluetooth.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Xamarin.Essentials;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static BluetoothManager _manager = (BluetoothManager) Application.Context.GetSystemService(Android.App.Application.BluetoothService);
        private static readonly EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static Android.Bluetooth.BluetoothDevice s_device;
        private static RequestDeviceOptions _currentRequest;

        static Task<bool> PlatformGetAvailability()
        {
            return Task.FromResult(BluetoothAdapter.DefaultAdapter.IsEnabled);
        }

        private static bool _oldAvailability;

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await PlatformGetAvailability();
        }

        private static void RemoveAvailabilityChanged()
        {
        }


        static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            _currentRequest = options;
            //Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
            //i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
            //i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", Java.Lang.Class.FromType(typeof(DevicePickerReceiver)).Name);
            //i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", false);

            //Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 1);

            Intent i = new Intent(Platform.CurrentActivity, typeof(DevicePickerActivity));
            Platform.CurrentActivity.StartActivity(i);

            return Task.Run(() =>
            {
                s_handle.WaitOne();

                if (s_device != null)
                {
                    return Task.FromResult<BluetoothDevice>(s_device);
                }
                else
                {
                    return Task.FromResult<BluetoothDevice>(null);
                }
            });
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            if (options != null)
            {
                List<ScanFilter> filters = new List<ScanFilter>();
                foreach (var f in options.Filters)
                {
                    foreach (var u in f.Services)
                    {
                        ScanFilter.Builder b = new ScanFilter.Builder();
                        b.SetServiceUuid(ParcelUuid.FromString(u.Value.ToString()));
                        filters.Add(b.Build());
                    }
                }
            }

            ScanSettings.Builder sb = new ScanSettings.Builder();
            sb.SetScanMode(Android.Bluetooth.LE.ScanMode.Balanced);
            var settings = sb.Build();
            var callback = new DevicesCallback();

            _manager.Adapter.BluetoothLeScanner.StartScan(callback);

            callback.WaitOne();

            return callback.Devices.AsReadOnly();
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            foreach (var device in _manager.Adapter.BondedDevices)
            {
                if(device.Type == BluetoothDeviceType.Le || device.Type == BluetoothDeviceType.Dual)
                {
                    devices.Add(device);
                }
            }

            return devices.AsReadOnly();
        }

        private class DevicesCallback : ScanCallback
        {
            private readonly EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            public List<BluetoothDevice> Devices { get; } = new List<BluetoothDevice>();

            public void WaitOne()
            {
                handle.WaitOne();
            }

            public override void OnBatchScanResults(IList<ScanResult> results)
            {
                System.Diagnostics.Debug.WriteLine("OnBatchScanResults");

                base.OnBatchScanResults(results);
            }

            public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
            {
                System.Diagnostics.Debug.WriteLine("OnScanResult");

                Devices.Add(result.Device);
                if (callbackType == ScanCallbackType.AllMatches)
                    handle.Set();

                base.OnScanResult(callbackType, result);
            }

            public override void OnScanFailed(ScanFailure errorCode)
            {
                System.Diagnostics.Debug.WriteLine("OnBatchScanResults");

                handle.Set();

                base.OnScanFailed(errorCode);
            }
        }

        private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            return new BluetoothLEScan(options, _manager.Adapter.BluetoothLeScanner);
        }

        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class DevicePickerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                

                Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
                i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
                i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", Java.Lang.Class.FromType(typeof(DevicePickerReceiver)).Name);
                i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", false);

                Platform.CurrentActivity.StartActivityForResult(i, 1);

            }

            // set the handle when the picker has completed and return control straight back to the calling activity
            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                System.Diagnostics.Debug.Write(resultCode.ToString());

                base.OnActivityResult(requestCode, resultCode, data);

                s_handle.Set();

                Finish();
            }
        }
    }

    [BroadcastReceiver(Enabled = true)]
    internal class DevicePickerReceiver : BroadcastReceiver
    {
        // receive broadcast if a device is selected and store the device.
        public override void OnReceive(Context context, Intent intent)
        {
            var dev = (Android.Bluetooth.BluetoothDevice)intent.Extras.Get("android.bluetooth.device.extra.DEVICE");
            Bluetooth.s_device = dev;
            
        }
    }
}
