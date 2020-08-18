// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.NativeMethods
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal static class NativeMethods
    {
        private const string bthpropsDll = "bthprops.cpl";
        private const string wsDll = "ws2_32.dll";
        
        internal const int NS_BTH = 16;
        internal const int PROTOCOL_RFCOMM = 3;

        [DllImport("User32")]
        internal static extern IntPtr GetActiveWindow();

        // Requires Vista SP2 or later
        [DllImport(bthpropsDll, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothRegisterForAuthenticationEx(ref BLUETOOTH_DEVICE_INFO pbtdi, out IntPtr phRegHandle, BluetoothAuthenticationCallbackEx pfnCallback, IntPtr pvParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool BluetoothAuthenticationCallbackEx(IntPtr pvParam, ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothUnregisterAuthentication(IntPtr hRegHandle);


        [DllImport(bthpropsDll, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO pauthResponse);

        [DllImport(bthpropsDll, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO pauthResponse);

        [DllImport(bthpropsDll, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO pauthResponse);

        [DllImport(bthpropsDll, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothGetDeviceInfo(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothAuthenticateDeviceEx(IntPtr hwndParentIn, IntPtr hRadioIn, ref BLUETOOTH_DEVICE_INFO pbtdiInout, byte[] pbtOobData, BluetoothAuthenticationRequirements authenticationRequirement);

        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern int BluetoothRemoveDevice(ref ulong pAddress);

        // Radio
        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, out IntPtr phRadio);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextRadio(IntPtr hFind, out IntPtr phRadio);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindRadioClose(IntPtr hFind);
        
        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern int BluetoothGetRadioInfo(IntPtr hRadio, ref BLUETOOTH_RADIO_INFO pRadioInfo);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothIsConnectable(IntPtr hRadio);
        
        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothIsDiscoverable(IntPtr hRadio);


        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, bool fEnabled);
        
        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, bool fEnabled);

        // Discovery
        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstDevice(ref BLUETOOTH_DEVICE_SEARCH_PARAMS pbtsp, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextDevice(IntPtr hFind, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindDeviceClose(IntPtr hFind);

        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern int BluetoothEnumerateInstalledServices(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi, ref int pcServices, byte[] pGuidServices);
        [DllImport(bthpropsDll, SetLastError = true)]
        internal static extern int BluetoothSetServiceState(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi, ref Guid pGuidService, uint dwServiceFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);

        //SetService
        [DllImport(wsDll, EntryPoint = "WSASetService", SetLastError = true)]
        internal static extern int WSASetService(ref WSAQUERYSET lpqsRegInfo, WSAESETSERVICEOP essoperation, int dwControlFlags);
        
        // Last Error
        [DllImport(wsDll, EntryPoint = "WSAGetLastError", SetLastError = true)]
        internal static extern int WSAGetLastError();


        // Picker
        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        internal delegate bool PFN_DEVICE_CALLBACK(IntPtr pvParam, ref BLUETOOTH_DEVICE_INFO pDevice);

        [DllImport(bthpropsDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothDisplayDeviceProperties(IntPtr hwndParent, ref BLUETOOTH_DEVICE_INFO pbtdi);
    }

    /// <summary>
    /// The BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS structure contains specific configuration information about the Bluetooth device responding to an authentication request.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS
    {
        /// <summary>
        /// A BLUETOOTH_DEVICE_INFO structure that contains information about a Bluetooth device.
        /// </summary>
        internal BLUETOOTH_DEVICE_INFO deviceInfo;

        /// <summary>
        /// A BLUETOOTH_AUTHENTICATION_METHOD enumeration that defines the authentication method utilized by the Bluetooth device.
        /// </summary>
        internal BluetoothAuthenticationMethod authenticationMethod;

        /// <summary>
        /// A BLUETOOTH_IO_CAPABILITY enumeration that defines the input/output capabilities of the Bluetooth device.
        /// </summary>
        internal BluetoothIoCapability ioCapability;

        /// <summary>
        /// A AUTHENTICATION_REQUIREMENTS specifies the 'Man in the Middle' protection required for authentication.
        /// </summary>
        internal BluetoothAuthenticationRequirements authenticationRequirements;

        //union{
        //    ULONG   Numeric_Value;
        //    ULONG   Passkey;
        //};

        /// <summary>
        /// A ULONG value used for Numeric Comparison authentication.
        /// or
        /// A ULONG value used as the passkey used for authentication.
        /// </summary>
        internal uint Numeric_Value_Passkey;
    }
}
