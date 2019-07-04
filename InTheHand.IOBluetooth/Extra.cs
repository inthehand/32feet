using System;
using System.Runtime.InteropServices;

namespace IOBluetooth
{
    public partial class BluetoothDevice
    {
        public BluetoothDeviceAddress Address
        {
            get
            {
                return (BluetoothDeviceAddress)Marshal.PtrToStructure(GetAddress(), typeof(IOBluetooth.BluetoothDeviceAddress));
            }
        }

        public override string ToString()
        {
            return this.NameOrAddress;
        }
    }
}
