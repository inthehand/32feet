using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattCharacteristic
    {
        Guid GetUuid()
        {
            return Guid.Empty;
        }

        GattCharacteristicProperties GetProperties()
        {
            return 0;
        }

        Task<GattDescriptor> DoGetDescriptor(Guid descriptor)
        {
            return Task.FromResult((GattDescriptor)null);
        }

        Task<byte[]> DoGetValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task<byte[]> DoReadValue()
        {
            return Task.FromResult<byte[]>(null);
        }

        Task DoWriteValue(byte[] value)
        {
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
