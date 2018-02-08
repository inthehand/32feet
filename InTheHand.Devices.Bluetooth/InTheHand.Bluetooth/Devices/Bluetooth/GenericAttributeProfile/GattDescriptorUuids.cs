//-----------------------------------------------------------------------
// <copyright file="GattDescriptorUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents an enumeration of the most well known Descriptor UUID values.
    /// </summary>
    public static class GattDescriptorUuids
    {
        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Aggregate Format Descriptor UUID.
        /// </summary>
        public static readonly Guid CharacteristicAggregateFormat = new Guid(0x00002905, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Extended Properties Descriptor UUID.
        /// </summary>
        public static readonly Guid CharacteristicExtendedProperties = new Guid(0x00002900, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Presentation Format Descriptor UUID.
        /// </summary>
        public static readonly Guid CharacteristicPresentationFormat = new Guid(0x00002904, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic User Description Descriptor UUID.
        /// </summary>
        public static readonly Guid CharacteristicUserDescription = new Guid(0x00002901, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Client Characteristic Configuration Descriptor UUID.
        /// </summary>
        public static readonly Guid ClientCharacteristicConfiguration = new Guid(0x00002902, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Guid EnvironmentalSensingConfiguration = new Guid(0x0000290B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid EnvironmentalSensingMeasurement = new Guid(0x0000290C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid EnvironmentalSensingTriggerSetting = new Guid(0x0000290D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid EnvironmentalSensingReportReference = new Guid(0x00002907, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid NumberOfDigitals = new Guid(0x00002909, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid ReportReference = new Guid(0x00002908, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Server Characteristic Configuration Descriptor UUID.
        /// </summary>
        public static readonly Guid ServerCharacteristicConfiguration = new Guid(0x00002903, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid TimeTriggerSetting = new Guid(0x0000290E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid ValidRange = new Guid(0x00002906, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid ValueTriggerSetting = new Guid(0x0000290A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }
}