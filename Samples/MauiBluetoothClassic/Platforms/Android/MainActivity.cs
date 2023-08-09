using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiBluetoothClassic
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestPermissions(new string[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.BluetoothAdmin }, 1);
        }
    }
}