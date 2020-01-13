// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.BLUETOOTH_FIND_RADIO_PARAMS
// 
// Copyright (c) 2003-2019 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
	// The BLUETOOTH_FIND_RADIO_PARAMS structure facilitates enumerating installed Bluetooth radios.
    [StructLayout(LayoutKind.Sequential)]
	internal struct BLUETOOTH_FIND_RADIO_PARAMS
	{
		public int dwSize;

        public static BLUETOOTH_FIND_RADIO_PARAMS Create()
        {
            BLUETOOTH_FIND_RADIO_PARAMS p = new BLUETOOTH_FIND_RADIO_PARAMS();
            p.dwSize = Marshal.SizeOf(p);
            return p;
        }
	}
}