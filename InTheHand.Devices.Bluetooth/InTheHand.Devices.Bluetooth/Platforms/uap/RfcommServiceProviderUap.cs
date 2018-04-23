using System;
using System.Collections.Generic;
using InTheHand.Networking.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;

namespace InTheHand.Devices.Bluetooth.Rfcomm
{
    partial class RfcommServiceProvider
    {
        
        private static async Task<RfcommServiceProvider> DoCreateAsync(RfcommServiceId serviceId)
        {
            var provider = await Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider.CreateAsync(serviceId);
            return new RfcommServiceProvider(provider);
        }

        private Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider _provider;
        private Windows.Networking.Sockets.StreamSocketListener _listener;

        private RfcommServiceProvider(Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider provider)
        {
            _provider = provider;
        }

        public static implicit operator Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider(RfcommServiceProvider provider)
        {
            return provider._provider;
        }

        public static implicit operator RfcommServiceProvider(Windows.Devices.Bluetooth.Rfcomm.RfcommServiceProvider provider)
        {
            return new RfcommServiceProvider(provider);
        }
        
        private void DoStartAdvertising()
        {
            if (_listener == null)
            {
                _listener = new Windows.Networking.Sockets.StreamSocketListener();
                _listener.ConnectionReceived += _listener_ConnectionReceived;
                _provider.StartAdvertising(_listener);
            }
        }

        private void _listener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            ConnectionReceived?.Invoke(this, new RfcommConnectionReceivedEventArgs(new NetworkStream(args.Socket)));
        }

        private void DoStopAdvertising()
        {
            if (_listener != null)
            {
                _provider.StopAdvertising();
                _listener.ConnectionReceived -= _listener_ConnectionReceived;
                _listener = null;
            }
        }
    }
}
