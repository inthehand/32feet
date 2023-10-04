//-----------------------------------------------------------------------
// <copyright file="BluetoothPermissions.MacCatalyst.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth.Permissions
{
    // <summary>
    //	BluetoothPermissions (.NET MAUI on macOS Catalyst).
    // </summary>
    partial class BluetoothPermissions
    {
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