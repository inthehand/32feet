//-----------------------------------------------------------------------
// <copyright file="BluetoothPermissions.Windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-23 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if NET6_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;
#else
using Xamarin.Essentials;
#endif
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.Permissions
{
    // <summary>
    //	BluetoothPermissions (.NET MAUI on Windows).
    // </summary>
    partial class Bluetooth
    {
        // All the code in this file is only included on Windows.
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            // for a desktop .NET 6.0 app you don't need to explicitly request Bluetooth in the manifest
            return Task.FromResult(PermissionStatus.Granted);
        }
    }
}