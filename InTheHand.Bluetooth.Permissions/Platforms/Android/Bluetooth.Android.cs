//-----------------------------------------------------------------------
// <copyright file="Bluetooth.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if NET6_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;
#else
using Xamarin.Essentials;
#endif
using Android;
using Android.Content.PM;
using Android.OS;
using System.Linq;
using System.Collections.Generic;
using System;

namespace InTheHand.Bluetooth.Permissions
{
    partial class Bluetooth
    {
        public static bool IsDeclaredInManifest(string permission)
        {
            var context = Android.App.Application.Context;
#pragma warning disable CS0618, CA1416, CA1422 // Deprecated in API 33: https://developer.android.com/reference/android/content/pm/PackageManager#getPackageInfo(java.lang.String,%20int)
            var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Permissions);
#pragma warning restore CS0618, CA1416, CA1422
            var requestedPermissions = packageInfo?.RequestedPermissions;

            return requestedPermissions?.Any(r => r.Equals(permission, StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                var permissions = new List<(string, bool)>();

                // When targeting Android 11 or lower, AccessFineLocation is required for Bluetooth.
                // For Android 12 and above, it is optional.
                if (Android.App.Application.Context.ApplicationInfo.TargetSdkVersion <= (BuildVersionCodes)30 || IsDeclaredInManifest(Manifest.Permission.AccessFineLocation))
                    permissions.Add((Manifest.Permission.AccessFineLocation, true));

#if __ANDROID_31__
                if (OperatingSystem.IsAndroidVersionAtLeast(31) && Android.App.Application.Context.ApplicationInfo.TargetSdkVersion >= BuildVersionCodes.S)
                {
                    // new runtime permissions on Android 12
                    if (IsDeclaredInManifest(Manifest.Permission.BluetoothScan))
                        permissions.Add((Manifest.Permission.BluetoothScan, true));
                    if (IsDeclaredInManifest(Manifest.Permission.BluetoothConnect))
                        permissions.Add((Manifest.Permission.BluetoothConnect, true));
                    if (IsDeclaredInManifest(Manifest.Permission.BluetoothAdvertise))
                        permissions.Add((Manifest.Permission.BluetoothAdvertise, true));
                }
#endif
                return permissions.ToArray();
            }
        }
    }
}