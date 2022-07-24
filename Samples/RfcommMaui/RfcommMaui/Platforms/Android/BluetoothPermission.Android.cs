using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Maui.ApplicationModel
{
    public class BluetoothPermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                var permissions = new List<(string androidPermission, bool isRuntime)>();

                if(Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.S )
                {
                    permissions.Add((global::Android.Manifest.Permission.BluetoothConnect, true));
                }
                else if(Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.R)
                {
                    permissions.Add((global::Android.Manifest.Permission.AccessFineLocation, true));
                }

                return permissions.ToArray();
            }
        } 
    }
}
