//-----------------------------------------------------------------------
// <copyright file="Bluetooth.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-24 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Devices.Radios;
using Windows.UI;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        private static BluetoothAdapter adapter;
        private static Radio radio;

        internal static Dictionary<ulong, WeakReference> KnownDevices = new Dictionary<ulong, WeakReference>();

        static async Task<BluetoothAdapter> GetAdapter()
        {
            if(adapter == null)
            {
                adapter = await BluetoothAdapter.GetDefaultAsync();
            }

            return adapter;
        }
        static async Task<Radio> GetRadio()
        {
            if(radio == null)
            {
                radio = await (await GetAdapter()).GetRadioAsync();
            }

            return radio;
        }

        static async Task<bool> PlatformGetAvailability()
        {
            if(await GetAdapter() != null)
            {
                if(adapter.IsLowEnergySupported)
                {
                    if(await GetRadio() != null)
                    {
                        return radio.State == RadioState.On;
                    }
                }
            }

            return false;
        }

        static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions? options)
        {
            DevicePicker picker = new DevicePicker();
            Windows.Foundation.Rect bounds = Windows.Foundation.Rect.Empty;
#if !UAP
            uint len = 64;
            byte[] buffer = new byte[len];
            bounds = new Windows.Foundation.Rect(0, 0, 480, 480);
            IntPtr hwnd = IntPtr.Zero;

            hwnd = GetActiveWindow();
            if (hwnd == IntPtr.Zero)
            {
                try
                {
                    // a console app will return a non-null string for title
                    if (!string.IsNullOrEmpty(Console.Title))
                    {

                        hwnd = GetConsoleWindow();
                    }
                }
                catch
                {

                }
            }

            if(hwnd == IntPtr.Zero)
            {
                hwnd = GetHWNDForCurrentThread();
            }

            // set host window as parent for picker


            int hasPackage = GetCurrentPackageId(ref len, buffer);

            if (hasPackage == 0x3d54)
            {
                foreach (var attr in System.Reflection.Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute)))
                {
                    picker.Appearance.Title = ((AssemblyProductAttribute)attr).Product + " wants to connect";
                    break;
                }
            }
            else
            {
#if NET6_0_OR_GREATER
                //hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
#endif
                picker.Appearance.Title = Windows.ApplicationModel.Package.Current.DisplayName + " wants to connect";
            }

#if NET6_0_OR_GREATER
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
#else
                ((IInitializeWithWindow)(object)picker).Initialize(hwnd);
#endif

#else
            picker.Appearance.Title = Windows.ApplicationModel.Package.Current.DisplayName + " wants to connect";
            bounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
            picker.Appearance.SelectedAccentColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
