//-----------------------------------------------------------------------
// <copyright file="BluetoothVersion.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-19 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

// Defined in Bluetooth specification in HCI_Version and LMP_Version

namespace InTheHand.Net.Bluetooth
{
    public enum BluetoothVersion
    {
        /// <summary>
        /// Bluetooth Core Specification 1.0b
        /// </summary>
        Version10 = 0,
        /// <summary>
        /// Bluetooth Core Specification 1.1
        /// </summary>
        Version11 = 1,
        /// <summary>
        /// Bluetooth Core Specification 1.2
        /// </summary>
        Version12 = 2,
        /// <summary>
        /// Bluetooth Core Specification 2.0 + EDR
        /// </summary>
        Version20 = 3,
        /// <summary>
        /// Bluetooth Core Specification 2.1 + EDR
        /// </summary>
        Version21 = 4,
        /// <summary>
        /// Bluetooth Core Specification 3.0 + HS
        /// </summary>
        Version30 = 5,
        /// <summary>
        /// Bluetooth Core Specification 4.0
        /// </summary>
        Version40 = 6,
        /// <summary>
        /// Bluetooth Core Specification 4.1
        /// </summary>
        Version41 = 7,
        /// <summary>
        /// Bluetooth Core Specification 4.2
        /// </summary>
        Version42 = 8,
        /// <summary>
        /// Bluetooth Core Specification 5.0
        /// </summary>
        Version50 = 9,
        /// <summary>
        /// Bluetooth Core Specification 5.1
        /// </summary>
        Version51 = 10,
    }
}