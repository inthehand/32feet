using System;
using System.Collections.Generic;
using InTheHand.Networking.Sockets;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommServiceProvider
    {
        
        private static async Task<RfcommServiceProvider> DoCreateAsync(RfcommServiceId serviceId)
        {
            var serverSocket = BluetoothAdapter.Default.Adapter.ListenUsingRfcommWithServiceRecord(serviceId.ToString(), Java.Util.UUID.NameUUIDFromBytes(serviceId.Uuid.ToByteArray()));
            return new RfcommServiceProvider(serverSocket);
        }

        private BluetoothServerSocket _socket;

        private RfcommServiceProvider(BluetoothServerSocket socket)
        {
            _socket = socket;
        }

        private void DoStartAdvertising()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (_socket != null)
                    {
                        var socket = await _socket.AcceptAsync();
                        ConnectionReceived?.Invoke(this, new RfcommConnectionReceivedEventArgs(new NetworkStream(socket)));
                    }
                }
                catch
                {
                    //TODO: only trap thread cancellation here
                }
            });
        }

        private void DoStopAdvertising()
        {
            if(_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }
    }
}
