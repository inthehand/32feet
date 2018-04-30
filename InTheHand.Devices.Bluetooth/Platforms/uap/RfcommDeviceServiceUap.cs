//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.uwp.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
#if WINDOWS_UWP
using Windows.UI.Core;
#endif
using InTheHand.Networking.Sockets;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommDeviceService
    {
        private static async Task<RfcommDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return await Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService.FromIdAsync(deviceId);
        }
        
        private static string GetDeviceSelectorImpl(RfcommServiceId serviceId)
        {
            return Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService.GetDeviceSelector(serviceId);
        }

        private Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService _service;

        private RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService(RfcommDeviceService service)
        {
            return service._service;
        }

        public static implicit operator RfcommDeviceService(Windows.Devices.Bluetooth.Rfcomm.RfcommDeviceService service)
        {
            return new RfcommDeviceService(service);
        }

#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_PHONE_81
        private BluetoothDevice GetDevice()
        {
            return _service.Device;
        }
#endif

        private RfcommServiceId GetServiceId()
        {
            return _service.ServiceId;
        }
        
        private Task<Stream> OpenStreamAsyncImpl()
        {
            return Task.Run<Stream>(async () =>
            {
#if WINDOWS_UWP
                await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await _service.RequestAccessAsync();
                });
#endif
                Windows.Networking.Sockets.StreamSocket socket = new Windows.Networking.Sockets.StreamSocket();
                await socket.ConnectAsync(_service.ConnectionHostName, _service.ConnectionServiceName);
                return new NetworkStream(socket);
            });
        }
    }
}