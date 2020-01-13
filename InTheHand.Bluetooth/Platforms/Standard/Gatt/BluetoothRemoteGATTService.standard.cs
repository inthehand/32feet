using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class BluetoothRemoteGATTService
    {
        Guid GetUuid()
        {
            return Guid.Empty;
        }

        bool GetIsPrimary()
        {
            return true;
        }

        Task<GattCharacteristic> DoGetCharacteristic(Guid characteristic)
        {
            return Task.FromResult((GattCharacteristic)null);
        }

        Task<IReadOnlyList<GattCharacteristic>> DoGetCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

            return Task.FromResult((IReadOnlyList<GattCharacteristic>)characteristics.AsReadOnly());
        }
    }
}