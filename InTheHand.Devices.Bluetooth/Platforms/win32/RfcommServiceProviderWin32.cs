using System;
using System.Collections.Generic;
using InTheHand.Networking.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommServiceProvider
    {
        private static async Task<RfcommServiceProvider> DoCreateAsync(RfcommServiceId serviceId)
        {
            return new RfcommServiceProvider(serviceId.Uuid);
        }

        private Socket _listenerSocket;
        private Guid _serviceUuid;

        private RfcommServiceProvider(Guid uuid)
        {
            _serviceUuid = uuid;        
        }
                
        private void DoStartAdvertising()
        {
            Task.Run(() =>
            {
                _listenerSocket = BluetoothSockets.CreateRfcommSocket();
                _listenerSocket.Bind(new BluetoothEndPoint(0, _serviceUuid));

                try
                {
                    _listenerSocket.Listen(1);

                    // TODO: Publish SDP record

                    while (_listenerSocket != null)
                    {                        
                        var socket = _listenerSocket.Accept();
                        ConnectionReceived?.Invoke(this, new RfcommConnectionReceivedEventArgs(new global::System.Net.Sockets.NetworkStream(socket)));
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
            if (_listenerSocket != null)
            {
                // TODO: remove SDP record

                _listenerSocket.Close();
                _listenerSocket = null;
            }
        }
    }
}
