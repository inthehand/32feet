//-----------------------------------------------------------------------
// <copyright file="Bluetooth.Windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if NET8_0_OR_GREATER
#else

#if NET6_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;
#else
using Xamarin.Essentials;
#endif
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.Permissions
{
    // <summary>
    //	Bluetooth (.NET MAUI on Windows).
    // </summary>
    partial class Bluetooth
    {
        /// <inheritdoc/>
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            // for a desktop .NET 6.0 app you don't need to explicitly request Bluetooth in the manifest
            return Task.FromResult(PermissionStatus.Granted);
        }
    }
}
#endif