//-----------------------------------------------------------------------
// <copyright file="GattServiceUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Based on values from https://www.bluetooth.com/specifications/gatt/services

using System;
using System.Reflection;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Provides service UUIDs for common GATT services.
    /// </summary>
    /// <remarks>To view a list of all Bluetooth SIG-defined service UUIDs, see <a href="https://www.bluetooth.com/specifications/gatt/services">Bluetooth SIG-defined Service UUIDs</a>.</remarks>
    public static class GattServiceUuids
    {
        /// <summary>
        /// Returns the Uuid for a service given the Uniform Type Identifier.
        /// </summary>
        /// <param name="bluetoothUti">Uniform Type Identifier of the service e.g. org.bluetooth.service.generic_access</param>
        /// <returns>The service Uuid on success else Guid.Empty.</returns>
        public static BluetoothUuid FromBluetoothUti(string bluetoothUti)
        {
            string requestedUti = bluetoothUti.ToLower();
            if (!requestedUti.StartsWith("org.bluetooth.service"))
            {
                requestedUti = "org.bluetooth.service." + bluetoothUti.ToLower();
            }
            
            var fields = typeof(GattServiceUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
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
        /// Gets the Bluetooth SIG-defined UUID for the Generic Access Service.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.generic_access")]
        public static readonly BluetoothUuid GenericAccess = 0x1800;

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Notification Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.alert_notification")]
        public static readonly BluetoothUuid AlertNotification = new Guid(0x00001811, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// The Automation IO service is used to expose the analog inputs/outputs and digital input/outputs of a generic IO module (IOM).
        /// </summary>
        [BluetoothUti("org.bluetooth.service.automation_io")]
        public static readonly BluetoothUuid AutomationIO = new Guid(0x00001815, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// The Battery Service exposes the state of a battery within a device. 
        /// </summary>
        [BluetoothUti("org.bluetooth.service.battery_service")]
        public static readonly BluetoothUuid Battery = new Guid(0x0000180F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BluetoothUuid BinarySensor = 0x183B;

        /// <summary>
        /// The Blood Pressure Service exposes blood pressure and other data related to a blood pressure monitor.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.blood_pressure")]
        public static readonly BluetoothUuid BloodPressure = new Guid(0x00001810, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Body Composition service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.body_composition")]
        public static readonly BluetoothUuid BodyComposition = new Guid(0x0000181B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Bond Management service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.bond_management")]
        public static readonly BluetoothUuid BondManagement = new Guid(0x0000181E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Continuous Glucose Monitoring service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.continuous_glucose_monitoring")]
        public static readonly BluetoothUuid ContinuousGlucoseMonitoring = new Guid(0x0000181F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Current Time service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.current_time")]
        public static readonly BluetoothUuid CurrentTime = new Guid(0x00001805, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.cycling_power")]
        public static readonly BluetoothUuid CyclingPower = new Guid(0x00001818, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Speed and Cadence Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.cycling_speed_and_cadence")]
        public static readonly BluetoothUuid CyclingSpeedAndCadence = new Guid(0x00001816, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Device Information service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.device_information")]
        public static readonly BluetoothUuid DeviceInformation = new Guid(0x0000180A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Environmental Sensing service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.environmental_sensing")]
        public static readonly BluetoothUuid EnvironmentalSensing = new Guid(0x0000181A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Fitness Machine service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.fitness_machine")]
        public static readonly BluetoothUuid FitnessMachine = new Guid(0x00001826, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Attribute Service.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.generic_attribute")]
        public static readonly BluetoothUuid GenericAttribute = new Guid(0x00001801, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.glucose")]
        public static readonly BluetoothUuid Glucose = new Guid(0x00001808, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Health Thermometer Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.health_thermometer")]
        public static readonly BluetoothUuid HealthThermometer = new Guid(0x00001809, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.heart_rate")]
        public static readonly BluetoothUuid HeartRate = new Guid(0x0000180D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined HTTP Proxy Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.http_proxy")]
        public static readonly BluetoothUuid HttpProxy = new Guid(0x00001823, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Human Interface Device service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.human_interface_device")]
        public static readonly BluetoothUuid HumanInterfaceDevice = new Guid(0x00001812, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Immediate Alert service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.immediate_alert")]
        public static readonly BluetoothUuid ImmediateAlert = new Guid(0x00001802, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Indoor Positioning service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.indoor_positioning")]
        public static readonly BluetoothUuid IndoorPositioning = new Guid(0x00001821, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Insulin Delivery service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.insulin_delivery")]
        public static readonly BluetoothUuid InsulinDelivery = new Guid(0x0000183A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Internet Protocol Support service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.internet_protocol_support")]
        public static readonly BluetoothUuid InternetProtocolSupport = new Guid(0x00001820, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Link Loss service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.link_loss")]
        public static readonly BluetoothUuid LinkLoss = new Guid(0x00001803, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Location and Navigation service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.location_and_navigation")]
        public static readonly BluetoothUuid LocationAndNavigation = new Guid(0x00001819, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Mesh Provisioning service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.mesh_provisioning")]
        public static readonly BluetoothUuid MeshProvisioning = new Guid(0x00001827, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Mesh Proxy service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.mesh_proxy")]
        public static readonly BluetoothUuid MeshProxy = new Guid(0x00001828, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Next DST Change service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.next_dst_change")]
        public static readonly BluetoothUuid NextDstChange = new Guid(0x00001807, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Object Transfer service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.object_transfer")]
        public static readonly BluetoothUuid ObjectTransfer = new Guid(0x00001825, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Phone Alert Status service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.phone_alert_status")]
        public static readonly BluetoothUuid PhoneAlertStatus = new Guid(0x0000180E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Pulse Oximeter service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.pulse_oximeter")]
        public static readonly BluetoothUuid PulseOximeter = new Guid(0x00001822, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Reconnection Configuration service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.reconnection_configuration")]
        public static readonly BluetoothUuid ReconnectionConfiguration = new Guid(0x00001829, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Reference Time Update service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.reference_time_update")]
        public static readonly BluetoothUuid ReferenceTimeUpdate = new Guid(0x00001806, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Running Speed and Cadence Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.running_speed_and_cadence")]
        public static readonly BluetoothUuid RunningSpeedAndCadence = new Guid(0x00001814, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Scan Parameters service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.scan_parameters")]
        public static readonly BluetoothUuid ScanParameters = new Guid(0x00001813, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Transport Discovery service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.transport_discovery")]
        public static readonly BluetoothUuid TransportDiscovery = new Guid(0x00001824, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        
        /// <summary>
        /// Gets the Bluetooth SIG-defined Tx Power service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.tx_power")]
        public static readonly BluetoothUuid TxPower = new Guid(0x00001804, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined User Data service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.user_data")]
        public static readonly BluetoothUuid UserData = new Guid(0x0000181C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Weight Scale service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.weight_scale")]
        public static readonly BluetoothUuid WeightScale = new Guid(0x0000181D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }
}
