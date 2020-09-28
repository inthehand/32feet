//-----------------------------------------------------------------------
// <copyright file="BluetoothAppearance.cs" company="In The Hand Ltd">
//   Copyright (c) 2020 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.Bluetooth
{
    public enum BluetoothAppearance : ushort
    {
        Unknown = 0,

        GenericPhone = 64,

        GenericComputer = 128,

        GenericWatch = 192,

        GenericClock = 256,

        GenericDisplay = 320,

        GenericRemoteControl = 384,

        GenericEyeGlasses = 448,

        GenericTag = 512,

        GenericKeyring = 576,

        GenericMediaPlayer = 640,

        GenericBarcodeScanner = 704,

        GenericThermometer = 768,

        GenericHeartRateSensor = 832,

        GenericBloodPressure = 896,

        GenericHumanInterfaceDevice = 960,

        GenericGlucoseMeter = 1024,

        GenericRunningWalkingSensor = 1088,

        GenericCycling = 1152,

        GenericControlDevice = 1216,

        GenericNetworkDevice = 1280,

        GenericSensor = 1344,

        GenericLightFixture = 1408,

        GenericFan = 1472,

        GenericHvac = 1536,

        GenericAirConditioning = 1600,

        GenericHumidifier = 1664,

        GenericHeating = 1728,

        GenericAccessControl = 1792,

        GenericMotorizedDevice = 1856,

        GenericPowerDevice = 1920,

        GenericLightSource = 1984,

        GenericPulseOximeter = 3136,

        GenericWeightScale = 3200,

        GenericOutdoorSportsActivity = 5184,
    }
}
