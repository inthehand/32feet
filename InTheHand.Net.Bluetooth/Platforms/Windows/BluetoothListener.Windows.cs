// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (WinRT)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using System;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    internal sealed class WindowsBluetoothListener : IBluetoothListener
    {
        private RfcommServiceProvider provider;
        private StreamSocketListener listener;
        private System.Threading.EventWaitHandle listenHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        private BluetoothClient currentClient = null;
        public bool Active { get; private set; }

        public ServiceClass ServiceClass { get; set; }
        public string ServiceName { get; set; }
        public ServiceRecord ServiceRecord { get; set; }
        public Guid ServiceUuid { get; set; }

        public void Start()
        {
            provider = RfcommServiceProvider.CreateAsync(RfcommServiceId.FromUuid(ServiceUuid)).AsTask().GetAwaiter().GetResult();
            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;
            listener.BindServiceNameAsync(provider.ServiceId.AsString(), SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication).AsTask().GetAwaiter().GetResult();
            provider.StartAdvertising(listener);
            Active = true;
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            pending = true;
            currentClient = new BluetoothClient(new WindowsBluetoothClient(args.Socket));
            listenHandle.Set();
        }

        public void Stop()
        {
            if(Active)
            {
                if(provider != null)
                {
                    provider.StopAdvertising();
                    provider = null;
                }

                if(listener != null)
                {
                    listener.ConnectionReceived-=Listener_ConnectionReceived;
                    listener.Dispose();
                    listener = null;
                }

                Active = false;
            }
        }

        private bool pending = false;
        public bool Pending()
        {
            return pending;
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            if(listener != null)
            {
                listenHandle.WaitOne();
                pending = false;
                return currentClient;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
