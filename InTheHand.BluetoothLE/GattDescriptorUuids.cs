//-----------------------------------------------------------------------
// <copyright file="GattDescriptorUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2017-2023 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/descriptors

using System.Reflection;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents an enumeration of the most well known Descriptor UUID values.
    /// </summary>
    /// <remarks>To view a list of all Bluetooth SIG-defined descriptor UUIDs, see <see href="https://bitbucket.org/bluetooth-SIG/public/src/main/assigned_numbers/uuids/descriptors.yaml">Bluetooth SIG-defined Descriptor UUIDs</a>.</remarks>
    [BluetoothUti(Namespace)]
    public static class GattDescriptorUuids
    {
        internal const string Namespace = "org.bluetooth.descriptor";

        /// <summary>
        /// Returns the Uuid for a descriptor given the Uniform Type Identifier.
        /// </summary>
        /// <param name="bluetoothUti">Uniform Type Identifier of the service e.g. org.bluetooth.descriptor.gatt.characteristic_aggregate_format</param>
        /// <returns>The descriptor Uuid on success else an empty <see cref="BluetoothUuid"/>.</returns>
        public static BluetoothUuid FromBluetoothUti(string bluetoothUti)
        {
            var requestedUti = bluetoothUti.ToLower();
            if (requestedUti.StartsWith(Namespace))
            {
                requestedUti = requestedUti.Replace(Namespace + ".", string.Empty);
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

        public static string GetDescriptorName(BluetoothUuid uuid, bool includeNamespace = false)
        {
            var shortid = BluetoothUuid.TryGetShortId(uuid);
            if (shortid.HasValue && (shortid.Value & 0xFF00) == 0x2900)
            {
                var fields = typeof(GattDescriptorUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (var field in fields)
                {
                    if ((BluetoothUuid)field.GetValue(null) == uuid)
                    {
                        var attr = field.GetCustomAttribute(typeof(BluetoothUtiAttribute));
                        if (attr != null)
                        {
                            return (includeNamespace ? Namespace + "." : string.Empty) + ((BluetoothUtiAttribute)attr).Uti;
                        }

                        return string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Extended Properties descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.characteristic_extended_properties")]
        public static readonly BluetoothUuid CharacteristicExtendedProperties = 0x2900;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic User Description descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.characteristic_user_description")]
        public static readonly BluetoothUuid CharacteristicUserDescription = 0x2901;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Client Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.client_characteristic_configuration")]
        public static readonly BluetoothUuid ClientCharacteristicConfiguration = 0x2902;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Server Characteristic Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.server_characteristic_configuration")]
        public static readonly BluetoothUuid ServerCharacteristicConfiguration = 0x2903;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Presentation Format descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.characteristic_presentation_format")]
        public static readonly BluetoothUuid CharacteristicPresentationFormat = 0x2904;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Aggregate Format descriptor UUID.
        /// </summary>
        [BluetoothUti("gatt.characteristic_aggregate_format")]
        public static readonly BluetoothUuid CharacteristicAggregateFormat = 0x2905;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Valid Range descriptor UUID.
        /// </summary>
        [BluetoothUti("valid_range")]
        public static readonly BluetoothUuid ValidRange = 0x2906;

        /// <summary>
        /// Gets the Bluetooth SIG-defined External Report Reference descriptor UUID.
        /// </summary>
        [BluetoothUti("external_report_reference")]
        public static readonly BluetoothUuid ExternalReportReference = 0x2907;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Report Reference descriptor UUID.
        /// </summary>
        [BluetoothUti("report_reference")]
        public static readonly BluetoothUuid ReportReference = 0x2908;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Number of Digitals descriptor UUID.
        /// </summary>
        [BluetoothUti("number_of_digitals")]
        public static readonly BluetoothUuid NumberOfDigitals = 0x2909;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Value Trigger Setting descriptor UUID.
        /// </summary>
        [BluetoothUti("value_trigger_setting")]
        public static readonly BluetoothUuid ValueTriggerSetting = 0x290A;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Environmental Sensing Configuration descriptor UUID.
        /// </summary>
        [BluetoothUti("es_configuration")]
        public static readonly BluetoothUuid EnvironmentalSensingConfiguration = 0x290B;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Environmental Sensing Measurement descriptor UUID.
        /// </summary>
        [BluetoothUti("es_measurement")]
        public static readonly BluetoothUuid EnvironmentalSensingMeasurement = 0x290C;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Environmental Sensing Trigger Setting descriptor UUID.
        /// </summary>
        [BluetoothUti("es_trigger_setting")]
        public static readonly BluetoothUuid EnvironmentalSensingTriggerSetting = 0x290D;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Time Trigger Setting descriptor UUID.
        /// </summary>
        [BluetoothUti("time_trigger_setting")]
        public static readonly BluetoothUuid TimeTriggerSetting = 0x290E;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Complete BR­EDR Transport Block Data descriptor UUID.
        /// </summary>
        [BluetoothUti("complete_bredr_transport_block_data")]
        public static readonly BluetoothUuid CompleteBR­EDRTransportBlockData = 0x290F;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Observation Schedule descriptor UUID.
        /// </summary>
        [BluetoothUti("observation_schedule")]
        public static readonly BluetoothUuid ObservationSchedule = 0x2910;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Valid Range and Accuracy descriptor UUID.
        /// </summary>
        [BluetoothUti("valid_range_accuracy")]
        public static readonly BluetoothUuid ValidRangeAndAccuracy = 0x2911;

    }
}
