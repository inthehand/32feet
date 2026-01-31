using System.Runtime.CompilerServices;

namespace IOBluetooth;

public partial class BluetoothDevice
{
    /// <summary>
    /// Get the Bluetooth device address for the target device.
    /// </summary>
    public unsafe BluetoothDeviceAddress Address => Unsafe.AsRef<BluetoothDeviceAddress>(GetAddress().ToPointer());

    public override string ToString()
    {
        return NameOrAddress;
    }
}