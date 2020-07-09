//-----------------------------------------------------------------------
// <copyright file="BluetoothUuid.android.cs" company="In The Hand Ltd">
//   Copyright (c) 2019-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using Java.Util;
using System;

namespace InTheHand.Bluetooth
{
    public static class BluetoothUuidExtensions
    {
        public static UUID ToUuid(this Guid g)
        {
            return UUID.FromString(g.ToString());
        }

        public static Guid ToGuid(this UUID u)
        {
            return Guid.Parse(u.ToString());
        }
    }
}
