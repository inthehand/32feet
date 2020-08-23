using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        Task<bool> DoGetAvailability()
        {
            return Task.FromResult(false);
        }

        Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromResult((BluetoothDevice)null);
        }

        Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

        Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

#if DEBUG
        private async Task<BluetoothLEScan> DoRequestLEScan(BluetoothLEScanFilter filter)
        {
            return null;
        }
#endif

        private void AddAvailabilityChanged()
        {
        }

        private void RemoveAvailabilityChanged()
        {
        }
    }
}
