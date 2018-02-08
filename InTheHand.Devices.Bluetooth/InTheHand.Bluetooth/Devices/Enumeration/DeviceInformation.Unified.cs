//-----------------------------------------------------------------------
// <copyright file="DeviceInformation.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.Devices.Bluetooth;
using CoreBluetooth;
using Foundation;
using System.Threading;
using System.Diagnostics;

namespace InTheHand.Devices.Enumeration
{
    partial class DeviceInformation
    {
        internal CBPeripheral _peripheral;
        private string _name;

        private static EventWaitHandle stateHandle;
        private static EventWaitHandle retrievedHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        internal static List<DeviceInformation> _devices = new List<DeviceInformation>();

        static DeviceInformation()
        {
            stateHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        internal DeviceInformation(CBPeripheral peripheral, string name)
        {
            _peripheral = peripheral;
            if (!string.IsNullOrEmpty(name))
            {
                _name = name;
            }
            else
            {
                _name = peripheral.Name;
            }
        }

        public static implicit operator CBPeripheral(DeviceInformation deviceInformation)
        {
            return deviceInformation._peripheral;
        }

        public static implicit operator DeviceInformation(CBPeripheral peripheral)
        {
            return new Enumeration.DeviceInformation(peripheral, null);
        }

        private static async Task FindAllAsyncImpl(string aqsFilter, List<DeviceInformation> list)
        {
            await Task.Run<IReadOnlyCollection<DeviceInformation>>(async () =>
            {
                if (BluetoothAdapter.Default.Manager.State != CBCentralManagerState.PoweredOn)
                {
                    stateHandle.WaitOne();
                }

                string[] filterParts = aqsFilter.Split(':');
                bool discover = true;
                Guid service = Guid.Empty;

                if (filterParts.Length == 2)
                {
                    switch (filterParts[0])
                    {
                        case "bluetoothPairing":
                            discover = !bool.Parse(filterParts[1]);
                            break;

                        case "bluetoothService":
                            service = Guid.Parse(filterParts[1]);
                            break;


                    }
                }
                //CBPeripheral[] peripherals = _manager.RetrieveConnectedPeripherals(CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()));

                /*CBPeripheral[] peripherals = _manager.RetrievePeripheralsWithIdentifiers(null);// new CBUUID[] { CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAttribute.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.GenericAccess.ToByteArray()), CBUUID.FromBytes(InTheHand.Devices.Bluetooth.GenericAttributeProfile.GattServiceUuids.Battery.ToByteArray()) });

             
                foreach (CBPeripheral p in peripherals)
                    {
                        _devices.Add(new DeviceInformation(p));
                    }*/
                //retrievedHandle.WaitOne();
                if (discover)
                {
                    BluetoothAdapter.Default.DiscoveredDevice += Default_DiscoveredDevice;
                    if (service != Guid.Empty)
                    {
                        BluetoothAdapter.Default.Manager.ScanForPeripherals(new CBUUID[] { CBUUID.FromBytes(service.ToByteArray()) });
                    }
                    else
                    {
                        BluetoothAdapter.Default.Manager.ScanForPeripherals(new CBUUID[] { });
                    }
                    await Task.Delay(5000);
                    BluetoothAdapter.Default.DiscoveredDevice -= Default_DiscoveredDevice;
                    BluetoothAdapter.Default.Manager.StopScan();
                }
                else
                {
                    var peripherals = BluetoothAdapter.Default.Manager.RetrieveConnectedPeripherals(CBUUID.FromBytes(service.ToByteArray()));
                    foreach (CBPeripheral p in peripherals)
                    {
                        _devices.Add(new DeviceInformation(p, p.Name));
                    }
                }

                return _devices.AsReadOnly();
            });
        }

        private static void Default_DiscoveredDevice(object sender, DeviceInformation e)
        {
            _devices.Add(e);
        }

        private string GetId()
        {
            return _peripheral.Identifier.AsString();
        }

        private string GetName()
        {
            if (string.IsNullOrEmpty(_peripheral.Name))
            {
                return _name;
            }

            return _peripheral.Name;
        }
    }
}