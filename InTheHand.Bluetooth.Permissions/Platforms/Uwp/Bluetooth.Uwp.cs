//-----------------------------------------------------------------------
// <copyright file="Bluetooth.Uwp.cs" company="In The Hand Ltd">
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
using System;
using System.Collections.Generic;

namespace InTheHand.Bluetooth.Permissions
{
    partial class Bluetooth
    {
        /// <inheritdoc/>
        protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
                new[] { "bluetooth" };

    }
}
#endif