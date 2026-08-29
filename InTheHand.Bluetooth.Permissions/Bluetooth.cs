//-----------------------------------------------------------------------
// <copyright file="Bluetooth.cs" company="In The Hand Ltd">
//   Copyright (c) 2022-26 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth.Permissions
{
    /// <summary>
    /// Additional Permission for the Xamarin Essentials and .NET MAUI Permissions systems.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Obsolete("Use the built-in Microsoft.Maui.ApplicationModel.Permissions.Bluetooth permission in .NET 8.0 and above", true)]
    public class Bluetooth : Microsoft.Maui.ApplicationModel.Permissions.Bluetooth
    {
    }
}