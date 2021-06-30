//-----------------------------------------------------------------------
// <copyright file="Bluetooth.windows.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.UI;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static Dictionary<ulong, WeakReference> KnownDevices = new Dictionary<ulong, WeakReference>();

        static async Task<bool> PlatformGetAvailability()
        {
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            return adaptor.IsLowEnergySupported;
        }

        static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            DevicePicker picker = new DevicePicker();
            Rect bounds = Rect.Empty;
#if !UAP
            uint len = 64;
            byte[] buffer = new byte[len];

            IntPtr hwnd = IntPtr.Zero;

            try
            {
                // a console app will return a non-null string for title
                if (!string.IsNullOrEmpty(Console.Title))
                {
                    bounds = new Rect(0, 0, 480, 480);
                    hwnd = GetConsoleWindow();
                    // set console host window as parent for picker
                    ((IInitializeWithWindow)(object)picker).Initialize(hwnd);
                }
            }
            catch
            {
            }

            int hasPackage = GetCurrentPackageId(ref len, buffer);

            if (hasPackage == 0x3d54)
            {
                foreach(var attr in System.Reflection.Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute)))
                {
                    picker.Appearance.Title = ((AssemblyProductAttribute)attr).Product + " wants to pair";
                    break;
                }
            }
            else
            {
                picker.Appearance.Title = Windows.ApplicationModel.Package.Current.DisplayName + " wants to pair";
            }
#else
            picker.Appearance.Title = Windows.ApplicationModel.Package.Current.DisplayName + " wants to pair";
            bounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
            picker.Appearance.SelectedAccentColor = (Color)Windows.UI.Xaml.Application.Current.Resources["SystemAccentColor"];
#endif

            if (options != null && !options.AcceptAllDevices)
            {
                foreach (var filter in options.Filters)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromDeviceName(filter.Name));
                    }
                    foreach (var service in filter.Services)
                    {
                        picker.Filter.SupportedDeviceSelectors.Add(GattDeviceService.GetDeviceSelectorFromUuid(service));
                    }
                }
            }

            if (picker.Filter.SupportedDeviceSelectors.Count == 0)
            {
                //picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelector());
                picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));
                picker.Filter.SupportedDeviceSelectors.Add(BluetoothLEDevice.GetDeviceSelectorFromPairingState(false));
            }

            try
            {
                var deviceInfo = await picker.PickSingleDeviceAsync(bounds);

                if (deviceInfo == null)
                    return null;

                var device = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
                var access = await device.RequestAccessAsync();
                return new BluetoothDevice(device);
            }
            catch(Exception ex)
            {
                if (ex.HResult == -2147023496)
                    throw new PlatformNotSupportedException("RequestDevice cannot be called from a Console application.");

                return null;
            }
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            // None of the build in selectors do a scan and return both paired and unpaired devices so here is the raw AQS string
            foreach (var device in await DeviceInformation.FindAllAsync("System.Devices.DevObjectType:=5 AND System.Devices.Aep.ProtocolId:=\"{BB7BB05E-5972-42B5-94FC-76EAA7084D49}\" AND (System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#False OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True OR System.Devices.Aep.Bluetooth.IssueInquiry:=System.StructuredQueryType.Boolean#True)"))
            {
                try {
                    devices.Add(await BluetoothLEDevice.FromIdAsync(device.Id));
                }
                catch(System.ArgumentException)
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
                    devices.Add(await BluetoothLEDevice.FromIdAsync(device.Id));
                }
                catch (System.ArgumentException)
                {
                }
            }

            return devices.AsReadOnly();
        }

        static bool _oldAvailability;

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await PlatformGetAvailability();
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            var radio = await adaptor.GetRadioAsync();
            radio.StateChanged += Radio_StateChanged;
        }

        private static async void Radio_StateChanged(Windows.Devices.Radios.Radio sender, object args)
        {
            bool newAvailability = await PlatformGetAvailability();
            if(newAvailability != _oldAvailability)
            {
                _oldAvailability = newAvailability;
                OnAvailabilityChanged();
            }
        }

        private static async void RemoveAvailabilityChanged()
        {
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            var radio = await adaptor.GetRadioAsync();
            radio.StateChanged -= Radio_StateChanged;
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
        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        private static extern int GetCurrentPackageId(ref uint bufferLength, byte[] buffer);

        [DllImport("Kernel32")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("User32")]
        static extern IntPtr GetActiveWindow();

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
#endif
    }
}
