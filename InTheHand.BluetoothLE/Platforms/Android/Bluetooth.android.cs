//-----------------------------------------------------------------------
// <copyright file="Bluetooth.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static BluetoothManager _manager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
        private static readonly EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static Android.Bluetooth.BluetoothDevice? s_device;
        private static BluetoothReceiver? _receiver;

#if MONOANDROID
        const string PermissionName = "android.permission.BLUETOOTH_CONNECT";
#else
        const string PermissionName = Manifest.Permission.BluetoothConnect;
#endif

        static Bluetooth()
        {
            AndroidActivity.Init();
        }

        private static Task<bool> PlatformGetAvailability()
        {
            return Task.FromResult(_manager != null && _manager.Adapter != null && _manager.Adapter.IsEnabled);
        }

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await PlatformGetAvailability();

            if (_receiver != null)
                return;

            _receiver = new BluetoothReceiver();
            // Register broadcast receiver to monitor state for AvailabilityChanged event
            AndroidActivity.CurrentActivity.RegisterReceiver(_receiver,
                new IntentFilter(BluetoothAdapter.ActionStateChanged));
        }

        private static void RemoveAvailabilityChanged()
        {
        }

        /// <summary>
        /// Performs a device lookup and prompts the user for permission if required.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context">Current activity.</param>
        /// <returns>A BluetoothDevice or null if unsuccessful.</returns>
        public static Task<BluetoothDevice?> RequestDevice(RequestDeviceOptions? options, Activity context)
        {
            InTheHand.AndroidActivity.CurrentActivity = context;
            return PlatformRequestDevice(options);
        }

        private static Task<BluetoothDevice?> PlatformRequestDevice(RequestDeviceOptions? options)
        {
            if (AndroidActivity.CurrentActivity == null)
                return Task.FromResult<BluetoothDevice?>(null);
            
            Intent i = new Intent(AndroidActivity.CurrentActivity, typeof(DevicePickerActivity));
            AndroidActivity.CurrentActivity.StartActivity(i);

            return Task.Run(() =>
            {
                s_handle.WaitOne();

                return Task.FromResult<BluetoothDevice?>(s_device ?? null);
            });
        }

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions? options, CancellationToken cancellationToken = default)
        {
            List<ScanFilter> filters = new List<ScanFilter>();
                
            if (options != null)
            {
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

            var sb = new ScanSettings.Builder();
            // This is a higher power consuming mode but should give better results for fixed discovery period.
            sb.SetScanMode(Android.Bluetooth.LE.ScanMode.LowLatency);
            var settings = sb.Build();
            var callback = new DevicesCallback(options?.Timeout);

            _manager.Adapter.BluetoothLeScanner.StartScan(filters, settings, callback);

            await Task.Run(() =>
            {
                callback.WaitOne();
            });

            _manager.Adapter.BluetoothLeScanner.StopScan(callback);
            

            return callback.Devices.AsReadOnly();
        }

        private static Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            var devices = new List<BluetoothDevice>();

            foreach (var device in _manager.Adapter.BondedDevices)
            {
                if(device.Type == BluetoothDeviceType.Le || device.Type == BluetoothDeviceType.Dual)
                {
                    devices.Add(device);
                }
            }

            return Task.FromResult<IReadOnlyCollection<BluetoothDevice>>(devices.AsReadOnly());
        }

        private class DevicesCallback : ScanCallback
        {
            private readonly TimeSpan _timeout;
            private readonly EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);

            public DevicesCallback(TimeSpan? timeout)
            {
                _timeout = timeout ?? TimeSpan.FromSeconds(5);

            }

            public List<BluetoothDevice> Devices { get; } = new List<BluetoothDevice>();

            public void WaitOne()
            {
                Task.Run(async () =>
                {
                    // ensure discovery times out after fixed delay
                    await Task.Delay(_timeout);
                    handle.Set();
                });
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

                if (!Devices.Contains(result.Device))
                {
                    Devices.Add(result.Device);
                }

                base.OnScanResult(callbackType, result);
            }

            public override void OnScanFailed(ScanFailure errorCode)
            {
                System.Diagnostics.Debug.WriteLine("OnBatchScanResults");

                handle.Set();

                base.OnScanFailed(errorCode);
            }
        }

        private static Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions? options)
        {
            return Task.FromResult(new BluetoothLEScan(options, _manager.Adapter.BluetoothLeScanner));
        }

        [Activity(NoHistory = false, LaunchMode = LaunchMode.Multiple)]
        private sealed class DevicePickerActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                // Android 12+
                if (OperatingSystem.IsAndroidVersionAtLeast(31) && CheckCallingOrSelfPermission(PermissionName) != Permission.Granted)
                {
                    RequestPermissions(new string[] { PermissionName }, 123);
                }
                else
                {
                    StartSystemPicker();
                }
            }

            private void StartSystemPicker()
            {
                Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
                i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Android.App.Application.Context.PackageName);
                i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", Java.Lang.Class.FromType(typeof(DevicePickerReceiver)).Name);
                i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", false);
                i.PutExtra("android.bluetooth.devicepicker.extra.FILTER_TYPE", 0);

                StartActivityForResult(i, 321);
            }

            // set the handle when the picker has completed and return control straight back to the calling activity
            protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                System.Diagnostics.Debug.Write(resultCode.ToString());

                base.OnActivityResult(requestCode, resultCode, data);

                s_handle.Set();

                Finish();
            }

            public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
            {
                bool bluetoothConnectGranted = false;

                // was bluetooth connect granted?
                for (int i = 0; i < permissions.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine($"{permissions[i]} {grantResults[i]}");

                    if (permissions[i] == PermissionName)
                    {
                        bluetoothConnectGranted = grantResults[i] == Permission.Granted;
                        break;
                    }
                }

                if (bluetoothConnectGranted)
                {
                    StartSystemPicker();
                }
                else
                {
                    s_handle.Set();

                    Finish();
                }

                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
    }
}