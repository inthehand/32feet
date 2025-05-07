//-----------------------------------------------------------------------
// <copyright file="Bluetooth.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-25 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#if __IOS__
using CoreGraphics;
#endif
#if !__MACOS__
using UIKit;
#endif

#if NET6_0_OR_GREATER || __WATCHOS__
using CBManagerState = CoreBluetooth.CBManagerState;
#else
using CBManagerState = CoreBluetooth.CBCentralManagerState;
#endif

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static CBCentralManager _manager;

        private static void Initialize()
        {
            if (_manager == null)
            {
                Debug.WriteLine("Initialize");
                
                var hasInfoKey = CoreFoundation.CFBundle.GetMain().InfoDictionary.ContainsKey(new NSString("NSBluetoothAlwaysUsageDescription"));
                if(!hasInfoKey)
                {
                    throw new PlatformNotSupportedException("Application info.plist must contain an entry for NSBluetoothAlwaysUsageDescription");
                }

                CBCentralInitOptions options = new CBCentralInitOptions
                {
                    ShowPowerAlert = true
#if __IOS__
                    //, RestoreIdentifier = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName")?.ToString()
#endif
                };

                _manager = new CBCentralManager(null, null, options);
                _manager.UpdatedState += Manager_UpdatedState;
                _manager.DiscoveredPeripheral += Manager_DiscoveredPeripheral;
                Debug.WriteLine($"CBCentralManager:{_manager.State}");
            }
        }

        private static void Manager_DiscoveredPeripheral(object? sender, CBDiscoveredPeripheralEventArgs e)
        {
            DiscoveredPeripheral?.Invoke(null, e);
            
            var bae = new BluetoothAdvertisingEvent(e.Peripheral, e.AdvertisementData, e.RSSI);
            AdvertisementReceived?.Invoke(null, bae);
            if (!string.IsNullOrWhiteSpace(e.Peripheral.Name) && !_foundDevices.Contains(e.Peripheral))
            {
                Debug.WriteLine($"Peripheral: {e.Peripheral.Identifier} Name: {e.Peripheral.Name} RSSI: {e.RSSI}");
                _foundDevices.Add(e.Peripheral);
            }
        }

        private static void Manager_UpdatedState(object? sender, EventArgs e)
        {
            OnAvailabilityChanged();
        }

        internal static bool IsAvailable
        {
            get
            {
                return _manager != null && (_manager.State == CBManagerState.PoweredOn || _manager.State == CBManagerState.Resetting);
            }
        }

        internal static event EventHandler<CBDiscoveredPeripheralEventArgs> DiscoveredPeripheral;

        

        private static ObservableCollection<BluetoothDevice> _foundDevices = new ObservableCollection<BluetoothDevice>();
               

#if __IOS__
        internal static event EventHandler<CBPeripheral[]> OnRetrievedPeripherals;
#endif

        private static Task<bool> PlatformGetAvailability()
        {
            Initialize();
            var available = false;
            
            if (_manager != null)
            {
                Debug.WriteLine($"GetAvailability:{_manager.State}");

                switch (_manager.State)
                {
                    case CBManagerState.PoweredOn:
                    case CBManagerState.Resetting:
                        available = true;
                        break;

                    case CBManagerState.Unknown:
                        // Unknown is returned on first creation of the manager - need to wait for first UpdatedState callback
                        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                        _manager.UpdatedState += StateHandler;
                        return tcs.Task;

                        void StateHandler(object? sender, EventArgs e)
                        {
                            if (_manager.State != CBManagerState.Unknown)
                            {
                                _manager.UpdatedState -= StateHandler;
                                tcs.SetResult(IsAvailable);
                            }
                        }
                }
            }

            return Task.FromResult(available);
        }

#if __IOS__
        private static UIAlertController _controller = null;
#endif

        internal static CBUUID[] GetUuidsForFilters(RequestDeviceOptions options)
        {
            var uuids = new List<CBUUID>();

            if (options is { AcceptAllDevices: false })
            {
                foreach (BluetoothLEScanFilter filter in options.Filters)
                {
                    foreach (BluetoothUuid service in filter.Services)
                    {
                        uuids.Add(service);
                    }
                }
            }

            return uuids.ToArray();
        }
        
        private static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            Initialize();

            if (!IsAvailable)
                return null;

