#if __IOS__

using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using CoreBluetooth;
using CoreFoundation;

namespace InTheHand.Bluetooth.Platforms.Apple
{
    internal sealed class BluetoothTableViewSource : UITableViewSource, IUITableViewDataSource, IUITableViewDelegate
    {
        private CBCentralManager _manager;
        private BluetoothDelegate _delegate;
        private RequestDeviceOptions _options;
        private UITableView _table;
        private List<CBPeripheral> _devices = new List<CBPeripheral>();

        public BluetoothTableViewSource(RequestDeviceOptions options)
        {
            _delegate = new BluetoothDelegate(this);
            _manager = new CBCentralManager(_delegate, DispatchQueue.MainQueue);
            _options = options;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, "bluetoothDevice");
            cell.TextLabel.Text = _devices[indexPath.Row].Name;
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
            if(_manager.IsScanning)
                _manager.StopScan();
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

        private sealed class BluetoothDelegate : CBCentralManagerDelegate
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
                if (!string.IsNullOrEmpty(peripheral.Name))
                {
                    System.Diagnostics.Debug.WriteLine(peripheral.Name + " " + peripheral.Identifier.ToString());
                    if (!_owner._devices.Contains(peripheral))
                    {
                        _owner._devices.Add(peripheral);
                    }

                    UIDevice.CurrentDevice.BeginInvokeOnMainThread(() =>
                    {
                        _owner.OnReloadData();
                    });
                }
            }
        }
    }
}
#endif