//-----------------------------------------------------------------------
// <copyright file="GattDescriptorUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/descriptors

using System;
using System.Reflection;

namespace InTheHand.Bluetooth
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
        /// <returns>The descriptor Uuid on success else an empty <see cref="BluetoothUuid"/>.</returns>
        public static BluetoothUuid FromBluetoothUti(string bluetoothUti)
        {
            string requestedUti = bluetoothUti.ToLower();
            if (!requestedUti.StartsWith("org.bluetooth.descriptor"))
            {
                requestedUti = "org.bluetooth.descriptor." + bluetoothUti.ToLower();
            }

            var fields = typeof(GattDescriptorUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute(typeof(BluetoothUtiAttribute));
                if (attr != null && ((BluetoothUtiAttribute)attr).Uti == requestedUti)
                {
                    return (BluetoothUuid)field.GetValue(null);
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Aggregate Format descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_aggregate_format")]
        public static readonly BluetoothUuid CharacteristicAggregateFormat = 0x2905;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Extended Properties descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_extended_properties")]
        public static readonly BluetoothUuid CharacteristicExtendedProperties = 0x2900;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Presentation Format descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_presentation_format")]
        public static readonly BluetoothUuid CharacteristicPresentationFormat = 0x2904;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic User Description descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.characteristic_user_description")]
        public static readonly BluetoothUuid CharacteristicUserDescription = 0x2901;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Client Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.client_characteristic_configuration")]
        public static readonly BluetoothUuid ClientCharacteristicConfiguration = 0x2902;

        /// <summary>
        /// 
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.es_configuration")]
        public static readonly BluetoothUuid EnvironmentalSensingConfiguration = 0x290B;

        [BluetoothUti("org.bluetooth.descriptor.es_measurement")]
        public static readonly BluetoothUuid EnvironmentalSensingMeasurement = 0x290C;

        [BluetoothUti("org.bluetooth.descriptor.es_trigger_setting")]
        public static readonly BluetoothUuid EnvironmentalSensingTriggerSetting = 0x290D;

        [BluetoothUti("org.bluetooth.descriptor.external_report_reference")]
        public static readonly BluetoothUuid EnvironmentalSensingReportReference = 0x2907;

        [BluetoothUti("org.bluetooth.descriptor.number_of_digitals")]
        public static readonly BluetoothUuid NumberOfDigitals = 0x2909;

        [BluetoothUti("org.bluetooth.descriptor.report_reference")]
        public static readonly BluetoothUuid ReportReference = 0x2908;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Server Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.descriptor.gatt.server_characteristic_configuration")]
        public static readonly BluetoothUuid ServerCharacteristicConfiguration = 0x2903;

        [BluetoothUti("org.bluetooth.descriptor.time_trigger_setting")]
        public static readonly BluetoothUuid TimeTriggerSetting = 0x290E;

        [BluetoothUti("org.bluetooth.descriptor.valid_range")]
        public static readonly BluetoothUuid ValidRange = 0x2906;

        [BluetoothUti("org.bluetooth.descriptor.value_trigger_setting")]
        public static readonly BluetoothUuid ValueTriggerSetting = 0x290A;
    }
}
