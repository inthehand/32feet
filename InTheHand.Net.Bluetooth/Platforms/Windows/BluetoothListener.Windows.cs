// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener (WinRT)
// 
// Copyright (c) 2018-2023 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using System;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;

namespace InTheHand.Net.Sockets
{
    partial class BluetoothListener
    {
        private RfcommServiceProvider provider;
        private StreamSocketListener listener;
        private System.Threading.EventWaitHandle listenHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
        private BluetoothClient currentClient = null;

        void PlatformStart()
        {
            provider = RfcommServiceProvider.CreateAsync(RfcommServiceId.FromUuid(this.serviceUuid)).AsTask().GetAwaiter().GetResult();
            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;
            listener.BindServiceNameAsync(provider.ServiceId.AsString(), SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication).AsTask().GetAwaiter().GetResult();
            provider.StartAdvertising(listener);
            Active = true;
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            pending = true;
            currentClient = new BluetoothClient(args.Socket);
            listenHandle.Set();
        }

        void PlatformStop()
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
        bool PlatformPending()
        {
            return pending;
        }

        BluetoothClient PlatformAcceptBluetoothClient()
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
