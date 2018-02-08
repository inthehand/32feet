// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CBUUIDExtensions.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using CoreBluetooth;
using System.Runtime.InteropServices;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Provides conversion support for CBUUID.
    /// </summary>
    internal static class CBUUIDExtensions
    {
        internal static Guid ToGuid(this CBUUID uuid)
        {
            byte[] b = new byte[16];

            switch(uuid.Data.Length)
            {
                case 2:
                    b = new Guid("0x00000000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB").ToByteArray();
                    Marshal.Copy(uuid.Data.Bytes, b, 0, (int)uuid.Data.Length);

                    break;
                default:
                    
                    Marshal.Copy(uuid.Data.Bytes, b, 0, (int)uuid.Data.Length);
                    break;
            }

            return new Guid(b);
        }
    }
}