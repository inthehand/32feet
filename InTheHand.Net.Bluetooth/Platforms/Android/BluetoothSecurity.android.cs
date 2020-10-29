// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Android)
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothSecurity
    {
        static bool PlatformPairRequest(BluetoothAddress device, string pin)
        {
            var nativeDevice = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(device.ToSixByteArray());
            
            if (pin != null)
            {
                nativeDevice.SetPin(System.Text.Encoding.ASCII.GetBytes(pin));
            }

            return nativeDevice.CreateBond();
        }

        static bool PlatformRemoveDevice(BluetoothAddress device)
        {
            var nativeDevice = Android.Bluetooth.BluetoothAdapter.DefaultAdapter.GetRemoteDevice(device.ToSixByteArray());
            var method = nativeDevice.Class.GetMethod("removeBond");
            var result = method.Invoke(nativeDevice);
            return false;
        }  
    }
}
