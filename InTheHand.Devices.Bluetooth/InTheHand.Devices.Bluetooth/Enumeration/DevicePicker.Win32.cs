//-----------------------------------------------------------------------
// <copyright file="DevicePicker.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;
using System.Diagnostics;
using InTheHand.Foundation;
using InTheHand.UI.Popups;
using System.Collections.Generic;

namespace InTheHand.Devices.Enumeration
{
    partial class DevicePicker
    {
        //private NativeMethods.PFN_DEVICE_CALLBACK _callback;

        private async Task<DeviceInformation> DoPickSingleDeviceAsync(Rect selection, Placement placement)
        {
            NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS sdp = new NativeMethods.BLUETOOTH_SELECT_DEVICE_PARAMS();
            sdp.dwSize = Marshal.SizeOf(sdp);
            sdp.hwndParent = Process.GetCurrentProcess().MainWindowHandle;
            sdp.numDevices = 1;
            if(!string.IsNullOrEmpty(Appearance.Title))
            {
                sdp.info = Appearance.Title;
            }

            //defaults
            sdp.fShowAuthenticated = true;
            sdp.fShowUnknown = false;

            List<int> codMasks = new List<int>();

            if(Filter.SupportedDeviceSelectors.Count > 0)
            {
                foreach(string filter in Filter.SupportedDeviceSelectors)
                {
                    var parts = filter.Split(':');
                    switch(parts[0])
                    {
                        case "bluetoothClassOfDevice":
                            int codMask = 0;
                            if (int.TryParse(parts[1], NumberStyles.HexNumber, null, out codMask))
                            {
                                codMasks.Add(codMask);      
                                break;
                            }
                            break;

                        case "bluetoothPairingState":
                            bool pairingState = bool.Parse(parts[1]);
                            if (pairingState)
                            {
                                sdp.fShowAuthenticated = true;
                                sdp.fShowUnknown = false;
                            }
                            else
                            {
                                sdp.fShowAuthenticated = false;
                                sdp.fShowUnknown = true;
                            }
                            break;
                    }
                }
            }

            sdp.hwndParent = NativeMethods.GetForegroundWindow();

            if(codMasks.Count > 0)
            {
                // marshal the CODs to native memory
                sdp.numOfClasses = codMasks.Count;
                sdp.prgClassOfDevices = Marshal.AllocHGlobal(8 * codMasks.Count);
                for (int i = 0; i < codMasks.Count; i++)
                {
                    Marshal.WriteInt32(IntPtr.Add(sdp.prgClassOfDevices, 8*i), codMasks[i]);
                }
            }

            /*if (Filter.SupportedDeviceSelectors.Count > 0)
            {
                _callback = new NativeMethods.PFN_DEVICE_CALLBACK(FilterDevices);
                sdp.pfnDeviceCallback = _callback;
                //sdp.pvParam = Marshal.AllocHGlobal(4);
                //Marshal.WriteInt32(sdp.pvParam, 1);
            }*/

            bool success = NativeMethods.BluetoothSelectDevices(ref sdp);

            if(sdp.prgClassOfDevices != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(sdp.prgClassOfDevices);
                sdp.prgClassOfDevices = IntPtr.Zero;
                sdp.numOfClasses = 0;
            }

            if (success)
            {
                BLUETOOTH_DEVICE_INFO info = Marshal.PtrToStructure<BLUETOOTH_DEVICE_INFO>(sdp.pDevices);
                NativeMethods.BluetoothSelectDevicesFree(ref sdp);

                foreach(string query in Filter.SupportedDeviceSelectors)
                {
                    var parts = query.Split(':');

                    if(parts[0] == "bluetoothService")
                    {
                        Guid serviceUuid = Guid.Parse(parts[1]);
                        foreach(Guid g in BluetoothDevice.GetRfcommServices(ref info))
                        {
                            if(g == serviceUuid)
                            {
                                return new DeviceInformation(info, serviceUuid);
                            }
                        }

                        return null;
                    }
                }

                return new DeviceInformation(info);
            }

            return null;
        }

        /*private bool FilterDevices(IntPtr param, ref BLUETOOTH_DEVICE_INFO info)
        {
            Debug.WriteLine(info.Address);

            Guid[] services = GetRemoteServices(info);
            if (services.Length > 0)
            {
                foreach (string filter in Filter.SupportedDeviceSelectors)
                {
                    Guid service = Guid.Parse(filter);
                    for (int i = 0; i < services.Length; i++)
                    {
                        if (services[i] == service)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }*/

        private Guid[] GetRemoteServices(BLUETOOTH_DEVICE_INFO info)
        {
            Guid[] services = new Guid[16];
            int ns = services.Length;
            int error = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref info, ref ns, services);
            if(error == 0)
            {
                Guid[] enumeratedServices = new Guid[ns];
                for(int i = 0; i < ns; i++)
                {
                    enumeratedServices[i] = services[i];
                }

                return enumeratedServices;
            }

            return new Guid[0];
        }

        private static class NativeMethods
        {
            [DllImport("User32", SetLastError = true)]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("bthprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevices(
                ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [DllImport("bthprops.cpl", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool BluetoothSelectDevicesFree(
                ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

            [DllImport("bthprops.cpl", SetLastError = true)]
            internal static extern int BluetoothEnumerateInstalledServices(
                IntPtr hRadio,
                ref BLUETOOTH_DEVICE_INFO pbtdi, 
                ref int pcServices, 
                Guid[] pGuidServices);

            [StructLayout(LayoutKind.Sequential, Size =60)]
            internal struct BLUETOOTH_SELECT_DEVICE_PARAMS
            {
                internal int dwSize;
                internal int numOfClasses;
                internal IntPtr prgClassOfDevices;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string info;
                internal IntPtr hwndParent;
                [MarshalAs(UnmanagedType.Bool)]
                bool fForceAuthentication;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowAuthenticated;
                [MarshalAs(UnmanagedType.Bool)]
                bool fShowRemembered;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fShowUnknown;
                [MarshalAs(UnmanagedType.Bool)]
                bool fAddNewDeviceWizard;
                [MarshalAs(UnmanagedType.Bool)]
                bool fSkipServicesPage;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                internal PFN_DEVICE_CALLBACK pfnDeviceCallback;
                internal IntPtr pvParam;
                internal uint numDevices;
                internal IntPtr /*PBLUETOOTH_DEVICE_INFO*/ pDevices;
            }
            
            internal delegate bool PFN_DEVICE_CALLBACK(IntPtr param, ref BLUETOOTH_DEVICE_INFO device);
        }
    }

    
}