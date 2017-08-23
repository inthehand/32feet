//-----------------------------------------------------------------------
// <copyright file="BluetoothMajorClass.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Devices.Bluetooth
{
    /// <summary>
    /// Indicates the Major Class code of the device, which is the general family of device with which the device is associated.
    /// </summary>
    public enum BluetoothMajorClass
    {
        /// <summary>
        /// Used when a more specific Major Class code is not suitable.
        /// </summary>
        Miscellaneous = 0,

        /// <summary>
        /// A computer. 
        /// Example devices are desktop, notebook, PDA and organizer.
        /// </summary>
        Computer = 1,

        /// <summary>
        /// A phone. 
        /// Example devices are cellular, cordless, pay phone and modem.
        /// </summary>
        Phone = 2,

        /// <summary>
        /// A LAN or network Access Point.
        /// </summary>
        NetworkAccessPoint = 3,

        /// <summary>
        /// An audio or video device. 
        /// Example devices are headset, speaker, stereo, video display and VCR.
        /// </summary>
        AudioVideo = 4,

        /// <summary>
        /// A peripheral device. 
        /// Examples are mouse, joystick and keyboard.
        /// </summary>
        Peripheral = 5,

        /// <summary>
        /// An imaging device. 
        /// Examples are printer, scanner, camera and display.
        /// </summary>
        Imaging = 6,

        /// <summary>
        /// A wearable device.
        /// </summary>
        Wearable = 7,

        /// <summary>
        /// A toy.
        /// </summary>
        Toy = 8,

        /// <summary>
        /// A health device. 
        /// An example is a heart rate monitor.
        /// </summary>
        Health = 9,
    }
}