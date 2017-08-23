//-----------------------------------------------------------------------
// <copyright file="Radio.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;

namespace InTheHand.Devices.Radios
{
    partial class Radio
    {
        private static Type s_type10 = Type.GetType("Windows.Devices.Radios.Radio, Windows, ContentType=WindowsRuntime");
        private object _object10 = null;

        internal Radio(object o10)
        {
            _object10 = o10;
        }

        private static string DoGetDeviceSelector()
        {
            if (s_type10 != null)
            {
                return s_type10.GetMethod(nameof(GetDeviceSelector), BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[0]).ToString();
            }

            return string.Empty;
        }

        private static async Task<RadioAccessStatus> DoRequestAccessAsync()
        {
            /*if (s_type10 != null)
            {
                object rta = s_type10.GetMethod("RequestAccessAsync").Invoke(null, new object[] { });
                Type tras = Type.GetType("Windows.Devices.Radios.RadioAccessStatus, Windows, ContentType=WindowsRuntime");
                Type tr = typeof(Windows.Foundation.IAsyncOperation<>).MakeGenericType(tras);
                object result = tr.GetMethod("GetResults").Invoke(rta, new object[] { });
                return (RadioAccessStatus)((int)result);
            }
            else
            {*/
                // Only add a Radio if support is present.
                return RadioAccessStatus.Allowed;
            //}
        }

        private static void DoGetRadiosAsync(List<Radio> radios)
        {
            /*if (s_type10 != null)
            {
                object rta = s_type10.GetMethod("GetRadiosAsync").Invoke(null, new object[] { });
                Type ivv = Type.GetType("Windows.Foundation.Collections.IVectorView`1, Windows, ContentType=WindowsRuntime");
                Type t = ivv.MakeGenericType(s_type10);
                Type tr = typeof(Windows.Foundation.IAsyncOperation<>).MakeGenericType(t);
                object results = tr.GetMethod("GetResults").Invoke(rta, new object[] { });
                uint count = (uint)t.GetProperty("Size").GetValue(results);
                IEnumerable il = results as IEnumerable;
                foreach(object o in il)
                {
                    radios.Add(new Radio(o));
                }
            }
            else
            {*/
                // Only add a Radio if support is present.
                if (NativeMethods.BluetoothIsVersionAvailable(2, 0))
                {
                    radios.Add(new Radio());
                }
            //}
        }

        private Task<RadioAccessStatus> DoSetStateAsync(RadioState state)
        {
            bool success = false;
            bool enable = state == RadioState.On;

            string path = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHPORT\\Parameters\\Radio Support", "SupportDLL", string.Empty).ToString();
            if (!string.IsNullOrEmpty(path))
            {
                //load dll
                IntPtr hmodule = NativeMethods.LoadLibrary(path);
                if (hmodule != IntPtr.Zero)
                {
                    //call function
                    IntPtr addr = NativeMethods.GetProcAddress(hmodule, "BluetoothEnableRadio");

                    if (addr != IntPtr.Zero)
                    {
                        NativeMethods.BluetoothEnableRadio deleg = Marshal.GetDelegateForFunctionPointer<NativeMethods.BluetoothEnableRadio>(addr);
                        int result = deleg.Invoke(enable);
                        success = result == 0;
                    }

                    //free dll
                    NativeMethods.FreeLibrary(hmodule);
                }
            }
            
            return Task.FromResult<RadioAccessStatus>(success ? RadioAccessStatus.Allowed : RadioAccessStatus.Unspecified);
        }

        // only supporting Bluetooth radio
        private RadioKind GetKind()
        {
            /*if (s_type10 != null)
            {
                return (RadioKind)((int)s_type10.GetProperty("Kind").GetValue(_object10));
            }
            else
            {*/
                return RadioKind.Bluetooth;
            //}
        }

        // matching the UWP behaviour (although we could have used the radio name)
        private string GetName()
        {
            /*if (s_type10 != null)
            {
                return s_type10.GetProperty("Name").GetValue(_object10).ToString();
            }
            else
            {*/
                return RadioKind.Bluetooth.ToString();
            //}
        }

        private RadioState GetState()
        {
            RadioState state = RadioState.Unknown;

            /*if (s_type10 != null)
            {
                return (RadioState)((int)s_type10.GetProperty("State").GetValue(_object10));
            }
            else
            {*/
                try
                {
                    string path = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\BTHPORT\\Parameters\\Radio Support", "SupportDLL", string.Empty).ToString();
                    if (!string.IsNullOrEmpty(path))
                    {
                        //load dll
                        IntPtr hmodule = NativeMethods.LoadLibrary(path);
                        if (hmodule != IntPtr.Zero)
                        {
                            //call function
                            IntPtr addr = NativeMethods.GetProcAddress(hmodule, "IsBluetoothRadioEnabled");

                            if (addr != IntPtr.Zero)
                            {
                                bool enabled = false;
                                NativeMethods.IsBluetoothRadioEnabled deleg = Marshal.GetDelegateForFunctionPointer<NativeMethods.IsBluetoothRadioEnabled>(addr);
                                int result = deleg.Invoke(ref enabled);
                                if (result == 0)
                                {
                                    state = enabled ? RadioState.On : RadioState.Off;
                                }
                            }

                            //free dll
                            NativeMethods.FreeLibrary(hmodule);
                        }
                    }

                }
                catch
                {
                }
            //}

            return state;
        }

        private static class NativeMethods
        {
            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableDiscovery(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothEnableIncomingConnections(IntPtr hRadio, [MarshalAs(UnmanagedType.Bool)] bool fEnabled);

            [DllImport("BluetoothApis", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothIsConnectable(IntPtr hRadio);

            [DllImport("BluetoothApis")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothIsVersionAvailable(byte MajorVersion, byte MinorVersion);

            [DllImport("Kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("Kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("Kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            internal delegate int BluetoothEnableRadio([MarshalAs(UnmanagedType.Bool)] bool fEnable);

            internal delegate int IsBluetoothRadioEnabled([MarshalAs(UnmanagedType.Bool)] ref bool pfEnabled);

            /*[DllImport("BluetoothApis", SetLastError = false)]
            internal static extern int BthpIsRadioSoftwareEnabled(out bool value);

            [DllImport("BluetoothApis", SetLastError = false)]
            internal static extern int BthpEnableRadioSoftware(bool fEnable);*/
        }
    }
}