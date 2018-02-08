//-----------------------------------------------------------------------
// <copyright file="BluetoothAdapter.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;
using CoreBluetooth;
using Foundation;
using InTheHand.Devices.Enumeration;
using System.Collections.Generic;
using System.Diagnostics;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothAdapter
    {
        private static Task<BluetoothAdapter> GetDefaultAsyncImpl()
        {
            return Task.Run<BluetoothAdapter>(() =>
            {
                if (s_default == null)
                {
                    s_default = new BluetoothAdapter();
                }

                return s_default;
            });
        }

        private CBCentralManager _manager;

        internal BluetoothAdapter()
        {
            _manager = new CBCentralManager(new CentralManagerDelegate(), CoreFoundation.DispatchQueue.MainQueue);
        }

        internal CBCentralManager Manager
        {
            get
            {
                return _manager;
            }
        }

        internal event EventHandler<CBPeripheral> ConnectionStateChanged;
        internal event EventHandler<DeviceInformation> DiscoveredDevice;
        internal event EventHandler StateChanged;

        private sealed class CentralManagerDelegate : CBCentralManagerDelegate
        {
            public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
            {
                foreach (KeyValuePair<NSObject, NSObject> kvp in advertisementData)
                {
                    Debug.WriteLine(kvp.Key.ToString() + " " + kvp.Value.ToString());
                }

                Debug.WriteLine(RSSI.ToString());
                string name = advertisementData.ContainsKey(new NSString("kCBAdvDataLocalName")) ? advertisementData["kCBAdvDataLocalName"].ToString() : peripheral.Name;
                s_default.DiscoveredDevice?.Invoke(this, new DeviceInformation(peripheral, name));
            }

            public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
            {
                s_default.ConnectionStateChanged?.Invoke(this, peripheral);
            }

            public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
            {
            }

            public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
            {
                s_default.ConnectionStateChanged?.Invoke(this, peripheral);
            }
#if !__TVOS__
            public override void RetrievedConnectedPeripherals(CBCentralManager central, CBPeripheral[] peripherals)
            {
            }
#endif
            public override void UpdatedState(CBCentralManager central)
            {
                s_default.StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private ulong GetBluetoothAddress()
        {
            return 0;
        }
        
        private BluetoothClassOfDevice GetClassOfDevice()
        {
            return new BluetoothClassOfDevice(0);
        }

        private bool GetIsClassicSupported()
        {
            return false;
        }

        private bool GetIsLowEnergySupported()
        {
            return true;
        }

        private string GetName()
        {
#if __IOS__
            return UIKit.UIDevice.CurrentDevice.Name;

#elif __MAC__
            return global::System.Environment.MachineName;
#else
            return string.Empty;
#endif
        }
    }
}