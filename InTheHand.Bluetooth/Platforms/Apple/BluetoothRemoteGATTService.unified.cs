using CoreBluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        private CBService _service;

        internal BluetoothRemoteGATTService(BluetoothDevice device, CBService service) : this(device)
        {
            _service = service;
        }

        public static implicit operator CBService(BluetoothRemoteGATTService service)
        {
            return service._service;
        }

        Guid GetUuid()
        {
            return _service.UUID.ToGuid();
        }

        bool GetIsPrimary()
        {
            return true;
        }

        Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            ((CBPeripheral)Device).DiscoverCharacteristics(new CBUUID[] { characteristic.ToCBUUID() }, _service);
            GattCharacteristic matchingCharacteristic = null;

            foreach(CBCharacteristic cbcharacteristic in _service.Characteristics)
            {
                if(cbcharacteristic.UUID.ToGuid() == characteristic)
                {
                    matchingCharacteristic = new GattCharacteristic(this, cbcharacteristic);
                    break;
                }
            }

            return Task.FromResult(matchingCharacteristic);
        }

        Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
            ((CBPeripheral)Device).DiscoverCharacteristics(_service);

            foreach (CBCharacteristic cbcharacteristic in _service.Characteristics)
            {
                characteristics.Add(new GattCharacteristic(this, cbcharacteristic));
            }

            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }
    }
}