#endif
            
            if (options != null && !options.AcceptAllDevices)
            {
                foreach (var filter in options.Filters)
                {
                    string filterString = string.Empty;

                    if (!string.IsNullOrEmpty(filter.NamePrefix))
                    {
                        filterString += $" AND System.ItemNameDisplay:~<\"{filter.NamePrefix}\"";
                    }
                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        filterString += $" AND System.ItemNameDisplay:=\"{filter.Name}\"";
                    }
                    foreach (var service in filter.Services)
                    {
                        filterString += $" AND System.Devices.AepService.Bluetooth.ServiceGuid:=\"{{{(Guid)service}}}\"";
                    }

                    picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true) + filterString);
                    picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(false) + filterString);
                }
            }

            if (picker.Filter.SupportedDeviceSelectors.Count == 0)
            {
                picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));
                picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(false));
            }

            try
            {
                var deviceInfo = await picker.PickSingleDeviceAsync(bounds);

                if (deviceInfo == null)
                    return null;

                var device = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);

                // it seems odd that the picker functions without the relevant permission
                // but then this fails so this is as early as we can catch it currently.
                if (device == null)
                    ThrowSecurityExceptionOnMissingDeviceCapability();

                var access = await device.RequestAccessAsync();
                return new BluetoothDevice(device);
            }
            catch(SecurityException se)
            {
                // we want to pass this exception on but catch more generic exceptions below.
                throw se;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147023496)
                    throw new PlatformNotSupportedException("RequestDevice cannot be called from a Console application.");

                return null;
            }
        }

        private static void ThrowSecurityExceptionOnMissingDeviceCapability()
        {
            throw new SecurityException("UWP Applications require the 'bluetooth' device capability to be declared in the Package.appxmanifest.");
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options, CancellationToken cancellationToken = default)
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            // None of the build in selectors do a scan and return both paired and unpaired devices so here is the raw AQS string
            string selectionQuery = "System.Devices.DevObjectType:=5 AND System.Devices.Aep.ProtocolId:=\"{BB7BB05E-5972-42B5-94FC-76EAA7084D49}\" AND (System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#False OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True OR System.Devices.Aep.Bluetooth.IssueInquiry:=System.StructuredQueryType.Boolean#True)";
            StringBuilder filterQuery = new StringBuilder();

            if (options is { Filters: not null })
            {
                foreach (var filter in options.Filters)
                {
                    List<string> clauses = new List<string>();

                    if (!string.IsNullOrEmpty(filter.Name)) 
                        clauses.Add($"System.ItemNameDisplay:=\"{filter.Name}\""); 
                    
                    if (!string.IsNullOrEmpty(filter.NamePrefix)) 
                        clauses.Add($"System.ItemNameDisplay:~<\"{filter.NamePrefix}\"");

                    filterQuery.Append(string.Join(" OR ", clauses));
                }

                if (filterQuery.Length > 0)
                    selectionQuery += " AND (" + filterQuery + ")";
            }

            foreach (var device in await DeviceInformation.FindAllAsync(selectionQuery))
            {
                try
                {
                    bool returnDevice = true;

                    var bluetoothDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                    if (bluetoothDevice != null)
                    {
                        if (options is { Filters: not null })
                        {
                            foreach (var filter in options.Filters)
                            {
                                foreach (var service in filter.Services)
                                {
                                    var deviceServices =
                                        await bluetoothDevice.GetGattServicesAsync(BluetoothCacheMode.Cached);
                                    if (deviceServices.Status == GattCommunicationStatus.Success)
                                    {
                                        returnDevice = false; // because service enumeration is so unreliable only apply the service filter if we are able to retrieve the services
                                        foreach (var gattService in deviceServices.Services)
                                        {
                                            if (gattService.Uuid == (Guid)service)
                                            {
                                                returnDevice = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if(returnDevice)
                            devices.Add(bluetoothDevice);
                    }
                    else
                    {
                        ThrowSecurityExceptionOnMissingDeviceCapability();
                    }
                }
                catch (ArgumentException)
                {
                }
            }

            return devices.AsReadOnly();
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            foreach (var device in await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)))
            {
                try
                {
                    var bluetoothDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
                    if(bluetoothDevice != null)
                    {
                        devices.Add(bluetoothDevice);
                    }
                    else
                    {
                        ThrowSecurityExceptionOnMissingDeviceCapability();
                    }
                }
                catch (System.ArgumentException)
                {
                }
            }

            return devices.AsReadOnly();
        }

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await PlatformGetAvailability();

            if (radio == null)
            {
                radio = await GetRadio();
            }

            if (radio != null)
            {
                radio.StateChanged += Radio_StateChanged;
            }
        }

        private static async void Radio_StateChanged(Windows.Devices.Radios.Radio sender, object args)
        {
            await OnAvailabilityChanged();
        }

        private static async void RemoveAvailabilityChanged()
        {
            var r = await GetRadio();
            
            if (r != null)
            {
                radio.StateChanged -= Radio_StateChanged;
            }
        }

        private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            if (options == null)
                return new BluetoothLEAdvertisementWatcher();

            return new BluetoothLEAdvertisementWatcher(options);
        }


        private static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            AdvertisementReceived?.Invoke(null, args);
        }


#if !UAP

        private static IntPtr GetHWNDForCurrentThread()
        {
            TaskCompletionSource<IntPtr> handleTask = new TaskCompletionSource<IntPtr>();

            int threadId = AppDomain.GetCurrentThreadId();
            EnumThreadWndProc handler = new EnumThreadWndProc((handle, lparam) => {
                if (handle != IntPtr.Zero)
                {
                    handleTask.SetResult(handle);
                    return false;
                }

                return true;
            });

            EnumThreadWindows(threadId, handler, IntPtr.Zero);

            handleTask.Task.Wait();

            return handleTask.Task.Result;
        }

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        private static extern int GetCurrentPackageId(ref uint bufferLength, byte[] buffer);

        [DllImport("Kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("User32")]
        static extern IntPtr GetActiveWindow();

        [DllImport("User32")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

        delegate bool EnumThreadWndProc(IntPtr hwnd, IntPtr lParam);

#if !NET6_0_OR_GREATER
        /// <exclude/>
        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
#endif

#endif
    }
}
