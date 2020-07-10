//-----------------------------------------------------------------------
// <copyright file="GattCharacteristicUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/characteristics

using System;
using System.Reflection;

namespace InTheHand.Bluetooth.GenericAttributeProfile
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
        public static Guid FromBluetoothUti(string bluetoothUti)
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
                    return (Guid)field.GetValue(null);
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Lower limit of the heart rate where the user enhances his endurance while exercising.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.aerobic_heart_rate_lower_limit")]
        public static readonly Guid AerobicHeartRateLowerLimit = new Guid(0x00002A7E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.aerobic_heart_rate_upper_limit")]
        public static readonly Guid AerobicHeartRateUpperLimit = new Guid(0x00002A84, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.aerobic_threshold")]
        public static readonly Guid AerobicThreshold = new Guid(0x00002A7F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.age")]
        public static readonly Guid Age = new Guid(0x00002A80, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.aggregate")]
        public static readonly Guid Aggregate = new Guid(0x00002A5A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Category ID characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_category_id")]
        public static readonly Guid AlertCategoryId = new Guid(0x00002A43, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-Defined Alert Category ID Bit Mask characteristic UUID
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_category_id_bit_mask")]
        public static readonly Guid AlertCategoryIdBitMask = new Guid(0x00002A42, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Level characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_level")]
        public static readonly Guid AlertLevel = new Guid(0x00002A06, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Notification Control Point characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_notification_control_point")]
        public static readonly Guid AlertNotificationControlPoint = new Guid(0x00002A44, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Status characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.alert_status")]
        public static readonly Guid AlertStatus = new Guid(0x00002A3F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.altitude")]
        public static readonly Guid Altitude = new Guid(0x00002AB3, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_heart_rate_lower_limit")]
        public static readonly Guid AnaerobicHeartRateLowerLimit = new Guid(0x00002A81, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_heart_rate_upper_limit")]
        public static readonly Guid AnaerobicHeartRateUpperLimit = new Guid(0x00002A82, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.anaerobic_threshold")]
        public static readonly Guid AnaerobicThreshold = new Guid(0x00002A83, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.analog")]
        public static readonly Guid Analog = new Guid(0x00002A58, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        [BluetoothUti("org.bluetooth.characteristic.analog_output")]
        public static readonly Guid AnalogOutput = new Guid(0x00002A59, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.apparent_wind_direction")]
        public static readonly Guid ApparentWindDirection = new Guid(0x00002A73, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.apparent_wind_speed")]
        public static readonly Guid ApparentWindSpeed = new Guid(0x00002A72, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.gap.appearance")]
        public static readonly Guid Appearance = new Guid(0x00002A01, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.barometric_pressure_trend")]
        public static readonly Guid BarometricPressureTrend = new Guid(0x00002AA3, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_level")]
        public static readonly Guid BatteryLevel = new Guid(0x00002A19, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level State characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_level_state")]
        public static readonly Guid BatteryLevelState = new Guid(0x00002A1B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level State characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.battery_power_state")]
        public static readonly Guid BatteryPowerState = new Guid(0x00002A1A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);


        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.blood_pressure_feature")]
        public static readonly Guid BloodPressureFeature = new Guid(0x00002A49, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Measurement Characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.blood_pressure_measurement")]
        public static readonly Guid BloodPressureMeasurement = new Guid(0x00002A35, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.body_composition_feature")]
        public static readonly Guid BodyCompositionFeature = new Guid(0x00002A9B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.body_composition_measurement")]
        public static readonly Guid BodyCompositionMeasurement = new Guid(0x00002A9C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Body Sensor Location characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.body_sensor_location")]
        public static readonly Guid BodySensorLocation = new Guid(0x00002A38, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.bond_management_control_point")]
        public static readonly Guid BondManagementControlPoint = new Guid(0x00002AA4, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.bond_management_feature")]
        public static readonly Guid BondManagementFeatures = new Guid(0x00002AA5, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Keyboard Input Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_keyboard_input_report")]
        public static readonly Guid BootKeyboardInputReport = new Guid(0x00002A22, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Keyboard Output Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_keyboard_output_report")]
        public static readonly Guid BootKeyboardOutputReport = new Guid(0x00002A32, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Mouse Input Report characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.boot_mouse_input_report")]
        public static readonly Guid BootMouseInputReport = new Guid(0x00002A33, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.gap.central_address_resolution")]
        public static readonly Guid CentralAddressResolution = new Guid(0x00002AA6, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_feature")]
        public static readonly Guid CGMFeature = new Guid(0x00002AA8, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_measurement")]
        public static readonly Guid CGMMeasurement = new Guid(0x00002AA7, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_session_run_time")]
        public static readonly Guid CGMSessionRunTime = new Guid(0x00002AAB, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_session_start_time")]
        public static readonly Guid CGMSessionStartTime = new Guid(0x00002AAA, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_specific_ops_control_point")]
        public static readonly Guid CGMSpecificOpsControlPoint = new Guid(0x00002AAC, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cgm_status")]
        public static readonly Guid CGMStatus = new Guid(0x00002AA9, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.cross_trainer_data")]
        public static readonly Guid CrossTrainerData = new Guid(0x00002ACE, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CSC Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.csc_feature")]
        public static readonly Guid CSCFeature = new Guid(0x00002A5C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CSC Measurement characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.csc_measurement")]
        public static readonly Guid CSCMeasurement = new Guid(0x00002A5B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Current Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.current_time")]
        public static readonly Guid CurrentTime = new Guid(0x00002A2B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Control Point characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_control_point")]
        public static readonly Guid CyclingPowerControlPoint = new Guid(0x00002A66, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Feature characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_feature")]
        public static readonly Guid CyclingPowerFeature = new Guid(0x00002A65, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Measurement characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_measurement")]
        public static readonly Guid CyclingPowerMeasurement = new Guid(0x00002A63, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Vector characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.cycling_power_vector")]
        public static readonly Guid CyclingPowerVector = new Guid(0x00002A64, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.database_change_increment")]
        public static readonly Guid DatabaseChangeIncrement = new Guid(0x00002A99, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Date of Birth characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.date_of_birth")]
        public static readonly Guid DateOfBirth = new Guid(0x00002A85, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.date_of_threshold_assessment")]
        public static readonly Guid DateOfThresholdAssessment = new Guid(0x00002A86, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Date Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.date_time")]
        public static readonly Guid DateTime = new Guid(0x00002A08, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.date_utc")]
        public static readonly Guid DateUTC = new Guid(0x00002AED, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Day Date Time characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.day_date_time")]
        public static readonly Guid DayDateTime = new Guid(0x00002A0A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Day Of Week characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.day_of_week")]
        public static readonly Guid DayOfWeek = new Guid(0x00002A09, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.descriptor_value_changed")]
        public static readonly Guid DescriptorValueChanged = new Guid(0x00002A7D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.gap.device_name")]
        public static readonly Guid DeviceName = new Guid(0x00002A00, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.dew_point")]
        public static readonly Guid DewPoint = new Guid(0x00002A7B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.digital")]
        public static readonly Guid Digital = new Guid(0x00002A56, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.digital_output")]
        public static readonly Guid DigitalOutput = new Guid(0x00002A57, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined DST Offset characteristic UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.characteristic.dst_offset")]
        public static readonly Guid DSTOffset = new Guid(0x00002A0D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.elevation")]
        public static readonly Guid Elevation = new Guid(0x00002A6C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.email_address")]
        public static readonly Guid EmailAddress = new Guid(0x00002A87, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.exact_time_100")]
        public static readonly Guid ExactTime100 = new Guid(0x00002A0B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.characteristic.exact_time_256")]
        public static readonly Guid ExactTime256 = new Guid(0x00002A0C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        [BluetoothUti("org.bluetooth.characteristic.fat_burn_heart_rate_lower_limit")]
        public static readonly Guid FatBurnHeartRateLowerLimit = new Guid(0x00002A88, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        [BluetoothUti("org.bluetooth.characteristic.fat_burn_heart_rate_upper_limit")]
        public static readonly Guid FatBurnHeartRateUpperLimit = new Guid(0x00002A89, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        [BluetoothUti("org.bluetooth.characteristic.firmware_revision_string")]
        public static readonly Guid FirmwareRevisionString = new Guid(0x00002A26, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        // TODO: Complete list of Characteristics
        //...
    }
}