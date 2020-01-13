using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static BluetoothManager _manager = (BluetoothManager) Application.Context.GetSystemService(Android.App.Application.BluetoothService);
        private static EventWaitHandle s_handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static Android.Bluetooth.BluetoothDevice s_device;
        private static RequestDeviceOptions _currentRequest;

        Task<bool> DoGetAvailability()
        {
            return Task.FromResult(BluetoothAdapter.DefaultAdapter.IsEnabled);
        }

        private bool _oldAvailability;

        private async void AddAvailabilityChanged()
        {
            _oldAvailability = await DoGetAvailability();
        }

        private void RemoveAvailabilityChanged()
        {
        }


        Task<BluetoothDevice> DoRequestDevice(RequestDeviceOptions options)
        {
            _currentRequest = options;
            //Intent i = new Intent("android.bluetooth.devicepicker.action.LAUNCH");
            //i.PutExtra("android.bluetooth.devicepicker.extra.LAUNCH_PACKAGE", Application.Context.PackageName);
            //i.PutExtra("android.bluetooth.devicepicker.extra.DEVICE_PICKER_LAUNCH_CLASS", Java.Lang.Class.FromType(typeof(DevicePickerReceiver)).Name);
            //i.PutExtra("android.bluetooth.devicepicker.extra.NEED_AUTH", false);

            //Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 1);

            Intent i = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, typeof(DevicePickerActivity));
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);

            return Task.Run<BluetoothDevice>(() =>
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

                Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivityForResult(i, 1);

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
