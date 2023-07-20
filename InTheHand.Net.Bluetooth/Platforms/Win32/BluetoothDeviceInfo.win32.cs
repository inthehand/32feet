// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothDeviceInfo (Win32)
// 
// Copyright (c) 2003-2022 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
using InTheHand.Net.Bluetooth.Sdp;
using InTheHand.Net.Bluetooth.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
            PlatformRefresh();
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

        async Task<IEnumerable<Guid>> PlatformGetRfcommServicesAsync(bool cached)
        {
            List<Guid> services = new List<Guid>();
            WSAQUERYSET qs = new WSAQUERYSET();
            IntPtr buffer = Marshal.AllocHGlobal(2048);

            try
            {
                qs.dwSize = Marshal.SizeOf(qs);
                qs.lpServiceClassId = Marshal.AllocHGlobal(16);
                // setting the service class scope to RFComm to exclude other L2CAP services
                Marshal.Copy(BluetoothProtocol.RFCommProtocol.ToByteArray(), 0, qs.lpServiceClassId, 16);
                qs.dwNameSpace = NativeMethods.NS_BTH;
                qs.dwNumberOfCsAddrs = 0;
                qs.lpszContext = $"({DeviceAddress:C})";

                IntPtr lookupHandle = IntPtr.Zero;
                int hresult = NativeMethods.WSALookupServiceBegin(ref qs, NativeMethods.LookupFlags.ReturnBlob | NativeMethods.LookupFlags.ReturnName | (cached ? 0 : NativeMethods.LookupFlags.FlushCache), out lookupHandle);
                int bufferLength = 2048;
                while (hresult == 0)
                {
                    bufferLength = 2048;
                    hresult = NativeMethods.WSALookupServiceNext(lookupHandle, NativeMethods.LookupFlags.ReturnBlob | NativeMethods.LookupFlags.ReturnName | (cached ? 0 : NativeMethods.LookupFlags.FlushCache), ref bufferLength, buffer);
                    if (hresult == 0)
                    {
                        var qsResult = Marshal.PtrToStructure<WSAQUERYSET>(buffer);
                        System.Diagnostics.Debug.WriteLine(qsResult.lpszServiceInstanceName);
                        var blobLength = Marshal.ReadInt32(qsResult.lpBlob);
                        byte[] blob = new byte[blobLength];
                        Marshal.Copy(Marshal.ReadIntPtr(qsResult.lpBlob, 4), blob, 0, blobLength);

                        // WSALookupServiceNext only returns empty service uuids even with the right flags set
                        // So we request the raw SDP record in the blob and parse it
                        ServiceRecordParser parser = new ServiceRecordParser();
                        var record = parser.Parse(blob);

                        // well known id which specifies the uuid for the service
                        var attribute = record.GetAttributeById(UniversalAttributeId.ServiceClassIdList);
                        if (attribute != null)
                        {
                            var vals = attribute.Value.GetValueAsElementList();
                            // values in a list from most to least specific so read the first entry
                            var mostSpecific = vals.FirstOrDefault();
                            // short ids are automatically converted to a long Guid
                            var guid = mostSpecific.GetValueAsUuid();
                            services.Add(guid);
                        }
                    }
                }

                NativeMethods.WSALookupServiceEnd(lookupHandle);

            }
            finally
            {
                Marshal.FreeHGlobal(qs.lpServiceClassId);
                Marshal.FreeHGlobal(buffer);
            }
            return services;
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

        void PlatformSetServiceState(Guid service, bool state)
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

        void PlatformRefresh()
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