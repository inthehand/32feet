// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Bluetooth.Win32.NativeMethods
// 
// Copyright (c) 2003-2021 In The Hand Ltd, All rights reserved.
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
        internal const int BluetoothSocketAddressLength = 30;
        internal const int BTH_PORT_ANY = -1;

        private static bool? _isRunningOnMono; 
        public static bool IsRunningOnMono()
        {
#if DEBUG
            //return true;
#endif
            if (!_isRunningOnMono.HasValue)
            {
                _isRunningOnMono = Type.GetType("Mono.Runtime") != null;
            }

            return _isRunningOnMono.Value;
        }

        
        // Authentication
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothRegisterForAuthenticationEx(ref BLUETOOTH_DEVICE_INFO pbtdi, out IntPtr phRegHandle, BluetoothAuthenticationCallbackEx pfnCallback, IntPtr pvParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool BluetoothAuthenticationCallbackEx(IntPtr pvParam, ref BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothUnregisterAuthentication(IntPtr hRegHandle);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__PIN_INFO pauthResponse);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__OOB_DATA_INFO pauthResponse);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadio, ref BLUETOOTH_AUTHENTICATE_RESPONSE__NUMERIC_COMPARISON_PASSKEY_INFO pauthResponse);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothGetDeviceInfo(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int BluetoothAuthenticateDeviceEx(IntPtr hwndParentIn, IntPtr hRadioIn, ref BLUETOOTH_DEVICE_INFO pbtdiInout, byte[] pbtOobData, BluetoothAuthenticationRequirements authenticationRequirement);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern int BluetoothRemoveDevice(ref ulong pAddress);


        // Radio
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, out IntPtr phRadio);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextRadio(IntPtr hFind, out IntPtr phRadio);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindRadioClose(IntPtr hFind);
        
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern int BluetoothGetRadioInfo(IntPtr hRadio, ref BLUETOOTH_RADIO_INFO pRadioInfo);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothIsConnectable(IntPtr hRadio);
        
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothIsDiscoverable(IntPtr hRadio);


        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, bool fEnabled);
        
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, bool fEnabled);


        // Discovery
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstDevice(ref BLUETOOTH_DEVICE_SEARCH_PARAMS pbtsp, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextDevice(IntPtr hFind, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindDeviceClose(IntPtr hFind);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern int BluetoothEnumerateInstalledServices(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi, ref int pcServices, byte[] pGuidServices);
        
        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        internal static extern int BluetoothSetServiceState(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi, ref Guid pGuidService, uint dwServiceFlags);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);

        // SetService
        [DllImport(wsDll, ExactSpelling = true, EntryPoint = "WSASetServiceW", SetLastError = true)]
        internal static extern int WSASetService(ref WSAQUERYSET lpqsRegInfo, WSAESETSERVICEOP essoperation, int dwControlFlags);
        
        // Last Error
        [DllImport(wsDll, ExactSpelling = true, EntryPoint = "WSAGetLastError", SetLastError = true)]
        internal static extern int WSAGetLastError();


        // Picker
        [DllImport("user32", ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetActiveWindow();

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        internal delegate bool PFN_DEVICE_CALLBACK(IntPtr pvParam, ref BLUETOOTH_DEVICE_INFO pDevice);


        [DllImport(bthpropsDll, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothDisplayDeviceProperties(IntPtr hwndParent, ref BLUETOOTH_DEVICE_INFO pbtdi);

        // Events
        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "RegisterDeviceNotification")]
        internal static extern RegisterDeviceNotificationSafeHandle RegisterDeviceNotification(
               IntPtr hRecipient,
               ref DEV_BROADCAST_HANDLE notificationFilter,
               RegisterDeviceNotificationFlags flags
               );
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

    struct DEV_BROADCAST_HANDLE
    {
        // 32-bit:     3*4 + 2*4 + 16 + 4 + a = 6*4+16 = 24+16 + a = 40 + a = 44
        // 64-bit: (3+1)*4 + 2*8 + 16 + 4 + a = 16+16+16+4 + a     = 52 + a = 56
        const int SizeWithoutFakeDataArray = 40;
        const int SizeOfOneByteWithPadding = 4;
        const int SizeWithFakeDataArray = SizeWithoutFakeDataArray + SizeOfOneByteWithPadding;
        static int ActualSizeWithFakeDataArray;

        public DEV_BROADCAST_HDR header;
        //--
        internal readonly IntPtr dbch_handle;
        internal readonly IntPtr dbch_hdevnotify;
        internal readonly Guid dbch_eventguid;
        internal readonly Int32 dbch_nameoffset;
        // We can't include the fake data array because we use this struct as 
        // the first field in other structs!
        // byte dbch_data__0; //dbch_data[1];

        //----
        public DEV_BROADCAST_HANDLE(IntPtr deviceHandle)
        {
            this.header.dbch_reserved = 0;
            this.dbch_hdevnotify = IntPtr.Zero;
            this.dbch_eventguid = Guid.Empty;
            this.dbch_nameoffset = 0;
            //--
            this.header.dbch_devicetype = DbtDevTyp.Handle;
            this.dbch_handle = deviceHandle;
            //System.Diagnostics.Debug.Assert(
            //    SizeWithoutFakeDataArray == System.Runtime.InteropServices.Marshal.SizeOf(typeof(DEV_BROADCAST_HANDLE)),
            //    "Size not as expected");
            if (ActualSizeWithFakeDataArray == 0)
            {
                int actualSizeWithoutFakeDataArray = System.Runtime.InteropServices.Marshal.SizeOf(typeof(DEV_BROADCAST_HANDLE));
                ActualSizeWithFakeDataArray = Pad(1 + actualSizeWithoutFakeDataArray, IntPtr.Size);
            }
            this.header.dbch_size = ActualSizeWithFakeDataArray;
            //this.header.dbch_size = actualSizeWithoutFakeDataArray;

        }

        private static int Pad(int size, int alignment)
        {
            int x = size + alignment - 1;
            x /= alignment;
            x *= alignment;
            return x;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DEV_BROADCAST_HDR
    {
        internal int dbch_size;
        internal DbtDevTyp dbch_devicetype;
        internal int dbch_reserved;
    }

    internal enum DbtDevTyp : uint
    {
        /// <summary>
        /// OEM-defined device type
        /// </summary>
        Oem = 0x00000000,
        /// <summary>
        /// Devnode number
        /// /// </summary>
        DevNode = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        Volume = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        Port = 0x00000003,
        /// <summary>
        /// Network resource
        /// </summary>
        Network = 0x00000004,
        /// <summary>
        /// Device interface class
        /// </summary>
        DeviceInterface = 0x00000005,
        /// <summary>
        /// File system handle
        /// </summary>
        Handle = 0x00000006
    }

    internal enum RegisterDeviceNotificationFlags
    {
        WINDOW_HANDLE = 0x00000000,
        SERVICE_HANDLE = 0x00000001,
        ALL_INTERFACE_CLASSES = 0x00000004
    }
}