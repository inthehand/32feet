using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTheHand.Bluetooth
{
    partial class Bluetooth
    {
        static Task<bool> DoGetAvailability()
        {
            return Task.FromResult(false);
        }

        static Task<BluetoothDevice> PlatformRequestDevice(RequestDeviceOptions options)
        {
            return Task.FromResult((BluetoothDevice)null);
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformScanForDevices(RequestDeviceOptions options)
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

        static Task<IReadOnlyCollection<BluetoothDevice>> PlatformGetPairedDevices()
        {
            return Task.FromResult((IReadOnlyCollection<BluetoothDevice>)new List<BluetoothDevice>().AsReadOnly());
        }

#if DEBUG
        private static async Task<BluetoothLEScan> DoRequestLEScan(BluetoothLEScanFilter filter)
        {
            return null;
        }
#endif

        private static void AddAvailabilityChanged()
        {
        }

        private static void RemoveAvailabilityChanged()
        {
        }
    }
}
