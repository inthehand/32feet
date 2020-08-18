using System;
using System.Threading.Tasks;
using Uap = Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Runtime.InteropServices.WindowsRuntime;

namespace InTheHand.Bluetooth
{
    partial class GattDescriptor
    {
        private Uap.GattDescriptor _descriptor;

        internal GattDescriptor(GattCharacteristic characteristic, Uap.GattDescriptor descriptor) : this(characteristic)
        {
            _descriptor = descriptor;
        }

        public static implicit operator Uap.GattDescriptor(GattDescriptor descriptor)
        {
            return descriptor._descriptor;
        }

        Guid GetUuid()
        {
            return _descriptor.Uuid;
        }

        async Task<byte[]> DoGetValue()
        {
            var result = await _descriptor.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Cached).AsTask();

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task<byte[]> DoReadValue()
        {
            var result = await _descriptor.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached).AsTask().ConfigureAwait(false);

            if (result.Status == Uap.GattCommunicationStatus.Success)
            {
                return result.Value.ToArray();
            }

            return null;
        }

        async Task DoWriteValue(byte[] value)
        {
            await _descriptor.WriteValueAsync(value.AsBuffer());
        }
    }
}