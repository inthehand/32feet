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

        Task<byte[]> PlatformGetValue()
        {
            return Task.FromResult(_characteristic.Value.ToArray());
        }

        Task<byte[]> PlatformReadValue()
        {
            ((CBPeripheral)Service.Device).ReadValue(_characteristic);
            return Task.FromResult(_characteristic.Value.ToArray());
        }

        Task PlatformWriteValue(byte[] value, bool requireResponse)
        {
            ((CBPeripheral)Service.Device).WriteValue(NSData.FromArray(value), _characteristic, requireResponse ? CBCharacteristicWriteType.WithResponse : CBCharacteristicWriteType.WithoutResponse);
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
