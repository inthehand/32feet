//-----------------------------------------------------------------------
// <copyright file="Bluetooth.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
#if !__MACOS__
using UIKit;
#endif

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        internal static CBCentralManager _manager = new CBCentralManager();

        public Bluetooth()
        {
            _manager.DiscoveredPeripheral += _manager_DiscoveredPeripheral;
        }

        private ObservableCollection<BluetoothDevice> _foundDevices = new ObservableCollection<BluetoothDevice>();

        private void _manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Peripheral.Identifier);
            _foundDevices.Add(e.Peripheral);
#if __IOS__
            if(controller !=null)
            {
                controller.AddTextField((t) => { t.Text = e.Peripheral.Name; });
            }
#endif
        }

        Task<bool> DoGetAvailability()
        {
            bool supported = false;

#if __WATCHOS__
            supported = true;
#else
            switch(_manager.State)
            {
                case CBCentralManagerState.Resetting:
                case CBCentralManagerState.PoweredOn:
                    supported = true;
                    break;
            }
#endif
            return Task.FromResult(supported);
        }

#if __IOS__
        private UIAlertController controller = null;
#endif
        Task<BluetoothDevice> DoRequestDevice(RequestDeviceOptions options)
        {
#if __IOS__
            EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);
            BluetoothDevice selectedDevice = null;

            controller = UIAlertController.Create("Select a Bluetooth accessory", null, UIAlertControllerStyle.Alert);
            controller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (a)=> {
                handle.Set();
                System.Diagnostics.Debug.WriteLine(a == null ? "<null>" : a.ToString());
            }));
            
            CGRect rect = new CGRect(0, 0, 272, 272);
            var tvc = new UITableViewController(UITableViewStyle.Plain);
            tvc.PreferredContentSize = rect.Size;
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
            
            return Task.Run(() =>
            {
                handle.WaitOne();
                return selectedDevice;
            });
#endif
            return Task.FromResult((BluetoothDevice)null);
        }

        private async Task DoRequestLEScan(BluetoothLEScan scan)
        {
        }

        private bool _oldAvailability;

        private async void AddAvailabilityChanged()
        {
            _oldAvailability = await DoGetAvailability();
            _manager.UpdatedState += _manager_UpdatedState;
        }

        private void RemoveAvailabilityChanged()
        {
            _manager.UpdatedState -= _manager_UpdatedState;
        }

        private async void _manager_UpdatedState(object sender, EventArgs e)
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
