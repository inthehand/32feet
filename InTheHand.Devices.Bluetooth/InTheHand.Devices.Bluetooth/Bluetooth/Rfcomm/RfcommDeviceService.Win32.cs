//-----------------------------------------------------------------------
// <copyright file="RfcommDeviceService.Win32.cs" company="In The Hand Ltd">
//   Copyright (c) 2017 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommDeviceService
    {
        private static async Task<RfcommDeviceService> FromIdAsyncImpl(string deviceId)
        {
            if(deviceId.StartsWith("BLUETOOTH#"))
            {
                var parts = deviceId.Split('#');
                var addr = parts[1];
                var uuid = parts[2];
                var device  = await BluetoothDevice.FromBluetoothAddressAsync(ulong.Parse(addr, NumberStyles.HexNumber));
                var service = RfcommServiceId.FromUuid(new Guid(uuid));

                return new RfcommDeviceService(device, service);
            }

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
                var socket = BluetoothSockets.CreateRfcommSocket();
                socket.Connect(new BluetoothEndPoint(this));
                return new NetworkStream(socket);
            });
        }
    }
}