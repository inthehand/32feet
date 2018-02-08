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
}