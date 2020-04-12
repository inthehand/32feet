// 32feet.NET - Personal Area Networking for .NET
//
// InTheHand.Net.Sockets.BluetoothListener
// 
// Copyright (c) 2003-2020 In The Hand Ltd, All rights reserved.
// This source code is licensed under the MIT License

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Sdp;
using System;

namespace InTheHand.Net.Sockets
{
    public sealed partial class BluetoothListener : IDisposable
    {
        Guid serviceUuid;

        public BluetoothListener(Guid service)
        {
            serviceUuid = service;
        }

        public BluetoothListener(Guid service, ServiceRecord sdpRecord) : this(service)
        {
            ServiceRecord = sdpRecord;
        }

        public bool Active
        {
            get;
            private set;
        }

        public ServiceClass ServiceClass
        {
            get;
            set;
        }

        public string ServiceName
        {
            get;
            set;
        }

        public ServiceRecord ServiceRecord { get; private set; }

        public bool Pending()
        {
            return DoPending();
        }

        public void Start()
        {
            DoStart();
            Active = true;
        }

        public void Stop()
        {
            DoStop();
            Active = false;
        }

        public BluetoothClient AcceptBluetoothClient()
        {
            if (!Active)
                throw new InvalidOperationException("Not listening. You must call the Start() method before calling this method.");

            return DoAcceptBluetoothClient();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (Active)
                    Stop();
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BluetoothListener()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
