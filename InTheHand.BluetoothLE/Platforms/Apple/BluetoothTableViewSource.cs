//-----------------------------------------------------------------------
// <copyright file="BluetoothTableViewSource.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

#if __IOS__

using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using CoreBluetooth;
using CoreFoundation;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.Platforms.Apple
{
    internal sealed class BluetoothTableViewSource : UITableViewSource, IUITableViewDataSource, IUITableViewDelegate
    {
        private RequestDeviceOptions _options;
        private UITableView _table;
        private List<CBPeripheral> _devices = new List<CBPeripheral>();

        public BluetoothTableViewSource(RequestDeviceOptions options)
        {
            _options = options;

            Bluetooth.DiscoveredPeripheral += Bluetooth_DiscoveredPeripheral;
            
            if (Bluetooth._manager.State == CBCentralManagerState.PoweredOn)
            {
                StartScanning();
            }
            else
            {
                Bluetooth.UpdatedState += _manager_UpdatedState;
            }
        }

        private void Bluetooth_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Peripheral.Name) && !_devices.Contains(e.Peripheral))
            {
                System.Diagnostics.Debug.WriteLine(e.Peripheral.Name + " " + e.Peripheral.Identifier.ToString());

                UIDevice.CurrentDevice.BeginInvokeOnMainThread(() =>
                {
                    _devices.Add(e.Peripheral);
                    OnReloadData();
                });
            }
        }

        private async void _manager_UpdatedState(object sender, EventArgs e)
        {
            if (Bluetooth._manager.State == CBCentralManagerState.PoweredOn)
            {
                if(!Bluetooth._manager.IsScanning)
                    await StartScanning();
            }
        }

        private async Task StartScanning()
        {
            await Task.Delay(300);

            List<CBUUID> services = new List<CBUUID>();
            if (_options != null)
            {
                if (!_options.AcceptAllDevices)
                {
                    foreach (BluetoothLEScanFilter filter in _options.Filters)
                    {
                        foreach (BluetoothUuid service in filter.Services)
                        {
                            services.Add(service);
                        }
                    }
                }
            }

            Bluetooth.StartScanning(services.ToArray());
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return false;
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.None;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, null);// "bluetoothDevice");
            cell.TextLabel.Text = string.IsNullOrEmpty(_devices[indexPath.Row].Name) ? _devices[indexPath.Row].Identifier.ToString() : _devices[indexPath.Row].Name;
            //cell.DetailTextLabel.Text = _devices[indexPath.Row].Identifier.ToString();
            return cell;
        }

        public event EventHandler<BluetoothDevice> DeviceSelected;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var device = _devices[indexPath.Row];
            DeviceSelected?.Invoke(this, device);
            StopDiscovery();
        }

        public void StopDiscovery()
        {
            Bluetooth.StopScanning();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            _table = tableview;

            return _devices.Count;
        }

        private void OnReloadData()
        {
            if(_table != null)
            {
                _table.ReloadData();
            }
        }

        /*private sealed class BluetoothDelegate : CBCentralManagerDelegate
        {
            private BluetoothTableViewSource _owner;

            public BluetoothDelegate(BluetoothTableViewSource owner)
            {
                _owner = owner;
            }

            public override void UpdatedState(CBCentralManager central)
            {
                if (central.State == CBCentralManagerState.PoweredOn)
                {
                    List<CBUUID> services = new List<CBUUID>();
                    if (!_owner._options.AcceptAllDevices)
                    {
                        foreach (BluetoothLEScanFilter filter in _owner._options.Filters)
                        {
                            foreach (BluetoothUuid service in filter.Services)
                            {
                                services.Add(service);
                            }
                        }
                    }

                    central.ScanForPeripherals(services.ToArray(), new PeripheralScanningOptions() { AllowDuplicatesKey = false });
                }
            }

            public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
            {
                foreach(var item in advertisementData)
                {
                    if (item.Key.ToString() == CBAdvertisement.DataLocalNameKey)
                    {
                        System.Diagnostics.Debug.WriteLine($"{item.Key}  {item.Value}");
                    }
                }

                //if (!string.IsNullOrEmpty(peripheral.Name))
                //{
                    System.Diagnostics.Debug.WriteLine(peripheral.Name + " " + peripheral.Identifier.ToString());
                if (!_owner._devices.Contains(peripheral))
                {
                    _owner._devices.Add(peripheral);
                }

                

                UIDevice.CurrentDevice.BeginInvokeOnMainThread(() =>
                    {
                        _owner.OnReloadData();
                    });
                //}
            }
        }*/
    }
}
#endif