//-----------------------------------------------------------------------
// <copyright file="BluetoothLEAppearanceCategory.cs" company="In The Hand Ltd">
//   Copyright (c) 2022 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Bluetooth
{
    /// <summary>
    /// Represents the external appearance of a device.
    /// </summary>
    /// <remarks>Referenced from <a href="https://specificationrefs.bluetooth.com/assigned-values/Appearance%20Values.pdf">https://specificationrefs.bluetooth.com/assigned-values/Appearance%20Values.pdf</a></remarks>
    public enum BluetoothLEAppearanceCategory : ushort
    {
        Unknown = 0x000,
        Phone = 0x001,
        Computer = 0x002,
        Watch = 0x003,
        Clock = 0x004,
        Display = 0x005,
        RemoteControl = 0x006,
        Eyeglasses = 0x007,
        Tag = 0x008,
        Keyring = 0x009,
        MediaPlayer = 0x00A,
        BarcodeScanner = 0x00B,
        Thermometer = 0x00C,
        HeartRateSensor = 0x00D,
        BloodPressure = 0x00E,
        HumanInterfaceDevice = 0x00F,
        GlucoseMeter = 0x010,
        RunningWalkingSensor = 0x011,
        Cycling = 0x012,
        ControlDevice = 0x013,
        NetworkDevice = 0x014,
        Sensor = 0x015,
        LightFixtures = 0x016,
        Fan = 0x017,
        Hvac = 0x018,
        AirConditioning = 0x019,
        Humidifier = 0x01A,
        Heating = 0x01B,
        AccessControl = 0x01C,
        MotorizedDevice = 0x01D,
        PowerDevice = 0x01E,
        LightSource = 0x01F,
        WindowCovering = 0x020,
        AudioSink = 0x021,
        AudioSource = 0x022,
        MotorizedVehicle = 0x023,
        DomesticAppliance = 0x024,
        WearableAudioDevice = 0x025,
        Aircraft = 0x026,
        AVEquipment = 0x027,
        DisplayEquipment = 0x028,
        Hearingaid = 0x029,
        Gaming = 0x02A,
        Signage = 0x02B,
        PulseOximeter = 0x031,
        WeightScale = 0x032,
        PersonalMobilityDevice = 0x033,
        ContinuousGlucoseMonitor = 0x034,
        InsulinPump = 0x035,
        MedicationDelivery = 0x036,
        OutdoorSportsActivity = 0x051,
    }
}
