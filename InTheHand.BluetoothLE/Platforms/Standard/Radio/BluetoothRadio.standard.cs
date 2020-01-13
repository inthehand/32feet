// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothRadio
// 
// Copyright (c) 2019 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Collections.Generic;
using InTheHand.Net;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothRadio
    {
        private static void GetAllRadios(List<BluetoothRadio> radios)
        {
        }

        private static BluetoothRadio GetDefault()
        {
            return null;
        }



        private string GetName()
        {
            return string.Empty;
        }

        private BluetoothAddress GetLocalAddress()
        {
            return BluetoothAddress.None;
        }

        private RadioMode GetMode()
        {
            return RadioMode.PowerOff;
        }

        private void SetMode(RadioMode value)
        {
        }
    }
}
