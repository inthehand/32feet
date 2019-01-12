//-----------------------------------------------------------------------
// <copyright file="GattServiceUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-18 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
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
        public static Guid FromBluetoothUti(string bluetoothUti)
        {
            string requestedUti = bluetoothUti.ToLower();
            if(!requestedUti.StartsWith("org.bluetooth.service"))
            {
                requestedUti = "org.bluetooth.service." + bluetoothUti.ToLower();
            }

#if !NETSTANDARD1_4
            var fields = typeof(GattServiceUuids).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach(var field in fields)
            {
                var attr = field.GetCustomAttribute(typeof(BluetoothUtiAttribute));
                if(attr != null && ((BluetoothUtiAttribute)attr).Uti == requestedUti)
                {
                    return (Guid)field.GetValue(null);
                }
            }
#endif

            return Guid.Empty;
        }

        /// <summary>
        /// Gets the Bluetooth SIG-defined AlertNotification Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.alert_notification")]
        public static readonly Guid AlertNotification = new Guid(0x00001811, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// The Automation IO service is used to expose the analog inputs/outputs and digital input/outputs of a generic IO module (IOM).
        /// </summary>
        [BluetoothUti("org.bluetooth.service.automation_io")]
        public static readonly Guid AutomationIO = new Guid(0x00001815, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// The Battery Service exposes the state of a battery within a device. 
        /// </summary>
        [BluetoothUti("org.bluetooth.service.battery_service")]
        public static readonly Guid Battery = new Guid(0x0000180F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// The Blood Pressure Service exposes blood pressure and other data related to a blood pressure monitor.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.blood_pressure")]
        public static readonly Guid BloodPressure = new Guid(0x00001810, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.body_composition")]
        public static readonly Guid BodyComposition = new Guid(0x0000181B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.bond_management")]
        public static readonly Guid BondManagement = new Guid(0x0000181E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.continuous_glucose_monitoring")]
        public static readonly Guid ContinuousGlucoseMonitoring = new Guid(0x0000181F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CurrentTime service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.current_time")]
        public static readonly Guid CurrentTime = new Guid(0x00001805, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CyclingPower service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.cycling_power")]
        public static readonly Guid CyclingPower = new Guid(0x00001818, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Speed And Cadence Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.cycling_speed_and_cadence")]
        public static readonly Guid CyclingSpeedAndCadence = new Guid(0x00001816, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined DeviceInformation service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.device_information")]
        public static readonly Guid DeviceInformation = new Guid(0x0000180A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined EnvironmentalSensing service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.environmental_sensing")]
        public static readonly Guid EnvironmentalSensing = new Guid(0x0000181A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.fitness_machine")]
        public static readonly Guid FitnessMachine = new Guid(0x00001826, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Access Service.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.generic_access")]
        public static readonly Guid GenericAccess = new Guid(0x00001800, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Attribute Service.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.generic_attribute")]
        public static readonly Guid GenericAttribute = new Guid(0x00001801, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.glucose")]
        public static readonly Guid Glucose = new Guid(0x00001808, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Health Thermometer Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.health_thermometer")]
        public static readonly Guid HealthThermometer = new Guid(0x00001809, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.heart_rate")]
        public static readonly Guid HeartRate = new Guid(0x0000180D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.http_proxy")]
        public static readonly Guid HttpProxy = new Guid(0x00001823, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined HumanInterfaceDevice service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.human_interface_device")]
        public static readonly Guid HumanInterfaceDevice = new Guid(0x00001812, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ImmediateAlert service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.immediate_alert")]
        public static readonly Guid ImmediateAlert = new Guid(0x00001802, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.indoor_positioning")]
        public static readonly Guid IndoorPositioning = new Guid(0x00001821, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.internet_protocol_support")]
        public static readonly Guid InternetProtocolSupport = new Guid(0x00001820, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined LinkLoss service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.link_loss")]
        public static readonly Guid LinkLoss = new Guid(0x00001803, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined LocationAndNavigation service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.location_and_navigation")]
        public static readonly Guid LocationAndNavigation = new Guid(0x00001819, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.mesh_provisioning")]
        public static readonly Guid MeshProvisioning = new Guid(0x00001827, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.mesh_proxy")]
        public static readonly Guid MeshProxy = new Guid(0x00001828, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined NextDstChange service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.next_dst_change")]
        public static readonly Guid NextDstChange = new Guid(0x00001807, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.object_transfer")]
        public static readonly Guid ObjectTransfer = new Guid(0x00001825, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined PhoneAlertStatus service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.phone_alert_status")]
        public static readonly Guid PhoneAlertStatus = new Guid(0x0000180E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.pulse_oximeter")]
        public static readonly Guid PulseOximeter = new Guid(0x00001822, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.reconnection_configuration")]
        public static readonly Guid ReconnectionConfiguration = new Guid(0x00001829, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ReferenceTimeUpdate service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.reference_time_update")]
        public static readonly Guid ReferenceTimeUpdate = new Guid(0x00001806, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Running Speed And Cadence Service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.running_speed_and_cadence")]
        public static readonly Guid RunningSpeedAndCadence = new Guid(0x00001814, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ScanParameters service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.scan_parameters")]
        public static readonly Guid ScanParameters = new Guid(0x00001813, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.transport_discovery")]
        public static readonly Guid TransportDiscovery = new Guid(0x00001824, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        /// <summary>
        /// Gets the Bluetooth SIG-defined TxPower service UUID.
        /// </summary>
        [BluetoothUti("org.bluetooth.service.tx_power")]
        public static readonly Guid TxPower = new Guid(0x00001804, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.user_data")]
        public static readonly Guid UserData = new Guid(0x0000181C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        [BluetoothUti("org.bluetooth.service.weight_scale")]
        public static readonly Guid WeightScale = new Guid(0x0000181D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }
}