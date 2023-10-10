//-----------------------------------------------------------------------
// <copyright file="Bluetooth.MacCatalyst.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if NET6_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;
#else
using Xamarin.Essentials;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.Permissions
{
    partial class Bluetooth
    {
        protected override Func<IEnumerable<string>> RequiredInfoPlistKeys => () => new string[] { "NSBluetoothAlwaysUsageDescription" };

        public override Task<PermissionStatus> CheckStatusAsync()
        {
            switch(CoreBluetooth.CBManager.Authorization)
            {
                case CoreBluetooth.CBManagerAuthorization.AllowedAlways:
                    return Task.FromResult(PermissionStatus.Granted);

                case CoreBluetooth.CBManagerAuthorization.Denied:
                    return Task.FromResult(PermissionStatus.Denied);

                case CoreBluetooth.CBManagerAuthorization.Restricted:
                    return Task.FromResult(PermissionStatus.Restricted);

                default:
                    return Task.FromResult(PermissionStatus.Unknown);
            }
        }
    }
}