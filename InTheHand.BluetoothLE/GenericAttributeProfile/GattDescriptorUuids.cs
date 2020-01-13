//-----------------------------------------------------------------------
// <copyright file="GattDescriptorUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/descriptors

using System;
using System.Reflection;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents an enumeration of the most well known Descriptor UUID values.
    /// </summary>
    public static class GattDescriptorUuids
    {
        /// <summary>
        /// Returns the Uuid for a descriptor given the Uniform Type Identifier.
        /// </summary>
        /// <param name="bluetoothUti">Uniform Type Identifier of the service e.g. org.bluetooth.descriptor.gatt.characteristic_aggregate_format</param>
        /// <returns>The descriptor Uuid on success else Guid.Empty.</returns>
        public static Guid FromBluetoothUti(string bluetoothUti)
        {
            string requestedUti = bluetoothUti.ToLower();
            if (!requestedUti.StartsWith("org.bluetooth.descriptor"))
            {
                requestedUti = "org.bluetooth.descriptor." + bluetoothUti.ToLower();
            }

            var fields = typeof(GattServiceUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute(typeof(BluetoothUtiAttribute));
                if (attr != null && ((BluetoothUtiAttribute)attr).Uti == requestedUti)
                {
                    return (Guid)field.GetValue(null);
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Aggregate Format descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_aggregate_format")]
        public static readonly Guid CharacteristicAggregateFormat = new Guid(0x00002905, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Extended Properties descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_extended_properties")]
        public static readonly Guid CharacteristicExtendedProperties = new Guid(0x00002900, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Presentation Format descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_presentation_format")]
        public static readonly Guid CharacteristicPresentationFormat = new Guid(0x00002904, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic User Description descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_user_description")]
        public static readonly Guid CharacteristicUserDescription = new Guid(0x00002901, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Client Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.client_characteristic_configuration")]
        public static readonly Guid ClientCharacteristicConfiguration = new Guid(0x00002902, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// 
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.es_configuration")]
        public static readonly Guid EnvironmentalSensingConfiguration = new Guid(0x0000290B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.es_measurement")]
        public static readonly Guid EnvironmentalSensingMeasurement = new Guid(0x0000290C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.es_trigger_setting")]
        public static readonly Guid EnvironmentalSensingTriggerSetting = new Guid(0x0000290D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.external_report_reference")]
        public static readonly Guid EnvironmentalSensingReportReference = new Guid(0x00002907, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.number_of_digitals")]
        public static readonly Guid NumberOfDigitals = new Guid(0x00002909, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.report_reference")]
        public static readonly Guid ReportReference = new Guid(0x00002908, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Server Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.server_characteristic_configuration")]
        public static readonly Guid ServerCharacteristicConfiguration = new Guid(0x00002903, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.time_trigger_setting")]
        public static readonly Guid TimeTriggerSetting = new Guid(0x0000290E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.valid_range")]
        public static readonly Guid ValidRange = new Guid(0x00002906, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.descriptor.value_trigger_setting")]
        public static readonly Guid ValueTriggerSetting = new Guid(0x0000290A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }
}