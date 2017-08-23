//-----------------------------------------------------------------------
// <copyright file="Bluetooth.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Devices.Enumeration;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace InTheHand.Devices.Bluetooth
{
    internal sealed class BluetoothEndPoint : global::System.Net.EndPoint
    {
        private ulong _bluetoothAddress;
        private Guid _serviceId;

        internal BluetoothEndPoint(Rfcomm.RfcommDeviceService service) : this(service.Device.BluetoothAddress, service.ServiceId.Uuid) {}

        internal BluetoothEndPoint(ulong bluetoothAddress, Guid serviceId)
        {
            _bluetoothAddress = bluetoothAddress;
            _serviceId = serviceId;
        }

        public override AddressFamily AddressFamily
        {
            get
            {
                return (AddressFamily)32;
            }
        }

        public override EndPoint Create(SocketAddress socketAddress)
        {
            if (socketAddress == null)
            {
                throw new ArgumentNullException("socketAddress");
            }

            if (socketAddress.Family == AddressFamily)
            {
                int ibyte;



                byte[] addrbytes = new byte[8];

                for (ibyte = 0; ibyte < 8; ibyte++)
                {
                    addrbytes[ibyte] = socketAddress[2 + ibyte];
                }
                ulong address = BitConverter.ToUInt64(addrbytes, 0);

                byte[] servicebytes = new byte[16];

                for (ibyte = 0; ibyte < 16; ibyte++)
                {
                    servicebytes[ibyte] = socketAddress[10 + ibyte];
                }

                return new BluetoothEndPoint(address, new Guid(servicebytes));
            }

            return base.Create(socketAddress);
        }

        public override SocketAddress Serialize()
        {
            SocketAddress btsa = new SocketAddress(AddressFamily, 30);

            //copy address type
            btsa[0] = checked((byte)AddressFamily);

            //copy device id
            byte[] deviceidbytes = BitConverter.GetBytes(_bluetoothAddress);

            for (int idbyte = 0; idbyte < 6; idbyte++)
            {
                btsa[idbyte + 2] = deviceidbytes[idbyte];
            }



            //copy service clsid

            if (_serviceId != Guid.Empty)
            {
                byte[] servicebytes = _serviceId.ToByteArray();

                for (int servicebyte = 0; servicebyte < 16; servicebyte++)
                {
                    btsa[servicebyte + 10] = servicebytes[servicebyte];
                }
            }

            return btsa;
        }

        public override string ToString()
        {
            return _bluetoothAddress.ToString("X6") + ":" + _serviceId.ToString("D");
        }
    }

    internal static class BluetoothSockets
    {
        public static AddressFamily BluetoothAddressFamily = (AddressFamily)32;
        public static ProtocolType RfcommProtocolType = (ProtocolType)3;

        public static Socket CreateRfcommSocket()
        {
            return new Socket(BluetoothAddressFamily, SocketType.Stream, RfcommProtocolType);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct BLUETOOTH_DEVICE_INFO
    {
        internal int dwSize;
        internal ulong Address;
        internal uint ulClassofDevice;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fConnected;
        [MarshalAs(UnmanagedType.Bool)]
        private bool fRemembered;
        [MarshalAs(UnmanagedType.Bool)]
        internal bool fAuthenticated;
        private SYSTEMTIME stLastSeen;
        private SYSTEMTIME stLastUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
        internal string szName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SYSTEMTIME
    {
        private ushort year;
        private short month;
        private short dayOfWeek;
        private short day;
        private short hour;
        private short minute;
        private short second;
        private short millisecond;
    }

    [return: MarshalAs(UnmanagedType.Bool)]
    internal delegate bool PFN_AUTHENTICATION_CALLBACK_EX(IntPtr pvParam, ref NativeMethods.BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS pAuthCallbackParams);

    internal static class NativeMethods
    {
        private const string bthDll = "bthprops.cpl";

        private const int BLUETOOTH_MAX_NAME_SIZE = 248;

        // Pairing

        [DllImport(bthDll)]
        internal static extern int BluetoothAuthenticateDevice(IntPtr hwndParent, IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi, string pszPasskey, int ulPasskeyLength);

        [DllImport(bthDll)]
        internal static extern int BluetoothAuthenticateDeviceEx(IntPtr hwndParentIn,
                IntPtr hRadioIn,
                ref BLUETOOTH_DEVICE_INFO pbtdiInout,
                IntPtr pbtOobData,
                AUTHENTICATION_REQUIREMENTS authenticationRequirement);

        [DllImport(bthDll)]
        internal static extern int BluetoothRegisterForAuthenticationEx(ref BLUETOOTH_DEVICE_INFO pbtdiln, out IntPtr phRegHandleOut, PFN_AUTHENTICATION_CALLBACK_EX pfnCallbackIn, IntPtr pvParam);

        [DllImport(bthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothUnregisterAuthentication(IntPtr hRegHandle);

        [DllImport(bthDll)]
        internal static extern int BluetoothSendAuthenticationResponseEx(IntPtr hRadioIn, ref BLUETOOTH_AUTHENTICATE_RESPONSE pauthResponse);

        [DllImport(bthDll)]
        internal static extern int BluetoothRemoveDevice(ref ulong pAddress);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_AUTHENTICATION_CALLBACK_PARAMS
        {
            internal BLUETOOTH_DEVICE_INFO deviceInfo;
            internal BLUETOOTH_AUTHENTICATION_METHOD authenticationMethod;
            internal BLUETOOTH_IO_CAPABILITY ioCapability;
            internal AUTHENTICATION_REQUIREMENTS authenticationRequirements;
            internal uint Numeric_Value_Passkey;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_AUTHENTICATE_RESPONSE
        {
            ulong bthAddressRemote;
            BLUETOOTH_AUTHENTICATION_METHOD authMethod;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            byte[] pairingInfo;
            /*union {
            BLUETOOTH_PIN_INFO pinInfo; //17
            BLUETOOTH_OOB_DATA oobInfo; //32
            BLUETOOTH_NUMERIC_COMPARISON_INFO numericCompInfo; //8
            BLUETOOTH_PASSKEY_INFO passkeyInfo; // 8
            };*/
            byte negativeResponse;
        }

        internal enum AUTHENTICATION_REQUIREMENTS
        {
            MITMProtectionNotRequired = 0x00,
            MITMProtectionRequired = 0x01,
            MITMProtectionNotRequiredBonding = 0x02,
            MITMProtectionRequiredBonding = 0x03,
            MITMProtectionNotRequiredGeneralBonding = 0x04,
            MITMProtectionRequiredGeneralBonding = 0x05,
            MITMProtectionNotDefined = 0xff,
        }

        internal enum BLUETOOTH_AUTHENTICATION_METHOD
        {
            LEGACY = 1,
            OOB,
            NUMERIC_COMPARISON,
            PASSKEY_NOTIFICATION,
            PASSKEY,
        }

        internal static DevicePairingKinds BluetoothAuthenticationMethodToDevicePairingKinds(BLUETOOTH_AUTHENTICATION_METHOD authenticationMethod)
        {
            switch (authenticationMethod)
            {
                case BLUETOOTH_AUTHENTICATION_METHOD.LEGACY:
                    return DevicePairingKinds.ProvidePin;

                case BLUETOOTH_AUTHENTICATION_METHOD.NUMERIC_COMPARISON:
                    return DevicePairingKinds.ConfirmPinMatch;

                case BLUETOOTH_AUTHENTICATION_METHOD.PASSKEY_NOTIFICATION:
                    return DevicePairingKinds.DisplayPin;

                case BLUETOOTH_AUTHENTICATION_METHOD.PASSKEY:
                    return DevicePairingKinds.ProvidePin;

                default:
                    return DevicePairingKinds.None;
            }
        }

        internal enum BLUETOOTH_IO_CAPABILITY
        {
            DISPLAYONLY = 0x00,
            DISPLAYYESNO = 0x01,
            KEYBOARDONLY = 0x02,
            NOINPUTNOOUTPUT = 0x03,
            UNDEFINED = 0xff
        }

        // Radio
        [DllImport(bthDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, out IntPtr phRadio);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_FIND_RADIO_PARAMS
        {
            public int dwSize;
        }

        [DllImport(bthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextRadio(IntPtr hFind, out IntPtr phRadio);

        [DllImport(bthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindRadioClose(IntPtr hFind);


        [DllImport(bthDll, SetLastError = true)]
        internal static extern int BluetoothGetRadioInfo(IntPtr hRadio, ref BLUETOOTH_RADIO_INFO pRadioInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct BLUETOOTH_RADIO_INFO
        {
            internal int dwSize;
            internal ulong address;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BLUETOOTH_MAX_NAME_SIZE)]
            internal string szName;
            internal uint ulClassofDevice;
            internal ushort lmpSubversion;
            [MarshalAs(UnmanagedType.U2)]
            internal ushort manufacturer;
        }

        // notifications
        internal const int WM_DEVICECHANGE = 0x219;

        [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint RegisterClass(ref WNDCLASS lpWndClass);

        internal delegate int WindowProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet =CharSet.Unicode)]
        internal struct WNDCLASS
        {
            internal uint style;
            internal WindowProc lpfnWndProc;
            int cbClsExtra;
            int cbWndExtra;
            internal IntPtr hInstance;
            IntPtr hIcon;
            IntPtr hCursor;
            IntPtr hbrBackground;
            IntPtr lpszMenuName;
            internal string lpszClassName;
        }

        [DllImport("Comctl32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, IntPtr dwRefData);

        internal delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, uint uIdSubclass, IntPtr dwRefData);

        internal static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

        [DllImport("User32", CharSet=CharSet.Unicode, SetLastError =true)]
        internal static extern IntPtr CreateWindowEx(uint exStyle, string lpClassName, 
            string lpWindowName, 
            uint dwStyle,
            int x, int y, 
            int nWidth, int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("User32", SetLastError = true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("User32", SetLastError = true)]
        //internal static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
   
        //[DllImport("User32", SetLastError = true)]
        //internal static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        //[DllImport("User32", SetLastError = true)]
        //internal static extern int DispatchMessage(ref MSG lpmsg);

        [DllImport("User32", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int index);

        [DllImport("User32", SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, WindowProc newProc);

        [DllImport("User32", SetLastError = true)]
        internal static extern int DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32", SetLastError = true)]
        internal static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        internal struct MSG
        {
            internal IntPtr hwnd;
            internal uint message;
            internal IntPtr wParam;
            internal IntPtr lParam;
            internal uint time;
            internal ulong pt;
        }

        [DllImport("User32")]
        internal static extern int RegisterDeviceNotification(IntPtr handle, ref DEV_BROADCAST_HANDLE notificationFilter, DEVICE_NOTIFY flags);

        [StructLayout(LayoutKind.Sequential)]
        internal struct DEV_BROADCAST_HANDLE
        {
            internal int dbch_size;
            internal DBT_DEVTYP dbch_devicetype;
            private uint dbch_reserved;
            internal IntPtr dbch_handle;
            internal IntPtr dbch_hdevnotify;
            internal Guid dbch_eventguid;
            internal int nameoffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct BTH_HCI_EVENT_INFO
        {
            internal ulong bthAddress;
            byte connectionType;
            internal byte connected;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct BTH_L2CAP_EVENT_INFO
        {
            internal ulong bthAddress;
            internal ushort psm;
            internal byte connected;
            internal byte initiated;
        }

        internal static readonly Guid GUID_BLUETOOTH_L2CAP_EVENT = new Guid("7EAE4030-B709-4AA8-AC55-E953829C9DAA");
        internal static readonly Guid GUID_BLUETOOTH_HCI_EVENT = new Guid("FC240062-1541-49BE-B463-84C4DCD7BF7F");

        internal enum DBT_DEVTYP
        {
            HANDLE = 0x6,
        }

        internal enum DEVICE_NOTIFY
        {
            WINDOWS_HANDLE = 0,
            SERVICE_HANDLE = 1,
        }

        [StructLayout(LayoutKind.Sequential, Size = 8)]
        internal class BLUETOOTH_COD_PAIRS
        {
            internal uint ulCODMask;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string pcszDescription;
        }

        [DllImport(bthDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstDevice(
                ref BLUETOOTH_DEVICE_SEARCH_PARAMS pbtsp,
                ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextDevice(
            IntPtr hFind,
            ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindDeviceClose(IntPtr hFind);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_DEVICE_SEARCH_PARAMS
        {
            internal int dwSize;
            internal bool fReturnAuthenticated;
            internal bool fReturnRemembered;
            internal bool fReturnUnknown;
            internal bool fReturnConnected;
            internal bool fIssueInquiry;
            internal ushort cTimeoutMultiplier;
            internal IntPtr hRadio;
        }


        [DllImport(bthDll)]
        internal static extern int BluetoothGetDeviceInfo(IntPtr hRadio, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(bthDll)]
        internal static extern int BluetoothEnumerateInstalledServices(IntPtr hRadio,
            ref BLUETOOTH_DEVICE_INFO pbtdi,
            ref int pcServices,
            byte[] pGuidServices);

        [DllImport("User32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetGUIThreadInfo(int idThread, ref GUITHREADINFO lpgui);

        [StructLayout(LayoutKind.Sequential)]
        internal struct GUITHREADINFO
        {
            internal int cbSize;
            internal int flags;
            internal IntPtr hwndActive;
            internal IntPtr hwndFocus;
            internal IntPtr hwndCapture;
            internal IntPtr hwndMenuOwner;
            internal IntPtr hwndMoveSize;
            internal IntPtr hwndCaret;
            internal RECT rcCaret;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }
    }

    internal sealed class BluetoothMessageWindow
    {
        private NativeMethods.WindowProc _wndProc;
        private IntPtr _hwnd;
        private IntPtr _prevWndProc;
            //private Thread _pumpThread;

        internal BluetoothMessageWindow()
        {
            //_pumpThread = new Thread(MessagePump);
            //_pumpThread.IsBackground = true;
            _wndProc = new NativeMethods.WindowProc(WindowProc);
            NativeMethods.WNDCLASS cls = new NativeMethods.WNDCLASS();
            cls.lpszClassName = "InTheHand.Devices.Bluetooth";
            cls.hInstance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule);
            cls.lpfnWndProc = _wndProc;
            uint registration = NativeMethods.RegisterClass(ref cls);
            _hwnd = NativeMethods.CreateWindowEx(0, cls.lpszClassName, cls.lpszClassName, 0, 0, 0, 0, 0, NativeMethods.HWND_MESSAGE, IntPtr.Zero, cls.hInstance, IntPtr.Zero);
            //_pumpThread.Start();
            _prevWndProc = NativeMethods.GetWindowLong(_hwnd, -4);

            NativeMethods.SetWindowLong(_hwnd, -4, _wndProc);
            bool success = NativeMethods.PostMessage(_hwnd, 0x6, IntPtr.Zero, IntPtr.Zero);
        }

        /*private void MessagePump(object param)
        {
            NativeMethods.MSG m;
            int result;
            while ((result = NativeMethods.GetMessage(out m, _hwnd, 0, 0)) != 0)
            {
                if (result == -1)
                {
                    Debug.WriteLine("error");
                    Debug.WriteLine(Marshal.GetLastWin32Error());
                }
                else
                {
                    Debug.WriteLine("dispatched");
                    int d = NativeMethods.DispatchMessage(ref m);
                    Debug.WriteLine(d);
                }
            }
            Debug.WriteLine("loop exited");
            Debug.WriteLine(Marshal.GetLastWin32Error());
        }*/

        internal IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }

        internal event EventHandler<ulong> ConnectionStateChanged;

        private int WindowProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine(uMsg);
            switch(uMsg)
            {
                case NativeMethods.WM_DEVICECHANGE:
                    if (lParam != IntPtr.Zero)
                    {
                        NativeMethods.DEV_BROADCAST_HANDLE dbh = Marshal.PtrToStructure<NativeMethods.DEV_BROADCAST_HANDLE>(lParam);
                        if (dbh.dbch_eventguid == NativeMethods.GUID_BLUETOOTH_HCI_EVENT)
                        {
                            //BTH_HCI_EVENT_INFO
                            IntPtr bthhci = IntPtr.Add(lParam, 40);
                            NativeMethods.BTH_HCI_EVENT_INFO ei = Marshal.PtrToStructure<NativeMethods.BTH_HCI_EVENT_INFO>(bthhci);
                            Debug.WriteLine(ei.bthAddress + (ei.connected > 0 ? " connected" : " disconnected"));
                            ConnectionStateChanged?.Invoke(null, ei.bthAddress);
                        }
                        else if(dbh.dbch_eventguid == NativeMethods.GUID_BLUETOOTH_L2CAP_EVENT)
                        {
                            //BTH_L2CAP_EVENT_INFO
                            IntPtr bthl2cap = IntPtr.Add(lParam, 40);
                            NativeMethods.BTH_L2CAP_EVENT_INFO ei = Marshal.PtrToStructure<NativeMethods.BTH_L2CAP_EVENT_INFO>(bthl2cap);
                            Debug.WriteLine(ei.bthAddress + (ei.connected > 0 ? " connected" : " disconnected"));
                            ConnectionStateChanged?.Invoke(null, ei.bthAddress);
                        }
                    }

                    return 0;

                case 0x81:
                    return -1;

                case 1:
                case 0x83:
                    return 0;
            }

            if (_prevWndProc != IntPtr.Zero)
            {
                return NativeMethods.CallWindowProc(_prevWndProc, hwnd, uMsg, wParam, lParam);
            }
            else
            {
                return NativeMethods.DefWindowProc(hwnd, uMsg, wParam, lParam);
            }
        }
    }
}