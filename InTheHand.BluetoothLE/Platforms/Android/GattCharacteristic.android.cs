using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        private Android.Bluetooth.BluetoothGattCharacteristic _characteristic;

        internal GattCharacteristic(BluetoothRemoteGATTService service, Android.Bluetooth.BluetoothGattCharacteristic characteristic) : this(service)
        {
            _characteristic = characteristic;
        }

        Guid GetUuid()
        {
            return new Guid(_characteristic.Uuid.ToString());
        }

        GattCharacteristicProperties GetProperties()
        {
            return (GattCharacteristicProperties)(int)_characteristic.Properties;
        }

        Task<GattDescriptor> DoGetDescriptor(Guid descriptor)
        {
            var uuid = Java.Util.UUID.FromString(descriptor.ToString());
            Guid g = new Guid(uuid.ToString());
            var gattDescriptor = _characteristic.GetDescriptor(uuid);
            if (gattDescriptor is null)
                return Task.FromResult<GattDescriptor>(null);

            return Task.FromResult(new GattDescriptor(this, gattDescriptor));
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult(_characteristic.GetValue());
        }

        Task<byte[]> DoReadValue()
        {
            return Task.Run(() =>
            {
                bool read = Service.Device.Gatt.NativeGatt.ReadCharacteristic(_characteristic);
                Service.Device.Gatt.WaitForCharacteristicRead();

                return DoGetValue();
            });
        }

        Task DoWriteValue(byte[] value)
        {
            _characteristic.SetValue(value);
            bool written = Service.Device.Gatt.NativeGatt.WriteCharacteristic(_characteristic);
            Service.Device.Gatt.WaitForCharacteristicWrite();
            return Task.CompletedTask;
        }

        void AddCharacteristicValueChanged()
        {
        }
        
        void RemoveCharacteristicValueChanged()
        {
        }
    }
}
