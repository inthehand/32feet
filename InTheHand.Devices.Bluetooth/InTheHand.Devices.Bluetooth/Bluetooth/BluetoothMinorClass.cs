//-----------------------------------------------------------------------
// <copyright file="BluetoothMinorClass.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates the Minor Class code of the device, which is the general family of device with which the device is associated.
    /// </summary>
    public enum BluetoothMinorClass
    {
        /// <summary>
        /// Use when a Minor Class code has not been assigned.
        /// </summary>
        Uncategorized = 0,


        /// <summary>
        /// A desktop computer.
        /// </summary>
        ComputerDesktop = 1,

        /// <summary>
        /// A server computer.
        /// </summary>
        ComputerServer = 2,

        /// <summary>
        /// A laptop computer.
        /// </summary>
        ComputerLaptop = 3,

        /// <summary>
        /// A handheld PC/PDA.
        /// </summary>
        ComputerHandheld = 4,

        /// <summary>
        /// A palm-sized PC/PDA.
        /// </summary>
        ComputerPalmSize = 5,

        /// <summary>
        /// A wearable, watch-sized, computer.
        /// </summary>
        ComputerWearable = 6,

        /// <summary>
        /// A tablet computer.
        /// </summary>
        ComputerTablet = 7,


        /// <summary>
        /// A cell phone.
        /// </summary>
        PhoneCellular = 1,

        /// <summary>
        /// A cordless phone.
        /// </summary>
        PhoneCordless = 2,

        /// <summary>
        /// A smartphone.
        /// </summary>
        PhoneSmartPhone = 3,

        /// <summary>
        /// A wired modem or voice gateway.
        /// </summary>
        PhoneWired = 4,

        /// <summary>
        /// Common ISDN access.
        /// </summary>
        PhoneIsdn = 5,


        /// <summary>
        /// Fully available.
        /// </summary>
        NetworkFullyAvailable = 0,

        /// <summary>
        /// 1% to 17% utilized.
        /// </summary>
        NetworkUsed01To17Percent = 8,

        /// <summary>
        /// 17% to 33% utilized.
        /// </summary>
        NetworkUsed17To33Percent = 16,

        /// <summary>
        /// 33% to 50% utilized.
        /// </summary>
        NetworkUsed33To50Percent = 24,

        /// <summary>
        /// 50% to 67% utilized.
        /// </summary>
        NetworkUsed50To67Percent = 32,

        /// <summary>
        /// 67% to 83% utilized.
        /// </summary>
        NetworkUsed67To83Percent = 40,

        /// <summary>
        /// 83% to 99% utilized.
        /// </summary>
        NetworkUsed83To99Percent = 48,

        /// <summary>
        /// Network service is not available.
        /// </summary>
        NetworkUsedNoServiceAvailable = 56,


        /// <summary>
        /// A wearable headset device.
        /// </summary>
        AudioVideoWearableHeadset = 1,

        /// <summary>
        /// A hands-free device.
        /// </summary>
        AudioVideoHandsFree = 2,

        /// <summary>
        /// A microphone.
        /// </summary>
        AudioVideoMicrophone = 4,

        /// <summary>
        /// A loudspeaker.
        /// </summary>
        AudioVideoLoudspeaker = 5,

        /// <summary>
        /// Headphones.
        /// </summary>
        AudioVideoHeadphones = 6,

        /// <summary>
        /// Portable audio device.
        /// </summary>
        AudioVideoPortableAudio = 7,

        /// <summary>
        /// A car audio device.
        /// </summary>
        AudioVideoCarAudio = 8,

        /// <summary>
        /// A set-top box.
        /// </summary>
        AudioVideoSetTopBox = 9,

        /// <summary>
        /// A HiFi audio device.
        /// </summary>
        AudioVideoHiFiAudioDevice = 10,

        /// <summary>
        /// A VCR.
        /// </summary>
        AudioVideoVcr = 11,

        /// <summary>
        /// A video camera.
        /// </summary>
        AudioVideoVideoCamera = 12,

        /// <summary>
        /// A camcorder.
        /// </summary>
        AudioVideoCamcorder = 13,

        /// <summary>
        /// A video monitor.
        /// </summary>
        AudioVideoVideoMonitor = 14,

        /// <summary>
        /// A video display and loudspeaker.
        /// </summary>
        AudioVideoVideoDisplayAndLoudspeaker = 15,

        /// <summary>
        /// A video conferencing device.
        /// </summary>
        AudioVideoVideoConferencing = 16,

        /// <summary>
        /// A gaming console or toy.
        /// </summary>
        AudioVideoGamingOrToy = 18,


        /// <summary>
        /// A joystick.
        /// </summary>
        PeripheralJoystick = 1,

        /// <summary>
        /// A gamepad.
        /// </summary>
        PeripheralGamepad = 2,

        /// <summary>
        /// A remote control.
        /// </summary>
        PeripheralRemoteControl = 3,

        /// <summary>
        /// A sensing device.
        /// </summary>
        PeripheralSensing = 4,

        /// <summary>
        /// A digitizer tablet.
        /// </summary>
        PeripheralDigitizerTablet = 5,

        /// <summary>
        /// A card reader.
        /// </summary>
        PeripheralCardReader = 6,

        /// <summary>
        /// A digital pen.
        /// </summary>
        PeripheralDigitalPen = 7,

        /// <summary>
        /// A handheld scanner for bar codes, RFID, etc.
        /// </summary>
        PeripheralHandheldScanner = 8,

        /// <summary>
        /// A handheld gesture input device, such as a "wand" form factor device.
        /// </summary>
        PeripheralHandheldGesture = 9,


        /// <summary>
        /// A display.
        /// </summary>
        ImagingDisplay = 4,

        /// <summary>
        /// A camera.
        /// </summary>
        ImagingCamera = 8,

        /// <summary>
        /// A scanner.
        /// </summary>
        ImagingScanner = 16,

        /// <summary>
        /// A printer;
        /// </summary>
        ImagingPrinter = 32,


        /// <summary>
        /// A wristwatch.
        /// </summary>
        WearableWristwatch = 1,

        /// <summary>
        /// A pager.
        /// </summary>
        WearablePager = 2,

        /// <summary>
        /// A jacket.
        /// </summary>
        WearableJacket = 3,

        /// <summary>
        /// A helmet.
        /// </summary>
        WearableHelmet = 4,

        /// <summary>
        /// Glasses.
        /// </summary>
        WearableGlasses = 5,


        /// <summary>
        /// A robot.
        /// </summary>
        ToyRobot = 1,

        /// <summary>
        /// A vehicle.
        /// </summary>
        ToyVehicle = 2,

        /// <summary>
        /// A doll or action figure.
        /// </summary>
        ToyDoll = 3,

        /// <summary>
        /// A controller.
        /// </summary>
        ToyController = 4,

        /// <summary>
        /// A game.
        /// </summary>
        ToyGame = 5,


        /// <summary>
        /// A blood pressure monitor.
        /// </summary>
        HealthBloodPressureMonitor = 1,

        /// <summary>
        /// A thermometer.
        /// </summary>
        HealthThermometer = 2,

        /// <summary>
        /// A weighing scale.
        /// </summary>
        HealthWeighingScale = 3,

        /// <summary>
        /// A glucose meter.
        /// </summary>
        HealthGlucoseMeter = 4,

        /// <summary>
        /// A pulse oximeter.
        /// </summary>
        HealthPulseOximeter = 5,

        /// <summary>
        /// A heart rate or pulse monitor.
        /// </summary>
        HealthHeartRateMonitor = 6,

        /// <summary>
        /// A health data display.
        /// </summary>
        HealthHealthDataDisplay = 7,

        /// <summary>
        /// A step counter.
        /// </summary>
        HealthStepCounter = 8,

        /// <summary>
        /// A body composition analyzer.
        /// </summary>
        HealthBodyCompositionAnalyzer = 9,

        /// <summary>
        /// A peak flow monitor.
        /// </summary>
        HealthPeakFlowMonitor = 10,

        /// <summary>
        /// A medication monitor.
        /// </summary>
        HealthMedicationMonitor = 11,

        /// <summary>
        /// A knee prosthesis.
        /// </summary>
        HealthKneeProsthesis = 12,

        /// <summary>
        /// An ankle prosthesis.
        /// </summary>
        HealthAnkleProsthesis = 13,

        /// <summary>
        /// A generic health manager.
        /// </summary>
        HealthGenericHealthManager = 14,

        /// <summary>
        /// A personal mobility device.
        /// </summary>
        HealthPersonalMobilityDevice = 15,
    }
}