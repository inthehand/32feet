//-----------------------------------------------------------------------
// <copyright file="GattPresentationFormatTypes.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the different well-known values that the GattPresentationFormat.FormatType property can take.
    /// </summary>
    public enum GattPresentationFormatTypes : byte
    {
        Boolean = 1,
        Bit2 = 2,
        Nibble = 3,
        UInt8 = 4,
        UInt12 = 5,
        UInt16 = 6,
        UInt24 = 7,
        UInt32 = 8,
        UInt48 = 9,
        UInt64 = 10,
        UInt128 = 11,
        SInt8 = 12,
        SInt12 = 13,
        SInt16 = 14,
        SInt24 = 15,
        SInt32 = 16,
        SInt48 = 17,
        SInt64 = 18,
        SInt128 = 19,
        Float32 = 20,
        Float64 = 21,
        SFloat = 22,
        Float = 23,
        DUInt16 = 24,
        Utf8 = 25,
        Utf16 = 26,
        Struct = 27,
    }

#if __UNIFIED__
    internal static class GattCharacteristicPropertiesHelper
    {
        public static GattCharacteristicProperties ToGattCharacteristicProperties(this CoreBluetooth.CBCharacteristicProperties value)
        {
            GattCharacteristicProperties p = GattCharacteristicProperties.None;

            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.AuthenticatedSignedWrites))
            {
                p |= GattCharacteristicProperties.AuthenticatedSignedWrites;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Broadcast))
            {
                p |= GattCharacteristicProperties.Broadcast;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.ExtendedProperties))
            {
                p |= GattCharacteristicProperties.ExtendedProperties;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Indicate))
            {
                p |= GattCharacteristicProperties.Indicate;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Notify))
            {
                p |= GattCharacteristicProperties.Notify;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Read))
            {
                p |= GattCharacteristicProperties.Read;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.Write))
            {
                p |= GattCharacteristicProperties.Write;
            }
            if (value.HasFlag(CoreBluetooth.CBCharacteristicProperties.WriteWithoutResponse))
            {
                p |= GattCharacteristicProperties.WriteWithoutResponse;
            }

            return p;
        }

        public static CoreBluetooth.CBCharacteristicProperties ToCBCharacteristicProperties(this GattCharacteristicProperties value)
        {
            CoreBluetooth.CBCharacteristicProperties p =  (CoreBluetooth.CBCharacteristicProperties)0;

            if(value.HasFlag(GattCharacteristicProperties.AuthenticatedSignedWrites))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.AuthenticatedSignedWrites;
            }
            if (value.HasFlag(GattCharacteristicProperties.Broadcast))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Broadcast;
            }
            if (value.HasFlag(GattCharacteristicProperties.ExtendedProperties))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.ExtendedProperties;
            }
            if (value.HasFlag(GattCharacteristicProperties.Indicate))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Indicate;
            }
            if (value.HasFlag(GattCharacteristicProperties.Notify))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Notify;
            }
            if (value.HasFlag(GattCharacteristicProperties.Read))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Read;
            }
            if (value.HasFlag(GattCharacteristicProperties.Write))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.Write;
            }
            if (value.HasFlag(GattCharacteristicProperties.WriteWithoutResponse))
            {
                p |= CoreBluetooth.CBCharacteristicProperties.WriteWithoutResponse;
            }

            return p;
        }
    }
#endif
}