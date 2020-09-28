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

        static async Task<bool> DoGetAvailability()
        {
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            return adaptor.IsLowEnergySupported;
        }

        static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            
            DevicePicker picker = new DevicePicker();
            Rect bounds = Rect.Empty;
            //picker.Appearance.AccentColor = Windows.UI.Colors.Green;
            //picker.Appearance.ForegroundColor = Windows.UI.Colors.White;
            //picker.Appearance.BackgroundColor = Windows.UI.Colors.DarkGray;
#if !UAP
            uint len = 64;
            byte[] buffer = new byte[len];

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

            if (!options.AcceptAllDevices)
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
            
            var deviceInfo = await picker.PickSingleDeviceAsync(bounds);
           
            if (deviceInfo == null)
                return null;

            var device = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
            var access = await device.RequestAccessAsync();
            return new BluetoothDevice(device);
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            foreach(var device in await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(false)))
            {
                devices.Add(await BluetoothLEDevice.FromIdAsync(device.Id));
            }

            return devices.AsReadOnly();
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            foreach (var device in await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true)))
            {
                devices.Add(await BluetoothLEDevice.FromIdAsync(device.Id));
            }

            return devices.AsReadOnly();
        }

        static bool _oldAvailability;

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await DoGetAvailability();
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            var radio = await adaptor.GetRadioAsync();
            radio.StateChanged += Radio_StateChanged;
        }

        private static async void Radio_StateChanged(Windows.Devices.Radios.Radio sender, object args)
        {
            bool newAvailability = await DoGetAvailability();
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
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GetCurrentPackageId(ref uint bufferLength, byte[] buffer);
#endif
    }
}
