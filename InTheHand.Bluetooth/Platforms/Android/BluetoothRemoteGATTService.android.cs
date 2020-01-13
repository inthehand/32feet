using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        private Android.Bluetooth.BluetoothGattService _service;

        internal BluetoothRemoteGATTService(BluetoothDevice device, Android.Bluetooth.BluetoothGattService service) : this(device)
        {
            if (service is null)
                throw new ArgumentNullException("service");

            _service = service;
        }

        Guid GetUuid()
        {
            return new Guid(_service.Uuid.ToString());
        }

        bool GetIsPrimary()
        {
            return _service.Type == Android.Bluetooth.GattServiceType.Primary;
        }

        Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            var nativeCharacteristic = _service.GetCharacteristic(Java.Util.UUID.FromString(characteristic.ToString()));
            if (nativeCharacteristic is null)
                return Task.FromResult((GattCharacteristic)null);

            return Task.FromResult(new GattCharacteristic(this, nativeCharacteristic));
        }

        Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            foreach(var characteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, characteristic));
            }
            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }
    }
}