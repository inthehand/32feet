// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Msft.BTH_DEVICE_INFO
// 
// Copyright (c) 2003-2011 In The Hand Ltd, All rights reserved.
// This source code is licensed under the In The Hand Community License - see License.txt

using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Msft
{
    //
    // The BTH_DEVICE_INFO structure stores information about a Bluetooth device.
    //
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct BTH_DEVICE_INFO
    {
        //
        // Combination BDIF_Xxx flags
        //
        internal BluetoothDeviceInfoProperties flags;
        //
        // Address of remote device.
        //
        internal long address;
        //
        // Class Of Device.
        //
        internal uint classOfDevice;
        //
        // name of the device (As UTF8 String)
        //
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = NativeMethods.BTH_MAX_NAME_SIZE)]
        internal byte[] name;
    }
}
