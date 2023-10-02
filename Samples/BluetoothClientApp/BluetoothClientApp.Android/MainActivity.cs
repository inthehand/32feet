//-----------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BluetoothClientApp.Droid
{
    [Activity(Label = "BluetoothClientApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            InTheHand.AndroidActivity.CurrentActivity = this;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            if (Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.R ||  (CheckSelfPermission("android.permission.BLUETOOTH_CONNECT") == Permission.Granted && CheckSelfPermission("android.permission.BLUETOOTH_SCAN") == Permission.Granted))
                LoadApplication(new App());
            else
            {
                if(ShouldShowRequestPermissionRationale("android.permission.BLUETOOTH_SCAN"))
                {
                    //message
                }

                RequestPermissions(new string[] { "android.permission.BLUETOOTH_SCAN", "android.permission.BLUETOOTH_CONNECT" }, 1);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            LoadApplication(new App());
        }
    }
}
