// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.BluetoothSecurity (Android)
// 
// Copyright (c) 2003-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using Android.Bluetooth;
using Android.Content;
using System;

namespace InTheHand.Net.Bluetooth
{
    partial class BluetoothSecurity
    {
        private static BluetoothAdapter _adapter;

        static BluetoothSecurity()
        {
            BluetoothManager manager = null;

            manager = InTheHand.AndroidActivity.CurrentActivity.GetSystemService(Context.BluetoothService) as BluetoothManager;

            if (manager == null || manager.Adapter == null)
                throw new PlatformNotSupportedException();

            _adapter = manager.Adapter;
        }

        static bool PlatformPairRequest(BluetoothAddress device, bool? requireMitmProtection, string pin)
        {
            var nativeDevice = _adapter.GetRemoteDevice(device.ToNetworkOrderSixByteArray());
            
            if (pin != null)
            {
                nativeDevice.SetPin(System.Text.Encoding.ASCII.GetBytes(pin));
            }
            /*if (Permissions.IsDeclaredInManifest("android.permission.BLUETOOTH_PRIVILEGED"))
            {
                nativeDevice.SetPairingConfirmation(true);
            }*/

            return nativeDevice.CreateBond();
        }

        static bool PlatformRemoveDevice(BluetoothAddress device)
        {
            var nativeDevice = _adapter.GetRemoteDevice(device.ToNetworkOrderSixByteArray());
            var method = nativeDevice.Class.GetMethod("removeBond");
            var result = method.Invoke(nativeDevice);
            return (bool)(result as Java.Lang.Boolean);
        }  
    }
}
