//-----------------------------------------------------------------------
// <copyright file="Bluetooth.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !__MACOS__
using UIKit;
#endif

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static CBCentralManager _manager;
        private static BluetoothDelegate _delegate = new BluetoothDelegate();
        private static bool availability = false;

        static Bluetooth()
        {
            Debug.WriteLine("Bluetooth_ctor");
            CBCentralInitOptions options = new CBCentralInitOptions { ShowPowerAlert = true
#if __IOS__
//                ,  RestoreIdentifier = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName")?.ToString()
#endif
            };

            _manager = new CBCentralManager(_delegate, DispatchQueue.MainQueue, options);
            //_manager.UpdatedState += _manager_UpdatedState;
#if __IOS__
            //_manager.DiscoveredPeripheral += _manager_DiscoveredPeripheral;
            //_manager.RetrievedConnectedPeripherals += _manager_RetrievedConnectedPeripherals;
#endif
        }

        internal static event EventHandler<CBPeripheral> ConnectedPeripheral;
        internal static event EventHandler<CBPeripheral> DisconnectedPeripheral;
        internal static event EventHandler<CBPeripheral> FailedToConnectPeripheral;

#if __IOS__
        private static void _manager_RetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void _manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            throw new NotImplementedException();
        }
#endif
        private sealed class BluetoothDelegate : CBCentralManagerDelegate
        {

            internal BluetoothDelegate()
            {
            }

            public override void UpdatedState(CBCentralManager central)
            {
                Debug.WriteLine(central.State);

                bool newAvailability = false;

#if !__WATCHOS__
                switch(central.State)
                {
                    case CBCentralManagerState.PoweredOn:
                    case CBCentralManagerState.Resetting:
                        newAvailability = true;
                        break;

                    default:
                        newAvailability = false;
                        break;
                }
#endif

                if(availability != newAvailability)
                {
                    availability = newAvailability;
                    OnAvailabilityChanged();
                }
            }

            public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
            {
                System.Diagnostics.Debug.WriteLine($"Connected {peripheral.Identifier}");
                Bluetooth.ConnectedPeripheral?.Invoke(central, peripheral);
            }

            public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
            {
                System.Diagnostics.Debug.WriteLine($"Disconnected {peripheral.Identifier} {error}");
                Bluetooth.DisconnectedPeripheral?.Invoke(central, peripheral);
            }

            public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to connect {peripheral.Identifier} {error.Code}");
                Bluetooth.FailedToConnectPeripheral?.Invoke(central, peripheral);
            }

            public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
            {
                OnDiscoveredPeripheral(central, peripheral, advertisementData, RSSI);
            }

#if __IOS__
            public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
            {
                base.RetrievedConnectedPeripherals(central, peripherals);
            }

            public override void WillRestoreState(CBCentralManager central, NSDictionary dict)
            {
                base.WillRestoreState(central, dict);
            }
#endif
        }

        private static ObservableCollection<BluetoothDevice> _foundDevices = new ObservableCollection<BluetoothDevice>();

        private static void OnDiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            
            System.Diagnostics.Debug.WriteLine($"{peripheral.Identifier} {RSSI}");
            _foundDevices.Add(peripheral);
#if __IOS__
            if(controller !=null)
            {
                controller.AddTextField((t) => { t.Text = peripheral.Name; });
            }
#endif
        }

#if __IOS__
        internal static event EventHandler<CBPeripheral[]> OnRetrievedPeripherals;


#endif


        static Task<bool> DoGetAvailability()
        {
            bool available = false;

            switch(_manager.State)
            {
#if __WATCHOS__
                case CBManagerState.PoweredOn:
                case CBManagerState.Resetting:
#else
                case CBCentralManagerState.PoweredOn:
                case CBCentralManagerState.Resetting:             
#endif
                    available = true;
                    break;
            }

            return Task.FromResult(available);
        }

#if __IOS__
        private static UIAlertController controller = null;
#endif
        static async Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
#if __IOS__
            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.ManualReset);

            if(_manager.State != CBCentralManagerState.PoweredOn)
            {
                throw new InvalidOperationException();
            }

            BluetoothDevice selectedDevice = null;

            controller = UIAlertController.Create("Select a Bluetooth accessory", null, UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (a)=> {
                handle.Set();
                System.Diagnostics.Debug.WriteLine(a == null ? "<null>" : a.ToString());
            }));
            
            CGRect rect = new CGRect(0, 0, 272, 272);
            var tvc = new UITableViewController(UITableViewStyle.Plain)
            {
                PreferredContentSize = rect.Size
            };
            controller.PreferredContentSize = rect.Size;
            var source = new InTheHand.Bluetooth.Platforms.Apple.BluetoothTableViewSource(options);
            source.DeviceSelected += (s, e) =>
             {
                 selectedDevice = e;
                 handle.Set();
                 tvc.DismissViewController(true, null);
             };

            tvc.TableView.Delegate = source;
            tvc.TableView.DataSource = source;

            tvc.TableView.UserInteractionEnabled = true;
            tvc.TableView.AllowsSelection = true;
            //controller.AddChildViewController(contentController);
            controller.SetValueForKey(tvc, new Foundation.NSString("contentViewController"));

            UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (currentController.PresentedViewController != null)
                currentController = currentController.PresentedViewController;

            currentController.PresentViewController(controller, true, null);
            
            return await Task.Run(() =>
            {
                var s2 = source;
                handle.WaitOne();
                return selectedDevice;
            });
#endif
            return null;
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
#if __IOS__
            PairedDeviceHandler deviceHandler = new PairedDeviceHandler();
            OnRetrievedPeripherals += deviceHandler.OnRetrievedPeripherals;
            List<BluetoothDevice> devices = new List<BluetoothDevice>();
            var periphs = _manager.RetrieveConnectedPeripherals(CBUUID.FromPartial(0x1801));
            foreach (var p in periphs)
            {
                devices.Add(p);
            }

            return Task.Run(() =>
                        {
                            return (IReadOnlyCollection<BluetoothDevice>)devices.AsReadOnly();
                        });
#else
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
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

#if DEBUG
        private static async Task<BluetoothLEScan> DoRequestLEScan(BluetoothLEScanFilter filter)
        {
            return null;
        }
#endif

        private static bool _oldAvailability;

        private static async void AddAvailabilityChanged()
        {
            _oldAvailability = await DoGetAvailability();
            _manager.UpdatedState += _manager_UpdatedState;
        }

        private static void RemoveAvailabilityChanged()
        {
            _manager.UpdatedState -= _manager_UpdatedState;
        }

        private static async void _manager_UpdatedState(object sender, EventArgs e)
        {
            bool newAvailability = await DoGetAvailability();
            if (newAvailability != _oldAvailability)
            {
                _oldAvailability = newAvailability;
                OnAvailabilityChanged();
            }
        }
    }
}
