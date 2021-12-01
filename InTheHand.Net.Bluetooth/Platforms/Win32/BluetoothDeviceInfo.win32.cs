// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Win32)
// 
// Copyright (c) 2003-2021 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Collections.Generic;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothDeviceInfo
    {
        private BLUETOOTH_DEVICE_INFO _info;

        internal BluetoothDeviceInfo(BLUETOOTH_DEVICE_INFO info)
        {
            _info = info;
        }

        /// <summary>
        /// Initializes an instance of the BluetoothDeviceInfo class for the device with the given address.
        /// </summary>
        /// <param name="address">The BluetoothAddress.</param>
        public BluetoothDeviceInfo(BluetoothAddress address)
        {
            _info = BLUETOOTH_DEVICE_INFO.Create();
            _info.Address = address;
            DoRefresh();
        }

        BluetoothAddress GetDeviceAddress()
        {
            return _info.Address;
        }

        string GetDeviceName()
        {
            return _info.szName.TrimEnd();
        }

        ClassOfDevice GetClassOfDevice()
        {
            return (ClassOfDevice)_info.ulClassofDevice;
        }

        IReadOnlyCollection<Guid> GetInstalledServices()
        {
            int serviceCount = 0;
            int result = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref _info, ref serviceCount, null);
            byte[] services = new byte[serviceCount * 16];
            result = NativeMethods.BluetoothEnumerateInstalledServices(IntPtr.Zero, ref _info, ref serviceCount, services);
            if (result < 0)
                return new Guid[0];

            List<Guid> foundServices = new List<Guid>();
            byte[] buffer = new byte[16];

            for (int s = 0; s < serviceCount; s++)
            {
                Buffer.BlockCopy(services, s * 16, buffer, 0, 16);
                foundServices.Add(new Guid(buffer));
            }

            return foundServices.AsReadOnly();
        }

        void DoSetServiceState(Guid service, bool state)
        {
            int result = NativeMethods.BluetoothSetServiceState(IntPtr.Zero, ref _info, ref service, state ? 1u : 0);
        }

        bool GetConnected()
        {
            return _info.fConnected;
        }

        bool GetAuthenticated()
        {
            return _info.fAuthenticated;
        }

        /// <summary>
        /// Specifies whether the device is a remembered device.
        /// Not all remembered devices are authenticated.
        /// </summary>
        /// <remarks>Windows caches information about previously seen devices even if not authenticated.</remarks>
        public bool Remembered
        { 
            get
            {
                return _info.fRemembered;
            } 
        }

        void DoRefresh()
        {
            NativeMethods.BluetoothGetDeviceInfo(IntPtr.Zero, ref _info);
        }

        /// <summary>
        /// Date and Time this device was last seen by the system.
        /// </summary>
        public DateTime LastSeen
        {
            get
            {
                return _info.LastSeen;
            }
        }

        /// <summary>
        /// Date and Time this device was last used by the system.
        /// </summary>
        public DateTime LastUsed
        {
            get
            {
                return _info.LastUsed;
            }
        }
    }
}