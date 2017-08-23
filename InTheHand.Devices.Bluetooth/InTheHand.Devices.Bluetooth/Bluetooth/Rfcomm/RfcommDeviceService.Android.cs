//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.Android.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using InTheHand.Networking.Sockets;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommDeviceService
    {
        private static async Task<RfcommDeviceService> FromIdAsyncImpl(string deviceId)
        {
            return null;
        }
        
        private static string GetDeviceSelectorImpl(RfcommServiceId serviceId)
        {
            return "bluetoothService:" + serviceId.Uuid.ToString();
        }

        private BluetoothDevice _device;
        private RfcommServiceId _service;

        internal RfcommDeviceService(BluetoothDevice device, RfcommServiceId service)
        {
            _device = device;
            _service = service;
        }

        private BluetoothDevice GetDevice()
        {
            return _device;
        }

        private RfcommServiceId GetServiceId()
        {
            return _service;
        }
        
        private Task<Stream> OpenStreamAsyncImpl()
        {
            return Task.Run<Stream>(() =>
            {
                var socket = _device._device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString(_service.Uuid.ToString()));
                socket.Connect();
                return new NetworkStream(socket);
            });
        }
    }
}