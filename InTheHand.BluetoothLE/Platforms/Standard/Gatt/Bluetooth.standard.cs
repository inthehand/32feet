using System;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        Task<bool> DoGetAvailability()
        {
            return Task.FromResult(false);
        }

        Task<BluetoothDevice> DoRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromResult((BluetoothDevice)null);
        }

        private async Task DoRequestLEScan(BluetoothLEScan scan)
        {
        }

        private void AddAvailabilityChanged()
        {
        }

        private void RemoveAvailabilityChanged()
        {
        }
    }
}
