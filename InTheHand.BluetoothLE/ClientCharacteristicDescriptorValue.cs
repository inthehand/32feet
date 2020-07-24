//-----------------------------------------------------------------------
// <copyright file="ClientCharacteristicDescriptorValue.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth
{
    [Flags]
    internal enum ClientCharacteristicDescriptorValue
    {
        None = 0,
        Notify = 1,
        Indicate = 2,
    }
}
