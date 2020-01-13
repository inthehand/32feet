using System;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal sealed class Win32BluetoothDeviceInfo
    {
        private BLUETOOTH_DEVICE_INFO _info;

        internal Win32BluetoothDeviceInfo(BLUETOOTH_DEVICE_INFO info)
        {
            _info = info;
        }

        public Win32BluetoothDeviceInfo(BluetoothAddress address)
        {
            _info = BLUETOOTH_DEVICE_INFO.Create();
            _info.Address = address;
            NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref _info);
        }

        public BluetoothAddress DeviceAddress => _info.Address;

        public string DeviceName => _info.szName.TrimEnd();

        public ClassOfDevice ClassOfDevice => (ClassOfDevice)_info.ulClassofDevice;

        public Guid[] InstalledServices
        {
            get
            {
                int serviceCount = 0;
                int result = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref _info, ref serviceCount, null);
                byte[] services = new byte[serviceCount * 16];
                result = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref _info, ref serviceCount, services);
                if (result < 0)
                    return new Guid[0];

                Guid[] foundServices = new Guid[serviceCount];
                byte[] buffer = new byte[16];

                for (int s = 0; s < serviceCount; s++)
                {
                    Buffer.BlockCopy(services, s * 16, buffer, 0, 16);
                    foundServices[s] = new Guid(buffer);
                }

                return foundServices;
            }
        }

        public void SetServiceState(Guid service, bool state)
        {
            int result = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref _info, ref service, state ? 1u : 0);
        }

        public bool Connected => _info.fConnected;

        public bool Authenticated => _info.fAuthenticated;

        public void Refresh()
        {
            NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref _info);
        }
    }
}
