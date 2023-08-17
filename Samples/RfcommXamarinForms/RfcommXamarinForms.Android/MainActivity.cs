using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android;

namespace RfcommXamarinForms.Droid
{
    [Activity(Label = "RfcommXamarinForms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            InTheHand.AndroidActivity.CurrentActivity = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation }, 1);

            try
            {
                var ac = InTheHand.AndroidActivity.CurrentActivity;
            }
            catch (Exception e)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(e.Message + "\r\n" + e.StackTrace);
                var alert = builder.Create();
                alert.Show();

                InTheHand.AndroidActivity.CurrentActivity = this;
            }

            try
            {
                var ac = InTheHand.AndroidActivity.CurrentActivity;
            }
            catch (Exception e)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(e.Message + "\r\n" + e.StackTrace);
                var alert = builder.Create();
                alert.Show();
            }

            LoadApplication(new App());

            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}