using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        async Task<bool> DoGetAvailability()
        {
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            return adaptor.IsLowEnergySupported;
        }

        async Task<BluetoothDevice> DoRequestDevice(RequestDeviceOptions options)
        {
            DevicePicker picker = new DevicePicker();
            picker.Appearance.AccentColor = Windows.UI.Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            picker.Appearance.ForegroundColor = Windows.UI.Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            picker.Appearance.Title = Windows.ApplicationModel.Package.Current.DisplayName + " wants to pair";

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
            
            var deviceInfo = await picker.PickSingleDeviceAsync(Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds);
           
            if (deviceInfo == null)
                return null;

            var device = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
            return new BluetoothDevice(device);
        }

        bool _oldAvailability;

        private async void AddAvailabilityChanged()
        {
            _oldAvailability = await DoGetAvailability();
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            var radio = await adaptor.GetRadioAsync();
            radio.StateChanged += Radio_StateChanged;
        }

        private async void Radio_StateChanged(Windows.Devices.Radios.Radio sender, object args)
        {
            bool newAvailability = await DoGetAvailability();
            if(newAvailability != _oldAvailability)
            {
                _oldAvailability = newAvailability;
                OnAvailabilityChanged();
            }
        }

        private async void RemoveAvailabilityChanged()
        {
            var adaptor = await BluetoothAdapter.GetDefaultAsync();
            var radio = await adaptor.GetRadioAsync();
            radio.StateChanged -= Radio_StateChanged;
        }

        BluetoothLEAdvertisementWatcher watcher;

        private async Task DoRequestLEScan(BluetoothLEScan scan)
        {
            var filter = new BluetoothLEAdvertisementFilter();

            if (watcher == null)
            {
                watcher = new BluetoothLEAdvertisementWatcher(filter);
                watcher.Received += Watcher_Received;
            }
            else
            {
                watcher.AdvertisementFilter = filter;
            }

            watcher.Start();
        }

        
        private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            AdvertisementReceived?.Invoke(this, args);
        }
    }
}
