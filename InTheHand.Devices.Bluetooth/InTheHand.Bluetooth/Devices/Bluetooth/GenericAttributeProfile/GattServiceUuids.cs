//-----------------------------------------------------------------------
// <copyright file="GattServiceUuids.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Provides service UUIDs for common GATT services.
    /// </summary>
    public static class GattServiceUuids
    {
        /// <summary>
        /// Gets the Bluetooth SIG-defined AlertNotification Service UUID.
        /// </summary>
        public static readonly Guid AlertNotification = new Guid(0x00001811, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid AutomationIO = new Guid(0x00001815, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Service UUID.
        /// </summary>
        public static readonly Guid Battery = new Guid(0x0000180F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Service UUID.
        /// </summary>
        public static readonly Guid BloodPressure = new Guid(0x00001810, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid BodyComposition = new Guid(0x0000181B, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid BondManagement = new Guid(0x0000181E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid ContinuousGlucoseMonitoring = new Guid(0x0000181F, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CurrentTime service UUID.
        /// </summary>
        public static readonly Guid CurrentTime = new Guid(0x00001805, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined CyclingPower service UUID.
        /// </summary>
        public static readonly Guid CyclingPower = new Guid(0x00001818, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Speed And Cadence Service UUID.
        /// </summary>
        public static readonly Guid CyclingSpeedAndCadence = new Guid(0x00001816, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined DeviceInformation service UUID.
        /// </summary>
        public static readonly Guid DeviceInformation = new Guid(0x0000180A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid EnvironmentalSensing = new Guid(0x0000181A, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Access Service.
        /// </summary>
        public static readonly Guid GenericAccess = new Guid(0x00001800, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Attribute Service.
        /// </summary>
        public static readonly Guid GenericAttribute = new Guid(0x00001801, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Service UUID.
        /// </summary>
        public static readonly Guid Glucose = new Guid(0x00001808, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Health Thermometer Service UUID.
        /// </summary>
        public static readonly Guid HealthThermometer = new Guid(0x00001809, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Service UUID.
        /// </summary>
        public static readonly Guid HeartRate = new Guid(0x0000180D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid HttpProxy = new Guid(0x00001823, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined HumanInterfaceDevice service UUID.
        /// </summary>
        public static readonly Guid HumanInterfaceDevice = new Guid(0x00001812, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ImmediateAlert service UUID.
        /// </summary>
        public static readonly Guid ImmediateAlert = new Guid(0x00001802, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid IndoorPositioning = new Guid(0x00001821, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid InternetProtocolSupport = new Guid(0x00001820, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined LinkLoss service UUID.
        /// </summary>
        public static readonly Guid LinkLoss = new Guid(0x00001803, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined LocationAndNavigation service UUID.
        /// </summary>
        public static readonly Guid LocationAndNavigation = new Guid(0x00001819, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined NextDstChange service UUID.
        /// </summary>
        public static readonly Guid NextDstChange = new Guid(0x00001807, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid ObjectTransfer = new Guid(0x00001825, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined PhoneAlertStatus service UUID.
        /// </summary>
        public static readonly Guid PhoneAlertStatus = new Guid(0x0000180E, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid PulseOximeter = new Guid(0x00001822, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ReferenceTimeUpdate service UUID.
        /// </summary>
        public static readonly Guid ReferenceTimeUpdate = new Guid(0x00001806, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined Running Speed And Cadence Service UUID.
        /// </summary>
        public static readonly Guid RunningSpeedAndCadence = new Guid(0x00001814, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        /// <summary>
        /// Gets the Bluetooth SIG-defined ScanParameters service UUID.
        /// </summary>
        public static readonly Guid ScanParameters = new Guid(0x00001813, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid TransportDiscovery = new Guid(0x00001824, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        /// <summary>
        /// Gets the Bluetooth SIG-defined TxPower service UUID.
        /// </summary>
        public static readonly Guid TxPower = new Guid(0x00001804, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid UserData = new Guid(0x0000181C, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        public static readonly Guid WeightScale = new Guid(0x0000181D, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
    }
}