#if __IOS__
            TaskCompletionSource<BluetoothDevice> tcs = new TaskCompletionSource<BluetoothDevice>();

            _controller = UIAlertController.Create("Select a Bluetooth accessory", null, UIAlertControllerStyle.Alert);
            _controller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (a)=> {
                tcs.SetResult(null);
                StopScanning();
                Debug.WriteLine(a == null ? "<null>" : a.ToString());
            }));
            
            CGRect rect = new CGRect(0, 0, 272, 272);
            var tvc = new UITableViewController(UITableViewStyle.Plain)
            {
                PreferredContentSize = rect.Size
            };
            _controller.PreferredContentSize = rect.Size;
            var source = new InTheHand.Bluetooth.Platforms.Apple.BluetoothTableViewSource(options);
            source.DeviceSelected += (s, e) =>
             {
                 tvc.DismissViewController(true, null);
                 tcs.SetResult(e);
             };

            tvc.TableView.Delegate = source;
            tvc.TableView.DataSource = source;

            tvc.TableView.UserInteractionEnabled = true;
            tvc.TableView.AllowsSelection = true;
            _controller.SetValueForKey(tvc, new Foundation.NSString("contentViewController"));

            //TODO: investigate what this means for multiple windows e.g. iPad
            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;

            currentController.PresentViewController(_controller, true, null);

            return await tcs.Task;
#else
            return null;
#endif
        }

        private static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options,
            CancellationToken cancellationToken = default)
        {
            var discoveredDevices = new List<BluetoothDevice>();
            var services = GetUuidsForFilters(options);

            DiscoveredPeripheral += (sender, args) =>
            {
                if (!args.AdvertisementData.ContainsKey(new NSString("kCBAdvDataIsConnectable")) ||
                    !args.AdvertisementData["kCBAdvDataIsConnectable"].Equals(NSNumber.FromBoolean(true))) return;

                var device = args.Peripheral;

                if (!string.IsNullOrEmpty(device.Name))
                {

                    var shouldAdd = true;

                    foreach (var filter in options?.Filters)
                    {
                        if (!string.IsNullOrEmpty(filter.Name) && !device.Name.Equals(filter.Name) ||
                            !string.IsNullOrEmpty(filter.NamePrefix) && !device.Name.StartsWith(filter.NamePrefix))
                        {
                            shouldAdd = false;
                            break;
                        }
                    }
                        
                    foreach (var existingDevice in discoveredDevices.Where(existingDevice =>
                                 ((CBPeripheral)existingDevice).Identifier.Equals(device.Identifier)))
                    {
                        shouldAdd = false;
                    }

                    if (shouldAdd)
                    {
                        discoveredDevices.Add(device);
                    }
                }
            };

            StartScanning(services);

            await Task.Run(async () => { await Task.Delay(options?.Timeout ?? TimeSpan.FromSeconds(5), cancellationToken); }, cancellationToken);
            StopScanning();
            return discoveredDevices.AsReadOnly();
        }

        static async Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            Initialize();

            if (!IsAvailable)
                return null;
#if __IOS__
            PairedDeviceHandler deviceHandler = new PairedDeviceHandler();
            OnRetrievedPeripherals += deviceHandler.OnRetrievedPeripherals;
            var devices = new List<BluetoothDevice>();
            var periphs = _manager.RetrieveConnectedPeripherals(GattServiceUuids.GenericAccess, GattServiceUuids.GenericAttribute, GattServiceUuids.DeviceInformation, GattServiceUuids.Battery);
            foreach (var p in periphs)
            {
                devices.Add(p);
            }

            return devices.AsReadOnly();
#else
            return (IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly();
#endif
        }

#if __IOS__
        private class PairedDeviceHandler
        {
            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.ManualReset);
            List<BluetoothDevice> devices = new List<BluetoothDevice>();

            public IReadOnlyCollection<BluetoothDevice> Devices
            {
                get
                {
                    return devices.AsReadOnly();
                }
            }

            public void WaitOne()
            {
                handle.WaitOne();
            }

            public void OnRetrievedPeripherals(object sender, CBPeripheral[] peripherals)
            {
                foreach (var peripheral in peripherals)
                {
                    devices.Add(peripheral);
                }

                handle.Set();
            }
        }
#endif

        private static async Task<BluetoothLEScan> PlatformRequestLEScan(BluetoothLEScanOptions options)
        {
            Initialize();

            if (!await PlatformGetAvailability())
                return null;

            return new BluetoothLEScan(options);
        }

        private static int _scanCount = 0;
        internal static void StartScanning(CBUUID[] services)
        {
            _scanCount++;
            Debug.WriteLine($"StartScanning count:{_scanCount}");

            if (!_manager.IsScanning)
                _manager.ScanForPeripherals(services, new PeripheralScanningOptions { AllowDuplicatesKey = false });
        }

        internal static void StopScanning()
        {
            _scanCount--;

            Debug.WriteLine($"StopScanning count:{_scanCount}");
            if (_scanCount == 0 && _manager.IsScanning)
                _manager.StopScan();
        }

        
        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await PlatformGetAvailability();
            _manager.UpdatedState += Manager_UpdatedState;
        }

        private static void RemoveAvailabilityChanged()
        {
            _manager.UpdatedState -= Manager_UpdatedState;
        }


    }
}
