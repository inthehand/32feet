// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.ClassOfDeviceHelper (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;

namespace InTheHand.Net.Bluetooth.Droid
{
    internal static class ClassOfDeviceHelper
    {
        public static ClassOfDevice ToClassOfDevice(Android.Bluetooth.BluetoothClass bluetoothClass)
        {
            DeviceClass deviceClass = (DeviceClass)((uint)bluetoothClass.DeviceClass);

            ServiceClass serviceClass = ServiceClass.None;

            foreach (Android.Bluetooth.ServiceClass servClass in Enum.GetValues(typeof(Android.Bluetooth.ServiceClass)))
            {
                if (bluetoothClass.HasService(servClass))
                {
                    serviceClass |= (ServiceClass)((uint)servClass >> 13);
                }
            }

            return new ClassOfDevice(deviceClass, serviceClass);
        }
    }
}