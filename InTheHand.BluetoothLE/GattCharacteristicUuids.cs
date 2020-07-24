//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/characteristics

using System;
using System.Reflection;

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Provides characteristic UUIDs for common GATT characteristics.
    /// </summary>
    /// <remarks>To view a list of all Bluetooth SIG-defined characteristic UUIDs, see <a href="https://www.bluetooth.com/specifications/gatt/characteristics">Bluetooth SIG-defined Characteristic UUIDs</a>.</remarks>
    public static class GattCharacteristicUuids
    {
        /// <summary>
        /// Returns the Uuid for a characteristic given the Uniform Type Identifier.
        /// </summary>
        /// <param name="bluetoothUti">Uniform Type Identifier of the characteristic e.g. org.bluetooth.characteristic.aerobic_heart_rate_lower_limit</param>
        /// <returns>The characteristic Uuid on success else Guid.Empty.</returns>
        public static BluetoothUuid FromBluetoothUti(string bluetoothUti)
        {
            string requestedUti = bluetoothUti.ToLower();
            if (!requestedUti.StartsWith("org.bluetooth.characteristic"))
            {
                requestedUti = "org.bluetooth.characteristic" + bluetoothUti.ToLower();
            }
            
            var fields = typeof(GattCharacteristicUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
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
        /// Lower limit of the heart rate where the user enhances his endurance while exercising.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.aerobic_heart_rate_lower_limit")]
        public static readonly BluetoothUuid AerobicHeartRateLowerLimit = 0x2A7E;

        [BluetoothUti("org.bluetooth.characteristic.aerobic_heart_rate_upper_limit")]
        public static readonly BluetoothUuid AerobicHeartRateUpperLimit = 0x2A84;

        [BluetoothUti("org.bluetooth.characteristic.aerobic_threshold")]
        public static readonly BluetoothUuid AerobicThreshold = 0x2A7F;

        [BluetoothUti("org.bluetooth.characteristic.age")]
        public static readonly BluetoothUuid Age = 0x2A80;

        [BluetoothUti("org.bluetooth.characteristic.aggregate")]
        public static readonly BluetoothUuid Aggregate = 0x2A5A;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Category ID characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_category_id")]
        public static readonly BluetoothUuid AlertCategoryId = 0x2A43;

        /// <summary>
        /// Gets the Bluetooth SIG-Defined Alert Category ID Bit Mask characteristic UUID
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_category_id_bit_mask")]
        public static readonly BluetoothUuid AlertCategoryIdBitMask = 0x2A42;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Level characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_level")]
        public static readonly BluetoothUuid AlertLevel = 0x2A06;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Notification Control Point characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_notification_control_point")]
        public static readonly BluetoothUuid AlertNotificationControlPoint = 0x2A44;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Status characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_status")]
        public static readonly BluetoothUuid AlertStatus = 0x2A3F;

        [BluetoothUti("org.bluetooth.characteristic.altitude")]
        public static readonly BluetoothUuid Altitude = 0x2AB3;

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_heart_rate_lower_limit")]
        public static readonly BluetoothUuid AnaerobicHeartRateLowerLimit = 0x2A81;

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_heart_rate_upper_limit")]
        public static readonly BluetoothUuid AnaerobicHeartRateUpperLimit = 0x2A82;

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_threshold")]
        public static readonly BluetoothUuid AnaerobicThreshold = 0x2A83;

        [BluetoothUti("org.bluetooth.characteristic.analog")]
        public static readonly BluetoothUuid Analog = 0x2A58;
        
        [BluetoothUti("org.bluetooth.characteristic.analog_output")]
        public static readonly BluetoothUuid AnalogOutput = 0x2A59;

        [BluetoothUti("org.bluetooth.characteristic.apparent_wind_direction")]
        public static readonly BluetoothUuid ApparentWindDirection = 0x2A73;

        [BluetoothUti("org.bluetooth.characteristic.apparent_wind_speed")]
        public static readonly BluetoothUuid ApparentWindSpeed = 0x2A72;

        [BluetoothUti("org.bluetooth.characteristic.gap.appearance")]
        public static readonly BluetoothUuid Appearance = 0x2A01;

        [BluetoothUti("org.bluetooth.characteristic.barometric_pressure_trend")]
        public static readonly BluetoothUuid BarometricPressureTrend = 0x2AA3;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_level")]
        public static readonly BluetoothUuid BatteryLevel = 0x2A19;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level State characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_level_state")]
        public static readonly BluetoothUuid BatteryLevelState = 0x2A1B;
        
        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level State characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_power_state")]
        public static readonly BluetoothUuid BatteryPowerState = 0x2A1A;


        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.blood_pressure_feature")]
        public static readonly BluetoothUuid BloodPressureFeature = 0x2A49;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Measurement Characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.blood_pressure_measurement")]
        public static readonly BluetoothUuid BloodPressureMeasurement = 0x2A35;

        [BluetoothUti("org.bluetooth.characteristic.body_composition_feature")]
        public static readonly BluetoothUuid BodyCompositionFeature = 0x2A9B;

        [BluetoothUti("org.bluetooth.characteristic.body_composition_measurement")]
        public static readonly BluetoothUuid BodyCompositionMeasurement = 0x2A9C;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Body Sensor Location characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.body_sensor_location")]
        public static readonly BluetoothUuid BodySensorLocation = 0x2A38;

        [BluetoothUti("org.bluetooth.characteristic.bond_management_control_point")]
        public static readonly BluetoothUuid BondManagementControlPoint = 0x2AA4;

        [BluetoothUti("org.bluetooth.characteristic.bond_management_feature")]
        public static readonly BluetoothUuid BondManagementFeatures = 0x2AA5;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Keyboard Input Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_keyboard_input_report")]
        public static readonly BluetoothUuid BootKeyboardInputReport = 0x2A22;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Keyboard Output Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_keyboard_output_report")]
        public static readonly BluetoothUuid BootKeyboardOutputReport = 0x2A32;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Mouse Input Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_mouse_input_report")]
        public static readonly BluetoothUuid BootMouseInputReport = 0x2A33;

        public static readonly BluetoothUuid BSSControlPoint = 0x2B2B;
        
        public static readonly BluetoothUuid BSSResponse = 0x2B2C;

         [BluetoothUti("org.bluetooth.characteristic.cgm_feature")]
        public static readonly BluetoothUuid CGMFeature = 0x2AA8;

        [BluetoothUti("org.bluetooth.characteristic.cgm_measurement")]
        public static readonly BluetoothUuid CGMMeasurement = 0x2AA7;

        [BluetoothUti("org.bluetooth.characteristic.cgm_session_run_time")]
        public static readonly BluetoothUuid CGMSessionRunTime = 0x2AAB;

        [BluetoothUti("org.bluetooth.characteristic.cgm_session_start_time")]
        public static readonly BluetoothUuid CGMSessionStartTime = 0x2AAA;

        [BluetoothUti("org.bluetooth.characteristic.cgm_specific_ops_control_point")]
        public static readonly BluetoothUuid CGMSpecificOpsControlPoint = 0x2AAC;

        [BluetoothUti("org.bluetooth.characteristic.cgm_status")]
        public static readonly BluetoothUuid CGMStatus = 0x2AA9;

        public static readonly BluetoothUuid ClientSupportedFeatures = 0x2B29;

        [BluetoothUti("org.bluetooth.characteristic.cross_trainer_data")]
        public static readonly BluetoothUuid CrossTrainerData = 0x2ACE;

        /// <summary>
        /// Gets the Bluetooth SIG-defined CSC Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.csc_feature")]
        public static readonly BluetoothUuid CSCFeature = 0x2A5C;

        /// <summary>
        /// Gets the Bluetooth SIG-defined CSC Measurement characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.csc_measurement")]
        public static readonly BluetoothUuid CSCMeasurement = 0x2A5B;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Current Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.current_time")]
        public static readonly BluetoothUuid CurrentTime = 0x2A2B;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Control Point characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_control_point")]
        public static readonly BluetoothUuid CyclingPowerControlPoint = 0x2A66;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_feature")]
        public static readonly BluetoothUuid CyclingPowerFeature = 0x2A65;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Measurement characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_measurement")]
        public static readonly BluetoothUuid CyclingPowerMeasurement = 0x2A63;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Vector characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_vector")]
        public static readonly BluetoothUuid CyclingPowerVector = 0x2A64;

        [BluetoothUti("org.bluetooth.characteristic.database_change_increment")]
        public static readonly BluetoothUuid DatabaseChangeIncrement = 0x2A99;
        
        public static readonly BluetoothUuid DatabaseHash = 0x2B2A;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Date of Birth characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.date_of_birth")]
        public static readonly BluetoothUuid DateOfBirth = 0x2A85;

        [BluetoothUti("org.bluetooth.characteristic.date_of_threshold_assessment")]
        public static readonly BluetoothUuid DateOfThresholdAssessment = 0x2A86;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Date Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.date_time")]
        public static readonly BluetoothUuid DateTime = 0x2A08;

        [BluetoothUti("org.bluetooth.characteristic.date_utc")]
        public static readonly BluetoothUuid DateUTC = 0x2AED;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Day Date Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.day_date_time")]
        public static readonly BluetoothUuid DayDateTime = 0x2A0A;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Day Of Week characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.day_of_week")]
        public static readonly BluetoothUuid DayOfWeek = 0x2A09;

        [BluetoothUti("org.bluetooth.characteristic.descriptor_value_changed")]
        public static readonly BluetoothUuid DescriptorValueChanged = 0x2A7D;

        [BluetoothUti("org.bluetooth.characteristic.dew_point")]
        public static readonly BluetoothUuid DewPoint = 0x2A7B;

        [BluetoothUti("org.bluetooth.characteristic.digital")]
        public static readonly BluetoothUuid Digital = 0x2A56;

        [BluetoothUti("org.bluetooth.characteristic.digital_output")]
        public static readonly BluetoothUuid DigitalOutput = 0x2A57;

        /// <summary>
        /// Gets the Bluetooth SIG-defined DST Offset characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.dst_offset")]
        public static readonly BluetoothUuid DSTOffset = 0x2A0D;

        [BluetoothUti("org.bluetooth.characteristic.elevation")]
        public static readonly BluetoothUuid Elevation = 0x2A6C;

        [BluetoothUti("org.bluetooth.characteristic.email_address")]
        public static readonly BluetoothUuid EmailAddress = 0x2A87;

        public static readonly BluetoothUuid EmergencyId = 0x2B2D;
        public static readonly BluetoothUuid EmergencyText = 0x2B2E;

        [BluetoothUti("org.bluetooth.characteristic.exact_time_100")]
        public static readonly BluetoothUuid ExactTime100 = 0x2A0B;
        [BluetoothUti("org.bluetooth.characteristic.exact_time_256")]
        public static readonly BluetoothUuid ExactTime256 = 0x2A0C;
        
        [BluetoothUti("org.bluetooth.characteristic.fat_burn_heart_rate_lower_limit")]
        public static readonly BluetoothUuid FatBurnHeartRateLowerLimit = 0x2A88;
        [BluetoothUti("org.bluetooth.characteristic.fat_burn_heart_rate_upper_limit")]
        public static readonly BluetoothUuid FatBurnHeartRateUpperLimit = 0x2A89;
        
        [BluetoothUti("org.bluetooth.characteristic.firmware_revision_string")]
        public static readonly BluetoothUuid FirmwareRevisionString = 0x2A26;

        [BluetoothUti("org.bluetooth.characteristic.first_name")]
        public static readonly BluetoothUuid FirstName = 0x2A8A;

        [BluetoothUti("org.bluetooth.characteristic.fitness_machine_control_point")]
        public static readonly BluetoothUuid FitnessMachineControlPoint = 0x2AD9;       
        [BluetoothUti("org.bluetooth.characteristic.fitness_machine_feature")]
        public static readonly BluetoothUuid FitnessMachineFeature = 0x2ACC;
        [BluetoothUti("org.bluetooth.characteristic.fitness_machine_status")]
        public static readonly BluetoothUuid FitnessMachineStatus = 0x2ADA;

        [BluetoothUti("org.bluetooth.characteristic.five_zone_heart_rate_limits")]
        public static readonly BluetoothUuid FiveZoneHeartRateLimits = 0x2A8B;

        [BluetoothUti("org.bluetooth.characteristic.floor_number")]
        public static readonly BluetoothUuid FloorNumber = 0x2AB2;

        [BluetoothUti("org.bluetooth.characteristic.gap.central_address_resolution")]
        public static readonly BluetoothUuid CentralAddressResolution = 0x2AA6;
        [BluetoothUti("org.bluetooth.characteristic.gap.device_name")]
        public static readonly BluetoothUuid DeviceName = 0x2A00;
        [BluetoothUti("org.bluetooth.characteristic.gap.peripheral_preferred_connection_parameters")]
        public static readonly BluetoothUuid PeripheralPreferredConnectionParameters = 0x2A04;
        [BluetoothUti("org.bluetooth.characteristic.gap.peripheral_privacy_flag")]
        public static readonly BluetoothUuid PeripheralPrivacyFlag = 0x2A02;
        [BluetoothUti("org.bluetooth.characteristic.gap.reconnection_address")]
        public static readonly BluetoothUuid ReconnectionAddress = 0x2A03;
        [BluetoothUti("org.bluetooth.characteristic.gap.service_changed")]
        public static readonly BluetoothUuid ServiceChanged = 0x2A05;

        [BluetoothUti("org.bluetooth.characteristic.gender")]
        public static readonly BluetoothUuid Gender = 0x2A8C;

        [BluetoothUti("org.bluetooth.characteristic.glucose_feature")]
        public static readonly BluetoothUuid GlucoseFeature = 0x2A51;
        [BluetoothUti("org.bluetooth.characteristic.glucose_measurement")]
        public static readonly BluetoothUuid GlucoseMeasurement = 0x2A18;
        [BluetoothUti("org.bluetooth.characteristic.glucose_measurement_context")]
        public static readonly BluetoothUuid GlucoseMeasurementContext = 0x2A34;
        
        [BluetoothUti("org.bluetooth.characteristic.gust_factor")]
        public static readonly BluetoothUuid GustFactor = 0x2A74;
        
        [BluetoothUti("org.bluetooth.characteristic.hardware_revision_string")]
        public static readonly BluetoothUuid HardwareRevisionString = 0x2A27;

        [BluetoothUti("org.bluetooth.characteristic.heart_rate_control_point")]
        public static readonly BluetoothUuid HeartRateControlPoint = 0x2A39;
        [BluetoothUti("org.bluetooth.characteristic.heart_rate_max")]
        public static readonly BluetoothUuid HeartRateMax = 0x2A8D;      
        [BluetoothUti("org.bluetooth.characteristic.heart_rate_measurement")]
        public static readonly BluetoothUuid HeartRateMeasurement = 0x2A37;

        [BluetoothUti("org.bluetooth.characteristic.heat_index")]
        public static readonly BluetoothUuid HeatIndex = 0x2A7A;
        
        [BluetoothUti("org.bluetooth.characteristic.height")]
        public static readonly BluetoothUuid Height = 0x2A8E;
        

        [BluetoothUti("org.bluetooth.characteristic.hid_control_point")]
        public static readonly BluetoothUuid HidControlPoint = 0x2A4C;
        [BluetoothUti("org.bluetooth.characteristic.hid_information")]
        public static readonly BluetoothUuid HidInformation = 0x2A4A;
        [BluetoothUti("org.bluetooth.characteristic.hip_circumference")]
        public static readonly BluetoothUuid HipCircumference = 0x2A8F;
        
        [BluetoothUti("org.bluetooth.characteristic.http_control_point")]
        public static readonly BluetoothUuid HttpControlPoint = 0x2ABA;
        [BluetoothUti("org.bluetooth.characteristic.http_entity_body")]
        public static readonly BluetoothUuid HttpEntityBody = 0x2AB9;
        [BluetoothUti("org.bluetooth.characteristic.http_headers")]
        public static readonly BluetoothUuid HttpHeaders = 0x2AB7;
        [BluetoothUti("org.bluetooth.characteristic.http_status_code")]
        public static readonly BluetoothUuid HttpStatusCode = 0x2AB8;
        [BluetoothUti("org.bluetooth.characteristic.https_security")]
        public static readonly BluetoothUuid HttpsSecurity = 0x2ABB;

        [BluetoothUti("org.bluetooth.characteristic.humidity")]
        public static readonly BluetoothUuid Humidity = 0x2A6F;

        [BluetoothUti("org.bluetooth.characteristic.idd_annunciation_status")]
        public static readonly BluetoothUuid IddAnnunciationStatus = 0x2B22;


        // TODO: Complete list of Characteristics
        //...
    }
}