//-----------------------------------------------------------------------
// <copyright file="GattCharacteristic.unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2018-20 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class GattCharacteristic
    {
        private CBCharacteristic _characteristic;

        internal GattCharacteristic(GattService service, CBCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        BluetoothUuid GetUuid()
        {
            return _characteristic.UUID;
        }

        GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)((int)_characteristic.Properties & 0xff);
        }

        string GetUserDescription()
        {
            return GetManualUserDescription();
        }

        Task<GattDescriptor> PlatformGetDescriptor(BluetoothUuid descriptor)
        {
            GattDescriptor matchingDescriptor = null;
            ((CBPeripheral)Service.Device).DiscoverDescriptors(_characteristic);
            foreach (CBDescriptor cbdescriptor in _characteristic.Descriptors)
            {
                if ((BluetoothUuid)cbdescriptor.UUID == descriptor)
                {
                    matchingDescriptor = new GattDescriptor(this, cbdescriptor);
                    break;
                }
            }

            return Task.FromResult(matchingDescriptor);
        }

        async Task<IReadOnlyList<GattDescriptor>> PlatformGetDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();
            ((CBPeripheral)Service.Device).DiscoverDescriptors(_characteristic);
            foreach (CBDescriptor cbdescriptor in _characteristic.Descriptors)
            {
                descriptors.Add(new GattDescriptor(this, cbdescriptor));

            }

            return descriptors;
        }

        byte[] PlatformGetValue()
        {
            return _characteristic.Value.ToArray();
        }

        Task<byte[]> PlatformReadValue()
        {
            ((CBPeripheral)Service.Device).ReadValue(_characteristic);
            return Task.FromResult(_characteristic.Value.ToArray());
        }

        Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            TaskCompletionSource<bool> tcs = null;
            CBPeripheral peripheral = Service.Device;

            if (requireResponse)
            {
                tcs = new TaskCompletionSource<bool>();

                void handler(object s, CBCharacteristicEventArgs e)
                {
                    if (e.Characteristic == _characteristic)
                    {
                        peripheral.WroteCharacteristicValue -= handler;

                        if (!tcs.Task.IsCompleted)
                        {
                            tcs.SetResult(e.Error == null);
                        }
                    }
                };

                peripheral.WroteCharacteristicValue += handler;
            }

            CBCharacteristicWriteType writeType = requireResponse ? CBCharacteristicWriteType.WithResponse : CBCharacteristicWriteType.WithoutResponse;

            if (!requireResponse && !peripheral.CanSendWriteWithoutResponse)
                writeType = CBCharacteristicWriteType.WithResponse;

            ((CBPeripheral)Service.Device).WriteValue(NSData.FromArray(value), _characteristic, writeType);

            if (requireResponse)
                return tcs.Task;

            return Task.CompletedTask;
        }

        void AddCharacteristicValueChanged()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.UpdatedCharacterteristicValue += Peripheral_UpdatedCharacterteristicValue;
        }

        void Peripheral_UpdatedCharacterteristicValue(object sender, CBCharacteristicEventArgs e)
        {
            if(e.Characteristic == _characteristic)
                characteristicValueChanged?.Invoke(this, EventArgs.Empty);
        }

        void RemoveCharacteristicValueChanged()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.UpdatedCharacterteristicValue -= Peripheral_UpdatedCharacterteristicValue;
            
        }

        private Task PlatformStartNotifications()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.SetNotifyValue(true, _characteristic);
            return Task.CompletedTask;
        }

        private Task PlatformStopNotifications()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.SetNotifyValue(false, _characteristic);
            return Task.CompletedTask;
        }
    }
}
