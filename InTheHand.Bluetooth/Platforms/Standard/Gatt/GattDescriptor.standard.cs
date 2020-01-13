using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth.GenericAttributeProfile
{
    partial class GattDescriptor
    {
        Guid GetUuid()
        {
            return Guid.Empty;
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
    }
}