using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InTheHand.Net.Bluetooth.Win32
{
    internal sealed class Win32BluetoothSecurity
    {
        private List<Win32BluetoothWin32Authentication> _authenticationHandlers = new List<Win32BluetoothWin32Authentication>();

        public bool PairRequest(BluetoothAddress device, string pin)
        {
            BLUETOOTH_DEVICE_INFO info = new BLUETOOTH_DEVICE_INFO();
            info.dwSize = Marshal.SizeOf(info);
            info.Address = device;

            RemoveRedundantAuthHandler(device);

            // Handle response without prompt
            _authenticationHandlers.Add(new Win32BluetoothWin32Authentication(device, pin));

            bool success = NativeMethods.BluetoothAuthenticateDeviceEx(IntPtr.Zero, IntPtr.Zero, ref info, null, BluetoothAuthenticationRequirements.MITMProtectionNotRequired) == 0;

            Win32BluetoothDeviceInfo deviceInfo = new Win32BluetoothDeviceInfo(info);
            deviceInfo.Refresh();

            // On Windows 7 these services are not automatically activated
            if(deviceInfo.ClassOfDevice.Device == DeviceClass.AudioVideoHeadset || deviceInfo.ClassOfDevice.Device == DeviceClass.AudioVideoHandsFree)
            {
                deviceInfo.SetServiceState(BluetoothService.Headset, true);
                deviceInfo.SetServiceState(BluetoothService.Handsfree, true);
            }

            return success;
        }

        private void RemoveRedundantAuthHandler(ulong address)
        {
            Win32BluetoothWin32Authentication redundantAuth = null;

            foreach (Win32BluetoothWin32Authentication authHandler in _authenticationHandlers)
            {
                if (authHandler.Address == address)
                {
                    redundantAuth = authHandler;
                    redundantAuth.Dispose();
                    break;
                }
            }

            if (redundantAuth != null)
            {
                _authenticationHandlers.Remove(redundantAuth);
            }
        }

        public bool RemoveDevice(BluetoothAddress device)
        {
            ulong addr = device;
            RemoveRedundantAuthHandler(device);
            return NativeMethods.BluetoothRemoveDevice(ref addr) == 0;
        }
    
    }
}
