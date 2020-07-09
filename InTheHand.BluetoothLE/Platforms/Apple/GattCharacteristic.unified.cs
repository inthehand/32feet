using CoreBluetooth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private CBCharacteristic _characteristic;

        internal GattCharacteristic(BluetoothRemoteGATTService service, CBCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        public static implicit operator CBCharacteristic(GattCharacteristic characteristic)
        {
            return characteristic._characteristic;
        }

        Guid GetUuid()
        {
            return _characteristic.UUID.ToGuid();
        }

        GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)((int)_characteristic.Properties & 0xff);
        }

        string GetUserDescription()
        {
            return GetManualUserDescription();
        }

        Task<GattDescriptor> DoGetDescriptor(Guid descriptor)
        {
            GattDescriptor matchingDescriptor = null;
            ((CBPeripheral)Service.Device).DiscoverDescriptors(_characteristic);
            foreach (CBDescriptor cbdescriptor in _characteristic.Descriptors)
            {
                if (cbdescriptor.UUID.ToGuid() == descriptor)
                {
                    matchingDescriptor = new GattDescriptor(this, cbdescriptor);
                    break;
                }
            }

            return Task.FromResult(matchingDescriptor);
        }

        async Task<IReadOnlyList<GattDescriptor>> DoGetDescriptors()
        {
            List<GattDescriptor> descriptors = new List<GattDescriptor>();
            ((CBPeripheral)Service.Device).DiscoverDescriptors(_characteristic);
            foreach (CBDescriptor cbdescriptor in _characteristic.Descriptors)
            {
                descriptors.Add(new GattDescriptor(this, cbdescriptor));

            }

            return descriptors;
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult(_characteristic.Value.ToArray());
        }

        Task<byte[]> DoReadValue()
        {
            ((CBPeripheral)Service.Device).ReadValue(_characteristic);
            return Task.FromResult(_characteristic.Value.ToArray());
        }

        Task DoWriteValue(byte[] value)
        {
            ((CBPeripheral)Service.Device).WriteValue(NSData.FromArray(value), _characteristic, CBCharacteristicWriteType.WithResponse);
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

        private Task DoStartNotifications()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.SetNotifyValue(true, _characteristic);
            return Task.CompletedTask;
        }

        private Task DoStopNotifications()
        {
            CBPeripheral peripheral = Service.Device;
            peripheral.SetNotifyValue(false, _characteristic);
            return Task.CompletedTask;
        }
    }
}
